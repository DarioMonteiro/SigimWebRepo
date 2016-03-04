using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Comercial
{
    public class TabelaVenda : BaseEntity
    {
	    public int BlocoId { get; set; }
        public Bloco Bloco { get; set; }
	    public String Nome { get; set; }
	    public String Situacao { get; set; }
        public Nullable<DateTime> DataElaboracao { get; set; }
	    public String Observacao { get; set; }
        public Decimal? PrecoReferencia { get; set; }
        public Decimal? PercentualCorretora { get; set; }
        public Decimal? PercentualCorretor { get; set; }

        public ICollection<Venda> ListaVenda { get; set; }

        public TabelaVenda()
        {
            this.ListaVenda = new HashSet<Venda>();
        }
        
    }
}
