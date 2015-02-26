using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Infrastructure.Crosscutting.Security
{
    public interface IAuthenticationService
    {
        CustomPrincipal GetUser();
        void Login(string userName, bool isPersistent, string customData, int timeout);
        void Logout();
        void PostAuthenticateRequest();
    }
}