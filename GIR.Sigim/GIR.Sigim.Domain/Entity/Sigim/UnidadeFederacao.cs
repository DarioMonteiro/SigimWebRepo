using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class UnidadeFederacao : BaseEntity
    {
        public string Sigla { get; set; }
        public string NomeUnidadeFederacao { get; set; }
        public int? CodigoIBGE { get; set; }

        public ICollection<Agencia> ListaAgencia { get; set; }

        public UnidadeFederacao()
        {
            this.ListaAgencia = new HashSet<Agencia>();
        }

    }
}
