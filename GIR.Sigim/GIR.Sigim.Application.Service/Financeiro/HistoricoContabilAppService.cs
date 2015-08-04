using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Resource;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Repository.Admin;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Application.Filtros;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public class HistoricoContabilAppService : BaseAppService, IHistoricoContabilAppService
    {
        private IHistoricoContabilRepository HistoricoContabilRepository;

        public HistoricoContabilAppService(IHistoricoContabilRepository HistoricoContabilRepository, MessageQueue messageQueue) 
            : base (messageQueue)
        {
            this.HistoricoContabilRepository = HistoricoContabilRepository;
        }

        #region métodos de ITipoMovimentoAppService

        public List<HistoricoContabilDTO> ListarTodos()
        {
            return HistoricoContabilRepository.ListarTodos().OrderBy(l => l.Descricao).To<List<HistoricoContabilDTO>>(); 
        }

        public List<HistoricoContabilDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<HistoricoContabil>)new TrueSpecification<HistoricoContabil>();


            return HistoricoContabilRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<HistoricoContabilDTO>>();
        }

        public HistoricoContabilDTO ObterPeloId(int? id)
        {
            return HistoricoContabilRepository.ObterPeloId(id).To<HistoricoContabilDTO>();
        }

        public bool Salvar(HistoricoContabilDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var historicoContabil = HistoricoContabilRepository.ObterPeloId(dto.Id);
            if (historicoContabil == null)
            {
                historicoContabil = new HistoricoContabil();
                novoItem = true;
            }

            historicoContabil.Descricao = dto.Descricao;
            historicoContabil.Tipo = dto.Tipo;


            if (Validator.IsValid(historicoContabil, out validationErrors))
            {
                if (novoItem)
                    HistoricoContabilRepository.Inserir(historicoContabil);
                else
                    HistoricoContabilRepository.Alterar(historicoContabil);

                HistoricoContabilRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                return true;
            }
            else
                messageQueue.AddRange(validationErrors, TypeMessage.Error);

            return false;
        }

        public bool Deletar(int? id)
        {
            var historicoContabil = HistoricoContabilRepository.ObterPeloId(id);

            if (historicoContabil == null)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
                return false;
            }

            try
            {
                HistoricoContabilRepository.Remover(historicoContabil);
                HistoricoContabilRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, historicoContabil.Descricao), TypeMessage.Error);
                return false;
            }
        }

        #endregion
    }
}
