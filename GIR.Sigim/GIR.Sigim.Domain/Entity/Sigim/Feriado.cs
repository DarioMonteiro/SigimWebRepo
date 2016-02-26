using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class Feriado : BaseEntity
    {
        public Nullable<DateTime> Data { get; set; }
        public string Descricao { get; set; }
        public string UnidadeFederacaoSigla { get; set; }
        public UnidadeFederacao UnidadeFederacao { get; set; }
        public bool Ativo { get; set; }
    }
}