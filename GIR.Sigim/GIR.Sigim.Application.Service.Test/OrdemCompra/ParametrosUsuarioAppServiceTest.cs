using System;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.OrdemCompra;
using GIR.Sigim.Application.Helper;
using GIR.Sigim.Application.Service.Admin;
using GIR.Sigim.Application.Service.Financeiro;
using GIR.Sigim.Application.Service.OrdemCompra;
using GIR.Sigim.Domain.Repository.Admin;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Domain.Repository.OrdemCompra;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Domain.Resource.OrdemCompra;
using GIR.Sigim.Infrastructure.Crosscutting.Adapter;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Infrastructure.Crosscutting.Security;
using GIR.Sigim.Infrastructure.Crosscutting.Validator;
using GIR.Sigim.Infrastructure.Data;
using GIR.Sigim.Infrastructure.Data.Repository.Admin;
using GIR.Sigim.Infrastructure.Data.Repository.Financeiro;
using GIR.Sigim.Infrastructure.Data.Repository.OrdemCompra;
using GIR.Sigim.Infrastructure.Data.Repository.Sigim;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GIR.Sigim.Application.Service.Test.OrdemCompra
{
    [TestClass]
    public class ParametrosUsuarioAppServiceTest
    {
        private IParametrosUsuarioRepository parametrosUsuarioRepository;
        private ICentroCustoRepository centroCustoRepository;
        private IUsuarioRepository usuarioRepository;
        private ILogAcessoRepository logAcessoRepository;
        private IUsuarioAppService usuarioAppService;
        private ICentroCustoAppService centroCustoService;
        private IModuloRepository moduloRepository;
        private IPerfilRepository perfilRepository;
        private MessageQueue messageQueue;

        [TestInitialize]
        public void Initialize()
        {
            AuthenticationServiceFactory.SetCurrent(new FormsAuthenticationFactory());
            TypeAdapterFactory.SetCurrent(new AutomapperTypeAdapterFactory());
            MapperHelper.Initialise();

            EntityValidatorFactory.SetCurrent(new DataAnnotationsEntityValidatorFactory());

            var unitOfWork = new UnitOfWork();
            parametrosUsuarioRepository = new ParametrosUsuarioRepository(unitOfWork);
            usuarioRepository = new UsuarioRepository(unitOfWork);
            logAcessoRepository = new LogAcessoRepository(unitOfWork);
            centroCustoRepository = new CentroCustoRepository(unitOfWork);
            moduloRepository = new ModuloRepository(unitOfWork);
            perfilRepository = new PerfilRepository(unitOfWork);
            messageQueue = new MessageQueue();
            usuarioAppService = new UsuarioAppService(usuarioRepository, logAcessoRepository, perfilRepository, moduloRepository, messageQueue);
            centroCustoService = new CentroCustoAppService(centroCustoRepository, usuarioAppService, messageQueue);
        }

        [TestMethod]
        public void SalvarParametrosUsuario_Completo_Success()
        {
            var service = new ParametrosUsuarioAppService(parametrosUsuarioRepository, centroCustoService, messageQueue);
            var dto = new ParametrosUsuarioDTO();
            dto.CentroCusto = new CentroCustoDTO();
            dto.Id = 2;
            dto.CentroCusto.Codigo = "1.00";
            dto.Email = "fulano.tal@gmail.com";
            dto.Senha = "1234";
            service.Salvar(dto);
            var resultDTO = service.ObterPeloIdUsuario(2);
            Assert.AreEqual(dto.Email, resultDTO.Email);
            Assert.AreEqual(dto.Senha, resultDTO.Senha);
            Assert.AreEqual(dto.CentroCusto.Codigo, resultDTO.CentroCusto.Codigo);
            Assert.AreEqual(TypeMessage.Success, messageQueue.GetAll()[0].Type);
            Assert.AreEqual(Resource.Sigim.SuccessMessages.SalvoComSucesso, messageQueue.GetAll()[0].Text);
        }

        [TestMethod]
        public void SalvarParametrosUsuario_EmailDiferente_Success()
        {
            var service = new ParametrosUsuarioAppService(parametrosUsuarioRepository, centroCustoService, messageQueue);
            var dto = new ParametrosUsuarioDTO();
            dto.CentroCusto = new CentroCustoDTO();
            dto.Id = 2;
            dto.CentroCusto.Codigo = "1.00";
            dto.Email = "fulano.tal.da.silva@gmail.com";
            service.Salvar(dto);
            var resultDTO = service.ObterPeloIdUsuario(2);
            Assert.AreEqual(dto.Email, resultDTO.Email);
            Assert.AreNotEqual(dto.Senha, resultDTO.Senha);
        }

        [TestMethod]
        public void SalvarParametrosUsuario_SemEmail_Success()
        {
            var service = new ParametrosUsuarioAppService(parametrosUsuarioRepository, centroCustoService, messageQueue);
            var dto = new ParametrosUsuarioDTO();
            dto.CentroCusto = new CentroCustoDTO();
            dto.Id = 2;
            dto.CentroCusto.Codigo = "1.00";
            service.Salvar(dto);
            var resultDTO = service.ObterPeloIdUsuario(2);
            Assert.AreEqual(dto.Email, resultDTO.Email);
            Assert.IsTrue(string.IsNullOrEmpty(resultDTO.Senha));
        }

        [TestMethod]
        public void SalvarParametrosUsuario_ComEmailSemSenha_Error()
        {
            SalvarParametrosUsuario_SemEmail_Success();
            messageQueue = new MessageQueue();
            var service = new ParametrosUsuarioAppService(parametrosUsuarioRepository, centroCustoService, messageQueue);
            var dto = new ParametrosUsuarioDTO();
            dto.CentroCusto = new CentroCustoDTO();
            dto.Id = 2;
            dto.Email = "fulano.tal@gmail.com";
            dto.CentroCusto.Codigo = "1.00";
            service.Salvar(dto);
            Assert.AreEqual(1, messageQueue.GetAll().Count);
            Assert.AreEqual(TypeMessage.Error, messageQueue.GetAll()[0].Type);
            Assert.AreEqual(Domain.Resource.OrdemCompra.ErrorMessages.SenhaDoEmailObrigatoria, messageQueue.GetAll()[0].Text);
        }

        [TestMethod]
        public void SalvarParametrosUsuario_SemCentroCusto_Success()
        {
            var service = new ParametrosUsuarioAppService(parametrosUsuarioRepository, centroCustoService, messageQueue);
            var dto = new ParametrosUsuarioDTO();
            dto.CentroCusto = new CentroCustoDTO();
            dto.Id = 2;
            dto.Email = "fulano.tal@gmail.com";
            dto.Senha = "1234";
            service.Salvar(dto);
            var resultDTO = service.ObterPeloIdUsuario(2);
            Assert.IsNull(resultDTO.CentroCusto);
        }

        [TestMethod]
        public void SalvarParametrosUsuario_SemRestricaoUsuarioCentroCustoDefinida_Success()
        {
            var service = new ParametrosUsuarioAppService(parametrosUsuarioRepository, centroCustoService, messageQueue);
            var dto = new ParametrosUsuarioDTO();
            dto.CentroCusto = new CentroCustoDTO();
            dto.Id = 1;
            dto.CentroCusto.Codigo = "1.00";
            dto.Email = "wilson.marques@gmail.com";
            dto.Senha = "1234";
            service.Salvar(dto);
            var resultDTO = service.ObterPeloIdUsuario(1);
            Assert.IsNotNull(resultDTO.CentroCusto);
            Assert.AreEqual(TypeMessage.Success, messageQueue.GetAll()[0].Type);
            Assert.AreEqual(Resource.Sigim.SuccessMessages.SalvoComSucesso, messageQueue.GetAll()[0].Text);
        }

        [TestMethod]
        public void SalvarParametrosUsuario_CentroCustoNaoExiste_Error()
        {
            var service = new ParametrosUsuarioAppService(parametrosUsuarioRepository, centroCustoService, messageQueue);
            var dto = new ParametrosUsuarioDTO();
            dto.CentroCusto = new CentroCustoDTO();
            dto.Id = 2;
            dto.CentroCusto.Codigo = "1.00.05";
            dto.Email = "fulano.tal@gmail.com";
            dto.Senha = "1234";
            service.Salvar(dto);
            var resultDTO = service.ObterPeloIdUsuario(2);
            Assert.AreEqual(1, messageQueue.GetAll().Count);
            Assert.AreEqual(TypeMessage.Error, messageQueue.GetAll()[0].Type);
            Assert.AreEqual(Resource.Financeiro.ErrorMessages.CentroCustoNaoCadastrado, messageQueue.GetAll()[0].Text);
        }

        [TestMethod]
        public void SalvarParametrosUsuario_CentroCustoInativo_Error()
        {
            var service = new ParametrosUsuarioAppService(parametrosUsuarioRepository, centroCustoService, messageQueue);
            var dto = new ParametrosUsuarioDTO();
            dto.CentroCusto = new CentroCustoDTO();
            dto.Id = 2;
            dto.CentroCusto.Codigo = "1.04";
            dto.Email = "fulano.tal@gmail.com";
            dto.Senha = "1234";
            service.Salvar(dto);
            var resultDTO = service.ObterPeloIdUsuario(2);
            Assert.AreEqual(1, messageQueue.GetAll().Count);
            Assert.AreEqual(TypeMessage.Error, messageQueue.GetAll()[0].Type);
            Assert.AreEqual(Resource.Financeiro.ErrorMessages.CentroCustoInativo, messageQueue.GetAll()[0].Text);
        }

        [TestMethod]
        public void SalvarParametrosUsuario_CentroCustoUltimoNivel_Error()
        {
            var service = new ParametrosUsuarioAppService(parametrosUsuarioRepository, centroCustoService, messageQueue);
            var dto = new ParametrosUsuarioDTO();
            dto.CentroCusto = new CentroCustoDTO();
            dto.Id = 1;
            dto.CentroCusto.Codigo = "1.05";
            dto.Email = "wilson.marques@gmail.com";
            dto.Senha = "1234";
            service.Salvar(dto);
            var resultDTO = service.ObterPeloIdUsuario(2);
            Assert.AreEqual(1, messageQueue.GetAll().Count);
            Assert.AreEqual(TypeMessage.Error, messageQueue.GetAll()[0].Type);
            Assert.AreEqual(Resource.Financeiro.ErrorMessages.CentroCustoUltimoNivel, messageQueue.GetAll()[0].Text);
        }

        [TestMethod]
        public void SalvarParametrosUsuario_SemAcessoCentroCusto_Error()
        {
            var service = new ParametrosUsuarioAppService(parametrosUsuarioRepository, centroCustoService, messageQueue);
            var dto = new ParametrosUsuarioDTO();
            dto.CentroCusto = new CentroCustoDTO();
            dto.Id = 2;
            dto.CentroCusto.Codigo = "1.01";
            dto.Email = "fulano.tal@gmail.com";
            dto.Senha = "1234";
            service.Salvar(dto);
            var resultDTO = service.ObterPeloIdUsuario(2);
            Assert.AreEqual(1, messageQueue.GetAll().Count);
            Assert.AreEqual(TypeMessage.Error, messageQueue.GetAll()[0].Type);
            Assert.AreEqual(Resource.Financeiro.ErrorMessages.UsuarioSemAcessoCentroCusto, messageQueue.GetAll()[0].Text);
        }
    }
}