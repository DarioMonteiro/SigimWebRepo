﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class RamoAtividade: BaseEntity
    {
        public string Descricao { get; set; }
        public bool? Automatico { get; set; }

    }
}