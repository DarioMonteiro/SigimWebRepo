﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class TipoDocumento : BaseEntity
    {
        public string Sigla { get; set; }
        public string Descricao { get; set; }
    }
}
