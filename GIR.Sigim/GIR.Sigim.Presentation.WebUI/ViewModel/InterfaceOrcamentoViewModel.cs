using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.Orcamento;
using GIR.Sigim.Application.Filtros.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.ViewModel;

namespace GIR.Sigim.Presentation.WebUI.ViewModel
{
    public class InterfaceOrcamentoViewModel
    {
        public bool ExibirTelaInterfaceOrcamento { get; set; }
        public List<OrcamentoComposicaoItemDTO> ListaItens { get; set; }
        public List<Message> ErrorMessages { get; set; }

        public InterfaceOrcamentoViewModel()
        {
            ListaItens = new List<OrcamentoComposicaoItemDTO>();
        }
    }
}