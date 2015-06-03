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
    public class NacionalidadeAppService : BaseAppService, INacionalidadeAppService
    {
        private INacionalidadeRepository nacionalidadeRepository;

        public NacionalidadeAppService(INacionalidadeRepository NacionalidadeRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.nacionalidadeRepository = NacionalidadeRepository;
        }

        public List<NacionalidadeDTO> ListarTodos()
        {
            return nacionalidadeRepository.ListarTodos().To<List<NacionalidadeDTO>>();
        }

        public List<NacionalidadeDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<Nacionalidade>)new TrueSpecification<Nacionalidade>();

            return nacionalidadeRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<NacionalidadeDTO>>();
        }

        public NacionalidadeDTO ObterPeloId(int? id)
        {
            return nacionalidadeRepository.ObterPeloId(id).To<NacionalidadeDTO>();
        }

        public bool Salvar(NacionalidadeDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var nacionalidade = nacionalidadeRepository.ObterPeloId(dto.Id);
            if (nacionalidade == null)
            {
                nacionalidade = new Nacionalidade();
                novoItem = true;
            }

            nacionalidade.Descricao = dto.Descricao;
            nacionalidade.Automatico = dto.Automatico;

            if (Validator.IsValid(nacionalidade, out validationErrors))
            {
                if (novoItem)
                    nacionalidadeRepository.Inserir(nacionalidade);
                else
                    nacionalidadeRepository.Alterar(nacionalidade);

                nacionalidadeRepository.UnitOfWork.Commit();
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

            var nacionalidade = nacionalidadeRepository.ObterPeloId(id);

            try
            {
                nacionalidadeRepository.Remover(nacionalidade);
                nacionalidadeRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, nacionalidade.Descricao), TypeMessage.Error);
                return false;
            }
        }
    }
}