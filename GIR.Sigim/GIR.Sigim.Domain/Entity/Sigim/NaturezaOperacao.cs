﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class NaturezaOperacao : BaseEntity
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
    }
}