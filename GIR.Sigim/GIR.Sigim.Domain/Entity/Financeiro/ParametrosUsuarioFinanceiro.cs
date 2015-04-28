using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Admin;
//using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Resource;

namespace GIR.Sigim.Domain.Entity.Financeiro
{

    [Flags] 
    public enum TipoImpressoraEnum
    {
        Bematech,
        Pertocheck
    }

    public class ParametrosUsuarioFinanceiro : BaseEntity 
    {
        public Usuario Usuario { get; set; }
        public String TipoImpressora { get; set; }
        public string PortaSerial { get; set; }
        
        public TipoImpressoraEnum TipoImpressoraEscolhida
        {
            get {
                return (TipoImpressora == TipoImpressoraEnum.Pertocheck.ToString().ToUpper()) ? TipoImpressoraEnum.Pertocheck : TipoImpressoraEnum.Bematech;
                }
            set { TipoImpressora = (value == TipoImpressoraEnum.Pertocheck) ? TipoImpressoraEnum.Pertocheck.ToString().ToUpper() : TipoImpressoraEnum.Bematech.ToString().ToUpper(); }
        }

    }
}
