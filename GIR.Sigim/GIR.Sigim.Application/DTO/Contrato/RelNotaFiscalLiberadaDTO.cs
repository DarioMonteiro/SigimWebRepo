using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.DTO.Contrato
{
    public class RelNotaFiscalLiberadaDTO : BaseDTO
    {
        public int ContratoId { get; set; }
        public ContratoDTO Contrato { get; set; }
        public int ContratoRetificacaoItemId { get; set; }
        public ContratoRetificacaoItemDTO ContratoRetificacaoItem { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime DataEmissao { get; set; }
        public DateTime DataVencimento { get; set; }
        public int? FornecedorClienteId { get; set; }
        public ClienteFornecedorDTO FornecedorCliente { get; set; }
        public int? TituloId { get; set; }
        public decimal Valor { get; set; }
        public string UsuarioLiberacao { get; set; }
        public decimal? ValorRetido { get; set; }
        public decimal ValorClasse { get; set; }

    }
}
