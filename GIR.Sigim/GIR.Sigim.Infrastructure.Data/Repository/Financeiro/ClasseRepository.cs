using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Repository.Financeiro;

namespace GIR.Sigim.Infrastructure.Data.Repository.Financeiro
{
    public class ClasseRepository : Repository<Classe>, IClasseRepository
    {
        #region Constructor

        public ClasseRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IClasseRepository Members

        public IEnumerable<Classe> ObterPeloCodigoEOrcamento(string codigo, int orcamentoId, params Expression<Func<Classe, object>>[] includes)
        {
            var set = QueryableUnitOfWork.CreateSet<Classe>().AsQueryable<Classe>();

            if (includes.Any())
                set = includes.Aggregate(set, (current, expression) => current.Include(expression));

            if (orcamentoId > 0)
                set = set.Where(l => l.ListaOrcamentoComposicao.Any(s => s.OrcamentoId == orcamentoId));

            //return set.Where(l => l.Codigo == codigo).SingleOrDefault();
            return set.Where(l => l.Codigo.StartsWith(codigo));
        }

        //public Classe ObterPeloCodigoEOrcamento(string codigo, int orcamentoId, params Expression<Func<Classe, object>>[] includes)
        //{
        //    var set = QueryableUnitOfWork.CreateSet<Classe>().AsQueryable<Classe>();

        //    if (includes.Any())
        //        set = includes.Aggregate(set, (current, expression) => current.Include(expression));

        //    if (orcamentoId > 0)
        //        set = set.Where(l => l.ListaOrcamentoComposicao.Any(s => s.OrcamentoId == orcamentoId));

        //    return set.Where(l => l.Codigo == codigo).SingleOrDefault();
        //}

        public IEnumerable<Classe> ListarRaizes()
        {
            return QueryableUnitOfWork.CreateSet<Classe>()
                .Where(l => l.ClassePai == null);
        }

        public Classe ObterPeloCodigo(string codigo, params Expression<Func<Classe, object>>[] includes)
        {
            var set = QueryableUnitOfWork.CreateSet<Classe>().AsQueryable<Classe>();

            if (includes.Any())
                set = includes.Aggregate(set, (current, expression) => current.Include(expression));

            return set.Where(l => l.Codigo == codigo).SingleOrDefault();
        }

        #endregion
    }
}