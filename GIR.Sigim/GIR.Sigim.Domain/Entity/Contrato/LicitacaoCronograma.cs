using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Domain.Entity.Contrato
{
    public class LicitacaoCronograma : BaseEntity 
    {
        public string CodigoCentroCusto { get; set; }
        public virtual CentroCusto CentroCusto { get; set; }
        public LicitacaoDescricao LicitacaoDescricao { get; set; }
        public DateTime DataInicioCartaConvite { get; set; }
        public DateTime DataFimCartaConvite { get; set; }
        public DateTime DataInicioQuadroComparativo { get; set; }
        public DateTime DataFimQuadroComparativo { get; set; }
        public DateTime DataInicioAssinatura { get; set; }
        public DateTime DataFimAssinatura { get; set; }
        public int? PrazoFabricacao { get; set; }
        public int DuracaoCartaConvite { get; set; }
        public int DuracaoQuadroComparativo { get; set; }
        public int DuracaoAssinatura { get; set; }
        public Nullable<DateTime> DataInicioCartaConviteRealizado { get; set; }
        public Nullable<DateTime> DataFimCartaConviteRealizado { get; set; }
        public Nullable<DateTime> DataInicioQuadroComprativoRealizado { get; set; }
        public Nullable<DateTime> DataFimQuadroComparativoRealizado { get; set; }
        public Nullable<DateTime> DataInicioAssinaturaRealizado { get; set; }
        public Nullable<DateTime> DataFimAssinaturaRealizado { get; set; }
        public Nullable<DateTime> DataInicioServicoRealizado { get; set; }

        public ICollection<Licitacao> ListaLicitacao { get; set; }

        public LicitacaoCronograma()
        {
            this.ListaLicitacao = new HashSet<Licitacao>();
        }

     }
}
