using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Application.DTO.OrdemCompra
{
    public class RequisicaoMaterialItemDTO : AbstractRequisicaoMaterialItemDTO
    {
        public int? RequisicaoMaterialId { get; set; }
        public int? PreRequisicaoMaterialItemId { get; set; }
        public SituacaoRequisicaoMaterialItem Situacao { get; set; }

        [Display(Name = "Situação")]
        public string SituacaoDescricao
        {
            get { return this.Situacao.ObterDescricao(); }
        }
    }
}