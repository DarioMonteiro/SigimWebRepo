using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.Sac
{
    public class ParametrosEmailSac : BaseEntity
    {
        public int? ParametrosId { get; set; }
        public ParametrosSac Parametros { get; set; }      
        public int? SetorId { get; set; }
        public Setor Setor { get; set; }                
        public int? Tipo { get; set; }
        public string Email { get; set; }
        public bool Anexo { get; set; }
    }
}