using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;  
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;  
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Domain.Entity.Contrato;    

namespace GIR.Sigim.Application.DTO.Contrato
{
    public class ContratoDTO : BaseDTO
    {
        public CentroCustoDTO CentroCusto { get; set; }
        public string CentroCustoDescricao
        {
            get { return this.CentroCusto.Codigo + " - " + this.CentroCusto.Descricao; } 
        }

        public int ContratoDescricaoId { get; set; }
        public LicitacaoDescricaoDTO ContratoDescricao { get; set; }      

        public int ContratanteId { get; set; }
        public ClienteFornecedorDTO Contratante { get; set; }

        public int ContratadoId { get; set; }
        public ClienteFornecedorDTO Contratado { get; set; }

        public Nullable<DateTime> DataAssinatura { get; set; }

        public SituacaoContrato Situacao { get; set; }
        public string SituacaoDescricao
        {
            get { return this.Situacao.ObterDescricao(); } 
        }

        public ICollection<ContratoRetificacaoDTO> ListaContratoRetificacao { get; set; } 

        public ContratoDTO()
        {
            this.CentroCusto = new CentroCustoDTO();
            this.Situacao = SituacaoContrato.Minuta;
            this.ListaContratoRetificacao = new HashSet<ContratoRetificacaoDTO>(); 
        }
    }
}
