using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO;
using GIR.Sigim.Domain.Repository.Admin;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Infrastructure.Crosscutting.Security;
using GIR.Sigim.Application.DTO.Admin;
using GIR.Sigim.Domain.Entity.Admin;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Application.Filtros.Admin;

namespace GIR.Sigim.Application.Service.Admin
{
    public class ModuloAppService : BaseAppService, IModuloAppService
    {
        private IModuloRepository moduloRepository;

        public ModuloAppService(IModuloRepository moduloRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.moduloRepository = moduloRepository;
        }

        #region IModuloAppService Members

        public List<ModuloDTO> ListarTodos()
        {
            return moduloRepository.ListarTodos().Where(l => l.Nome.Contains("WEB")).OrderBy(l => l.Nome).To<List<ModuloDTO>>();
        }

        public ModuloDTO ObterPeloId(int? id)
        {
            return moduloRepository.ObterPeloId(id).To<ModuloDTO>();
        }

           #endregion
    }
}