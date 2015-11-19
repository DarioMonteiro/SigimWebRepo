using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Resource;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Repository.Admin;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Application.Filtros;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Application.Reports.Financeiro;
using CrystalDecisions.Shared;
using System.Data;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public class TipoDocumentoAppService : BaseAppService, ITipoDocumentoAppService
    {
        private ITipoDocumentoRepository tipoDocumentoRepository;
        private IParametrosFinanceiroRepository parametrosFinanceiroRepository;

        public TipoDocumentoAppService(ITipoDocumentoRepository tipoDocumentoRepository,
                                       IParametrosFinanceiroRepository parametrosFinanceiroRepository,
                                       MessageQueue messageQueue) 
            : base (messageQueue)
        {
            this.tipoDocumentoRepository = tipoDocumentoRepository;
            this.parametrosFinanceiroRepository = parametrosFinanceiroRepository;
        }

        #region métodos de ITipoDocumentoAppService

        public List<TipoDocumentoDTO> ListarTodos()
        {
            return tipoDocumentoRepository.ListarTodos().OrderBy(l => l.Sigla).To<List<TipoDocumentoDTO>>(); 
        }

        public List<TipoDocumentoDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<TipoDocumento>)new TrueSpecification<TipoDocumento>();


            return tipoDocumentoRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<TipoDocumentoDTO>>();
        }

        public TipoDocumentoDTO ObterPeloId(int? id)
        {
            return tipoDocumentoRepository.ObterPeloId(id).To<TipoDocumentoDTO>();
        }

        public bool Salvar(TipoDocumentoDTO dto)
        {
            if (!EhPermitidoSalvar())
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var tipoDocumento = tipoDocumentoRepository.ObterPeloId(dto.Id);
            if (tipoDocumento == null)
            {
                tipoDocumento = new TipoDocumento();
                novoItem = true;
            }

            tipoDocumento.Sigla = dto.Sigla;
            tipoDocumento.Descricao = dto.Descricao;

            if (Validator.IsValid(tipoDocumento, out validationErrors))
            {
                if (novoItem)
                    tipoDocumentoRepository.Inserir(tipoDocumento);
                else
                    tipoDocumentoRepository.Alterar(tipoDocumento);

                tipoDocumentoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                return true;
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

            var tipoCompromisso = tipoDocumentoRepository.ObterPeloId(id);

            try
            {
                tipoDocumentoRepository.Remover(tipoCompromisso);
                tipoDocumentoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, tipoCompromisso.Descricao), TypeMessage.Error);
                return false;
            }
        }

        public bool EhPermitidoSalvar()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.TipoDocumentoGravar);
        }

        public bool EhPermitidoDeletar()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.TipoDocumentoDeletar);
        }

        public bool EhPermitidoImprimir()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.TipoDocumentoImprimir);
        }

        public FileDownloadDTO ExportarRelTipoDocumento(FormatoExportacaoArquivo formato)
        {
            if (!EhPermitidoImprimir())
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return null;
            }

            var specification = (Specification<TipoDocumento>)new TrueSpecification<TipoDocumento>();

            var listaTipoDocumento = tipoDocumentoRepository.ListarPeloFiltro(specification).To<List<TipoDocumento>>();
            listaTipoDocumento = listaTipoDocumento.OrderBy(l => l.Sigla).ToList();
            relTipoDocumento objRel = new relTipoDocumento();

            objRel.SetDataSource(RelTipoDocumentoToDataTable(listaTipoDocumento));

            var parametros = parametrosFinanceiroRepository.Obter();
            CentroCusto centroCusto = null;

            var caminhoImagem = PrepararIconeRelatorio(centroCusto, parametros);

            var nomeEmpresa = ObterNomeEmpresa(centroCusto, parametros);
            objRel.SetParameterValue("nomeEmpresa", nomeEmpresa);
            objRel.SetParameterValue("caminhoImagem", caminhoImagem);

            FileDownloadDTO arquivo = new FileDownloadDTO("Rel. Tipo documento",
                                                          objRel.ExportToStream((ExportFormatType)formato),
                                                          formato);
            if (System.IO.File.Exists(caminhoImagem))
                System.IO.File.Delete(caminhoImagem);
            return arquivo;
        }

        #endregion
        #region métodos privados de ICaixaAppService

        private DataTable RelTipoDocumentoToDataTable(List<TipoDocumento> listaTipoDocumento)
        {
            DataTable dta = new DataTable();
            DataColumn codigo = new DataColumn("codigo");
            DataColumn descricao = new DataColumn("descricao");
            DataColumn sigla = new DataColumn("sigla");
            DataColumn girErro = new DataColumn("girErro");

            dta.Columns.Add(codigo);
            dta.Columns.Add(descricao);
            dta.Columns.Add(sigla);
            dta.Columns.Add(girErro);

            foreach (var registro in listaTipoDocumento)
            {
                TipoDocumentoDTO tipoDocumento = registro.To<TipoDocumentoDTO>();
                DataRow row = dta.NewRow();

                row[codigo] = tipoDocumento.Id;
                row[descricao] = tipoDocumento.Descricao;
                row[sigla] = tipoDocumento.Sigla;
                row[girErro] = "";
                dta.Rows.Add(row);
            }

            return dta;
        }

        #endregion

    }
}
