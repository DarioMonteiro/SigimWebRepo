using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Domain.Specification.OrdemCompra
{
    public class RequisicaoMaterialSpecification : AbstractRequisicaoMaterialSpecification<RequisicaoMaterial>
    {
        public static Specification<RequisicaoMaterial> PertenceAoCentroCustoIniciadoPor(string centroCustoId)
        {
            Specification<RequisicaoMaterial> specification = new TrueSpecification<RequisicaoMaterial>();

            if (!string.IsNullOrEmpty(centroCustoId))
            {
                var directSpecification = new DirectSpecification<RequisicaoMaterial>(l => l.CentroCusto.Codigo.StartsWith(centroCustoId));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<RequisicaoMaterial> UsuarioPossuiAcessoAoCentroCusto(int? idUsuario, string modulo)
        {
            Specification<RequisicaoMaterial> specification = new TrueSpecification<RequisicaoMaterial>();

            if ((idUsuario.HasValue) && (!string.IsNullOrEmpty(modulo)))
            {
                var directSpecification = new DirectSpecification<RequisicaoMaterial>(l =>
                    l.CentroCusto.ListaUsuarioCentroCusto.Any(c =>
                        c.UsuarioId == idUsuario && c.Modulo.Nome == modulo));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<RequisicaoMaterial> EhAprovada()
        {
            return new DirectSpecification<RequisicaoMaterial>(l => l.Situacao == SituacaoRequisicaoMaterial.Aprovada);
        }

        public static Specification<RequisicaoMaterial> EhCancelada()
        {
            return new DirectSpecification<RequisicaoMaterial>(l => l.Situacao == SituacaoRequisicaoMaterial.Cancelada);
        }

        public static Specification<RequisicaoMaterial> EhFechada()
        {
            return new DirectSpecification<RequisicaoMaterial>(l => l.Situacao == SituacaoRequisicaoMaterial.Fechada);
        }

        public static Specification<RequisicaoMaterial> EhRequisitada()
        {
            return new DirectSpecification<RequisicaoMaterial>(l => l.Situacao == SituacaoRequisicaoMaterial.Requisitada);
        }
    }
}