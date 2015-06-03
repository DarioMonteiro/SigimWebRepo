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
    public class TipologiaAppService : BaseAppService, ITipologiaAppService
    {
        private ITipologiaRepository tipologiaRepository;

        public TipologiaAppService(ITipologiaRepository TipologiaRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.tipologiaRepository = TipologiaRepository;
        }

        public List<TipologiaDTO> ListarTodos()
        {
            return tipologiaRepository.ListarTodos().To<List<TipologiaDTO>>();
        }

        public List<TipologiaDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<Tipologia>)new TrueSpecification<Tipologia>();

            return tipologiaRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<TipologiaDTO>>();
        }

        public TipologiaDTO ObterPeloId(int? id)
        {
            return tipologiaRepository.ObterPeloId(id).To<TipologiaDTO>();
        }

        public bool Salvar(TipologiaDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var tipologia = tipologiaRepository.ObterPeloId(dto.Id);
            if (tipologia == null)
            {
                tipologia = new Tipologia();
                novoItem = true;
            }

            tipologia.Descricao = dto.Descricao;
            tipologia.Automatico = dto.Automatico;

            if (Validator.IsValid(tipologia, out validationErrors))
            {
                if (novoItem)
                    tipologiaRepository.Inserir(tipologia);
                else
                    tipologiaRepository.Alterar(tipologia);

                tipologiaRepository.UnitOfWork.Commit();
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

            var tipologia = tipologiaRepository.ObterPeloId(id);

            try
            {
                tipologiaRepository.Remover(tipologia);
                tipologiaRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, tipologia.Descricao), TypeMessage.Error);
                return false;
            }
        }
    }
}