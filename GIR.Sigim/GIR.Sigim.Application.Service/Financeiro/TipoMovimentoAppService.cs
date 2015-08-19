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
using GIR.Sigim.Domain.Specification.Financeiro;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public class TipoMovimentoAppService : BaseAppService, ITipoMovimentoAppService
    {
        private ITipoMovimentoRepository tipoMovimentoRepository;

        public TipoMovimentoAppService(ITipoMovimentoRepository tipoMovimentoRepository, MessageQueue messageQueue) 
            : base (messageQueue)
        {
            this.tipoMovimentoRepository = tipoMovimentoRepository;
        }

        #region métodos de ITipoMovimentoAppService

        public List<TipoMovimentoDTO> ListarTodos()
        {
            return tipoMovimentoRepository.ListarTodos().OrderBy(l => l.Id).To<List<TipoMovimentoDTO>>(); 
        }

        public List<TipoMovimentoDTO> ListarNaoAutomatico(BaseFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<TipoMovimento>)new TrueSpecification<TipoMovimento>();

            specification &= TipoMovimentoSpecification.EhNaoAutomatico();

            return tipoMovimentoRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros,
                l => l.HistoricoContabil).To<List<TipoMovimentoDTO>>();
        }


        public TipoMovimentoDTO ObterPeloId(int? id)
        {
            return tipoMovimentoRepository.ObterPeloId(id).To<TipoMovimentoDTO>();
        }

        public bool Salvar(TipoMovimentoDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var tipoMovimento = tipoMovimentoRepository.ObterPeloId(dto.Id);
            if (tipoMovimento == null)
            {
                tipoMovimento = new TipoMovimento();
                novoItem = true;
            }

            tipoMovimento.Descricao = dto.Descricao;
            tipoMovimento.HistoricoContabilId = dto.HistoricoContabilId;
            tipoMovimento.Tipo = dto.Tipo;
            tipoMovimento.Operacao = dto.Operacao;

            try
            {
                if (!EhValidoSalvarTipoMovimento(tipoMovimento)){
                    return false;
                }

                if (Validator.IsValid(tipoMovimento, out validationErrors))
                {
                    if (novoItem)
                        tipoMovimentoRepository.Inserir(tipoMovimento);
                    else
                        tipoMovimentoRepository.Alterar(tipoMovimento);

                    tipoMovimentoRepository.UnitOfWork.Commit();
                    messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                    return true;
                }
                else
                    messageQueue.AddRange(validationErrors, TypeMessage.Error);
            }
            catch (Exception exception)
            {
                QueueExeptionMessages(exception);
            }
            return false;
        }

        public bool Deletar(int? id)
        {
            if (id == null)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
                return false;
            }

            var tipoMovimento = tipoMovimentoRepository.ObterPeloId(id);

            try
            {
                tipoMovimentoRepository.Remover(tipoMovimento);
                tipoMovimentoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, tipoMovimento.Descricao), TypeMessage.Error);
                return false;
            }
        }

        #endregion

        #region métodos privados de ITipoMovimentoAppService

        private bool EhValidoSalvarTipoMovimento(TipoMovimento tipoMovimento)
        {
            if (!EhNaoAutomatico(tipoMovimento.Id)){
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.RegistroProtegido, TypeMessage.Error);
                return false;
            }

            return true;
        }

        private bool EhNaoAutomatico(int? tipoMovimentoId)
        {
            if (tipoMovimentoId.HasValue)
            {
                TipoMovimentoDTO tipoMovimento = new TipoMovimentoDTO();

                tipoMovimento = ObterPeloId(tipoMovimentoId);
                if (tipoMovimento.Automatico.HasValue)
                {
                    if (tipoMovimento.Automatico.Value)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        #endregion
    }
}
