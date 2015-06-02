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
    public class CotacaoDTO : BaseDTO
    {
        public DateTime Data { get; set; }
        public SituacaoCotacao Situacao { get; set; }

        [Display(Name = "Situação")]
        public string SituacaoDescricao
        {
            get { return this.Situacao.ObterDescricao(); }
        }

        public string Observacao { get; set; }
        public DateTime DataCadastro { get; set; }
        public string LoginUsuarioCadastro { get; set; }
        public Nullable<DateTime> DataCancelamento { get; set; }
        public string LoginUsuarioCancelamento { get; set; }
        public string MotivoCancelamento { get; set; }
        public List<CotacaoItemDTO> ListaItens { get; set; }

        public CotacaoDTO()
        {
            this.ListaItens = new List<CotacaoItemDTO>();
        }
    }
}