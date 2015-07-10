using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Resource;
using GIR.Sigim.Application.Service.Admin;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Repository.Admin;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public class CentroCustoAppService : BaseAppService, ICentroCustoAppService
    {
        private ICentroCustoRepository centroCustoRepository;
        private IUsuarioAppService usuarioAppService;

        public CentroCustoAppService(ICentroCustoRepository centroCustoRepository,
            IUsuarioAppService usuarioAppService,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.centroCustoRepository = centroCustoRepository;
            this.usuarioAppService = usuarioAppService;
        }

        #region ICentroCustoService Members

        public CentroCustoDTO ObterPeloCodigo(string codigo)
        {
            return centroCustoRepository.ObterPeloCodigo(codigo, l => l.ListaFilhos).To<CentroCustoDTO>();
        }

        public bool EhCentroCustoValido(CentroCustoDTO centroCusto)
        {
            if (centroCusto == null)
            {
                messageQueue.Add(Resource.Financeiro.ErrorMessages.CentroCustoNaoCadastrado, TypeMessage.Error);
                return false;
            }

            if (!centroCusto.Ativo)
            {
                messageQueue.Add(Resource.Financeiro.ErrorMessages.CentroCustoInativo, TypeMessage.Error);
                return false;
            }

            return true;
        }

        public bool EhCentroCustoUltimoNivelValido(CentroCustoDTO centroCusto)
        {
            if (!EhCentroCustoValido(centroCusto))
                return false;

            var filhosCentroCusto = centroCustoRepository.ListarPeloFiltro(l => l.Codigo.StartsWith(centroCusto.Codigo));
            if (filhosCentroCusto.Count() > 1)
            {
                messageQueue.Add(Resource.Financeiro.ErrorMessages.CentroCustoUltimoNivel, TypeMessage.Error);
                return false;
            }

            return true;
        }

        public bool UsuarioPossuiAcessoCentroCusto(CentroCustoDTO centroCustoDTO, int? idUsuario, string modulo)
        {
            string codigoCentroCusto = string.Empty;
            if (centroCustoDTO != null)
                codigoCentroCusto = centroCustoDTO.Codigo;

            if (usuarioAppService.UsuarioPossuiCentroCustoDefinidoNoModulo(idUsuario, modulo))
            {
                var centroCusto = centroCustoRepository.ObterPeloCodigo(codigoCentroCusto, l => l.ListaUsuarioCentroCusto) ?? new CentroCusto();
                if (!centroCusto.UsuarioPossuiAcesso(idUsuario, modulo))
                {
                    messageQueue.Add(Resource.Financeiro.ErrorMessages.UsuarioSemAcessoCentroCusto, TypeMessage.Error);
                    return false;
                }
            }

            return true;
        }

        public List<TreeNodeDTO> ListarRaizesAtivas()
        {
            var lista = centroCustoRepository.ListarRaizesAtivas().To<List<TreeNodeDTO>>();
            return RemoverNosInativos(lista);
        }

        public byte[] ObterIconeRelatorioPeloCentroCusto(string codigo)
        {
            var centroCusto = centroCustoRepository.ObterPeloCodigo(codigo, l => l.ListaCentroCustoEmpresa);
            return ObterIconeRelatorio(centroCusto);
        }

        #endregion

        private byte[] ObterIconeRelatorio(CentroCusto centroCusto)
        {
            if (centroCusto == null)
                return null;
            else
            {
                var centroCustoEmpresa = centroCusto.ListaCentroCustoEmpresa.FirstOrDefault();
                if ((centroCustoEmpresa != null) && (centroCustoEmpresa.IconeRelatorio != null))
                    return centroCustoEmpresa.IconeRelatorio;
                else
                    return ObterIconeRelatorio(centroCusto.CentroCustoPai);
            }
        }

        private List<TreeNodeDTO> RemoverNosInativos(List<TreeNodeDTO> arvore)
        {
            for (int i = arvore.Count - 1; i >= 0; i--)
            {
                if (arvore[i].Ativo)
                    arvore[i].ListaFilhos = RemoverNosInativos(arvore[i].ListaFilhos.ToList());
                else
                    arvore.Remove(arvore[i]);
            }
            return arvore;
        }
    }
}