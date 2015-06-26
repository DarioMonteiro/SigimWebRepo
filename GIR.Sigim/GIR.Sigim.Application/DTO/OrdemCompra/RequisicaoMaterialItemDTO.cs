using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Orcamento;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Application.DTO.OrdemCompra
{
    public class RequisicaoMaterialItemDTO : AbstractRequisicaoMaterialItemDTO
    {
        public int? RequisicaoMaterialId { get; set; }
        public int? PreRequisicaoMaterialItemId { get; set; }
        public SituacaoRequisicaoMaterialItem Situacao { get; set; }
        public int? UltimaCotacao { get; set; }
        public int? UltimaOrdemCompra { get; set; }
        public OrcamentoInsumoRequisitadoDTO OrcamentoInsumoRequisitado { get; set; }
        public string TemInterfaceOrcamentoDescricao
        {
            get { return (OrcamentoInsumoRequisitado != null) ? "Sim" : "Não"; }
        }
        public List<CotacaoItemDTO> ListaCotacaoItem { get; set; }
        public List<OrdemCompraItemDTO> ListaOrdemCompraItem { get; set; }

        [Display(Name = "Situação")]
        public string SituacaoDescricao
        {
            get { return this.Situacao.ObterDescricao(); }
        }

        public RequisicaoMaterialItemDTO()
        {
            this.ListaCotacaoItem = new List<CotacaoItemDTO>();
            this.ListaOrdemCompraItem = new List<OrdemCompraItemDTO>();
        }
    }
}