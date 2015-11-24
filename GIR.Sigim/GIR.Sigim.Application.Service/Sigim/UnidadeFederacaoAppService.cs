using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;


namespace GIR.Sigim.Application.Service.Sigim
{
    public class UnidadeFederacaoAppService : BaseAppService, IUnidadeFederacaoAppService
    {
        #region Declaração
        private IUnidadeFederacaoRepository unidadeFederacaoRepository;
        #endregion

        #region Constructor

        public UnidadeFederacaoAppService(IUnidadeFederacaoRepository unidadeFederacaoRepository,
                                          MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.unidadeFederacaoRepository = unidadeFederacaoRepository;
        }
        #endregion

        #region IUnidadeMedidaAppService Members

        public List<UnidadeFederacaoDTO> ListarTodos()
        {
            return unidadeFederacaoRepository.ListarTodos().To<List<UnidadeFederacaoDTO>>();
        }

        #endregion

    }
}
