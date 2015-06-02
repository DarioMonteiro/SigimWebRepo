using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;  
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Domain.Entity.Contrato;

namespace GIR.Sigim.Application.DTO.Contrato
{
    public class ContratoRetificacaoItemDTO : BaseDTO 
    {
        public int ContratoId { get; set; }
        public ContratoDTO Contrato { get; set; }
        public int ContratoRetificacaoId { get; set; }
        public ContratoRetificacaoDTO ContratoRetificacao { get; set; }
        public Int16 Sequencial { get; set; }
        public string ComplementoDescricao { get; set; }
        public NaturezaItem NaturezaItem { get; set; }
        [Display(Name = "Natureza")]
        public string DescricaoNaturezaItem
        {
            get { return this.Id.HasValue ? this.NaturezaItem.ObterDescricao() : "" ; }
        }
        public bool EhNaturezaItemGenericoPorPrecoGlobal 
        {
            get { return NaturezaItem == NaturezaItem.PrecoGlobal; }
            set { NaturezaItem = value ? NaturezaItem.PrecoGlobal : NaturezaItem.PrecoUnitario; } 
        }
        public bool EhNaturezaItemGenericoPorPrecoUnitario 
        {
            get { return NaturezaItem == NaturezaItem.PrecoUnitario; }
            set { NaturezaItem = value ? NaturezaItem.PrecoUnitario : NaturezaItem.PrecoGlobal; }
        }
        public int ServicoId { get; set; }
        public ServicoDTO Servico { get; set; }
        public string SequencialDescricaoItemComplemento 
        {
            get
            {
                string sequencialDescricaoComplemento;
                if (Servico != null)
                {
                    if (string.IsNullOrEmpty(ComplementoDescricao))
                    {
                        sequencialDescricaoComplemento = Sequencial.ToString() + " - " + Servico.Descricao;
                    }
                    else
                    {
                        sequencialDescricaoComplemento = Sequencial.ToString() + " - " + Servico.Descricao + " ( " + ComplementoDescricao + " ) ";
                    }
                }
                else
                {
                    sequencialDescricaoComplemento = Sequencial.ToString();
                }

                return sequencialDescricaoComplemento;
            }
        }
        public decimal Quantidade { get; set; }
        [Display(Name = "Preço unitário")]
        public decimal PrecoUnitario { get; set; }
        public decimal? ValorItem { get; set; }
        public string CodigoClasse { get; set; }
        public ClasseDTO Classe { get; set; }
        public decimal? RetencaoItem { get; set; }
        public decimal? BaseRetencaoItem { get; set; }
        public int? RetencaoPrazoResgate { get; set; }
        public bool? Alterado { get; set; }
        public int? RetencaoTipoCompromissoId { get; set; }
        public TipoCompromissoDTO RetencaoTipoCompromisso { get; set; }

        public ContratoRetificacaoItemDTO()
        {
            this.Contrato = new ContratoDTO();
            this.ContratoRetificacao = new ContratoRetificacaoDTO();
            this.Servico = new ServicoDTO();
            this.Classe = new ClasseDTO();
            this.RetencaoTipoCompromisso = new TipoCompromissoDTO();
        }

    }
}
