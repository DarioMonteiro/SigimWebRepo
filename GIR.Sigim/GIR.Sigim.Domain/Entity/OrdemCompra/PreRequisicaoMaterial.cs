using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Admin;

namespace GIR.Sigim.Domain.Entity.OrdemCompra
{
    public class PreRequisicaoMaterial : AbstractRequisicaoMaterial
    {
        public SituacaoPreRequisicaoMaterial Situacao { get; set; }
        public ICollection<PreRequisicaoMaterialItem> ListaItens { get; set; }

        public PreRequisicaoMaterial()
        {
            this.Situacao = SituacaoPreRequisicaoMaterial.Requisitada;
            this.ListaItens = new HashSet<PreRequisicaoMaterialItem>();
        }
    }
}