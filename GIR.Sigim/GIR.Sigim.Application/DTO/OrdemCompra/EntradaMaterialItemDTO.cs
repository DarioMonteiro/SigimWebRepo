using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Application.DTO.OrdemCompra
{
    public class EntradaMaterialItemDTO : BaseDTO
    {
        public int? EntradaMaterialId { get; set; }
        public OrdemCompraItemDTO OrdemCompraItem { get; set; }
        public ClasseDTO Classe { get; set; }
        public int Sequencial { get; set; }
        public decimal Quantidade { get; set; }
        public decimal QuantidadeInicial { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal PercentualIPI { get; set; }
        public decimal PercentualDesconto { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal BaseICMS { get; set; }
        public decimal PercentualICMS { get; set; }
        public decimal BaseIPI { get; set; }
        public decimal BaseICMSST { get; set; }
        public decimal PercentualICMSST { get; set; }
        public string CodigoComplementoNaturezaOperacao { get; set; }
        public ComplementoNaturezaOperacaoDTO ComplementoNaturezaOperacao { get; set; }
        public string CodigoComplementoCST { get; set; }
        public ComplementoCSTDTO ComplementoCST { get; set; }
        public string CodigoNaturezaReceita { get; set; }
        public NaturezaReceitaDTO NaturezaReceita { get; set; }

        public EntradaMaterialItemDTO()
        {
            this.OrdemCompraItem = new OrdemCompraItemDTO();
            this.Classe = new ClasseDTO();
        }
    }
}