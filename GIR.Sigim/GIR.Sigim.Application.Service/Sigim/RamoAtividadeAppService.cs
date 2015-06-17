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
    public class RamoAtividadeAppService : BaseAppService, IRamoAtividadeAppService
    {
        private IRamoAtividadeRepository ramoAtividadeRepository;

        public RamoAtividadeAppService(IRamoAtividadeRepository RamoAtividadeRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.ramoAtividadeRepository = RamoAtividadeRepository;
        }

        public List<RamoAtividadeDTO> ListarTodos()
        {
            return ramoAtividadeRepository.ListarTodos().To<List<RamoAtividadeDTO>>();
        }

        public List<RamoAtividadeDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<RamoAtividade>)new TrueSpecification<RamoAtividade>();

            return ramoAtividadeRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<RamoAtividadeDTO>>();
        }

        public RamoAtividadeDTO ObterPeloId(int? id)
        {
            return ramoAtividadeRepository.ObterPeloId(id).To<RamoAtividadeDTO>();
        }

        public bool Salvar(RamoAtividadeDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var ramoAtividade = ramoAtividadeRepository.ObterPeloId(dto.Id);
            if (ramoAtividade == null)
            {
                ramoAtividade = new RamoAtividade();
                novoItem = true;
            }

            ramoAtividade.Descricao = dto.Descricao;
            ramoAtividade.Automatico = dto.Automatico;

            if (Validator.IsValid(ramoAtividade, out validationErrors))
            {
                if (novoItem)
                    ramoAtividadeRepository.Inserir(ramoAtividade);
                else
                    ramoAtividadeRepository.Alterar(ramoAtividade);

                ramoAtividadeRepository.UnitOfWork.Commit();
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

            var ramoAtividade = ramoAtividadeRepository.ObterPeloId(id);

            try
            {
                ramoAtividadeRepository.Remover(ramoAtividade);
                ramoAtividadeRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, ramoAtividade.Descricao), TypeMessage.Error);
                return false;
            }
        }
    }
}