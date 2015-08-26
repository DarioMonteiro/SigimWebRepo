using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GIR.Sigim.Application.Filtros.Contrato;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Presentation.WebUI.Areas.Contrato.ViewModel
{
    public class LiberacaoContratoListaViewModel
    {
        public LiberacaoContratoFiltro Filtro { get; set; }

        public LiberacaoContratoListaViewModel()
        {
            Filtro = new LiberacaoContratoFiltro();
            Filtro.Contratante = new ClienteFornecedorDTO();
            Filtro.Contratado = new ClienteFornecedorDTO();
        }
    }
}