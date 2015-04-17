using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.OrdemCompra;
using GIR.Sigim.Application.Filtros.OrdemCompras;
using GIR.Sigim.Application.Service.Admin;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Repository.OrdemCompra;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Domain.Specification.OrdemCompra;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;

namespace GIR.Sigim.Application.Service.OrdemCompra
{
    public class RequisicaoMaterialAppService : BaseAppService, IRequisicaoMaterialAppService
    {
        private IRequisicaoMaterialRepository requisicaoMaterialRepository;
        private IUsuarioAppService usuarioAppService;

        public RequisicaoMaterialAppService(
            IRequisicaoMaterialRepository requisicaoMaterialRepository,
            IUsuarioAppService usuarioAppService,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.requisicaoMaterialRepository = requisicaoMaterialRepository;
            this.usuarioAppService = usuarioAppService;
        }

        #region IRequisicaoMaterialAppService Members

        public List<RequisicaoMaterialDTO> ListarPeloFiltro(RequisicaoMaterialFiltro filtro, int? idUsuario, out int totalRegistros)
        {
            var specification = (Specification<RequisicaoMaterial>)new TrueSpecification<RequisicaoMaterial>();

            if (usuarioAppService.UsuarioPossuiCentroCustoDefinidoNoModulo(idUsuario, Resource.Sigim.NomeModulo.OrdemCompra))
                specification &= RequisicaoMaterialSpecification.UsuarioPossuiAcessoAoCentroCusto(idUsuario, Resource.Sigim.NomeModulo.OrdemCompra);

            if (filtro.Id.HasValue)
                specification &= RequisicaoMaterialSpecification.MatchingId(filtro.Id);
            else
            {
                specification &= RequisicaoMaterialSpecification.DataMaiorOuIgual(filtro.DataInicial);
                specification &= RequisicaoMaterialSpecification.DataMenorOuIgual(filtro.DataFinal);
                specification &= RequisicaoMaterialSpecification.PertenceAoCentroCusto(filtro.CentroCusto.Codigo);

                if (filtro.EhAprovada || filtro.EhCancelada || filtro.EhFechada || filtro.EhRequisitada)
                {
                    specification &= ((filtro.EhAprovada ? RequisicaoMaterialSpecification.EhAprovada() : new FalseSpecification<RequisicaoMaterial>())
                        || ((filtro.EhCancelada) ? RequisicaoMaterialSpecification.EhCancelada() : new FalseSpecification<RequisicaoMaterial>())
                        || ((filtro.EhFechada) ? RequisicaoMaterialSpecification.EhFechada() : new FalseSpecification<RequisicaoMaterial>())
                        || ((filtro.EhRequisitada) ? RequisicaoMaterialSpecification.EhRequisitada() : new FalseSpecification<RequisicaoMaterial>()));
                }
            }

            return requisicaoMaterialRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros,
                l => l.ListaItens.Select(c => c.RequisicaoMaterial)).To<List<RequisicaoMaterialDTO>>();
        }

        public RequisicaoMaterialDTO ObterPeloId(int? id)
        {
            return requisicaoMaterialRepository.ObterPeloId(id).To<RequisicaoMaterialDTO>();
        }

        public bool Salvar(RequisicaoMaterialDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var requisicaoMaterial = requisicaoMaterialRepository.ObterPeloId(dto.Id, l => l.ListaItens);
            if (requisicaoMaterial == null)
            {
                requisicaoMaterial = new RequisicaoMaterial();
                requisicaoMaterial.Situacao = SituacaoRequisicaoMaterial.Requisitada;
                requisicaoMaterial.DataCadastro = DateTime.Now;
                requisicaoMaterial.LoginUsuarioCadastro = AuthenticationService.GetUser().Login;
                novoItem = true;
            }

            if (!PodeSerSalvaNaSituacaoAtual(requisicaoMaterial.Situacao))
            {
                var msg = string.Format(Resource.OrdemCompra.ErrorMessages.RequisicaoSituacaoInvalida, requisicaoMaterial.Situacao.ObterDescricao());
                messageQueue.Add(msg, TypeMessage.Error);
                return false;
            }

            requisicaoMaterial.Data = dto.Data;
            requisicaoMaterial.CodigoCentroCusto = dto.CentroCusto.Codigo;
            requisicaoMaterial.Observacao = dto.Observacao;
            ProcessarItens(dto, requisicaoMaterial);

            //if (requisicaoMaterial.ListaItens.All(l => l.Situacao == SituacaoRequisicaoMaterialItem.Aprovado || l.Situacao == SituacaoRequisicaoMaterialItem.Cancelado))
            //    requisicaoMaterial.Situacao = SituacaoRequisicaoMaterial.Fechada;

            if (Validator.IsValid(requisicaoMaterial, out validationErrors))
            {
                try
                {
                    if (novoItem)
                        requisicaoMaterialRepository.Inserir(requisicaoMaterial);
                    else
                        requisicaoMaterialRepository.Alterar(requisicaoMaterial);

                    requisicaoMaterialRepository.UnitOfWork.Commit();
                    dto.Id = requisicaoMaterial.Id;
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

        #endregion

        private bool PodeSerSalvaNaSituacaoAtual(SituacaoRequisicaoMaterial situacao)
        {
            if ((situacao != SituacaoRequisicaoMaterial.Requisitada) && (situacao != SituacaoRequisicaoMaterial.Aprovada))
                return false;

            return true;
        }

        private void ProcessarItens(RequisicaoMaterialDTO dto, RequisicaoMaterial requisicaoMaterial)
        {
            RemoverItens(dto, requisicaoMaterial);
            AlterarItens(dto, requisicaoMaterial);
            AdicionarItens(dto, requisicaoMaterial);
        }

        private void RemoverItens(RequisicaoMaterialDTO dto, RequisicaoMaterial requisicaoMaterial)
        {
            for (int i = requisicaoMaterial.ListaItens.Count - 1; i >= 0; i--)
            {
                var item = requisicaoMaterial.ListaItens.ToList()[i];
                if (!dto.ListaItens.Any(l => l.Id == item.Id))
                {
                    requisicaoMaterial.ListaItens.Remove(item);
                    requisicaoMaterialRepository.RemoverItem(item);
                }
            }
        }

        private static void AlterarItens(RequisicaoMaterialDTO dto, RequisicaoMaterial requisicaoMaterial)
        {
            foreach (var item in requisicaoMaterial.ListaItens)
            {
                if (item.Situacao == SituacaoRequisicaoMaterialItem.Requisitado)
                {
                    var itemDTO = dto.ListaItens.Where(l => l.Id == item.Id).SingleOrDefault();
                    item.Sequencial = itemDTO.Sequencial;
                    item.Material = null;
                    item.MaterialId = itemDTO.Material.Id;
                    item.UnidadeMedida = itemDTO.Material.SiglaUnidadeMedida.Trim();
                    item.Complemento = itemDTO.Complemento.Trim();
                    item.Classe = null;
                    item.CodigoClasse = itemDTO.Classe.Codigo;
                    item.Quantidade = itemDTO.Quantidade;
                    item.QuantidadeAprovada = itemDTO.QuantidadeAprovada;
                    item.DataMaxima = itemDTO.DataMaxima;
                    item.DataMinima = itemDTO.DataMinima;
                    item.Situacao = itemDTO.Situacao;
                }
            }
        }

        private static void AdicionarItens(RequisicaoMaterialDTO dto, RequisicaoMaterial requisicaoMaterial)
        {
            foreach (var item in dto.ListaItens.Where(l => !l.Id.HasValue))
            {
                var itemLista = item.To<RequisicaoMaterialItem>();
                itemLista.RequisicaoMaterial = requisicaoMaterial;
                requisicaoMaterial.ListaItens.Add(itemLista);
            }
        }
    }
}