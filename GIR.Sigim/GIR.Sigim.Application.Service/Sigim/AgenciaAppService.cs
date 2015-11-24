using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros.Sigim;
using GIR.Sigim.Application.Reports.Sigim;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Domain.Specification.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Application.Reports.Financeiro;
using CrystalDecisions.Shared;
using System.Data;


namespace GIR.Sigim.Application.Service.Sigim
{
    public class AgenciaAppService : BaseAppService, IAgenciaAppService
    {
        #region Declaração

        private IAgenciaRepository agenciaRepository;
        private IParametrosFinanceiroRepository parametrosFinanceiroRepository;

        #endregion
        
        #region Construtor

        public AgenciaAppService(IAgenciaRepository agenciaRepository, 
                                 IParametrosFinanceiroRepository parametrosFinanceiroRepository, 
                                 MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.agenciaRepository = agenciaRepository;
            this.parametrosFinanceiroRepository = parametrosFinanceiroRepository;
        }
              
        #endregion

        #region IAgenciaAppService Members

       
        public List<AgenciaDTO> ListarPeloFiltro(AgenciaFiltro filtro, int? idUsuario, out int totalRegistros)
        {
            var specification = (Specification<Agencia>)new TrueSpecification<Agencia>();

                specification = AgenciaSpecification.PertenceAoBanco(filtro.BancoId);
         
            return agenciaRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros,
                l => l.Banco).To<List<AgenciaDTO>>();
           
        }

           
        public AgenciaDTO ObterPeloId(int? id)
        {
            
            return agenciaRepository.ObterPeloId(id).To<AgenciaDTO>();
        }

        public List<AgenciaDTO> ListarPeloBanco(int? bancoId)
        {
            return agenciaRepository.ListarTodos().Where(l => l.BancoId == bancoId).To<List<AgenciaDTO>>();
        }

        public bool Salvar(AgenciaDTO dto)
        {
            if (!EhPermitidoSalvar())
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var agencia = agenciaRepository.ObterPeloId(dto.Id, l => l.Banco);
            if (agencia == null)
            {
                agencia = new Agencia();                
                novoItem = true;
            }

            if (!agencia.Id.HasValue)
            {
                agencia.BancoId = dto.BancoId;
            }
            agencia.AgenciaCodigo = dto.AgenciaCodigo;
            agencia.DVAgencia = dto.DVAgencia;
            agencia.Nome = dto.Nome;
            agencia.NomeContato = dto.NomeContato;
            agencia.TelefoneContato = dto.TelefoneContato;
            agencia.TipoLogradouro = dto.TipoLogradouro;
            agencia.Logradouro = dto.Logradouro;
            agencia.Complemento = dto.Complemento;
            agencia.Numero = dto.Numero;
            agencia.Cidade = dto.Cidade;  
            
            if (Validator.IsValid(agencia, out validationErrors))
            {
                try
                {
                    if (novoItem)
                        agenciaRepository.Inserir(agencia);
                    else
                        agenciaRepository.Alterar(agencia);

                    agenciaRepository.UnitOfWork.Commit();
                   
                    dto.Id = agencia.Id;
                    messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                    return true;
                }
                catch (Exception exception)
                {
                    QueueExeptionMessages(exception);
                }
            }
            else
                messageQueue.AddRange(validationErrors, TypeMessage.Error);

            return false;
        }

        public bool Deletar(int? id)
        {
            if (!EhPermitidoDeletar())
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            if (id == null)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
                return false;
            }

            var agencia = agenciaRepository.ObterPeloId(id);

            try
            {
                agenciaRepository.Remover(agencia);
                agenciaRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, agencia.Nome), TypeMessage.Error);
                return false;
            }
        }

        public bool EhPermitidoSalvar()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.AgenciaGravar);
        }

        public bool EhPermitidoDeletar()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.AgenciaDeletar);
        }

        public bool EhPermitidoImprimir()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.AgenciaImprimir);
        }

        public bool EhPermitidoAcessarContaCorrente()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.ContaCorrenteAcessar);
        }

        public FileDownloadDTO ExportarRelAgencia(int? bancoId, FormatoExportacaoArquivo formato)
        {
            if (!EhPermitidoImprimir())
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return null;
            }

            var specification = (Specification<Agencia>)new TrueSpecification<Agencia>();

            specification = AgenciaSpecification.PertenceAoBanco(bancoId);

            var listaAgencia = agenciaRepository.ListarPeloFiltro(specification,l => l.Banco).OrderBy(l => l.BancoId).To<List<Agencia>>();
            relAgencia objRel = new relAgencia();

            objRel.SetDataSource(RelAgenciaToDataTable(listaAgencia));

            var parametros = parametrosFinanceiroRepository.Obter();
            CentroCusto centroCusto = null;

            var caminhoImagem = PrepararIconeRelatorio(centroCusto, parametros);

            //var nomeEmpresa = ObterNomeEmpresa(centroCusto, parametros);
            objRel.SetParameterValue("nomeSistema", "FINANCEIRO");
            objRel.SetParameterValue("caminhoImagem", caminhoImagem);

            FileDownloadDTO arquivo = new FileDownloadDTO("Rel. Agência",
                                                          objRel.ExportToStream((ExportFormatType)formato),
                                                          formato);
            if (System.IO.File.Exists(caminhoImagem))
                System.IO.File.Delete(caminhoImagem);
            return arquivo;
        }

       #endregion

        #region métodos privados de IAgenciaAppService

        private DataTable RelAgenciaToDataTable(List<Agencia> listaAgencia)
        {
            DataTable dta = new DataTable();
            DataColumn codigo = new DataColumn("codigo", System.Type.GetType("System.Int32"));
            DataColumn codigoBanco = new DataColumn("codigoBanco", System.Type.GetType("System.Int32"));
            DataColumn nomeBanco = new DataColumn("nomeBanco");
            DataColumn numeroAgencia = new DataColumn("agencia");
            DataColumn DVAgencia = new DataColumn("DVAgencia");
            DataColumn nome = new DataColumn("nome");
            DataColumn nomeContato = new DataColumn("nomeContato");
            DataColumn telefoneContato = new DataColumn("telefoneContato");
            DataColumn girErro = new DataColumn("girErro");

            dta.Columns.Add(codigo);
            dta.Columns.Add(codigoBanco);
            dta.Columns.Add(nomeBanco);
            dta.Columns.Add(numeroAgencia);
            dta.Columns.Add(DVAgencia);
            dta.Columns.Add(nome);
            dta.Columns.Add(nomeContato);
            dta.Columns.Add(telefoneContato);
            dta.Columns.Add(girErro);

            foreach (var registro in listaAgencia)
            {
                AgenciaDTO agencia = registro.To<AgenciaDTO>();
                DataRow row = dta.NewRow();

                row[codigo] = agencia.Id;
                row[codigoBanco] = agencia.Banco.Id;
                row[nomeBanco] = agencia.Banco.Nome;
                row[numeroAgencia] = agencia.AgenciaCodigo;
                row[DVAgencia] = agencia.DVAgencia;
                row[nome] = agencia.Nome;
                row[nomeContato] = agencia.NomeContato;
                row[telefoneContato] = agencia.TelefoneContato;
                row[girErro] = "";
                dta.Rows.Add(row);
            }

            return dta;
        }

        #endregion

    }
}