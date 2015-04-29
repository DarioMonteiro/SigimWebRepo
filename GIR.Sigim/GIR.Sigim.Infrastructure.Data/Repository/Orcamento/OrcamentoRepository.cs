using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Repository.Orcamento;

namespace GIR.Sigim.Infrastructure.Data.Repository.Orcamento
{
    public class OrcamentoRepository : Repository<Domain.Entity.Orcamento.Orcamento>, IOrcamentoRepository
    {
        #region Constructor

        public OrcamentoRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IOrcamentoRepository Members

        public Domain.Entity.Orcamento.Orcamento ObterUltimoOrcamentoPeloCentroCusto(string codigoCentroCusto)
        {
            var set = CreateSetAsQueryable();

            set = set.Where(l => l.Obra.CentroCusto.Codigo == codigoCentroCusto
                && l.Situacao == "A" 
                && l.Obra.CentroCusto.ListaCentroCustoEmpresa.Any(s => s.EhClasseOrcamento.Value));

            set = set.OrderByDescending(l => l.Sequencial);
            return set.FirstOrDefault();
            //return set.Aggregate((o1, o2) => o1.Sequencial > o2.Sequencial ? o1 : o2);
        }

        #endregion
    }
}