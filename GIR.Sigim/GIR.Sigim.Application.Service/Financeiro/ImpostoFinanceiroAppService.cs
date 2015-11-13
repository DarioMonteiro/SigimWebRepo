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
using GIR.Sigim.Application.Filtros;
using GIR.Sigim.Application.Constantes;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public class ImpostoFinanceiroAppService : BaseAppService, IImpostoFinanceiroAppService
    {
        private IImpostoFinanceiroRepository impostoFinanceiroRepository;

        public ImpostoFinanceiroAppService(IImpostoFinanceiroRepository impostoFinanceiroRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.impostoFinanceiroRepository = impostoFinanceiroRepository;
        }

        #region IImpostoFinanceiroAppService Members

        public List<ImpostoFinanceiroDTO> ListarTodos()
        {
            return impostoFinanceiroRepository.ListarTodos().To<List<ImpostoFinanceiroDTO>>();
        }

        public List<ImpostoFinanceiroDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<ImpostoFinanceiro>)new TrueSpecification<ImpostoFinanceiro>();

            return impostoFinanceiroRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros,
                l => l.TipoCompromisso,
                l => l.Cliente).To<List<ImpostoFinanceiroDTO>>();
        }

        public ImpostoFinanceiroDTO ObterPeloId(int? id)
        {
            return impostoFinanceiroRepository.ObterPeloId(id).To<ImpostoFinanceiroDTO>();
        }

        public bool Salvar(ImpostoFinanceiroDTO dto)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.ImpostoFinanceiroGravar))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            if (dto == null) throw new ArgumentNullException("dto");

            if (ValidaSalvar(dto) == false) { return false; } 

            bool novoItem = false;

            var impostoFinanceiro = impostoFinanceiroRepository.ObterPeloId(dto.Id);
            if (impostoFinanceiro == null)
            {
                impostoFinanceiro = new ImpostoFinanceiro();
                novoItem = true;
            }

            impostoFinanceiro.Sigla = dto.Sigla;
            impostoFinanceiro.Descricao = dto.Descricao;
            impostoFinanceiro.Aliquota = dto.Aliquota;
            impostoFinanceiro.ContaContabil = dto.ContaContabil;
            impostoFinanceiro.ClienteId = dto.ClienteId;
            impostoFinanceiro.TipoCompromissoId = dto.TipoCompromissoId;
            impostoFinanceiro.EhRetido = dto.EhRetido;
            impostoFinanceiro.Indireto = dto.Indireto;
            impostoFinanceiro.PagamentoEletronico = dto.PagamentoEletronico;
            if (dto.Periodicidade != null)
            {
                impostoFinanceiro.Periodicidade = (PeriodicidadeImpostoFinanceiro)dto.Periodicidade;
            }
            if (dto.FimDeSemana != null)
            {
                impostoFinanceiro.FimDeSemana = (FimDeSemanaImpostoFinanceiro)dto.FimDeSemana;
            }
            impostoFinanceiro.DiaVencimento = dto.DiaVencimento;
            if (dto.FatoGerador != null)
            {
                impostoFinanceiro.FatoGerador = (FatoGeradorImpostoFinanceiro)dto.FatoGerador;
            }
            
            if (Validator.IsValid(impostoFinanceiro, out validationErrors))
            {
                if (novoItem)
                    impostoFinanceiroRepository.Inserir(impostoFinanceiro);
                else
                    impostoFinanceiroRepository.Alterar(impostoFinanceiro);

                impostoFinanceiroRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                return true;
            }
            else
                messageQueue.AddRange(validationErrors, TypeMessage.Error);

            return false;
        }

        public bool Deletar(int? id)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.ImpostoFinanceiroDeletar))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            if (id == null)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
                return false;
            }

            var impostoFinanceiro = impostoFinanceiroRepository.ObterPeloId(id);

            try
            {
                impostoFinanceiroRepository.Remover(impostoFinanceiro);
                impostoFinanceiroRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, impostoFinanceiro.Descricao), TypeMessage.Error);
                return false;
            }
        }

        public List<ItemListaDTO> ListarOpcoesPeriodicidade()
        {  return typeof(PeriodicidadeImpostoFinanceiro).ToItemListaDTO(); }

        public List<ItemListaDTO> ListarOpcoesFimDeSemana()
        {  return typeof(FimDeSemanaImpostoFinanceiro).ToItemListaDTO(); }

        public List<ItemListaDTO> ListarOpcoesFatoGerador()
        {  return typeof(FatoGeradorImpostoFinanceiro).ToItemListaDTO();}


        public bool EhPermitidoSalvar()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.ImpostoFinanceiroGravar);
        }

        public bool EhPermitidoDeletar()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.ImpostoFinanceiroDeletar);
        }

        //public bool EhPermitidoImprimir()
        //{
        //    return UsuarioLogado.IsInRole(Funcionalidade.ImpostoFinanceiroImprimir);
        //}

        #endregion


        #region Métodos Privados

        public bool ValidaSalvar(ImpostoFinanceiroDTO dto)
        {
            bool retorno = true;

            if (dto == null)
            {
                retorno = false;
                throw new ArgumentNullException("dto");
            }

            if (dto.DiaVencimento != null)
            {
                if (dto.DiaVencimento < 0)
                {
                    messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.ValorDeveSerMaiorQue, "Dia do vencimento", "0"), TypeMessage.Error);
                    retorno = false;
                }
                if (dto.DiaVencimento > 31)
                {
                    messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.ValorDeveSerMenorQue, "Dia do vencimento", "31"), TypeMessage.Error);
                    retorno = false;
                }
            }

            return retorno;
        }

        #endregion


    }
}