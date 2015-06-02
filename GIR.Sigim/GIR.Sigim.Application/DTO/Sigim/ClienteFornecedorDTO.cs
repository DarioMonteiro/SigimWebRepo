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

        public string TipoPessoa;

        public string Situacao;

        public string TipoCliente;

        public string ClienteAPagar;

        public string ClienteAReceber;

        public string ClienteOrdemCompra;

        public string ClienteContrato;

        public string ClienteAluguel;

        public string ClienteEmpreitada;

        public PessoaFisicaDTO PessoaFisica;

        public PessoaJuridicaDTO PessoaJuridica; 
        
    }
}