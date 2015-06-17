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
    public class AssuntoContatoAppService : BaseAppService, IAssuntoContatoAppService
    {
        private IAssuntoContatoRepository assuntoContatoRepository;

        public AssuntoContatoAppService(IAssuntoContatoRepository AssuntoContatoRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.assuntoContatoRepository = AssuntoContatoRepository;
        }

        public List<AssuntoContatoDTO> ListarTodos()
        {
            return assuntoContatoRepository.ListarTodos().To<List<AssuntoContatoDTO>>();
        }

        public List<AssuntoContatoDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<AssuntoContato>)new TrueSpecification<AssuntoContato>();

            return assuntoContatoRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<AssuntoContatoDTO>>();
        }

        public AssuntoContatoDTO ObterPeloId(int? id)
        {
            return assuntoContatoRepository.ObterPeloId(id).To<AssuntoContatoDTO>();
        }

        public bool Salvar(AssuntoContatoDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var assuntoContato = assuntoContatoRepository.ObterPeloId(dto.Id);
            if (assuntoContato == null)
            {
                assuntoContato = new AssuntoContato();
                novoItem = true;
            }

            assuntoContato.Descricao = dto.Descricao;
            assuntoContato.Automatico = dto.Automatico;

            if (Validator.IsValid(assuntoContato, out validationErrors))
            {
                if (novoItem)
                    assuntoContatoRepository.Inserir(assuntoContato);
                else
                    assuntoContatoRepository.Alterar(assuntoContato);

                assuntoContatoRepository.UnitOfWork.Commit();
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

            var assuntoContato = assuntoContatoRepository.ObterPeloId(id);

            try
            {
                assuntoContatoRepository.Remover(assuntoContato);
                assuntoContatoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, assuntoContato.Descricao), TypeMessage.Error);
                return false;
            }
        }
    }
}