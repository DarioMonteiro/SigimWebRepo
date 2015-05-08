using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Domain.Repository.Contrato;
using GIR.Sigim.Application.DTO.Contrato;

namespace GIR.Sigim.Application.Service.Contrato
{
    public class ContratoRetificacaoProvisaoAppService : BaseAppService, IContratoRetificacaoProvisaoAppService
    {
        #region Declaração

        private IContratoRetificacaoProvisaoRepository contratoRetificacaoProvisaoRepository;

        private IContratoRetificacaoItemMedicaoAppService contratoRetificacaoItemMedicaoAppService;

        #endregion

        #region Contrutor

        public ContratoRetificacaoProvisaoAppService(IContratoRetificacaoProvisaoRepository contratoRetificacaoProvisaoRepository, 
                                                     IContratoRetificacaoItemMedicaoAppService contratoRetificacaoItemMedicaoAppService,
                                                     MessageQueue messageQueue)
            :base(messageQueue)

        {
            this.contratoRetificacaoProvisaoRepository = contratoRetificacaoProvisaoRepository;
            this.contratoRetificacaoItemMedicaoAppService = contratoRetificacaoItemMedicaoAppService;
        }
        #endregion

        #region Médodos IContratoRetificacaoProvisaoAppService

        public List<ContratoRetificacaoProvisaoDTO> ObterListaCronograma(int id)
        {

            List<ContratoRetificacaoProvisaoDTO> listaContratoRetificacaoProvisaoDTO =
                contratoRetificacaoProvisaoRepository.ListarPeloFiltro(l => l.ContratoRetificacaoItemId == id,
                                                                       l => l.ContratoRetificacaoItem.Servico,
                                                                       l => l.ContratoRetificacaoItem.RetencaoTipoCompromisso,
                                                                       l => l.ContratoRetificacaoItemCronograma).To<List<ContratoRetificacaoProvisaoDTO>>();

            List<ContratoRetificacaoProvisaoDTO> novaLista = new List<ContratoRetificacaoProvisaoDTO>();
            foreach (var item in listaContratoRetificacaoProvisaoDTO)
            {
                decimal qtdTotalMedido = 0;
                decimal vlrTotalMedido = 0;
                decimal qtdTotalLiberado = 0;
                decimal vlrTotalLiberado = 0;

                contratoRetificacaoItemMedicaoAppService.ObterQuantidadesEhValoresMedicao(item.ContratoRetificacaoItemId.Value,
                                                                                          item.ContratoRetificacaoItemCronogramaId.Value,
                                                                                          ref qtdTotalMedido,
                                                                                          ref vlrTotalMedido,
                                                                                          ref qtdTotalLiberado,
                                                                                          ref vlrTotalLiberado);
                item.QuantidadeTotalMedida = qtdTotalMedido;
                item.ValorTotalMedido = vlrTotalMedido;
                item.QuantidadeTotalLiberada = qtdTotalLiberado;
                item.ValorTotalLiberado = vlrTotalLiberado;

                item.QuantidadePendente = item.Quantidade - item.QuantidadeTotalMedida;
                item.ValorPendente = item.Valor - item.ValorTotalMedido;

                novaLista.Add(item);
            }

            return novaLista;
        }

        public bool ExisteContratoRetificacaoProvisao(List<ContratoRetificacaoProvisaoDTO> listaContratoRetificacaoProvisao)
        {
            if (listaContratoRetificacaoProvisao == null)
            {
                messageQueue.Add(Resource.Contrato.ErrorMessages.RetificacaoItemSemProvisionamento, TypeMessage.Error);
                return false;
            }
            if (listaContratoRetificacaoProvisao.Count == 0)
            {
                messageQueue.Add(Resource.Contrato.ErrorMessages.RetificacaoItemSemProvisionamento, TypeMessage.Error);
                return false;
            }

            return true;
        }

        #endregion

    }
}
