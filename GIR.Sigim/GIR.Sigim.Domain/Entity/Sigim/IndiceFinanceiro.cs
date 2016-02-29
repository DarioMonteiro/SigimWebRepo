using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.CredCob;
using GIR.Sigim.Domain.Entity.Comercial;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class IndiceFinanceiro : BaseEntity
    {
        public String Indice { get; set; }
        public String Classe { get; set; }
        public Nullable<DateTime> Data { get; set; }
        public String Periodicidade { get; set; }
        public String Descricao { get; set; }
        public String FonteOrigem { get; set; }
        public String Status { get; set; }
        public bool? Dissidio { get; set; } 

        public ICollection<TituloCredCob> ListaTituloCredCobIndice { get; set; }
        public ICollection<TituloCredCob> ListaTituloCredCobIndiceAtraso { get; set; }
        //public ICollection<VendaSerie> ListaVendaSerieIndiceCorrecao { get; set; }
        //public ICollection<VendaSerie> ListaVendaSerieIndiceAtrasoCorrecao { get; set; }
        //public ICollection<VendaSerie> ListaVendaSerieIndiceReajuste { get; set; }
        public ICollection<CotacaoValores> ListaCotacaoValores { get; set; }

        public IndiceFinanceiro()
        {
            this.ListaTituloCredCobIndice = new HashSet<TituloCredCob>();
            this.ListaTituloCredCobIndiceAtraso = new HashSet<TituloCredCob>();
            //this.ListaVendaSerieIndiceCorrecao = new HashSet<VendaSerie>();
            //this.ListaVendaSerieIndiceAtrasoCorrecao = new HashSet<VendaSerie>();
            //this.ListaVendaSerieIndiceReajuste = new HashSet<VendaSerie>();
            this.ListaCotacaoValores = new HashSet<CotacaoValores>();
        }
    }
}
