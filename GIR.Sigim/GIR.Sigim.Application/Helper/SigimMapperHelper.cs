using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Application.Enums;

namespace GIR.Sigim.Application.Helper
{
    public class SigimMapperHelper
    {
        public static void Initialise()
        {
            Mapper.CreateMap<Agencia, AgenciaDTO>();
            Mapper.CreateMap<AgenciaDTO, Agencia>();

            Mapper.CreateMap<AssuntoContato, AssuntoContatoDTO>();
            Mapper.CreateMap<AssuntoContato, TabelaBasicaDTO>();
            Mapper.CreateMap<AssuntoContatoDTO, AssuntoContato>();


            Mapper.CreateMap<AssuntoContatoDTO, TabelaBasicaDTO>()
               .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.AssuntoContato));
            Mapper.CreateMap<TabelaBasicaDTO, AssuntoContatoDTO>();

            Mapper.CreateMap<AvaliacaoFornecedor, AvaliacaoFornecedorDTO>();
            Mapper.CreateMap<AvaliacaoFornecedorDTO, AvaliacaoFornecedor>();

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

            Mapper.CreateMap<ComplementoCST, ComplementoCSTDTO>();
            Mapper.CreateMap<ComplementoCSTDTO, ComplementoCST>();

            Mapper.CreateMap<ComplementoNaturezaOperacao, ComplementoNaturezaOperacaoDTO>();
            Mapper.CreateMap<ComplementoNaturezaOperacaoDTO, ComplementoNaturezaOperacao>();

            Mapper.CreateMap<ContaCorrente, ContaCorrenteDTO>();
            Mapper.CreateMap<ContaCorrenteDTO, ContaCorrente>();

            Mapper.CreateMap<Composicao, ComposicaoDTO>();
            Mapper.CreateMap<ComposicaoDTO, Composicao>();

            Mapper.CreateMap<CST, CSTDTO>();
            Mapper.CreateMap<CSTDTO, CST>();

            Mapper.CreateMap<InteresseBairro, InteresseBairroDTO>();
            Mapper.CreateMap<InteresseBairro, TabelaBasicaDTO>();
            Mapper.CreateMap<InteresseBairroDTO, InteresseBairro>();

            Mapper.CreateMap<InteresseBairroDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.BairroInteresse));
            Mapper.CreateMap<TabelaBasicaDTO, InteresseBairroDTO>();

            Mapper.CreateMap<Material, MaterialDTO>()
                .ForMember(d => d.DescricaoTipoTabela, m => m.MapFrom(s => s.TipoTabela.ObterDescricao()));
            Mapper.CreateMap<MaterialDTO, Material>();

            Mapper.CreateMap<MaterialClasseInsumo, MaterialClasseInsumoDTO>();
            Mapper.CreateMap<MaterialClasseInsumoDTO, MaterialClasseInsumo>();

            Mapper.CreateMap<NaturezaOperacao, NaturezaOperacaoDTO>();
            Mapper.CreateMap<NaturezaOperacaoDTO, NaturezaOperacao>();

            Mapper.CreateMap<NaturezaReceita, NaturezaReceitaDTO>();
            Mapper.CreateMap<NaturezaReceitaDTO, NaturezaReceita>();

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

            Mapper.CreateMap<MaterialClasseInsumo, TreeNodeDTO>()
                .ForMember(d => d.ListaFilhos, m => m.MapFrom(s => s.ListaFilhos))
                .ForMember(d => d.Ativo, m => m.MapFrom(s => true));

            Mapper.CreateMap<UnidadeMedida, UnidadeMedidaDTO>();
            Mapper.CreateMap<UnidadeMedidaDTO, UnidadeMedida>();

            Mapper.CreateMap<EstadoCivil, EstadoCivilDTO>();
            Mapper.CreateMap<EstadoCivil, TabelaBasicaDTO>();
            Mapper.CreateMap<EstadoCivilDTO, EstadoCivil>();

            Mapper.CreateMap<EstadoCivilDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.EstadoCivil));
            Mapper.CreateMap<TabelaBasicaDTO, EstadoCivilDTO>();

            Mapper.CreateMap<FonteNegocio, FonteNegocioDTO>();
            Mapper.CreateMap<FonteNegocio, TabelaBasicaDTO>();
            Mapper.CreateMap<FonteNegocioDTO, FonteNegocio>();

            Mapper.CreateMap<FonteNegocioDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.FonteNegocio));
            Mapper.CreateMap<TabelaBasicaDTO, FonteNegocioDTO>();

            Mapper.CreateMap<FormaRecebimento, FormaRecebimentoDTO>();
            Mapper.CreateMap<FormaRecebimentoDTO, FormaRecebimento>();

            Mapper.CreateMap<Grupo, GrupoDTO>();
            Mapper.CreateMap<Grupo, TabelaBasicaDTO>();
            Mapper.CreateMap<GrupoDTO, Grupo>();

            Mapper.CreateMap<GrupoDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.Grupo));
            Mapper.CreateMap<TabelaBasicaDTO, GrupoDTO>();

            Mapper.CreateMap<Nacionalidade, NacionalidadeDTO>();
            Mapper.CreateMap<Nacionalidade, TabelaBasicaDTO>();
            Mapper.CreateMap<NacionalidadeDTO, Nacionalidade>();

            Mapper.CreateMap<NacionalidadeDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.Nacionalidade));
            Mapper.CreateMap<TabelaBasicaDTO, NacionalidadeDTO>();

            Mapper.CreateMap<Parentesco, ParentescoDTO>();
            Mapper.CreateMap<Parentesco, TabelaBasicaDTO>();
            Mapper.CreateMap<ParentescoDTO, Parentesco>();

            Mapper.CreateMap<ParentescoDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.Parentesco));
            Mapper.CreateMap<TabelaBasicaDTO, ParentescoDTO>();

            Mapper.CreateMap<Profissao, ProfissaoDTO>();
            Mapper.CreateMap<Profissao, TabelaBasicaDTO>();
            Mapper.CreateMap<ProfissaoDTO, Profissao>();

            Mapper.CreateMap<ProfissaoDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.Profissao));
            Mapper.CreateMap<TabelaBasicaDTO, ProfissaoDTO>();

            Mapper.CreateMap<RamoAtividade, RamoAtividadeDTO>();
            Mapper.CreateMap<RamoAtividade, TabelaBasicaDTO>();
            Mapper.CreateMap<RamoAtividadeDTO, RamoAtividade>();

            Mapper.CreateMap<RamoAtividadeDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.RamoAtividade));
            Mapper.CreateMap<TabelaBasicaDTO, RamoAtividadeDTO>();

            Mapper.CreateMap<Relacionamento, RelacionamentoDTO>();
            Mapper.CreateMap<Relacionamento, TabelaBasicaDTO>();
            Mapper.CreateMap<RelacionamentoDTO, Relacionamento>();

            Mapper.CreateMap<RelacionamentoDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.Relacionamento));
            Mapper.CreateMap<TabelaBasicaDTO, RelacionamentoDTO>();

            Mapper.CreateMap<Tipologia, TipologiaDTO>();
            Mapper.CreateMap<Tipologia, TabelaBasicaDTO>();
            Mapper.CreateMap<TipologiaDTO, Tipologia>();

            Mapper.CreateMap<TipologiaDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.Tipologia));
            Mapper.CreateMap<TabelaBasicaDTO, TipologiaDTO>();

            Mapper.CreateMap<Tratamento, TratamentoDTO>();
            Mapper.CreateMap<Tratamento, TabelaBasicaDTO>();
            Mapper.CreateMap<TratamentoDTO, Tratamento>();

            Mapper.CreateMap<TratamentoDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.Tratamento));
            Mapper.CreateMap<TabelaBasicaDTO, TratamentoDTO>();

            Mapper.CreateMap<TipoArea, TipoAreaDTO>();
            Mapper.CreateMap<TipoArea, TabelaBasicaDTO>();
            Mapper.CreateMap<TipoAreaDTO, TipoArea>();

            Mapper.CreateMap<TipoAreaDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.TipoArea));
            Mapper.CreateMap<TabelaBasicaDTO, TipoAreaDTO>();

            Mapper.CreateMap<TipoCaracteristica, TipoCaracteristicaDTO>();
            Mapper.CreateMap<TipoCaracteristica, TabelaBasicaDTO>();
            Mapper.CreateMap<TipoCaracteristicaDTO, TipoCaracteristica>();

            Mapper.CreateMap<TipoCaracteristicaDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.TipoCaracteristica));
            Mapper.CreateMap<TabelaBasicaDTO, TipoCaracteristicaDTO>();

            Mapper.CreateMap<TipoEspecificacao, TipoEspecificacaoDTO>();
            Mapper.CreateMap<TipoEspecificacao, TabelaBasicaDTO>();
            Mapper.CreateMap<TipoEspecificacaoDTO, TipoEspecificacao>();

            Mapper.CreateMap<TipoEspecificacaoDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.TipoEspecificacao));
            Mapper.CreateMap<TabelaBasicaDTO, TipoEspecificacaoDTO>();

            Mapper.CreateMap<UnidadeFederacao, UnidadeFederacaoDTO>();
            Mapper.CreateMap<UnidadeFederacaoDTO, UnidadeFederacao>();

        }
    }
}