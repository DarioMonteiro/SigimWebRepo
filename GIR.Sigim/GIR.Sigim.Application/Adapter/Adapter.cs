using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Infrastructure.Crosscutting.Adapter;

namespace GIR.Sigim.Application.Adapter
{
    public static class Adapter
    {
        public static TTarget To<TTarget>(this object source)
            where TTarget : class, new()
        {
            return TypeAdapterFactory.Create().Adapt<TTarget>(source);
        }
    }
}