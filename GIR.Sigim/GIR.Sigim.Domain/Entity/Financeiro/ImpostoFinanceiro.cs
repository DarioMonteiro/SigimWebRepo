using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class ImpostoFinanceiro : BaseEntity
    {
        public string Sigla { get; set; }
        public string Descricao { get; set; }
        public decimal Aliquota { get; set; }
        public bool? EhRetido { get; set; }
        public bool? Indireto { get; set; }
        public bool? PagamentoEletronico { get; set; }
        public TipoCompromisso TipoCompromisso { get; set; }
        public int? TipoCompromissoId { get; set; }
        public ClienteFornecedor Cliente { get; set; }
        public int? ClienteId { get; set; }
        public string ContaContabil { get; set; }
        public PeriodicidadeImpostoFinanceiro? Periodicidade { get; set; }
        public FimDeSemanaImpostoFinanceiro? FimDeSemana { get; set; }
        public FatoGeradorImpostoFinanceiro? FatoGerador { get; set; }
        public Int16? DiaVencimento { get; set; }

        public ICollection<ContratoRetificacaoItemImposto> ListaContratoRetificacaoItemImposto { get; set; }
        public ICollection<EntradaMaterialImposto> ListaEntradaMaterialImposto { get; set; }
        public ICollection<ImpostoPagar> ListaImpostoPagar { get; set; }
        public ICollection<ImpostoReceber> ListaImpostoReceber { get; set; }

        public ImpostoFinanceiro()
        {
            this.ListaContratoRetificacaoItemImposto = new HashSet<ContratoRetificacaoItemImposto>();
            this.ListaEntradaMaterialImposto = new HashSet<EntradaMaterialImposto>();
            this.ListaImpostoPagar = new HashSet<ImpostoPagar>();
            this.ListaImpostoReceber = new HashSet<ImpostoReceber>();
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            bool condicao = false;

            condicao = (Aliquota == 0);
            if (condicao)
            {
                yield return new ValidationResult(string.Format(Resource.Sigim.ErrorMessages.ValorDeveSerMaiorQue, "Aliquota",0));
            }

        }

    }
}