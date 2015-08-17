using System;
using System.Collections.Generic;
using System.Linq;
using GIR.Sigim.Application.DTO.Financeiro;
using System.ComponentModel.DataAnnotations;

namespace GIR.Sigim.Presentation.WebUI.Areas.Financeiro.ViewModel
{
    public class ParametrosUsuarioFinanceiroViewModel
    {
        public ParametrosUsuarioFinanceiroDTO ParametrosUsuarioFinanceiro { get; set; }
        public bool PodeSalvar { get; set; }
    }
}