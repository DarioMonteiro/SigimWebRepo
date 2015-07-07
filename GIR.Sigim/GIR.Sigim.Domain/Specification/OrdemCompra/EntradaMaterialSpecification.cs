using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Domain.Specification.OrdemCompra
{
    public class EntradaMaterialSpecification : BaseSpecification<EntradaMaterial>
    {
        public static Specification<EntradaMaterial> DataMaiorOuIgual(DateTime? data)
        {
            Specification<EntradaMaterial> specification = new TrueSpecification<EntradaMaterial>();

            if (data.HasValue)
            {
                var directSpecification = new DirectSpecification<EntradaMaterial>(l => l.Data >= data);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<EntradaMaterial> DataMenorOuIgual(DateTime? data)
        {
            Specification<EntradaMaterial> specification = new TrueSpecification<EntradaMaterial>();

            if (data.HasValue)
            {
                var directSpecification = new DirectSpecification<EntradaMaterial>(l => l.Data <= data);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<EntradaMaterial> PertenceAoCentroCustoIniciadoPor(string centroCustoId)
        {
            Specification<EntradaMaterial> specification = new TrueSpecification<EntradaMaterial>();

            if (!string.IsNullOrEmpty(centroCustoId))
            {
                var directSpecification = new DirectSpecification<EntradaMaterial>(l => l.CentroCusto.Codigo.StartsWith(centroCustoId));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<EntradaMaterial> UsuarioPossuiAcessoAoCentroCusto(int? idUsuario, string modulo)
        {
            Specification<EntradaMaterial> specification = new TrueSpecification<EntradaMaterial>();

            if ((idUsuario.HasValue) && (!string.IsNullOrEmpty(modulo)))
            {
                var directSpecification = new DirectSpecification<EntradaMaterial>(l =>
                    l.CentroCusto.ListaUsuarioCentroCusto.Any(c =>
                        c.UsuarioId == idUsuario && c.Modulo.Nome == modulo));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<EntradaMaterial> EhPendente()
        {
            return new DirectSpecification<EntradaMaterial>(l => l.Situacao == SituacaoEntradaMaterial.Pendente);
        }

        public static Specification<EntradaMaterial> EhCancelada()
        {
            return new DirectSpecification<EntradaMaterial>(l => l.Situacao == SituacaoEntradaMaterial.Cancelada);
        }

        public static Specification<EntradaMaterial> EhFechada()
        {
            return new DirectSpecification<EntradaMaterial>(l => l.Situacao == SituacaoEntradaMaterial.Fechada);
        }
    }
}