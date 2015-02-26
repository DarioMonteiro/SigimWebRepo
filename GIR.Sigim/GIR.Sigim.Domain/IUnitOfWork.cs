using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        void RollbackChanges();
    }
}