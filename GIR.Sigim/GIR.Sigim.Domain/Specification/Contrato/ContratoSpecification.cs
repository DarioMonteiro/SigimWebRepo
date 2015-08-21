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

        public static Specification<Domain.Entity.Contrato.Contrato> IdNoIntervalo(int? inicio, int? fim)
        {
            Specification<Domain.Entity.Contrato.Contrato> specification = new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            if (inicio.HasValue)
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Contrato.Contrato>(l => l.Id >= inicio);
                specification &= directSpecification;
            }

            if (fim.HasValue)
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Contrato.Contrato>(l => l.Id <= fim);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.Contrato.Contrato> CentroCustoContem(string descricao)
        {
            Specification<Domain.Entity.Contrato.Contrato> specification = new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            if (!string.IsNullOrEmpty(descricao))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Contrato.Contrato>(l => (l.CodigoCentroCusto + " - " + l.CentroCusto.Descricao).Contains(descricao));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.Contrato.Contrato> CentroCustoNoIntervalo(string inicio, string fim)
        {
            Specification<Domain.Entity.Contrato.Contrato> specification = new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            if (!string.IsNullOrEmpty(inicio))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Contrato.Contrato>(l => (l.CodigoCentroCusto + " - " + l.CentroCusto.Descricao).CompareTo(inicio) >= 0);
                specification &= directSpecification;
            }

            if (!string.IsNullOrEmpty(fim))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Contrato.Contrato>(l => (l.CodigoCentroCusto + " - " + l.CentroCusto.Descricao).Substring(0, fim.Length).CompareTo(fim) <= 0);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.Contrato.Contrato> DescricaoContratoContem(string descricaoContrato)
        {
            Specification<Domain.Entity.Contrato.Contrato> specification = new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            if (!string.IsNullOrEmpty(descricaoContrato))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Contrato.Contrato>(l => l.ContratoDescricao.Descricao.Contains(descricaoContrato));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.Contrato.Contrato> DescricaoContratoNoIntervalo(string inicio, string fim)
        {
            Specification<Domain.Entity.Contrato.Contrato> specification = new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            if (!string.IsNullOrEmpty(inicio))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Contrato.Contrato>(l => l.ContratoDescricao.Descricao.CompareTo(inicio) >= 0);
                specification &= directSpecification;
            }

            if (!string.IsNullOrEmpty(fim))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Contrato.Contrato>(l => l.ContratoDescricao.Descricao.Substring(0, fim.Length).CompareTo(fim) <= 0);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.Contrato.Contrato> ContratanteContem(string nome)
        {
            Specification<Domain.Entity.Contrato.Contrato> specification = new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            if (!string.IsNullOrEmpty(nome))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Contrato.Contrato>(l => l.Contratante.Nome.Contains(nome));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.Contrato.Contrato> ContratanteNoIntervalo(string inicio, string fim)
        {
            Specification<Domain.Entity.Contrato.Contrato> specification = new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            if (!string.IsNullOrEmpty(inicio))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Contrato.Contrato>(l => l.Contratante.Nome.CompareTo(inicio) >= 0);
                specification &= directSpecification;
            }

            if (!string.IsNullOrEmpty(fim))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Contrato.Contrato>(l => l.Contratante.Nome.Substring(0, fim.Length).CompareTo(fim) <= 0);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.Contrato.Contrato> ContratadoContem(string nome)
        {
            Specification<Domain.Entity.Contrato.Contrato> specification = new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            if (!string.IsNullOrEmpty(nome))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Contrato.Contrato>(l => l.Contratado.Nome.Contains(nome));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.Contrato.Contrato> ContratadoNoIntervalo(string inicio, string fim)
        {
            Specification<Domain.Entity.Contrato.Contrato> specification = new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            if (!string.IsNullOrEmpty(inicio))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Contrato.Contrato>(l => l.Contratado.Nome.CompareTo(inicio) >= 0);
                specification &= directSpecification;
            }

            if (!string.IsNullOrEmpty(fim))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Contrato.Contrato>(l => l.Contratado.Nome.Substring(0, fim.Length).CompareTo(fim) <= 0);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.Contrato.Contrato> DataAssinaturaContem(Nullable<DateTime> dataAssinatura)
        {
            Specification<Domain.Entity.Contrato.Contrato> specification = new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            if (dataAssinatura.HasValue)
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Contrato.Contrato>(l => l.DataAssinatura == dataAssinatura);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.Contrato.Contrato> DataAssinaturaNoIntervalo(Nullable<DateTime> inicio, Nullable<DateTime> fim)
        {
            Specification<Domain.Entity.Contrato.Contrato> specification = new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            if (inicio.HasValue)
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Contrato.Contrato>(l => l.DataAssinatura.Value >= inicio);
                specification &= directSpecification;
            }

            if (fim.HasValue)
            {
                DateTime dataUltimaHora = new DateTime(fim.Value.Year, fim.Value.Month, fim.Value.Day, 23, 59, 59);

                var directSpecification = new DirectSpecification<Domain.Entity.Contrato.Contrato>(l => l.DataAssinatura.Value <= dataUltimaHora);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.Contrato.Contrato> EhSituacaoIgual(SituacaoContrato? situacao)
        {
            Specification<Domain.Entity.Contrato.Contrato> specification = new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            if (situacao.HasValue)
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Contrato.Contrato>(l => l.Situacao == situacao);
                specification &= directSpecification;
            }

            return specification;
        }


    }
}
