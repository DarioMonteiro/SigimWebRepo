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
    public class FormaRecebimentoAppService : BaseAppService, IFormaRecebimentoAppService
    {
        private IFormaRecebimentoRepository formaRecebimentoRepository;

        public FormaRecebimentoAppService(IFormaRecebimentoRepository FormaRecebimentoRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.formaRecebimentoRepository = FormaRecebimentoRepository;
        }

        public List<FormaRecebimentoDTO> ListarTodos()
        {
            return formaRecebimentoRepository.ListarTodos().To<List<FormaRecebimentoDTO>>();
        }

        public List<FormaRecebimentoDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<FormaRecebimento>)new TrueSpecification<FormaRecebimento>();

            return formaRecebimentoRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<FormaRecebimentoDTO>>();
        }

        public FormaRecebimentoDTO ObterPeloId(int? id)
        {
            return formaRecebimentoRepository.ObterPeloId(id).To<FormaRecebimentoDTO>();
        }

        public bool Salvar(FormaRecebimentoDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var formaRecebimento = formaRecebimentoRepository.ObterPeloId(dto.Id);
            if (formaRecebimento == null)
            {
                formaRecebimento = new FormaRecebimento();
                novoItem = true;
            }
            if (formaRecebimento.Automatico == true)
            {
                return false;
            }
                       
            formaRecebimento.Descricao = dto.Descricao;
            formaRecebimento.Automatico = dto.Automatico;
            formaRecebimento.TipoRecebimento = dto.TipoRecebimento;
            formaRecebimento.NumeroDias = dto.NumeroDias;

            if (Validator.IsValid(formaRecebimento, out validationErrors))
            {
                if (novoItem)
                    formaRecebimentoRepository.Inserir(formaRecebimento);
                else
                    formaRecebimentoRepository.Alterar(formaRecebimento);

                formaRecebimentoRepository.UnitOfWork.Commit();
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

            var formaRecebimento = formaRecebimentoRepository.ObterPeloId(id);

            if (formaRecebimento.Automatico == true)
            {
                return false;
            }

            try
            {                
                formaRecebimentoRepository.Remover(formaRecebimento);
                formaRecebimentoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;                
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, formaRecebimento.Descricao), TypeMessage.Error);
                return false;
            }
        }

        public List<ItemListaDTO> ListarOpcoesTipoRecebimento()
        { return typeof(TipoFormaRecebimento).ToItemListaDTO(); }

    }
}