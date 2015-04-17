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
    public class RequisicaoMaterialDTO : AbstractRequisicaoMaterialDTO
    {
        public SituacaoRequisicaoMaterial Situacao { get; set; }

        [Display(Name = "Situação")]
        public string SituacaoDescricao
        {
            get { return this.Situacao.ObterDescricao(); }
        }

        public CentroCustoDTO CentroCusto { get; set; }
        public string CentroCustoDescricao
        {
            get { return this.CentroCusto.Codigo + " - " + this.CentroCusto.Descricao; }
        }

        [Display(Name = "Data aprovação")]
        public Nullable<DateTime> DataAprovacao { get; set; }

        [Display(Name = "Aprovado por")]
        public string LoginUsuarioAprovacao { get; set; }

        public List<RequisicaoMaterialItemDTO> ListaItens { get; set; }

        public RequisicaoMaterialDTO()
        {
            this.Situacao = SituacaoRequisicaoMaterial.Requisitada;
            this.Data = DateTime.Now;
            this.CentroCusto = new CentroCustoDTO();
            this.ListaItens = new List<RequisicaoMaterialItemDTO>();
        }
    }
}