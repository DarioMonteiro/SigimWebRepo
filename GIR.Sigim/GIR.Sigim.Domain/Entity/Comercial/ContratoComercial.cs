using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.CredCob;

namespace GIR.Sigim.Domain.Entity.Comercial
{
    public class ContratoComercial : BaseEntity
    {
        public int UnidadeId { get; set; }
        public Unidade Unidade { get; set; }
        public string TipoContrato { get; set; }
        public string SituacaoContrato { get; set; }

        public int? VendaId { get; set; }
        public Venda Venda { get; set; }

        public ICollection<TituloCredCob> ListaTituloCredCob { get; set; }
        public ICollection<VendaParticipante> ListaVendaParticipante { get; set; }

        public ContratoComercial()
        {
            this.ListaTituloCredCob = new HashSet<TituloCredCob>();
            this.ListaVendaParticipante = new HashSet<VendaParticipante>();
        }
    }
}
