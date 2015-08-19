using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Sac;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Resource;
using GIR.Sigim.Domain.Entity.Sac;
using GIR.Sigim.Domain.Repository.Admin;
using GIR.Sigim.Domain.Repository.Sac;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Application.Filtros.Sac;

namespace GIR.Sigim.Application.Service.Sac
{
    public class SetorAppService : BaseAppService, ISetorAppService
    {
        private ISetorRepository SetorRepository;

        public SetorAppService(ISetorRepository SetorRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.SetorRepository = SetorRepository;
        }

        #region ISetorAppService Members


        public List<SetorDTO> ListarPeloFiltro(SetorFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<Setor>)new TrueSpecification<Setor>();


            return SetorRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<SetorDTO>>();
        }

        public SetorDTO ObterPeloId(int? id)
        {
            return SetorRepository.ObterPeloId(id).To<SetorDTO>();
        }

        public bool Salvar(SetorDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var Setor = SetorRepository.ObterPeloId(dto.Id);
            if (Setor == null)
            {
                Setor = new Setor();
                novoItem = true;
            }

            Setor.Descricao = dto.Descricao;

            if (Validator.IsValid(Setor, out validationErrors))
            {
                if (novoItem)
                    SetorRepository.Inserir(Setor);
                else
                    SetorRepository.Alterar(Setor);

                SetorRepository.UnitOfWork.Commit();
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

            var Setor = SetorRepository.ObterPeloId(id);

            try
            {
                SetorRepository.Remover(Setor);
                SetorRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, Setor.Descricao), TypeMessage.Error);
                return false;
            }
        }

        public List<SetorDTO> ListarTodos()
        {
            return SetorRepository.ListarTodos().OrderBy(l => l.Descricao).To<List<SetorDTO>>(); 
        }


        #endregion
      
    }
}