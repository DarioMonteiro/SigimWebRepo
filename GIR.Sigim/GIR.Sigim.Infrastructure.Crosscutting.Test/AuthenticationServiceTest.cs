using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using GIR.Sigim.Infrastructure.Crosscutting.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GIR.Sigim.Infrastructure.Crosscutting.Test
{
    [TestClass]
    public class AuthenticationServiceTest
    {
        private IAuthenticationService authenticationService;

        public AuthenticationServiceTest()
        {
            CustomWorkerRequest workerRequest = new CustomWorkerRequest();
            HttpContext context = new HttpContext(workerRequest);
            HttpContext.Current = context;
        }

        //[TestMethod]
        //[ExpectedException(typeof(System.NullReferenceException))]
        //public void CriarInstanciaDeIAuthenticationServiceSemFactoryDefinidaCausaExcecao()
        //{
        //    authenticationService = AuthenticationServiceFactory.Create();
        //}

        [TestMethod]
        public void EfetuarLoginObterUsuarioLogado()
        {
            AuthenticationServiceFactory.SetCurrent(new FormsAuthenticationFactory());
            authenticationService = AuthenticationServiceFactory.Create();

            var serializeModel = new CustomPrincipalSerializeModel();
            serializeModel.Id = 1;
            serializeModel.Nome = "Wilson Marques";
            serializeModel.Login = "wilson";
            serializeModel.Roles = new string[] { "Gerente", "Coordenador", "Administrador" };

            var serializer = new JavaScriptSerializer();
            var userData = serializer.Serialize(serializeModel);

            authenticationService.Login(serializeModel.Nome, true, userData, 20);
            authenticationService.PostAuthenticateRequest();
            var user = authenticationService.GetUser();
            Assert.IsNotNull(user);
            Assert.IsInstanceOfType(user, typeof(CustomPrincipal));
            Assert.AreEqual("wilson", user.Login);
            Assert.IsTrue(user.IsInRole("Gerente"));
        }
    }
}