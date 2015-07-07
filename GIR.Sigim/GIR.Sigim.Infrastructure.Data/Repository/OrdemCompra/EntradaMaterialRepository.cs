using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Repository.OrdemCompra;
using GIR.Sigim.Domain.Specification;

namespace GIR.Sigim.Infrastructure.Data.Repository.OrdemCompra
{
    public class EntradaMaterialRepository : Repository<EntradaMaterial>, IEntradaMaterialRepository
    {
        #region Constructor

        public EntradaMaterialRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        public override IEnumerable<EntradaMaterial> ListarPeloFiltroComPaginacao(
            ISpecification<EntradaMaterial> specification,
            int pageIndex,
            int pageCount,
            string orderBy,
            bool ascending,
            out int totalRecords,
            params Expression<Func<EntradaMaterial, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);

            set = set.Where(specification.SatisfiedBy());

            totalRecords = set.Count();

            switch (orderBy)
            {
                case "situacao":
                    set = ascending ? set.OrderBy(l => l.Situacao) : set.OrderByDescending(l => l.Situacao);
                    break;
                case "data":
                    set = ascending ? set.OrderBy(l => l.Data) : set.OrderByDescending(l => l.Data);
                    break;
                case "centroCusto":
                    set = ascending ? set.OrderBy(l => l.CodigoCentroCusto) : set.OrderByDescending(l => l.CodigoCentroCusto);
                    break;
                case "notaFiscal":
                    set = ascending ? set.OrderBy(l => l.NumeroNotaFiscal) : set.OrderByDescending(l => l.NumeroNotaFiscal);
                    break;
                case "observacao":
                    set = ascending ? set.OrderBy(l => l.Observacao) : set.OrderByDescending(l => l.Observacao);
                    break;
                //case "fornecedor":
                //    fornecedorSortingSuffix += sortingSuffix;
                //    break;
                case "motivoCancelamento":
                    set = ascending ? set.OrderBy(l => l.MotivoCancelamento) : set.OrderByDescending(l => l.MotivoCancelamento);
                    break;
                case "responsavelCancelamento":
                    set = ascending ? set.OrderBy(l => l.LoginUsuarioCancelamento) : set.OrderByDescending(l => l.LoginUsuarioCancelamento);
                    break;
                case "responsavelLiberacao":
                    set = ascending ? set.OrderBy(l => l.LoginUsuarioLiberacao) : set.OrderByDescending(l => l.LoginUsuarioLiberacao);
                    break;
                case "responsavelConferencia":
                    set = ascending ? set.OrderBy(l => l.LoginUsuarioConferencia) : set.OrderByDescending(l => l.LoginUsuarioConferencia);
                    break;
                case "dataConferencia":
                    set = ascending ? set.OrderBy(l => l.DataConferencia) : set.OrderByDescending(l => l.DataConferencia);
                    break;
                case "id":
                default:
                    set = ascending ? set.OrderBy(l => l.Id) : set.OrderByDescending(l => l.Id);
                    break;
            }

            return set.Skip(pageCount * pageIndex).Take(pageCount);
        }
    }
}