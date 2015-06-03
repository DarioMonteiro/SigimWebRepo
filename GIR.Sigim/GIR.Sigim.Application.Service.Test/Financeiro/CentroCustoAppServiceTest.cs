using System;
using System.Linq;
using GIR.Sigim.Application.Helper;
using GIR.Sigim.Application.Service.Admin;
using GIR.Sigim.Application.Service.Financeiro;
using GIR.Sigim.Domain.Repository.Admin;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Infrastructure.Crosscutting.Adapter;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Infrastructure.Crosscutting.Validator;
using GIR.Sigim.Infrastructure.Data;
using GIR.Sigim.Infrastructure.Data.Repository.Admin;
using GIR.Sigim.Infrastructure.Data.Repository.Financeiro;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GIR.Sigim.Application.Service.Test.Financeiro
{
    [TestClass]
    public class CentroCustoAppServiceTest
    {
        private ICentroCustoRepository centroCustoRepository;
        private IUsuarioRepository usuarioRepository;
        private IUsuarioAppService usuarioAppService;
        private ICentroCustoAppService centroCustoService;
        private MessageQueue messageQueue;

        [TestInitialize]
        public void Initialize()
        {
            TypeAdapterFactory.SetCurrent(new AutomapperTypeAdapterFactory());
            MapperHelper.Initialise();
            EntityValidatorFactory.SetCurrent(new DataAnnotationsEntityValidatorFactory());

            var unitOfWork = new UnitOfWork();
            centroCustoRepository = new CentroCustoRepository(unitOfWork);
            usuarioRepository = new UsuarioRepository(unitOfWork);
            messageQueue = new MessageQueue();
            usuarioAppService = new UsuarioAppService(usuarioRepository, messageQueue);
            centroCustoService = new CentroCustoAppService(centroCustoRepository, usuarioAppService, messageQueue);
        }

        [TestMethod]
        public void ListarCentroCustoAtivos_Success()
        {
            var result = centroCustoService.ListarRaizesAtivas();
            Assert.IsFalse(result.Any(l => !l.Ativo));
        }
    }
}