using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;

namespace GIR.Sigim.Application.DTO.Contrato
{
    public class ContratoRetificacaoDTO : BaseDTO
    {
        public int ContratoId { get; set; }
        public ContratoDTO Contrato { get; set; }
        public int Sequencial { get; set; }
        public bool Aprovada { get; set; }
        public Nullable<DateTime> DataAprovacao { get; set; }
        public string UsuarioAprovacao { get; set; }
        public string Motivo { get; set; }
        public string Observacao { get; set; }
        public string Anotacoes { get; set; }
        public string ReferenciaDigital { get; set; }
        public decimal? RetencaoContratual { get; set; }
        public int? RetencaoPrazoResgate { get; set; }
        public int? RetencaoTipoCompromissoId { get; set; }
        public TipoCompromissoDTO RetencaoTipoCompromisso { get; set; }

        public ICollection<ContratoRetificacaoItemDTO> ListaContratoRetificacaoItem { get; set; }
        public ICollection<ContratoRetificacaoProvisaoDTO> ListaContratoRetificacaoProvisao { get; set; }
        public ICollection<ContratoRetificacaoItemCronogramaDTO> ListaContratoRetificacaoItemCronograma { get; set; }


        public ContratoRetificacaoDTO()
        {
            this.ListaContratoRetificacaoItem = new HashSet<ContratoRetificacaoItemDTO>();
            this.ListaContratoRetificacaoProvisao = new HashSet<ContratoRetificacaoProvisaoDTO>();
            this.ListaContratoRetificacaoItemCronograma = new HashSet<ContratoRetificacaoItemCronogramaDTO>();
        }

    }
}
