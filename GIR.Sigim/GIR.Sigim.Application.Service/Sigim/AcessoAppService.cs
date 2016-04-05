using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Domain.Entity.GirCliente;

namespace GIR.Sigim.Application.Service.Sigim
{
    public class AcessoAppService : BaseAppService, IAcessoAppService
    {

        #region Declaração

        private IModuloSigimAppService moduloSigimAppService;
        
        #endregion

        #region Construtor

        public AcessoAppService(IModuloSigimAppService moduloSigimAppService,
                                MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.moduloSigimAppService = moduloSigimAppService;
        }

        #endregion

        #region "IAcessoAppService Members"

        public ClienteAcessoChaveAcesso ObterInfoAcesso(string textCripto)
        {
            ClienteAcessoChaveAcesso infoAcesso = new ClienteAcessoChaveAcesso();

            string chaveAcessoDescryptografada = moduloSigimAppService.UnCrypt(textCripto);

            string infoRecup = moduloSigimAppService.GetPiece(chaveAcessoDescryptografada, "|", 1);

            if (!string.IsNullOrEmpty(infoRecup))
            {
                infoAcesso.ClienteFornecedor.Id = Convert.ToInt32(infoRecup);
            }

            infoRecup = moduloSigimAppService.GetPiece(chaveAcessoDescryptografada, "|", 2);
            if (!string.IsNullOrEmpty(infoRecup)) infoAcesso.ClienteFornecedor.Nome = infoRecup;

            infoRecup = moduloSigimAppService.GetPiece(chaveAcessoDescryptografada, "|", 3);
            if (!string.IsNullOrEmpty(infoRecup)) infoAcesso.DataExpiracao = Convert.ToDateTime(infoRecup);

            infoAcesso.NumeroUsuario = 0;
            infoRecup = moduloSigimAppService.GetPiece(chaveAcessoDescryptografada, "|", 4);
            if (!string.IsNullOrEmpty(infoRecup)) infoAcesso.NumeroUsuario = Convert.ToInt32(infoRecup);

            return infoAcesso;
        }

        //public bool ValidaSistemaBloqueado(string NomeModulo)
        //{
        //    if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
        //    {
        //        Object objClienteSistemaBloqueioWS = new cliente
        //    }

        //    return true;
        //}
        #endregion
    }
}
