using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Infrastructure.Crosscutting.Security
{
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }
        public int? Id { get; set; }
        public string Login { get; set; }
        public string Nome { get; set; }
        public string HostName { get; set; }
        public string[] Roles { get; set; }

        public CustomPrincipal(string login)
        {
            this.Identity = new GenericIdentity(login);
        }

        public bool IsInRole(string role)
        {
            if ((Login.ToUpper() == "SIGIM") || (Login.ToUpper() == "GIR"))
                return true;

            return Roles.Any(l => l == role);
        }
    }
}