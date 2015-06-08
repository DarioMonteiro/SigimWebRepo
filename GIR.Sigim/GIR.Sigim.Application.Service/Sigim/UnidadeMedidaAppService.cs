using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Resource;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Repository.Admin;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Application.Filtros.Sigim;
using System.Linq.Expressions;

namespace GIR.Sigim.Application.Service.Sigim
{
    public class UnidadeMedidaAppService : BaseAppService, IUnidadeMedidaAppService
    {
        private IUnidadeMedidaRepository unidadeMedidaRepository;

        public UnidadeMedidaAppService(IUnidadeMedidaRepository unidadeMedidaRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.unidadeMedidaRepository = unidadeMedidaRepository;
        }

        #region IUnidadeMedidaAppService Members

        public List<UnidadeMedidaDTO> ListarPeloFiltro(UnidadeMedidaFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<UnidadeMedida>)new TrueSpecification<UnidadeMedida>();


            return unidadeMedidaRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<UnidadeMedidaDTO>>();
        }

        public UnidadeMedidaDTO ObterPeloCodigo(string sigla)
        {
            return unidadeMedidaRepository.ObterPeloCodigo(sigla).To<UnidadeMedidaDTO>();
        }

        public List<UnidadeMedidaDTO> ListarTodos()
        {
            return unidadeMedidaRepository.ListarTodos().To<List<UnidadeMedidaDTO>>();
        }

        public bool Salvar(UnidadeMedidaDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var unidadeMedida = unidadeMedidaRepository.ObterPeloCodigo(dto.Sigla);
            if (unidadeMedida == null)
            {
                unidadeMedida = new UnidadeMedida();
                novoItem = true;
            }

            unidadeMedida.Descricao = dto.Descricao;
            unidadeMedida.Sigla = dto.Sigla;

            if (Validator.IsValid(unidadeMedida, out validationErrors))
            {
                if (novoItem)                    
                    unidadeMedidaRepository.Inserir(unidadeMedida);
                else
                    unidadeMedidaRepository.Alterar(unidadeMedida);

                unidadeMedidaRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                return true;
            }
            else
                messageQueue.AddRange(validationErrors, TypeMessage.Error);

            return false;
        }

        public bool Deletar(string sigla)
        {           
            if (sigla == null)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
                return false;
            }

            var unidadeMedida = unidadeMedidaRepository.ObterPeloCodigo(sigla);

            try
            {
                unidadeMedidaRepository.Remover(unidadeMedida);
                unidadeMedidaRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, unidadeMedida.Descricao), TypeMessage.Error);
                return false;
            }
        }

        #endregion
    }
}