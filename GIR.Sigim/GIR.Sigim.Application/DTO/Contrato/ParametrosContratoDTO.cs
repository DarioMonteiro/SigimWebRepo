﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Contrato
{
    public class ParametrosContratoDTO : BaseDTO
    {
        public string MascaraClasseInsumo { get; set; }
        public byte[] IconeRelatorio { get; set; }
        public int? DiasMedicao { get; set; }
        public int? DiasPagamento { get; set; }
        public bool? DadosSped { get; set; }

    }
}