using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Application.DTO.OrdemCompra;
using GIR.Sigim.Application.Resource.Sigim;
using GIR.Sigim.Application.Service.Financeiro;
using GIR.Sigim.Domain.Entity.Admin;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Domain.Repository.OrdemCompra;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Infrastructure.Crosscutting.Security;

namespace GIR.Sigim.Application.Service.OrdemCompra
{
    public class ParametrosUsuarioAppService : BaseAppService, IParametrosUsuarioAppService
    {
        private IParametrosUsuarioRepository parametrosUsuarioRepository;
        private ICentroCustoAppService centroCustoService;

        public ParametrosUsuarioAppService(IParametrosUsuarioRepository parametrosUsuarioRepository, ICentroCustoAppService centroCustoService, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.parametrosUsuarioRepository = parametrosUsuarioRepository;
            this.centroCustoService = centroCustoService;
        }

        public ParametrosUsuarioDTO ObterPeloIdUsuario(int? idUsuario)
        {
            return parametrosUsuarioRepository.ObterPeloId(idUsuario, l => l.CentroCusto).To<ParametrosUsuarioDTO>();
        }

        public void Salvar(ParametrosUsuarioDTO dto)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.ParametroUsuarioOrdemCompraGravar))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return;
            }

            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var parametrosUsuario = parametrosUsuarioRepository.ObterPeloId(dto.Id);
            if (parametrosUsuario == null)
            {
                parametrosUsuario = new ParametrosUsuario() { Id = dto.Id };
                novoItem = true;
            }

            parametrosUsuario.Email = dto.Email;

            if (string.IsNullOrEmpty(dto.Email))
                parametrosUsuario.Senha = string.Empty;
            else if (!string.IsNullOrEmpty(dto.Senha))
                parametrosUsuario.Senha = dto.Senha;


            if (string.IsNullOrEmpty(dto.CentroCusto.Codigo))
                parametrosUsuario.CodigoCentroCusto = null;
            else
            {
                var centroCusto = centroCustoService.ObterPeloCodigo(dto.CentroCusto.Codigo);

                if (!centroCustoService.EhCentroCustoUltimoNivelValido(centroCusto))
                    return;

                if (!centroCustoService.UsuarioPossuiAcessoCentroCusto(dto.CentroCusto, dto.Id, NomeModulo.OrdemCompra))
                    return;

                parametrosUsuario.CodigoCentroCusto = centroCusto.Codigo;
            }

            if (Validator.IsValid(parametrosUsuario, out validationErrors))
            {
                if (novoItem)
                    parametrosUsuarioRepository.Inserir(parametrosUsuario);
                else
                    parametrosUsuarioRepository.Alterar(parametrosUsuario);

                parametrosUsuarioRepository.UnitOfWork.Commit();

                messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
            }
            else
                messageQueue.AddRange(validationErrors, TypeMessage.Error);
        }

        public bool EhPermitidoSalvar()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.ParametroUsuarioOrdemCompraGravar);
        }
    }
}