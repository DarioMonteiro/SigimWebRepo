using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Comercial
{
    public class Unidade : BaseEntity
    {
        public string Descricao { get; set; }
        public int? EmpreendimentoId { get; set; }
        public Empreendimento Empreendimento { get; set; }
        public int? BlocoId { get; set; }
        public Bloco Bloco { get; set; }

        public Nullable<Decimal> MultaPorAtraso { get; set; } 
        public bool? ConsiderarParametroUnidade { get; set; }

        public ICollection<ContratoComercial> ListaContratoComercial { get; set; }

        public Unidade()
        {
            this.ListaContratoComercial = new HashSet<ContratoComercial>();
        }
    }
}
