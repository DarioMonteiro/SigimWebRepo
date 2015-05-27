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
using GIR.Sigim.Application.Filtros.Financeiro;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public class TipoCompromissoAppService : BaseAppService, ITipoCompromissoAppService
    {
        private ITipoCompromissoRepository tipoCompromissoRepository;

        public TipoCompromissoAppService(ITipoCompromissoRepository tipoCompromissoRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.tipoCompromissoRepository = tipoCompromissoRepository;
        }

        #region ITipoCompromissoAppService Members

        public List<TipoCompromissoDTO> ListarTipoPagar()
        {
            return tipoCompromissoRepository.ListarPeloFiltro(l => l.TipoPagar.HasValue && l.TipoPagar.Value).To<List<TipoCompromissoDTO>>();
        }

        public List<TipoCompromissoDTO> ListarPeloFiltro(TipoCompromissoFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<TipoCompromisso>)new TrueSpecification<TipoCompromisso>();


            return tipoCompromissoRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<TipoCompromissoDTO>>();
        }

        public TipoCompromissoDTO ObterPeloId(int? id)
        {
            return tipoCompromissoRepository.ObterPeloId(id).To<TipoCompromissoDTO>();
        }

        public bool Salvar(TipoCompromissoDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var tipoCompromisso = tipoCompromissoRepository.ObterPeloId(dto.Id);
            if (tipoCompromisso == null)
            {
                tipoCompromisso = new TipoCompromisso();
                novoItem = true;
            }

            tipoCompromisso.Descricao = dto.Descricao;
            tipoCompromisso.TipoPagar = dto.TipoPagar;
            tipoCompromisso.TipoReceber = dto.TipoReceber;
            
            if (Validator.IsValid(tipoCompromisso, out validationErrors))
            {
                if (novoItem)
                    tipoCompromissoRepository.Inserir(tipoCompromisso);
                else
                    tipoCompromissoRepository.Alterar(tipoCompromisso);

                tipoCompromissoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                return true;
            }
            else
                messageQueue.AddRange(validationErrors, TypeMessage.Error);

            return false;
        }

        #endregion
    }
}