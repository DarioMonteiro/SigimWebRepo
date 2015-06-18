using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Domain.Entity.Contrato;

namespace GIR.Sigim.Application.DTO.Contrato
{
    public class LicitacaoDTO : BaseDTO
    {
        public string CodigoCentroCusto { get; set; }
        public virtual CentroCustoDTO CentroCusto { get; set; }
        public int LicitacaoCronogramaId { get; set; }
        public LicitacaoCronogramaDTO LicitacaoCronograma { get; set; }
        public DateTime DataLicitacao { get; set; }
        public SituacaoLicitacao Situacao { get; set; }
        public string Observacao { get; set; }
        public int? ClienteFornecedorId { get; set; }
        public ClienteFornecedorDTO ClienteFornecedor { get; set; }
        public Nullable<DateTime> DataLimiteEmail { get; set; }
        public string ReferenciaDigital { get; set; }
        public DateTime DataCadastro { get; set; }
        public string UsuarioCadastro { get; set; }
        public Nullable<DateTime> DataCancela { get; set; }
        public string UsuarioCancela { get; set; }
        public string MotivoCancela { get; set; }

        public List<ContratoDTO> ListaContrato { get; set; }

        public LicitacaoDTO()
        {
            this.ListaContrato = new List<ContratoDTO>();
        }

    }
}
