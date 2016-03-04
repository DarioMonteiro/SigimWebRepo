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
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Application.Filtros.Comercial ;

namespace GIR.Sigim.Application.Service.Comercial
    {
    public class IncorporadorAppService : BaseAppService, IIncorporadorAppService
    {
        private IIncorporadorRepository incorporadorRepository;

        public IncorporadorAppService(IIncorporadorRepository incorporadorRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.incorporadorRepository = incorporadorRepository;
        }

        #region IIncorporadorAppService Members

        public List<IncorporadorDTO> ListarTodos()
        {
            return incorporadorRepository.ListarTodos().To<List<IncorporadorDTO>>();
        }

        #endregion
    }
}