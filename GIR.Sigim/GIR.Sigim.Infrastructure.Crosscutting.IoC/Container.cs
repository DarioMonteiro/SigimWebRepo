﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Service;
using GIR.Sigim.Application.Service.Admin;
using GIR.Sigim.Application.Service.Contrato;
using GIR.Sigim.Application.Service.Financeiro;
using GIR.Sigim.Application.Service.Orcamento;
using GIR.Sigim.Application.Service.OrdemCompra;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Domain.Repository.Admin;
using GIR.Sigim.Domain.Repository.Contrato;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Domain.Repository.Orcamento;
using GIR.Sigim.Domain.Repository.OrdemCompra;
using GIR.Sigim.Domain.Repository.Sigim;
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
using GIR.Sigim.Infrastructure.Data.Repository.Sigim;
using Microsoft.Practices.Unity;

namespace GIR.Sigim.Infrastructure.Crosscutting.IoC
{
    public static class Container
    {
        #region Properties

        static IUnityContainer currentContainer;
        public static IUnityContainer Current
        {
            get
            {
                return currentContainer;
            }
        }

        #endregion

        #region Constructor

        static Container()
        {
            ConfigureContainer();
            ConfigureFactories();
        }

        #endregion

        #region Methods

        static void ConfigureContainer()
        {
            currentContainer = new UnityContainer();

            currentContainer.RegisterType(typeof(MessageQueue), new PerResolveLifetimeManager());

            #region Unit of Work and repositories
            currentContainer.RegisterType(typeof(UnitOfWork), new PerResolveLifetimeManager());

            #region Admin
            currentContainer.RegisterType<IUsuarioRepository, UsuarioRepository>();
            #endregion

            #region Contrato
            currentContainer.RegisterType<IParametrosContratoRepository, ParametrosContratoRepository>();
            #endregion

            #region Financeiro
            currentContainer.RegisterType<ICentroCustoRepository, CentroCustoRepository>();
            currentContainer.RegisterType<IClasseRepository, ClasseRepository>();
            currentContainer.RegisterType<ITipoCompromissoRepository, TipoCompromissoRepository>();
            currentContainer.RegisterType<IMotivoCancelamentoRepository, MotivoCancelamentoRepository>();
            currentContainer.RegisterType<IParametrosUsuarioFinanceiroRepository, ParametrosUsuarioFinanceiroRepository>();
            currentContainer.RegisterType<IParametrosFinanceiroRepository, ParametrosFinanceiroRepository>();           
            #endregion

            #region Orçamento
            currentContainer.RegisterType<IOrcamentoRepository, OrcamentoRepository>();
            currentContainer.RegisterType<IParametrosOrcamentoRepository, ParametrosOrcamentoRepository>();
            #endregion

            #region Ordem de Compra
            currentContainer.RegisterType<IParametrosOrdemCompraRepository, ParametrosOrdemCompraRepository>();
            currentContainer.RegisterType<IParametrosUsuarioRepository, ParametrosUsuarioRepository>();
            currentContainer.RegisterType<IPreRequisicaoMaterialRepository, PreRequisicaoMaterialRepository>();
            currentContainer.RegisterType<IRequisicaoMaterialRepository, RequisicaoMaterialRepository>();
            #endregion

            #region Sigim
            currentContainer.RegisterType<IAssuntoContatoRepository, AssuntoContatoRepository>();
            currentContainer.RegisterType<IBancoLayoutRepository, BancoLayoutRepository>();
            currentContainer.RegisterType<IClienteFornecedorRepository, ClienteFornecedorRepository>();
            currentContainer.RegisterType<IMaterialRepository, MaterialRepository>();
            #endregion

            #endregion

            #region Adapters
            currentContainer.RegisterType<ITypeAdapterFactory, AutomapperTypeAdapterFactory>(new ContainerControlledLifetimeManager());
            #endregion

            #region Domain Services
            #endregion

            #region Application services
            #region Admin
            currentContainer.RegisterType<IUsuarioAppService, UsuarioAppService>();
            #endregion

            #region Contrato
            currentContainer.RegisterType<IParametrosContratoAppService, ParametrosContratoAppService>();
            #endregion

            #region Financeiro
            currentContainer.RegisterType<ICentroCustoAppService, CentroCustoAppService>();
            currentContainer.RegisterType<IClasseAppService, ClasseAppService>();
            currentContainer.RegisterType<ITipoCompromissoAppService, TipoCompromissoAppService>();
            currentContainer.RegisterType<IMotivoCancelamentoAppService, MotivoCancelamentoAppService>();
            currentContainer.RegisterType<IParametrosUsuarioFinanceiroAppService, ParametrosUsuarioFinanceiroAppService>();
            currentContainer.RegisterType<IParametrosFinanceiroAppService, ParametrosFinanceiroAppService>();
            #endregion

            #region Orçamento
            currentContainer.RegisterType<IOrcamentoAppService, OrcamentoAppService>();
            currentContainer.RegisterType<IParametrosOrcamentoAppService, ParametrosOrcamentoAppService>();
            #endregion

            #region Ordem de Compra
            currentContainer.RegisterType<IParametrosOrdemCompraAppService, ParametrosOrdemCompraAppService>();
            currentContainer.RegisterType<IParametrosUsuarioAppService, ParametrosUsuarioAppService>();
            currentContainer.RegisterType<IPreRequisicaoMaterialAppService, PreRequisicaoMaterialAppService>();
            currentContainer.RegisterType<IRequisicaoMaterialAppService, RequisicaoMaterialAppService>();
            #endregion

            #region Sigim
            currentContainer.RegisterType<IAssuntoContatoAppService, AssuntoContatoAppService>();
            currentContainer.RegisterType<IBancoLayoutAppService, BancoLayoutAppService>();
            currentContainer.RegisterType<IClienteFornecedorAppService, ClienteFornecedorAppService>();
            currentContainer.RegisterType<IMaterialAppService, MaterialAppService>();
            #endregion

            #endregion
        }

        static void ConfigureFactories()
        {
            //LoggerFactory.SetCurrent(new TraceSourceLogFactory());
            EntityValidatorFactory.SetCurrent(new DataAnnotationsEntityValidatorFactory());

            AuthenticationServiceFactory.SetCurrent(new FormsAuthenticationFactory());

            var typeAdapterFactory = currentContainer.Resolve<ITypeAdapterFactory>();
            TypeAdapterFactory.SetCurrent(typeAdapterFactory);
        }

        #endregion
    }
}