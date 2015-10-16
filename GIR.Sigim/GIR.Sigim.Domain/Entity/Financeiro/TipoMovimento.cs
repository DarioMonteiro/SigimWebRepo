using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class TipoMovimento : BaseEntity
    {
        public string Descricao { get; set; }
        public string Operacao { get; set; }
        public bool Automatico { get; set; }
        public int? HistoricoContabilId { get; set; }
        public HistoricoContabil HistoricoContabil { get; set; }
        public string Tipo { get; set; }
        public Byte? FormaPagamento { get; set; }

        public ICollection<MovimentoFinanceiro> ListaMovimentoFinanceiro { get; set; }

        public TipoMovimento()
        {
            this.ListaMovimentoFinanceiro = new HashSet<MovimentoFinanceiro>();
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Descricao) )
            {
                yield return new ValidationResult(string.Format(Resource.Sigim.ErrorMessages.CampoObrigatorio, "Descrição"));
            }
        }

    }
}