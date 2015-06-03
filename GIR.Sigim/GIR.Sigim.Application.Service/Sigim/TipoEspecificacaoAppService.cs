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
    public class TipoEspecificacaoAppService : BaseAppService, ITipoEspecificacaoAppService
    {
        private ITipoEspecificacaoRepository tipoEspecificacaoRepository;

        public TipoEspecificacaoAppService(ITipoEspecificacaoRepository TipoEspecificacaoRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.tipoEspecificacaoRepository = TipoEspecificacaoRepository;
        }

        public List<TipoEspecificacaoDTO> ListarTodos()
        {
            return tipoEspecificacaoRepository.ListarTodos().To<List<TipoEspecificacaoDTO>>();
        }

        public List<TipoEspecificacaoDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<TipoEspecificacao>)new TrueSpecification<TipoEspecificacao>();

            return tipoEspecificacaoRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<TipoEspecificacaoDTO>>();
        }

        public TipoEspecificacaoDTO ObterPeloId(int? id)
        {
            return tipoEspecificacaoRepository.ObterPeloId(id).To<TipoEspecificacaoDTO>();
        }

        public bool Salvar(TipoEspecificacaoDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var tipoEspecificacao = tipoEspecificacaoRepository.ObterPeloId(dto.Id);
            if (tipoEspecificacao == null)
            {
                tipoEspecificacao = new TipoEspecificacao();
                novoItem = true;
            }

            tipoEspecificacao.Descricao = dto.Descricao;
            tipoEspecificacao.Automatico = dto.Automatico;

            if (Validator.IsValid(tipoEspecificacao, out validationErrors))
            {
                if (novoItem)
                    tipoEspecificacaoRepository.Inserir(tipoEspecificacao);
                else
                    tipoEspecificacaoRepository.Alterar(tipoEspecificacao);

                tipoEspecificacaoRepository.UnitOfWork.Commit();
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

            var tipoEspecificacao = tipoEspecificacaoRepository.ObterPeloId(id);

            try
            {
                tipoEspecificacaoRepository.Remover(tipoEspecificacao);
                tipoEspecificacaoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, tipoEspecificacao.Descricao), TypeMessage.Error);
                return false;
            }
        }
    }
}