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
using GIR.Sigim.Domain.Specification.Admin;
using GIR.Sigim.Application.Filtros.Admin;
using GIR.Sigim.Application.Constantes;

namespace GIR.Sigim.Application.Service.Admin
{
    public class PerfilAppService : BaseAppService, IPerfilAppService
    {
        private IPerfilRepository perfilRepository;
        private IPerfilFuncionalidadeRepository perfilFuncionalidadeRepository;
        

        public PerfilAppService(IPerfilRepository perfilRepository, IPerfilFuncionalidadeRepository perfilFuncionalidadeRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.perfilRepository = perfilRepository;
            this.perfilFuncionalidadeRepository = perfilFuncionalidadeRepository;
        }

        #region IPerfilAppService Members

        public List<PerfilDTO> ListarPeloModulo(int moduloId)
        {
            return perfilRepository.ListarPeloFiltro(l => l.ModuloId == moduloId).To<List<PerfilDTO>>();
        }

        public List<PerfilDTO> ListarPeloFiltro(PerfilFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<Perfil>)new TrueSpecification<Perfil>();

            specification &= PerfilSpecification.IgualAoModuloId(filtro.ModuloId);
            specification &= PerfilSpecification.IdMaiorQueZero();

            return perfilRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros, l => l.Modulo).To<List<PerfilDTO>>();
        }

        public PerfilDTO ObterPeloId(int? id)
        {
            return perfilRepository.ObterPeloId(id, l => l.ListaFuncionalidade).To<PerfilDTO>();
        }

        public bool Salvar(PerfilDTO dto)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.PerfilGravar))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            if (dto == null)
                throw new ArgumentNullException("dto");

            if (ValidaSalvar(dto) == false) { return false; }

            var perfilFuncionalidade = new PerfilFuncionalidade();
            bool novoItem = false;
            var perfil = perfilRepository.ObterPeloId(dto.Id, l => l.ListaFuncionalidade);
            if (perfil == null)
            {
                perfil = new Perfil();
                perfil.ModuloId = dto.ModuloId;
                novoItem = true;
            }
            else
            {
                for (int i = perfil.ListaFuncionalidade.Count - 1; i >= 0; i--)
                {
                    perfilFuncionalidadeRepository.Remover(perfil.ListaFuncionalidade.ToList()[i]);
                }

                foreach (var item in dto.ListaFuncionalidade)
                {
                    item.PerfilId = perfil.Id.Value;
                }
            }

            perfil.Descricao = dto.Descricao;
            perfil.ListaFuncionalidade = dto.ListaFuncionalidade.To<List<PerfilFuncionalidade>>();
                                    
            if (Validator.IsValid(perfil, out validationErrors))
            {
                if (novoItem)
                    perfilRepository.Inserir(perfil);
                else
                    perfilRepository.Alterar(perfil);

                perfilRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                return true;
            }
            else
                messageQueue.AddRange(validationErrors, TypeMessage.Error);

            return false;
        }

        public bool Deletar(int? id)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.PerfilDeletar))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            if (id == null)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
                return false;
            }

            var perfil = perfilRepository.ObterPeloId(id, l => l.ListaFuncionalidade);

            try
            {
                for (int i = perfil.ListaFuncionalidade.Count - 1; i >= 0; i--)
                {
                    perfilFuncionalidadeRepository.Remover(perfil.ListaFuncionalidade.ToList()[i]);
                }

                perfilRepository.Remover(perfil);
                perfilRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, perfil.Descricao), TypeMessage.Error);
                return false;
            }
        }

        public bool EhPermitidoSalvar()
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.PerfilGravar))
                return false;

            return true;
        }

        public bool EhPermitidoDeletar()
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.PerfilDeletar))
                return false;

            return true;
        }

        #endregion

        #region Métodos Privados

        public bool ValidaSalvar(PerfilDTO dto)
        {
            bool retorno = true;

            if (dto == null)
            {
                retorno = false;
                throw new ArgumentNullException("dto");
            }

            if (dto.Descricao == null)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.CampoObrigatorio, "Descrição"), TypeMessage.Error);
                retorno = false;
            }
            if (dto.Descricao == "")
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.CampoObrigatorio, "Descrição"), TypeMessage.Error);
                retorno = false;
            }
            if (!dto.Id.HasValue  && dto.ModuloId == 0)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.CampoObrigatorio, "Módulo"), TypeMessage.Error);
                retorno = false;
            }
            if (dto.ListaFuncionalidade.Count == 0)
            {
                messageQueue.Add(Application.Resource.Admin.ErrorMessages.FuncionalidadesNaoMarcadas, TypeMessage.Error);
                retorno = false;
            }

            return retorno;
        }

        #endregion
    }
}