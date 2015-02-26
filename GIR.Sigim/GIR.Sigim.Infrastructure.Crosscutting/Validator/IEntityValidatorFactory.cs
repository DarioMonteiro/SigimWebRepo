using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Infrastructure.Crosscutting.Validator
{
    public interface IEntityValidatorFactory
    {
        IEntityValidator Create();
    }
}