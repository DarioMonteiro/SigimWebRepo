using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;    

namespace GIR.Sigim.Application.DTO.Sigim
{
    public class ClienteFornecedorDTO : BaseDTO
    {
        public string Nome { get; set; }
        public string TipoPessoa { get; set; }
        public string Situacao { get; set; }
        public string TipoCliente { get; set; }
        public string ClienteAPagar { get; set; }
        public string ClienteAReceber { get; set; }
        public string ClienteOrdemCompra { get; set; }
        public string ClienteContrato { get; set; }
        public string ClienteAluguel { get; set; }
        public string ClienteEmpreitada { get; set; }
        public PessoaFisicaDTO PessoaFisica { get; set; }
        public PessoaJuridicaDTO PessoaJuridica { get; set; }
    }
}