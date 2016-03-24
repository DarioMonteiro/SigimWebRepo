using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.DTO.CredCob;

namespace GIR.Sigim.Application.DTO.Financeiro
{
    public class MovimentoFinanceiroDTO : BaseDTO
    {
        public int? TipoMovimentoId { get; set; }
        public TipoMovimentoDTO TipoMovimento { get; set; }
        public DateTime DataMovimento { get; set; }
        public int? ContaCorrenteId { get; set; }
        public ContaCorrenteDTO ContaCorrente { get; set; }
        public int? CaixaId { get; set; }
        public CaixaDTO Caixa { get; set; }
        public string Referencia { get; set; }
        public string Documento { get; set; }
        public Decimal Valor { get; set; }
        public string Situacao { get; set; }
        public String UsuarioLancamento { get; set; }
        public Nullable<DateTime> DataLancamento { get; set; }
        public String UsuarioConferencia { get; set; }
        public Nullable<DateTime> DataConferencia { get; set; }
        public String UsuarioApropriacao { get; set; }
        public Nullable<DateTime> DataApropriacao { get; set; }
        public int? MovimentoPaiId { get; set; }
        public MovimentoFinanceiroDTO MovimentoPai { get; set; }
        public int? MovimentoOposto { get; set; }
        public int? BorderoTransferencia { get; set; }

        public List<MovimentoFinanceiroDTO> ListaFilhos { get; set; }
        public List<ApropriacaoDTO> ListaApropriacao { get; set; }
        public List<TituloMovimentoDTO> ListaTituloMovimento { get; set; }
        public List<TituloPagarDTO> ListaTituloPagar { get; set; }

        public MovimentoFinanceiroDTO()
        {
            this.ListaFilhos = new List<MovimentoFinanceiroDTO>();
            this.ListaApropriacao = new List<ApropriacaoDTO>();
            this.ListaTituloMovimento = new List<TituloMovimentoDTO>();
            this.ListaTituloPagar = new List<TituloPagarDTO>();
        }
    }
}
