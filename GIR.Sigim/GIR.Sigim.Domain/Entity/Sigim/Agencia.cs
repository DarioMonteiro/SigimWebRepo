using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class Agencia : BaseEntity
    {
        public int? BancoId { get; set; }
        public Banco Banco { get; set; }
        public string AgenciaCodigo { get; set; }
        public string DVAgencia { get; set; }
        public string Nome { get; set; }
        public string NomeContato { get; set; }
        public string TelefoneContato { get; set; }
        public string TipoLogradouro { get; set; }
        public string Logradouro { get; set; }
        public string Cidade { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public ICollection<ContaCorrente> ListaContaCorrente { get; set; }
        

        public Agencia()
        {            
            this.ListaContaCorrente = new HashSet<ContaCorrente>();
        }
    }
}