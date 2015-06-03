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
    public class ParentescoAppService : BaseAppService, IParentescoAppService
    {
        private IParentescoRepository parentescoRepository;

        public ParentescoAppService(IParentescoRepository ParentescoRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.parentescoRepository = ParentescoRepository;
        }

        public List<ParentescoDTO> ListarTodos()
        {
            return parentescoRepository.ListarTodos().To<List<ParentescoDTO>>();
        }

        public List<ParentescoDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<Parentesco>)new TrueSpecification<Parentesco>();

            return parentescoRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<ParentescoDTO>>();
        }

        public ParentescoDTO ObterPeloId(int? id)
        {
            return parentescoRepository.ObterPeloId(id).To<ParentescoDTO>();
        }

        public bool Salvar(ParentescoDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var parentesco = parentescoRepository.ObterPeloId(dto.Id);
            if (parentesco == null)
            {
                parentesco = new Parentesco();
                novoItem = true;
            }

            parentesco.Descricao = dto.Descricao;
            parentesco.Automatico = dto.Automatico;

            if (Validator.IsValid(parentesco, out validationErrors))
            {
                if (novoItem)
                    parentescoRepository.Inserir(parentesco);
                else
                    parentescoRepository.Alterar(parentesco);

                parentescoRepository.UnitOfWork.Commit();
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

            var parentesco = parentescoRepository.ObterPeloId(id);

            try
            {
                parentescoRepository.Remover(parentesco);
                parentescoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, parentesco.Descricao), TypeMessage.Error);
                return false;
            }
        }
    }
}