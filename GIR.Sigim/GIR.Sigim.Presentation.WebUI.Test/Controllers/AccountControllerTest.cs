using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using GIR.Sigim.Application.Resource.Admin;
using GIR.Sigim.Infrastructure.Crosscutting.IoC;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Infrastructure.Crosscutting.Security;
using GIR.Sigim.Infrastructure.Crosscutting.Test;
using GIR.Sigim.Presentation.WebUI.Controllers;
using GIR.Sigim.Presentation.WebUI.IoC;
using GIR.Sigim.Presentation.WebUI.Test.TestUtilities;
using GIR.Sigim.Presentation.WebUI.ViewModel.Admin;
using Microsoft.Practices.Unity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GIR.Sigim.Presentation.WebUI.Test.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        AccountController controller;
        Dictionary<string, object> sessionState;
        HttpCookieCollection cookieCollection;

        public void Initialize(bool isAuthenticated)
        {
            sessionState = new Dictionary<string, object>();
            cookieCollection = new HttpCookieCollection();
            CustomPrincipal user = new CustomPrincipal("wilson.marques")
            {
                Id = 1,
                Nome = "Wilson Marques",
                Login = "wilson.marques"
            };

            System.Web.Fakes.ShimHttpContext.CurrentGet = () =>
            {
                return new System.Web.Fakes.ShimHttpContext()
                {
                    UserGet = () => user,
                    SessionGet = () => new System.Web.SessionState.Fakes.ShimHttpSessionState
                        {
                            ItemGetString = (key) =>
                            {
                                object result = null;
                                sessionState.TryGetValue(key, out result);
                                return result;
                            }
                        },
                    RequestGet = () => new System.Web.Fakes.ShimHttpRequest()
                        {
                            IsAuthenticatedGet = () => { return isAuthenticated; },
                            CookiesGet = () => cookieCollection,
                            BrowserGet = () => new HttpBrowserCapabilities()
                        },
                    ResponseGet = () => new System.Web.Fakes.ShimHttpResponse()
                        {
                            CookiesGet = () => cookieCollection
                        },
                };
            };

            System.Web.Mvc.Fakes.ShimController.AllInstances.UrlGet = (e) =>
            {
                return new System.Web.Mvc.Fakes.ShimUrlHelper();
            };

            AuthenticationServiceFactory.SetCurrent(new FormsAuthenticationFactory());
            Bootstrapper.Initialise();
            controller = (AccountController)Container.Current.Resolve(typeof(AccountController));
        }

        [TestMethod]
        public void JsonDeserialize()
        {
            var json = "[{\"CentroCusto\":{\"Codigo\":\"3\",\"Descricao\":\"Solar da GIR\"},\"Classe.Codigo\":\"\",\"Classe.Descricao\":\"\",\"SiglaUnidadeMedida\":\"\",\"Complemento\":\"\",\"Quantidade\":\"0,0000\",\"QuantidadeAprovada\":\"0,0000\",\"DataMinima\":\"16/02/2015\",\"Prazo\":\"10\",\"DataMaxima\":\"26/02/2015\"}]";
            var resultWithJS = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<List<GIR.Sigim.Application.DTO.OrdemCompra.PreRequisicaoMaterialItemDTO>>(json);
            var resultWithNewtonsoft = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GIR.Sigim.Application.DTO.OrdemCompra.PreRequisicaoMaterialItemDTO>>(json);
            Assert.AreEqual("3", resultWithJS[0].CentroCusto.Codigo);
            Assert.AreEqual("3", resultWithNewtonsoft[0].CentroCusto.Codigo);
        }

        [TestMethod]
        public void MyTestMethod()
        {
            //1300244400000
            //2011-03-16 00:00:00.000
            var dateTime = new DateTime(2011, 3, 16);
            long DatetimeMinTimeTicks = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;
            Assert.AreEqual(1300244400000, (dateTime.ToUniversalTime().Ticks - DatetimeMinTimeTicks) / 10000);
        }

        [TestMethod]
        public void AcessandoPaginaLogin_SemEstarLogado_Success()
        {
            using (ShimsContext.Create())
            {
                Initialize(false);
                ViewResult result = controller.Login("/Home/Index") as ViewResult;
                Assert.AreEqual("/Home/Index", result.ViewBag.ReturnUrl);
            }
        }

        [TestMethod]
        public void AcessandoPaginaLogin_EstandoLogado_Success()
        {
            using (ShimsContext.Create())
            {
                Initialize(true);
                ViewResult result = controller.Login("/Home/Index") as ViewResult;
                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public void LoginPost_UsuarioIncorreto_Error()
        {
            using (ShimsContext.Create())
            {
                Initialize(false);
                LoginViewModel model = new LoginViewModel();
                model.UserName = "abcde";
                model.Password = "1234567";
                model.RememberMe = true;

                ViewResult result = controller.Login(model, "/Home/Index") as ViewResult;
                Assert.IsNotNull(result);
                Assert.AreEqual(1, controller.messageQueue.GetAll().Count);
                Assert.AreEqual(TypeMessage.Error, controller.messageQueue.GetAll()[0].Type);
                Assert.AreEqual(controller.messageQueue.GetAll()[0].Text, ErrorMessages.UsuarioOuSenhaIncorretos);
            }
        }

        [TestMethod]
        public void LoginPost_SenhaIncorreta_Error()
        {
            using (ShimsContext.Create())
            {
                Initialize(false);
                LoginViewModel model = new LoginViewModel();
                model.UserName = "wilson.marques";
                model.Password = "abcd";
                model.RememberMe = true;

                ViewResult result = controller.Login(model, "/Home/Index") as ViewResult;
                Assert.IsNotNull(result);
                Assert.AreEqual(1, controller.messageQueue.GetAll().Count);
                Assert.AreEqual(TypeMessage.Error, controller.messageQueue.GetAll()[0].Type);
                Assert.AreEqual(ErrorMessages.UsuarioOuSenhaIncorretos, controller.messageQueue.GetAll()[0].Text);
            }
        }

        [TestMethod]
        public void LoginPost_UsuarioSenhaCorretos_Success()
        {
            using (ShimsContext.Create())
            {
                Initialize(false);
                LoginViewModel model = new LoginViewModel();
                model.UserName = "wilson.marques";
                model.Password = "1234567";
                model.RememberMe = true;

                ViewResult result = controller.Login(model, "/Home/About") as ViewResult;
                AuthenticationServiceFactory.Create().PostAuthenticateRequest();
                var user = controller.Usuario;
                Assert.IsNull(result);
                Assert.AreEqual("Wilson Marques", user.Nome);
            }
        }

        [TestMethod]
        public void AcessandoPaginaChangePassword_Success()
        {
            using (ShimsContext.Create())
            {
                Initialize(false);
                ViewResult result = controller.ChangePassword() as ViewResult;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void AlterarSenha_NovaSenhaIgualLogin_Error()
        {
            using (ShimsContext.Create())
            {
                Initialize(false);
                var model = new ChangePasswordViewModel()
                {
                    OldPassword = "1234567",
                    NewPassword = "wilson.marques",
                    ConfirmPassword = "wilson.marques"
                };

                controller.ChangePassword(model);
                Assert.AreEqual(1, controller.messageQueue.GetAll().Count);
                Assert.AreEqual(TypeMessage.Error, controller.messageQueue.GetAll()[0].Type);
                Assert.AreEqual(ErrorMessages.NovaSenhaIgualLogin, controller.messageQueue.GetAll()[0].Text);
            }
        }

        [TestMethod]
        public void AlterarSenha_ConfirmacaoDiferenteNovaSenha_Error()
        {
            using (ShimsContext.Create())
            {
                Initialize(false);
                var model = new ChangePasswordViewModel()
                {
                    OldPassword = "1234567",
                    NewPassword = "gir$2010",
                    ConfirmPassword = "gir$2011"
                };

                controller.ChangePassword(model);
                Assert.AreEqual(1, controller.messageQueue.GetAll().Count);
                Assert.AreEqual(TypeMessage.Error, controller.messageQueue.GetAll()[0].Type);
                Assert.AreEqual(ErrorMessages.ConfirmacaoNovaSenhaNaoConfere, controller.messageQueue.GetAll()[0].Text);
            }
        }

        [TestMethod]
        public void AlterarSenha_SenhaAtualIncorreta_Error()
        {
            using (ShimsContext.Create())
            {
                Initialize(false);
                var model = new ChangePasswordViewModel()
                {
                    OldPassword = "12345678",
                    NewPassword = "gir$2010",
                    ConfirmPassword = "gir$2010"
                };

                controller.ChangePassword(model);
                Assert.AreEqual(1, controller.messageQueue.GetAll().Count);
                Assert.AreEqual(TypeMessage.Error, controller.messageQueue.GetAll()[0].Type);
                Assert.AreEqual(ErrorMessages.SenhaAtualIncorreta, controller.messageQueue.GetAll()[0].Text);
            }
        }

        [TestMethod]
        public void AlterarSenha_NovaSenhaContendoEspaco_Error()
        {
            using (ShimsContext.Create())
            {
                Initialize(false);
                var model = new ChangePasswordViewModel()
                {
                    OldPassword = "1234567",
                    NewPassword = "gir$ 2010",
                    ConfirmPassword = "gir$ 2010"
                };

                controller.ChangePassword(model);
                Assert.AreEqual(1, controller.messageQueue.GetAll().Count);
                Assert.AreEqual(TypeMessage.Error, controller.messageQueue.GetAll()[0].Type);
                Assert.AreEqual(ErrorMessages.SenhaComEspacos, controller.messageQueue.GetAll()[0].Text);
            }
        }

        [TestMethod]
        public void AlterarSenha_NovaSenhaEmbranco_Error()
        {
            using (ShimsContext.Create())
            {
                Initialize(false);
                var model = new ChangePasswordViewModel()
                {
                    OldPassword = "1234567",
                    NewPassword = string.Empty,
                    ConfirmPassword = string.Empty
                };

                controller.ChangePassword(model);
                Assert.AreEqual(1, controller.messageQueue.GetAll().Count);
                Assert.AreEqual(TypeMessage.Error, controller.messageQueue.GetAll()[0].Type);
                Assert.AreEqual(ErrorMessages.SenhaEmBranco, controller.messageQueue.GetAll()[0].Text);
            }
        }

        [TestMethod]
        public void AlterarSenha_NovaSenhaComMenosDeSeteCaracteres_Error()
        {
            using (ShimsContext.Create())
            {
                Initialize(false);
                var model = new ChangePasswordViewModel()
                {
                    OldPassword = "1234567",
                    NewPassword = "gir$20",
                    ConfirmPassword = "gir$20"
                };

                controller.ChangePassword(model);
                Assert.AreEqual(1, controller.messageQueue.GetAll().Count);
                Assert.AreEqual(TypeMessage.Error, controller.messageQueue.GetAll()[0].Type);
                Assert.AreEqual(ErrorMessages.SenhaComMenosDeSeteCaracteres, controller.messageQueue.GetAll()[0].Text);
            }
        }

        [TestMethod]
        public void AlterarSenha_NovaSenhaSemNumero_Error()
        {
            using (ShimsContext.Create())
            {
                Initialize(false);
                var model = new ChangePasswordViewModel()
                {
                    OldPassword = "1234567",
                    NewPassword = "gir$sigim",
                    ConfirmPassword = "gir$sigim"
                };

                controller.ChangePassword(model);
                Assert.AreEqual(1, controller.messageQueue.GetAll().Count);
                Assert.AreEqual(TypeMessage.Error, controller.messageQueue.GetAll()[0].Type);
                Assert.AreEqual(ErrorMessages.SenhaSemNumero, controller.messageQueue.GetAll()[0].Text);
            }
        }

        [TestMethod]
        public void AlterarSenha_NovaSenhaSemLetra_Error()
        {
            using (ShimsContext.Create())
            {
                Initialize(false);
                var model = new ChangePasswordViewModel()
                {
                    OldPassword = "1234567",
                    NewPassword = "123#4567",
                    ConfirmPassword = "123#4567"
                };

                controller.ChangePassword(model);
                Assert.AreEqual(1, controller.messageQueue.GetAll().Count);
                Assert.AreEqual(TypeMessage.Error, controller.messageQueue.GetAll()[0].Type);
                Assert.AreEqual(ErrorMessages.SenhaSemLetra, controller.messageQueue.GetAll()[0].Text);
            }
        }

        [TestMethod]
        public void AlterarSenha_NovaSenhaSemCaracterEspecial_Error()
        {
            using (ShimsContext.Create())
            {
                Initialize(false);
                var model = new ChangePasswordViewModel()
                {
                    OldPassword = "1234567",
                    NewPassword = "abc@4567",
                    ConfirmPassword = "abc@4567"
                };

                controller.ChangePassword(model);
                Assert.AreEqual(1, controller.messageQueue.GetAll().Count);
                Assert.AreEqual(TypeMessage.Error, controller.messageQueue.GetAll()[0].Type);
                Assert.AreEqual(ErrorMessages.SenhaSemCaracterEspecial, controller.messageQueue.GetAll()[0].Text);
            }
        }

        [TestMethod]
        public void AlterarSenha_Success()
        {
            using (ShimsContext.Create())
            {
                Initialize(false);
                var model = new ChangePasswordViewModel()
                {
                    OldPassword = "1234567",
                    NewPassword = "gir$2010",
                    ConfirmPassword = "gir$2010"
                };

                controller.ChangePassword(model);
                Assert.AreEqual(1, controller.messageQueue.GetAll().Count);
                Assert.AreEqual(TypeMessage.Success, controller.messageQueue.GetAll()[0].Type);
                Assert.AreEqual(SuccessMessages.SenhaAlteradaComSucesso, controller.messageQueue.GetAll()[0].Text);
            }
        }

        [TestMethod]
        public void AlterarSenha_NovaSenhaIgualAntiga_Error()
        {
            using (ShimsContext.Create())
            {
                Initialize(false);
                var model = new ChangePasswordViewModel()
                {
                    OldPassword = "gir$2010",
                    NewPassword = "gir$2010",
                    ConfirmPassword = "gir$2010"
                };

                controller.ChangePassword(model);
                Assert.AreEqual(1, controller.messageQueue.GetAll().Count);
                Assert.AreEqual(TypeMessage.Error, controller.messageQueue.GetAll()[0].Type);
                Assert.AreEqual(ErrorMessages.NovaSenhaIgualAtual, controller.messageQueue.GetAll()[0].Text);
            }
        }

        [TestMethod]
        public void LogoutGet_Success()
        {
            using (ShimsContext.Create())
            {
                Initialize(true);
                var result = controller.Logout();
                Assert.IsTrue(System.Web.HttpContext.Current.Request.IsAuthenticated);
            }
        }
    }
}