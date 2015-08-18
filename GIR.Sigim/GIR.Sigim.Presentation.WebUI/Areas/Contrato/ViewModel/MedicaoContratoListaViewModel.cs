using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.Filtros.Contrato;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Presentation.WebUI.Areas.Contrato.ViewModel
{
    public class MedicaoContratoListaViewModel 
    {
        public MedicaoContratoFiltro Filtro { get; set; }

        public MedicaoContratoListaViewModel()
        {
            Filtro = new MedicaoContratoFiltro();
            Filtro.Contratante = new ClienteFornecedorDTO();
            Filtro.Contratado = new ClienteFornecedorDTO();

        }
    }
}
