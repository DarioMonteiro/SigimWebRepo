using System;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.OrdemCompra;
using GIR.Sigim.Application.Helper;
using GIR.Sigim.Application.Service.Contrato;
using GIR.Sigim.Application.Service.Financeiro;
using GIR.Sigim.Application.Service.Orcamento;
using GIR.Sigim.Application.Service.OrdemCompra;
using GIR.Sigim.Domain.Repository.Admin;
using GIR.Sigim.Domain.Repository.Contrato;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Domain.Repository.Orcamento;
using GIR.Sigim.Domain.Repository.OrdemCompra;
using GIR.Sigim.Infrastructure.Crosscutting.Adapter;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Infrastructure.Crosscutting.Security;
using GIR.Sigim.Infrastructure.Crosscutting.Validator;
using GIR.Sigim.Infrastructure.Data;
using GIR.Sigim.Infrastructure.Data.Repository.Admin;
using GIR.Sigim.Infrastructure.Data.Repository.Contrato;
using GIR.Sigim.Infrastructure.Data.Repository.Financeiro;
using GIR.Sigim.Infrastructure.Data.Repository.Orcamento;
using GIR.Sigim.Infrastructure.Data.Repository.OrdemCompra;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GIR.Sigim.Application.Service.Test.OrdemCompra
{
    [TestClass]
    public class ParametrosOrdemCompraAppServiceTest
    {
        private IParametrosOrdemCompraRepository parametrosOrdemCompraRepository;
        private IParametrosOrcamentoRepository parametrosOrcamentoRepository;
        private IParametrosContratoRepository parametrosContratoRepository;
        private IParametrosOrcamentoAppService parametrosOrcamentoAppService;
        private IParametrosContratoAppService parametrosContratoAppService;
        private MessageQueue messageQueue;

        [TestInitialize]
        public void Initialize()
        {
            AuthenticationServiceFactory.SetCurrent(new FormsAuthenticationFactory());
            TypeAdapterFactory.SetCurrent(new AutomapperTypeAdapterFactory());
            MapperHelper.Initialise();

            EntityValidatorFactory.SetCurrent(new DataAnnotationsEntityValidatorFactory());

            var unitOfWork = new UnitOfWork();
            parametrosOrdemCompraRepository = new Infrastructure.Data.Repository.OrdemCompra.ParametrosOrdemCompraRepository(unitOfWork);
            messageQueue = new MessageQueue();
            parametrosOrcamentoRepository = new ParametrosOrcamentoRepository(unitOfWork);
            parametrosContratoRepository = new ParametrosContratoRepository(unitOfWork);
            parametrosOrcamentoAppService = new ParametrosOrcamentoAppService(parametrosOrcamentoRepository, messageQueue);
            parametrosContratoAppService = new ParametrosContratoAppService(parametrosContratoRepository, messageQueue);
        }

        [TestMethod]
        public void SalvarParametros_Completo_Success()
        {
            var service = new ParametrosOrdemCompraAppService(parametrosOrdemCompraRepository, parametrosOrcamentoAppService, parametrosContratoAppService, messageQueue);
            var dto = new ParametrosOrdemCompraDTO();
            //dto.ClienteId = 
            dto.Responsavel = "Eduardo Campos";
            dto.MascaraClasseInsumo = "##.##.##.##.##.##";
            dto.IconeRelatorio = null;
            //dto.AssuntoContatoId = 
            dto.GeraTituloAguardando = true;
            dto.GeraProvisionamentoNaCotacao = true;
            dto.DiasDataMinima = 5;
            dto.DiasPrazo = 5;
            dto.EhPreRequisicaoMaterial = true;
            //dto.TipoCompromissoFreteId = 
            dto.SmtpServidorSaidaEmail = "smtp.gir.srv.br";
            dto.SmtpPortaSaidaEmail = 587;
            dto.EhRequisicaoObrigatoria = true;
            dto.EhInterfaceOrcamento = true;
            dto.HabilitaSSL = true;
            dto.InibeFormaPagamento = true;
            dto.EhInterfaceContabil = true;
            //dto.InterfaceCotacao = 
            dto.DiasEntradaMaterial = 2000;
            dto.ConfereNF = false;
            dto.GravaCotacaoWeb = false;
            //dto.LayoutSPEDId = 

            service.Salvar(dto);
            var resultDTO = service.Obter();
            Assert.AreEqual(dto.Responsavel, resultDTO.Responsavel);
            Assert.IsTrue(dto.GeraTituloAguardando);
            Assert.AreEqual(TypeMessage.Success, messageQueue.GetAll()[0].Type);
            Assert.AreEqual(Resource.Sigim.SuccessMessages.SalvoComSucesso, messageQueue.GetAll()[0].Text);

            var parametrosOrcamento = parametrosOrcamentoAppService.Obter();
            Assert.AreEqual("##.##.##.##.##.##", parametrosOrcamento.MascaraClasseInsumo);

            var parametrosContrato = parametrosContratoAppService.Obter();
            Assert.AreEqual("##.##.##.##.##.##", parametrosContrato.MascaraClasseInsumo);
        }

        //[TestMethod]
        //public void ObterParametros_Success()
        //{
        //    var service = new ParametrosOrdemCompraAppService(parametrosOrdemCompraRepository, parametrosOrcamentoAppService, parametrosContratoAppService, messageQueue);
        //    var resultDTO = service.Obter();
        //    Assert.IsNotNull(resultDTO);
        //    Assert.AreEqual("Eduardo Campos", resultDTO.Responsavel);
        //    Assert.IsTrue(resultDTO.GeraTituloAguardando);
        //}
    }
}