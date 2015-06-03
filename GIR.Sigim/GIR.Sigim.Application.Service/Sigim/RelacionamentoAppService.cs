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
    public class RelacionamentoAppService : BaseAppService, IRelacionamentoAppService
    {
        private IRelacionamentoRepository relacionamentoRepository;

        public RelacionamentoAppService(IRelacionamentoRepository RelacionamentoRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.relacionamentoRepository = RelacionamentoRepository;
        }

        public List<RelacionamentoDTO> ListarTodos()
        {
            return relacionamentoRepository.ListarTodos().To<List<RelacionamentoDTO>>();
        }

        public List<RelacionamentoDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<Relacionamento>)new TrueSpecification<Relacionamento>();

            return relacionamentoRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<RelacionamentoDTO>>();
        }

        public RelacionamentoDTO ObterPeloId(int? id)
        {
            return relacionamentoRepository.ObterPeloId(id).To<RelacionamentoDTO>();
        }

        public bool Salvar(RelacionamentoDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var relacionamento = relacionamentoRepository.ObterPeloId(dto.Id);
            if (relacionamento == null)
            {
                relacionamento = new Relacionamento();
                novoItem = true;
            }

            relacionamento.Descricao = dto.Descricao;
            relacionamento.Automatico = dto.Automatico;

            if (Validator.IsValid(relacionamento, out validationErrors))
            {
                if (novoItem)
                    relacionamentoRepository.Inserir(relacionamento);
                else
                    relacionamentoRepository.Alterar(relacionamento);

                relacionamentoRepository.UnitOfWork.Commit();
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

            var relacionamento = relacionamentoRepository.ObterPeloId(id);

            try
            {
                relacionamentoRepository.Remover(relacionamento);
                relacionamentoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, relacionamento.Descricao), TypeMessage.Error);
                return false;
            }
        }
    }
}