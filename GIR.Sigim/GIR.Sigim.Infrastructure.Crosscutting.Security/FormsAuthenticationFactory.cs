using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Infrastructure.Crosscutting.Security
{
    public class FormsAuthenticationFactory : IAuthenticationServiceFactory
    {
        public IAuthenticationService Create()
        {
            return new FormsAuthenticationService();
        }
    }
}