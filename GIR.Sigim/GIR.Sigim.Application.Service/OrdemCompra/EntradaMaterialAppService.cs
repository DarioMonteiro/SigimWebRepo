using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CrystalDecisions.Shared;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.OrdemCompra;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros.OrdemCompras;
using GIR.Sigim.Application.Reports.OrdemCompra;
using GIR.Sigim.Application.Service.Admin;
using GIR.Sigim.Application.Service.Financeiro;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Domain.Entity.Estoque;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Orcamento;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Repository.Estoque;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Domain.Repository.OrdemCompra;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Domain.Specification.OrdemCompra;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;

namespace GIR.Sigim.Application.Service.OrdemCompra
{
    public class EntradaMaterialAppService : BaseAppService, IEntradaMaterialAppService
    {
        private IEntradaMaterialRepository entradaMaterialRepository;
        private IUsuarioAppService usuarioAppService;
        private IParametrosOrdemCompraRepository parametrosOrdemCompraRepository;
        private IParametrosFinanceiroAppService parametrosFinanceiroAppService;
        private ICentroCustoRepository centroCustoRepository;
        private IEstoqueRepository estoqueRepository;
        private ITituloPagarRepository tituloPagarRepository;
        private IApropriacaoRepository apropriacaoRepository;
        private ILogOperacaoAppService logOperacaoAppService;
        private IOrdemCompraRepository ordemCompraRepository;
        private IFeriadoRepository feriadoRepository;

        private ParametrosOrdemCompra parametrosOrdemCompra;
        public ParametrosOrdemCompra ParametrosOrdemCompra
        {
            get
            {
                if (parametrosOrdemCompra == null)
                    parametrosOrdemCompra = parametrosOrdemCompraRepository.Obter();

                return parametrosOrdemCompra;
            }
        }

        private ParametrosFinanceiroDTO parametrosFinanceiro;
        public ParametrosFinanceiroDTO ParametrosFinanceiro
        {
            get
            {
                if (parametrosFinanceiro == null)
                    parametrosFinanceiro = parametrosFinanceiroAppService.Obter();

                return parametrosFinanceiro;
            }
        }

        public EntradaMaterialAppService(
            IEntradaMaterialRepository entradaMaterialRepository,
            IUsuarioAppService usuarioAppService,
            IParametrosOrdemCompraRepository parametrosOrdemCompraRepository,
            IParametrosFinanceiroAppService parametrosFinanceiroAppService,
            ICentroCustoRepository centroCustoRepository,
            IEstoqueRepository estoqueRepository,
            ITituloPagarRepository tituloPagarRepository,
            IApropriacaoRepository apropriacaoRepository,
            ILogOperacaoAppService logOperacaoAppService,
            IOrdemCompraRepository ordemCompraRepository,
            IFeriadoRepository feriadoRepository,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.entradaMaterialRepository = entradaMaterialRepository;
            this.usuarioAppService = usuarioAppService;
            this.parametrosOrdemCompraRepository = parametrosOrdemCompraRepository;
            this.parametrosFinanceiroAppService = parametrosFinanceiroAppService;
            this.centroCustoRepository = centroCustoRepository;
            this.estoqueRepository = estoqueRepository;
            this.tituloPagarRepository = tituloPagarRepository;
            this.apropriacaoRepository = apropriacaoRepository;
            this.logOperacaoAppService = logOperacaoAppService;
            this.ordemCompraRepository = ordemCompraRepository;
            this.feriadoRepository = feriadoRepository;
        }

        #region IEntradaMaterialAppService Members

        public List<EntradaMaterialDTO> ListarPeloFiltro(EntradaMaterialFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<EntradaMaterial>)new TrueSpecification<EntradaMaterial>();

            if (usuarioAppService.UsuarioPossuiCentroCustoDefinidoNoModulo(UsuarioLogado.Id, Resource.Sigim.NomeModulo.OrdemCompra))
            {
                specification &= EntradaMaterialSpecification.UsuarioPossuiAcessoAoCentroCusto(UsuarioLogado.Id, Resource.Sigim.NomeModulo.OrdemCompra);
            }
            else
            {
                specification &= EntradaMaterialSpecification.EhCentroCustoAtivo();
            }

            if (filtro.Id.HasValue)
                specification &= EntradaMaterialSpecification.MatchingId(filtro.Id);
            else
            {
                specification &= EntradaMaterialSpecification.DataMaiorOuIgual(filtro.DataInicial);
                specification &= EntradaMaterialSpecification.DataMenorOuIgual(filtro.DataFinal);
                specification &= EntradaMaterialSpecification.PertenceAoCentroCustoIniciadoPor(filtro.CentroCusto.Codigo);
                specification &= EntradaMaterialSpecification.MatchingNumeroNotaFiscal(filtro.NumeroNotaFiscal);

                if (filtro.EhPendente || filtro.EhCancelada || filtro.EhFechada)
                {
                    specification &= ((filtro.EhPendente ? EntradaMaterialSpecification.EhPendente() : new FalseSpecification<EntradaMaterial>())
                        || ((filtro.EhCancelada) ? EntradaMaterialSpecification.EhCancelada() : new FalseSpecification<EntradaMaterial>())
                        || ((filtro.EhFechada) ? EntradaMaterialSpecification.EhFechada() : new FalseSpecification<EntradaMaterial>()));
                }
            }

            return entradaMaterialRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros,
                l => l.CentroCusto,
                l => l.ClienteFornecedor,
                l => l.FornecedorNota,
                l => l.ListaAvaliacaoFornecedor).To<List<EntradaMaterialDTO>>();
        }

        public EntradaMaterialDTO ObterPeloId(int? id)
        {
            return ObterPeloIdEUsuario(id,
                UsuarioLogado.Id,
                l => l.CentroCusto,
                l => l.ClienteFornecedor,
                l => l.FornecedorNota,
                l => l.ListaItens.Select(o => o.Classe),
                l => l.ListaItens.Select(o => o.OrdemCompraItem.Material),
                l => l.ListaItens.Select(o => o.ComplementoCST),
                l => l.ListaItens.Select(o => o.ComplementoNaturezaOperacao),
                l => l.ListaItens.Select(o => o.NaturezaReceita),
                l => l.ListaFormaPagamento.Select(o => o.TituloPagar),
                l => l.ListaImposto,
                l => l.ListaMovimentoEstoque).To<EntradaMaterialDTO>();
        }

        public List<OrdemCompraItemDTO> ListarItensDeOrdemCompraLiberadaComSaldo(int? entradaMaterialId)
        {
            var entradaMaterial = ObterPeloIdEUsuario(entradaMaterialId, UsuarioLogado.Id);
            if (entradaMaterial != null)
            {
                if (!PodeSerSalvaNaSituacaoAtual(entradaMaterial.Situacao))
                {
                    var msg = string.Format(Resource.OrdemCompra.ErrorMessages.EntradaMaterialSituacaoInvalida, entradaMaterial.Situacao.ObterDescricao());
                    messageQueue.Add(msg, TypeMessage.Error);
                    return null;
                }

                var specification = (Specification<Domain.Entity.OrdemCompra.OrdemCompra>)new TrueSpecification<Domain.Entity.OrdemCompra.OrdemCompra>();

                specification &= OrdemCompraSpecification.EhLiberada();
                specification &= OrdemCompraSpecification.PertenceAoCentroCustoIniciadoPor(entradaMaterial.CodigoCentroCusto);
                specification &= OrdemCompraSpecification.MatchingFornecedorId(entradaMaterial.ClienteFornecedorId);

                var listaOrdemCompra = ordemCompraRepository.ListarPeloFiltro(specification,
                    l => l.ListaItens.Select(o => o.Classe),
                    l => l.ListaItens.Select(o => o.Material));

                return listaOrdemCompra.SelectMany(l => l.ListaItens.Where(o => o.Saldo > 0)).To<List<OrdemCompraItemDTO>>();
            }
            else {
                messageQueue.Add(Resource.OrdemCompra.ErrorMessages.SelecioneUmaEntradaMaterial, TypeMessage.Error);
                return null;
            }
        }

        public bool AdicionarItens(int? entradaMaterialId, int?[] itens)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.EntradaMaterialGravar))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            var entradaMaterial = ObterPeloIdEUsuario(entradaMaterialId, UsuarioLogado.Id, l => l.ListaItens);
            if (entradaMaterial == null)
            {
                messageQueue.Add(Resource.OrdemCompra.ErrorMessages.SelecioneUmaEntradaMaterial, TypeMessage.Error);
                return false;
            }

            if (!PodeSerSalvaNaSituacaoAtual(entradaMaterial.Situacao))
            {
                var msg = string.Format(Resource.OrdemCompra.ErrorMessages.EntradaMaterialSituacaoInvalida, entradaMaterial.Situacao.ObterDescricao());
                messageQueue.Add(msg, TypeMessage.Error);
                return false;
            }

            var listaOrdemCompraItem = ordemCompraRepository.ListarItensPeloId(itens);

            foreach (var ordemCompraItem in listaOrdemCompraItem)
            {
                int sequencial = entradaMaterial.ListaItens.Any() ? entradaMaterial.ListaItens.Max(l => l.Sequencial) + 1 : 1;
                var entradaMaterialItem = new EntradaMaterialItem();
                entradaMaterialItem.OrdemCompraItem = ordemCompraItem;
                entradaMaterialItem.CodigoClasse = ordemCompraItem.CodigoClasse;
                entradaMaterialItem.Sequencial = sequencial;
                entradaMaterialItem.Quantidade = ordemCompraItem.Saldo;
                entradaMaterialItem.ValorUnitario = ordemCompraItem.ValorUnitario;
                entradaMaterialItem.PercentualIPI = ordemCompraItem.PercentualIPI;
                entradaMaterialItem.PercentualDesconto = ordemCompraItem.PercentualDesconto;
                entradaMaterialItem.ValorTotal = ordemCompraItem.ValorTotalComImposto;
                entradaMaterialItem.BaseIPI = ordemCompraItem.Quantidade * ordemCompraItem.ValorUnitario;
                entradaMaterial.ListaItens.Add(entradaMaterialItem);

                ordemCompraItem.QuantidadeEntregue += entradaMaterialItem.Quantidade;
            }

            try
            {
                entradaMaterialRepository.Alterar(entradaMaterial);
                entradaMaterialRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception exception)
            {
                QueueExeptionMessages(exception);
            }

            return false;
        }

        public bool RemoverItens(int? entradaMaterialId, int?[] itens)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.EntradaMaterialGravar))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            var entradaMaterial = ObterPeloIdEUsuario(entradaMaterialId,
                UsuarioLogado.Id,
                l => l.ListaItens.Select(o => o.OrdemCompraItem.OrdemCompra),
                l => l.ListaFormaPagamento.Select(o => o.OrdemCompraFormaPagamento));

            if (entradaMaterial == null)
            {
                messageQueue.Add(Resource.OrdemCompra.ErrorMessages.SelecioneUmaEntradaMaterial, TypeMessage.Error);
                return false;
            }

            if (!PodeSerSalvaNaSituacaoAtual(entradaMaterial.Situacao))
            {
                var msg = string.Format(Resource.OrdemCompra.ErrorMessages.EntradaMaterialSituacaoInvalida, entradaMaterial.Situacao.ObterDescricao());
                messageQueue.Add(msg, TypeMessage.Error);
                return false;
            }

            var listaItens = entradaMaterial.ListaItens.Where(l => itens.Contains(l.Id)).ToList();
            var listaFormasPagamentoOC = entradaMaterial.ListaFormaPagamento.Select(l => l.OrdemCompraFormaPagamento.OrdemCompraId).Distinct();

            if (listaFormasPagamentoOC.Intersect(listaItens.Select(l => l.OrdemCompraItem.OrdemCompraId)).Any())
            {
                messageQueue.Add(Resource.OrdemCompra.ErrorMessages.ExisteFormaPagamentoParaItemSelecionado, TypeMessage.Error);
                return false;
            }

            for (int i = listaItens.Count() - 1; i >= 0; i--)
            {
                var entradaMaterialItem = listaItens[i];
                entradaMaterialItem.OrdemCompraItem.QuantidadeEntregue -= entradaMaterialItem.Quantidade;
                entradaMaterialItem.OrdemCompraItem.OrdemCompra.Situacao = SituacaoOrdemCompra.Liberada;
                entradaMaterialRepository.RemoverEntradaMaterialItem(entradaMaterialItem);
            }
            
            try
            {
                entradaMaterialRepository.Alterar(entradaMaterial);
                entradaMaterialRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception exception)
            {
                QueueExeptionMessages(exception);
            }

            return false;
        }

        public List<EntradaMaterialItemDTO> ListarItens(int? entradaMaterialId)
        {
            var entradaMaterial = ObterPeloIdEUsuario(entradaMaterialId,
                UsuarioLogado.Id,
                l => l.ListaItens.Select(o => o.Classe),
                l => l.ListaItens.Select(o => o.OrdemCompraItem.Material));

            if (entradaMaterial == null)
            {
                messageQueue.Add(Resource.OrdemCompra.ErrorMessages.SelecioneUmaEntradaMaterial, TypeMessage.Error);
                return null;
            }

            return entradaMaterial.ListaItens.To<List<EntradaMaterialItemDTO>>();
        }

        public bool CancelarEntrada(int? id, string motivo)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.EntradaMaterialCancelar))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            var entradaMaterial = ObterPeloIdEUsuario(id, UsuarioLogado.Id,
                l => l.ListaItens.Select(o => o.OrdemCompraItem.Material),
                l => l.ListaItens.Select(o => o.OrdemCompraItem.OrdemCompra.ListaOrdemCompraFormaPagamento.Select(s => s.TituloPagar.ListaApropriacao)),
                l => l.ListaFormaPagamento.Select(o => o.TituloPagar.ListaImpostoPagar.Select(s => s.TituloPagarImposto)),
                l => l.ListaFormaPagamento.Select(o => o.OrdemCompraFormaPagamento.TituloPagar.ListaImpostoPagar),
                l => l.ListaFormaPagamento.Select(o => o.ListaTituloPagarAdiantamento.Select(s => s.ListaApropriacaoAdiantamento)),
                l => l.ListaImposto,
                l => l.ListaMovimentoEstoque,
                l => l.OrdemCompraFrete,
                l => l.TituloFrete.ListaApropriacao);

            if (!EhCancelamentoPossivel(entradaMaterial))
                return false;

            if (string.IsNullOrEmpty(motivo.Trim()))
            {
                messageQueue.Add(Resource.OrdemCompra.ErrorMessages.InformeMotivoCancelamentoEntradaMaterial, TypeMessage.Error);
                return false;
            }

            LiberarFreteDaOrdemCampra(entradaMaterial);
            LiberarOrdemCompra(entradaMaterial);
            AtualizarQuantidadeEntregueOrdemCompraItem(entradaMaterial);
            AtualizarFormaPagamentoParaNaoUtulizada(entradaMaterial);
            AtualizarFormaPagamentoAdiantadaParaNaoAssociada(entradaMaterial);
            DeletarTitulosPagarAdiantamento(entradaMaterial);

            List<TituloPagar> listaTitulosAlterados = new List<TituloPagar>();
            ProcessarCancelamentoTitulosDeImpostos(entradaMaterial, motivo, listaTitulosAlterados);
            ReverterTitulosPagar(entradaMaterial, motivo, listaTitulosAlterados);

            ReprovisionarApropriacoesDasFormasPagamentoNaoUtilizadas(entradaMaterial);
            RemoverImpostoPagar(entradaMaterial);
            AjustarEstoque(entradaMaterial);
            RemoverEntradaMaterialItem(entradaMaterial);
            RemoverEntradaMaterialImposto(entradaMaterial);
            RemoverEntradaMaterialFormaPagamento(entradaMaterial);

            entradaMaterial.MotivoCancelamento = motivo;
            entradaMaterial.Situacao = SituacaoEntradaMaterial.Cancelada;
            entradaMaterial.DataCancelamento = DateTime.Now;
            entradaMaterial.LoginUsuarioCancelamento = UsuarioLogado.Login;
            entradaMaterial.DataFrete = null;
            entradaMaterial.TransportadoraId = null;
            entradaMaterial.Transportadora = null;
            entradaMaterial.TituloFreteId = null;
            entradaMaterial.ValorFrete = null;
            entradaMaterial.NumeroNotaFrete = null;
            entradaMaterial.TipoNotaFreteId = null;
            entradaMaterial.TipoNotaFrete = null;
            entradaMaterial.OrdemCompraFreteId = null;

            entradaMaterialRepository.Alterar(entradaMaterial);
            entradaMaterialRepository.UnitOfWork.Commit();
            GravarLogOperacaoCancelamento(entradaMaterial, listaTitulosAlterados);
            messageQueue.Add(Resource.OrdemCompra.SuccessMessages.CancelamentoComSucesso, TypeMessage.Success);
            return true;
        }

        public bool LiberarTitulos(int? id)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.EntradaMaterialLiberar))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            var entradaMaterial = ObterPeloIdEUsuario(id, UsuarioLogado.Id,
                l => l.ListaItens.Select(o => o.OrdemCompraItem.Material),
                l => l.ListaItens.Select(o => o.OrdemCompraItem.OrdemCompra.ListaOrdemCompraFormaPagamento.Select(s => s.TituloPagar.ListaApropriacao)),
                l => l.ListaFormaPagamento.Select(o => o.TituloPagar.Cliente),
                l => l.ListaFormaPagamento.Select(o => o.TituloPagar.ListaImpostoPagar.Select(s => s.TituloPagarImposto)),
                l => l.ListaFormaPagamento.Select(o => o.OrdemCompraFormaPagamento.TituloPagar.ListaImpostoPagar),
                l => l.ListaFormaPagamento.Select(o => o.ListaTituloPagarAdiantamento.Select(s => s.ListaApropriacaoAdiantamento)),
                l => l.ListaImposto.Select(o => o.ImpostoFinanceiro),
                l => l.ListaMovimentoEstoque,
                l => l.OrdemCompraFrete,
                l => l.TituloFrete.ListaApropriacao,
                l => l.ClienteFornecedor,
                l => l.FornecedorNota);

            //if (!EhCancelamentoPossivel(entradaMaterial))
            //    return false;

            var fornecedor = entradaMaterial.FornecedorNotaId.HasValue ? entradaMaterial.FornecedorNota : entradaMaterial.ClienteFornecedor;

            ProcessarLiberacao(entradaMaterial, fornecedor);

            if (entradaMaterial.TituloFrete != null)
            {
                if (entradaMaterial.TituloFrete.DataEmissaoDocumento > entradaMaterial.DataFrete.Value)
                    entradaMaterial.TituloFrete.DataEmissaoDocumento = entradaMaterial.DataFrete.Value;

                entradaMaterial.TituloFrete.DataVencimento = entradaMaterial.DataFrete.Value;
                entradaMaterial.TituloFrete.ValorTitulo = entradaMaterial.ValorFrete.Value;
                entradaMaterial.TituloFrete.Situacao = ParametrosOrdemCompra.GeraTituloAguardando.Value ? SituacaoTituloPagar.AguardandoLiberacao : SituacaoTituloPagar.Liberado;
                entradaMaterial.TituloFrete.TipoDocumentoId = entradaMaterial.TipoNotaFreteId;
                entradaMaterial.TituloFrete.Documento = entradaMaterial.NumeroNotaFrete;
                entradaMaterial.TituloFrete.TipoCompromissoId = ParametrosOrdemCompra.TipoCompromissoFreteId;
            }

            ReprovisionarApropriacoesDasFormasPagamentoNaoUtilizadas(entradaMaterial);

            entradaMaterial.Situacao = SituacaoEntradaMaterial.Fechada;
            entradaMaterial.DataLiberacao = DateTime.Now;
            entradaMaterial.LoginUsuarioLiberacao = UsuarioLogado.Login;

            entradaMaterialRepository.Alterar(entradaMaterial);
            entradaMaterialRepository.UnitOfWork.Commit();
            //GravarLogOperacaoCancelamento(entradaMaterial, listaTitulosAlterados);
            messageQueue.Add(Resource.OrdemCompra.SuccessMessages.LiberacaoRealizadaComSucesso, TypeMessage.Success);
            return true;
        }

        private void RemoverApropriacoesDoTitulo(TituloPagar titulo)
        {
            for (int i = titulo.ListaApropriacao.Count() - 1; i >= 0; i--)
            {
                var apropriacao = titulo.ListaApropriacao.ToList()[i];
                apropriacaoRepository.Remover(apropriacao);
            }
        }

        private void ProcessarLiberacao(EntradaMaterial entradaMaterial, ClienteFornecedor fornecedor)
        {
            var valorTotalLiberado = entradaMaterial.ListaFormaPagamento.Where(l => !l.OrdemCompraFormaPagamento.EhPagamentoAntecipado.Value).Sum(s => s.Valor);
            var valorTotalItens = entradaMaterial.ListaItens.Sum(l => l.ValorTotal);
            var valorDesconto = (decimal)(entradaMaterial.PercentualDesconto.HasValue ? valorTotalItens * entradaMaterial.Desconto / 100 : entradaMaterial.Desconto);
            var valorTotalImposto = entradaMaterial.ListaImposto.Where(l => l.ImpostoFinanceiro.EhRetido == true).Sum(o => o.Valor);
            bool jaApropriouTituloFrete = false;

            if (entradaMaterial.TituloFrete != null)
                RemoverApropriacoesDoTitulo(entradaMaterial.TituloFrete);

            foreach (var formaPagamentoEM in entradaMaterial.ListaFormaPagamento)
            {
                var percentualTitulo = valorTotalLiberado > 0 ? (formaPagamentoEM.Valor / valorTotalLiberado * 100) : 0;
                var descontoTitulo = (decimal)(valorDesconto * percentualTitulo / 100);
                var valorImpostoTitulo = (decimal)(valorTotalImposto * percentualTitulo / 100);
                var valorAbatimentoTitulo = descontoTitulo + valorImpostoTitulo;

                TituloPagarAdiantamento tituloPagarAdiantamento = null;

                #region Geração de títulos

                if (formaPagamentoEM.OrdemCompraFormaPagamento.EhPagamentoAntecipado.Value)
                {
                    tituloPagarAdiantamento = new TituloPagarAdiantamento();
                    tituloPagarAdiantamento.Cliente = fornecedor;
                    tituloPagarAdiantamento.Identificacao = "Ref.OC : " + formaPagamentoEM.OrdemCompraFormaPagamento.OrdemCompraId + " EM : " + entradaMaterial.Id;
                    tituloPagarAdiantamento.TipoDocumentoId = entradaMaterial.TipoNotaFiscalId;
                    tituloPagarAdiantamento.Documento = entradaMaterial.NumeroNotaFiscal;
                    tituloPagarAdiantamento.DataEmissaoDocumento = entradaMaterial.DataEmissaoNota.Value;
                    tituloPagarAdiantamento.ValorAdiantamento = formaPagamentoEM.Valor;
                    tituloPagarAdiantamento.LoginUsuarioCadastro = UsuarioLogado.Login;
                    tituloPagarAdiantamento.DataCadastro = DateTime.Now;
                    tituloPagarAdiantamento.TituloPagarId = formaPagamentoEM.TituloPagarId;
                    tituloPagarAdiantamento.EntradaMaterialFormaPagamentoId = formaPagamentoEM.Id;
                    formaPagamentoEM.ListaTituloPagarAdiantamento.Add(tituloPagarAdiantamento);
                }
                else
                {                    

                    if (!formaPagamentoEM.TituloPagarId.HasValue)
                    {
                        formaPagamentoEM.TituloPagar = new TituloPagar();
                        formaPagamentoEM.TituloPagar.TipoCompromissoId = formaPagamentoEM.TipoCompromissoId;
                        formaPagamentoEM.TituloPagar.TipoTitulo = TipoTitulo.Normal;
                        formaPagamentoEM.TituloPagar.Multa = 0;
                        formaPagamentoEM.TituloPagar.EhMultaPercentual = false;
                        formaPagamentoEM.TituloPagar.TaxaPermanencia = 0;
                        formaPagamentoEM.TituloPagar.EhTaxaPermanenciaPercentual = false;
                        formaPagamentoEM.TituloPagar.LoginUsuarioCadastro = UsuarioLogado.Login;
                        formaPagamentoEM.TituloPagar.DataCadastro = DateTime.Now;
                        formaPagamentoEM.TituloPagar.SistemaOrigem = "OC";
                    }

                    formaPagamentoEM.TituloPagar.Identificacao = "Ref.OC : " + formaPagamentoEM.OrdemCompraFormaPagamento.OrdemCompraId + " EM : " + entradaMaterial.Id;
					formaPagamentoEM.TituloPagar.Cliente = fornecedor;
                    formaPagamentoEM.TituloPagar.DataVencimento = formaPagamentoEM.Data;
					formaPagamentoEM.TituloPagar.ValorTitulo = formaPagamentoEM.Valor;
                    formaPagamentoEM.TituloPagar.ValorImposto = valorImpostoTitulo;
					formaPagamentoEM.TituloPagar.DataEmissaoDocumento = entradaMaterial.DataEmissaoNota.Value;
					formaPagamentoEM.TituloPagar.Situacao = ParametrosOrdemCompra.GeraTituloAguardando.Value ? SituacaoTituloPagar.AguardandoLiberacao : SituacaoTituloPagar.Liberado;
					formaPagamentoEM.TituloPagar.TipoDocumentoId = entradaMaterial.TipoNotaFiscalId;
					formaPagamentoEM.TituloPagar.Documento = entradaMaterial.NumeroNotaFiscal;
                    formaPagamentoEM.TituloPagar.Desconto = descontoTitulo;
                    formaPagamentoEM.TituloPagar.DataLimiteDesconto = null;
                    if (valorDesconto > 0)
                        formaPagamentoEM.TituloPagar.DataLimiteDesconto = formaPagamentoEM.Data;

                    formaPagamentoEM.TituloPagar.LoginUsuarioApropriacao = UsuarioLogado.Login;
                    formaPagamentoEM.TituloPagar.DataApropriacao = DateTime.Now;

                    formaPagamentoEM.OrdemCompraFormaPagamento.Data = formaPagamentoEM.Data;
                    formaPagamentoEM.OrdemCompraFormaPagamento.Valor = formaPagamentoEM.Valor;
                    formaPagamentoEM.OrdemCompraFormaPagamento.TituloPagar = formaPagamentoEM.TituloPagar;

                    RemoverApropriacoesDoTitulo(formaPagamentoEM.TituloPagar);
                }

                #endregion

                #region Apropriação de Títulos

                //foreach (var ordemCompra in entradaMaterial.ListaItens.Select(l => l.OrdemCompraItem.OrdemCompra).Distinct())
                //{
                    var ordemCompra = formaPagamentoEM.OrdemCompraFormaPagamento.OrdemCompra;
                    var listaItensPorOC = entradaMaterial.ListaItens.Where(l => l.OrdemCompraItem.OrdemCompraId == ordemCompra.Id).ToList();
                    var valorTotal = decimal.Round(listaItensPorOC.Sum(o => o.ValorTotal).Value, 5);
                    var listaCodigoClasses = entradaMaterial.ListaItens
                        .Where(l => l.OrdemCompraItem.OrdemCompraId == ordemCompra.Id)
                        .Select(o => o.CodigoClasse).Distinct();

                    foreach (var codigoClasse in listaCodigoClasses)
                    {
                        var valorTotalClasse = decimal.Round(listaItensPorOC.Where(l => l.CodigoClasse == codigoClasse).Sum(o => o.ValorTotal).Value, 5);
                        var percentualClasse = decimal.Round((valorTotalClasse / valorTotal * 100), 5);
                        var valorAbatimentoClasse = decimal.Round((valorAbatimentoTitulo * percentualClasse / 100), 5);

                        var valorClasseTitulo = decimal.Round((formaPagamentoEM.Valor * percentualClasse / 100) - valorAbatimentoClasse, 5);
                        var percentualClasseTitulo = decimal.Round((valorClasseTitulo / formaPagamentoEM.Valor * 100), 5);

                        if (tituloPagarAdiantamento != null)
                        {
                            ApropriacaoAdiantamento apropriacaoAdiantamento = new ApropriacaoAdiantamento();
                            apropriacaoAdiantamento.CodigoClasse = codigoClasse;
                            apropriacaoAdiantamento.CodigoCentroCusto = ordemCompra.CodigoCentroCusto;
                            apropriacaoAdiantamento.TituloPagarAdiantamento = tituloPagarAdiantamento;
                            apropriacaoAdiantamento.Percentual = percentualClasse;
                            apropriacaoAdiantamento.Valor = decimal.Round((formaPagamentoEM.Valor * percentualClasse / 100), 5);

                            tituloPagarAdiantamento.ListaApropriacaoAdiantamento.Add(apropriacaoAdiantamento);
                        }
                        else
                        {
                            Apropriacao apropriacao = new Apropriacao();
                            apropriacao.CodigoClasse = codigoClasse;
                            apropriacao.CodigoCentroCusto = ordemCompra.CodigoCentroCusto;
                            apropriacao.TituloPagar = formaPagamentoEM.TituloPagar;
                            apropriacao.Percentual = percentualClasseTitulo;
                            apropriacao.Valor = valorClasseTitulo;

                            formaPagamentoEM.TituloPagar.ListaApropriacao.Add(apropriacao);
                        }

                        if (entradaMaterial.TituloFreteId.HasValue && !jaApropriouTituloFrete && entradaMaterial.OrdemCompraFreteId == ordemCompra.Id)
                        {
                            Apropriacao apropriacaoTituloFrete = new Apropriacao();
                            apropriacaoTituloFrete.CodigoClasse = codigoClasse;
                            apropriacaoTituloFrete.CodigoCentroCusto = ordemCompra.CodigoCentroCusto;
                            apropriacaoTituloFrete.TituloPagarId = entradaMaterial.TituloFreteId;
                            apropriacaoTituloFrete.Percentual = percentualClasse;
                            apropriacaoTituloFrete.Valor = decimal.Round((entradaMaterial.TituloFrete.ValorTitulo * percentualClasse / 100), 5);

                            entradaMaterial.TituloFrete.ListaApropriacao.Add(apropriacaoTituloFrete);
                        }
                    }

                    if (entradaMaterial.TituloFreteId.HasValue && entradaMaterial.TituloFrete.ListaApropriacao.Any())
                        jaApropriouTituloFrete = true;

                    if (ordemCompra.ListaItens.All(l => l.Quantidade == l.QuantidadeEntregue))
                    {
                        ordemCompra.Situacao = SituacaoOrdemCompra.Fechada;
                    }
                //}

                #endregion

                #region Gerar Impostos

                int contadorImposto = 1;
                foreach (var imposto in entradaMaterial.ListaImposto)
	            {
                    ImpostoPagar impostoPagar = new ImpostoPagar();
                    impostoPagar.TituloPagar = formaPagamentoEM.TituloPagar;
                    impostoPagar.ImpostoFinanceiroId = imposto.ImpostoFinanceiroId;
                    impostoPagar.BaseCalculo = decimal.Round((imposto.BaseCalculo * percentualTitulo / 100), 5);
                    impostoPagar.ValorImposto = decimal.Round((imposto.Valor * percentualTitulo / 100), 5);

                    formaPagamentoEM.TituloPagar.ListaImpostoPagar.Add(impostoPagar);

                    if (DeveGerarTituloImposto(imposto))
                    {
                        CalcularDataVencimentoImposto(formaPagamentoEM, imposto);

                        TituloPagar tituloPagarImposto = new TituloPagar();
                        tituloPagarImposto.ClienteId = imposto.ImpostoFinanceiro.ClienteId.Value;
                        tituloPagarImposto.TipoCompromissoId = imposto.ImpostoFinanceiro.TipoCompromissoId;
                        tituloPagarImposto.Identificacao = imposto.ImpostoFinanceiro.Sigla + " - " + formaPagamentoEM.TituloPagar.Cliente.Nome;
                        tituloPagarImposto.Situacao = ParametrosOrdemCompra.GeraTituloAguardando.Value ? SituacaoTituloPagar.AguardandoLiberacao : SituacaoTituloPagar.Liberado;
                        tituloPagarImposto.TipoDocumentoId = formaPagamentoEM.TituloPagar.TipoDocumentoId;
                        tituloPagarImposto.Documento = formaPagamentoEM.TituloPagar.Documento + "/" + contadorImposto++;
                        tituloPagarImposto.DataVencimento = imposto.DataVencimento.Value;
                        tituloPagarImposto.DataEmissaoDocumento = formaPagamentoEM.TituloPagar.DataEmissaoDocumento;
                        tituloPagarImposto.TipoTitulo = TipoTitulo.Normal;
                        tituloPagarImposto.ValorTitulo = decimal.Round((imposto.Valor * percentualTitulo / 100), 5);
                        tituloPagarImposto.LoginUsuarioCadastro = UsuarioLogado.Login;
                        tituloPagarImposto.DataCadastro = DateTime.Now;
                        tituloPagarImposto.LoginUsuarioApropriacao = UsuarioLogado.Login;
                        tituloPagarImposto.DataApropriacao = DateTime.Now;
                        tituloPagarImposto.FormaPagamento = formaPagamentoEM.TituloPagar.FormaPagamento;
                        tituloPagarImposto.CodigoInterface = formaPagamentoEM.TituloPagar.CodigoInterface;
                        tituloPagarImposto.SistemaOrigem = formaPagamentoEM.TituloPagar.SistemaOrigem;
                        impostoPagar.TituloPagarImposto = tituloPagarImposto;
                        imposto.TituloPagarImposto = tituloPagarImposto;

                        #region Apropriação dos títulos

                        decimal percentualTotal = formaPagamentoEM.TituloPagar.ListaApropriacao.Sum(l => l.Percentual);
                        if (percentualTotal < 100)
                        {
                            foreach (var apropriacaoTituloOrigem in formaPagamentoEM.TituloPagar.ListaApropriacao)
                            {
                                Apropriacao apropriacao = new Apropriacao();
                                //apropriacao.TituloPagar = tituloPagarImposto;
                                apropriacao.CodigoClasse = apropriacaoTituloOrigem.CodigoClasse;
                                apropriacao.CodigoCentroCusto = apropriacaoTituloOrigem.CodigoCentroCusto;
                                apropriacao.Percentual = decimal.Round((apropriacaoTituloOrigem.Percentual / percentualTotal * 100), 5);
                                apropriacao.Valor = decimal.Round((tituloPagarImposto.ValorTitulo * apropriacaoTituloOrigem.Percentual / percentualTotal), 5);
                                tituloPagarImposto.ListaApropriacao.Add(apropriacao);
                            }
                        }

                        #endregion
                    }
                }

                #endregion
            }
        }

        private void CalcularDataVencimentoImposto(EntradaMaterialFormaPagamento formaPagamentoEM, EntradaMaterialImposto imposto)
        {
            if (imposto.ImpostoFinanceiro.Periodicidade != null && !imposto.DataVencimento.HasValue)
            {
                DateTime dataGeradora = formaPagamentoEM.TituloPagar.DataVencimento;

                if (imposto.ImpostoFinanceiro.FatoGerador == FatoGeradorImpostoFinanceiro.EmissaoDocumento)
                    dataGeradora = formaPagamentoEM.TituloPagar.DataEmissaoDocumento;

                dataGeradora = dataGeradora.AddMonths(1);
                if (imposto.ImpostoFinanceiro.Periodicidade == PeriodicidadeImpostoFinanceiro.Quinzenal)
                {
                    if (dataGeradora.Day > 15)
                        imposto.DataVencimento = new DateTime(dataGeradora.Year, dataGeradora.Month, 15);
                    else
                        imposto.DataVencimento = new DateTime(dataGeradora.Year, dataGeradora.Month, DateTime.DaysInMonth(dataGeradora.Year, dataGeradora.Month));
                }
                else if (imposto.ImpostoFinanceiro.Periodicidade == PeriodicidadeImpostoFinanceiro.Mensal)
                {
                    imposto.DataVencimento = new DateTime(dataGeradora.Year, dataGeradora.Month, imposto.ImpostoFinanceiro.DiaVencimento.Value);
                }

                var listaFeriados = feriadoRepository.ListarTodos();
                while (imposto.DataVencimento.Value.DayOfWeek == DayOfWeek.Saturday
                    || imposto.DataVencimento.Value.DayOfWeek == DayOfWeek.Sunday
                    || listaFeriados.Any(l => l.Data == imposto.DataVencimento))
                {
                    if (imposto.ImpostoFinanceiro.FimDeSemana == FimDeSemanaImpostoFinanceiro.Antecipa)
                    {
                        imposto.DataVencimento.Value.AddDays(-1);
                    }
                    else if (imposto.ImpostoFinanceiro.FimDeSemana == FimDeSemanaImpostoFinanceiro.Posterga)
                    {
                        imposto.DataVencimento.Value.AddDays(1);
                    }
                }
            }
        }

        private bool DeveGerarTituloImposto(EntradaMaterialImposto imposto)
        {
            return ParametrosFinanceiro.GeraTituloImposto && imposto.ImpostoFinanceiro.ClienteId.HasValue && imposto.ImpostoFinanceiro.EhRetido.Value;
        }

        public FileDownloadDTO Exportar(int? id, FormatoExportacaoArquivo formato)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.EntradaMaterialImprimir))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return null;
            }

            var entradaMaterial = ObterPeloIdEUsuario(id,
                UsuarioLogado.Id,
                l => l.CentroCusto,
                l => l.ClienteFornecedor.PessoaFisica,
                l => l.ClienteFornecedor.PessoaJuridica,
                l => l.TipoNotaFiscal,
                l => l.ListaItens.Select(o => o.OrdemCompraItem.Classe),
                l => l.ListaItens.Select(o => o.OrdemCompraItem.Material),
                l => l.ListaFormaPagamento.Select(o => o.OrdemCompraFormaPagamento),
                l => l.ListaFormaPagamento.Select(o => o.TipoCompromisso),
                l => l.ListaFormaPagamento.Select(o => o.TituloPagar),
                l => l.ListaImposto.Select(o => o.ImpostoFinanceiro));
            relEntradaMaterial objRel = new relEntradaMaterial();

            objRel.Database.Tables["OrdemCompra_entradaMaterialRelatorio"].SetDataSource(EntradaMaterialToDataTable(entradaMaterial));
            objRel.Database.Tables["OrdemCompra_entradaMaterialItemRelatorio"].SetDataSource(ListaEntradaMaterialItemToDataTable(entradaMaterial.ListaItens.ToList()));
            objRel.Database.Tables["OrdemCompra_entradaMaterialFormaPagamentoRelatorio"].SetDataSource(ListaFormaPagamentoToDataTable(entradaMaterial.ListaFormaPagamento.ToList()));
            objRel.Database.Tables["OrdemCompra_entradaMaterialImpostoRelatorio"].SetDataSource(ListaImpostoToDataTable(entradaMaterial.ListaImposto.ToList()));

            var centroCusto = centroCustoRepository.ObterPeloCodigo(entradaMaterial.CodigoCentroCusto, l => l.ListaCentroCustoEmpresa);

            var caminhoImagem = PrepararIconeRelatorio(centroCusto, ParametrosOrdemCompra);
            objRel.SetParameterValue("caminhoImagem", caminhoImagem);

            var nomeEmpresa = ObterNomeEmpresa(centroCusto, ParametrosOrdemCompra);
            objRel.SetParameterValue("nomeEmpresa", nomeEmpresa);

            FileDownloadDTO arquivo = new FileDownloadDTO(
                "EntradaMaterial_" + id.ToString(),
                objRel.ExportToStream((ExportFormatType)formato),
                formato);

            RemoverIconeRelatorio(caminhoImagem);

            return arquivo;
        }

        public bool EhPermitidoSalvar(EntradaMaterialDTO dto)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.EntradaMaterialGravar))
                return false;

            return PodeSerSalvaNaSituacaoAtual(dto.Situacao);
        }

        public bool EhPermitidoCancelar(EntradaMaterialDTO dto)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.EntradaMaterialCancelar))
                return false;

            if (!dto.Id.HasValue)
                return false;

            if (!PodeCancelarNaSituacaoAtual(dto.Situacao))
                return false;

            return true;
        }

        public bool EhPermitidoImprimir(EntradaMaterialDTO dto)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.EntradaMaterialImprimir))
                return false;

            if (!dto.Id.HasValue)
                return false;

            return true;
        }

        public bool EhPermitidoLiberarTitulos(EntradaMaterialDTO dto)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.EntradaMaterialLiberar))
                return false;

            if (!dto.Id.HasValue)
                return false;

            if (!PodeLiberarNaSituacaoAtual(dto.Situacao))
                return false;

            return true;
        }

        public bool EhPermitidoAdicionarItem(EntradaMaterialDTO dto)
        {
            return EhPermitidoSalvar(dto);
        }

        public bool EhPermitidoRemoverItem(EntradaMaterialDTO dto)
        {
            return EhPermitidoSalvar(dto);
        }

        public bool EhPermitidoEditarItem(EntradaMaterialDTO dto)
        {
            return EhPermitidoSalvar(dto);
        }

        public bool EhPermitidoEditarCentroCusto(EntradaMaterialDTO dto)
        {
            if (!EhPermitidoSalvar(dto))
                return false;

            if (dto.ListaItens.Count > 0)
                return false;

            return true;
        }

        public bool EhPermitidoEditarFornecedor(EntradaMaterialDTO dto)
        {
            if (!EhPermitidoSalvar(dto))
                return false;

            if (dto.ListaItens.Count > 0)
                return false;

            return true;
        }

        public bool ExisteEstoqueParaCentroCusto(string codigoCentroCusto)
        {
            return estoqueRepository.ObterEstoqueAtivoPeloCentroCusto(codigoCentroCusto) != null;
        }

        public bool ExisteMovimentoNoEstoque(EntradaMaterialDTO dto)
        {
            return dto.ListaMovimentoEstoque.Any();
        }

        public bool HaPossibilidadeCancelamentoEntradaMaterial(int? entradaMaterialId)
        {
            var entradaMaterial = ObterPeloIdEUsuario(entradaMaterialId, UsuarioLogado.Id,
                l => l.ListaItens.Select(o => o.OrdemCompraItem.Material),
                l => l.ListaFormaPagamento.Select(o => o.TituloPagar.ListaImpostoPagar.Select(s => s.TituloPagarImposto)),
                l => l.ListaImposto,
                l => l.ListaMovimentoEstoque,
                l => l.TituloFrete);

            return EhCancelamentoPossivel(entradaMaterial);
        }

        #endregion

        #region Métodos Privados

        #region Rotinas de Cancelamento da Entrada de Material

        private static void LiberarFreteDaOrdemCampra(EntradaMaterial entradaMaterial)
        {
            if (entradaMaterial.OrdemCompraFrete != null)
            {
                entradaMaterial.OrdemCompraFrete.EntradaMaterialFreteId = null;
                entradaMaterial.OrdemCompraFrete.EntradaMaterialFrete = null;
            }
        }

        private void LiberarOrdemCompra(EntradaMaterial entradaMaterial)
        {
            foreach (var item in entradaMaterial.ListaItens)
                item.OrdemCompraItem.OrdemCompra.Situacao = SituacaoOrdemCompra.Liberada;
        }

        private void AtualizarQuantidadeEntregueOrdemCompraItem(EntradaMaterial entradaMaterial)
        {
            foreach (var item in entradaMaterial.ListaItens)
                item.OrdemCompraItem.QuantidadeEntregue -= item.Quantidade;
        }

        private void AtualizarFormaPagamentoParaNaoUtulizada(EntradaMaterial entradaMaterial)
        {
            foreach (var item in entradaMaterial.ListaFormaPagamento.Where(l => !l.OrdemCompraFormaPagamento.EhPagamentoAntecipado.HasValue || l.OrdemCompraFormaPagamento.EhPagamentoAntecipado == false))
                item.OrdemCompraFormaPagamento.EhUtilizada = false;
        }

        private void AtualizarFormaPagamentoAdiantadaParaNaoAssociada(EntradaMaterial entradaMaterial)
        {
            foreach (var item in entradaMaterial.ListaFormaPagamento.Where(l => l.OrdemCompraFormaPagamento.EhPagamentoAntecipado == true))
                item.OrdemCompraFormaPagamento.EhAssociada = false;
        }

        private void DeletarTitulosPagarAdiantamento(EntradaMaterial entradaMaterial)
        {
            List<TituloPagarAdiantamento> listaTituloPagarAdiantamento = entradaMaterial.ListaFormaPagamento
                .Where(l => l.OrdemCompraFormaPagamento.EhPagamentoAntecipado == true)
                .SelectMany(l => l.ListaTituloPagarAdiantamento).ToList();

            for (int i = listaTituloPagarAdiantamento.Count() - 1; i >= 0; i--)
            {
                var listaApropriacaoAdiantamento = listaTituloPagarAdiantamento[i].ListaApropriacaoAdiantamento.ToList();
                for (int j = listaApropriacaoAdiantamento.Count() - 1; j >= 0; j--)
                    tituloPagarRepository.RemoverApropriacaoAdiantamento(listaApropriacaoAdiantamento[j]);

                tituloPagarRepository.RemoverTituloPagarAdiantamento(listaTituloPagarAdiantamento[i]);
            }
        }

        private void ProcessarCancelamentoTitulosDeImpostos(EntradaMaterial entradaMaterial, string motivo, List<TituloPagar> listaTitulosAlterados)
        {
            foreach (var impostoPagar in entradaMaterial.ListaFormaPagamento.Where(l =>
                !l.OrdemCompraFormaPagamento.EhPagamentoAntecipado.HasValue ||
                l.OrdemCompraFormaPagamento.EhPagamentoAntecipado == false)
                    .SelectMany(o => o.OrdemCompraFormaPagamento.TituloPagar.ListaImpostoPagar)
                        .Where(s => s.TituloPagarImpostoId.HasValue))
            {
                CancelarTituloDeImposto(impostoPagar.TituloPagarImposto, motivo, listaTitulosAlterados);
                ProcessarCancelamentoTitulosDeImpostosDesdobrados(impostoPagar.TituloPagarImposto.ListaFilhos, motivo, listaTitulosAlterados);
            }
        }

        private void CancelarTituloDeImposto(TituloPagar titulo, string motivo, List<TituloPagar> listaTitulosAlterados)
        {
            titulo.Situacao = SituacaoTituloPagar.Cancelado;
            titulo.LoginUsuarioSituacao = UsuarioLogado.Login;
            titulo.DataSituacao = DateTime.Now;
            titulo.MotivoCancelamentoInterface = motivo;

            listaTitulosAlterados.Add(titulo);
        }

        private void ProcessarCancelamentoTitulosDeImpostosDesdobrados(IEnumerable<TituloPagar> listaTituloPagarDeImpostos, string motivo, List<TituloPagar> listaTitulosAlterados)
        {
            foreach (var titulo in listaTituloPagarDeImpostos)
            {
                ProcessarCancelamentoTitulosDeImpostosDesdobrados(titulo.ListaFilhos, motivo, listaTitulosAlterados);
                CancelarTituloDeImposto(titulo, motivo, listaTitulosAlterados);
            }
        }

        private void ReverterTitulosPagar(EntradaMaterial entradaMaterial, string motivo, List<TituloPagar> listaTitulosAlterados)
        {
            foreach (var titulo in entradaMaterial.ListaFormaPagamento.Where(l =>
                !l.OrdemCompraFormaPagamento.EhPagamentoAntecipado.HasValue ||
                l.OrdemCompraFormaPagamento.EhPagamentoAntecipado == false)
                    .Select(o => o.OrdemCompraFormaPagamento.TituloPagar))
            {
                titulo.Situacao = SituacaoTituloPagar.Provisionado;
                titulo.Documento = null;
                titulo.TipoDocumentoId = null;
                var ordemCompraFormaPagamentoId = titulo.ListaEntradaMaterialFormaPagamento
                    .Where(l => l.TituloPagarId == titulo.Id)
                    .FirstOrDefault()
                    .OrdemCompraFormaPagamento
                    .OrdemCompraId;
                titulo.Identificacao = "Ref.OC : " + ordemCompraFormaPagamentoId.ToString();
                titulo.ValorImposto = 0;
                titulo.Desconto = 0;
                titulo.DataLimiteDesconto = null;
                titulo.TipoTitulo = TipoTitulo.Normal;

                listaTitulosAlterados.Add(titulo);
                CancelarTitulosPagarDesdobrados(titulo.ListaFilhos, motivo, listaTitulosAlterados);
            }

            ReverterTituloFrete(entradaMaterial, motivo, listaTitulosAlterados);
        }

        private void ReverterTituloFrete(EntradaMaterial entradaMaterial, string motivo, List<TituloPagar> listaTitulosAlterados)
        {
            if (entradaMaterial.TituloFrete != null)
            {
                entradaMaterial.TituloFrete.Situacao = SituacaoTituloPagar.Provisionado;
                entradaMaterial.TituloFrete.TipoDocumentoId = null;
                entradaMaterial.TituloFrete.TipoDocumento = null;
                entradaMaterial.TituloFrete.Documento = null;
                entradaMaterial.TituloFrete.ValorImposto = 0;
                entradaMaterial.TituloFrete.Desconto = 0;
                entradaMaterial.TituloFrete.DataLimiteDesconto = null;
                entradaMaterial.TituloFrete.TipoTitulo = TipoTitulo.Normal;
                CancelarTitulosPagarDesdobrados(entradaMaterial.TituloFrete.ListaFilhos, motivo, listaTitulosAlterados);
            }
        }

        private void CancelarTitulosPagarDesdobrados(IEnumerable<TituloPagar> listaTituloPagar, string motivo, List<TituloPagar> listaTitulosAlterados)
        {
            foreach (var titulo in listaTituloPagar)
            {
                CancelarTitulosPagarDesdobrados(titulo.ListaFilhos, motivo, listaTitulosAlterados);
                titulo.Situacao = SituacaoTituloPagar.Cancelado;
                titulo.TipoTitulo = TipoTitulo.Normal;
                titulo.TituloPaiId = null;
                titulo.TituloPai = null;
                titulo.LoginUsuarioSituacao = UsuarioLogado.Login;
                titulo.DataSituacao = DateTime.Now;
                titulo.MotivoCancelamentoInterface = motivo;

                listaTitulosAlterados.Add(titulo);
            }
        }

        private void ReprovisionarApropriacoesDasFormasPagamentoNaoUtilizadas(EntradaMaterial entradaMaterial)
        {
            foreach (var ordemCompra in entradaMaterial.ListaItens.Select(l => l.OrdemCompraItem.OrdemCompra).Distinct())
            {
                var listaFormaPagamentoNaoUtulizada = ordemCompra.ListaOrdemCompraFormaPagamento.Where(l => l.EhUtilizada == false);
                List<Apropriacao> listaApropriacao = listaFormaPagamentoNaoUtulizada.SelectMany(o => o.TituloPagar.ListaApropriacao).ToList();
                if (entradaMaterial.TituloFreteId.HasValue && entradaMaterial.TituloFrete.Situacao == SituacaoTituloPagar.Provisionado && entradaMaterial.OrdemCompraFreteId == ordemCompra.Id)
                    listaApropriacao.AddRange(entradaMaterial.TituloFrete.ListaApropriacao);

                for (int i = listaApropriacao.Count() - 1; i >= 0; i--)
                {
                    var apropriacao = listaApropriacao.ToList()[i];
                    apropriacaoRepository.Remover(apropriacao);
                }

                var valorTotalPendente = ordemCompra.ListaItens.Where(l => l.Quantidade > l.QuantidadeEntregue).Sum(o => (o.Quantidade - o.QuantidadeEntregue) * o.ValorUnitario);
                var listaCodigoClasses = ordemCompra.ListaItens.Select(l => l.CodigoClasse).Distinct();
                if (valorTotalPendente > 0)
                {
                    foreach (var codigoClasse in listaCodigoClasses)
                    {
                        var valorPendenteClasse = ordemCompra.ListaItens.Where(l => l.CodigoClasse == codigoClasse && l.Quantidade > l.QuantidadeEntregue).Sum(o => (o.Quantidade - o.QuantidadeEntregue) * o.ValorUnitario);
                        var percentualClasse = decimal.Round((valorPendenteClasse / valorTotalPendente * 100).Value, 5);

                        if (valorPendenteClasse > 0)
                        {
                            foreach (var formaPagamento in listaFormaPagamentoNaoUtulizada)
                            {
                                Apropriacao apropriacao = new Apropriacao();
                                apropriacao.CodigoClasse = codigoClasse;
                                apropriacao.CodigoCentroCusto = ordemCompra.CodigoCentroCusto;
                                apropriacao.TituloPagarId = formaPagamento.TituloPagarId;
                                apropriacao.Percentual = percentualClasse;
                                apropriacao.Valor = formaPagamento.Valor * apropriacao.Percentual / 100;

                                formaPagamento.TituloPagar.ListaApropriacao.Add(apropriacao);
                            }

                            if (entradaMaterial.TituloFreteId.HasValue && (entradaMaterial.TituloFrete.Situacao == SituacaoTituloPagar.Provisionado) && entradaMaterial.OrdemCompraFreteId == ordemCompra.Id)
                            {
                                Apropriacao apropriacaoTituloFrete = new Apropriacao();
                                apropriacaoTituloFrete.CodigoClasse = codigoClasse;
                                apropriacaoTituloFrete.CodigoCentroCusto = ordemCompra.CodigoCentroCusto;
                                apropriacaoTituloFrete.TituloPagarId = entradaMaterial.TituloFreteId;
                                apropriacaoTituloFrete.Percentual = percentualClasse;
                                apropriacaoTituloFrete.Valor = entradaMaterial.TituloFrete.ValorTitulo * percentualClasse / 100;

                                entradaMaterial.TituloFrete.ListaApropriacao.Add(apropriacaoTituloFrete);
                            }
                        }
                    }
                }
            }
        }

        private void RemoverImpostoPagar(EntradaMaterial entradaMaterial)
        {
            var listaImpostoPagar = entradaMaterial.ListaFormaPagamento.SelectMany(l => l.TituloPagar.ListaImpostoPagar);
            for (int i = listaImpostoPagar.Count() - 1; i >= 0; i--)
            {
                var impostoPagar = listaImpostoPagar.ToList()[i];
                entradaMaterialRepository.RemoverImpostoPagar(impostoPagar);
            }
        }

        private void AjustarEstoque(EntradaMaterial entradaMaterial)
        {
            if (entradaMaterial.ListaMovimentoEstoque.Any())
            {
                var estoque = estoqueRepository.ObterEstoqueAtivoPeloCentroCusto(entradaMaterial.CodigoCentroCusto);
                if (estoque != null)
                {
                    Movimento movimento = new Movimento();

                    movimento.EstoqueId = estoque.Id;
                    movimento.ClienteFornecedorId = entradaMaterial.ClienteFornecedorId;
                    movimento.TipoMovimento = TipoMovimentoEstoque.SaidaEM;
                    movimento.Data = entradaMaterial.DataEntregaNota;
                    movimento.Observacao = entradaMaterial.Observacao + " - Motivo cancelamento: " + entradaMaterial.MotivoCancelamento;
                    movimento.TipoDocumentoId = entradaMaterial.TipoNotaFiscalId;
                    movimento.Documento = entradaMaterial.NumeroNotaFiscal;
                    movimento.DataEmissao = entradaMaterial.DataEmissaoNota;
                    movimento.DataEntrega = entradaMaterial.DataEntregaNota;
                    movimento.EntradaMaterialId = entradaMaterial.Id;
                    movimento.DataOperacao = DateTime.Now;
                    movimento.LoginUsuarioOperacao = UsuarioLogado.Login;
                    movimento.EhMovimentoTemporario = false;

                    foreach (var entradaMaterialItem in entradaMaterial.ListaItens)
                    {
                        var movimentoItem = new MovimentoItem();
                        movimentoItem.MaterialId = entradaMaterialItem.OrdemCompraItem.MaterialId;
                        movimentoItem.CodigoClasse = entradaMaterialItem.CodigoClasse;
                        movimentoItem.Quantidade = entradaMaterialItem.Quantidade * -1;
                        movimentoItem.Valor = 0;
                        movimentoItem.Observacao = entradaMaterialItem.OrdemCompraItem.Complemento;

                        movimento.ListaMovimentoItem.Add(movimentoItem);

                        var estoqueMaterial = estoque.ListaEstoqueMaterial.Where(l => l.MaterialId == entradaMaterialItem.OrdemCompraItem.MaterialId).SingleOrDefault();

                        if (estoqueMaterial == null)
                        {
                            estoqueMaterial = new EstoqueMaterial();
                            estoqueMaterial.QuantidadeTemporaria = 0;
                            estoqueMaterial.Valor = 0;

                            estoque.ListaEstoqueMaterial.Add(estoqueMaterial);
                        }

                        estoqueMaterial.Quantidade += movimentoItem.Quantidade;
                    }

                    entradaMaterial.ListaMovimentoEstoque.Add(movimento);
                    estoqueRepository.Alterar(estoque);
                }
            }
        }

        private void RemoverEntradaMaterialItem(EntradaMaterial entradaMaterial)
        {
            for (int i = entradaMaterial.ListaItens.Count() - 1; i >= 0; i--)
            {
                var item = entradaMaterial.ListaItens.ToList()[i];
                entradaMaterialRepository.RemoverEntradaMaterialItem(item);
            }
        }

        private void RemoverEntradaMaterialImposto(EntradaMaterial entradaMaterial)
        {
            for (int i = entradaMaterial.ListaImposto.Count() - 1; i >= 0; i--)
            {
                var imposto = entradaMaterial.ListaImposto.ToList()[i];
                entradaMaterialRepository.RemoverEntradaMaterialImposto(imposto);
            }
        }

        private void RemoverEntradaMaterialFormaPagamento(EntradaMaterial entradaMaterial)
        {
            for (int i = entradaMaterial.ListaFormaPagamento.Count() - 1; i >= 0; i--)
            {
                var formaPagamento = entradaMaterial.ListaFormaPagamento.ToList()[i];
                entradaMaterialRepository.RemoverEntradaMaterialFormaPagamento(formaPagamento);
            }
        }

        private void GravarLogOperacaoCancelamento(EntradaMaterial entradaMaterial, List<TituloPagar> listaTitulosAlterados)
        {
            logOperacaoAppService.Gravar("Cancelamento da entrada de material",
                "OrdemCompra.entradaMaterial_Cancela",
                "OrdemCompra.entradaMaterial",
                "UPDATE",
                EntradaMaterialToXML(entradaMaterial));

            logOperacaoAppService.Gravar("Cancelamento da entrada de material (Títulos alterados)",
                "OrdemCompra.entradaMaterial_Cancela",
                "Financeiro.tituloPagar",
                "UPDATE",
                ListaTitulosAlteradosToXML(listaTitulosAlterados, entradaMaterial.Id));
        }

        private static string EntradaMaterialToXML(EntradaMaterial entradaMaterial)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<entradaMaterial>");
            sb.Append("<OrdemCompra.entradaMaterial>");
            sb.Append("<codigo>" + entradaMaterial.Id + "</codigo>");
            sb.Append("<centroCusto>" + entradaMaterial.CodigoCentroCusto + "</centroCusto>");
            sb.Append("<clienteFornecedor>" + entradaMaterial.ClienteFornecedorId + "</clienteFornecedor>");
            sb.Append("<fornecedorNota>" + entradaMaterial.FornecedorNotaId + "</fornecedorNota>");
            sb.Append("<dataEntradaMaterial>" + entradaMaterial.Data + "</dataEntradaMaterial>");
            sb.Append("<situacao>" + (int)entradaMaterial.Situacao + "</situacao>");
            sb.Append("<tipoNotaFiscal>" + entradaMaterial.TipoNotaFiscalId + "</tipoNotaFiscal>");
            sb.Append("<numeroNotaFiscal>" + entradaMaterial.NumeroNotaFiscal + "</numeroNotaFiscal>");
            sb.Append("<dataEmissaoNota>" + entradaMaterial.DataEmissaoNota + "</dataEmissaoNota>");
            sb.Append("<dataEntregaNota>" + entradaMaterial.DataEntregaNota + "</dataEntregaNota>");
            sb.Append("<observacao>" + entradaMaterial.Observacao + "</observacao>");
            sb.Append("<percentualDesconto>" + entradaMaterial.PercentualDesconto + "</percentualDesconto>");
            sb.Append("<desconto>" + entradaMaterial.Desconto + "</desconto>");
            sb.Append("<percentualISS>" + entradaMaterial.PercentualISS + "</percentualISS>");
            sb.Append("<iss>" + entradaMaterial.ISS + "</iss>");
            sb.Append("<freteIncluso>" + entradaMaterial.FreteIncluso + "</freteIncluso>");
            sb.Append("<dataCadastro>" + entradaMaterial.DataCadastro + "</dataCadastro>");
            sb.Append("<usuarioCadastro>" + entradaMaterial.LoginUsuarioCadastro + "</usuarioCadastro>");
            sb.Append("<dataLibera>" + entradaMaterial.DataLiberacao + "</dataLibera>");
            sb.Append("<usuarioLibera>" + entradaMaterial.LoginUsuarioLiberacao + "</usuarioLibera>");
            sb.Append("<dataCancela>" + entradaMaterial.DataCancelamento + "</dataCancela>");
            sb.Append("<usuarioCancela>" + entradaMaterial.LoginUsuarioCancelamento + "</usuarioCancela>");
            sb.Append("<motivoCancela>" + entradaMaterial.MotivoCancelamento + "</motivoCancela>");
            sb.Append("<transportadora>" + entradaMaterial.TransportadoraId + "</transportadora>");
            sb.Append("<dataFrete>" + entradaMaterial.DataFrete + "</dataFrete>");
            sb.Append("<valorFrete>" + entradaMaterial.ValorFrete + "</valorFrete>");
            sb.Append("<tipoNotaFrete>" + entradaMaterial.TipoNotaFreteId + "</tipoNotaFrete>");
            sb.Append("<numeroNotaFrete>" + entradaMaterial.NumeroNotaFrete + "</numeroNotaFrete>");
            sb.Append("<ordemCompraFrete>" + entradaMaterial.OrdemCompraFreteId + "</ordemCompraFrete>");
            sb.Append("<tituloFrete>" + entradaMaterial.TituloFreteId + "</tituloFrete>");
            sb.Append("<tipoCompra>" + entradaMaterial.CodigoTipoCompra + "</tipoCompra>");
            sb.Append("<CIFFOB>" + entradaMaterial.CifFobId + "</CIFFOB>");
            sb.Append("<naturezaOperacao>" + entradaMaterial.CodigoNaturezaOperacao + "</naturezaOperacao>");
            sb.Append("<serieNF>" + entradaMaterial.SerieNFId + "</serieNF>");
            sb.Append("<CST>" + entradaMaterial.CodigoCST + "</CST>");
            sb.Append("<codigoContribuicao>" + entradaMaterial.CodigoContribuicaoId + "</codigoContribuicao>");
            sb.Append("<codigoBarras>" + entradaMaterial.CodigoBarras + "</codigoBarras>");
            sb.Append("<conferido>" + (entradaMaterial.EhConferido == true ? 1 : 0) + "</conferido>");
            sb.Append("<usuarioConferencia>" + entradaMaterial.LoginUsuarioConferencia + "</usuarioConferencia>");
            sb.Append("<dataConferencia>" + entradaMaterial.DataConferencia + "</dataConferencia>");
            sb.Append("</OrdemCompra.entradaMaterial>");
            sb.Append("</entradaMaterial>");

            return sb.ToString();
        }

        private static string ListaTitulosAlteradosToXML(List<TituloPagar> listaTitulosAlterados, int? entradaMaterialId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<Financeiro.tituloPagar>");
            foreach (var tituloPagar in listaTitulosAlterados)
            {
                sb.Append("<Financeiro.tituloPagar>");
                sb.Append("<entradaMaterial>" + entradaMaterialId + "</entradaMaterial>");
                sb.Append("<codigo>" + tituloPagar.Id + "</codigo>");
                sb.Append("<cliente>" + tituloPagar.ClienteId + "</cliente>");
                sb.Append("<tipoCompromisso>" + tituloPagar.TipoCompromissoId + "</tipoCompromisso>");
                sb.Append("<identificacao>" + tituloPagar.Identificacao + "</identificacao>");
                sb.Append("<situacao>" + (int)tituloPagar.Situacao + "</situacao>");
                sb.Append("<tipoDocumento>" + tituloPagar.TipoDocumentoId + "</tipoDocumento>");
                sb.Append("<documento>" + tituloPagar.Documento + "</documento>");
                sb.Append("<dataEmissaoDocumento>" + tituloPagar.DataEmissaoDocumento + "</dataEmissaoDocumento>");
                sb.Append("<dataVencimento>" + tituloPagar.DataVencimento + "</dataVencimento>");
                sb.Append("<tipoTitulo>" + (int)tituloPagar.TipoTitulo + "</tipoTitulo>");
                sb.Append("<tituloPai>" + tituloPagar.TituloPaiId + "</tituloPai>");
                sb.Append("<parcela>" + tituloPagar.Parcela + "</parcela>");
                sb.Append("<valorTitulo>" + tituloPagar.ValorTitulo + "</valorTitulo>");
                sb.Append("<valorImposto>" + tituloPagar.ValorImposto + "</valorImposto>");
                sb.Append("<desconto>" + tituloPagar.Desconto + "</desconto>");
                sb.Append("<dataLimiteDesconto>" + tituloPagar.DataLimiteDesconto + "</dataLimiteDesconto>");
                sb.Append("<multa>" + tituloPagar.Multa + "</multa>");
                sb.Append("<percentualMulta>" + tituloPagar.EhMultaPercentual + "</percentualMulta>");
                sb.Append("<taxaPermanencia>" + tituloPagar.TaxaPermanencia + "</taxaPermanencia>");
                sb.Append("<percentualTaxa>" + tituloPagar.EhTaxaPermanenciaPercentual + "</percentualTaxa>");
                sb.Append("<motivoDesconto>" + tituloPagar.MotivoDesconto + "</motivoDesconto>");
                sb.Append("<dataEmissao>" + tituloPagar.DataEmissao + "</dataEmissao>");
                sb.Append("<dataPagamento>" + tituloPagar.DataPagamento + "</dataPagamento>");
                sb.Append("<dataBaixa>" + tituloPagar.DataBaixa + "</dataBaixa>");
                sb.Append("<operadorCadastro>" + tituloPagar.LoginUsuarioCadastro + "</operadorCadastro>");
                sb.Append("<dataCadastro>" + tituloPagar.DataCadastro + "</dataCadastro>");
                sb.Append("<operadorStatus>" + tituloPagar.LoginUsuarioSituacao + "</operadorStatus>");
                sb.Append("<dataStatus>" + tituloPagar.DataSituacao + "</dataStatus>");
                sb.Append("<operadorApropriacao>" + tituloPagar.LoginUsuarioApropriacao + "</operadorApropriacao>");
                sb.Append("<dataApropriacao>" + tituloPagar.DataApropriacao + "</dataApropriacao>");
                sb.Append("<formaPagamento>" + tituloPagar.FormaPagamento + "</formaPagamento>");
                sb.Append("<codigoInterface>" + tituloPagar.CodigoInterface + "</codigoInterface>");
                sb.Append("<sistemaOrigem>" + tituloPagar.SistemaOrigem + "</sistemaOrigem>");
                sb.Append("<cbBanco>" + tituloPagar.CBBanco + "</cbBanco>");
                sb.Append("<cbMoeda>" + tituloPagar.CBMoeda + "</cbMoeda>");
                sb.Append("<cbCampoLivre>" + tituloPagar.CBCampoLivre + "</cbCampoLivre>");
                sb.Append("<cbDV>" + tituloPagar.CBDV + "</cbDV>");
                sb.Append("<cbValor>" + tituloPagar.CBValor + "</cbValor>");
                sb.Append("<cbDataValor>" + tituloPagar.CBDataValor + "</cbDataValor>");
                sb.Append("<cbBarra>" + tituloPagar.CBBarra + "</cbBarra>");
                sb.Append("<valorPago>" + tituloPagar.ValorPago + "</valorPago>");
                sb.Append("<motivoCancelamento>" + tituloPagar.MotivoCancelamentoId + "</motivoCancelamento>");
                sb.Append("<movimento>" + tituloPagar.MovimentoId + "</movimento>");
                sb.Append("<bancoBaseBordero>" + tituloPagar.BancoBaseBordero + "</bancoBaseBordero>");
                sb.Append("<agenciaContaBaseBordero>" + tituloPagar.AgenciaContaBaseBordero + "</agenciaContaBaseBordero>");
                sb.Append("<contaCorrente>" + tituloPagar.ContaCorrenteId + "</contaCorrente>");
                sb.Append("<retencao>" + tituloPagar.Retencao + "</retencao>");
                sb.Append("<observacao>" + tituloPagar.Observacao + "</observacao>");
                sb.Append("<motivoCancelamentoInterface>" + tituloPagar.MotivoCancelamentoInterface + "</motivoCancelamentoInterface>");
                sb.Append("<pagamentoAntecipado>" + (tituloPagar.EhPagamentoAntecipado == true ? 1 : 0) + "</pagamentoAntecipado>");
                sb.Append("<cbConcessionaria>" + tituloPagar.CBConcessionaria + "</cbConcessionaria>");
                sb.Append("<tituloPrincipalAgrupamento>" + tituloPagar.TituloPrincipalAgrupamentoId + "</tituloPrincipalAgrupamento>");
                sb.Append("</Financeiro.tituloPagar>");
            }
            sb.Append("</Financeiro.tituloPagar>");

            return sb.ToString();
        }

        #endregion

        private bool EhCancelamentoPossivel(EntradaMaterial entradaMaterial)
        {
            if (entradaMaterial == null)
            {
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
                return false;
            }

            if (!PodeCancelarNaSituacaoAtual(entradaMaterial.Situacao))
            {
                var msg = string.Format(Resource.OrdemCompra.ErrorMessages.EntradaMaterialSituacaoInvalida, entradaMaterial.Situacao.ObterDescricao());
                messageQueue.Add(msg, TypeMessage.Error);
                return false;
            }

            if (JaHouvePagamentoEfetuado(entradaMaterial))
                return false;

            if (!EhEstoqueValido(entradaMaterial))
                return false;

            return true;
        }

        private bool EhEstoqueValido(EntradaMaterial entradaMaterial)
        {
            foreach (var item in entradaMaterial.ListaItens)
            {
                if (item.OrdemCompraItem.Material.EhControladoPorEstoque.Value)
                {
                    var estoqueMaterial = estoqueRepository.ObterEstoqueMaterialAtivoPeloCentroCustoEMaterial(entradaMaterial.CodigoCentroCusto, item.OrdemCompraItem.MaterialId, l => l.Estoque);
                    if (estoqueMaterial != null)
                    {
                        if (estoqueMaterial.Estoque.ListaMovimento.Any(l => l.EntradaMaterialId == entradaMaterial.Id))
                        {
                            if (item.Quantidade > estoqueMaterial.Quantidade)
                            {
                                if (estoqueMaterial.Material.EhControladoPorEstoque.Value)
                                {
                                    var msg = "Quantidade do estoque: " + estoqueMaterial.Quantidade.ToString() + "\n";
                                    msg += "Quantidade da entrada de material: " + item.Quantidade.ToString() + "\n";
                                    msg += "A quantidade do material: '" + item.OrdemCompraItem.Material.Descricao + "' que será retirado do estoque '" + estoqueMaterial.Estoque.Descricao + "' está maior que o saldo existente.";
                                    messageQueue.Add(msg, TypeMessage.Error);
                                    return false;
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        private EntradaMaterial ObterPeloIdEUsuario(int? id, int? idUsuario, params Expression<Func<EntradaMaterial, object>>[] includes)
        {
            var specification = (Specification<EntradaMaterial>)new TrueSpecification<EntradaMaterial>();

            if (usuarioAppService.UsuarioPossuiCentroCustoDefinidoNoModulo(idUsuario, Resource.Sigim.NomeModulo.OrdemCompra))
            {
                specification &= EntradaMaterialSpecification.UsuarioPossuiAcessoAoCentroCusto(idUsuario, Resource.Sigim.NomeModulo.OrdemCompra);
            }
            else
            {
                specification &= EntradaMaterialSpecification.EhCentroCustoAtivo();
            }

            return entradaMaterialRepository.ObterPeloId(id, specification, includes);
        }

        private bool PodeSerSalvaNaSituacaoAtual(SituacaoEntradaMaterial situacao)
        {
            return situacao == SituacaoEntradaMaterial.Pendente;
        }

        private bool PodeCancelarNaSituacaoAtual(SituacaoEntradaMaterial situacao)
        {
            return situacao != SituacaoEntradaMaterial.Cancelada;
        }

        private bool PodeLiberarNaSituacaoAtual(SituacaoEntradaMaterial situacao)
        {
            return situacao == SituacaoEntradaMaterial.Pendente;
        }

        private bool JaHouvePagamentoEfetuado(EntradaMaterial entradaMaterial)
        {
            int? tituloPagarId;
            foreach (var formaPagamento in entradaMaterial.ListaFormaPagamento)
            {
                if (PossuiTituloPago(formaPagamento.TituloPagar, out tituloPagarId))
                {
                    var msg = string.Format(Resource.OrdemCompra.ErrorMessages.TituloEstaPago, tituloPagarId.ToString());
                    messageQueue.Add(msg, TypeMessage.Error);
                    return true;
                }

                if (PossuiTituloImpostoPago(formaPagamento.TituloPagar, out tituloPagarId))
                {
                    var msg = string.Format(Resource.OrdemCompra.ErrorMessages.TituloImpostoEstaPago, tituloPagarId.ToString());
                    messageQueue.Add(msg, TypeMessage.Error);
                    return true;
                }
            }

            if (PossuiTituloPago(entradaMaterial.TituloFrete, out tituloPagarId))
            {
                var msg = string.Format(Resource.OrdemCompra.ErrorMessages.TituloFreteEstaPago, tituloPagarId.ToString());
                messageQueue.Add(msg, TypeMessage.Error);
                return true;
            }

            return false;
        }

        private bool PossuiTituloImpostoPago(TituloPagar tituloPagar, out int? tituloPagarId)
        {
            tituloPagarId = null;
            if (tituloPagar != null)
            {
                foreach (var item in tituloPagar.ListaImpostoPagar)
                {
                    if (item.TituloPagarImpostoId.HasValue && item.TituloPagarImposto.TipoTitulo == TipoTitulo.Pai)
                    {
                        if (PossuiTituloDesdobradoPago(item.TituloPagarImposto.ListaFilhos, out tituloPagarId))
                            return true;
                    }
                    else if (EhTituloPago(item.TituloPagarImposto, out tituloPagarId))
                        return true;
                }
            }

            return false;
        }

        private bool PossuiTituloPago(TituloPagar tituloPagar, out int? tituloPagarId)
        {
            tituloPagarId = null;
            if (tituloPagar != null)
            {
                if (tituloPagar.TipoTitulo == TipoTitulo.Pai)
                {
                    if (PossuiTituloDesdobradoPago(tituloPagar.ListaFilhos, out tituloPagarId))
                        return true;
                }
                else if (EhTituloPago(tituloPagar, out tituloPagarId))
                    return true;
            }

            return false;
        }

        private bool PossuiTituloDesdobradoPago(ICollection<TituloPagar> listaFilhos, out int? tituloPagarId)
        {
            tituloPagarId = null;
            foreach (var titulo in listaFilhos)
            {
                if (titulo.TipoTitulo == TipoTitulo.Pai)
                {
                    if (PossuiTituloDesdobradoPago(titulo.ListaFilhos, out tituloPagarId))
                        return true;
                }
                else if (EhTituloPago(titulo, out tituloPagarId))
                    return true;
            }

            return false;
        }

        private static bool EhTituloPago(TituloPagar titulo, out int? tituloPagarId)
        {
            tituloPagarId = null;
            if ((titulo != null)
                && ((titulo.Situacao == SituacaoTituloPagar.Emitido)
                || (titulo.Situacao == SituacaoTituloPagar.Pago)
                || (titulo.Situacao == SituacaoTituloPagar.Baixado)))
            {
                tituloPagarId = titulo.Id;
                return true;
            }

            return false;
        }

        private DataTable EntradaMaterialToDataTable(EntradaMaterial entradaMaterial)
        {
            DataTable dta = new DataTable();
            DataColumn codigo = new DataColumn("codigo");
            DataColumn dataEntradaMaterial = new DataColumn("dataEntradaMaterial");
            DataColumn observacao = new DataColumn("observacao");
            DataColumn siglaTipoNotaFiscal = new DataColumn("siglaTipoNotaFiscal");
            DataColumn numeroNotaFiscal = new DataColumn("numeroNotaFiscal");
            DataColumn dataEmissaoNota = new DataColumn("dataEmissaoNota");
            DataColumn dataEntregaNota = new DataColumn("dataEntregaNota");
            DataColumn codigoDescricaoCentroCusto = new DataColumn("codigoDescricaoCentroCusto");
            DataColumn descricaoSituacao = new DataColumn("descricaoSituacao");
            DataColumn nomeFornecedor = new DataColumn("nomeFornecedor");
            DataColumn nomeFornecedorNota = new DataColumn("nomeFornecedorNota");

            dta.Columns.Add(codigo);
            dta.Columns.Add(dataEntradaMaterial);
            dta.Columns.Add(observacao);
            dta.Columns.Add(siglaTipoNotaFiscal);
            dta.Columns.Add(numeroNotaFiscal);
            dta.Columns.Add(dataEmissaoNota);
            dta.Columns.Add(dataEntregaNota);
            dta.Columns.Add(codigoDescricaoCentroCusto);
            dta.Columns.Add(descricaoSituacao);
            dta.Columns.Add(nomeFornecedor);
            dta.Columns.Add(nomeFornecedorNota);

            DataRow row = dta.NewRow();
            row[codigo] = entradaMaterial.Id;
            row[dataEntradaMaterial] = entradaMaterial.Data.ToString("dd/MM/yyyy");
            row[observacao] = entradaMaterial.Observacao;
            row[siglaTipoNotaFiscal] = entradaMaterial.TipoNotaFiscalId.HasValue ? entradaMaterial.TipoNotaFiscal.Sigla : string.Empty;
            row[numeroNotaFiscal] = entradaMaterial.NumeroNotaFiscal;
            row[dataEmissaoNota] = entradaMaterial.DataEmissaoNota.HasValue ? entradaMaterial.DataEmissaoNota.Value.ToString("dd/MM/yyyy") : string.Empty;
            row[dataEntregaNota] = entradaMaterial.DataEntregaNota.HasValue ? entradaMaterial.DataEntregaNota.Value.ToString("dd/MM/yyyy") : string.Empty;
            row[codigoDescricaoCentroCusto] = !string.IsNullOrEmpty(entradaMaterial.CodigoCentroCusto) ? entradaMaterial.CodigoCentroCusto + " - " + entradaMaterial.CentroCusto.Descricao : string.Empty;
            row[descricaoSituacao] = entradaMaterial.Situacao.ObterDescricao();
            row[nomeFornecedor] = entradaMaterial.ClienteFornecedorId.HasValue ? entradaMaterial.ClienteFornecedor.Nome : string.Empty;
            row[nomeFornecedorNota] = entradaMaterial.FornecedorNotaId.HasValue ? entradaMaterial.FornecedorNota.Nome : string.Empty;
            dta.Rows.Add(row);
            return dta;
        }

        private DataTable ListaEntradaMaterialItemToDataTable(List<EntradaMaterialItem> listaEntradaMaterialItem)
        {
            DataTable dta = new DataTable();
            DataColumn codigo = new DataColumn("codigo", typeof(int));
            DataColumn sequencialEntradaMaterialItem = new DataColumn("sequencialEntradaMaterialItem");
            DataColumn quantidade = new DataColumn("quantidade", typeof(decimal));
            DataColumn valorUnitario = new DataColumn("valorUnitario", typeof(decimal));
            DataColumn percentualIPI = new DataColumn("percentualIPI", typeof(decimal));
            DataColumn percentualDesconto = new DataColumn("percentualDesconto", typeof(decimal));
            DataColumn valorTotal = new DataColumn("valorTotal", typeof(decimal));
            DataColumn material = new DataColumn("material");
            DataColumn descricaoMaterial = new DataColumn("descricaoMaterial");
            DataColumn unidadeMedida = new DataColumn("unidadeMedida");
            DataColumn complementoDescricao = new DataColumn("complementoDescricao");
            DataColumn codigoDescricaoClasse = new DataColumn("codigoDescricaoClasse");
            DataColumn codigoOrdemCompra = new DataColumn("codigoOrdemCompra");
            DataColumn sequencialOrdemCompraItem = new DataColumn("sequencialOrdemCompraItem");

            dta.Columns.Add(codigo);
            dta.Columns.Add(sequencialEntradaMaterialItem);
            dta.Columns.Add(quantidade);
            dta.Columns.Add(valorUnitario);
            dta.Columns.Add(percentualIPI);
            dta.Columns.Add(percentualDesconto);
            dta.Columns.Add(valorTotal);
            dta.Columns.Add(material);
            dta.Columns.Add(descricaoMaterial);
            dta.Columns.Add(unidadeMedida);
            dta.Columns.Add(complementoDescricao);
            dta.Columns.Add(codigoDescricaoClasse);
            dta.Columns.Add(codigoOrdemCompra);
            dta.Columns.Add(sequencialOrdemCompraItem);

            foreach (var item in listaEntradaMaterialItem)
            {
                DataRow row = dta.NewRow();
                row[codigo] = item.Id.Value;
                row[sequencialEntradaMaterialItem] = item.Sequencial;
                row[quantidade] = item.Quantidade;
                row[valorUnitario] = item.ValorUnitario;
                row[percentualIPI] = item.PercentualIPI;
                row[percentualDesconto] = item.PercentualDesconto;
                row[valorTotal] = item.ValorTotal;
                row[material] = item.OrdemCompraItem.MaterialId;
                row[descricaoMaterial] = item.OrdemCompraItem.Material.Descricao;
                row[unidadeMedida] = item.OrdemCompraItem.Material.SiglaUnidadeMedida;
                row[complementoDescricao] = item.OrdemCompraItem.Complemento;
                row[codigoDescricaoClasse] = item.CodigoClasse + " - " + item.Classe.Descricao;
                row[codigoOrdemCompra] = item.OrdemCompraItem.OrdemCompraId;
                row[sequencialOrdemCompraItem] = item.OrdemCompraItem.Sequencial;
                dta.Rows.Add(row);
            }

            return dta;
        }

        private DataTable ListaFormaPagamentoToDataTable(List<EntradaMaterialFormaPagamento> listaFormaPagamento)
        {
            DataTable dta = new DataTable();
            DataColumn codigo = new DataColumn("codigo", typeof(int));
            DataColumn data = new DataColumn("data");
            DataColumn valor = new DataColumn("valor", typeof(decimal));
            DataColumn tituloPagar = new DataColumn("tituloPagar", typeof(int));
            DataColumn codigoOrdemCompra = new DataColumn("codigoOrdemCompra");
            DataColumn descricaoTipoCompromisso = new DataColumn("descricaoTipoCompromisso");
            DataColumn descricaoSituacaoTitulo = new DataColumn("descricaoSituacaoTitulo");

            dta.Columns.Add(codigo);
            dta.Columns.Add(data);
            dta.Columns.Add(valor);
            dta.Columns.Add(tituloPagar);
            dta.Columns.Add(codigoOrdemCompra);
            dta.Columns.Add(descricaoTipoCompromisso);
            dta.Columns.Add(descricaoSituacaoTitulo);

            foreach (var item in listaFormaPagamento)
            {
                DataRow row = dta.NewRow();
                row[codigo] = item.Id.Value;
                row[data] = item.Data.ToString("dd/MM/yyyy");
                row[valor] = item.Valor;
                row[tituloPagar] = item.TituloPagarId;
                row[codigoOrdemCompra] = item.OrdemCompraFormaPagamento.OrdemCompraId;
                row[descricaoTipoCompromisso] = item.TipoCompromissoId.HasValue ? item.TipoCompromisso.Descricao : string.Empty;
                row[descricaoSituacaoTitulo] = item.TituloPagar.Situacao.ObterDescricao();
                dta.Rows.Add(row);
            }

            return dta;
        }

        private DataTable ListaImpostoToDataTable(List<EntradaMaterialImposto> listaImposto)
        {
            DataTable dta = new DataTable();
            DataColumn codigo = new DataColumn("codigo", typeof(int));
            DataColumn baseCalculo = new DataColumn("baseCalculo", typeof(decimal));
            DataColumn valorImposto = new DataColumn("valorImposto", typeof(decimal));
            DataColumn dataVencimento = new DataColumn("dataVencimento");
            DataColumn TituloPagarImposto = new DataColumn("TituloPagarImposto");
            DataColumn sigla = new DataColumn("sigla");
            DataColumn descricao = new DataColumn("descricao");
            DataColumn aliquota = new DataColumn("aliquota", typeof(decimal));

            dta.Columns.Add(codigo);
            dta.Columns.Add(baseCalculo);
            dta.Columns.Add(valorImposto);
            dta.Columns.Add(dataVencimento);
            dta.Columns.Add(TituloPagarImposto);
            dta.Columns.Add(sigla);
            dta.Columns.Add(descricao);
            dta.Columns.Add(aliquota);

            foreach (var imposto in listaImposto)
            {
                DataRow row = dta.NewRow();
                row[codigo] = imposto.Id.Value;
                row[baseCalculo] = imposto.BaseCalculo;
                row[valorImposto] = imposto.Valor;
                row[dataVencimento] = imposto.DataVencimento.HasValue ? imposto.DataVencimento.Value.ToString("dd/MM/yyyy") : string.Empty;
                row[TituloPagarImposto] = imposto.TituloPagarImposto;
                row[sigla] = imposto.ImpostoFinanceiro.Sigla;
                row[descricao] = imposto.ImpostoFinanceiro.Descricao;
                row[aliquota] = imposto.ImpostoFinanceiro.Aliquota;
                dta.Rows.Add(row);
            }

            return dta;
        }

        #endregion
    }
}