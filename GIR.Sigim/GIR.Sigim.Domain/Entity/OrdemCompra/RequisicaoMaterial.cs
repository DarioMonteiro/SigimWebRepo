using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Admin;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Domain.Entity.OrdemCompra
{
    public class RequisicaoMaterial : AbstractRequisicaoMaterial
    {
        public string CodigoCentroCusto { get; set; }
        public virtual CentroCusto CentroCusto { get; set; }
        public SituacaoRequisicaoMaterial Situacao { get; set; }
        public Nullable<DateTime> DataAprovacao { get; set; }
        //TODO: Criar relação com a classe Usuario
        public string LoginUsuarioAprovacao { get; set; }
        public ICollection<RequisicaoMaterialItem> ListaItens { get; set; }

        public RequisicaoMaterial()
        {
            this.ListaItens = new HashSet<RequisicaoMaterialItem>();
        }
    }
}