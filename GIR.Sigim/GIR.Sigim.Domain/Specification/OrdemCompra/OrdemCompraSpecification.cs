using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Domain.Specification.OrdemCompra
{
    public class OrdemCompraSpecification : BaseSpecification<Domain.Entity.OrdemCompra.OrdemCompra>
    {
        public static Specification<Domain.Entity.OrdemCompra.OrdemCompra> IdNoIntervalo(int? inicio, int? fim)
        {
            Specification<Domain.Entity.OrdemCompra.OrdemCompra> specification = new TrueSpecification<Domain.Entity.OrdemCompra.OrdemCompra>();

            if (inicio.HasValue)
            {
                var directSpecification = new DirectSpecification<Domain.Entity.OrdemCompra.OrdemCompra>(l => l.Id >= inicio);
                specification &= directSpecification;
            }

            if (fim.HasValue)
            {
                var directSpecification = new DirectSpecification<Domain.Entity.OrdemCompra.OrdemCompra>(l => l.Id <= fim);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.OrdemCompra.OrdemCompra> CentroCustoContem(string descricao)
        {
            Specification<Domain.Entity.OrdemCompra.OrdemCompra> specification = new TrueSpecification<Domain.Entity.OrdemCompra.OrdemCompra>();

            if (!string.IsNullOrEmpty(descricao))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.OrdemCompra.OrdemCompra>(l => (l.CodigoCentroCusto + " - " + l.CentroCusto.Descricao).Contains(descricao));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.OrdemCompra.OrdemCompra> CentroCustoNoIntervalo(string inicio, string fim)
        {
            Specification<Domain.Entity.OrdemCompra.OrdemCompra> specification = new TrueSpecification<Domain.Entity.OrdemCompra.OrdemCompra>();

            if (!string.IsNullOrEmpty(inicio))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.OrdemCompra.OrdemCompra>(l => (l.CodigoCentroCusto + " - " + l.CentroCusto.Descricao).CompareTo(inicio) >= 0);
                specification &= directSpecification;
            }

            if (!string.IsNullOrEmpty(fim))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.OrdemCompra.OrdemCompra>(l => (l.CodigoCentroCusto + " - " + l.CentroCusto.Descricao).Substring(0, fim.Length).CompareTo(fim) <= 0);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.OrdemCompra.OrdemCompra> FornecedorContem(string nome)
        {
            Specification<Domain.Entity.OrdemCompra.OrdemCompra> specification = new TrueSpecification<Domain.Entity.OrdemCompra.OrdemCompra>();

            if (!string.IsNullOrEmpty(nome))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.OrdemCompra.OrdemCompra>(l => l.ClienteFornecedor.Nome.Contains(nome));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.OrdemCompra.OrdemCompra> FornecedorNoIntervalo(string inicio, string fim)
        {
            Specification<Domain.Entity.OrdemCompra.OrdemCompra> specification = new TrueSpecification<Domain.Entity.OrdemCompra.OrdemCompra>();

            if (!string.IsNullOrEmpty(inicio))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.OrdemCompra.OrdemCompra>(l => l.ClienteFornecedor.Nome.CompareTo(inicio) >= 0);
                specification &= directSpecification;
            }

            if (!string.IsNullOrEmpty(fim))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.OrdemCompra.OrdemCompra>(l => l.ClienteFornecedor.Nome.Substring(0, fim.Length).CompareTo(fim) <= 0);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.OrdemCompra.OrdemCompra> DataOrdemCompraContem(Nullable<DateTime> dataOrdemCompra)
        {
            Specification<Domain.Entity.OrdemCompra.OrdemCompra> specification = new TrueSpecification<Domain.Entity.OrdemCompra.OrdemCompra>();

            if (dataOrdemCompra.HasValue)
            {
                var directSpecification = new DirectSpecification<Domain.Entity.OrdemCompra.OrdemCompra>(l => l.Data == dataOrdemCompra);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.OrdemCompra.OrdemCompra> DataOrdemCompraNoIntervalo(Nullable<DateTime> inicio, Nullable<DateTime> fim)
        {
            Specification<Domain.Entity.OrdemCompra.OrdemCompra> specification = new TrueSpecification<Domain.Entity.OrdemCompra.OrdemCompra>();

            if (inicio.HasValue)
            {
                var directSpecification = new DirectSpecification<Domain.Entity.OrdemCompra.OrdemCompra>(l => l.Data >= inicio);
                specification &= directSpecification;
            }

            if (fim.HasValue)
            {
                DateTime dataUltimaHora = new DateTime(fim.Value.Year, fim.Value.Month, fim.Value.Day, 23, 59, 59);

                var directSpecification = new DirectSpecification<Domain.Entity.OrdemCompra.OrdemCompra>(l => l.Data <= dataUltimaHora);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.OrdemCompra.OrdemCompra> PertenceAoCentroCustoIniciadoPor(string centroCustoId)
        {
            Specification<Domain.Entity.OrdemCompra.OrdemCompra> specification = new TrueSpecification<Domain.Entity.OrdemCompra.OrdemCompra>();

            if (!string.IsNullOrEmpty(centroCustoId))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.OrdemCompra.OrdemCompra>(l => l.CentroCusto.Codigo.StartsWith(centroCustoId));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.OrdemCompra.OrdemCompra> MatchingFornecedorId(int? fornecedorId)
        {
            Specification<Domain.Entity.OrdemCompra.OrdemCompra> specification = new TrueSpecification<Domain.Entity.OrdemCompra.OrdemCompra>();

            if (fornecedorId.HasValue)
            {
                var idSpecification = new DirectSpecification<Domain.Entity.OrdemCompra.OrdemCompra>(l => l.ClienteFornecedorId == fornecedorId);
                specification &= idSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.OrdemCompra.OrdemCompra> EhLiberada()
        {
            return new DirectSpecification<Domain.Entity.OrdemCompra.OrdemCompra>(l => l.Situacao == SituacaoOrdemCompra.Liberada);
        }
    }
}