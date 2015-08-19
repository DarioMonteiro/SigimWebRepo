using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.Service.Sigim
{
    public class BloqueioContabilAppService : BaseAppService, IBloqueioContabilAppService
    {
        #region Declaração
            private IBloqueioContabilRepository BloqueioContabilRepository;
        #endregion

        #region Construtor
            public BloqueioContabilAppService(IBloqueioContabilRepository BloqueioContabilRepository, MessageQueue messageQueue)
                : base(messageQueue)
            {
                this.BloqueioContabilRepository = BloqueioContabilRepository;
            }
        #endregion

        #region IBloqueioContabilAppService

            public bool OcorreuBloqueioContabil(string codigoCentroCusto, DateTime dataOperacao, out Nullable<DateTime> dataBloqueio)
            {

                List<BloqueioContabil> listaBloqueio
                 = BloqueioContabilRepository.ListarPeloFiltro((l => l.CodigoCentroCusto == codigoCentroCusto),
                                                                            c => c.CentroCusto).ToList<BloqueioContabil>();

                dataBloqueio = null;
                if (listaBloqueio.Count > 0)
                {
                    dataBloqueio = listaBloqueio.Max(l => l.Data);
                }                                                                           

                if (dataBloqueio.HasValue)
                {
                    if (dataOperacao <= dataBloqueio) return true;
                }

                return false;
            }

        #endregion

    }
}
