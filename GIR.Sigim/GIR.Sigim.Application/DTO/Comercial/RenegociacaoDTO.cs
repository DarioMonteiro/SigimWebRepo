using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Comercial
{
    public class RenegociacaoDTO : BaseDTO
    {
        public int ContratoId { get; set; }
        public ContratoComercialDTO Contrato { get; set; }
        public DateTime DataReferencia { get; set; }
        public DateTime DataRenegociacao { get; set; }
        public decimal ValorRenegociado { get; set; }
        public Nullable<DateTime> DataCancelamento { get; set; }
        public string MotivoCancelamento { get; set; }
        public DateTime DataCadastramento { get; set; }
        public string Tipo { get; set; }
        public bool Aprovado { get; set; }
        public Nullable<DateTime> DataAprovacao { get; set; }
        public string UsuarioAprovacao { get; set; }

        public List<VendaSerieDTO> ListaVendaSerie { get; set; }

        public RenegociacaoDTO()
        {
            this.ListaVendaSerie = new List<VendaSerieDTO>();
        }

    }
}
