using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Infrastructure.Crosscutting.Adapter
{
    public interface ITypeAdapter
    {
        TTarget Adapt<TSource, TTarget>(TSource source)
            where TSource : class
            where TTarget : class, new();

        TTarget Adapt<TTarget>(object source)
            where TTarget : class,new();
    }
}