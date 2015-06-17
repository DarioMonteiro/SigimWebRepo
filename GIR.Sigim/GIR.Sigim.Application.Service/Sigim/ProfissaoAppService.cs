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
    public class ProfissaoAppService : BaseAppService, IProfissaoAppService
    {
        private IProfissaoRepository profissaoRepository;

        public ProfissaoAppService(IProfissaoRepository ProfissaoRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.profissaoRepository = ProfissaoRepository;
        }

        public List<ProfissaoDTO> ListarTodos()
        {
            return profissaoRepository.ListarTodos().To<List<ProfissaoDTO>>();
        }

        public List<ProfissaoDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<Profissao>)new TrueSpecification<Profissao>();

            return profissaoRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<ProfissaoDTO>>();
        }

        public ProfissaoDTO ObterPeloId(int? id)
        {
            return profissaoRepository.ObterPeloId(id).To<ProfissaoDTO>();
        }

        public bool Salvar(ProfissaoDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var profissao = profissaoRepository.ObterPeloId(dto.Id);
            if (profissao == null)
            {
                profissao = new Profissao();
                novoItem = true;
            }

            profissao.Descricao = dto.Descricao;
            profissao.Automatico = dto.Automatico;

            if (Validator.IsValid(profissao, out validationErrors))
            {
                if (novoItem)
                    profissaoRepository.Inserir(profissao);
                else
                    profissaoRepository.Alterar(profissao);

                profissaoRepository.UnitOfWork.Commit();
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

            var profissao = profissaoRepository.ObterPeloId(id);

            try
            {
                profissaoRepository.Remover(profissao);
                profissaoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, profissao.Descricao), TypeMessage.Error);
                return false;
            }
        }
    }
}