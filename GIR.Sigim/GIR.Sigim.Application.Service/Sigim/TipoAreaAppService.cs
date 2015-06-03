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
    public class TipoAreaAppService : BaseAppService, ITipoAreaAppService
    {
        private ITipoAreaRepository tipoAreaRepository;

        public TipoAreaAppService(ITipoAreaRepository TipoAreaRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.tipoAreaRepository = TipoAreaRepository;
        }

        public List<TipoAreaDTO> ListarTodos()
        {
            return tipoAreaRepository.ListarTodos().To<List<TipoAreaDTO>>();
        }

        public List<TipoAreaDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<TipoArea>)new TrueSpecification<TipoArea>();

            return tipoAreaRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<TipoAreaDTO>>();
        }

        public TipoAreaDTO ObterPeloId(int? id)
        {
            return tipoAreaRepository.ObterPeloId(id).To<TipoAreaDTO>();
        }

        public bool Salvar(TipoAreaDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var tipoArea = tipoAreaRepository.ObterPeloId(dto.Id);
            if (tipoArea == null)
            {
                tipoArea = new TipoArea();
                novoItem = true;
            }

            tipoArea.Descricao = dto.Descricao;
            tipoArea.Automatico = dto.Automatico;

            if (Validator.IsValid(tipoArea, out validationErrors))
            {
                if (novoItem)
                    tipoAreaRepository.Inserir(tipoArea);
                else
                    tipoAreaRepository.Alterar(tipoArea);

                tipoAreaRepository.UnitOfWork.Commit();
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

            var tipoArea = tipoAreaRepository.ObterPeloId(id);

            try
            {
                tipoAreaRepository.Remover(tipoArea);
                tipoAreaRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, tipoArea.Descricao), TypeMessage.Error);
                return false;
            }
        }
    }
}