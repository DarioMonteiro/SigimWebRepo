using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO;
using GIR.Sigim.Application.DTO.Admin;
using GIR.Sigim.Application.Filtros;
using GIR.Sigim.Application.Filtros.Admin;
using GIR.Sigim.Domain.Entity.Admin;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Repository.Admin;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.IoC;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Infrastructure.Crosscutting.Security;
using Microsoft.Practices.Unity;
using GIR.Sigim.Application.Constantes;

namespace GIR.Sigim.Application.Service.Admin
{
    public class UsuarioAppService : BaseAppService, IUsuarioAppService
    {
        private IUsuarioRepository usuarioRepository;
        private ILogAcessoRepository logAcessoRepository;
        private IPerfilRepository perfilRepository;
        private IModuloRepository moduloRepository;

        public UsuarioAppService(IUsuarioRepository usuarioRepository,
                                 ILogAcessoRepository logAcessoRepository,
                                 IPerfilRepository perfilRepository,
                                 IModuloRepository moduloRepository,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            if (usuarioRepository == null)
                throw new ArgumentNullException("usuarioRepository");

            if (logAcessoRepository == null)
                throw new ArgumentNullException("logAcessoRepository");

            this.usuarioRepository = usuarioRepository;
            this.logAcessoRepository = logAcessoRepository;
            this.perfilRepository = perfilRepository;
            this.moduloRepository = moduloRepository;
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
                GravarLogAcesso(hostName, login, "IN");
                return true;
            }

            messageQueue.Add(Resource.Admin.ErrorMessages.UsuarioOuSenhaIncorretos, TypeMessage.Error);
            return false;
        }

        public void Logout()
        {
            AuthenticationServiceFactory.Create().Logout();
            GravarLogAcesso(UsuarioLogado.HostName, UsuarioLogado.Login, "OUT");
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
            if ((usuario.Login.ToUpper().Contains("SIGIM")) || (usuario.Login.ToUpper().Contains("GIR"))) return false;
            return usuario.ListaUsuarioCentroCusto.Any(l => l.Modulo.Nome == modulo);
        }

        public UsuarioDTO ObterUsuarioPorLogin(string login)
        {
            return usuarioRepository.ObterPeloLogin(login).To<UsuarioDTO>();
        }

        public UsuarioDTO ObterUsuarioPorId(int id)
        {
            return usuarioRepository.ObterPeloId(id, l => l.ListaUsuarioFuncionalidade, l => l.ListaUsuarioPerfil).To<UsuarioDTO>();
        }

        public string[] ObterPermissoesUsuario(int? usuarioId)
        {
            var usuarioRepository = Container.Current.Resolve<IUsuarioRepository>();
            var usuario = usuarioRepository.ObterPeloId(usuarioId,
                l => l.ListaUsuarioFuncionalidade,
                l => l.ListaUsuarioPerfil.Select(o => o.Perfil.ListaFuncionalidade));

            List<string> roles = new List<string>();
            roles.AddRange(usuario.ListaUsuarioFuncionalidade.To<List<string>>());
            roles.AddRange(usuario.ListaUsuarioPerfil.SelectMany(l => l.Perfil.ListaFuncionalidade).To<List<string>>());

            return roles.ToArray<string>();
        }

        public bool EhPermitidoSalvar()
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.UsuarioFuncionalidadeGravar))
                return false;

            return true;
        }

        public bool EhPermitidoDeletar()
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.UsuarioFuncionalidadeDeletar))
                return false;

            return true;
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

        private void GravarLogAcesso(string hostName, string login, string tipo)
        {
            try
            {
                var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                System.Data.SqlClient.SqlConnectionStringBuilder connectionStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder(connectionString);
                
                LogAcesso log = new LogAcesso()
                {
                    Data = DateTime.Now,
                    HostName = hostName.ToUpper(),
                    LoginUsuario = login.ToUpper(),
                    NomeBaseDados = connectionStringBuilder.InitialCatalog.ToUpper(),
                    Servidor = connectionStringBuilder.DataSource.ToUpper(),
                    Sistema = "SIGIMWEB",
                    Tipo = tipo.ToUpper(),
                    Versao = System.Reflection.Assembly.GetCallingAssembly().GetName().Version.ToString()
                };

                logAcessoRepository.Inserir(log);
                logAcessoRepository.UnitOfWork.Commit();
            }
            catch (Exception exception)
            {
                QueueExeptionMessages(exception);
            }
        }

        public List<UsuarioDTO> ListarTodos()
        {
            return usuarioRepository.ListarTodos().To<List<UsuarioDTO>>();
        }

        public List<UsuarioPerfilDTO> ListarPeloUsuarioModulo(UsuarioFuncionalidadeFiltro filtro, out int totalRegistros)
        {
            List<UsuarioFuncionalidade> listaFuncionalidadeAux;
            List<UsuarioFuncionalidade> listaFuncionalidade = new List<UsuarioFuncionalidade>();
            List<UsuarioPerfil> listaPerfilAux;
            List<UsuarioPerfil> listaPerfil = new List<UsuarioPerfil>();
            List<UsuarioPerfilDTO> listaRetorno = new List<UsuarioPerfilDTO>();
            UsuarioPerfilDTO objRetorno = new UsuarioPerfilDTO();

            var listaUsuario = usuarioRepository.ListarTodos(l => l.ListaUsuarioFuncionalidade.Select(i => i.Modulo),
                                                             l => l.ListaUsuarioPerfil.Select(i => i.Modulo),
                                                             l => l.ListaUsuarioPerfil.Select(i => i.Perfil)).ToList<Usuario>();

            foreach (Usuario usuario in listaUsuario)
            {
                if (filtro.ModuloId != null)
                {
                    listaFuncionalidadeAux = usuario.ListaUsuarioFuncionalidade.Where(i => i.ModuloId == filtro.ModuloId).ToList<UsuarioFuncionalidade>();
                    listaPerfilAux = usuario.ListaUsuarioPerfil.Where(i => i.ModuloId == filtro.ModuloId).ToList<UsuarioPerfil>();
                }
                else 
                {
                    listaFuncionalidadeAux = usuario.ListaUsuarioFuncionalidade.ToList<UsuarioFuncionalidade>();
                    listaPerfilAux = usuario.ListaUsuarioPerfil.ToList<UsuarioPerfil>();
                }


                if (filtro.UsuarioId != null)
                {
                    if (filtro.UsuarioId == usuario.Id)
                    {
                        listaFuncionalidade.AddRange(listaFuncionalidadeAux);
                        listaPerfil.AddRange(listaPerfilAux);

                        break;
                    }
                }
                else
                {
                    listaFuncionalidade.AddRange(listaFuncionalidadeAux);
                    listaPerfil.AddRange(listaPerfilAux);
                }

            }

            string chave;
            var vetRepeticao = new System.Collections.Hashtable();
            foreach (var item in listaPerfil)
            {
                chave = item.UsuarioId + "|" + item.ModuloId;

                if (vetRepeticao.ContainsKey(chave) == true) { continue; }

                objRetorno = null;
                objRetorno = new UsuarioPerfilDTO();
                objRetorno.Modulo = item.Modulo.To<ModuloDTO>();
                objRetorno.ModuloId = objRetorno.Modulo.Id.Value;
                objRetorno.Usuario = item.Usuario.To<UsuarioDTO>();
                objRetorno.UsuarioId = objRetorno.Usuario.Id.Value;
                objRetorno.Perfil = item.Perfil.To<PerfilDTO>();
                objRetorno.PerfilId = objRetorno.Perfil.Id.Value;
                listaRetorno.Add(objRetorno);
                vetRepeticao.Add(chave, chave);
            }
                        
            foreach (var item in listaFuncionalidade)
            {
                chave = item.UsuarioId + "|" + item.ModuloId;

                if (vetRepeticao.ContainsKey(chave) == true) { continue; }

                objRetorno = null;
                objRetorno = new UsuarioPerfilDTO();
                objRetorno.Modulo = item.Modulo.To<ModuloDTO>();
                objRetorno.ModuloId = objRetorno.Modulo.Id.Value;
                objRetorno.Usuario = item.Usuario.To<UsuarioDTO>();
                objRetorno.UsuarioId = objRetorno.Usuario.Id.Value;
                objRetorno.Perfil = new PerfilDTO();
                listaRetorno.Add(objRetorno);
                vetRepeticao.Add(chave, chave);
            }

            int pageCount = filtro.PaginationParameters.PageSize;
            int pageIndex = filtro.PaginationParameters.PageIndex;


            if (string.IsNullOrEmpty(filtro.PaginationParameters.OrderBy))
            {
                listaRetorno = listaRetorno.OrderBy(l => l.Usuario.Login).ThenBy(i => i.Modulo.NomeCompleto).ToList<UsuarioPerfilDTO>();
            }

            switch (filtro.PaginationParameters.OrderBy)
            {
                case "usuario":
                    if (filtro.PaginationParameters.Ascending) { listaRetorno = listaRetorno.OrderBy(l => l.Usuario.Login).ToList<UsuarioPerfilDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRetorno = listaRetorno.OrderByDescending(l => l.Usuario.Login).ToList<UsuarioPerfilDTO>();}
                    break;
                case "modulo":
                    if (filtro.PaginationParameters.Ascending) { listaRetorno = listaRetorno.OrderBy(l => l.Modulo.NomeCompleto).ToList<UsuarioPerfilDTO>();}
                    if (!filtro.PaginationParameters.Ascending) { listaRetorno = listaRetorno.OrderByDescending(l => l.Modulo.NomeCompleto).ToList<UsuarioPerfilDTO>();}
                    break;
                case "perfil":
                    if (filtro.PaginationParameters.Ascending) { listaRetorno = listaRetorno.OrderBy(l => l.Perfil.Descricao).ToList<UsuarioPerfilDTO>();}
                    if (!filtro.PaginationParameters.Ascending) { listaRetorno = listaRetorno.OrderByDescending(l => l.Perfil.Descricao).ToList<UsuarioPerfilDTO>(); }
                    break;
                case "id":
                default:
                    break;
            }

            totalRegistros = listaRetorno.Count();
            
            return listaRetorno.Skip(pageCount * pageIndex).Take(pageCount).To<List<UsuarioPerfilDTO>>();
        }

        public bool SalvarPermissoes(int UsuarioId, int ModuloId, int? PerfilId, List<UsuarioFuncionalidadeDTO> listaFuncionalidadeDTO)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.UsuarioFuncionalidadeGravar))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            if (!ValidaSalvar(UsuarioId, ModuloId, PerfilId, listaFuncionalidadeDTO)) { return false; }

            var objUsuario = usuarioRepository.ObterPeloId(UsuarioId,
                                                           l => l.ListaUsuarioFuncionalidade,
                                                           l => l.ListaUsuarioPerfil);

            RemoverFuncionalidade(objUsuario, ModuloId);
            AdicionarFuncionalidade(objUsuario, ModuloId, listaFuncionalidadeDTO);
            RemoverPerfil(objUsuario, ModuloId);
            AdicionarPerfil(objUsuario, ModuloId, PerfilId);

            if (Validator.IsValid(objUsuario, out validationErrors))
            {
                try
                {
                    usuarioRepository.Alterar(objUsuario);
                    usuarioRepository.UnitOfWork.Commit();
                    messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                    return true;
                }
                catch (Exception exception)
                {
                    QueueExeptionMessages(exception);
                    return false;
                }
            }
            else 
            {
                messageQueue.AddRange(validationErrors, TypeMessage.Error);
                return false;
            }
        }

        public bool DeletarPermissoes(int UsuarioId, int ModuloId)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.UsuarioFuncionalidadeDeletar))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            if (!ValidaDeletar(UsuarioId, ModuloId)) { return false; }

            var objUsuario = usuarioRepository.ObterPeloId(UsuarioId, l => l.ListaUsuarioFuncionalidade, l => l.ListaUsuarioPerfil);

            RemoverFuncionalidade(objUsuario, ModuloId);
            RemoverPerfil(objUsuario, ModuloId);

            if (Validator.IsValid(objUsuario, out validationErrors))
            {
                try
                {
                    usuarioRepository.Alterar(objUsuario);
                    usuarioRepository.UnitOfWork.Commit();
                    messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                    return true;
                }
                catch (Exception exception)
                {
                    QueueExeptionMessages(exception);
                    return false;
                }
            }
            else
            {
                messageQueue.AddRange(validationErrors, TypeMessage.Error);
                return false;
            }
        }

        private void AdicionarFuncionalidade(Usuario usuario, int moduloId, List<UsuarioFuncionalidadeDTO> listaFuncionalidadeDTO)
        {
            var modulo = moduloRepository.ObterPeloId(moduloId);

            foreach (UsuarioFuncionalidadeDTO item in listaFuncionalidadeDTO)
            {
                var usuarioFuncionalidade = item.To<UsuarioFuncionalidade>();
                usuarioFuncionalidade.Usuario = usuario;
                usuarioFuncionalidade.UsuarioId = usuario.Id.Value;
                usuarioFuncionalidade.ModuloId = moduloId;
                usuarioFuncionalidade.Modulo = modulo;
                usuarioFuncionalidade.Funcionalidade = item.Funcionalidade;
                usuario.ListaUsuarioFuncionalidade.Add(usuarioFuncionalidade);
            }
        }

        private void AdicionarPerfil(Usuario usuario, int ModuloId, int? PerfilId)
        {

            var modulo = moduloRepository.ObterPeloId(ModuloId);

            if (PerfilId != null)
            {
                var perfil = perfilRepository.ObterPeloId(PerfilId);

                var usuarioPerfil = new UsuarioPerfil();
                usuarioPerfil.Usuario = usuario;
                usuarioPerfil.UsuarioId = usuario.Id.Value;
                usuarioPerfil.Modulo = modulo;
                usuarioPerfil.ModuloId = ModuloId;
                usuarioPerfil.Perfil = perfil;
                usuarioPerfil.PerfilId = PerfilId.Value;
                usuario.ListaUsuarioPerfil.Add(usuarioPerfil);
            }
        }

        private void RemoverFuncionalidade(Usuario usuario, int ModuloId)
        {
            if (usuario.ListaUsuarioFuncionalidade.Any(l => l.ModuloId == ModuloId))
            {
                for (int i = usuario.ListaUsuarioFuncionalidade.Count - 1; i >= 0; i--)
                {
                    var item = usuario.ListaUsuarioFuncionalidade.ToList()[i];
                    if (item.ModuloId == ModuloId)
                    {
                        usuarioRepository.RemoverFuncionalidade(item);
                    }
                }
            }
        }

        private void RemoverPerfil(Usuario usuario, int ModuloId)
        {
            if (usuario.ListaUsuarioPerfil.Any(l => l.ModuloId == ModuloId))
            {
                for (int i = usuario.ListaUsuarioPerfil.Count - 1; i >= 0; i--)
                {
                    var item = usuario.ListaUsuarioPerfil.ToList()[i];
                    if (item.ModuloId == ModuloId)
                    {
                        //usuario.ListaUsuarioPerfil.Remove(item);
                        usuarioRepository.RemoverPerfil(item);
                    }
                }
            }
        }

        public bool ValidaSalvar(int UsuarioId, int ModuloId, int? PerfilId, List<UsuarioFuncionalidadeDTO> listaDto)
        {
            if (!ValidaUsuario(UsuarioId)) { return false; }

            if (!ValidaModulo(ModuloId)) { return false; }

            if (PerfilId != null)
            {
                var perfil = perfilRepository.ObterPeloId(PerfilId);
                if (perfil.ModuloId != ModuloId)
                {
                    messageQueue.Add(Application.Resource.Admin.ErrorMessages.PerfilNaoPertenceAoSistemaSelecionado, TypeMessage.Error);
                    return false;
                }
            }

            if ((PerfilId == null) && (listaDto.Count == 0))
            {
                messageQueue.Add(Application.Resource.Admin.ErrorMessages.FuncionalidadesNaoMarcadas, TypeMessage.Error);
                return false;
            }

            return true;
        }

        public bool ValidaDeletar(int UsuarioId, int ModuloId)
        {
            if (!ValidaUsuario(UsuarioId)) { return false; }

            if (!ValidaModulo(ModuloId)) { return false; }

            var usuario = usuarioRepository.ObterPeloId(UsuarioId, l => l.ListaUsuarioFuncionalidade, l => l.ListaUsuarioPerfil);
           
            int qtdFuncionalidade = 0;
            int qtdPerfil = 0;

            qtdFuncionalidade = usuario.ListaUsuarioFuncionalidade.Where(l => l.ModuloId == ModuloId).Count();
            qtdPerfil = usuario.ListaUsuarioPerfil.Where(l => l.ModuloId == ModuloId).Count();

            if ((qtdFuncionalidade == 0) && (qtdPerfil == 0))
            {
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.RegistroNaoRecuperado, TypeMessage.Error);
                return false;
            }

            return true;
        }

        public bool ValidaUsuario(int UsuarioId)
        {
            if (UsuarioId == 0)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.CampoObrigatorio, "Usuário"), TypeMessage.Error);
                return false;
            }

            var usuario = usuarioRepository.ObterPeloId(UsuarioId);
            if (usuario == null)
            {
                messageQueue.Add(Application.Resource.Admin.ErrorMessages.UsuarioInexistente, TypeMessage.Error);
                return false;
            }

            return true;
        }

        public bool ValidaModulo(int ModuloId)
        {
            if (ModuloId == 0)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.CampoObrigatorio, "Módulo"), TypeMessage.Error);
                return false;
            }

            var modulo = moduloRepository.ObterPeloId(ModuloId);
            if (modulo == null)
            {
                messageQueue.Add(Application.Resource.Admin.ErrorMessages.ModuloInexistente, TypeMessage.Error);
                return false;
            }
            return true;
        }

    }
}