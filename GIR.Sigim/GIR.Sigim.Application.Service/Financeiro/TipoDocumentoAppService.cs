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
    public class TipoDocumentoAppService : BaseAppService, ITipoDocumentoAppService
    {
        private ITipoDocumentoRepository tipoDocumentoRepository;

        public TipoDocumentoAppService(ITipoDocumentoRepository tipoDocumentoRepository, MessageQueue messageQueue) 
            : base (messageQueue)
        {
            this.tipoDocumentoRepository = tipoDocumentoRepository;
        }

        #region métodos de ITipoDocumentoAppService

        public List<TipoDocumentoDTO> ListarTodos()
        {
            return tipoDocumentoRepository.ListarTodos().OrderBy(l => l.Sigla).To<List<TipoDocumentoDTO>>(); 
        }

        public List<TipoDocumentoDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<TipoDocumento>)new TrueSpecification<TipoDocumento>();


            return tipoDocumentoRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<TipoDocumentoDTO>>();
        }

        public TipoDocumentoDTO ObterPeloId(int? id)
        {
            return tipoDocumentoRepository.ObterPeloId(id).To<TipoDocumentoDTO>();
        }

        public bool Salvar(TipoDocumentoDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var tipoDocumento = tipoDocumentoRepository.ObterPeloId(dto.Id);
            if (tipoDocumento == null)
            {
                tipoDocumento = new TipoDocumento();
                novoItem = true;
            }

            tipoDocumento.Sigla = dto.Sigla;
            tipoDocumento.Descricao = dto.Descricao;

            if (Validator.IsValid(tipoDocumento, out validationErrors))
            {
                if (novoItem)
                    tipoDocumentoRepository.Inserir(tipoDocumento);
                else
                    tipoDocumentoRepository.Alterar(tipoDocumento);

                tipoDocumentoRepository.UnitOfWork.Commit();
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

            var tipoCompromisso = tipoDocumentoRepository.ObterPeloId(id);

            try
            {
                tipoDocumentoRepository.Remover(tipoCompromisso);
                tipoDocumentoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, tipoCompromisso.Descricao), TypeMessage.Error);
                return false;
            }
        }

        #endregion
    }
}
