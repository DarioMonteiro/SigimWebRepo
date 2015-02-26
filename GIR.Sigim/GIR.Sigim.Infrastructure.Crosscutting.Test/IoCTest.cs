using System;
using GIR.Sigim.Infrastructure.Crosscutting.IoC;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GIR.Sigim.Infrastructure.Crosscutting.Test
{
    [TestClass]
    public class IoCTest
    {
        [TestMethod]
        public void ResolveDependenceTest()
        {
            var currentContainer = Container.Current;
            Container.Current.RegisterType<IUsuarioService, UsuarioService>();
            var usuario = new Usuario() { Id = 1, Nome = "Wilson", Sobrenome = "Marques" };
            var usuarioService = currentContainer.Resolve<IUsuarioService>();
            Assert.AreEqual("Wilson Marques", usuarioService.NomeCompleto(usuario), false);
        }

        interface IUsuarioService
        {
            string NomeCompleto(Usuario usuario);
        }

        class UsuarioService : IUsuarioService
        {
            #region IUsuarioService Members

            public string NomeCompleto(Usuario usuario)
            {
                return usuario.Nome + " " + usuario.Sobrenome;
            }

            #endregion
        }

        class Usuario
        {
            public int Id { get; set; }
            public string Nome { get; set; }
            public string Sobrenome { get; set; }
        }
    }
}