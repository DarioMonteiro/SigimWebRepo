using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Domain.Repository.Contrato;

namespace GIR.Sigim.Application.Service.Contrato
{
    public class ContratoRetificacaoItemImpostoAppService : BaseAppService, IContratoRetificacaoItemImpostoAppService
    {

        #region Declaração
            private IContratoRetificacaoItemImpostoRepository contratoRetificacaoItemImpostoRepository;
        #endregion

        #region Construtor

            public ContratoRetificacaoItemImpostoAppService(IContratoRetificacaoItemImpostoRepository contratoRetificacaoItemImpostoRepository,
                                                            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.contratoRetificacaoItemImpostoRepository = contratoRetificacaoItemImpostoRepository;
        }

        #endregion

        #region Métodos IContratoRetificacaoItemImpostoAppService

            public List<ContratoRetificacaoItemImposto> RecuperaImpostoPorContratoDadosDaNota(int contratoId,
                                                                                              int tipoDocumentoId,
                                                                                              string numeroDocumento,
                                                                                              DateTime dataEmissao,
                                                                                              int? contratadoId,
                                                                                              params Expression<Func<ContratoRetificacaoItemImposto, object>>[] includes)
            {
                return contratoRetificacaoItemImpostoRepository.RecuperaImpostoPorContratoDadosDaNota(contratoId, 
                                                                                                      tipoDocumentoId, 
                                                                                                      numeroDocumento, 
                                                                                                      dataEmissao, 
                                                                                                      contratadoId,
                                                                                                      includes).To<List<ContratoRetificacaoItemImposto>>();
            }

        #endregion

    }
}
