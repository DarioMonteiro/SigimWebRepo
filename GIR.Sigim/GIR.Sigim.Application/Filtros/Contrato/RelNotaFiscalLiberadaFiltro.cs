﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.Filtros.Contrato
{
    public class RelNotaFiscalLiberadaFiltro : BaseFiltro
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
        [Display(Name = "Contrato")]
        public int? ContratoId { get; set; }

        [RegularExpression(@"[0-9]*$", ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Fornecedor/Cliente")]
        public int? FornecedorClienteId { get; set; }
        public ClienteFornecedorDTO FornecedorCliente { get; set; }

        [Display(Name = "Documento")]
        [StringLength(10, ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        public string Documento { get; set; }

        public CentroCustoDTO CentroCusto { get; set; }

    }
}
