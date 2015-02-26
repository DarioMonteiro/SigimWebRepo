using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;

namespace GIR.Sigim.Application.Service.Sigim
{
    public class BancoLayoutAppService : BaseAppService, IBancoLayoutAppService
    {
        private IBancoLayoutRepository bancoLayoutRepository;

        public BancoLayoutAppService(IBancoLayoutRepository bancoLayoutRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.bancoLayoutRepository = bancoLayoutRepository;
        }

        #region IBancoLayoutAppService Members

        public List<BancoLayoutDTO> ListarPeloTipoInterfaceCotacao()
        {
            return bancoLayoutRepository.ListarPeloFiltro(l => l.Tipo == TipoLayout.InterfaceCotacao).To<List<BancoLayoutDTO>>();
        }

        public List<BancoLayoutDTO> ListarPeloTipoInterfaceSpedFiscal()
        {
            return bancoLayoutRepository.ListarPeloFiltro(l => l.Tipo == TipoLayout.InterfaceSpedFiscal).To<List<BancoLayoutDTO>>();
        }

        #endregion
    }
}