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
    public class ContaCorrenteAppService : BaseAppService, IContaCorrenteAppService
    {
        #region Declaração

        private IContaCorrenteRepository contaCorrenteRepository;

        #endregion
        
        #region Construtor

        public ContaCorrenteAppService(IContaCorrenteRepository contaCorrenteRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.contaCorrenteRepository = contaCorrenteRepository;
        }
              
        #endregion

        #region IContaCorrenteAppService Members

       
        public List<ContaCorrenteDTO> ListarPeloFiltro(ContaCorrenteFiltro filtro, int? idUsuario, out int totalRegistros)
        {
            var specification = (Specification<ContaCorrente>)new TrueSpecification<ContaCorrente>();

                specification = ContaCorrenteSpecification.PertenceAoBanco(filtro.BancoId);
         
            return contaCorrenteRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros,
                l => l.Banco,
                l => l.Agencia).To<List<ContaCorrenteDTO>>();
           
        }

           
        public ContaCorrenteDTO ObterPeloId(int? id)
        {
            
            return contaCorrenteRepository.ObterPeloId(id).To<ContaCorrenteDTO>();
        }


        //public bool Salvar(AgenciaDTO dto)
        //{
        //    if (dto == null)
        //        throw new ArgumentNullException("dto");

        //    bool novoItem = false;

        //    var agencia = agenciaRepository.ObterPeloId(dto.Id, l => l.Banco);
        //    if (agencia == null)
        //    {
        //        agencia = new Agencia();                
        //        novoItem = true;
        //    }
            
        //    agencia.BancoId = dto.BancoId;
        //    agencia.AgenciaCodigo = dto.AgenciaCodigo;
        //    agencia.DVAgencia = dto.DVAgencia;
        //    agencia.Nome = dto.Nome;
        //    agencia.NomeContato = dto.NomeContato;
        //    agencia.TelefoneContato = dto.TelefoneContato;
        //    agencia.TipoLogradouro = dto.TipoLogradouro;
        //    agencia.Logradouro = dto.Logradouro;
        //    agencia.Complemento = dto.Complemento;
        //    agencia.Numero = dto.Numero;
        //    agencia.Cidade = dto.Cidade;  
            
        //    if (Validator.IsValid(agencia, out validationErrors))
        //    {
        //        try
        //        {
        //            if (novoItem)
        //                agenciaRepository.Inserir(agencia);
        //            else
        //                agenciaRepository.Alterar(agencia);

        //            agenciaRepository.UnitOfWork.Commit();
                   
        //            dto.Id = agencia.Id;
        //            messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
        //            return true;
        //        }
        //        catch (Exception exception)
        //        {
        //            QueueExeptionMessages(exception);
        //        }
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

        //    var agencia = agenciaRepository.ObterPeloId(id);

        //    try
        //    {
        //        agenciaRepository.Remover(agencia);
        //        agenciaRepository.UnitOfWork.Commit();
        //        messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, agencia.Nome), TypeMessage.Error);
        //        return false;
        //    }
        //}

       #endregion
    }
}