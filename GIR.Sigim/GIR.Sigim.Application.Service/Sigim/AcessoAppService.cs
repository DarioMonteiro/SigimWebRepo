using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Domain.Entity.GirCliente;
using GIR.Sigim.Application.DTO.Admin;
using GIR.Sigim.Domain.Repository.Admin;
using GIR.Sigim.Application.Service.Admin;
using GIR.Sigim.Application.DTO.Sigim;


namespace GIR.Sigim.Application.Service.Sigim
{
    public class AcessoAppService : BaseAppService, IAcessoAppService
    {

        #region Declaração

        private IModuloSigimAppService moduloSigimAppService;
        private IModuloAppService moduloAppService;
        private IUsuarioAppService usuarioAppService;
        private IUsuarioFuncionalidadeAppService usuarioFuncionalidadeAppService;
        
        #endregion

        #region Construtor

        public AcessoAppService(IModuloSigimAppService moduloSigimAppService,
                                IModuloAppService moduloAppService,
                                IUsuarioAppService usuarioAppService,
                                IUsuarioFuncionalidadeAppService usuarioFuncionalidadeAppService,
                                MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.moduloSigimAppService = moduloSigimAppService;
            this.moduloAppService = moduloAppService;
            this.usuarioAppService = usuarioAppService;
            this.usuarioFuncionalidadeAppService = usuarioFuncionalidadeAppService;
        }

        #endregion

        #region "IAcessoAppService Members"

        public bool ValidaAcessoAoModulo(string nomeModulo, InformacaoConfiguracaoDTO informacaoConfiguracao)
        {
            string nomeModuloAux = nomeModulo + "WEB";

            ModuloDTO modulo = moduloAppService.ObterPeloNome(nomeModuloAux.ToUpper());

            if (string.IsNullOrEmpty(modulo.ChaveAcesso))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.ChaveAcessoNaoInformada, TypeMessage.Error);
                return false;
            }

            ClienteAcessoChaveAcesso infoAcesso = ObterInfoAcesso(modulo.ChaveAcesso);

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

            //infoAcesso.ClienteFornecedor.Id = 4215;
            if (EhSistemaBloqueado(nomeModulo, infoAcesso.ClienteFornecedor.Id.Value, informacaoConfiguracao.LogGirCliente))
            {
                TratarBloqueioNoSistemaSigim(infoAcesso.ClienteFornecedor.Id.Value, informacaoConfiguracao.LogGirCliente);
                messageQueue.Add(Resource.Sigim.ErrorMessages.SistemaBloqueado, TypeMessage.Error);
                return false;
            }

            return true;
        }

        public bool ValidaAcessoGirCliente(string nomeModulo, int usuarioId, InformacaoConfiguracaoDTO informacaoConfiguracao)
        {
            bool validou = true;

            if (!informacaoConfiguracao.LogGirCliente) return validou;
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) return validou;

            string nomeModuloAux = nomeModulo + "WEB";

            ModuloDTO modulo = moduloAppService.ObterPeloNome(nomeModuloAux.ToUpper());

            UsuarioDTO usuario = usuarioAppService.ObterUsuarioPorId(usuarioId);

            if (string.IsNullOrEmpty(modulo.ChaveAcesso))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.ChaveAcessoNaoInformada, TypeMessage.Error);
                return false;
            }

            ClienteAcessoChaveAcesso infoAcesso = ObterInfoAcesso(modulo.ChaveAcesso);

            int numeroUsuarioSistema = usuarioFuncionalidadeAppService.ObterQuantidadeDeUsuariosNoModulo(modulo.Id.Value);

            clienteAcessoLogWS.clienteAcessoLogWS clienteAcessoLogWS = new clienteAcessoLogWS.clienteAcessoLogWS();
            clienteAcessoLogWS.Timeout = 10000;
            validou = clienteAcessoLogWS.AtualizaAcessoCliente(infoAcesso.ClienteFornecedor.Id.Value,
                                                               infoAcesso.ClienteFornecedor.Nome,
                                                               informacaoConfiguracao.StringConexao,
                                                               informacaoConfiguracao.EnderecoIP,
                                                               usuario.Login,
                                                               modulo.Nome,
                                                               numeroUsuarioSistema,
                                                               informacaoConfiguracao.Instancia,
                                                               modulo.Versao);  
            if (!validou)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.ErroAcessoLogGirCliente, TypeMessage.Error);
                return false;
            }

            return validou;
        }

        #endregion

        #region "Métodos Privados"
        private ClienteAcessoChaveAcesso ObterInfoAcesso(string textCripto)
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

        private bool EhSistemaBloqueado(string NomeModulo, int ClienteGirClienteId, bool LogarGirCliente)
        {
            bool bloqueado = false;

            if (!LogarGirCliente) return bloqueado;
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) return bloqueado;

            clienteSistemaBloqueioWS.clienteSistemaBloqueioWS objClienteSistemaBloqueioWS = new clienteSistemaBloqueioWS.clienteSistemaBloqueioWS();
            objClienteSistemaBloqueioWS.Timeout = 10000;
            clienteSistemaBloqueioWS.clienteSistemaBloqueioSIGIMWS[] lstClienteSistemaBloqueioWS = objClienteSistemaBloqueioWS.RecuperaPorCliente(ClienteGirClienteId);
            foreach (clienteSistemaBloqueioWS.clienteSistemaBloqueioSIGIMWS objReg in lstClienteSistemaBloqueioWS)
            {
                ModuloDTO modulo = moduloAppService.ObterPeloNome(objReg.nomeInternoSistema);
                if (modulo.Nome == NomeModulo)
                {
                    bloqueado = true;
                }
            }

            return bloqueado;
        }

        private void TratarBloqueioNoSistemaSigim(int clienteGirClienteId, bool logarGirCliente)
        {
            List<ModuloDTO> listaSistemasBloqueados = new List<ModuloDTO>();

            if (!logarGirCliente) return;
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) return;

            clienteSistemaBloqueioWS.clienteSistemaBloqueioWS objClienteSistemaBloqueioWS = new clienteSistemaBloqueioWS.clienteSistemaBloqueioWS();
            objClienteSistemaBloqueioWS.Timeout = 10000;
            clienteSistemaBloqueioWS.clienteSistemaBloqueioSIGIMWS[] lstClienteSistemaBloqueioWS = objClienteSistemaBloqueioWS.RecuperaPorCliente(clienteGirClienteId);
            foreach (clienteSistemaBloqueioWS.clienteSistemaBloqueioSIGIMWS objReg in lstClienteSistemaBloqueioWS)
            {
                ModuloDTO modulo = moduloAppService.ObterPeloNome(objReg.nomeInternoSistema);

                listaSistemasBloqueados.Add(modulo);
            }

            List<ModuloDTO> listaSistemas = moduloAppService.ListarTodos();
            foreach (ModuloDTO modulo in listaSistemas)
            {
                bool bloqueio = false;
                if (listaSistemasBloqueados.Any(l => l.Id == modulo.Id))
                {
                    bloqueio = true;
                }
                moduloAppService.AtualizaBloqueio(modulo.Id.Value, bloqueio);
            }

        }

        #endregion

    }
}
