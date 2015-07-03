using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Contrato;


namespace GIR.Sigim.Domain.Specification.Contrato
{
    public class ContratoSpecification : BaseSpecification<Domain.Entity.Contrato.Contrato>
    {
        public static Specification<Domain.Entity.Contrato.Contrato> PertenceAoCentroCustoIniciadoPor(string centroCustoId)
        {
            Specification<Domain.Entity.Contrato.Contrato> specification = new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            if (!string.IsNullOrEmpty(centroCustoId))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Contrato.Contrato>(l => l.CentroCusto.Codigo.StartsWith(centroCustoId));
                specification &= directSpecification; 
            }

            return specification;
        }

        public static Specification<Domain.Entity.Contrato.Contrato> PertenceAoContratante(int? idContratante)
        {
            Specification<Domain.Entity.Contrato.Contrato> specification = new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            if (idContratante.HasValue)
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Contrato.Contrato>(l => l.ContratanteId == idContratante);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.Contrato.Contrato> PertenceAoContratado(int? idContratado)
        {
            Specification<Domain.Entity.Contrato.Contrato> specification = new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            if (idContratado.HasValue)
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Contrato.Contrato>(l => l.ContratadoId == idContratado);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.Contrato.Contrato> UsuarioPossuiAcessoAoCentroCusto(int? idUsuario, string modulo)
        {
            Specification<Domain.Entity.Contrato.Contrato> specification = new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            if ((idUsuario.HasValue) && (!string.IsNullOrEmpty(modulo)))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Contrato.Contrato>(l =>
                    l.CentroCusto.ListaUsuarioCentroCusto.Any(c =>
                            c.UsuarioId == idUsuario && c.Modulo.Nome == modulo && c.CentroCusto.Situacao == "A"));
                specification &= directSpecification;
            }

            return specification;
        }

    }
}
