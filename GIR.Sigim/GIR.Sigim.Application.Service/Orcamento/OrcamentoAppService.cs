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

namespace GIR.Sigim.Application.Service.Orcamento
{
    public class OrcamentoAppService : BaseAppService, IOrcamentoAppService
    {
        private IOrcamentoRepository orcamentoRepository;

        public OrcamentoAppService(IOrcamentoRepository orcamentoRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.orcamentoRepository = orcamentoRepository;
        }

        public OrcamentoDTO ObterUltimoOrcamentoPeloCentroCustoClasseOrcamento(string codigoCentroCusto)
        {
            return orcamentoRepository.ObterUltimoOrcamentoPeloCentroCustoClasseOrcamento(codigoCentroCusto).To<OrcamentoDTO>();
        }

        public OrcamentoDTO ObterUltimoOrcamentoPeloCentroCusto(string codigoCentroCusto)
        {
            return orcamentoRepository.ObterUltimoOrcamentoPeloCentroCusto(codigoCentroCusto).To<OrcamentoDTO>();
        }
    }
}