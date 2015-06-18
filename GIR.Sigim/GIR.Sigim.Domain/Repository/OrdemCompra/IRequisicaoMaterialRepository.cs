using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Orcamento;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Domain.Repository.OrdemCompra
{
    public interface IRequisicaoMaterialRepository : IRepository<RequisicaoMaterial>
    {
        void RemoverItem(RequisicaoMaterialItem item);
        void RemoverInsumoRequisitado(OrcamentoInsumoRequisitado insumoRequisitado);
    }
}