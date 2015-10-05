using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Repository.Financeiro;

namespace GIR.Sigim.Infrastructure.Data.Repository.Financeiro
{
    public class TituloPagarRepository : Repository<TituloPagar>, ITituloPagarRepository
    {

        #region Construtor

        public TituloPagarRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }
        #endregion

        #region ITituloPagarRepository Members

        public void RemoverTituloPagarAdiantamento(TituloPagarAdiantamento titulo)
        {
            if (titulo != (TituloPagarAdiantamento)null)
            {
                QueryableUnitOfWork.Attach(titulo);
                QueryableUnitOfWork.CreateSet<TituloPagarAdiantamento>().Remove(titulo);
            }
        }

        public void RemoverApropriacaoAdiantamento(ApropriacaoAdiantamento apropriacao)
        {
            if (apropriacao != (ApropriacaoAdiantamento)null)
            {
                QueryableUnitOfWork.Attach(apropriacao);
                QueryableUnitOfWork.CreateSet<ApropriacaoAdiantamento>().Remove(apropriacao);
            }
        }

        public void RemoverImpostoPagar(ImpostoPagar impostoPagar)
        {
            if (impostoPagar != (ImpostoPagar)null)
            {
                QueryableUnitOfWork.Attach(impostoPagar);
                QueryableUnitOfWork.CreateSet<ImpostoPagar>().Remove(impostoPagar);
            }
        }

        public void RemoverTituloPagar(TituloPagar titulo)
        {
            if (titulo != (TituloPagar)null)
            {
                QueryableUnitOfWork.Attach(titulo);
                QueryableUnitOfWork.CreateSet<TituloPagar>().Remove(titulo);
            }
        }

        public void RemoverApropriacao(Apropriacao apropriacao)
        {
            if (apropriacao != (Apropriacao)null)
            {
                QueryableUnitOfWork.Attach(apropriacao);
                QueryableUnitOfWork.CreateSet<Apropriacao>().Remove(apropriacao);
            }
        }

        #endregion
    }
}