using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Infrastructure.Crosscutting.Security
{
    public class CustomPrincipalSerializeModel
    {
        public int? Id { get; set; }
        public string Login { get; set; }
        public string Nome { get; set; }
        public string[] Roles { get; set; }
    }
}