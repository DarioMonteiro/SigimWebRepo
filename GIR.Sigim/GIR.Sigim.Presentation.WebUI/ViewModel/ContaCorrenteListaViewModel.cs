﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros.Sigim;
using GIR.Sigim.Presentation.WebUI.ViewModel;

namespace GIR.Sigim.Presentation.WebUI.ViewModel
{
    public class ContaCorrenteListaViewModel
    {
        public ContaCorrenteDTO ContaCorrente { get; set; }
        public ContaCorrenteFiltro Filtro { get; set; }
        public SelectList ListaBanco { get; set; }

        public ContaCorrenteListaViewModel()
        {
            Filtro = new ContaCorrenteFiltro();
        }
    }

}