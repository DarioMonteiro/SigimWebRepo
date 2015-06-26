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

        public static Specification<Material> EhAtivo()
        {
            return new DirectSpecification<Material>(l => l.Situacao == "A");
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