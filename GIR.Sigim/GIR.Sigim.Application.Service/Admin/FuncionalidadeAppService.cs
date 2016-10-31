using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO;
using GIR.Sigim.Domain.Repository.Admin;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Infrastructure.Crosscutting.Security;
using GIR.Sigim.Application.DTO.Admin;
using GIR.Sigim.Domain.Entity.Admin;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Application.Filtros.Admin;
using GIR.Sigim.Application.Constantes;

namespace GIR.Sigim.Application.Service.Admin
{
    public class FuncionalidadeAppService : BaseAppService, IFuncionalidadeAppService
    {
        private IModuloAppService moduloAppService;

        public FuncionalidadeAppService(IModuloAppService moduloAppService, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.moduloAppService = moduloAppService;
        }

        #region IFuncionalidadeAppService Members

        public List<FuncionalidadeDTO> ListarPeloModulo(int moduloId)
        {
            ModuloDTO modulo = new ModuloDTO();
            List<FuncionalidadeDTO> listaFuncionalidade = new List<FuncionalidadeDTO>();
            if (moduloId != 0) 
            {
                modulo = moduloAppService.ObterPeloId(moduloId);

                listaFuncionalidade = RecuperaFuncionalidadePeloModulo(modulo.Nome);
            }
            
            return listaFuncionalidade;
        }

        private List<FuncionalidadeDTO> RecuperaFuncionalidadePeloModulo(string nomeModulo)
        {
            List<FuncionalidadeDTO> listaFuncionalidade = new List<FuncionalidadeDTO>();
            Funcionalidade Funcionalidade = new Funcionalidade();
            System.Collections.Hashtable Menu = new System.Collections.Hashtable();

            switch (nomeModulo)
            {
                case GIR.Sigim.Application.Constantes.Modulo.AdminWeb:
                    Menu = Funcionalidade.MenuAdmin;
                    break;

                case GIR.Sigim.Application.Constantes.Modulo.ComercialWeb:
                    Menu = Funcionalidade.MenuComercial;
                    break;

                case GIR.Sigim.Application.Constantes.Modulo.ContratoWeb:
                    Menu = Funcionalidade.MenuContrato;
                    break;

                case GIR.Sigim.Application.Constantes.Modulo.FinanceiroWeb:
                    Menu = Funcionalidade.MenuFinanceiro;
                    break;

                case GIR.Sigim.Application.Constantes.Modulo.OrcamentoWeb:
                    Menu = Funcionalidade.MenuOrcamento;
                    break;

                case GIR.Sigim.Application.Constantes.Modulo.OrdemCompraWeb: 
                    Menu = Funcionalidade.MenuOrdemCompra;
                    break;

                case GIR.Sigim.Application.Constantes.Modulo.SacWeb:
                    Menu = Funcionalidade.MenuSac;
                    break;                

                default:
                    break;
            }

            listaFuncionalidade = TransformaEmFuncionalidadeDTO(Menu);

            return listaFuncionalidade;

        }

        private List<FuncionalidadeDTO> TransformaEmFuncionalidadeDTO(System.Collections.Hashtable Menu)
        {
            FuncionalidadeDTO Funcionalidade = new FuncionalidadeDTO();
            List<FuncionalidadeDTO> listaFuncionalidade = new List<FuncionalidadeDTO>();

            if (Menu != null)
            {
                foreach (var item in Menu.Keys)
                {
                    Funcionalidade = new FuncionalidadeDTO();
                    Funcionalidade.Codigo = item.ToString();
                    Funcionalidade.Descricao = Menu[item].ToString();
                    listaFuncionalidade.Add(Funcionalidade);
                }

                listaFuncionalidade = listaFuncionalidade.OrderBy(l => l.Codigo).ToList<FuncionalidadeDTO>();
            }

            return listaFuncionalidade;

        }

        #endregion
    }
}