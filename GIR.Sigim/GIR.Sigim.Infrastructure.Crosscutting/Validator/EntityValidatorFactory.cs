using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Infrastructure.Crosscutting.Validator
{
    public static class EntityValidatorFactory
    {
        static IEntityValidatorFactory currentEntityValidatorFactory = null;

        public static void SetCurrent(IEntityValidatorFactory entityValidatorFactory)
        {
            currentEntityValidatorFactory = entityValidatorFactory;
        }

        public static IEntityValidator Create()
        {
            if (currentEntityValidatorFactory == null)
                throw new NullReferenceException("Uma instância de IEntityValidatorFactory deve ser fornecida através do método SetCurrent(IEntityValidatorFactory entityValidatorFactory).");

            return currentEntityValidatorFactory.Create();
        }
    }
}