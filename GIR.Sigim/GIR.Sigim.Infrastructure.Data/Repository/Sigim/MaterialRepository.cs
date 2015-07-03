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
using GIR.Sigim.Domain.Specification.Sigim;

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

        public IEnumerable<Material> Pesquisar(
            ISpecification<Material> specification,
            string orderBy,
            bool ascending,
            params Expression<Func<Material, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);
            set = set.Where(specification.SatisfiedBy());
            set = ConfigurarOrdenacao(set, orderBy, ascending);

            return set.ToList();
        }

        #endregion

        private static IQueryable<Material> ConfigurarOrdenacao(IQueryable<Material> set, string orderBy, bool ascending)
        {
            switch (orderBy)
            {
                case "unidadeMedida":
                    set = ascending ? set.OrderBy(l => l.SiglaUnidadeMedida) : set.OrderByDescending(l => l.SiglaUnidadeMedida);
                    break;
                case "id":
                    set = ascending ? set.OrderBy(l => l.Id) : set.OrderByDescending(l => l.Id);
                    break;
                case "classeInsumo":
                    set = ascending ? set.OrderBy(l => l.CodigoMaterialClasseInsumo) : set.OrderByDescending(l => l.MaterialClasseInsumo);
                    break;
                case "precoUnitario":
                    set = ascending ? set.OrderBy(l => l.PrecoUnitario) : set.OrderByDescending(l => l.PrecoUnitario);
                    break;
                case "tipoTabela":
                    set = ascending ? set.OrderBy(l => l.TipoTabela) : set.OrderByDescending(l => l.TipoTabela);
                    break;
                case "mesAno":
                    set = ascending ? set.OrderBy(l => l.AnoMes) : set.OrderByDescending(l => l.AnoMes);
                    break;
                case "codigoExterno":
                    set = ascending ? set.OrderBy(l => l.CodigoExterno) : set.OrderByDescending(l => l.CodigoExterno);
                    break;
                case "descricao":
                default:
                    set = ascending ? set.OrderBy(l => l.Descricao) : set.OrderByDescending(l => l.Descricao);
                    break;
            }
            return set;
        }
    }
}