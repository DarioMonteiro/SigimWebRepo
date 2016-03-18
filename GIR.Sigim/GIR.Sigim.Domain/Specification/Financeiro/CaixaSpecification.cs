using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Domain.Specification.Financeiro
{
    public class CaixaSpecification : BaseSpecification<Caixa>
    {
        public static Specification<Caixa> UsuarioPossuiAcessoAoCentroCusto(int? idUsuario, string modulo)
        {
            Specification<Caixa> specification = new TrueSpecification<Caixa>();

            if ((idUsuario.HasValue) && (!string.IsNullOrEmpty(modulo)))
            {
                var directSpecification = new DirectSpecification<Caixa>(l =>
                    l.CentroCusto.ListaUsuarioCentroCusto.Any(c =>
                        c.UsuarioId == idUsuario && c.Modulo.Nome == modulo && c.CentroCusto.Situacao == "A"));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Caixa> EhAtivo()
        {
            return new DirectSpecification<Caixa>(l => l.Situacao == "A");
        }

    }
}
