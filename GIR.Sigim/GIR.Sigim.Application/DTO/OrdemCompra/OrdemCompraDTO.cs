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
using GIR.Sigim.Application.Filtros;

namespace GIR.Sigim.Application.DTO.OrdemCompra
{
    public class OrdemCompraDTO : BaseDTO
    {
        public string CodigoCentroCusto { get; set; }
        public CentroCustoDTO CentroCusto { get; set; }
        public string DescricaoCentroCusto
        {
            get
            {
                return CentroCusto != null ? CentroCusto.Codigo + " - " + CentroCusto.Descricao : "";
            }
        }
        public int ClienteFornecedorId { get; set; }
        public ClienteFornecedorDTO ClienteFornecedor { get; set; }
        public DateTime Data { get; set; }
        public SituacaoOrdemCompra Situacao { get; set; }
        [Display(Name = "Situação")]
        public string SituacaoDescricao
        {
            get { return this.Situacao.ObterDescricao(); }
        }
        public int? PrazoEntrega { get; set; }
        public List<OrdemCompraItemDTO> ListaItens { get; set; }
        public PaginationParameters PaginationParameters { get; set; }
        public decimal? ValorFrete { get; set; }
        public decimal? ValorTotalOC { get; set; }

        public OrdemCompraDTO()
        {
            this.ListaItens = new List<OrdemCompraItemDTO>();
            PaginationParameters = new PaginationParameters();
            PaginationParameters.UniqueIdentifier = "_" + Guid.NewGuid().ToString().Replace("-", string.Empty);
        }
    }
}