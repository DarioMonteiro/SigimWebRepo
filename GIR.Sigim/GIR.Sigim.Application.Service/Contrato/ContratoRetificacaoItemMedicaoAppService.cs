using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Domain.Repository.Contrato;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Repository.Contrato;
using GIR.Sigim.Domain.Entity.Contrato;

namespace GIR.Sigim.Application.Service.Contrato
{
    public class ContratoRetificacaoItemMedicaoAppService : BaseAppService, IContratoRetificacaoItemMedicaoAppService
    {
        #region Declaração

        private IContratoRetificacaoItemMedicaoRepository contratoRetificacaoItemMedicaoRepository;

        #endregion

        #region Construtor

        public ContratoRetificacaoItemMedicaoAppService(IContratoRetificacaoItemMedicaoRepository contratoRetificacaoItemMedicaoRepository,
                                                        MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.contratoRetificacaoItemMedicaoRepository = contratoRetificacaoItemMedicaoRepository;
        }

        #endregion

        #region Métodos IContratoRetificacaoItemMedicaoAppService

        public void ObterQuantidadesEhValoresMedicao(int ContratoRetificacaoItemId,
                                                     int ContratoRetificacaoItemCronogramaId, 
                                                     ref decimal QuantidadeTotalMedido,
                                                     ref decimal ValorTotalMedido,
                                                     ref decimal QuantidadeTotalLiberado,
                                                     ref decimal ValorTotalLiberado)
        {

            List<ContratoRetificacaoItemMedicao> listaContratoRetificacaoItemMedicao = 
                    contratoRetificacaoItemMedicaoRepository.ListarPeloFiltro(  ( l => 
                                                                                    l.ContratoRetificacaoItemId == ContratoRetificacaoItemId && 
                                                                                    l.ContratoRetificacaoItemCronogramaId == ContratoRetificacaoItemCronogramaId
                                                                                 ), 
                                                                                l => l.ContratoRetificacaoItem,
                                                                                l => l.ContratoRetificacaoItemCronograma).Where(c =>
                                                                                    (c.SequencialItem == c.ContratoRetificacaoItem.Sequencial &&
                                                                                     c.SequencialCronograma == c.ContratoRetificacaoItemCronograma.Sequencial)
                                                                                ).ToList <ContratoRetificacaoItemMedicao>();

            var queryMedido =
                        from c in listaContratoRetificacaoItemMedicao
                        where (
                                ((c.Situacao == SituacaoMedicao.AguardandoAprovacao) &&
                                 (c.Situacao == SituacaoMedicao.AguardandoLiberacao))
                              )
                        group c by c.ContratoRetificacaoItemId into g
                        select new
                        {
                            ContratoRetificacaoItemId   = g.Key,
                            QuantidadeTotalMedido       = g.Sum(i => i.Quantidade),
                            ValorTotalMedido            = g.Sum(i => i.Valor)
                        };

            foreach (var item in queryMedido)
            {
                QuantidadeTotalMedido = item.QuantidadeTotalMedido; 
                ValorTotalMedido = item.ValorTotalMedido;
            }

            var queryLiberado = 
                        from c in listaContratoRetificacaoItemMedicao
                        where (
                                (c.Situacao == SituacaoMedicao.Liberado)
                              )
                        group c by c.ContratoRetificacaoItemId into g
                        select new
                        {
                            ContratoRetificacaoItemId   = g.Key,
                            QuantidadeTotalLiberado     = g.Sum(i => i.Quantidade),
                            ValorTotalLiberado          = g.Sum(i => i.Valor)
                        };

            foreach (var item in queryLiberado)
            {
                QuantidadeTotalLiberado = item.QuantidadeTotalLiberado;
                ValorTotalLiberado = item.ValorTotalLiberado;
            }

        }

        #endregion
    }
}
