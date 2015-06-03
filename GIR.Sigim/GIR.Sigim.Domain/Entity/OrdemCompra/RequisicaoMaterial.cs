using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Admin;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Resource;

namespace GIR.Sigim.Domain.Entity.OrdemCompra
{
    public class RequisicaoMaterial : AbstractRequisicaoMaterial
    {
        public string CodigoCentroCusto { get; set; }
        public virtual CentroCusto CentroCusto { get; set; }
        public SituacaoRequisicaoMaterial Situacao { get; set; }
        public Nullable<DateTime> DataAprovacao { get; set; }
        //TODO: Criar relação com a classe Usuario
        public string LoginUsuarioAprovacao { get; set; }
        public ICollection<RequisicaoMaterialItem> ListaItens { get; set; }

        public RequisicaoMaterial()
        {
            this.ListaItens = new HashSet<RequisicaoMaterialItem>();
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(CodigoCentroCusto))
            {
                yield return new ValidationResult(string.Format(ErrorMessages.CampoObrigatorio, "Centro de Custo"));
            }

            if (Data == DateTime.MinValue)
            {
                yield return new ValidationResult(string.Format(ErrorMessages.CampoObrigatorio, "Data"));
            }
        }
    }
}