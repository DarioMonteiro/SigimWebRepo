using System;
using AutoMapper;
using GIR.Sigim.Infrastructure.Crosscutting.Adapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GIR.Sigim.Infrastructure.Crosscutting.Test
{
    [TestClass]
    public class AutomapperTypeAdapterTest
    {
        //[TestMethod]
        //[ExpectedException(typeof(System.NullReferenceException))]
        //public void CriarInstanciaDeITypeAdapterSemFactoryDefinidaCausaExcecao()
        //{
        //    ITypeAdapter adapter = TypeAdapterFactory.Create();
        //}

        [TestMethod]
        public void MapperTest()
        {
            TypeAdapterFactory.SetCurrent(new AutomapperTypeAdapterFactory());
            ITypeAdapter adapter = TypeAdapterFactory.Create();
            Mapper.CreateMap<Usuario, UsuarioDTO>();

            var usuario = new Usuario { Id = 10, Nome = "Wilson Marques" };
            var usuarioDTO = adapter.Adapt<Usuario, UsuarioDTO>(usuario);

            Assert.IsInstanceOfType(usuarioDTO, typeof(UsuarioDTO));
            Assert.AreEqual(usuario.Id, usuarioDTO.Id);
        }

        class Usuario
        {
            public int Id { get; set; }
            public string Nome { get; set; }
        }

        class UsuarioDTO
        {
            public int Id { get; set; }
            public string Nome { get; set; }
        }
    }
}