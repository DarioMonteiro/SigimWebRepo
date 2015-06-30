using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Domain.Specification;

namespace GIR.Sigim.Infrastructure.Data.Repository.Sigim
{
    public class MaterialRepository : Repository<Material>, IMaterialRepository
    {
        #region Constructor

        public MaterialRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IMaterialRepository Members

        public IEnumerable<Material> ListarAtivosPeloTipoTabelaPropria(string descricao, params Expression<Func<Material, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);

            if (!string.IsNullOrEmpty(descricao))
                set = set.Where(l => l.Descricao.Contains(descricao));

            //TODO: Alterar no banco os materiais com situação == NULL para "A", para simplificar a consulta.
            return set.Where(l => l.TipoTabela == TipoTabela.Propria && (l.Situacao == "A" || string.IsNullOrEmpty(l.Situacao)))
                .OrderBy(l => l.Descricao);
        }

        public IEnumerable<Material> ListarPeloFiltro(ISpecification<Material> specification, params Expression<Func<Material, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);
            set = set.Where(specification.SatisfiedBy());
            return set.OrderBy(l => l.Descricao);
        }

        public override IEnumerable<Material> ListarPeloFiltroComPaginacao(
            Expression<Func<Material, bool>> filtro,
            int pageIndex,
            int pageCount,
            string orderBy,
            bool ascending,
            out int totalRecords,
            params Expression<Func<Material, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);

            set = set.Where(filtro);

            totalRecords = set.Count();

            switch (orderBy)
            {
                case "descricao":
                    set = ascending ? set.OrderBy(l => l.Descricao) : set.OrderByDescending(l => l.Descricao);
                    break;
                case "id":
                default:
                    set = ascending ? set.OrderBy(l => l.Id) : set.OrderByDescending(l => l.Id);
                    break;
            }

            return set.Skip(pageCount * pageIndex).Take(pageCount);
        }

        #endregion
    }
}