using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Admin;
using GIR.Sigim.Domain.Repository.Admin;
using GIR.Sigim.Domain.Specification;

namespace GIR.Sigim.Infrastructure.Data.Repository.Admin
{
    public class UsuarioFuncionalidadeRepository : Repository<UsuarioFuncionalidade>, IUsuarioFuncionalidadeRepository
    {
        #region Constructor

        public UsuarioFuncionalidadeRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IUsuarioFuncionalidadeRepository Members

        public override IEnumerable<UsuarioFuncionalidade> ListarPeloFiltroComPaginacao(
            ISpecification<UsuarioFuncionalidade> specification,
            int pageIndex,
            int pageCount,
            string orderBy,
            bool ascending,
            out int totalRecords,
            params Expression<Func<UsuarioFuncionalidade, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);
            var retorno = new List<UsuarioFuncionalidade>();

            set = set.Where(specification.SatisfiedBy());

            totalRecords = set.Count();

            switch (orderBy)
            {
                case "id":
                    set = ascending ? set.OrderBy(l => l.Id) : set.OrderByDescending(l => l.Id);
                    break;
                case "usuario":
                    set = ascending ? set.OrderBy(l => l.Usuario.Nome) : set.OrderByDescending(l => l.Usuario.Nome);
                    break;
                case "modulo":
                    set = ascending ? set.OrderBy(l => l.Modulo.Nome) : set.OrderByDescending(l => l.Modulo.Nome);
                    break;
                default:
                    set = ascending ? set.OrderBy(l => l.Id) : set.OrderByDescending(l => l.Id);
                    break;
            }

            retorno = set.ToList<UsuarioFuncionalidade>();

            string texto;
            var vetTeste = new System.Collections.Hashtable();
            foreach (var item in Enumerable.Reverse(retorno))
            {
                texto = item.Usuario.Id  + "|" + item.ModuloId;

                if (vetTeste.ContainsKey(texto) == true)
                {
                    retorno.Remove(item);
                }
                else
                {
                    vetTeste.Add(texto, texto);
                }
            }

            //return set.Skip(pageCount * pageIndex).Take(pageCount);

            return retorno.Skip(pageCount * pageIndex).Take(pageCount);

        }



        #endregion
    }
}