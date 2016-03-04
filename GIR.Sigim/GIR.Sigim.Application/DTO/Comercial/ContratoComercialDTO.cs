using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.CredCob;
using GIR.Sigim.Domain.Entity.Comercial;

namespace GIR.Sigim.Application.DTO.Comercial
{
    public class ContratoComercialDTO : BaseDTO
    {
        public int UnidadeId { get; set; }
        public UnidadeDTO Unidade { get; set; }
        public string TipoContrato { get; set; }
        public string SituacaoContrato { get; set; }

        public int? VendaId { get; set; }
        public VendaDTO Venda { get; set; }

        public List<TituloCredCobDTO> ListaTituloCredCob { get; set; }
        public List<VendaParticipanteDTO> ListaVendaParticipante { get; set; }
        public List<VendaSerieDTO> ListaVendaSerie { get; set; }
        public ICollection<Renegociacao> ListaRenegociacao { get; set; }

        public ContratoComercialDTO()
        {
            this.ListaTituloCredCob = new List<TituloCredCobDTO>();
            this.ListaVendaParticipante = new List<VendaParticipanteDTO>();
            this.ListaVendaSerie = new List<VendaSerieDTO>();
            this.ListaRenegociacao = new HashSet<Renegociacao>();
        }

    }
}
