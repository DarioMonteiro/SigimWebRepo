using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Admin;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Domain.Entity.OrdemCompra
{
    public class ParametrosUsuario : BaseEntity
    {
        public Usuario Usuario { get; set; }
        public string CodigoCentroCusto { get; set; }
        public CentroCusto CentroCusto { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Senha))
            {
                yield return new ValidationResult(Domain.Resource.OrdemCompra.ErrorMessages.SenhaDoEmailObrigatoria, new[] { "Senha" });
            }
        }
    }
}