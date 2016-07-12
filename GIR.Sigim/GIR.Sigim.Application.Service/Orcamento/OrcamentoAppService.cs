using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Orcamento;
using GIR.Sigim.Domain.Entity.Orcamento;
using GIR.Sigim.Domain.Repository.Orcamento;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Application.Filtros.Financeiro;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Application.Filtros.Orcamento;
using GIR.Sigim.Application.Enums;
using GIR.Sigim.Domain.Specification.Orcamento;

namespace GIR.Sigim.Application.Service.Orcamento
{
    public class OrcamentoAppService : BaseAppService, IOrcamentoAppService
    {
        #region Declaração

        private IOrcamentoRepository orcamentoRepository;

        #endregion

        #region Construtor

        public OrcamentoAppService(IOrcamentoRepository orcamentoRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.orcamentoRepository = orcamentoRepository;
        }

        #endregion

        #region Métodos IOrcamentoAppService

        public OrcamentoDTO ObterUltimoOrcamentoPeloCentroCustoClasseOrcamento(string codigoCentroCusto)
        {
            return orcamentoRepository.ObterUltimoOrcamentoPeloCentroCustoClasseOrcamento(codigoCentroCusto).To<OrcamentoDTO>();
        }

        public OrcamentoDTO ObterUltimoOrcamentoPeloCentroCusto(string codigoCentroCusto)
        {
            return orcamentoRepository.ObterUltimoOrcamentoPeloCentroCusto(codigoCentroCusto).To<OrcamentoDTO>();
        }

        public bool EhPermitidoImprimirRelOrcamento()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.RelatorioOrcamentoImprimir);
        }

        public List<OrcamentoDTO> PesquisarOrcamentosPeloFiltro(OrcamentoPesquisaFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<Domain.Entity.Orcamento.Orcamento>)new TrueSpecification<Domain.Entity.Orcamento.Orcamento>();
            int? inicio;
            int? fim;

            specification &= OrcamentoSpecification.MatchingEmpresa(filtro.EmpresaId);
            specification &= OrcamentoSpecification.MatchingObra(filtro.ObraId);

            bool EhTipoSelecaoContem = filtro.TipoSelecao == TipoPesquisa.Contem;
            bool EhTipoSelecaoIntervalo = filtro.TipoSelecao == TipoPesquisa.Intervalo;
            switch (filtro.Campo)
            {
                case "sequencial":
                    if (EhTipoSelecaoIntervalo)
                    {
                        inicio = !string.IsNullOrEmpty(filtro.TextoInicio) ? Convert.ToInt32(filtro.TextoInicio) : (int?)null;
                        fim = !string.IsNullOrEmpty(filtro.TextoFim) ? Convert.ToInt32(filtro.TextoFim) : (int?)null;
                        specification &= OrcamentoSpecification.SequencialNoIntervalo(inicio, fim);
                    }
                    break;
                case "descricao":
                    specification &= EhTipoSelecaoContem ? OrcamentoSpecification.DescricaoContem(filtro.TextoInicio)
                        : OrcamentoSpecification.DescricaoNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                    break;
                case "obra":
                    specification &= EhTipoSelecaoContem ? OrcamentoSpecification.ObraNumeroContem(filtro.TextoInicio)
                        : OrcamentoSpecification.ObraNumeroNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                    break;
                case "empresa":
                    specification &= EhTipoSelecaoContem ? OrcamentoSpecification.EmpresaNumeroContem(filtro.TextoInicio)
                        : OrcamentoSpecification.EmpresaNumeroNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                    break;
                case "centroCusto":
                    specification &= EhTipoSelecaoContem ? OrcamentoSpecification.CentroCustoContem(filtro.TextoInicio)
                        : OrcamentoSpecification.CentroCustoNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                    break;
                case "situacao":
                    specification &= EhTipoSelecaoContem ? OrcamentoSpecification.SituacaoContem(filtro.TextoInicio)
                        : OrcamentoSpecification.SituacaoNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                    break;
                case "data":
                    Nullable<DateTime> datInicio = !string.IsNullOrEmpty(filtro.TextoInicio) ? Convert.ToDateTime(filtro.TextoInicio) : (Nullable<DateTime>)null;
                    Nullable<DateTime> datFim = !string.IsNullOrEmpty(filtro.TextoFim) ? Convert.ToDateTime(filtro.TextoFim) : (Nullable<DateTime>)null;
                    specification &= EhTipoSelecaoContem ? OrcamentoSpecification.DataContem(datInicio)
                        : OrcamentoSpecification.DataNoIntervalo(datInicio, datFim);
                    break;
                //case "simplificado":
                //    specification &= EhTipoSelecaoContem ? OrcamentoSpecification.SimplificadoContem(filtro.TextoInicio)
                //        : OrcamentoSpecification.SimplificadoNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                //    break;

                default:
                    break;
            }

            return orcamentoRepository.Pesquisar(specification,
                                                filtro.PageIndex,
                                                filtro.PageSize,
                                                filtro.OrderBy,
                                                filtro.Ascending,
                                                out totalRegistros,
                                                l => l.Obra.CentroCusto,
                                                l => l.Empresa.ClienteFornecedor).To<List<OrcamentoDTO>>();
        }

        #endregion
    }
}