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
    public class FonteNegocioAppService : BaseAppService, IFonteNegocioAppService
    {
        private IFonteNegocioRepository fonteNegocioRepository;

        public FonteNegocioAppService(IFonteNegocioRepository FonteNegocioRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.fonteNegocioRepository = FonteNegocioRepository;
        }

        public List<FonteNegocioDTO> ListarTodos()
        {
            return fonteNegocioRepository.ListarTodos().To<List<FonteNegocioDTO>>();
        }

        public List<FonteNegocioDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<FonteNegocio>)new TrueSpecification<FonteNegocio>();

            return fonteNegocioRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<FonteNegocioDTO>>();
        }

        public FonteNegocioDTO ObterPeloId(int? id)
        {
            return fonteNegocioRepository.ObterPeloId(id).To<FonteNegocioDTO>();
        }

        public bool Salvar(FonteNegocioDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var fonteNegocio = fonteNegocioRepository.ObterPeloId(dto.Id);
            if (fonteNegocio == null)
            {
                fonteNegocio = new FonteNegocio();
                novoItem = true;
            }

            fonteNegocio.Descricao = dto.Descricao;
            fonteNegocio.Automatico = dto.Automatico;

            if (Validator.IsValid(fonteNegocio, out validationErrors))
            {
                if (novoItem)
                    fonteNegocioRepository.Inserir(fonteNegocio);
                else
                    fonteNegocioRepository.Alterar(fonteNegocio);

                fonteNegocioRepository.UnitOfWork.Commit();
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

            var fonteNegocio = fonteNegocioRepository.ObterPeloId(id);

            try
            {
                fonteNegocioRepository.Remover(fonteNegocio);
                fonteNegocioRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, fonteNegocio.Descricao), TypeMessage.Error);
                return false;
            }
        }
    }
}