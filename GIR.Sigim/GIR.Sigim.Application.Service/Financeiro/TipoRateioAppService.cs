using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Application.Filtros;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Application.Reports.Financeiro;
using CrystalDecisions.Shared;
using System.Data;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public class TipoRateioAppService : BaseAppService, ITipoRateioAppService
    {
        #region declaracao

        private ITipoRateioRepository tipoRateioRepository;
        private IParametrosFinanceiroRepository parametrosFinanceiroRepository;

        #endregion 

        #region construtor

        public TipoRateioAppService(ITipoRateioRepository tipoRateioRepository,
                                    IParametrosFinanceiroRepository parametrosFinanceiroRepository, 
                                    MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.tipoRateioRepository = tipoRateioRepository;
            this.parametrosFinanceiroRepository = parametrosFinanceiroRepository;
        }

        #endregion

        #region métodos de ITipoRateioAppService

        public List<TipoRateioDTO> ListarTodos()
        {
            return tipoRateioRepository.ListarTodos().OrderBy(l => l.Descricao).To<List<TipoRateioDTO>>();
        }

        public List<TipoRateioDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<TipoRateio>)new TrueSpecification<TipoRateio>();


            return tipoRateioRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<TipoRateioDTO>>();
        }

        public TipoRateioDTO ObterPeloId(int? id)
        {
            return tipoRateioRepository.ObterPeloId(id).To<TipoRateioDTO>();
        }

        public bool Salvar(TipoRateioDTO dto)
        {
            if (!EhPermitidoSalvar())
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            if (dto == null)
                throw new ArgumentNullException("dto");

            if (string.IsNullOrEmpty(dto.Descricao))
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.CampoObrigatorio, "Descrição"), TypeMessage.Error);
                return false;
            }

            bool novoItem = false;

            var tipoRateio = tipoRateioRepository.ObterPeloId(dto.Id);
            if (tipoRateio == null)
            {
                tipoRateio = new TipoRateio();
                novoItem = true;
            }

            tipoRateio.Descricao = dto.Descricao;

            if (Validator.IsValid(tipoRateio, out validationErrors))
            {
                if (novoItem)
                    tipoRateioRepository.Inserir(tipoRateio);
                else
                    tipoRateioRepository.Alterar(tipoRateio);

                tipoRateioRepository.UnitOfWork.Commit();
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

            var tipoRateio = tipoRateioRepository.ObterPeloId(id);

            try
            {
                tipoRateioRepository.Remover(tipoRateio);
                tipoRateioRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, tipoRateio.Descricao), TypeMessage.Error);
                return false;
            }
        }

        public bool EhPermitidoSalvar()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.TipoRateioGravar);
        }

        public bool EhPermitidoDeletar()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.TipoRateioDeletar);
        }

        public bool EhPermitidoImprimir()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.TipoRateioImprimir);
        }

        public FileDownloadDTO ExportarRelTipoRateio(FormatoExportacaoArquivo formato)
        {
            if (!EhPermitidoImprimir())
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return null;
            }

            var specification = (Specification<TipoRateio>)new TrueSpecification<TipoRateio>();

            var listaTipoRateio = tipoRateioRepository.ListarPeloFiltro(specification).OrderBy(l => l.Descricao).To<List<TipoRateio>>();
            relTipoRateio objRel = new relTipoRateio();

            objRel.SetDataSource(RelTipoRateioToDataTable(listaTipoRateio));

            var parametros = parametrosFinanceiroRepository.Obter();
            CentroCusto centroCusto = null;

            var caminhoImagem = PrepararIconeRelatorio(centroCusto, parametros);

            var nomeEmpresa = ObterNomeEmpresa(centroCusto, parametros);
            objRel.SetParameterValue("nomeEmpresa", nomeEmpresa);
            objRel.SetParameterValue("caminhoImagem", caminhoImagem);

            FileDownloadDTO arquivo = new FileDownloadDTO("Rel. Tipo rateio",
                                                          objRel.ExportToStream((ExportFormatType)formato),
                                                          formato);
            if (System.IO.File.Exists(caminhoImagem))
                System.IO.File.Delete(caminhoImagem);
            return arquivo;
        }

        #endregion

        #region métodos privados de ICaixaAppService

        private DataTable RelTipoRateioToDataTable(List<TipoRateio> listaTipoRateio)
        {
            DataTable dta = new DataTable();
            DataColumn codigo = new DataColumn("codigo");
            DataColumn descricao = new DataColumn("descricao");
            DataColumn girErro = new DataColumn("girErro");

            dta.Columns.Add(codigo);
            dta.Columns.Add(descricao);
            dta.Columns.Add(girErro);

            foreach (var tipoRateio in listaTipoRateio)
            {
                DataRow row = dta.NewRow();

                row[codigo] = tipoRateio.Id;
                row[descricao] = tipoRateio.Descricao;
                row[girErro] = "";
                dta.Rows.Add(row);
            }

            return dta;
        }

        #endregion


    }
}
