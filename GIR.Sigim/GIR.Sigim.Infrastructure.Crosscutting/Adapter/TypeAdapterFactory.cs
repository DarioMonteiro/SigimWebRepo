using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Infrastructure.Crosscutting.Adapter
{
    public static class TypeAdapterFactory
    {
        static ITypeAdapterFactory currentTypeAdapterFactory = null;

        public static void SetCurrent(ITypeAdapterFactory typeAdapterFactory)
        {
            currentTypeAdapterFactory = typeAdapterFactory;
        }

        public static ITypeAdapter Create()
        {
            if (currentTypeAdapterFactory == null)
                throw new NullReferenceException("Uma instância de ITypeAdapterFactory deve ser fornecida através do método SetCurrent(ITypeAdapterFactory typeAdapterFactory).");
            
            return currentTypeAdapterFactory.Create();
        }
    }
}