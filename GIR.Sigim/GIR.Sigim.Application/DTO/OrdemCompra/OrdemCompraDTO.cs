﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Application.DTO.OrdemCompra
{
    public class OrdemCompraDTO : BaseDTO
    {
        public DateTime Data { get; set; }
        public SituacaoOrdemCompra Situacao { get; set; }

        [Display(Name = "Situação")]
        public string SituacaoDescricao
        {
            get { return this.Situacao.ObterDescricao(); }
        }

        public List<OrdemCompraItemDTO> ListaItens { get; set; }

        public OrdemCompraDTO()
        {
            this.ListaItens = new List<OrdemCompraItemDTO>();
        }
    }
}