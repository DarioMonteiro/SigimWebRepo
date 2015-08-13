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
        private IHistoricoContabilRepository historicoContabilRepository;

        public HistoricoContabilAppService(IHistoricoContabilRepository historicoContabilRepository, MessageQueue messageQueue) 
            : base (messageQueue)
        {
            this.historicoContabilRepository = historicoContabilRepository;
        }

        #region métodos de ITipoMovimentoAppService

        public List<HistoricoContabilDTO> ListarTodos()
        {
            return historicoContabilRepository.ListarTodos().OrderBy(l => l.Descricao).To<List<HistoricoContabilDTO>>(); 
        }

        public List<HistoricoContabilDTO> ListarPorTipo(int tipo)
        {
            return historicoContabilRepository.ListarPeloFiltro(l => l.Tipo == tipo).To<List<HistoricoContabilDTO>>();
        }

        public List<HistoricoContabilDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<HistoricoContabil>)new TrueSpecification<HistoricoContabil>();


            return historicoContabilRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<HistoricoContabilDTO>>();
        }

        public HistoricoContabilDTO ObterPeloId(int? id)
        {
            return historicoContabilRepository.ObterPeloId(id).To<HistoricoContabilDTO>();
        }

        public bool Salvar(HistoricoContabilDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var historicoContabil = historicoContabilRepository.ObterPeloId(dto.Id);
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
                    historicoContabilRepository.Inserir(historicoContabil);
                else
                    historicoContabilRepository.Alterar(historicoContabil);

                historicoContabilRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                return true;
            }
            else
                messageQueue.AddRange(validationErrors, TypeMessage.Error);

            return false;
        }

        public bool Deletar(int? id)
        {
            var historicoContabil = historicoContabilRepository.ObterPeloId(id);

            if (historicoContabil == null)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
                return false;
            }

            try
            {
                historicoContabilRepository.Remover(historicoContabil);
                historicoContabilRepository.UnitOfWork.Commit();
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
