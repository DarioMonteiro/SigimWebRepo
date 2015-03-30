using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Admin;
using GIR.Sigim.Application.DTO.Contrato;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Orcamento;
using GIR.Sigim.Application.DTO.OrdemCompra;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Domain.Entity.Admin;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Orcamento;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Adapter;

namespace GIR.Sigim.Application.Helper
{
    public class MapperHelper
    {
        public static void Initialise()
        {
            #region Admin
            Mapper.CreateMap<Usuario, UsuarioDTO>();
            Mapper.CreateMap<UsuarioDTO, Usuario>();
            #endregion

            #region Contrato
            Mapper.CreateMap<ParametrosContrato, ParametrosContratoDTO>();
            Mapper.CreateMap<ParametrosContratoDTO, ParametrosContrato>()
                .ForMember(d => d.IconeRelatorio, m => m.ResolveUsing(s => s.IconeRelatorio == null ? null : s.IconeRelatorio));
            #endregion
            
            #region Financeiro
            Mapper.CreateMap<CentroCusto, CentroCustoDTO>();
            Mapper.CreateMap<CentroCustoDTO, CentroCusto>();

            Mapper.CreateMap<Classe, ClasseDTO>();
            Mapper.CreateMap<ClasseDTO, Classe>();

            Mapper.CreateMap<TipoCompromisso, TipoCompromissoDTO>();
            Mapper.CreateMap<TipoCompromissoDTO, TipoCompromisso>();

            Mapper.CreateMap<ParametrosUsuarioFinanceiro, ParametrosUsuarioFinanceiroDTO>();
            Mapper.CreateMap<ParametrosUsuarioFinanceiroDTO, ParametrosUsuarioFinanceiro>();

            #endregion

            #region Orçamento
            Mapper.CreateMap<Obra, ObraDTO>();
            Mapper.CreateMap<ObraDTO, Obra>();

            Mapper.CreateMap<Orcamento, OrcamentoDTO>();
            Mapper.CreateMap<OrcamentoDTO, Orcamento>();

            Mapper.CreateMap<ParametrosOrcamento, ParametrosOrcamentoDTO>();
            Mapper.CreateMap<ParametrosOrcamentoDTO, ParametrosOrcamento>()
                .ForMember(d => d.IconeRelatorio, m => m.ResolveUsing(s => s.IconeRelatorio == null ? null : s.IconeRelatorio));
            #endregion

            #region Ordem de Compra
            Mapper.CreateMap<InterfaceCotacao, InterfaceCotacaoDTO>();
            Mapper.CreateMap<InterfaceCotacaoDTO, InterfaceCotacao>();

            Mapper.CreateMap<ParametrosOrdemCompra, ParametrosOrdemCompraDTO>();
            Mapper.CreateMap<ParametrosOrdemCompraDTO, ParametrosOrdemCompra>()
                .ForMember(d => d.IconeRelatorio, m => m.ResolveUsing(s => s.IconeRelatorio == null ? null : s.IconeRelatorio));

            Mapper.CreateMap<ParametrosUsuario, ParametrosUsuarioDTO>()
                .ForMember(d => d.CentroCusto, m => m.MapFrom(s => s.CentroCusto));
            Mapper.CreateMap<ParametrosUsuarioDTO, ParametrosUsuario>();

            Mapper.CreateMap<PreRequisicaoMaterial, PreRequisicaoMaterialDTO>()
                .ForMember(d => d.SituacaoDescricao, m => m.MapFrom(s => s.Situacao.ObterDescricao()))
                .ForMember(d => d.RMGeradas,m => m.MapFrom(s => string.Join(", ", s.ListaItens.SelectMany(l => l.ListaRequisicaoMaterialItem.Select(c => c.RequisicaoMaterialId.ToString())).Distinct().OrderBy(o => o))));
            Mapper.CreateMap<PreRequisicaoMaterialDTO, PreRequisicaoMaterial>();

            Mapper.CreateMap<PreRequisicaoMaterialItem, PreRequisicaoMaterialItemDTO>();
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

            Mapper.CreateMap<RequisicaoMaterialItem, RequisicaoMaterialItemDTO>();
            Mapper.CreateMap<RequisicaoMaterialItemDTO, RequisicaoMaterialItem>();
            #endregion

            #region Sigim
            Mapper.CreateMap<AssuntoContato, AssuntoContatoDTO>();
            Mapper.CreateMap<AssuntoContatoDTO, AssuntoContato>();

            Mapper.CreateMap<BancoLayout, BancoLayoutDTO>();
            Mapper.CreateMap<BancoLayoutDTO, BancoLayout>();

            Mapper.CreateMap<CentroCusto, TreeNodeDTO>()
                .ForMember(d => d.ListaFilhos, m => m.MapFrom(s => s.ListaFilhos));

            Mapper.CreateMap<Classe, TreeNodeDTO>()
                .ForMember(d => d.ListaFilhos, m => m.MapFrom(s => s.ListaFilhos))
                .ForMember(d => d.Ativo, m => m.MapFrom(s => true));

            Mapper.CreateMap<ClienteFornecedor, ClienteFornecedorDTO>();
            Mapper.CreateMap<ClienteFornecedorDTO, ClienteFornecedor>();

            Mapper.CreateMap<Material, MaterialDTO>();
            Mapper.CreateMap<MaterialDTO, Material>();

            Mapper.CreateMap<UnidadeMedida, UnidadeMedidaDTO>();
            Mapper.CreateMap<UnidadeMedidaDTO, UnidadeMedida>();
            #endregion
        }
    }
}