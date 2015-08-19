using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Estoque;

namespace GIR.Sigim.Domain.Repository.Estoque
{
    public interface IEstoqueRepository : IRepository<Domain.Entity.Estoque.Estoque>
    {
        Domain.Entity.Estoque.Estoque ObterEstoqueAtivoPeloCentroCusto(string codigoCentroCusto, params Expression<Func<Domain.Entity.Estoque.Estoque, object>>[] includes);
        EstoqueMaterial ObterEstoqueMaterialAtivoPeloCentroCustoEMaterial(string codigoCentroCusto, int? materialId, params Expression<Func<EstoqueMaterial, object>>[] includes);
    }
}