using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros.Sigim;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Domain.Specification.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;


namespace GIR.Sigim.Application.Service.Sigim
{
    public class AgenciaAppService : BaseAppService, IAgenciaAppService
    {
        #region Declaração

        private IAgenciaRepository agenciaRepository;

        #endregion
        
        #region Construtor

        public AgenciaAppService(IAgenciaRepository agenciaRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.agenciaRepository = agenciaRepository;
        }
              
        #endregion

        #region IAgenciaAppService Members

       
        public List<AgenciaDTO> ListarPeloFiltro(AgenciaFiltro filtro, int? idUsuario, out int totalRegistros)
        {
            var specification = (Specification<Agencia>)new TrueSpecification<Agencia>();

                specification = AgenciaSpecification.PertenceAoBanco(filtro.BancoId);
         
            return agenciaRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros,
                l => l.Banco).To<List<AgenciaDTO>>();
           
        }

           
        public AgenciaDTO ObterPeloId(int? id)
        {
            
            return agenciaRepository.ObterPeloId(id).To<AgenciaDTO>();
        }


        public bool Salvar(AgenciaDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var agencia = agenciaRepository.ObterPeloId(dto.Id, l => l.Banco);
            if (agencia == null)
            {
                agencia = new Agencia();                
                novoItem = true;
            }
            
            agencia.BancoId = dto.BancoId;
            agencia.AgenciaCodigo = dto.AgenciaCodigo;
            agencia.DVAgencia = dto.DVAgencia;
            agencia.Nome = dto.Nome;
            agencia.NomeContato = dto.NomeContato;
            agencia.TelefoneContato = dto.TelefoneContato;
            agencia.TipoLogradouro = dto.TipoLogradouro;
            agencia.Logradouro = dto.Logradouro;
            agencia.Complemento = dto.Complemento;
            agencia.Numero = dto.Numero;
            agencia.Cidade = dto.Cidade;  
            
            if (Validator.IsValid(agencia, out validationErrors))
            {
                try
                {
                    if (novoItem)
                        agenciaRepository.Inserir(agencia);
                    else
                        agenciaRepository.Alterar(agencia);

                    agenciaRepository.UnitOfWork.Commit();
                   
                    dto.Id = agencia.Id;
                    messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                    return true;
                }
                catch (Exception exception)
                {
                    QueueExeptionMessages(exception);
                }
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

            var agencia = agenciaRepository.ObterPeloId(id);

            try
            {
                agenciaRepository.Remover(agencia);
                agenciaRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, agencia.Nome), TypeMessage.Error);
                return false;
            }
        }

       #endregion
    }
}