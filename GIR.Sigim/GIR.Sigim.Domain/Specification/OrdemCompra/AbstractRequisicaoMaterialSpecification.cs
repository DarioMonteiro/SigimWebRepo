using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Domain.Specification.OrdemCompra
{
    public abstract class AbstractRequisicaoMaterialSpecification<T> : BaseSpecification<T> where T : AbstractRequisicaoMaterial
    {
        public static Specification<T> DataMaiorOuIgual(DateTime? data)
        {
            Specification<T> specification = new TrueSpecification<T>();

            if (data.HasValue)
            {
                var directSpecification = new DirectSpecification<T>(l => l.Data >= data);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<T> DataMenorOuIgual(DateTime? data)
        {
            Specification<T> specification = new TrueSpecification<T>();

            if (data.HasValue)
            {
                var directSpecification = new DirectSpecification<T>(l => l.Data <= data);
                specification &= directSpecification;
            }

            return specification;
        }
    }
}