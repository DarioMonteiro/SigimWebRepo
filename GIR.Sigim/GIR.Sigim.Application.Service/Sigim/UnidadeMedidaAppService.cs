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

        #region ITipoCompromissoAppService Members

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


        //public bool Salvar(TipoCompromissoDTO dto)
        //{
        //    if (dto == null)
        //        throw new ArgumentNullException("dto");

        //    bool novoItem = false;

        //    var tipoCompromisso = tipoCompromissoRepository.ObterPeloId(dto.Id);
        //    if (tipoCompromisso == null)
        //    {
        //        tipoCompromisso = new TipoCompromisso();
        //        novoItem = true;
        //    }

        //    tipoCompromisso.Descricao = dto.Descricao;
        //    tipoCompromisso.TipoPagar = dto.TipoPagar;
        //    tipoCompromisso.TipoReceber = dto.TipoReceber;
            
        //    if (Validator.IsValid(tipoCompromisso, out validationErrors))
        //    {
        //        if (novoItem)
        //            tipoCompromissoRepository.Inserir(tipoCompromisso);
        //        else
        //            tipoCompromissoRepository.Alterar(tipoCompromisso);

        //        tipoCompromissoRepository.UnitOfWork.Commit();
        //        messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
        //        return true;
        //    }
        //    else
        //        messageQueue.AddRange(validationErrors, TypeMessage.Error);

        //    return false;
        //}

        //public bool Deletar(int? id)
        //{
        //    if (id == null)
        //    {
        //        messageQueue.Add(Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
        //        return false;
        //    }

        //    var tipoCompromisso = tipoCompromissoRepository.ObterPeloId(id);

        //    try
        //    {
        //        tipoCompromissoRepository.Remover(tipoCompromisso);
        //        tipoCompromissoRepository.UnitOfWork.Commit();
        //        messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, tipoCompromisso.Descricao), TypeMessage.Error);
        //        return false;
        //    }
        //}

        #endregion
    }
}