using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.CredCob;
using GIR.Sigim.Application.DTO.Comercial;

namespace GIR.Sigim.Application.DTO.Sigim
{
    public class IndiceFinanceiroDTO : BaseDTO
    {
        public String Indice { get; set; }
        public String Classe { get; set; }
        public Nullable<DateTime> Data { get; set; }
        public String Periodicidade { get; set; }
        public String Descricao { get; set; }
        public String FonteOrigem { get; set; }
        public String Status { get; set; }
        public bool? Dissidio { get; set; } 

        public List<TituloCredCobDTO> ListaTituloCredCobIndice { get; set; }
        public List<TituloCredCobDTO> ListaTituloCredCobIndiceAtraso { get; set; }
        public List<VendaSerieDTO> ListaVendaSerieIndiceCorrecao { get; set; }
        public List<VendaSerieDTO> ListaVendaSerieIndiceAtrasoCorrecao { get; set; }
        public List<VendaSerieDTO> ListaVendaSerieIndiceReajuste { get; set; }
        public List<CotacaoValoresDTO> ListaCotacaoValores { get; set; }

        public IndiceFinanceiroDTO()
        {
            this.ListaTituloCredCobIndice = new List<TituloCredCobDTO>();
            this.ListaTituloCredCobIndiceAtraso = new List<TituloCredCobDTO>();
            this.ListaVendaSerieIndiceCorrecao = new List<VendaSerieDTO>();
            this.ListaVendaSerieIndiceAtrasoCorrecao = new List<VendaSerieDTO>();
            this.ListaVendaSerieIndiceReajuste = new List<VendaSerieDTO>();
            this.ListaCotacaoValores = new List<CotacaoValoresDTO>();
        }

    }
}
