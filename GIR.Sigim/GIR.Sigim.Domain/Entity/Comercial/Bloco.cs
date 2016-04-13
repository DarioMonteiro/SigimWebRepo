using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Domain.Entity.Comercial
{
    public class Bloco : BaseEntity
    {
        public int? EmpreendimentoId { get; set; }
        public Empreendimento Empreendimento { get; set; }
        public string Nome { get; set; }
        public string CodigoCentroCusto { get; set; }
        public CentroCusto CentroCusto { get; set; }

        public ICollection<Unidade> ListaUnidade { get; set; }
        public ICollection<TabelaVenda> ListaTabelaVenda { get; set; }
        public ICollection<IncorporadorAssociado> ListaIncorporadorAssociado { get; set; }


        public Bloco()
        {
            this.ListaUnidade = new HashSet<Unidade>();
            this.ListaTabelaVenda = new HashSet<TabelaVenda>();
            this.ListaIncorporadorAssociado = new HashSet<IncorporadorAssociado>();
        }

    }
}
