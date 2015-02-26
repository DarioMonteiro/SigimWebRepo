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
    public class PreRequisicaoMaterialDTO : AbstractRequisicaoMaterialDTO
    {
        public SituacaoPreRequisicaoMaterial Situacao { get; set; }

        [Display(Name = "Situação")]
        public string SituacaoDescricao
        {
            get { return this.Situacao.ObterDescricao(); }
        }

        public string RMGeradas { get; set; }

        public List<PreRequisicaoMaterialItemDTO> ListaItens { get; set; }

        public PreRequisicaoMaterialDTO()
        {
            this.Situacao = SituacaoPreRequisicaoMaterial.Requisitada;
            this.Data = DateTime.Now;
            this.ListaItens = new List<PreRequisicaoMaterialItemDTO>();
        }
    }
}