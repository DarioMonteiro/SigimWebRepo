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

namespace GIR.Sigim.Application.Helper
{
    public class SigimMapperHelper
    {
        public static void Initialise()
        {
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

            Mapper.CreateMap<Composicao, ComposicaoDTO>();
            Mapper.CreateMap<ComposicaoDTO, Composicao>();

            Mapper.CreateMap<CST, CSTDTO>();
            Mapper.CreateMap<CSTDTO, CST>();

            Mapper.CreateMap<InteresseBairro, InteresseBairroDTO>();
            Mapper.CreateMap<InteresseBairroDTO, InteresseBairro>();

            Mapper.CreateMap<Material, MaterialDTO>()
                .ForMember(d => d.DescricaoTipoTabela, m => m.MapFrom(s => s.TipoTabela.ObterDescricao()));
            Mapper.CreateMap<MaterialDTO, Material>();

            Mapper.CreateMap<MaterialClasseInsumo, MaterialClasseInsumoDTO>();
            Mapper.CreateMap<MaterialClasseInsumoDTO, MaterialClasseInsumo>();

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

            Mapper.CreateMap<MaterialClasseInsumo, TreeNodeDTO>()
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
        }
    }
}