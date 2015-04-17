﻿using System;
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
    public class PreRequisicaoMaterialAppService : BaseAppService, IPreRequisicaoMaterialAppService
    {
        private IPreRequisicaoMaterialRepository preRequisicaoMaterialRepository;
        private IRequisicaoMaterialRepository requisicaoMaterialRepository;
        private IUsuarioAppService usuarioAppService;

        public PreRequisicaoMaterialAppService(
            IPreRequisicaoMaterialRepository preRequisicaoMaterialRepository,
            IRequisicaoMaterialRepository requisicaoMaterialRepository,
            IUsuarioAppService usuarioAppService,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.preRequisicaoMaterialRepository = preRequisicaoMaterialRepository;
            this.requisicaoMaterialRepository = requisicaoMaterialRepository;
            this.usuarioAppService = usuarioAppService;
        }

        #region IPreRequisicaoMaterialAppService Members

        public List<PreRequisicaoMaterialDTO> ListarPeloFiltro(PreRequisicaoMaterialFiltro filtro, int? idUsuario, out int totalRegistros)
        {
            var specification = (Specification<PreRequisicaoMaterial>)new TrueSpecification<PreRequisicaoMaterial>();

            if (usuarioAppService.UsuarioPossuiCentroCustoDefinidoNoModulo(idUsuario, Resource.Sigim.NomeModulo.OrdemCompra))
                specification &= PreRequisicaoMaterialSpecification.UsuarioPossuiAcessoAoCentroCusto(idUsuario, Resource.Sigim.NomeModulo.OrdemCompra);

            if (filtro.Id.HasValue)
                specification &= PreRequisicaoMaterialSpecification.MatchingId(filtro.Id);
            else
            {
                specification &= PreRequisicaoMaterialSpecification.DataMaiorOuIgual(filtro.DataInicial);
                specification &= PreRequisicaoMaterialSpecification.DataMenorOuIgual(filtro.DataFinal);

                if (filtro.EhCancelada || filtro.EhFechada || filtro.EhParcialmenteAprovada || filtro.EhRequisitada)
                {
                    specification &= ((filtro.EhCancelada ? PreRequisicaoMaterialSpecification.EhCancelada() : new FalseSpecification<PreRequisicaoMaterial>())
                        || ((filtro.EhFechada) ? PreRequisicaoMaterialSpecification.EhFechada() : new FalseSpecification<PreRequisicaoMaterial>())
                        || ((filtro.EhParcialmenteAprovada) ? PreRequisicaoMaterialSpecification.EhParcialmenteAprovada() : new FalseSpecification<PreRequisicaoMaterial>())
                        || ((filtro.EhRequisitada) ? PreRequisicaoMaterialSpecification.EhRequisitada() : new FalseSpecification<PreRequisicaoMaterial>()));
                }
            }

            return preRequisicaoMaterialRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros,
                l => l.ListaItens.Select(c => c.PreRequisicaoMaterial),
                l => l.ListaItens.Select(c => c.CentroCusto.ListaUsuarioCentroCusto)).To<List<PreRequisicaoMaterialDTO>>();
        }

        public PreRequisicaoMaterialDTO ObterPeloId(int? id, int? idUsuario)
        {
            var specification = (Specification<PreRequisicaoMaterial>)new TrueSpecification<PreRequisicaoMaterial>();

            if (usuarioAppService.UsuarioPossuiCentroCustoDefinidoNoModulo(idUsuario, Resource.Sigim.NomeModulo.OrdemCompra))
                specification &= PreRequisicaoMaterialSpecification.UsuarioPossuiAcessoAoCentroCusto(idUsuario, Resource.Sigim.NomeModulo.OrdemCompra);

            return preRequisicaoMaterialRepository.ObterPeloId(id, specification, l => l.ListaItens.Select(s => s.ListaRequisicaoMaterialItem.Select(c => c.RequisicaoMaterial))).To<PreRequisicaoMaterialDTO>();
        }

        public bool Salvar(PreRequisicaoMaterialDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var preRequisicaoMaterial = preRequisicaoMaterialRepository.ObterPeloId(dto.Id, l => l.ListaItens);
            if (preRequisicaoMaterial == null)
            {
                preRequisicaoMaterial = new PreRequisicaoMaterial();
                preRequisicaoMaterial.Situacao = SituacaoPreRequisicaoMaterial.Requisitada;
                preRequisicaoMaterial.DataCadastro = DateTime.Now;
                preRequisicaoMaterial.LoginUsuarioCadastro = AuthenticationService.GetUser().Login;
                novoItem = true;
            }

            if (!PodeSerSalvaNaSituacaoAtual(preRequisicaoMaterial.Situacao))
            {
                var msg = string.Format(Resource.OrdemCompra.ErrorMessages.PreRequisicaoSituacaoInvalida, preRequisicaoMaterial.Situacao.ObterDescricao());
                messageQueue.Add(msg, TypeMessage.Error);
                return false;
            }

            preRequisicaoMaterial.Data = dto.Data;
            preRequisicaoMaterial.Observacao = dto.Observacao;
            ProcessarItens(dto, preRequisicaoMaterial);

            AjustarSituacaoPreRequisicao(preRequisicaoMaterial);

            if (Validator.IsValid(preRequisicaoMaterial, out validationErrors))
            {
                try
                {
                    if (novoItem)
                        preRequisicaoMaterialRepository.Inserir(preRequisicaoMaterial);
                    else
                        preRequisicaoMaterialRepository.Alterar(preRequisicaoMaterial);

                    preRequisicaoMaterialRepository.UnitOfWork.Commit();
                    dto.Id = preRequisicaoMaterial.Id;
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

        public bool Aprovar(int? id, int[] itens)
        {
            if (!itens.Any())
            {
                messageQueue.Add(Resource.OrdemCompra.ErrorMessages.SelecioneUmItemParaAprocacao, TypeMessage.Error);
                return false;
            }

            var preRequisicaoMaterial = preRequisicaoMaterialRepository.ObterPeloId(id, l => l.ListaItens);

            if (preRequisicaoMaterial == null)
            {
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
                return false;
            }

            if (!PodeAprovarNaSituacaoAtual(preRequisicaoMaterial.Situacao))
            {
                var msg = string.Format(Resource.OrdemCompra.ErrorMessages.PreRequisicaoSituacaoInvalida, preRequisicaoMaterial.Situacao.ObterDescricao());
                messageQueue.Add(msg, TypeMessage.Error);
                return false;
            }

            if (ItemJaAprovadoFoiSelecionado(itens, preRequisicaoMaterial))
                return false;

            var listaCentroCusto = preRequisicaoMaterial.ListaItens.Where(l => itens.Contains(l.Id.Value)).Select(l => l.CodigoCentroCusto).Distinct().ToList();
            
            foreach (var centroCusto in listaCentroCusto)
            {
                RequisicaoMaterial requisicaoMaterial = new RequisicaoMaterial();
                int sequencial = 1;

                requisicaoMaterial = new RequisicaoMaterial();
                requisicaoMaterial.Situacao = SituacaoRequisicaoMaterial.Aprovada;
                requisicaoMaterial.DataAprovacao = DateTime.Now;
                requisicaoMaterial.LoginUsuarioAprovacao = AuthenticationService.GetUser().Login;

                requisicaoMaterial.CodigoCentroCusto = centroCusto;
                requisicaoMaterial.Data = preRequisicaoMaterial.Data;
                requisicaoMaterial.DataCadastro = preRequisicaoMaterial.DataCadastro;
                requisicaoMaterial.LoginUsuarioCadastro = preRequisicaoMaterial.LoginUsuarioCadastro;
                requisicaoMaterial.Observacao = preRequisicaoMaterial.Observacao;

                var listaItens = preRequisicaoMaterial.ListaItens.Where(l => itens.Contains(l.Id.Value) && l.CodigoCentroCusto == centroCusto).ToList();
                foreach (var item in listaItens)
                {
                    RequisicaoMaterialItem requisicaoMaterialItem = new RequisicaoMaterialItem();
                    requisicaoMaterialItem.Sequencial = sequencial;
                    requisicaoMaterialItem.MaterialId = item.MaterialId;
                    requisicaoMaterialItem.CodigoClasse = item.CodigoClasse;
                    requisicaoMaterialItem.Complemento = item.Complemento;
                    requisicaoMaterialItem.UnidadeMedida = item.UnidadeMedida;
                    requisicaoMaterialItem.Quantidade = item.Quantidade;
                    requisicaoMaterialItem.QuantidadeAprovada = item.QuantidadeAprovada;
                    requisicaoMaterialItem.DataMaxima = item.DataMaxima;
                    requisicaoMaterialItem.DataMinima = item.DataMinima;
                    requisicaoMaterialItem.Situacao = SituacaoRequisicaoMaterialItem.Requisitado;
                    requisicaoMaterialItem.PreRequisicaoMaterialItemId = item.Id;
                    requisicaoMaterialItem.RequisicaoMaterial = requisicaoMaterial;
                    requisicaoMaterial.ListaItens.Add(requisicaoMaterialItem);

                    item.ListaRequisicaoMaterialItem.Add(requisicaoMaterialItem);
                    item.Situacao = SituacaoPreRequisicaoMaterialItem.Aprovado;
                    item.DataAprovacao = DateTime.Now;
                    item.LoginUsuarioAprovacao = AuthenticationService.GetUser().Login;
                    sequencial++;
                }
                requisicaoMaterialRepository.Inserir(requisicaoMaterial);
            }

            AjustarSituacaoPreRequisicao(preRequisicaoMaterial);

            preRequisicaoMaterialRepository.Alterar(preRequisicaoMaterial);
            preRequisicaoMaterialRepository.UnitOfWork.Commit();
            messageQueue.Add(Resource.OrdemCompra.SuccessMessages.AprovacaoItensComSucesso, TypeMessage.Success);
            return true;
        }

        public bool EhPermitidoSalvar(PreRequisicaoMaterialDTO dto)
        {
            return PodeSerSalvaNaSituacaoAtual(dto.Situacao);
        }

        public bool EhPermitidoCancelar(PreRequisicaoMaterialDTO dto)
        {
            if (!dto.Id.HasValue)
                return false;

            if (dto.Situacao != SituacaoPreRequisicaoMaterial.Requisitada)
                return false;

            if (dto.ListaItens.Any())
                return false;

            return true;
        }

        public bool EhPermitidoAdicionarItem(PreRequisicaoMaterialDTO dto)
        {
            return EhPermitidoSalvar(dto);
        }

        public bool EhPermitidoCancelarItem(PreRequisicaoMaterialDTO dto)
        {
            return EhPermitidoSalvar(dto);
        }

        public bool EhPermitidoEditarItem(PreRequisicaoMaterialDTO dto)
        {
            return EhPermitidoSalvar(dto);
        }

        public bool EhPermitidoAprovarItem(PreRequisicaoMaterialDTO dto)
        {
            if ((dto.Situacao != SituacaoPreRequisicaoMaterial.Requisitada) && (dto.Situacao != SituacaoPreRequisicaoMaterial.ParcialmenteAprovada))
                return false;

            if (!dto.ListaItens.Any(l => l.Situacao == SituacaoPreRequisicaoMaterialItem.Requisitado))
                return false;

            return true;
        }
        
        #endregion

        private bool PodeSerSalvaNaSituacaoAtual(SituacaoPreRequisicaoMaterial situacao)
        {
            if (situacao != SituacaoPreRequisicaoMaterial.Requisitada)
                return false;

            return true;
        }

        private bool PodeAprovarNaSituacaoAtual(SituacaoPreRequisicaoMaterial situacao)
        {
            if ((situacao != SituacaoPreRequisicaoMaterial.Requisitada) && (situacao != SituacaoPreRequisicaoMaterial.ParcialmenteAprovada))
                return false;

            return true;
        }

        private void AjustarSituacaoPreRequisicao(PreRequisicaoMaterial preRequisicao)
        {
            if ((preRequisicao.ListaItens.Any())
                && (preRequisicao.ListaItens.All(l => l.Situacao == SituacaoPreRequisicaoMaterialItem.Aprovado
                    || l.Situacao == SituacaoPreRequisicaoMaterialItem.Cancelado)))
                preRequisicao.Situacao = SituacaoPreRequisicaoMaterial.Fechada;
            else if (preRequisicao.ListaItens.Any(l => l.Situacao == SituacaoPreRequisicaoMaterialItem.Aprovado))
                preRequisicao.Situacao = SituacaoPreRequisicaoMaterial.ParcialmenteAprovada;
        }

        private void ProcessarItens(PreRequisicaoMaterialDTO dto, PreRequisicaoMaterial preRequisicaoMaterial)
        {
            RemoverItens(dto, preRequisicaoMaterial);
            AlterarItens(dto, preRequisicaoMaterial);
            AdicionarItens(dto, preRequisicaoMaterial);
        }

        private void RemoverItens(PreRequisicaoMaterialDTO dto, PreRequisicaoMaterial preRequisicaoMaterial)
        {
            for (int i = preRequisicaoMaterial.ListaItens.Count - 1; i >= 0; i--)
            {
                var item = preRequisicaoMaterial.ListaItens.ToList()[i];
                if (!dto.ListaItens.Any(l => l.Id == item.Id))
                {
                    preRequisicaoMaterial.ListaItens.Remove(item);
                    preRequisicaoMaterialRepository.RemoverItem(item);
                }
            }
        }

        private static void AlterarItens(PreRequisicaoMaterialDTO dto, PreRequisicaoMaterial preRequisicaoMaterial)
        {
            foreach (var item in preRequisicaoMaterial.ListaItens)
            {
                if (item.Situacao == SituacaoPreRequisicaoMaterialItem.Requisitado)
                {
                    var itemDTO = dto.ListaItens.Where(l => l.Id == item.Id).SingleOrDefault();
                    item.Sequencial = itemDTO.Sequencial;
                    item.Material = null;
                    item.MaterialId = itemDTO.Material.Id;
                    item.UnidadeMedida = itemDTO.Material.SiglaUnidadeMedida.Trim();
                    item.Complemento = itemDTO.Complemento.Trim();
                    item.Classe = null;
                    item.CodigoClasse = itemDTO.Classe.Codigo;
                    item.CentroCusto = null;
                    item.CodigoCentroCusto = itemDTO.CentroCusto.Codigo;
                    item.Quantidade = itemDTO.Quantidade;
                    item.QuantidadeAprovada = itemDTO.QuantidadeAprovada;
                    item.DataMaxima = itemDTO.DataMaxima;
                    item.DataMinima = itemDTO.DataMinima;
                    item.Situacao = itemDTO.Situacao;
                }
            }
        }

        private static void AdicionarItens(PreRequisicaoMaterialDTO dto, PreRequisicaoMaterial preRequisicaoMaterial)
        {
            foreach (var item in dto.ListaItens.Where(l => !l.Id.HasValue))
            {
                var itemLista = item.To<PreRequisicaoMaterialItem>();
                itemLista.PreRequisicaoMaterial = preRequisicaoMaterial;
                preRequisicaoMaterial.ListaItens.Add(itemLista);
            }
        }

        private bool ItemJaAprovadoFoiSelecionado(int[] itens, PreRequisicaoMaterial preRequisicaoMaterial)
        {
            bool existeItemJaAprovado = false;
            foreach (var itemId in itens)
            {
                var item = preRequisicaoMaterial.ListaItens.Where(l => l.Id == itemId).SingleOrDefault();
                if (item.ListaRequisicaoMaterialItem.Count > 0)
                {
                    messageQueue.Add(string.Format(Application.Resource.OrdemCompra.ErrorMessages.ItemJaAprovado, item.Sequencial.ToString()), TypeMessage.Error);
                    existeItemJaAprovado = true;
                }
            }

            return existeItemJaAprovado;
        }
    }
}