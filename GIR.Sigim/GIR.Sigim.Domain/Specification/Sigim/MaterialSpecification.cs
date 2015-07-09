using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Specification.Sigim
{
    public class MaterialSpecification : BaseSpecification<Material>
    {
        public static Specification<Material> DescricaoContem(string descricao)
        {
            Specification<Material> specification = new TrueSpecification<Material>();

            if (!string.IsNullOrEmpty(descricao))
            {
                var directSpecification = new DirectSpecification<Material>(l => l.Descricao.Contains(descricao));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Material> UnidadeMedidaContem(string descricao)
        {
            Specification<Material> specification = new TrueSpecification<Material>();

            if (!string.IsNullOrEmpty(descricao))
            {
                var directSpecification = new DirectSpecification<Material>(l => l.SiglaUnidadeMedida.Contains(descricao));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Material> MatchingId(int? id)
        {
            Specification<Material> specification = new TrueSpecification<Material>();

            if (id.HasValue)
            {
                var directSpecification = new DirectSpecification<Material>(l => l.Id == id);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Material> ClasseInsumoContem(string descricao)
        {
            Specification<Material> specification = new TrueSpecification<Material>();

            if (!string.IsNullOrEmpty(descricao))
            {
                var directSpecification = new DirectSpecification<Material>(l => (l.CodigoMaterialClasseInsumo + " - " + l.MaterialClasseInsumo.Descricao).Contains(descricao));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Material> CodigoExternoContem(string descricao)
        {
            Specification<Material> specification = new TrueSpecification<Material>();

            if (!string.IsNullOrEmpty(descricao))
            {
                var directSpecification = new DirectSpecification<Material>(l => l.CodigoExterno.Contains(descricao));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Material> DescricaoNoIntervalo(string inicio, string fim)
        {
            Specification<Material> specification = new TrueSpecification<Material>();

            if (!string.IsNullOrEmpty(inicio))
            {
                var directSpecification = new DirectSpecification<Material>(l => l.Descricao.CompareTo(inicio) >= 0);
                specification &= directSpecification;
            }

            if (!string.IsNullOrEmpty(fim))
            {
                var directSpecification = new DirectSpecification<Material>(l => l.Descricao.Substring(0, fim.Length).CompareTo(fim) <= 0);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Material> UnidadeMedidaNoIntervalo(string inicio, string fim)
        {
            Specification<Material> specification = new TrueSpecification<Material>();

            if (!string.IsNullOrEmpty(inicio))
            {
                var directSpecification = new DirectSpecification<Material>(l => l.SiglaUnidadeMedida.CompareTo(inicio) >= 0);
                specification &= directSpecification;
            }

            if (!string.IsNullOrEmpty(fim))
            {
                var directSpecification = new DirectSpecification<Material>(l => l.SiglaUnidadeMedida.Substring(0, fim.Length).CompareTo(fim) <= 0);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Material> IdNoIntervalo(int? inicio, int? fim)
        {
            Specification<Material> specification = new TrueSpecification<Material>();

            if (inicio.HasValue)
            {
                var directSpecification = new DirectSpecification<Material>(l => l.Id >= inicio);
                specification &= directSpecification;
            }

            if (fim.HasValue)
            {
                var directSpecification = new DirectSpecification<Material>(l => l.Id <= fim);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Material> ClasseInsumoNoIntervalo(string inicio, string fim)
        {
            Specification<Material> specification = new TrueSpecification<Material>();

            if (!string.IsNullOrEmpty(inicio))
            {
                var directSpecification = new DirectSpecification<Material>(l => (l.CodigoMaterialClasseInsumo + " - " + l.MaterialClasseInsumo.Descricao).CompareTo(inicio) >= 0);
                specification &= directSpecification;
            }

            if (!string.IsNullOrEmpty(fim))
            {
                var directSpecification = new DirectSpecification<Material>(l => (l.CodigoMaterialClasseInsumo + " - " + l.MaterialClasseInsumo.Descricao).Substring(0, fim.Length).CompareTo(fim) <= 0);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Material> CodigoExternoNoIntervalo(string inicio, string fim)
        {
            Specification<Material> specification = new TrueSpecification<Material>();

            if (!string.IsNullOrEmpty(inicio))
            {
                var directSpecification = new DirectSpecification<Material>(l => l.CodigoExterno.CompareTo(inicio) >= 0);
                specification &= directSpecification;
            }

            if (!string.IsNullOrEmpty(fim))
            {
                var directSpecification = new DirectSpecification<Material>(l => l.CodigoExterno.Substring(0, fim.Length).CompareTo(fim) <= 0);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Material> EhAtivo()
        {
            return new DirectSpecification<Material>(l => l.Situacao == "A" || l.Situacao == null);
        }

        public static Specification<Material> EhTipoTabela(TipoTabela tipoTabela)
        {
            return new DirectSpecification<Material>(l => l.TipoTabela == tipoTabela);
        }

        public static Specification<Material> MatchingAnoMes(int? anoMes)
        {
            Specification<Material> specification = new TrueSpecification<Material>();

            if (anoMes.HasValue)
            {
                var directSpecification = new DirectSpecification<Material>(l => l.AnoMes == anoMes);
                specification &= directSpecification;
            }

            return specification;
        }
    }
}