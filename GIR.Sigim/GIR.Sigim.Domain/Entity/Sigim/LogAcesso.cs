using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Resource;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class LogAcesso : BaseEntity
    {
        public Nullable<DateTime> Data { get; set; }
        //TODO: Alterar tipo IN/OUT para um enum
        public string Tipo { get; set; }
        public string HostName { get; set; }
        public string LoginUsuario { get; set; }
        public string Sistema { get; set; }
        public string Versao { get; set; }
        public string NomeBaseDados { get; set; }
        public string Servidor { get; set; }
    }
}