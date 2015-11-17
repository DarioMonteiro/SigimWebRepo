using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Application.Reports.Sigim;
using CrystalDecisions.Shared;
using System.Data;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Application.Service.Sigim
{
    public class FormaRecebimentoAppService : BaseAppService, IFormaRecebimentoAppService
    {
        private IFormaRecebimentoRepository formaRecebimentoRepository;
        private IParametrosFinanceiroRepository parametrosFinanceiroRepository;

        public FormaRecebimentoAppService(IFormaRecebimentoRepository formaRecebimentoRepository, 
                                          IParametrosFinanceiroRepository parametrosFinanceiroRepository, 
                                          MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.formaRecebimentoRepository = formaRecebimentoRepository;
            this.parametrosFinanceiroRepository = parametrosFinanceiroRepository;
        }

        #region IFormaRecebimentoAppService Members

        public List<FormaRecebimentoDTO> ListarTodos()
        {
            return formaRecebimentoRepository.ListarTodos().To<List<FormaRecebimentoDTO>>();
        }

        public List<FormaRecebimentoDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<FormaRecebimento>)new TrueSpecification<FormaRecebimento>();

            return formaRecebimentoRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<FormaRecebimentoDTO>>();
        }

        public FormaRecebimentoDTO ObterPeloId(int? id)
        {
            return formaRecebimentoRepository.ObterPeloId(id).To<FormaRecebimentoDTO>();
        }

        public bool Salvar(FormaRecebimentoDTO dto)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.FormaRecebimentoGravar))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            if (dto == null)
            {
                throw new ArgumentNullException("dto");
            }

            if (dto.NumeroDias.HasValue)
            {
                if (dto.NumeroDias.Value < 0)
                {
                    messageQueue.Add("O número de dias não pode ser menor que zero !", TypeMessage.Error);
                    return false;
                }
            }

            bool novoItem = false;

            var formaRecebimento = formaRecebimentoRepository.ObterPeloId(dto.Id);
            if (formaRecebimento == null)
            {
                formaRecebimento = new FormaRecebimento();
                novoItem = true;
            }
            if (formaRecebimento.Automatico == true)
            {
                return false;
            }
                       
            formaRecebimento.Descricao = dto.Descricao;
            formaRecebimento.Automatico = dto.Automatico;
            formaRecebimento.TipoRecebimento = dto.TipoRecebimento;
            formaRecebimento.NumeroDias = dto.NumeroDias;
            if (!dto.NumeroDias.HasValue) formaRecebimento.NumeroDias = 0;

            if (Validator.IsValid(formaRecebimento, out validationErrors))
            {
                if (novoItem)
                    formaRecebimentoRepository.Inserir(formaRecebimento);
                else
                    formaRecebimentoRepository.Alterar(formaRecebimento);

                formaRecebimentoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                return true;
            }
            else
            {
                messageQueue.AddRange(validationErrors, TypeMessage.Error);
                return false;
            }
        }

        public bool Deletar(int? id)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.FormaRecebimentoDeletar))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            if (id == null)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
                return false;
            }

            var formaRecebimento = formaRecebimentoRepository.ObterPeloId(id);

            if (formaRecebimento.Automatico == true)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.RegistroProtegido, TypeMessage.Error);
                return false;
            }

            try
            {                
                formaRecebimentoRepository.Remover(formaRecebimento);
                formaRecebimentoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;                
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, formaRecebimento.Descricao), TypeMessage.Error);
                return false;
            }
        }

        public List<ItemListaDTO> ListarTipoRecebimento()
        { return typeof(TipoFormaRecebimento).ToItemListaDTO(); }

        public bool EhPermitidoSalvar()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.FormaRecebimentoGravar);
        }

        public bool EhPermitidoDeletar()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.FormaRecebimentoDeletar);
        }

        public bool EhPermitidoImprimir()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.FormaRecebimentoImprimir);
        }

        public FileDownloadDTO ExportarRelFormaRecebimento(FormatoExportacaoArquivo formato)
        {
            if (!EhPermitidoImprimir())
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return null;
            }

            var specification = (Specification<FormaRecebimento>)new TrueSpecification<FormaRecebimento>();

            var listaFormaRecebimento = formaRecebimentoRepository.ListarPeloFiltro(specification).To<List<FormaRecebimento>>();

            relFormaRecebimento objRel = new relFormaRecebimento();

            objRel.SetDataSource(RelFormaRecebimentoToDataTable(listaFormaRecebimento));

            var parametros = parametrosFinanceiroRepository.Obter();
            CentroCusto centroCusto = null;

            var caminhoImagem = PrepararIconeRelatorio(centroCusto, parametros);

            objRel.SetParameterValue("nomeSistema", "FINANCEIRO");
            objRel.SetParameterValue("caminhoImagem", caminhoImagem);

            FileDownloadDTO arquivo = new FileDownloadDTO("Rel. Forma Recebimento",
                                                          objRel.ExportToStream((ExportFormatType)formato),
                                                          formato);
            if (System.IO.File.Exists(caminhoImagem))
                System.IO.File.Delete(caminhoImagem);
            return arquivo;
        }

        #endregion

        #region métodos privados de IFormaRecebimento

        private DataTable RelFormaRecebimentoToDataTable(List<FormaRecebimento> listaFormaRecebimento)
        {
            DataTable dta = new DataTable();
            DataColumn codigo = new DataColumn("codigo");
            DataColumn descricao = new DataColumn("descricao");
            DataColumn tipoRecebimento = new DataColumn("tipoRecebimento");
            DataColumn numeroDias = new DataColumn("numeroDias");
            DataColumn girErro = new DataColumn("girErro");

            dta.Columns.Add(codigo);
            dta.Columns.Add(descricao);
            dta.Columns.Add(tipoRecebimento);
            dta.Columns.Add(numeroDias);
            dta.Columns.Add(girErro);

            foreach (var registro in listaFormaRecebimento)
            {
                FormaRecebimentoDTO formaRecebimento = registro.To<FormaRecebimentoDTO>();
                DataRow row = dta.NewRow();

                row[codigo] = formaRecebimento.Id;
                row[descricao] = formaRecebimento.Descricao;
                row[tipoRecebimento] = formaRecebimento.TipoRecebimentoDescricao;
                row[numeroDias] = DBNull.Value;
                if (formaRecebimento.NumeroDias.HasValue)
                {
                    row[numeroDias] = formaRecebimento.NumeroDias.Value;
                }
                row[girErro] = "";
                dta.Rows.Add(row);
            }

            return dta;
        }

        #endregion

    }
}