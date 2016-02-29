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
        public ICollection<Feriado> ListaFeriado { get; set; }
        public ICollection<Endereco> ListaEndereco { get; set; }

        public UnidadeFederacao()
        {
            this.ListaAgencia = new HashSet<Agencia>();
            this.ListaFeriado = new HashSet<Feriado>();
            this.ListaEndereco = new HashSet<Endereco>();
        }

    }
}
