using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Infrastructure.Crosscutting.Security
{
    public static class AuthenticationServiceFactory
    {
        private static IAuthenticationServiceFactory current = null;

        public static void SetCurrent(IAuthenticationServiceFactory authenticationFactory)
        {
            current = authenticationFactory;
        }

        public static IAuthenticationService Create()
        {
            if (current == null)
                throw new NullReferenceException("Uma instância de IAuthenticationServiceFactory deve ser fornecida através do método SetCurrent(IAuthenticationServiceFactory authenticationFactory).");

            return current.Create();
        }
    }
}