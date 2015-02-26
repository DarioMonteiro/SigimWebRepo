using System;
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
    public class PreRequisicaoMaterialItemDTO : AbstractRequisicaoMaterialItemDTO
    {
        public CentroCustoDTO CentroCusto { get; set; }
        public SituacaoPreRequisicaoMaterialItem Situacao { get; set; }

        [Display(Name = "Situação")]
        public string SituacaoDescricao
        {
            get { return this.Situacao.ObterDescricao(); }
        }

        public List<RequisicaoMaterialItemDTO> ListaRequisicaoMaterialItem { get; set; }

        //public int? RMgerada
        //{
        //    get
        //    {
        //        if (ListaRequisicaoMaterialItem.Any())
        //            return ListaRequisicaoMaterialItem[0].RequisicaoMaterial.Id;

        //        return null;
        //    }
        //}

        public PreRequisicaoMaterialItemDTO()
        {
            this.CentroCusto = new CentroCustoDTO();
            this.ListaRequisicaoMaterialItem = new List<RequisicaoMaterialItemDTO>();
        }
    }
}