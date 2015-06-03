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
    public class TratamentoAppService : BaseAppService, ITratamentoAppService
    {
        private ITratamentoRepository tratamentoRepository;

        public TratamentoAppService(ITratamentoRepository TratamentoRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.tratamentoRepository = TratamentoRepository;
        }

        public List<TratamentoDTO> ListarTodos()
        {
            return tratamentoRepository.ListarTodos().To<List<TratamentoDTO>>();
        }

        public List<TratamentoDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<Tratamento>)new TrueSpecification<Tratamento>();

            return tratamentoRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<TratamentoDTO>>();
        }

        public TratamentoDTO ObterPeloId(int? id)
        {
            return tratamentoRepository.ObterPeloId(id).To<TratamentoDTO>();
        }

        public bool Salvar(TratamentoDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var tratamento = tratamentoRepository.ObterPeloId(dto.Id);
            if (tratamento == null)
            {
                tratamento = new Tratamento();
                novoItem = true;
            }

            tratamento.Descricao = dto.Descricao;
            tratamento.Automatico = dto.Automatico;

            if (Validator.IsValid(tratamento, out validationErrors))
            {
                if (novoItem)
                    tratamentoRepository.Inserir(tratamento);
                else
                    tratamentoRepository.Alterar(tratamento);

                tratamentoRepository.UnitOfWork.Commit();
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

            var tratamento = tratamentoRepository.ObterPeloId(id);

            try
            {
                tratamentoRepository.Remover(tratamento);
                tratamentoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, tratamento.Descricao), TypeMessage.Error);
                return false;
            }
        }
    }
}