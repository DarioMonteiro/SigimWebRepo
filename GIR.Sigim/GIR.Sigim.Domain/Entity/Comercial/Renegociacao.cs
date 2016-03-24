using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Comercial
{
    public class Renegociacao : BaseEntity
    {
        public int ContratoId { get; set; }
        public ContratoComercial Contrato { get; set; }
        public DateTime DataReferencia { get; set; }
        public DateTime DataRenegociacao { get; set; }
        public decimal ValorRenegociado { get; set; }
        public Nullable<DateTime> DataCancelamento { get; set; }
        public string MotivoCancelamento { get; set; }
        public DateTime DataCadastramento { get; set; }
        public string Tipo { get; set; }
        public bool? Aprovado { get; set; }
        public Nullable<DateTime> DataAprovacao { get; set; }
        public string UsuarioAprovacao { get; set; }

        public ICollection<VendaSerie> ListaVendaSerie { get; set; }

        public Renegociacao()
        {
            this.ListaVendaSerie = new HashSet<VendaSerie>();
        }
    }
}
