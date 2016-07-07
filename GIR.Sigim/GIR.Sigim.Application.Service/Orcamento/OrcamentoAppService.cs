using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Orcamento;
using GIR.Sigim.Domain.Entity.Orcamento;
using GIR.Sigim.Domain.Repository.Orcamento;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Application.Filtros.Financeiro;
using GIR.Sigim.Application.Constantes;

namespace GIR.Sigim.Application.Service.Orcamento
{
    public class OrcamentoAppService : BaseAppService, IOrcamentoAppService
    {
        #region Declaração

        private IOrcamentoRepository orcamentoRepository;

        #endregion

        #region Construtor

        public OrcamentoAppService(IOrcamentoRepository orcamentoRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.orcamentoRepository = orcamentoRepository;
        }

        #endregion

        #region Métodos IOrcamentoAppService

        public OrcamentoDTO ObterUltimoOrcamentoPeloCentroCustoClasseOrcamento(string codigoCentroCusto)
        {
            return orcamentoRepository.ObterUltimoOrcamentoPeloCentroCustoClasseOrcamento(codigoCentroCusto).To<OrcamentoDTO>();
        }

        public OrcamentoDTO ObterUltimoOrcamentoPeloCentroCusto(string codigoCentroCusto)
        {
            return orcamentoRepository.ObterUltimoOrcamentoPeloCentroCusto(codigoCentroCusto).To<OrcamentoDTO>();
        }

        public bool EhPermitidoImprimirRelOrcamento()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.RelatorioOrcamentoImprimir);
        }

        #endregion
    }
}