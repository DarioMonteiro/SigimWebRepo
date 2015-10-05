using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.OrdemCompra;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Application.Helper
{
    public class OrdemCompraMapperHelper
    {
        public static void Initialise()
        {
            Mapper.CreateMap<Cotacao, CotacaoDTO>();
            Mapper.CreateMap<CotacaoDTO, Cotacao>();

            Mapper.CreateMap<CotacaoItem, CotacaoItemDTO>();
            Mapper.CreateMap<CotacaoItemDTO, CotacaoItem>();

            Mapper.CreateMap<EntradaMaterial, EntradaMaterialDTO>()
                .ForMember(d => d.PossuiAvaliacaoFornecedor, m => m.MapFrom(s => s.ListaAvaliacaoFornecedor.Any()))
                .ForMember(d => d.EhDescontoPercentual, m => m.MapFrom(s => s.PercentualDesconto.HasValue));
            Mapper.CreateMap<EntradaMaterialDTO, EntradaMaterial>();

            Mapper.CreateMap<EntradaMaterialFormaPagamento, EntradaMaterialFormaPagamentoDTO>();
            Mapper.CreateMap<EntradaMaterialFormaPagamentoDTO, EntradaMaterialFormaPagamento>();

            Mapper.CreateMap<EntradaMaterialImposto, EntradaMaterialImpostoDTO>()
                //BACALHAU para contornar gambiarra de data.
                .ForMember(d => d.DataVencimento, m => m.MapFrom(s => s.DataVencimento.HasValue && s.DataVencimento != new DateTime(9999, 1, 1) ? s.DataVencimento : null));
            Mapper.CreateMap<EntradaMaterialImpostoDTO, EntradaMaterialImposto>();

            Mapper.CreateMap<EntradaMaterialItem, EntradaMaterialItemDTO>()
                .ForMember(d => d.CodigoComplementoCST, m => m.MapFrom(s => string.IsNullOrEmpty(s.CodigoComplementoCST) ? string.Empty : s.CodigoComplementoCST))
                .ForMember(d => d.CodigoComplementoNaturezaOperacao, m => m.MapFrom(s => string.IsNullOrEmpty(s.CodigoComplementoNaturezaOperacao) ? string.Empty : s.CodigoComplementoNaturezaOperacao))
                .ForMember(d => d.CodigoNaturezaReceita, m => m.MapFrom(s => string.IsNullOrEmpty(s.CodigoNaturezaReceita) ? string.Empty : s.CodigoNaturezaReceita))
                .ForMember(d => d.QuantidadeInicial, m => m.MapFrom(s => s.Quantidade));
            Mapper.CreateMap<EntradaMaterialItemDTO, EntradaMaterialItem>();

            Mapper.CreateMap<OrdemCompra, FreteDTO>()
                .ForMember(d => d.Data, m => m.MapFrom(s => s.DataFrete))
                .ForMember(d => d.Valor, m => m.MapFrom(s => s.ValorFrete))
                .ForMember(d => d.TituloPagarId, m => m.MapFrom(s => s.TituloFreteId));

            Mapper.CreateMap<InterfaceCotacao, InterfaceCotacaoDTO>();
            Mapper.CreateMap<InterfaceCotacaoDTO, InterfaceCotacao>();

            Mapper.CreateMap<OrdemCompra, OrdemCompraDTO>();
            Mapper.CreateMap<OrdemCompraDTO, OrdemCompra>();

            Mapper.CreateMap<OrdemCompraItem, OrdemCompraItemDTO>()
                .ForMember(d => d.DataOrdemCompra, m => m.MapFrom(s => s.OrdemCompra.Data))
                .ForMember(d => d.PercentualDescontoOrdemCompra, m => m.MapFrom(s => s.OrdemCompra.PercentualDesconto));
            Mapper.CreateMap<OrdemCompraItemDTO, OrdemCompraItem>();

            Mapper.CreateMap<ParametrosOrdemCompra, ParametrosOrdemCompraDTO>();
            Mapper.CreateMap<ParametrosOrdemCompraDTO, ParametrosOrdemCompra>()
                .ForMember(d => d.IconeRelatorio, m => m.ResolveUsing(s => s.IconeRelatorio == null ? null : s.IconeRelatorio));

            Mapper.CreateMap<ParametrosUsuario, ParametrosUsuarioDTO>()
                .ForMember(d => d.CentroCusto, m => m.MapFrom(s => s.CentroCusto));
            Mapper.CreateMap<ParametrosUsuarioDTO, ParametrosUsuario>();

            Mapper.CreateMap<PreRequisicaoMaterial, PreRequisicaoMaterialDTO>()
                .ForMember(d => d.SituacaoDescricao, m => m.MapFrom(s => s.Situacao.ObterDescricao()))
                .ForMember(d => d.RMGeradas, m => m.MapFrom(s => string.Join(", ", s.ListaItens.SelectMany(l => l.ListaRequisicaoMaterialItem.Where(r => r.RequisicaoMaterial.Situacao != SituacaoRequisicaoMaterial.Cancelada).Select(c => c.RequisicaoMaterialId.ToString())).Distinct().OrderBy(o => o))));
            Mapper.CreateMap<PreRequisicaoMaterialDTO, PreRequisicaoMaterial>();

            Mapper.CreateMap<PreRequisicaoMaterialItem, PreRequisicaoMaterialItemDTO>()
                .ForMember(d => d.ListaRequisicaoMaterialItem, m => m.MapFrom(s => s.ListaRequisicaoMaterialItem.Where(l => l.RequisicaoMaterial.Situacao != SituacaoRequisicaoMaterial.Cancelada)));
            Mapper.CreateMap<PreRequisicaoMaterialItemDTO, PreRequisicaoMaterialItem>()
                .ForMember(d => d.CentroCusto, m => m.UseValue(null))
                .ForMember(d => d.CodigoCentroCusto, m => m.MapFrom(s => s.CentroCusto.Codigo))
                .ForMember(d => d.Classe, m => m.UseValue(null))
                .ForMember(d => d.CodigoClasse, m => m.MapFrom(s => s.Classe.Codigo))
                .ForMember(d => d.UnidadeMedida, m => m.MapFrom(s => s.Material.SiglaUnidadeMedida))
                .ForMember(d => d.Material, m => m.UseValue(null));

            Mapper.CreateMap<RequisicaoMaterial, RequisicaoMaterialDTO>()
                .ForMember(d => d.SituacaoDescricao, m => m.MapFrom(s => s.Situacao.ObterDescricao()));
            Mapper.CreateMap<RequisicaoMaterialDTO, RequisicaoMaterial>();

            Mapper.CreateMap<RequisicaoMaterialItem, RequisicaoMaterialItemDTO>()
                .ForMember(d => d.UltimaCotacao, m => m.MapFrom(s => s.ListaCotacaoItem.Where(l => l.Cotacao.Situacao != SituacaoCotacao.Cancelada).Max(c => c.CotacaoId)))
                .ForMember(d => d.UltimaOrdemCompra, m => m.MapFrom(s => s.ListaOrdemCompraItem.Where(l => l.OrdemCompra.Situacao != SituacaoOrdemCompra.Cancelada).Max(c => c.OrdemCompraId)))
                .ForMember(d => d.OrcamentoInsumoRequisitado, m => m.MapFrom(s => s.ListaOrcamentoInsumoRequisitado.FirstOrDefault()));
            Mapper.CreateMap<RequisicaoMaterialItemDTO, RequisicaoMaterialItem>()
                .ForMember(d => d.Classe, m => m.UseValue(null))
                .ForMember(d => d.CodigoClasse, m => m.MapFrom(s => s.Classe.Codigo))
                .ForMember(d => d.UnidadeMedida, m => m.MapFrom(s => s.Material.SiglaUnidadeMedida))
                .ForMember(d => d.Material, m => m.UseValue(null))
                .ForMember(d => d.MaterialId, m => m.MapFrom(s => s.Material.Id));
            Mapper.CreateMap<RequisicaoMaterialItem, RequisicaoMaterialItemDTO>();
            Mapper.CreateMap<RequisicaoMaterialItemDTO, RequisicaoMaterialItem>();
        }
    }
}