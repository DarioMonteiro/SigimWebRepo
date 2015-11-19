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
using GIR.Sigim.Application.Filtros.Financeiro;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Application.Reports.Financeiro;
using CrystalDecisions.Shared;
using System.Data;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public class TipoCompromissoAppService : BaseAppService, ITipoCompromissoAppService
    {
        private ITipoCompromissoRepository tipoCompromissoRepository;
        private IParametrosFinanceiroRepository parametrosFinanceiroRepository;

        public TipoCompromissoAppService(ITipoCompromissoRepository tipoCompromissoRepository,
                                         IParametrosFinanceiroRepository parametrosFinanceiroRepository,
                                         MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.tipoCompromissoRepository = tipoCompromissoRepository;
            this.parametrosFinanceiroRepository = parametrosFinanceiroRepository;
        }

        #region ITipoCompromissoAppService Members

        public List<TipoCompromissoDTO> ListarTipoPagar()
        {
            return tipoCompromissoRepository.ListarPeloFiltro(l => l.TipoPagar.HasValue && l.TipoPagar.Value).To<List<TipoCompromissoDTO>>();
        }

        public List<TipoCompromissoDTO> ListarPeloFiltro(TipoCompromissoFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<TipoCompromisso>)new TrueSpecification<TipoCompromisso>();


            return tipoCompromissoRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<TipoCompromissoDTO>>();
        }

        public TipoCompromissoDTO ObterPeloId(int? id)
        {
            return tipoCompromissoRepository.ObterPeloId(id).To<TipoCompromissoDTO>();
        }

        public bool Salvar(TipoCompromissoDTO dto)
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
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Descrição"), TypeMessage.Error);
                return false;
            }

            bool novoItem = false;

            var tipoCompromisso = tipoCompromissoRepository.ObterPeloId(dto.Id);
            if (tipoCompromisso == null)
            {
                tipoCompromisso = new TipoCompromisso();
                novoItem = true;
            }

            tipoCompromisso.Descricao = dto.Descricao;
            tipoCompromisso.TipoPagar = dto.TipoPagar;
            tipoCompromisso.TipoReceber = dto.TipoReceber;
            
            if (Validator.IsValid(tipoCompromisso, out validationErrors))
            {
                if (novoItem)
                    tipoCompromissoRepository.Inserir(tipoCompromisso);
                else
                    tipoCompromissoRepository.Alterar(tipoCompromisso);

                tipoCompromissoRepository.UnitOfWork.Commit();
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

            var tipoCompromisso = tipoCompromissoRepository.ObterPeloId(id);

            try
            {
                tipoCompromissoRepository.Remover(tipoCompromisso);
                tipoCompromissoRepository.UnitOfWork.Commit();
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
            return UsuarioLogado.IsInRole(Funcionalidade.TipoCompromissoGravar);
        }

        public bool EhPermitidoDeletar()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.TipoCompromissoDeletar);
        }

        public bool EhPermitidoImprimir()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.TipoCompromissoImprimir);
        }

        public FileDownloadDTO ExportarRelTipoCompromisso(FormatoExportacaoArquivo formato)
        {
            if (!EhPermitidoImprimir())
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return null;
            }

            var specification = (Specification<TipoCompromisso>)new TrueSpecification<TipoCompromisso>();

            var listaTipoCompromisso = tipoCompromissoRepository.ListarPeloFiltro(specification).To<List<TipoCompromisso>>();

            relTipoCompromisso objRel = new relTipoCompromisso();

            objRel.SetDataSource(RelTipoCompromissoToDataTable(listaTipoCompromisso));

            var parametros = parametrosFinanceiroRepository.Obter();
            CentroCusto centroCusto = null;

            var caminhoImagem = PrepararIconeRelatorio(centroCusto, parametros);

            var nomeEmpresa = ObterNomeEmpresa(centroCusto, parametros);
            objRel.SetParameterValue("nomeEmpresa", nomeEmpresa);
            objRel.SetParameterValue("caminhoImagem", caminhoImagem);

            FileDownloadDTO arquivo = new FileDownloadDTO("Rel. Tipo compromisso",
                                                          objRel.ExportToStream((ExportFormatType)formato),
                                                          formato);
            if (System.IO.File.Exists(caminhoImagem))
                System.IO.File.Delete(caminhoImagem);
            return arquivo;
        }


        #endregion

        #region métodos privados de ITipoCompromissoAppService

        private DataTable RelTipoCompromissoToDataTable(List<TipoCompromisso> listaTipoCompromisso)
        {
            DataTable dta = new DataTable();
            DataColumn codigo = new DataColumn("codigo");
            DataColumn descricao = new DataColumn("descricao");
            DataColumn descricaoGeraTitulo = new DataColumn("descricaoGeraTitulo");
            DataColumn descricaoTipoPagar = new DataColumn("descricaoTipoPagar");
            DataColumn descricaoTipoReceber = new DataColumn("descricaoTipoReceber");
            DataColumn girErro = new DataColumn("girErro");

            dta.Columns.Add(codigo);
            dta.Columns.Add(descricao);
            dta.Columns.Add(descricaoGeraTitulo);
            dta.Columns.Add(descricaoTipoPagar);
            dta.Columns.Add(descricaoTipoReceber);
            dta.Columns.Add(girErro);

            foreach (var registro in listaTipoCompromisso)
            {
                TipoCompromissoDTO tipoCompromisso = registro.To<TipoCompromissoDTO>();
                DataRow row = dta.NewRow();

                row[codigo] = tipoCompromisso.Id;
                row[descricao] = tipoCompromisso.Descricao;
                row[descricaoGeraTitulo] = tipoCompromisso.GeraTituloDescricao;
                row[descricaoTipoPagar] = tipoCompromisso.TipoPagarDescricao;
                row[descricaoTipoReceber] = tipoCompromisso.TipoReceberDescricao;
                row[girErro] = "";
                dta.Rows.Add(row);
            }

            return dta;
        }

        #endregion

    }
}