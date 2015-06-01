using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Application.Filtros;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public class TipoRateioAppService : BaseAppService, ITipoRateioAppService
    {
        #region declaracao

        private ITipoRateioRepository tipoRateioRepository;

        #endregion 

        #region construtor

        public TipoRateioAppService(ITipoRateioRepository tipoRateioRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.tipoRateioRepository = tipoRateioRepository;
        }

        #endregion

        #region métodos de ITipoRateioAppService

        public List<TipoRateioDTO> ListarTodos()
        {
            return tipoRateioRepository.ListarTodos().OrderBy(l => l.Descricao).To<List<TipoRateioDTO>>();
        }

        public List<TipoRateioDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<TipoRateio>)new TrueSpecification<TipoRateio>();


            return tipoRateioRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<TipoRateioDTO>>();
        }

        public TipoRateioDTO ObterPeloId(int? id)
        {
            return tipoRateioRepository.ObterPeloId(id).To<TipoRateioDTO>();
        }

        public bool Salvar(TipoRateioDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var tipoRateio = tipoRateioRepository.ObterPeloId(dto.Id);
            if (tipoRateio == null)
            {
                tipoRateio = new TipoRateio();
                novoItem = true;
            }

            tipoRateio.Descricao = dto.Descricao;

            if (Validator.IsValid(tipoRateio, out validationErrors))
            {
                if (novoItem)
                    tipoRateioRepository.Inserir(tipoRateio);
                else
                    tipoRateioRepository.Alterar(tipoRateio);

                tipoRateioRepository.UnitOfWork.Commit();
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

            var tipoRateio = tipoRateioRepository.ObterPeloId(id);

            try
            {
                tipoRateioRepository.Remover(tipoRateio);
                tipoRateioRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, tipoRateio.Descricao), TypeMessage.Error);
                return false;
            }
        }

        #endregion

    }
}
