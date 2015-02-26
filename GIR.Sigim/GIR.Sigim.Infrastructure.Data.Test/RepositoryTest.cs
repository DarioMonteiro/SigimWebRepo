using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GIR.Sigim.Domain.Entity.Admin;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Repository;
using GIR.Sigim.Domain.Repository.Admin;
using GIR.Sigim.Infrastructure.Crosscutting.Adapter;
using GIR.Sigim.Infrastructure.Crosscutting.Security;
using GIR.Sigim.Infrastructure.Data.Repository;
using GIR.Sigim.Infrastructure.Data.Repository.Admin;
using GIR.Sigim.Infrastructure.Data.Test.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GIR.Sigim.Infrastructure.Data.Test
{
    [TestClass]
    public class RepositoryTest
    {
        [TestInitialize]
        public void Initialize()
        {
            System.Data.Entity.Database.SetInitializer<UnitOfWork>(new System.Data.Entity.DropCreateDatabaseAlways<UnitOfWork>());
            TypeAdapterFactory.SetCurrent(new AutomapperTypeAdapterFactory());
        }

        //[TestMethod]
        //[ExpectedException(typeof(System.ArgumentNullException))]
        //public void InstanciarRepositorioComUnitOfWorkNulaCausaExcecao()
        //{
        //    IRepository<Usuario> repositorio = new Repository<Usuario>(null);
        //}

        [TestMethod]
        public void AdicionarModulo()
        {
            var modulo = new Modulo()
            {
                ChaveAcesso = "OC",
                Nome = Application.Resource.Sigim.NomeModulo.OrdemCompra,
                NomeCompleto = "Ordem de Compra",
                Versao = "1.0.2.3"
            };

            IRepository<Modulo> moduloRepository = new Repository<Modulo>(new UnitOfWork());
            moduloRepository.Inserir(modulo);
            moduloRepository.UnitOfWork.Commit();
            Assert.IsNotNull(modulo.Id);
        }

        //[TestMethod]
        //public void AdicionarFuncionalidades()
        //{
        //    var unitOfWork = new UnitOfWork();
        //    List<Funcionalidade> funcionalidades = new List<Funcionalidade>();
        //    funcionalidades.Add(new Funcionalidade()
        //    {
        //        Ativo = true,
        //        ChaveAcesso = "AddOrdemCompra",
        //        Descricao = "Adicinar Ordem de Compra",
        //        ModuloId = 1
        //    });

        //    funcionalidades.Add(new Funcionalidade()
        //    {
        //        Ativo = true,
        //        ChaveAcesso = "DelOrdemCompra",
        //        Descricao = "Remover Ordem de Compra",
        //        ModuloId = 1
        //    });

        //    IRepository<Funcionalidade> funcionalidadeRepository = new Repository<Funcionalidade>(unitOfWork);
        //    foreach (var item in funcionalidades)
        //        funcionalidadeRepository.Inserir(item);

        //    funcionalidadeRepository.UnitOfWork.Commit();
        //    foreach (var item in funcionalidades)
        //        Assert.IsNotNull(item.Id);
        //}

        [TestMethod]
        public void AdicionarCentroDeCusto()
        {
            var unitOfWork = new UnitOfWork();
            IRepository<CentroCusto> centroCustoRepository = new Repository<CentroCusto>(unitOfWork);
            var centroCusto = new CentroCusto()
            {
                Codigo = "1",
                Descricao = "EMPRESAS",
                TipoTabela = 1,
                Ativo = true
            };

            centroCustoRepository.Inserir(centroCusto);

            centroCusto = new CentroCusto()
            {
                Codigo = "1.00",
                CodigoPai = "1",
                Descricao = "TESTE RELATÓRIO %",
                TipoTabela = 1,
                Ativo = true
            };

            centroCustoRepository.Inserir(centroCusto);

            centroCusto = new CentroCusto()
            {
                Codigo = "1.01",
                CodigoPai = "1",
                Descricao = "Centro de Custo de Teste",
                TipoTabela = 1,
                Ativo = true
            };

            centroCustoRepository.Inserir(centroCusto);

            centroCusto = new CentroCusto()
            {
                Codigo = "1.04",
                CodigoPai = "1",
                Descricao = "Centro Inativo",
                TipoTabela = 1,
                Ativo = false
            };

            centroCustoRepository.Inserir(centroCusto);

            centroCusto = new CentroCusto()
            {
                Codigo = "1.05",
                CodigoPai = "1",
                Descricao = "Centro de Custo Com Filho",
                TipoTabela = 1,
                Ativo = true
            };

            centroCustoRepository.Inserir(centroCusto);

            centroCusto = new CentroCusto()
            {
                Codigo = "1.05.00",
                CodigoPai = "1.05",
                Descricao = "Centro de Custo de 3º Nível",
                TipoTabela = 1,
                Ativo = true
            };

            centroCustoRepository.Inserir(centroCusto);

            centroCusto = new CentroCusto()
            {
                Codigo = "2",
                CodigoPai = null,
                Descricao = "Segunda Raíz",
                TipoTabela = 1,
                Ativo = true
            };

            centroCustoRepository.Inserir(centroCusto);

            centroCusto = new CentroCusto()
            {
                Codigo = "2.01",
                CodigoPai = "2",
                Descricao = "Primeiro Filho da Segunda Raíz",
                TipoTabela = 1,
                Ativo = true
            };

            centroCustoRepository.Inserir(centroCusto);

            centroCusto = new CentroCusto()
            {
                Codigo = "2.02",
                CodigoPai = "2",
                Descricao = "Segundo Filho da Segunda Raíz",
                TipoTabela = 1,
                Ativo = true
            };

            centroCustoRepository.Inserir(centroCusto);
            centroCustoRepository.UnitOfWork.Commit();
        }

        [TestMethod]
        public void AdicionarUsuarioComTodasAsFuncionalidadesDoModuloOrdemCompra()
        {
            var unitOfWork = new UnitOfWork();
            IRepository<Usuario> usuarioRepository = new Repository<Usuario>(unitOfWork);
            IRepository<Funcionalidade> funcionalidadeRepository = new Repository<Funcionalidade>(unitOfWork);

            var usuario = new Usuario()
            {
                Ativo = true,
                Login = "wilson",
                Senha = CryptographyHelper.CreateHashPassword("1234567"),
                Nome = "Wilson Marques",
                ParametrosUsuario = new Domain.Entity.OrdemCompra.ParametrosUsuario()
                {
                    Email = "wilson.marques@gmail.com",
                    Senha = "1234",
                    CodigoCentroCusto = "1.00"
                }
            };

            //var funcionalidades = funcionalidadeRepository.ObterPeloFiltro(l => l.Modulo.Nome == "OrdemCompra");

            //foreach (var item in funcionalidades)
            //    usuario.ListaFuncionalidade.Add(item);

            usuarioRepository.Inserir(usuario);

            var usuario2 = new Usuario()
            {
                Ativo = true,
                Login = "fulano",
                Senha = CryptographyHelper.CreateHashPassword("1234567"),
                Nome = "Fulano de Tal"
                //ParametrosUsuario = new Domain.Entity.OrdemCompra.ParametrosUsuario()
                //{
                //    Email = "fulano.tal@gmail.com",
                //    Senha = "1234",
                //    CentroCustoId = 2
                //}
            };

            usuario2.ListaUsuarioCentroCusto.Add(new UsuarioCentroCusto()
            {
                CodigoCentroCusto = "1.00",
                ModuloId = 1
            });

            usuarioRepository.Inserir(usuario2);
            usuarioRepository.UnitOfWork.Commit();
            Assert.IsNotNull(usuario.Id);
        }

        //[TestMethod]
        //public void AdicionarPerfilComTodasAsFuncionalidades()
        //{
        //    var unitOfWork = new UnitOfWork();
        //    IRepository<Perfil> perfilRepository = new Repository<Perfil>(unitOfWork);
        //    IRepository<Funcionalidade> funcionalidadeRepository = new Repository<Funcionalidade>(unitOfWork);

        //    var perfil = new Perfil()
        //    {
        //        Descricao = "Administrador",
        //        ModuloId = 1
        //    };

        //    perfil.ListaFuncionalidade = funcionalidadeRepository.ObterTodos().ToList();

        //    perfilRepository.Inserir(perfil);
        //    perfilRepository.UnitOfWork.Commit();
        //    Assert.IsNotNull(perfil.Id);
        //}

        [TestMethod]
        public void ModificarLoginUsuario()
        {
            var unitOfWork = new UnitOfWork();
            IUsuarioRepository usuarioRepository = new UsuarioRepository(unitOfWork);

            var usuario = usuarioRepository.ObterPeloLogin("wilson");
            usuario.Login = "wilson.marques";

            usuarioRepository.Alterar(usuario);
            usuarioRepository.UnitOfWork.Commit();

            usuario = usuarioRepository.ObterPeloId(1);
            Assert.AreEqual("wilson.marques", usuario.Login);
        }

        //[TestMethod]
        //public void RetirarFuncionalidadeUsuario()
        //{
        //    var unitOfWork = new UnitOfWork();
        //    IRepository<Usuario> usuarioRepository = new Repository<Usuario>(unitOfWork);

        //    var usuario = usuarioRepository.ObterPeloId(1, l => l.ListaFuncionalidade);

        //    usuario.ListaFuncionalidade.Remove(usuario.ListaFuncionalidade.Where(l => l.Id == 1).SingleOrDefault());

        //    usuarioRepository.Alterar(usuario);
        //    usuarioRepository.UnitOfWork.Commit();

        //    var user = usuarioRepository.ObterPeloId(1);
        //    Assert.AreEqual(1, user.ListaFuncionalidade.Count);
        //    Assert.IsTrue(user.ListaFuncionalidade.Any());
        //}

        //[TestMethod]
        //public void AtribuirPerfilUsuario()
        //{
        //    var unitOfWork = new UnitOfWork();
        //    IRepository<Usuario> usuarioRepository = new Repository<Usuario>(unitOfWork);
        //    IRepository<Perfil> perfilRepository = new Repository<Perfil>(unitOfWork);

        //    var usuario = usuarioRepository.ObterPeloId(1);

        //    usuario.ListaPerfil.Add(perfilRepository.ObterPeloId(1));

        //    usuarioRepository.Alterar(usuario);
        //    usuarioRepository.UnitOfWork.Commit();

        //    usuario = usuarioRepository.ObterPeloId(1);
        //    Assert.IsTrue(usuario.ListaPerfil.Any());
        //}

        [TestMethod]
        public void SerializarUsuario()
        {
            var unitOfWork = new UnitOfWork();
            IRepository<Usuario> usuarioRepository = new Repository<Usuario>(unitOfWork);

            //var usuario = usuarioRepository.ObterPeloId(1, l => l.ListaFuncionalidade.Select(f => f.Modulo), l => l.ListaPerfil.Select(f => f.ListaFuncionalidade));
            var usuario = usuarioRepository.ObterPeloId(1);
            ITypeAdapter adapter = TypeAdapterFactory.Create();
            Mapper.CreateMap<Funcionalidade, FuncionalidadeDTO>();
            Mapper.CreateMap<Modulo, ModuloDTO>();
            Mapper.CreateMap<Perfil, PerfilDTO>();
            Mapper.CreateMap<Usuario, UsuarioDTO>();

            var usuarioDTO = adapter.Adapt<Usuario, UsuarioDTO>(usuario);

            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(usuarioDTO);

            Assert.IsInstanceOfType(usuarioDTO, typeof(UsuarioDTO));
            Assert.AreEqual(usuario.Id, usuarioDTO.Id);
            //Assert.IsTrue(usuarioDTO.ListaPerfil.Count > 0);
            Assert.IsTrue(json.Length > 10);
        }

        [TestMethod]
        public void ListarTodosUsuarios()
        {
            IRepository<Usuario> usuarioRepository = new Repository<Usuario>(new UnitOfWork());
            var result = usuarioRepository.ListarTodos();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ToList().Count > 0);
        }

        //[TestMethod]
        //public void RemoverFuncionalidade()
        //{
        //    var unitOfWork = new UnitOfWork();
        //    IRepository<Funcionalidade> funcionalidadeRepository = new Repository<Funcionalidade>(unitOfWork);

        //    var funcionalidade = funcionalidadeRepository.ObterPeloId(1, l => l.ListaPerfil);
        //    funcionalidade.ListaPerfil.Clear();
        //    funcionalidadeRepository.Remover(funcionalidade);
        //    funcionalidadeRepository.UnitOfWork.Commit();

        //    funcionalidade = funcionalidadeRepository.ObterPeloId(1);
        //    Assert.IsNull(funcionalidade);
        //}

        //[TestMethod]
        //public void RollbackingChanges()
        //{
        //    IRepository<Modulo> moduloRepository = new Repository<Modulo>(new UnitOfWork());

        //    var modulo = new Modulo()
        //    {
        //        Id = 1,
        //        ChaveAcesso = "OC",
        //        Nome = "OrdemCompraABC",
        //        NomeCompleto = "Ordem de Compra",
        //        Versao = "1.0.2.3"
        //    };

        //    moduloRepository.Alterar(modulo);
        //    moduloRepository.UnitOfWork.RollbackChanges();
        //    moduloRepository.UnitOfWork.Commit();

        //    modulo = moduloRepository.ObterPeloId(1);
        //    Assert.AreNotEqual("OrdemCompraABC", modulo.Nome);
        //    Assert.AreEqual(Application.Resource.Sigim.NomeModulo.OrdemCompra, modulo.Nome);
        //}
    }
}