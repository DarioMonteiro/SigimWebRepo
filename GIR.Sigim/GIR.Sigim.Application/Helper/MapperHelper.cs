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
using GIR.Sigim.Application.DTO.Sac;
using GIR.Sigim.Domain.Entity.Admin;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Orcamento;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Entity.Sac;
using GIR.Sigim.Infrastructure.Crosscutting.Adapter;
using GIR.Sigim.Application.Enums;

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

            Mapper.CreateMap<Contrato, ContratoDTO>()
                .ForMember(d => d.SituacaoDescricao, m => m.MapFrom(s => s.Situacao.ObterDescricao()));
            Mapper.CreateMap<ContratoDTO, Contrato>()
                .ForMember(d => d.CentroCusto, m => m.UseValue(null))
                .ForMember(d => d.CodigoCentroCusto, m => m.MapFrom(s => s.CentroCusto.Codigo));

            Mapper.CreateMap<ContratoRetificacao, ContratoRetificacaoDTO>();
            Mapper.CreateMap<ContratoRetificacaoDTO, ContratoRetificacao>();

            Mapper.CreateMap<ContratoRetificacaoItemCronograma, ContratoRetificacaoItemCronogramaDTO>();
            Mapper.CreateMap<ContratoRetificacaoItemCronogramaDTO, ContratoRetificacaoItemCronograma>();

            Mapper.CreateMap<ContratoRetificacaoItem, ContratoRetificacaoItemDTO>();
            Mapper.CreateMap<ContratoRetificacaoItemDTO, ContratoRetificacaoItem>();

            Mapper.CreateMap<ContratoRetificacaoItemMedicao, ContratoRetificacaoItemMedicaoDTO>();
            Mapper.CreateMap<ContratoRetificacaoItemMedicaoDTO, ContratoRetificacaoItemMedicao>();

            Mapper.CreateMap<ContratoRetificacaoProvisao, ContratoRetificacaoProvisaoDTO>();
            Mapper.CreateMap<ContratoRetificacaoProvisaoDTO, ContratoRetificacaoProvisao>();

            Mapper.CreateMap<LicitacaoDescricao, LicitacaoDescricaoDTO>();
            Mapper.CreateMap<LicitacaoDescricaoDTO, LicitacaoDescricao>();

            Mapper.CreateMap<ParametrosContrato, ParametrosContratoDTO>();
            Mapper.CreateMap<ParametrosContratoDTO, ParametrosContrato>()
                .ForMember(d => d.IconeRelatorio, m => m.ResolveUsing(s => s.IconeRelatorio == null ? null : s.IconeRelatorio));

            #endregion
            
            #region Financeiro

            Mapper.CreateMap<CentroCusto, CentroCustoDTO>();
            Mapper.CreateMap<CentroCustoDTO, CentroCusto>();

            Mapper.CreateMap<Classe, ClasseDTO>();
            Mapper.CreateMap<ClasseDTO, Classe>();

            Mapper.CreateMap<Caixa, CaixaDTO>();
            Mapper.CreateMap<CaixaDTO, Caixa>();

            Mapper.CreateMap<ParametrosUsuarioFinanceiro, ParametrosUsuarioFinanceiroDTO>();
            Mapper.CreateMap<ParametrosUsuarioFinanceiroDTO, ParametrosUsuarioFinanceiro>();

            Mapper.CreateMap<TipoCompromisso, TipoCompromissoDTO>();
            Mapper.CreateMap<TipoCompromissoDTO, TipoCompromisso>();

            Mapper.CreateMap<TipoDocumento, TipoDocumentoDTO>();
            Mapper.CreateMap<TipoDocumentoDTO, TipoDocumento>();

            Mapper.CreateMap<TipoRateio, TipoRateioDTO>();
            Mapper.CreateMap<TipoRateioDTO, TipoRateio>();

            Mapper.CreateMap<TituloPagar, TituloPagarDTO>();
            Mapper.CreateMap<TituloPagarDTO, TituloPagar>();

            Mapper.CreateMap<TituloReceber, TituloReceberDTO>();
            Mapper.CreateMap<TituloReceberDTO, TituloReceber>();
            Mapper.CreateMap<MotivoCancelamento, MotivoCancelamentoDTO>();
            Mapper.CreateMap<MotivoCancelamentoDTO, MotivoCancelamento>();

            Mapper.CreateMap<ParametrosUsuarioFinanceiro, ParametrosUsuarioFinanceiroDTO>();
            Mapper.CreateMap<ParametrosUsuarioFinanceiroDTO, ParametrosUsuarioFinanceiro>();

            Mapper.CreateMap<ParametrosFinanceiro, ParametrosFinanceiroDTO>();
            Mapper.CreateMap<ParametrosFinanceiroDTO, ParametrosFinanceiro>();

            Mapper.CreateMap<ImpostoFinanceiro, ImpostoFinanceiroDTO>();
            Mapper.CreateMap<ImpostoFinanceiroDTO, ImpostoFinanceiro>();

            Mapper.CreateMap<AssuntoContatoDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.AssuntoContato));
            Mapper.CreateMap<TabelaBasicaDTO, AssuntoContatoDTO>();

            Mapper.CreateMap<InteresseBairroDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.BairroInteresse));
            Mapper.CreateMap<TabelaBasicaDTO, InteresseBairroDTO>();

            Mapper.CreateMap<EstadoCivilDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.EstadoCivil));
            Mapper.CreateMap<TabelaBasicaDTO, EstadoCivilDTO>();

            Mapper.CreateMap<FonteNegocioDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.FonteNegocio));
            Mapper.CreateMap<TabelaBasicaDTO, FonteNegocioDTO>();

            Mapper.CreateMap<GrupoDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.Grupo));
            Mapper.CreateMap<TabelaBasicaDTO, GrupoDTO>();

            Mapper.CreateMap<NacionalidadeDTO , TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.Nacionalidade));
            Mapper.CreateMap<TabelaBasicaDTO, NacionalidadeDTO>();

            Mapper.CreateMap<ParentescoDTO , TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.Parentesco));
            Mapper.CreateMap<TabelaBasicaDTO, ParentescoDTO>();

            Mapper.CreateMap<ProfissaoDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.Profissao));
            Mapper.CreateMap<TabelaBasicaDTO, ProfissaoDTO>();

            Mapper.CreateMap<RamoAtividadeDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.RamoAtividade ));
            Mapper.CreateMap<TabelaBasicaDTO, RamoAtividadeDTO>();

            Mapper.CreateMap<RelacionamentoDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.Relacionamento));
            Mapper.CreateMap<TabelaBasicaDTO, RelacionamentoDTO>();

            Mapper.CreateMap<TipologiaDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.Tipologia));
            Mapper.CreateMap<TabelaBasicaDTO, TipologiaDTO>();
            
            Mapper.CreateMap<TratamentoDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.Tratamento));
            Mapper.CreateMap<TabelaBasicaDTO, TratamentoDTO>();

            Mapper.CreateMap<TipoAreaDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.TipoArea));
            Mapper.CreateMap<TabelaBasicaDTO, TipoAreaDTO>();

            Mapper.CreateMap<TipoCaracteristicaDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.TipoCaracteristica));
            Mapper.CreateMap<TabelaBasicaDTO, TipoCaracteristicaDTO>();

            Mapper.CreateMap<TipoEspecificacaoDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.TipoEspecificacao));
            Mapper.CreateMap<TabelaBasicaDTO, TipoEspecificacaoDTO>();

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
            Mapper.CreateMap<Cotacao, CotacaoDTO>();
            Mapper.CreateMap<CotacaoDTO, Cotacao>();

            Mapper.CreateMap<CotacaoItem, CotacaoItemDTO>();
            Mapper.CreateMap<CotacaoItemDTO, CotacaoItem>();

            Mapper.CreateMap<InterfaceCotacao, InterfaceCotacaoDTO>();
            Mapper.CreateMap<InterfaceCotacaoDTO, InterfaceCotacao>();

            Mapper.CreateMap<OrdemCompra, OrdemCompraDTO>();
            Mapper.CreateMap<OrdemCompraDTO, OrdemCompra>();

            Mapper.CreateMap<OrdemCompraItem, OrdemCompraItemDTO>();
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
                .ForMember(d => d.TemInterfaceOrcamento, m => m.MapFrom(s => s.ListaOrcamentoInsumoRequisitado.Any()));
            Mapper.CreateMap<RequisicaoMaterialItemDTO, RequisicaoMaterialItem>()
                .ForMember(d => d.Classe, m => m.UseValue(null))
                .ForMember(d => d.CodigoClasse, m => m.MapFrom(s => s.Classe.Codigo))
                .ForMember(d => d.UnidadeMedida, m => m.MapFrom(s => s.Material.SiglaUnidadeMedida))
                .ForMember(d => d.Material, m => m.UseValue(null))
                .ForMember(d => d.MaterialId, m => m.MapFrom(s => s.Material.Id));
            Mapper.CreateMap<RequisicaoMaterialItem, RequisicaoMaterialItemDTO>();
            Mapper.CreateMap<RequisicaoMaterialItemDTO, RequisicaoMaterialItem>();

            #endregion

            #region Sigim

            Mapper.CreateMap<Agencia, AgenciaDTO>();
            Mapper.CreateMap<AgenciaDTO, Agencia>();

            Mapper.CreateMap<AssuntoContato, AssuntoContatoDTO>();
            Mapper.CreateMap<AssuntoContatoDTO, AssuntoContato>();

            Mapper.CreateMap<Banco, BancoDTO>();
            Mapper.CreateMap<BancoDTO, Banco>();

            Mapper.CreateMap<BancoLayout, BancoLayoutDTO>();
            Mapper.CreateMap<BancoLayoutDTO, BancoLayout>();

            Mapper.CreateMap<BloqueioContabil, BloqueioContabilDTO>();
            Mapper.CreateMap<BloqueioContabilDTO, BloqueioContabil>();

            Mapper.CreateMap<CifFob, CifFobDTO>();
            Mapper.CreateMap<CifFobDTO, CifFob>();

            Mapper.CreateMap<ClienteFornecedor, ClienteFornecedorDTO>();
            Mapper.CreateMap<ClienteFornecedorDTO, ClienteFornecedor>();

            Mapper.CreateMap<CodigoContribuicao, CodigoContribuicaoDTO>();
            Mapper.CreateMap<CodigoContribuicaoDTO, CodigoContribuicao>();

            Mapper.CreateMap<ContaCorrente, ContaCorrenteDTO>();
            Mapper.CreateMap<ContaCorrenteDTO, ContaCorrente>();

            Mapper.CreateMap<CST, CSTDTO>();
            Mapper.CreateMap<CSTDTO, CST>();

            Mapper.CreateMap<InteresseBairro, InteresseBairroDTO>();
            Mapper.CreateMap<InteresseBairroDTO, InteresseBairro>();

            Mapper.CreateMap<Material, MaterialDTO>();
            Mapper.CreateMap<MaterialDTO, Material>();

            Mapper.CreateMap<NaturezaOperacao, NaturezaOperacaoDTO>();
            Mapper.CreateMap<NaturezaOperacaoDTO, NaturezaOperacao>();

            Mapper.CreateMap<PessoaFisica, PessoaFisicaDTO>();
            Mapper.CreateMap<PessoaFisicaDTO, PessoaFisica>();

            Mapper.CreateMap<PessoaJuridica, PessoaJuridicaDTO>();
            Mapper.CreateMap<PessoaJuridicaDTO, PessoaJuridica>();

            Mapper.CreateMap<SerieNF, SerieNFDTO>();
            Mapper.CreateMap<SerieNFDTO, SerieNF>();

            Mapper.CreateMap<Servico, ServicoDTO>();
            Mapper.CreateMap<ServicoDTO, Servico>();

            Mapper.CreateMap<TipoCompra, TipoCompraDTO>();
            Mapper.CreateMap<TipoCompraDTO, TipoCompra>();

            Mapper.CreateMap<CentroCusto, TreeNodeDTO>()
                .ForMember(d => d.ListaFilhos, m => m.MapFrom(s => s.ListaFilhos));

            Mapper.CreateMap<Classe, TreeNodeDTO>()
                .ForMember(d => d.ListaFilhos, m => m.MapFrom(s => s.ListaFilhos))
                .ForMember(d => d.Ativo, m => m.MapFrom(s => true));
            
            Mapper.CreateMap<UnidadeMedida, UnidadeMedidaDTO>();
            Mapper.CreateMap<UnidadeMedidaDTO, UnidadeMedida>();

            Mapper.CreateMap<EstadoCivil, EstadoCivilDTO>();
            Mapper.CreateMap<EstadoCivilDTO, EstadoCivil>();

            Mapper.CreateMap<FonteNegocio, FonteNegocioDTO>();
            Mapper.CreateMap<FonteNegocioDTO, FonteNegocio>();

            Mapper.CreateMap<FormaRecebimento, FormaRecebimentoDTO>();
            Mapper.CreateMap<FormaRecebimentoDTO, FormaRecebimento>();

            Mapper.CreateMap<Grupo, GrupoDTO>();
            Mapper.CreateMap<GrupoDTO, Grupo>();

            Mapper.CreateMap<Nacionalidade, NacionalidadeDTO>();
            Mapper.CreateMap<NacionalidadeDTO, Nacionalidade>();

            Mapper.CreateMap<Parentesco, ParentescoDTO>();
            Mapper.CreateMap<ParentescoDTO, Parentesco>();

            Mapper.CreateMap<Profissao, ProfissaoDTO>();
            Mapper.CreateMap<ProfissaoDTO, Profissao>();

            Mapper.CreateMap<RamoAtividade, RamoAtividadeDTO>();
            Mapper.CreateMap<RamoAtividadeDTO, RamoAtividade>();

            Mapper.CreateMap<Relacionamento, RelacionamentoDTO>();
            Mapper.CreateMap<RelacionamentoDTO, Relacionamento>();

            Mapper.CreateMap<Tipologia, TipologiaDTO>();
            Mapper.CreateMap<TipologiaDTO, Tipologia>();

            Mapper.CreateMap<Tratamento, TratamentoDTO>();
            Mapper.CreateMap<TratamentoDTO, Tratamento>();

            Mapper.CreateMap<TipoArea, TipoAreaDTO>();
            Mapper.CreateMap<TipoAreaDTO, TipoArea>();

            Mapper.CreateMap<TipoCaracteristica, TipoCaracteristicaDTO>();
            Mapper.CreateMap<TipoCaracteristicaDTO, TipoCaracteristica>();

            Mapper.CreateMap<TipoEspecificacao, TipoEspecificacaoDTO>();
            Mapper.CreateMap<TipoEspecificacaoDTO, TipoEspecificacao>();

            #endregion

            # region Sac
            Mapper.CreateMap<ParametrosSac, ParametrosSacDTO>();
            Mapper.CreateMap<ParametrosSacDTO, ParametrosSac>()
                .ForMember(d => d.IconeRelatorio, m => m.ResolveUsing(s => s.IconeRelatorio == null ? null : s.IconeRelatorio));
            # endregion
        }
    }
}