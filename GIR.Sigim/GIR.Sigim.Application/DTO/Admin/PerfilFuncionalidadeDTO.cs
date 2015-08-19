using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Admin
{
    public class PerfilFuncionalidadeDTO : BaseDTO 
    {
        public int? PerfilId { get; set; }
        //public PerfilDTO Perfil { get; set; }
        public string Funcionalidade { get; set; }
    }
}