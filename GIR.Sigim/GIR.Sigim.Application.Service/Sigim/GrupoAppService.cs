using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Domain.Specification;

namespace GIR.Sigim.Application.Service.Sigim
{
    public class GrupoAppService : BaseAppService, IGrupoAppService
    {
        private IGrupoRepository grupoRepository;

        public GrupoAppService(IGrupoRepository GrupoRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.grupoRepository = GrupoRepository;
        }

        public List<GrupoDTO> ListarTodos()
        {
            return grupoRepository.ListarTodos().To<List<GrupoDTO>>();
        }

        public List<GrupoDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<Grupo>)new TrueSpecification<Grupo>();

            return grupoRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<GrupoDTO>>();
        }

        public GrupoDTO ObterPeloId(int? id)
        {
            return grupoRepository.ObterPeloId(id).To<GrupoDTO>();
        }

        public bool Salvar(GrupoDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var grupo = grupoRepository.ObterPeloId(dto.Id);
            if (grupo == null)
            {
                grupo = new Grupo();
                novoItem = true;
            }

            grupo.Descricao = dto.Descricao;
            grupo.Automatico = dto.Automatico;

            if (Validator.IsValid(grupo, out validationErrors))
            {
                if (novoItem)
                    grupoRepository.Inserir(grupo);
                else
                    grupoRepository.Alterar(grupo);

                grupoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                return true;
            }
            else
                messageQueue.AddRange(validationErrors, TypeMessage.Error);

            return false;
        }

        public bool Deletar(int? id)
        {
            if (id == null)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
                return false;
            }

            var grupo = grupoRepository.ObterPeloId(id);

            try
            {
                grupoRepository.Remover(grupo);
                grupoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, grupo.Descricao), TypeMessage.Error);
                return false;
            }
        }
    }
}