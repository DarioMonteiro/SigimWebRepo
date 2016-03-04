using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.CredCob;
using GIR.Sigim.Domain.Constantes;

namespace GIR.Sigim.Application.DTO.Comercial
{
    public class ContratoComercialDTO : BaseDTO
    {
        public int UnidadeId { get; set; }
        public UnidadeDTO Unidade { get; set; }
        public string TipoContrato { get; set; }
        public string SituacaoContrato { get; set; }

        public string DescricaoSituacaoContrato
        {
            get
            {
                switch (SituacaoContrato)
                {
                    case GIR.Sigim.Domain.Constantes.Comercial.ContratoSituacaoPropostaCodigo: return GIR.Sigim.Domain.Constantes.Comercial.ContratoSituacaoPropostaDescricao;
                    case GIR.Sigim.Domain.Constantes.Comercial.ContratoSituacaoAssinadoCodigo: return GIR.Sigim.Domain.Constantes.Comercial.ContratoSituacaoAssinadoDescricao;
                    case GIR.Sigim.Domain.Constantes.Comercial.ContratoSituacaoCanceladoCodigo: return GIR.Sigim.Domain.Constantes.Comercial.ContratoSituacaoCanceladoDescricao;
                    case GIR.Sigim.Domain.Constantes.Comercial.ContratoSituacaoRescindidoCodigo: return GIR.Sigim.Domain.Constantes.Comercial.ContratoSituacaoRescindidoDescricao;
                    case GIR.Sigim.Domain.Constantes.Comercial.ContratoSituacaoQuitadoCodigo: return GIR.Sigim.Domain.Constantes.Comercial.ContratoSituacaoQuitadoDescricao;
                    case GIR.Sigim.Domain.Constantes.Comercial.ContratoSituacaoEscrituradoCodigo: return GIR.Sigim.Domain.Constantes.Comercial.ContratoSituacaoEscrituradoDescricao;
                    default: return "?????";
                }

            }
        }
        
        public int? VendaId { get; set; }
        public VendaDTO Venda { get; set; }

        public List<TituloCredCobDTO> ListaTituloCredCob { get; set; }
        public List<VendaParticipanteDTO> ListaVendaParticipante { get; set; }
        public List<VendaSerieDTO> ListaVendaSerie { get; set; }

        public ContratoComercialDTO()
        {
            this.ListaTituloCredCob = new List<TituloCredCobDTO>();
            this.ListaVendaParticipante = new List<VendaParticipanteDTO>();
            this.ListaVendaSerie = new List<VendaSerieDTO>();
        }

    }
}
