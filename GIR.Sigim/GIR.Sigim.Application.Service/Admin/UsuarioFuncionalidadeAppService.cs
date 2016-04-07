using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Admin;
using GIR.Sigim.Application.Resource;
using GIR.Sigim.Domain.Repository.Admin;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Domain.Specification.Admin;
using GIR.Sigim.Application.Filtros.Admin;
using GIR.Sigim.Application.Filtros;
using GIR.Sigim.Domain.Entity.Admin;

namespace GIR.Sigim.Application.Service.Admin
{
    public class UsuarioFuncionalidadeAppService : BaseAppService, IUsuarioFuncionalidadeAppService
    {
        private IUsuarioFuncionalidadeRepository usuarioFuncionalidadeRepository;

        public UsuarioFuncionalidadeAppService(IUsuarioFuncionalidadeRepository usuarioFuncionalidadeRepository, 
                                               MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.usuarioFuncionalidadeRepository = usuarioFuncionalidadeRepository;
        }

        #region IUsuarioFuncionalidadeRepository Members

        public int ObterQuantidadeDeUsuariosNoModulo(int ModuloId)
        {
            int? quantidade = usuarioFuncionalidadeRepository.ListarPeloFiltro(l => l.ModuloId == ModuloId).Select(l => l.UsuarioId).Distinct().Count();
            if (!quantidade.HasValue)
            {
                quantidade = 0;
            }

            return quantidade.Value;
        }

        #endregion


        #region Métodos Privados


        #endregion


    }
}