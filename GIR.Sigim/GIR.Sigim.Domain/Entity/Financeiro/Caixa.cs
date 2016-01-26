using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Entity.Contrato; 

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class Caixa : BaseEntity
    {
        public string Descricao { get; set; }
        public string Situacao { get; set; }
        public string CentroContabil { get; set; }
        public string CodigoCentroCusto { get; set; }
        public CentroCusto CentroCusto { get; set; }

        public ICollection<MovimentoFinanceiro> ListaMovimentoFinanceiro { get; set; }

        public Caixa()
        {
            this.ListaMovimentoFinanceiro = new HashSet<MovimentoFinanceiro>();
        }

    }
}