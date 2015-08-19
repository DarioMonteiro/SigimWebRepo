using System;
using GIR.Sigim.Application.DTO.Admin;
using GIR.Sigim.Application.Helper;
using GIR.Sigim.Application.Service.Admin;
using GIR.Sigim.Domain.Entity.Admin;
using GIR.Sigim.Domain.Repository.Admin;
using GIR.Sigim.Infrastructure.Crosscutting.Adapter;
using GIR.Sigim.Infrastructure.Data;
using GIR.Sigim.Infrastructure.Data.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Infrastructure.Data.Repository.Admin;
using GIR.Sigim.Infrastructure.Crosscutting.Security;
using GIR.Sigim.Infrastructure.Data.Repository.Sigim;
using GIR.Sigim.Domain.Repository.Sigim;

namespace GIR.Sigim.Application.Service.Test.Admin
{
    [TestClass]
    public class UsuarioAppServiceTest
    {
        private MessageQueue messageQueue;
        private IUsuarioRepository usuarioRepository;
        private ILogAcessoRepository logAcessoRepository;
        private UnitOfWork unitOfWork;
        private IUsuarioAppService usuarioService;
        private IModuloRepository moduloRepository;
        private IPerfilRepository perfilRepository;

        [TestInitialize]
        public void Initialize()
        {
            //System.Data.Entity.Database.SetInitializer<UnitOfWork>(new System.Data.Entity.DropCreateDatabaseAlways<UnitOfWork>());
            TypeAdapterFactory.SetCurrent(new AutomapperTypeAdapterFactory());
            MapperHelper.Initialise();

            AuthenticationServiceFactory.SetCurrent(new FormsAuthenticationFactory());

            messageQueue = new MessageQueue();

            unitOfWork = new UnitOfWork();
            usuarioRepository = new UsuarioRepository(unitOfWork);
            logAcessoRepository = new LogAcessoRepository(unitOfWork);
            moduloRepository = new ModuloRepository(unitOfWork);
            perfilRepository = new PerfilRepository(unitOfWork);

            usuarioService = new UsuarioAppService(usuarioRepository, logAcessoRepository, perfilRepository, moduloRepository, messageQueue);
        }

        //[TestMethod]
        //[ExpectedException(typeof(System.ArgumentNullException))]
        //public void InstanciarUsuarioServiceComRepositorioNuloCausaExcecao()
        //{
        //    IUsuarioAppService usuarioService = new UsuarioAppService(null, messageQueue);
        //}
    }
}