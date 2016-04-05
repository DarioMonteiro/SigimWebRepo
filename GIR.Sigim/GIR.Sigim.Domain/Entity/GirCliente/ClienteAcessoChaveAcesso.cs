using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.GirCliente
{
    public class ClienteAcessoChaveAcesso
    {
        public ClienteFornecedor ClienteFornecedor { get; set; }
        public Nullable<DateTime> DataExpiracao { get; set; }
        public string NomeUsuario { get; set; }
        public int? NumeroUsuario { get; set; }

        public ClienteAcessoChaveAcesso()
        {
            this.ClienteFornecedor = new ClienteFornecedor();
        }
    }
}
