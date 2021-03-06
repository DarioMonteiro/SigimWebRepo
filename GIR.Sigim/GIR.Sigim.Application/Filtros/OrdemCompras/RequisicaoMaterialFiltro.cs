﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;

namespace GIR.Sigim.Application.Filtros.OrdemCompras
{
    public class RequisicaoMaterialFiltro : BaseFiltro
    {
        [DataType(DataType.Date, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "InformeDataValida")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Data inicial")]
        public Nullable<DateTime> DataInicial { get; set; }

        [DataType(DataType.Date, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "InformeDataValida")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Data final")]
        public Nullable<DateTime> DataFinal { get; set; }

        [RegularExpression(@"[0-9]*$", ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Requisição")]
        public int? Id { get; set; }

        public CentroCustoDTO CentroCusto { get; set; }

        [Display(Name = "Requisitada")]
        public bool EhRequisitada { get; set; }

        [Display(Name = "Fechada")]
        public bool EhFechada { get; set; }

        [Display(Name = "Aprovada")]
        public bool EhAprovada { get; set; }

        [Display(Name = "Cancelada")]
        public bool EhCancelada { get; set; }
    }
}