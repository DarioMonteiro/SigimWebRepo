﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros.Sigim;

namespace GIR.Sigim.Presentation.WebUI.ViewModel
{
    public class BancoViewModel
    {
        public BancoDTO Banco { get; set; }
        public SelectList ListaBanco { get; set; }

        public BancoFiltro Filtro { get; set; }

        public BancoViewModel()
        {
            Filtro = new BancoFiltro();
        }
        

    }
}