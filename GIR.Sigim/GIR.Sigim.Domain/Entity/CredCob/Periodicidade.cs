using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.CredCob
{
    public enum Periodicidade : byte
    {
        [Description("Única")]
        Unica = 0,

        [Description("Mensal")]
        Mensal = 1,

        [Description("Bimestral")]
        Bimestral = 2,

        [Description("Trimestral")]
        Trimestral = 3,

        [Description("Quadrimestral")]
        Quadrimestral = 4,

        [Description("Pentamestral")]
        Pentamestral = 5,

        [Description("Semestral")]
        Semestral = 6,

        [Description("Heptamestral")]
        Heptamestral = 7,

        [Description("Octamestral")]
        Octamestral = 8,

        [Description("Nonamestral")]
        Nonamestral = 9,

        [Description("Decamestral")]
        Decamestral = 10,

        [Description("Undemestral")]
        Undemestral = 11,

        [Description("Anual")]
        Anual = 12
    }
}
