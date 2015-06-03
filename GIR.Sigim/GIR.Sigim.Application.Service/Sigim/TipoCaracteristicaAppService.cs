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
    public class TipoCaracteristicaAppService : BaseAppService, ITipoCaracteristicaAppService
    {
        private ITipoCaracteristicaRepository tipoCaracteristicaRepository;

        public TipoCaracteristicaAppService(ITipoCaracteristicaRepository TipoCaracteristicaRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.tipoCaracteristicaRepository = TipoCaracteristicaRepository;
        }

        public List<TipoCaracteristicaDTO> ListarTodos()
        {
            return tipoCaracteristicaRepository.ListarTodos().To<List<TipoCaracteristicaDTO>>();
        }

        public List<TipoCaracteristicaDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<TipoCaracteristica>)new TrueSpecification<TipoCaracteristica>();

            return tipoCaracteristicaRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<TipoCaracteristicaDTO>>();
        }

        public TipoCaracteristicaDTO ObterPeloId(int? id)
        {
            return tipoCaracteristicaRepository.ObterPeloId(id).To<TipoCaracteristicaDTO>();
        }

        public bool Salvar(TipoCaracteristicaDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var tipoCaracteristica = tipoCaracteristicaRepository.ObterPeloId(dto.Id);
            if (tipoCaracteristica == null)
            {
                tipoCaracteristica = new TipoCaracteristica();
                novoItem = true;
            }

            tipoCaracteristica.Descricao = dto.Descricao;
            tipoCaracteristica.Automatico = dto.Automatico;

            if (Validator.IsValid(tipoCaracteristica, out validationErrors))
            {
                if (novoItem)
                    tipoCaracteristicaRepository.Inserir(tipoCaracteristica);
                else
                    tipoCaracteristicaRepository.Alterar(tipoCaracteristica);

                tipoCaracteristicaRepository.UnitOfWork.Commit();
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

            var tipoCaracteristica = tipoCaracteristicaRepository.ObterPeloId(id);

            try
            {
                tipoCaracteristicaRepository.Remover(tipoCaracteristica);
                tipoCaracteristicaRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, tipoCaracteristica.Descricao), TypeMessage.Error);
                return false;
            }
        }
    }
}