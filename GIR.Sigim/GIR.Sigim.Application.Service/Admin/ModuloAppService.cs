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
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Domain.Entity.GirCliente;

namespace GIR.Sigim.Application.Service.Admin
{
    public class ModuloAppService : BaseAppService, IModuloAppService
    {
        private IModuloRepository moduloRepository;
        private IModuloSigimAppService moduloSigimAppService;
        //private IAcessoAppService acessoAppService;

        public ModuloAppService(IModuloRepository moduloRepository, 
                                IModuloSigimAppService moduloSigimAppService,
                                MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.moduloSigimAppService = moduloSigimAppService;
            this.moduloRepository = moduloRepository;
        }

        #region IModuloAppService Members

        public List<ModuloDTO> ListarTodosWEB()
        {
            return moduloRepository.ListarTodos().Where(l => l.Nome.Contains("WEB")).Where(l => l.Nome != "SIGIMWEB").OrderBy(l => l.Nome).To<List<ModuloDTO>>();
        }

        public List<ModuloDTO> ListarTodos()
        {
            return moduloRepository.ListarTodos().To<List<ModuloDTO>>();
        }

        public ModuloDTO ObterPeloId(int? id)
        {
            return moduloRepository.ObterPeloId(id).To<ModuloDTO>();
        }

        public ModuloDTO ObterPeloNome(string nomeModulo)
        {
            return moduloRepository.ListarPeloFiltro(l => l.Nome.ToUpper() == nomeModulo.ToUpper()).FirstOrDefault().To<ModuloDTO>();
        }

        public bool PossuiModulo(string nomeModulo)
        {
            string nomeModuloAux =  nomeModulo + "WEB";
            if (moduloRepository.ListarTodos().Any(l => l.Nome.ToUpper() == nomeModuloAux.ToUpper()))
            {
                return true;
            }

            messageQueue.Add(Resource.Sigim.ErrorMessages.ModuloNaoPermitido, TypeMessage.Info);
            return false;
        }

        public bool AtualizaBloqueio(int ModuloId,bool bloqueio)
        {
            bool atualizou = false;

            var modulo = moduloRepository.ObterPeloId(ModuloId);
            if (modulo != null)
            {
                modulo.Bloqueio = bloqueio;
                try
                {
                    //moduloRepository.Alterar(modulo);
                    //moduloRepository.UnitOfWork.Commit();
                    atualizou = true;
                }
                catch (Exception exception)
                {
                }
            }

            return atualizou;
        }


        #endregion

        #region "Métodos Privados"

        #endregion
    }
}