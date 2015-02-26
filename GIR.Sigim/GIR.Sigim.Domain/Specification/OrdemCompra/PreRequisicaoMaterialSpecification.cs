using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Domain.Specification.OrdemCompra
{
    public class PreRequisicaoMaterialSpecification : AbstractRequisicaoMaterialSpecification<PreRequisicaoMaterial>
    {
        public static Specification<PreRequisicaoMaterial> UsuarioPossuiAcessoAoCentroCusto(int? idUsuario, string modulo)
        {
            Specification<PreRequisicaoMaterial> specification = new TrueSpecification<PreRequisicaoMaterial>();

            if ((idUsuario.HasValue) && (!string.IsNullOrEmpty(modulo)))
            {
                var directSpecification = new DirectSpecification<PreRequisicaoMaterial>(l =>
                    !l.ListaItens.Any() ||
                    l.ListaItens.All(s =>
                        s.CentroCusto.ListaUsuarioCentroCusto.Any(c =>
                            c.UsuarioId == idUsuario && c.Modulo.Nome == modulo)));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<PreRequisicaoMaterial> EhCancelada()
        {
            return new DirectSpecification<PreRequisicaoMaterial>(l => l.Situacao == SituacaoPreRequisicaoMaterial.Cancelada);
        }

        public static Specification<PreRequisicaoMaterial> EhFechada()
        {
            return new DirectSpecification<PreRequisicaoMaterial>(l => l.Situacao == SituacaoPreRequisicaoMaterial.Fechada);
        }

        public static Specification<PreRequisicaoMaterial> EhParcialmenteAprovada()
        {
            return new DirectSpecification<PreRequisicaoMaterial>(l => l.Situacao == SituacaoPreRequisicaoMaterial.ParcialmenteAprovada);
        }

        public static Specification<PreRequisicaoMaterial> EhRequisitada()
        {
            return new DirectSpecification<PreRequisicaoMaterial>(l => l.Situacao == SituacaoPreRequisicaoMaterial.Requisitada);
        }
    }
}