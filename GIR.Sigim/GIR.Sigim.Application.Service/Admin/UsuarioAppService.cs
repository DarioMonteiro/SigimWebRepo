﻿using System;
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

namespace GIR.Sigim.Application.Service.Admin
{
    public class UsuarioAppService : BaseAppService, IUsuarioAppService
    {
        private IUsuarioRepository usuarioRepository;

        public UsuarioAppService(IUsuarioRepository usuarioRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            if (usuarioRepository == null)
                throw new ArgumentNullException("usuarioRepository");

            this.usuarioRepository = usuarioRepository;
        }

        private IAuthenticationService authenticationService;
        protected IAuthenticationService AuthenticationService
        {
            get
            {
                if (authenticationService == null)
                    authenticationService = AuthenticationServiceFactory.Create();

                return authenticationService;
            }
        }

        #region IUsuarioService Members

        public bool Login(string login, string senha, bool permacenerLogado, int timeout, string hostName)
        {
            if (string.IsNullOrEmpty(login))
                throw new ArgumentNullException("login");

            if (string.IsNullOrEmpty(senha))
                throw new ArgumentNullException("senha");

            var usuario = usuarioRepository.ObterPeloLogin(login);
            if (usuario != null && CryptographyHelper.VerifyHashedPassword(senha, usuario.Senha))
            {
                var customPrincipal = new CustomPrincipalSerializeModel();
                customPrincipal.Id = usuario.Id;
                customPrincipal.Nome = usuario.Nome;
                customPrincipal.Login = usuario.Login;
                customPrincipal.HostName = hostName;

                var serializer = new JavaScriptSerializer();
                var dadosUsuario = serializer.Serialize(customPrincipal);

                AuthenticationService.Login(customPrincipal.Nome, permacenerLogado, dadosUsuario, timeout);

                return true;
            }

            messageQueue.Add(Resource.Admin.ErrorMessages.UsuarioOuSenhaIncorretos, TypeMessage.Error);
            return false;
        }

        public void Logout()
        {
            AuthenticationServiceFactory.Create().Logout();
        }

        public bool ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            if (newPassword.Trim() == UsuarioLogado.Login)
            {
                messageQueue.Add(Resource.Admin.ErrorMessages.NovaSenhaIgualLogin, TypeMessage.Error);
                return false;
            }

            if (newPassword.Trim() != confirmPassword.Trim())
            {
                messageQueue.Add(Resource.Admin.ErrorMessages.ConfirmacaoNovaSenhaNaoConfere, TypeMessage.Error);
                return false;
            }

            var usuario = usuarioRepository.ObterPeloLogin(UsuarioLogado.Login);
            if (usuario != null && CryptographyHelper.VerifyHashedPassword(currentPassword, usuario.Senha))
            {
                if (IsValidPassword(newPassword))
                {
                    newPassword = CryptographyHelper.CreateHashPassword(newPassword.Trim());
                    if (newPassword == usuario.Senha)
                    {
                        messageQueue.Add(Resource.Admin.ErrorMessages.NovaSenhaIgualAtual, TypeMessage.Error);
                        return false;
                    }

                    if (Validator.IsValid(usuario, out validationErrors))
                    {
                        usuario.Senha = newPassword;
                        usuarioRepository.Alterar(usuario);
                        usuarioRepository.UnitOfWork.Commit();
                        messageQueue.Add(Resource.Admin.SuccessMessages.SenhaAlteradaComSucesso, TypeMessage.Success);
                        return true;
                    }
                    else
                        messageQueue.AddRange(validationErrors, TypeMessage.Error);
                }
            }
            else
                messageQueue.Add(Resource.Admin.ErrorMessages.SenhaAtualIncorreta, TypeMessage.Error);

            return false;
        }

        public bool UsuarioPossuiCentroCustoDefinidoNoModulo(int? idUsuario, string modulo)
        {
            var usuario = usuarioRepository.ObterPeloId(idUsuario, l => l.ListaUsuarioCentroCusto);
            return usuario.ListaUsuarioCentroCusto.Any(l => l.Modulo.Nome == modulo);
        }

        public UsuarioDTO ObterUsuarioPorLogin(string login)
        {
            return usuarioRepository.ObterPeloLogin(login).To<UsuarioDTO>();
        }

        #endregion

        private bool IsValidPassword(string senha)
        {
            if (senha.Contains(' '))
            {
                messageQueue.Add(Resource.Admin.ErrorMessages.SenhaComEspacos, TypeMessage.Error);
                return false;
            }

            if (string.IsNullOrEmpty(senha.Trim()))
            {
                messageQueue.Add(Resource.Admin.ErrorMessages.SenhaEmBranco, TypeMessage.Error);
                return false;
            }

            if (senha.Trim().Length < 7)
            {
                messageQueue.Add(Resource.Admin.ErrorMessages.SenhaComMenosDeSeteCaracteres, TypeMessage.Error);
                return false;
            }

            if (!Regex.IsMatch(senha.Trim(), @"\d"))
            {
                messageQueue.Add(Resource.Admin.ErrorMessages.SenhaSemNumero, TypeMessage.Error);
                return false;
            }

            if (!Regex.IsMatch(senha.Trim(), "[a-zA-Z]"))
            {
                messageQueue.Add(Resource.Admin.ErrorMessages.SenhaSemLetra, TypeMessage.Error);
                return false;
            }
            
            if (!Regex.IsMatch(senha.Trim(), @"[!#$\-+\?:\]\[()]"))
            {
                messageQueue.Add(Resource.Admin.ErrorMessages.SenhaSemCaracterEspecial, TypeMessage.Error);
                return false;
            }

            return true;
        }
    }
}