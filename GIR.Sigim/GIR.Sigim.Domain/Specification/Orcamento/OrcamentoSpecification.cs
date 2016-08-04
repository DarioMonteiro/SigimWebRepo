using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Specification.Orcamento
{
    public class OrcamentoSpecification : BaseSpecification<Domain.Entity.Orcamento.Orcamento>
    {
        public static Specification<Domain.Entity.Orcamento.Orcamento> MatchingEmpresa(int? empresaId)
        {
            Specification<Domain.Entity.Orcamento.Orcamento> specification = new TrueSpecification<Domain.Entity.Orcamento.Orcamento>();

            if (empresaId.HasValue)
            {
                var idSpecification = new DirectSpecification<Domain.Entity.Orcamento.Orcamento>(l => l.EmpresaId == empresaId);
                specification &= idSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.Orcamento.Orcamento> MatchingObra(int? obraId)
        {
            Specification<Domain.Entity.Orcamento.Orcamento> specification = new TrueSpecification<Domain.Entity.Orcamento.Orcamento>();

            if (obraId.HasValue)
            {
                var idSpecification = new DirectSpecification<Domain.Entity.Orcamento.Orcamento>(l => l.ObraId == obraId);
                specification &= idSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.Orcamento.Orcamento> CentroCustoContem(string descricao)
        {
            Specification<Domain.Entity.Orcamento.Orcamento> specification = new TrueSpecification<Domain.Entity.Orcamento.Orcamento>();

            if (!string.IsNullOrEmpty(descricao))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Orcamento.Orcamento>(l => (l.Obra.CodigoCentroCusto + " - " + l.Obra.CentroCusto.Descricao).Contains(descricao));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.Orcamento.Orcamento> CentroCustoNoIntervalo(string inicio, string fim)
        {
            Specification<Domain.Entity.Orcamento.Orcamento> specification = new TrueSpecification<Domain.Entity.Orcamento.Orcamento>();

            if (!string.IsNullOrEmpty(inicio))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Orcamento.Orcamento>(l => (l.Obra.CentroCusto.Codigo + " - " + l.Obra.CentroCusto.Descricao).CompareTo(inicio) >= 0);
                specification &= directSpecification;
            }

            if (!string.IsNullOrEmpty(fim))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Orcamento.Orcamento>(l => (l.Obra.CentroCusto.Codigo + " - " + l.Obra.CentroCusto.Descricao).Substring(0, fim.Length).CompareTo(fim) <= 0);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.Orcamento.Orcamento> DescricaoContem(string descricao)
        {
            Specification<Domain.Entity.Orcamento.Orcamento> specification = new TrueSpecification<Domain.Entity.Orcamento.Orcamento>();

            if (!string.IsNullOrEmpty(descricao))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Orcamento.Orcamento>(l => l.Descricao.Contains(descricao));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.Orcamento.Orcamento> DescricaoNoIntervalo(string inicio, string fim)
        {
            Specification<Domain.Entity.Orcamento.Orcamento> specification = new TrueSpecification<Domain.Entity.Orcamento.Orcamento>();

            if (!string.IsNullOrEmpty(inicio))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Orcamento.Orcamento>(l => l.Descricao.CompareTo(inicio) >= 0);
                specification &= directSpecification;
            }

            if (!string.IsNullOrEmpty(fim))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Orcamento.Orcamento>(l => l.Descricao.Substring(0, fim.Length).CompareTo(fim) <= 0);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.Orcamento.Orcamento> ObraNumeroContem(string obra)
        {
            Specification<Domain.Entity.Orcamento.Orcamento> specification = new TrueSpecification<Domain.Entity.Orcamento.Orcamento>();

            if (!string.IsNullOrEmpty(obra))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Orcamento.Orcamento>(l => l.Obra.Numero.Contains(obra));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.Orcamento.Orcamento> ObraNumeroNoIntervalo(string inicio, string fim)
        {
            Specification<Domain.Entity.Orcamento.Orcamento> specification = new TrueSpecification<Domain.Entity.Orcamento.Orcamento>();

            if (!string.IsNullOrEmpty(inicio))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Orcamento.Orcamento>(l => l.Obra.Numero.CompareTo(inicio) >= 0);
                specification &= directSpecification;
            }

            if (!string.IsNullOrEmpty(fim))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Orcamento.Orcamento>(l => l.Obra.Numero.Substring(0, fim.Length).CompareTo(fim) <= 0);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.Orcamento.Orcamento> SequencialNoIntervalo(int? inicio, int? fim)
        {
            Specification<Domain.Entity.Orcamento.Orcamento> specification = new TrueSpecification<Domain.Entity.Orcamento.Orcamento>();

            if (inicio.HasValue)
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Orcamento.Orcamento>(l => l.Sequencial >= inicio);
                specification &= directSpecification;
            }

            if (fim.HasValue)
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Orcamento.Orcamento>(l => l.Sequencial >= fim);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.Orcamento.Orcamento> EmpresaNumeroContem(string empresa)
        {
            Specification<Domain.Entity.Orcamento.Orcamento> specification = new TrueSpecification<Domain.Entity.Orcamento.Orcamento>();

            if (!string.IsNullOrEmpty(empresa))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Orcamento.Orcamento>(l => l.Empresa.Numero.Contains(empresa));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.Orcamento.Orcamento> EmpresaNumeroNoIntervalo(string inicio, string fim)
        {
            Specification<Domain.Entity.Orcamento.Orcamento> specification = new TrueSpecification<Domain.Entity.Orcamento.Orcamento>();

            if (!string.IsNullOrEmpty(inicio))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Orcamento.Orcamento>(l => l.Empresa.Numero.CompareTo(inicio) >= 0);
                specification &= directSpecification;
            }

            if (!string.IsNullOrEmpty(fim))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Orcamento.Orcamento>(l => l.Empresa.Numero.Substring(0, fim.Length).CompareTo(fim) <= 0);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.Orcamento.Orcamento> DataContem(Nullable<DateTime> data)
        {
            Specification<Domain.Entity.Orcamento.Orcamento> specification = new TrueSpecification<Domain.Entity.Orcamento.Orcamento>();

            if (data.HasValue)
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Orcamento.Orcamento>(l => l.Data == data);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.Orcamento.Orcamento> DataNoIntervalo(Nullable<DateTime> inicio, Nullable<DateTime> fim)
        {
            Specification<Domain.Entity.Orcamento.Orcamento> specification = new TrueSpecification<Domain.Entity.Orcamento.Orcamento>();

            if (inicio.HasValue)
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Orcamento.Orcamento>(l => l.Data.Value >= inicio);
                specification &= directSpecification;
            }

            if (fim.HasValue)
            {
                DateTime dataUltimaHora = new DateTime(fim.Value.Year, fim.Value.Month, fim.Value.Day, 23, 59, 59);

                var directSpecification = new DirectSpecification<Domain.Entity.Orcamento.Orcamento>(l => l.Data.Value <= dataUltimaHora);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.Orcamento.Orcamento> SituacaoContem(string situacao)
        {
            Specification<Domain.Entity.Orcamento.Orcamento> specification = new TrueSpecification<Domain.Entity.Orcamento.Orcamento>();

            if (!string.IsNullOrEmpty(situacao))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Orcamento.Orcamento>(l => l.Situacao.Contains(situacao));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Domain.Entity.Orcamento.Orcamento> SituacaoNoIntervalo(string inicio, string fim)
        {
            Specification<Domain.Entity.Orcamento.Orcamento> specification = new TrueSpecification<Domain.Entity.Orcamento.Orcamento>();

            if (!string.IsNullOrEmpty(inicio))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Orcamento.Orcamento>(l => l.Situacao.CompareTo(inicio) >= 0);
                specification &= directSpecification;
            }

            if (!string.IsNullOrEmpty(fim))
            {
                var directSpecification = new DirectSpecification<Domain.Entity.Orcamento.Orcamento>(l => l.Situacao.Substring(0, fim.Length).CompareTo(fim) <= 0);
                specification &= directSpecification;
            }

            return specification;
        }

    }

}
