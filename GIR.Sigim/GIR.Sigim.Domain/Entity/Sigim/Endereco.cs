using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class Endereco : BaseEntity
    {
        public string TipoLogradouro { get; set; }
        public string Logradouro { get; set; }
        public string Cidade { get; set; }
        public string Numero { get; set; }
        public string Complemento {get; set; }
        public string UnidadeFederacaoSigla { get; set; }
        public UnidadeFederacao UnidadeFederacao { get; set; } 
        public string Cep { get; set; }
        public string Bairro { get; set; }
        public string Telefone { get; set; }
        public string TelefoneCelular { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public int? MunicipioId { get; set; }

        public ICollection<ClienteFornecedor> ListaClienteFornecedorEndResidencial { get; set; }
        public ICollection<ClienteFornecedor> ListaClienteFornecedorEndComercial { get; set; }
        public ICollection<ClienteFornecedor> ListaClienteFornecedorEndOutro { get; set; }

        public Endereco()
        {
            this.ListaClienteFornecedorEndResidencial = new HashSet<ClienteFornecedor>();
            this.ListaClienteFornecedorEndComercial = new HashSet<ClienteFornecedor>();
            this.ListaClienteFornecedorEndOutro = new HashSet<ClienteFornecedor>();
        }

    }
}
