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
    public class InteresseBairroAppService : BaseAppService, IInteresseBairroAppService
    {
        private IInteresseBairroRepository interesseBairroRepository;

        public InteresseBairroAppService(IInteresseBairroRepository InteresseBairroRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.interesseBairroRepository = InteresseBairroRepository;
        }

        public List<InteresseBairroDTO> ListarTodos()
        {
            return interesseBairroRepository.ListarTodos().To<List<InteresseBairroDTO>>();
        }

        public List<InteresseBairroDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<InteresseBairro>)new TrueSpecification<InteresseBairro>();

            return interesseBairroRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<InteresseBairroDTO>>();
        }

        public InteresseBairroDTO ObterPeloId(int? id)
        {
            return interesseBairroRepository.ObterPeloId(id).To<InteresseBairroDTO>();
        }

        public bool Salvar(InteresseBairroDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var interesseBairro = interesseBairroRepository.ObterPeloId(dto.Id);
            if (interesseBairro == null)
            {
                interesseBairro = new InteresseBairro();
                novoItem = true;
            }

            interesseBairro.Descricao = dto.Descricao;
            interesseBairro.Automatico = dto.Automatico;

            if (Validator.IsValid(interesseBairro, out validationErrors))
            {
                if (novoItem)
                    interesseBairroRepository.Inserir(interesseBairro);
                else
                    interesseBairroRepository.Alterar(interesseBairro);

                interesseBairroRepository.UnitOfWork.Commit();
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

            var interesseBairro = interesseBairroRepository.ObterPeloId(id);

            try
            {
                interesseBairroRepository.Remover(interesseBairro);
                interesseBairroRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, interesseBairro.Descricao), TypeMessage.Error);
                return false;
            }
        }
    }
}