using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.CredCob;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class FormaRecebimento : BaseEntity
    {
        public string Descricao { get; set; }
        public string TipoRecebimento { get; set; }
        public bool? Automatico { get; set; }
        public int? NumeroDias { get; set; }

        public ICollection<TituloCredCob> ListaTituloCredCob { get; set; }

        public FormaRecebimento()
        {
            this.ListaTituloCredCob = new HashSet<TituloCredCob>();
        }


        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Descricao))
            {
                yield return new ValidationResult(string.Format(Resource.Sigim.ErrorMessages.CampoObrigatorio, "Descrição"));
            }

            if (string.IsNullOrEmpty(TipoRecebimento))
            {
                yield return new ValidationResult(string.Format(Resource.Sigim.ErrorMessages.CampoObrigatorio, "Descrição"));
            }

        }

    }
}