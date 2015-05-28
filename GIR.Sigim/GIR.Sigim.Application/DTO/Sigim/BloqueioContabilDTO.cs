using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;

namespace GIR.Sigim.Application.DTO.Sigim
{
    public class BloqueioContabilDTO : BaseDTO
    {
        public int? EmpreendimentoId { get; set; }
        public int? BlocoId { get; set; }
        public string CodigoCentroCusto { get; set; }
        public CentroCustoDTO CentroCusto { get; set; }
        public DateTime Data { get; set; }
        public string Sistema { get; set; }
        public string UsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
