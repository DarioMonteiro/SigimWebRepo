using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.Sac
{
    public class ParametrosSac : BaseEntity
    {
        public int? ClienteId { get; set; }
        public ClienteFornecedor Cliente { get; set; }
        public string Mascara { get; set; }
        public byte[] IconeRelatorio { get; set; }
        public int? PrazoAvaliacao { get; set; }
        public int? PrazoConclusao { get; set; }
        public string EmailEnvio { get; set; }
        public string SenhaEnvio { get; set; }
        public string PortaEnvio { get; set; }
        public string ServidorEnvio { get; set; }
        public string CorpoMensagemAutomaticaSacweb { get; set; }
        public bool? HabilitaSSL { get; set; }

        public ICollection<ParametrosEmailSac> ListaParametrosEmailSac { get; set; }

        public ParametrosSac()
        {
            this.ListaParametrosEmailSac = new HashSet<ParametrosEmailSac>(); 
        }

    }
}