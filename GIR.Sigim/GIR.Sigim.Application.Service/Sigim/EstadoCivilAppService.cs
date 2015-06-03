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
    public class EstadoCivilAppService : BaseAppService, IEstadoCivilAppService
    {
        private IEstadoCivilRepository estadoCivilRepository;

        public EstadoCivilAppService(IEstadoCivilRepository EstadoCivilRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.estadoCivilRepository = EstadoCivilRepository;
        }

        public List<EstadoCivilDTO> ListarTodos()
        {
            return estadoCivilRepository.ListarTodos().To<List<EstadoCivilDTO>>();
        }

        public List<EstadoCivilDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<EstadoCivil>)new TrueSpecification<EstadoCivil>();

            return estadoCivilRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<EstadoCivilDTO>>();
        }

        public EstadoCivilDTO ObterPeloId(int? id)
        {
            return estadoCivilRepository.ObterPeloId(id).To<EstadoCivilDTO>();
        }

        public bool Salvar(EstadoCivilDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var estadoCivil = estadoCivilRepository.ObterPeloId(dto.Id);
            if (estadoCivil == null)
            {
                estadoCivil = new EstadoCivil();
                novoItem = true;
            }

            estadoCivil.Descricao = dto.Descricao;
            estadoCivil.Automatico = dto.Automatico;

            if (Validator.IsValid(estadoCivil, out validationErrors))
            {
                if (novoItem)
                    estadoCivilRepository.Inserir(estadoCivil);
                else
                    estadoCivilRepository.Alterar(estadoCivil);

                estadoCivilRepository.UnitOfWork.Commit();
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

            var estadoCivil = estadoCivilRepository.ObterPeloId(id);

            try
            {
                estadoCivilRepository.Remover(estadoCivil);
                estadoCivilRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, estadoCivil.Descricao), TypeMessage.Error);
                return false;
            }
        }
    }
}