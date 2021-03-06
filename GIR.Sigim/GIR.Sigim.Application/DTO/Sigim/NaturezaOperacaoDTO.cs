﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Sigim
{
    public class NaturezaOperacaoDTO : BaseDTO
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string CodigoComDescricao
        {
            get { return "(" + Codigo + ") " + Descricao; }
        }
    }
}
