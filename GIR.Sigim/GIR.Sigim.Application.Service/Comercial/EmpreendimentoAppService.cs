using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO;
using GIR.Sigim.Domain.Repository.Comercial;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Infrastructure.Crosscutting.Security;
using GIR.Sigim.Application.DTO.Comercial;
using GIR.Sigim.Domain.Entity.Comercial;
using GIR.Sigim.Domain.Repository.Comercial;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Domain.Specification.Comercial;
using GIR.Sigim.Application.Filtros.Comercial;
using GIR.Sigim.Application.Constantes;

namespace GIR.Sigim.Application.Service.Comercial
    {
    public class EmpreendimentoAppService : BaseAppService, IEmpreendimentoAppService
    {
        private IEmpreendimentoRepository empreendimentoRepository;

        public EmpreendimentoAppService(IEmpreendimentoRepository empreendimentoRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.empreendimentoRepository = empreendimentoRepository;
        }

        #region IEmpreendimentoAppService Members

        public List<EmpreendimentoDTO> ListarPeloIncorporador(int incorporadorId)
        {
            return empreendimentoRepository.ListarPeloFiltro(l => l.IncorporadorId == incorporadorId).To<List<EmpreendimentoDTO>>();
        }

        #endregion

    }
}