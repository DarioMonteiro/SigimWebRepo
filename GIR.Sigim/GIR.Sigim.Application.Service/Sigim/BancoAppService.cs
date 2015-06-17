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
    public class BancoAppService : BaseAppService, IBancoAppService
    {
        private IBancoRepository bancoRepository;

        public BancoAppService(IBancoRepository bancoRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.bancoRepository = bancoRepository;
        }

        #region IBancoAppService Members

        public List<BancoDTO> ListarPeloFiltro(BancoFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<Banco>)new TrueSpecification<Banco>();


            return bancoRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<BancoDTO>>();
        }

        public BancoDTO ObterPeloId(int? id)
        {
            return bancoRepository.ObterPeloId(id).To<BancoDTO>();
        }

        public List<BancoDTO> ListarTodos()
        {
            return bancoRepository.ListarTodos().To<List<BancoDTO>>();
        }

        public bool Salvar(BancoDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var banco = bancoRepository.ObterPeloId(dto.Id);
            if (banco == null)
            {
                banco = new Banco();
                novoItem = true;
            }
            banco.Id = dto.Id;
            banco.Nome = dto.Nome;
            banco.Ativo = true;
            banco.Situacao = "A";
            banco.NumeroRemessa = dto.NumeroRemessa;
            banco.NumeroRemessaPagamento = dto.NumeroRemessaPagamento;
            banco.InterfaceEletronica = dto.InterfaceEletronica;

            if (Validator.IsValid(banco, out validationErrors))
            {
                if (novoItem)                    
                    bancoRepository.Inserir(banco);
                else
                    bancoRepository.Alterar(banco);

                bancoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                return true;
            }
            else
                messageQueue.AddRange(validationErrors, TypeMessage.Error);

            return false;
        }

        public bool Deletar(int? Id)
        {           
            if (Id == null)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
                return false;
            }

            var banco = bancoRepository.ObterPeloId(Id);

            try
            {
                bancoRepository.Remover(banco);
                bancoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, banco.Nome), TypeMessage.Error);
                return false;
            }
        }

        #endregion
    }
}