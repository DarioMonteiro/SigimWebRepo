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
            
            return contaCorrenteRepository.ObterPeloId(id, l =>l.Banco.ListaAgencia).To<ContaCorrenteDTO>();
        }

        public List<ItemListaDTO> ListarTipo()
        { 
            return typeof(TipoContaCorrente).ToItemListaDTO(); 
        }

        public List<ContaCorrenteDTO> ListarAtivosPorBanco(int? bancoId)
        {
            return contaCorrenteRepository.ListarPeloFiltro(l => l.Situacao == "A" && l.BancoId == bancoId,
                                                            l => l.Banco.ListaAgencia).To<List<ContaCorrenteDTO>>();
        }


        public bool Salvar(ContaCorrenteDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var contaCorrente = contaCorrenteRepository.ObterPeloId(dto.Id);
            if (contaCorrente == null)
            {
                contaCorrente = new ContaCorrente();
                novoItem = true;
            }

            contaCorrente.BancoId = dto.BancoId;
            contaCorrente.AgenciaId = dto.AgenciaId;
            contaCorrente.ContaCodigo = dto.ContaCodigo;
            contaCorrente.DVConta = dto.DVConta;
            contaCorrente.Descricao = dto.Descricao;
            contaCorrente.CodigoEmpresa = dto.CodigoEmpresa;
            contaCorrente.NomeCedente = dto.NomeCedente;
            contaCorrente.CNPJCedente = dto.CNPJCedente;            
            contaCorrente.Complemento = dto.Complemento;
            contaCorrente.Tipo = dto.Tipo;
            contaCorrente.Situacao = dto.Situacao;

            if (Validator.IsValid(contaCorrente, out validationErrors))
            {
                try
                {
                    if (novoItem)
                        contaCorrenteRepository.Inserir(contaCorrente);
                    else
                        contaCorrenteRepository.Alterar(contaCorrente);

                    contaCorrenteRepository.UnitOfWork.Commit();

                    dto.Id = contaCorrente.Id;
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

            var contaCorrente = contaCorrenteRepository.ObterPeloId(id, l => l.Banco.ListaAgencia);

            try
            {
                contaCorrenteRepository.Remover(contaCorrente);
                contaCorrenteRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, contaCorrente.ContaCodigo), TypeMessage.Error);
                return false;
            }
        }

       #endregion
    }
}