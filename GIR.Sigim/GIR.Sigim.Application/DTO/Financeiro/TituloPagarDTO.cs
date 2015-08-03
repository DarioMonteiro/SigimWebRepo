﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Contrato;

namespace GIR.Sigim.Application.DTO.Financeiro
{
    public class TituloPagarDTO : AbstractTituloDTO
    {
        public List<TituloPagarDTO> ListaFilhos { get; set; }
        public List<ImpostoPagarDTO> ListaImpostoPagar { get; set; }

        public TituloPagarDTO()
        {
            this.ListaFilhos = new List<TituloPagarDTO>();
            this.ListaImpostoPagar = new List<ImpostoPagarDTO>();
        }
    }
}