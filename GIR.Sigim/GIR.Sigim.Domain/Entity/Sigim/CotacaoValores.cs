using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class CotacaoValores : BaseEntity
    {
        public int IndiceFinanceiroId { get; set; }
        public IndiceFinanceiro IndiceFinanceiro { get; set; }
        public int Codigo { get; set; }
        public Nullable<DateTime> Data { get; set; }
        public Nullable<Decimal> Valor { get; set; }
        public Nullable<Decimal> Variacao { get; set; }
        public String MesAno { get; set; }
        public Nullable<DateTime> DataCadastro { get; set; }
        public String UsuarioCadastro { get; set; }
        public Nullable<DateTime> DataAlteracao { get; set; }
        public String UsuarioAlteracao { get; set; }
    }
}
