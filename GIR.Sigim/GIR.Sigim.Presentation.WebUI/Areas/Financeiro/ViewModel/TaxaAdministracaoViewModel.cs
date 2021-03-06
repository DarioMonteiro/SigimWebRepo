﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros;
using System.ComponentModel.DataAnnotations;

namespace GIR.Sigim.Presentation.WebUI.Areas.Financeiro.ViewModel
{
    public class TaxaAdministracaoViewModel
    {
        public CentroCustoDTO CentroCusto { get; set; }
        //public int ClienteId { get; set; }
        public ClienteFornecedorDTO Cliente { get; set; }
        public ClasseDTO Classe { get; set; }
        public decimal Percentual { get; set; }
        public string JsonItens { get; set; }
        public int IndexSelecionado { get; set; }
        public SelectList ListaCliente { get; set; }

        public bool PodeSalvar { get; set; }
        public bool PodeDeletar { get; set; }
        public bool PodeImprimir { get; set; }

        public TaxaAdministracaoViewModel()
        {
            JsonItens = "[]";
            Cliente = new ClienteFornecedorDTO();
        }

    }
}