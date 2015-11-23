using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Resource;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Repository.Admin;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Application.Filtros.Sigim;
using System.Linq.Expressions;
using GIR.Sigim.Domain.Specification.Sigim;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Application.Reports.Sigim;
using CrystalDecisions.Shared;
using System.Data;


namespace GIR.Sigim.Application.Service.Sigim
{
    public class BancoAppService : BaseAppService, IBancoAppService
    {
        private IBancoRepository bancoRepository;
        private IParametrosFinanceiroRepository parametrosFinanceiroRepository;

        public BancoAppService(IBancoRepository bancoRepository, 
                               IParametrosFinanceiroRepository parametrosFinanceiroRepository,
                               MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.bancoRepository = bancoRepository;
            this.parametrosFinanceiroRepository = parametrosFinanceiroRepository;
        }

        #region IBancoAppService Members

        public List<BancoDTO> ListarPeloFiltro(BancoFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<Banco>)new TrueSpecification<Banco>();
           
            specification = BancoSpecification.EhBanco();

            return bancoRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros,
                l => l.ListaAgencia).To<List<BancoDTO>>();
        }

        public BancoDTO ObterPeloId(int? id)
        {
            return bancoRepository.ObterPeloId(id).To<BancoDTO>();
        }

        public List<BancoDTO> ListarTodos()
        {
            return bancoRepository.ListarTodos().To<List<BancoDTO>>();
        }

        public List<BancoDTO> ListarTodosBancoComExcecaoCarteira()
        {
            return bancoRepository.ListarTodos().Where(l => l.Id != CarteiraBanco.CodigoCarteiraBanco).To<List<BancoDTO>>();
        }

        public bool Salvar(BancoDTO dto)
        {
            if (!EhPermitidoSalvar())
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            if (dto == null)
                throw new ArgumentNullException("dto");

            if (!dto.Id.HasValue)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.CampoObrigatorio, "Código do banco"), TypeMessage.Error);
                return false;
            }

            if (dto.Id.Value == 0)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.CampoInvalido, "Código do banco"), TypeMessage.Error);
                return false;
            }

            if (string.IsNullOrEmpty(dto.Nome))
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.CampoObrigatorio, "Descrição"), TypeMessage.Error);
                return false;
            }

            bool novoItem = false;

            var banco = bancoRepository.ObterPeloId(dto.Id);
            if (banco == null)
            {
                banco = new Banco();
                novoItem = true;
            }
            banco.Id = dto.Id;
            banco.Nome = dto.Nome;
            banco.Ativo = true;
            banco.NumeroRemessa = dto.NumeroRemessa;
            banco.NumeroRemessaPagamento = dto.NumeroRemessaPagamento;
            banco.InterfaceEletronica = dto.InterfaceEletronica;

            if (Validator.IsValid(banco, out validationErrors))
            {
                if (novoItem)                    
                    bancoRepository.Inserir(banco);
                else
                    bancoRepository.Alterar(banco);

                bancoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                return true;
            }
            else
                messageQueue.AddRange(validationErrors, TypeMessage.Error);

            return false;
        }

        public bool Deletar(int? Id)
        {
            if (!EhPermitidoDeletar())
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }
           
            if (Id == null)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
                return false;
            }

            var banco = bancoRepository.ObterPeloId(Id);

            try
            {
                bancoRepository.Remover(banco);
                bancoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception e)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, banco.Nome), TypeMessage.Error);
                QueueExeptionMessages(e);
                return false;
            }
        }

        public bool EhPermitidoSalvar()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.BancoGravar);
        }

        public bool EhPermitidoDeletar()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.BancoDeletar);
        }

        public bool EhPermitidoImprimir()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.BancoImprimir);
        }

        public bool EhPermitidoAcessarAgencia()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.AgenciaAcessar);
        }


        public FileDownloadDTO ExportarRelBanco(FormatoExportacaoArquivo formato)
        {
            if (!EhPermitidoImprimir())
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return null;
            }

            var specification = (Specification<Banco>)new TrueSpecification<Banco>();

            var listaBanco = bancoRepository.ListarPeloFiltro(specification,l => l.ListaAgencia).OrderBy(l => l.Nome).To<List<Banco>>();
            relBanco objRel = new relBanco();

            objRel.SetDataSource(RelBancoToDataTable(listaBanco));

            var parametros = parametrosFinanceiroRepository.Obter();
            CentroCusto centroCusto = null;

            var caminhoImagem = PrepararIconeRelatorio(centroCusto, parametros);

            var nomeEmpresa = ObterNomeEmpresa(centroCusto, parametros);
            objRel.SetParameterValue("nomeSistema", "FINANCEIRO");
            objRel.SetParameterValue("caminhoImagem", caminhoImagem);

            FileDownloadDTO arquivo = new FileDownloadDTO("Rel. Banco",
                                                          objRel.ExportToStream((ExportFormatType)formato),
                                                          formato);
            if (System.IO.File.Exists(caminhoImagem))
                System.IO.File.Delete(caminhoImagem);
            return arquivo;
        }

        #endregion

        #region métodos privados de IBancoAppService

        private DataTable RelBancoToDataTable(List<Banco> listaBanco)
        {
            DataTable dta = new DataTable();
            DataColumn codigoBC = new DataColumn("codigoBC", System.Type.GetType("System.Int32"));
            DataColumn nome = new DataColumn("nome");
            DataColumn descricaoInterfaceEletronica = new DataColumn("descricaoInterfaceEletronica");
            DataColumn numeroRemessa = new DataColumn("numeroRemessa", System.Type.GetType("System.Int32"));
            DataColumn numeroRemessaPagamento = new DataColumn("numeroRemessaPagamento", System.Type.GetType("System.Int32"));
            DataColumn girErro = new DataColumn("girErro");

            dta.Columns.Add(codigoBC);
            dta.Columns.Add(nome);
            dta.Columns.Add(descricaoInterfaceEletronica);
            dta.Columns.Add(numeroRemessa);
            dta.Columns.Add(numeroRemessaPagamento);
            dta.Columns.Add(girErro);

            foreach (var registro in listaBanco)
            {
                BancoDTO banco = registro.To<BancoDTO>();
                DataRow row = dta.NewRow();

                row[codigoBC] = banco.Id;
                row[nome] = banco.Nome;
                row[descricaoInterfaceEletronica] = banco.InterfaceEletronicaDescricao;
                int numeroRem = 0;
                if (banco.NumeroRemessa.HasValue){
                    numeroRem = banco.NumeroRemessa.Value;
                }
                row[numeroRemessa] = numeroRem;
                int numeroRemPagto = 0;
                if (banco.NumeroRemessaPagamento.HasValue)
                {
                    numeroRemPagto = banco.NumeroRemessaPagamento.Value;
                }
                row[numeroRemessaPagamento] = numeroRemPagto;
                row[girErro] = "";
                dta.Rows.Add(row);
            }

            return dta;
        }

        #endregion

    }
}