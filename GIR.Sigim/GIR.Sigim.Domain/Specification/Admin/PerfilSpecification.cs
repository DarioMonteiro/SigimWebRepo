using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Admin;

namespace GIR.Sigim.Domain.Specification.Admin
{
    public class PerfilSpecification : BaseSpecification<Perfil>
    {
        public static Specification<Perfil> IgualAoModuloId(int? moduloId)
        {
            Specification<Perfil> specification = new TrueSpecification<Perfil>();

            if (moduloId.HasValue)
            {
                var directSpecification = new DirectSpecification<Perfil>(l => l.ModuloId == moduloId);
                specification &= directSpecification;
            }

            return specification;
        }

    }
}