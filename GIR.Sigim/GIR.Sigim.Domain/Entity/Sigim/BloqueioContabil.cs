using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class BloqueioContabil : BaseEntity
    {
        public int? EmpreendimentoId { get; set; }
        public int? BlocoId { get; set; }
        public string CodigoCentroCusto { get; set; }
        public CentroCusto CentroCusto { get; set; }
        public DateTime Data { get; set; }
        public string Sistema { get; set; }
        public string UsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
