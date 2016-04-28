using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Repository.Sigim;

namespace GIR.Sigim.Infrastructure.Data.Repository.Sigim
{
    public class CotacaoValoresRepository : Repository<CotacaoValores>, ICotacaoValoresRepository
    {
        #region Contructor

        public CotacaoValoresRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IRepository<OrdemCompra> Métodos

        public decimal RecuperaCotacao(int indiceId, DateTime data)
        {
            decimal valor = 0;
            List<CotacaoValores> listaCotacao = this.ListarPeloFiltro(l => l.IndiceFinanceiroId == indiceId && l.Data.Value <= data).ToList<CotacaoValores>();
            if (listaCotacao.Count > 0)
            {
                Nullable<DateTime> ultimaData;

                ultimaData = listaCotacao.Select(l => l.Data.Value).Max();
                if (ultimaData.HasValue)
                {
                    valor = this.ListarPeloFiltro(l => l.IndiceFinanceiroId == indiceId && l.Data.Value == ultimaData.Value).FirstOrDefault().Valor.Value;
                }
            }

            return valor;
        }

        #endregion
    }
}
