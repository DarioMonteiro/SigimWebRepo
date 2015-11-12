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
using GIR.Sigim.Domain.Specification.Financeiro;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Application.Reports.Financeiro;
using CrystalDecisions.Shared;
using System.Data;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public class TipoMovimentoAppService : BaseAppService, ITipoMovimentoAppService
    {
        private ITipoMovimentoRepository tipoMovimentoRepository;
        private IParametrosFinanceiroRepository parametrosFinanceiroRepository;

        public TipoMovimentoAppService(ITipoMovimentoRepository tipoMovimentoRepository, 
                                       IParametrosFinanceiroRepository parametrosFinanceiroRepository, 
                                       MessageQueue messageQueue) 
            : base (messageQueue)
        {
            this.tipoMovimentoRepository = tipoMovimentoRepository;
            this.parametrosFinanceiroRepository = parametrosFinanceiroRepository;
        }

        #region métodos de ITipoMovimentoAppService

        public List<TipoMovimentoDTO> ListarTodos()
        {
            return tipoMovimentoRepository.ListarTodos().OrderBy(l => l.Id).To<List<TipoMovimentoDTO>>(); 
        }

        public List<TipoMovimentoDTO> ListarNaoAutomatico(BaseFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<TipoMovimento>)new TrueSpecification<TipoMovimento>();

            specification &= TipoMovimentoSpecification.EhNaoAutomatico();

            return tipoMovimentoRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros,
                l => l.HistoricoContabil).To<List<TipoMovimentoDTO>>();
        }


        public TipoMovimentoDTO ObterPeloId(int? id)
        {
            return tipoMovimentoRepository.ObterPeloId(id,l => l.ListaMovimentoFinanceiro).To<TipoMovimentoDTO>();
        }

        public bool Salvar(TipoMovimentoDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var tipoMovimento = tipoMovimentoRepository.ObterPeloId(dto.Id);
            if (tipoMovimento == null)
            {
                tipoMovimento = new TipoMovimento();
                novoItem = true;
            }
            else
            {
                if (tipoMovimento.Automatico)
                {
                    messageQueue.Add(Resource.Sigim.ErrorMessages.RegistroProtegido, TypeMessage.Error);
                    return false;
                }
            }

            tipoMovimento.Descricao = dto.Descricao;
            tipoMovimento.HistoricoContabilId = dto.HistoricoContabilId;
            tipoMovimento.Tipo = dto.Tipo;
            tipoMovimento.Operacao = dto.Operacao;

            try
            {
                if (!EhValidoSalvarTipoMovimento(tipoMovimento)){
                    return false;
                }

                if (Validator.IsValid(tipoMovimento, out validationErrors))
                {
                    if (novoItem)
                        tipoMovimentoRepository.Inserir(tipoMovimento);
                    else
                        tipoMovimentoRepository.Alterar(tipoMovimento);

                    tipoMovimentoRepository.UnitOfWork.Commit();
                    messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                    return true;
                }
                else
                    messageQueue.AddRange(validationErrors, TypeMessage.Error);
            }
            catch (Exception exception)
            {
                QueueExeptionMessages(exception);
            }
            return false;
        }

        public bool Deletar(int? id)
        {
            if (id == null)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
                return false;
            }

            var tipoMovimento = tipoMovimentoRepository.ObterPeloId(id);

            if (tipoMovimento.Automatico)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.RegistroProtegido, TypeMessage.Error);
                return false;
            }

            try
            {
                tipoMovimentoRepository.Remover(tipoMovimento);
                tipoMovimentoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, tipoMovimento.Descricao), TypeMessage.Error);
                return false;
            }
        }

        public bool EhPermitidoSalvar()
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.TipoMovimentoGravar))
                return false;

            return true;
        }

        public bool EhPermitidoDeletar()
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.TipoMovimentoDeletar))
                return false;

            return true;
        }

        public bool EhPermitidoImprimir()
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.TipoMovimentoImprimir))
                return false;

            return true;
        }

        public FileDownloadDTO ExportarRelTipoMovimento(FormatoExportacaoArquivo formato)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.TipoMovimentoImprimir))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return null;
            }

            var specification = (Specification<TipoMovimento>)new TrueSpecification<TipoMovimento>();

            specification &= TipoMovimentoSpecification.EhNaoAutomatico();

            var listaTipoMovimento = tipoMovimentoRepository.ListarPeloFiltro(specification,
                                                                              l => l.HistoricoContabil).To<List<TipoMovimento>>();
            relTipoMovimento objRel = new relTipoMovimento();

            objRel.SetDataSource(RelTipoMovimentoToDataTable(listaTipoMovimento));

            var parametros = parametrosFinanceiroRepository.Obter();
            CentroCusto centroCusto = null;

            var caminhoImagem = PrepararIconeRelatorio(centroCusto, parametros);

            var nomeEmpresa = ObterNomeEmpresa(centroCusto, parametros);
            objRel.SetParameterValue("nomeEmpresa", nomeEmpresa);
            objRel.SetParameterValue("caminhoImagem", caminhoImagem);

            FileDownloadDTO arquivo = new FileDownloadDTO("Rel. Tipo Movimento",
                                                          objRel.ExportToStream((ExportFormatType)formato),
                                                          formato);
            if (System.IO.File.Exists(caminhoImagem))
                System.IO.File.Delete(caminhoImagem);
            return arquivo;
        }


        #endregion

        #region métodos privados de ITipoMovimentoAppService

        private DataTable RelTipoMovimentoToDataTable(List<TipoMovimento> listaTipoMovimento)
        {
            DataTable dta = new DataTable();
            DataColumn codigo = new DataColumn("codigo");
            DataColumn descricao = new DataColumn("descricao");
            DataColumn descricaoHistoricoContabil = new DataColumn("descricaoHistoricoContabil");
            DataColumn descricaoTipo = new DataColumn("descricaoTipo");
            DataColumn descricaoOperacao = new DataColumn("descricaoOperacao");
            DataColumn girErro = new DataColumn("girErro");

            dta.Columns.Add(codigo);
            dta.Columns.Add(descricao);
            dta.Columns.Add(descricaoHistoricoContabil);
            dta.Columns.Add(descricaoTipo);
            dta.Columns.Add(descricaoOperacao);
            dta.Columns.Add(girErro);

            foreach (var registro in listaTipoMovimento)
            {
                TipoMovimentoDTO tipoMovimento = registro.To<TipoMovimentoDTO>();
                DataRow row = dta.NewRow();

                row[codigo] = tipoMovimento.Id;
                row[descricao] = tipoMovimento.Descricao ;
                row[descricaoHistoricoContabil] = tipoMovimento.HistoricoContabilDescricao;
                row[descricaoTipo] = tipoMovimento.TipoDescricao;
                row[descricaoOperacao] = tipoMovimento.OperacaoDescricao;
                row[girErro] = "";
                dta.Rows.Add(row);
            }

            return dta;
        }

        private bool EhValidoSalvarTipoMovimento(TipoMovimento tipoMovimento)
        {
            if (!EhNaoAutomatico(tipoMovimento.Id)){
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.RegistroProtegido, TypeMessage.Error);
                return false;
            }

            return true;
        }

        private bool EhNaoAutomatico(int? tipoMovimentoId)
        {
            if (tipoMovimentoId.HasValue)
            {
                TipoMovimentoDTO tipoMovimento = new TipoMovimentoDTO();

                tipoMovimento = ObterPeloId(tipoMovimentoId);
                if (tipoMovimento.Automatico.HasValue)
                {
                    if (tipoMovimento.Automatico.Value)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        #endregion
    }
}
