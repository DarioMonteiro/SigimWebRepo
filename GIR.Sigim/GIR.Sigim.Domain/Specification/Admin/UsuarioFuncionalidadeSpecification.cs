using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Admin;

namespace GIR.Sigim.Domain.Specification.Admin
{
    public class UsuarioFuncionalidadeSpecification : BaseSpecification<UsuarioFuncionalidade>
    {
        public static Specification<UsuarioFuncionalidade> IgualAoUsuarioId(int? UsuarioId)
        {
            Specification<UsuarioFuncionalidade> specification = new TrueSpecification<UsuarioFuncionalidade>();

            if (UsuarioId.HasValue)
            {
                var directSpecification = new DirectSpecification<UsuarioFuncionalidade>(l => l.UsuarioId == UsuarioId);
                specification &= directSpecification;
            }

            return specification;
        }
        public static Specification<UsuarioFuncionalidade> IgualAoModuloId(int? ModuloId)
        {
            Specification<UsuarioFuncionalidade> specification = new TrueSpecification<UsuarioFuncionalidade>();

            if (ModuloId.HasValue)
            {
                var directSpecification = new DirectSpecification<UsuarioFuncionalidade>(l => l.ModuloId == ModuloId);
                specification &= directSpecification;
            }

            return specification;
        }

    }
}