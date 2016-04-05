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
        private IAcessoAppService acessoAppService;

        public ModuloAppService(IModuloRepository moduloRepository, 
                                IModuloSigimAppService moduloSigimAppService,
                                IAcessoAppService acessoAppService,
                                MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.moduloSigimAppService = moduloSigimAppService;
            this.moduloRepository = moduloRepository;
            this.acessoAppService = acessoAppService;
        }

        #region IModuloAppService Members

        public List<ModuloDTO> ListarTodos()
        {
            return moduloRepository.ListarTodos().Where(l => l.Nome.Contains("WEB")).Where(l => l.Nome != "SIGIMWEB").OrderBy(l => l.Nome).To<List<ModuloDTO>>();
        }

        public ModuloDTO ObterPeloId(int? id)
        {
            return moduloRepository.ObterPeloId(id).To<ModuloDTO>();
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

        public bool ValidaAcessoAoModulo(string nomeModulo)
        {
            string nomeModuloAux = nomeModulo + "WEB";

            Modulo modulo = moduloRepository.ListarPeloFiltro(l => l.Nome.ToUpper() == nomeModuloAux.ToUpper()).FirstOrDefault();

            if (string.IsNullOrEmpty(modulo.ChaveAcesso))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.ChaveAcessoNaoInformada, TypeMessage.Error);
                return false;
            }

            ClienteAcessoChaveAcesso infoAcesso = acessoAppService.ObterInfoAcesso(modulo.ChaveAcesso);

            if (!infoAcesso.DataExpiracao.HasValue)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.DataExpiracaoNaoInformada, TypeMessage.Error);
                return false;
            }

            if ((infoAcesso.DataExpiracao.HasValue) && (infoAcesso.DataExpiracao.Value.Date < DateTime.Now.Date))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.DataExpirada, TypeMessage.Error);
                return false;
            }

            //if (acessoAppService.ValidaSistemaBloqueado(nomeModulo))
            //{
            //    messageQueue.Add(Resource.Sigim.ErrorMessages.DataExpiracaoNaoInformada, TypeMessage.Error);
            //    return false;
            //}

            return true;
        }

        #endregion

        #region "Métodos Privados"

        #endregion
    }
}