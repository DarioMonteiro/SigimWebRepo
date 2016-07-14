using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Orcamento;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Enums;
using GIR.Sigim.Application.Filtros.Sigim;
using GIR.Sigim.Domain.Entity.Orcamento;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Domain.Repository.Orcamento;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Domain.Specification.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;

namespace GIR.Sigim.Application.Service.Sigim
{
    public class MaterialAppService : BaseAppService, IMaterialAppService
    {
        private IMaterialRepository materialRepository;
        private IOrcamentoRepository orcamentoRepository;
        private ICentroCustoRepository centroCustoRepository;

        public MaterialAppService(IMaterialRepository MaterialRepository,
            IOrcamentoRepository orcamentoRepository,
            ICentroCustoRepository centroCustoRepository,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.materialRepository = MaterialRepository;
            this.orcamentoRepository = orcamentoRepository;
            this.centroCustoRepository = centroCustoRepository;
        }

        #region IMaterialAppService Members

        public List<MaterialDTO> ListarPeloFiltro(MaterialFiltro filtro, out int totalRegistros)
        {
            return materialRepository.ListarPeloFiltroComPaginacao(
                l => l.Descricao.Contains(filtro.Descricao),
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<MaterialDTO>>();
        }

        public List<MaterialDTO> ListarAtivosPeloCentroCustoEDescricao(string codigoCentroCusto, string descricao)
        {
            var specification = (Specification<Material>)new TrueSpecification<Material>();
            specification &= MaterialSpecification.DescricaoContem(descricao);
            specification &= MaterialSpecification.EhAtivo();

            specification &= ObterTipoTabelaSpecification(specification, codigoCentroCusto);

            return materialRepository.ListarPeloFiltro(specification).To<List<MaterialDTO>>();
        }

        public List<MaterialDTO> PesquisarAtivosPeloFiltro(MaterialPesquisaFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<Material>)new TrueSpecification<Material>();
            specification &= MaterialSpecification.EhAtivo();
            specification &= ObterTipoTabelaSpecification(specification, filtro.CodigoCentroCusto);

            bool EhTipoSelecaoContem = filtro.TipoSelecao == TipoPesquisa.Contem;
            switch (filtro.Campo)
            {
                case "unidadeMedida":
                    specification &= EhTipoSelecaoContem ? MaterialSpecification.UnidadeMedidaContem(filtro.TextoInicio)
                        : MaterialSpecification.UnidadeMedidaNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                    break;
                case "id":
                    int? inicio = !string.IsNullOrEmpty(filtro.TextoInicio) ? Convert.ToInt32(filtro.TextoInicio) : (int?)null;
                    int? fim = !string.IsNullOrEmpty(filtro.TextoFim) ? Convert.ToInt32(filtro.TextoFim) : (int?)null;
                    specification &= EhTipoSelecaoContem ? MaterialSpecification.MatchingId(inicio)
                        : MaterialSpecification.IdNoIntervalo(inicio, fim);
                    break;
                case "classeInsumo":
                    specification &= EhTipoSelecaoContem ? MaterialSpecification.ClasseInsumoContem(filtro.TextoInicio)
                        : MaterialSpecification.ClasseInsumoNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                    break;
                case "codigoExterno":
                    specification &= EhTipoSelecaoContem ? MaterialSpecification.CodigoExternoContem(filtro.TextoInicio)
                        : MaterialSpecification.CodigoExternoNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                    break;
                case "descricao":
                default:
                    specification &= EhTipoSelecaoContem ? MaterialSpecification.DescricaoContem(filtro.TextoInicio)
                        : MaterialSpecification.DescricaoNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                    break;
            }

            return materialRepository.Pesquisar(
                specification,
                filtro.PageIndex,
                filtro.PageSize,
                filtro.OrderBy,
                filtro.Ascending,
                out totalRegistros).To<List<MaterialDTO>>();
        }

        public List<OrcamentoComposicaoItemDTO> ListarOrcamentoComposicaoItem(int? materialId, string codigoCentroCusto, string codigoClasse, out bool possuiInterfaceOrcamento)
        {
            possuiInterfaceOrcamento = false;
            List<OrcamentoComposicaoItemDTO> listaDTO = new List<OrcamentoComposicaoItemDTO>();
            var orcamento = orcamentoRepository.ObterUltimoOrcamentoPeloCentroCusto(codigoCentroCusto,
                l => l.ListaOrcamentoComposicao.Select(o => o.ListaOrcamentoComposicaoItem.Select(s => s.Material.ListaOrcamentoInsumoRequisitado)),
                l => l.ListaOrcamentoComposicao.Select(o => o.ListaOrcamentoComposicaoItem.Select(s => s.OrcamentoComposicao.Classe)),
                l => l.ListaOrcamentoComposicao.Select(o => o.ListaOrcamentoComposicaoItem.Select(s => s.OrcamentoComposicao.Composicao)),
                l => l.ListaOrcamentoComposicao.Select(o => o.ListaOrcamentoComposicaoItem.Select(s => s.OrcamentoComposicao.Orcamento.Obra)));

            if (orcamento != null)
            {
                if (MaterialSelecionadoEncontradoNoOrcamentoEmElaboracao(orcamento, materialId))
                    messageQueue.Add("O insumo está em um orçamento que encontra-se em manutenção.", TypeMessage.Error);
                else
                {
                    var listaOrcamentoComposicao = orcamento.ListaOrcamentoComposicao.SelectMany(l =>
                        l.ListaOrcamentoComposicaoItem.Where(o =>
                            o.MaterialId == materialId
                            && o.EhControlado == true));
                    //&& (!string.IsNullOrEmpty(codigoClasse) ? o.OrcamentoComposicao.codigoClasse == codigoClasse : true))).To<List<OrcamentoComposicaoItemDTO>>();

                    if (!listaOrcamentoComposicao.Any())
                    {
                        var material = materialRepository.ObterPeloId(materialId);
                        listaOrcamentoComposicao = orcamento.ListaOrcamentoComposicao.SelectMany(l =>
                            l.ListaOrcamentoComposicaoItem.Where(o =>
                                o.MaterialId.HasValue
                                && o.EhControlado == true
                                && o.Material.CodigoMaterialClasseInsumo == material.CodigoMaterialClasseInsumo));
                        //&& (!string.IsNullOrEmpty(codigoClasse) ? o.OrcamentoComposicao.codigoClasse == codigoClasse : true))).To<List<OrcamentoComposicaoItemDTO>>();
                    }

                    if (listaOrcamentoComposicao.Any())
                        possuiInterfaceOrcamento = true;

                    listaDTO = listaOrcamentoComposicao.Where(l => !string.IsNullOrEmpty(codigoClasse) ? l.OrcamentoComposicao.CodigoClasse == codigoClasse : true).To<List<OrcamentoComposicaoItemDTO>>();
                }
            }
            return listaDTO;
        }

        private bool MaterialSelecionadoEncontradoNoOrcamentoEmElaboracao(Domain.Entity.Orcamento.Orcamento orcamento, int? materialId)
        {
            return orcamento.Situacao == "E"
                && orcamento.ListaOrcamentoComposicao.Any(l =>
                    l.ListaOrcamentoComposicaoItem.Any(o => o.MaterialId == materialId && o.EhControlado == true));
        }

        private Specification<Material> ObterTipoTabelaSpecification(Specification<Material> specification, string codigoCentroCusto)
        {
            var centroCusto = centroCustoRepository.ObterPeloCodigo(codigoCentroCusto);
            if (centroCusto != null)
            {
                var tipoTabela = centroCusto.TipoTabela.HasValue ? (TipoTabela)centroCusto.TipoTabela.Value : TipoTabela.Propria;
                int? anoMes = centroCusto.AnoMes;

                if (tipoTabela != TipoTabela.Propria)
                    specification &= (MaterialSpecification.EhTipoTabela(TipoTabela.Propria)
                        || (MaterialSpecification.EhTipoTabela(tipoTabela)
                            && MaterialSpecification.MatchingAnoMes(anoMes)));
                else
                    specification &= MaterialSpecification.EhTipoTabela(tipoTabela);
            }
            else
                specification &= MaterialSpecification.EhTipoTabela(TipoTabela.Propria);

            return specification;
        }

        #endregion
    }
}