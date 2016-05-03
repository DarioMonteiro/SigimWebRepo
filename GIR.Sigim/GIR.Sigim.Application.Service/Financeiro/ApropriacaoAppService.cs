﻿using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using System.Threading.Tasks;
using CrystalDecisions.Shared;
using GIR.Sigim.Application.Service.Admin;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.CredCob;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Application.Filtros.Financeiro;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Domain.Specification.Financeiro;
using GIR.Sigim.Application.Reports.Financeiro;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.Service.CredCob;
using GIR.Sigim.Domain.Repository.CredCob;
using GIR.Sigim.Application.DTO.CredCob;
using GIR.Sigim.Domain.Repository.Orcamento;
using GIR.Sigim.Domain.Entity.Orcamento;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Application.Service.Orcamento;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public class ApropriacaoAppService : BaseAppService, IApropriacaoAppService
    {
        #region Declaração

        private IUsuarioAppService usuarioAppService;
        private IParametrosFinanceiroRepository parametrosFinanceiroRepository;
        private ICentroCustoRepository centroCustoRepository;
        private IClasseRepository classeRepository;
        private IApropriacaoRepository apropriacaoRepository;
        private ITituloCredCobAppService tituloCredCobAppService;
        private ITituloCredCobRepository tituloCredCobRepository;
        private ITituloMovimentoAppService tituloMovimentoAppService;
        private ITituloMovimentoRepository tituloMovimentoRepository;
        private IOrcamentoAppService orcamentoAppService;
        private IOrcamentoRepository orcamentoRepository;
        private IModuloSigimAppService moduloSigimAppService;
        private ICotacaoValoresRepository cotacaoValoresRepository;
        private ICronogramaFisicoFinanceiroRepository cronogramaFisicoFinanceiroRepository;

        #endregion

        #region Construtor

        public ApropriacaoAppService(IUsuarioAppService usuarioAppService,
                                     IParametrosFinanceiroRepository parametrosFinanceiroRepository, 
                                     ICentroCustoRepository centroCustoRepository,
                                     IClasseRepository classeRepository,
                                     IApropriacaoRepository apropriacaoRepository,
                                     ITituloCredCobAppService tituloCredCobAppService,
                                     ITituloCredCobRepository tituloCredCobRepository,
                                     ITituloMovimentoAppService tituloMovimentoAppService,
                                     ITituloMovimentoRepository tituloMovimentoRepository,
                                     IOrcamentoRepository orcamentoRepository,
                                     IOrcamentoAppService orcamentoAppService,
                                     IModuloSigimAppService moduloSigimAppService,
                                     ICotacaoValoresRepository cotacaoValoresRepository,
                                     ICronogramaFisicoFinanceiroRepository cronogramaFisicoFinanceiroRepository,
                                     MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.usuarioAppService = usuarioAppService;
            this.parametrosFinanceiroRepository = parametrosFinanceiroRepository;
            this.centroCustoRepository = centroCustoRepository;
            this.classeRepository = classeRepository;
            this.apropriacaoRepository = apropriacaoRepository;
            this.tituloCredCobAppService = tituloCredCobAppService;
            this.tituloCredCobRepository = tituloCredCobRepository;
            this.tituloMovimentoAppService = tituloMovimentoAppService;
            this.tituloMovimentoRepository = tituloMovimentoRepository;
            this.orcamentoRepository = orcamentoRepository;
            this.orcamentoAppService = orcamentoAppService;
            this.moduloSigimAppService = moduloSigimAppService;
            this.cotacaoValoresRepository = cotacaoValoresRepository;
            this.cronogramaFisicoFinanceiroRepository = cronogramaFisicoFinanceiroRepository;
        }

        #endregion

        #region Métodos IApropriacaoAppService

        public List<ItemListaDTO> ListarTipoPesquisaRelatorioApropriacaoPorClasse()
        {
            return typeof(TipoPesquisaRelatorioApropriacaoPorClasse).ToItemListaDTO();
        }

        public List<ItemListaDTO> ListarOpcoesRelatorioApropriacaoPorClasse()
        {
            return typeof(OpcoesRelatorioApropriacaoPorClasse).ToItemListaDTO();
        }

        public bool EhPermitidoImprimirRelApropriacaoPorClasse()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.RelatorioApropriacaoPorClasseImprimir);
        }

        public List<ApropriacaoClasseCCRelatorioDTO> GerarRelatorioApropriacaoPorClasse(RelApropriacaoPorClasseFiltro filtro, int? usuarioId)
        {
            if (!EhPermitidoImprimirRelApropriacaoPorClasse())
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return null;
            }

            bool situacaoPagamentoPendente = filtro.EhSituacaoAPagarProvisionado || filtro.EhSituacaoAPagarAguardandoLiberacao || filtro.EhSituacaoAPagarLiberado || filtro.EhSituacaoAPagarCancelado;
            bool situacaoPago = filtro.EhSituacaoAPagarEmitido || filtro.EhSituacaoAPagarPago || filtro.EhSituacaoAPagarBaixado;

            bool situacaoRecebimentoPendente = filtro.EhSituacaoAReceberProvisionado || filtro.EhSituacaoAReceberAFatura || filtro.EhSituacaoAReceberFaturado || filtro.EhSituacaoAReceberCancelado;
            bool situacaoRecebido = filtro.EhSituacaoAReceberPreDatado || filtro.EhSituacaoAReceberRecebido || filtro.EhSituacaoAReceberQuitado;

            List<ApropriacaoClasseCCRelatorio> listaApropriacaoClasseRelatorio = new List<ApropriacaoClasseCCRelatorio>();

            if (situacaoPagamentoPendente)
            {
                var specification = (Specification<Apropriacao>)new TrueSpecification<Apropriacao>();
                specification = MontarSpecificationContasAPagarPendentesRelApropriacaoPorClasse(filtro, usuarioId);
                var listaApropriacao =
                 apropriacaoRepository.ListarPeloFiltro(specification,
                                                        l => l.CentroCusto,
                                                        l => l.CentroCusto.ListaUsuarioCentroCusto.Select(u => u.Modulo),
                                                        l => l.CentroCusto.ListaCentroCustoEmpresa,
                                                        l => l.Classe,
                                                        l => l.TituloPagar).To<List<Apropriacao>>();

                GeraListaRelApropriacaoPorClassePagamentosPendentesEhPagos(listaApropriacao, listaApropriacaoClasseRelatorio);
            }
            if (situacaoPago)
            {
                var specification = (Specification<Apropriacao>)new TrueSpecification<Apropriacao>();
                specification = MontarSpecificationContasAPagarPagosRelApropriacaoPorClasse(filtro, usuarioId);
                var listaApropriacao =
                 apropriacaoRepository.ListarPeloFiltro(specification,
                                                        l => l.CentroCusto,
                                                        l => l.CentroCusto.ListaUsuarioCentroCusto.Select(u => u.Modulo),
                                                        l => l.CentroCusto.ListaCentroCustoEmpresa,
                                                        l => l.Classe,
                                                        l => l.TituloPagar).To<List<Apropriacao>>();

                GeraListaRelApropriacaoPorClassePagamentosPendentesEhPagos(listaApropriacao, listaApropriacaoClasseRelatorio);
            }
            if (situacaoRecebimentoPendente)
            {
                var specification = (Specification<Apropriacao>)new TrueSpecification<Apropriacao>();
                specification = MontarSpecificationContasAReceberPendentesRelApropriacaoPorClasse(filtro, usuarioId);
                var listaApropriacao =
                 apropriacaoRepository.ListarPeloFiltro(specification,
                                                        l => l.CentroCusto,
                                                        l => l.CentroCusto.ListaUsuarioCentroCusto.Select(u => u.Modulo),
                                                        l => l.CentroCusto.ListaCentroCustoEmpresa,
                                                        l => l.Classe,
                                                        l => l.TituloReceber).To<List<Apropriacao>>();

                GeraListaRelApropriacaoPorClasseRecebimentosPendentesEhRecebidos(listaApropriacao, listaApropriacaoClasseRelatorio);
            }
            if (situacaoRecebido)
            {
                var specification = (Specification<Apropriacao>)new TrueSpecification<Apropriacao>();
                specification = MontarSpecificationContasAReceberRecebidosRelApropriacaoPorClasse(filtro, usuarioId);
                var listaApropriacao =
                 apropriacaoRepository.ListarPeloFiltro(specification,
                                                        l => l.CentroCusto,
                                                        l => l.CentroCusto.ListaUsuarioCentroCusto.Select(u => u.Modulo),
                                                        l => l.CentroCusto.ListaCentroCustoEmpresa,
                                                        l => l.Classe,
                                                        l => l.TituloReceber).To<List<Apropriacao>>();

                GeraListaRelApropriacaoPorClasseRecebimentosPendentesEhRecebidos(listaApropriacao, listaApropriacaoClasseRelatorio);
            }
            if (filtro.EhMovimentoDebito)
            {
                var specification = (Specification<Apropriacao>)new TrueSpecification<Apropriacao>();
                specification = MontarSpecificationMovimentoRelApropriacaoPorClasse(filtro, usuarioId, TipoMovimentoRelatorioApropriacaoPorClasse.MovimentoDebito);
                var listaApropriacao =
                 apropriacaoRepository.ListarPeloFiltro(specification,
                                                        l => l.CentroCusto,
                                                        l => l.CentroCusto.ListaUsuarioCentroCusto.Select(u => u.Modulo),
                                                        l => l.CentroCusto.ListaCentroCustoEmpresa,
                                                        l => l.Classe,
                                                        l => l.Movimento).To<List<Apropriacao>>();

                GeraListaRelApropriacaoPorClasseMovimentoDebito(listaApropriacao, listaApropriacaoClasseRelatorio);
            }
            if (filtro.EhMovimentoDebitoCaixa)
            {
                var specification = (Specification<Apropriacao>)new TrueSpecification<Apropriacao>();
                specification = MontarSpecificationMovimentoRelApropriacaoPorClasse(filtro, usuarioId, TipoMovimentoRelatorioApropriacaoPorClasse.MovimentoDebitoCaixa);
                var listaApropriacao =
                 apropriacaoRepository.ListarPeloFiltro(specification,
                                                        l => l.CentroCusto,
                                                        l => l.CentroCusto.ListaUsuarioCentroCusto.Select(u => u.Modulo),
                                                        l => l.CentroCusto.ListaCentroCustoEmpresa,
                                                        l => l.Classe,
                                                        l => l.Movimento).To<List<Apropriacao>>();

                GeraListaRelApropriacaoPorClasseMovimentoDebito(listaApropriacao, listaApropriacaoClasseRelatorio);
            }
            if (filtro.EhMovimentoCredito)
            {
                var specification = (Specification<Apropriacao>)new TrueSpecification<Apropriacao>();
                specification = MontarSpecificationMovimentoRelApropriacaoPorClasse(filtro, usuarioId, TipoMovimentoRelatorioApropriacaoPorClasse.MovimentoCredito);
                var listaApropriacao =
                 apropriacaoRepository.ListarPeloFiltro(specification,
                                                        l => l.CentroCusto,
                                                        l => l.CentroCusto.ListaUsuarioCentroCusto.Select(u => u.Modulo),
                                                        l => l.CentroCusto.ListaCentroCustoEmpresa,
                                                        l => l.Classe,
                                                        l => l.Movimento).To<List<Apropriacao>>();

                GeraListaRelApropriacaoPorClasseMovimentoCredito(listaApropriacao, listaApropriacaoClasseRelatorio);
            }
            if (filtro.EhMovimentoCreditoCaixa)
            {
                var specification = (Specification<Apropriacao>)new TrueSpecification<Apropriacao>();
                specification = MontarSpecificationMovimentoRelApropriacaoPorClasse(filtro, usuarioId, TipoMovimentoRelatorioApropriacaoPorClasse.MovimentoCreditoCaixa);
                var listaApropriacao =
                 apropriacaoRepository.ListarPeloFiltro(specification,
                                                        l => l.CentroCusto,
                                                        l => l.CentroCusto.ListaUsuarioCentroCusto.Select(u => u.Modulo),
                                                        l => l.CentroCusto.ListaCentroCustoEmpresa,
                                                        l => l.Classe,
                                                        l => l.Movimento).To<List<Apropriacao>>();

                GeraListaRelApropriacaoPorClasseMovimentoCredito(listaApropriacao, listaApropriacaoClasseRelatorio);
            }
            if (filtro.EhMovimentoCreditoCobranca)
            {
                if (filtro.EhSituacaoAReceberFaturado || filtro.EhSituacaoAReceberRecebido)
                {
                    var specification = (Specification<TituloCredCob>)new TrueSpecification<TituloCredCob>();
                    specification = tituloCredCobAppService.MontarSpecificationMovimentoCredCobRelApropriacaoPorClasse(filtro, usuarioId);

                    var listaTituloCredCob =
                     tituloCredCobRepository.ListarPeloFiltro(specification,
                                                              l => l.Contrato.Unidade.Bloco.CentroCusto,
                                                              l => l.Contrato.Unidade.Bloco.CentroCusto.ListaUsuarioCentroCusto.Select(u => u.Modulo),
                                                              l => l.Contrato.Unidade.Bloco.CentroCusto.ListaCentroCustoEmpresa,
                                                              l => l.Contrato.Venda.Contrato.ListaVendaParticipante,
                                                              l => l.VerbaCobranca.Classe,
                                                              l => l.VendaSerie.Renegociacao,
                                                              l => l.VendaSerie.IndiceCorrecao,
                                                              l => l.VendaSerie.IndiceAtrasoCorrecao,
                                                              l => l.VendaSerie.IndiceReajuste,
                                                              l => l.Indice).To<List<TituloCredCob>>();

                    List<TituloDetalheCredCob> listaTituloDetalheCredCob = tituloCredCobAppService.RecTit(listaTituloCredCob, DateTime.Now.Date, false, false);
                    GeraListaRelApropriacaoPorClasseCreditoCobranca(listaTituloDetalheCredCob, listaApropriacaoClasseRelatorio);
                }


                if ((!filtro.EhSituacaoAReceberRecebido) && filtro.EhSituacaoAReceberQuitado)
                {
                    var specification = (Specification<TituloMovimento>)new TrueSpecification<TituloMovimento>();
                    specification = tituloMovimentoAppService.MontarSpecificationTituloMovimentoRelApropriacaoPorClasse(filtro, usuarioId);

                    var listaTituloMovimento =
                     tituloMovimentoRepository.ListarPeloFiltro(specification,
                                                                l => l.TituloCredCob.Contrato.Unidade.Bloco.CentroCusto.ListaUsuarioCentroCusto.Select(u => u.Modulo),
                                                                l => l.TituloCredCob.Contrato.Unidade.Bloco.CentroCusto.ListaCentroCustoEmpresa,
                                                                l => l.TituloCredCob.Contrato.ListaVendaParticipante,
                                                                l => l.TituloCredCob.VerbaCobranca.Classe,
                                                                l => l.MovimentoFinanceiro).To<List<TituloMovimento>>();
                    GeraListaRelApropriacaoPorClasseCreditoCobrancaTituloMovimento(listaTituloMovimento, listaApropriacaoClasseRelatorio);

                }

            }


            List<ApropriacaoClasseCCRelatorioDTO> listaApropriacaoClasseRelatorioDTO = new List<ApropriacaoClasseCCRelatorioDTO>();
            if (listaApropriacaoClasseRelatorio.Count == 0)
            {
                listaApropriacaoClasseRelatorioDTO = null;
            }
            else
            {
                listaApropriacaoClasseRelatorioDTO = listaApropriacaoClasseRelatorio.To<List<ApropriacaoClasseCCRelatorioDTO>>();
            }

            return listaApropriacaoClasseRelatorioDTO;

        }

        public FileDownloadDTO ExportarRelApropriacaoPorClasse(RelApropriacaoPorClasseFiltro filtro, List<ApropriacaoClasseCCRelatorioDTO> listaApropriacaoClasseRelatorioDTO, FormatoExportacaoArquivo formato)
        {
            relApropriacaoPorClasseSintetico objRel = new relApropriacaoPorClasseSintetico();

            DataTable dtaRelatorio = CriaDataTableApropriacaoClasseCCRelatorio();

            List<ApropriacaoClasseCCRelatorio> listaApropriacaoClasseRelatorio = listaApropriacaoClasseRelatorioDTO.To<List<ApropriacaoClasseCCRelatorio>>();

            if (filtro.OpcoesRelatorio.Value == (int)OpcoesRelatorioApropriacaoPorClasse.Analitico)
            {
                dtaRelatorio = RelApropriacaoPorClasseAnaliticoToDataTable(listaApropriacaoClasseRelatorio);
            }

            objRel.SetDataSource(dtaRelatorio);

            var parametros = parametrosFinanceiroRepository.Obter();
            var centroCusto = centroCustoRepository.ObterPeloCodigo(filtro.CentroCusto.Codigo, l => l.ListaCentroCustoEmpresa);
            var caminhoImagem = PrepararIconeRelatorio(centroCusto, parametros);
            var nomeEmpresa = ObterNomeEmpresa(centroCusto, parametros);

            string situacaoPagarSelecao = MontarStringSituacaoAPagar(filtro);
            string situacaoReceberSelecao = MontaStringSituacaoAReceber(filtro);
            string tipoData = MontaStringTipoPesquisa(filtro);

            objRel.SetParameterValue("DataInicial", filtro.DataInicial.Value.ToString("dd/MM/yyyy"));
            objRel.SetParameterValue("DataFinal", filtro.DataFinal.Value.ToString("dd/MM/yyyy"));
            objRel.SetParameterValue("CentroCusto", centroCusto != null ? centroCusto.Codigo + "-" + centroCusto.Descricao : "");
            if (filtro.OpcoesRelatorio.Value == (int)OpcoesRelatorioApropriacaoPorClasse.Analitico)
            {
                objRel.SetParameterValue("Tipo", "A");
            }
            else
            {
                objRel.SetParameterValue("Tipo", "S");
            }
            objRel.SetParameterValue("nomeEmpresa", nomeEmpresa);
            objRel.SetParameterValue("SituacaoPagar", situacaoPagarSelecao.Trim());
            objRel.SetParameterValue("SituacaoReceber", situacaoReceberSelecao.Trim());
            objRel.SetParameterValue("TipoData", tipoData);
            objRel.SetParameterValue("caminhoImagem", caminhoImagem);

            FileDownloadDTO arquivo = new FileDownloadDTO("Rel. Apropriação por classe", objRel.ExportToStream((ExportFormatType)formato), formato);

            if (System.IO.File.Exists(caminhoImagem))
                System.IO.File.Delete(caminhoImagem);
            return arquivo;

        }

        public bool EhPermitidoImprimirRelAcompanhamentoFinanceiro()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.RelatorioAcompanhamentoFinanceiroImprimir);
        }

        public List<RelAcompanhamentoFinanceiroDTO> ListarPeloFiltroRelAcompanhamentoFinanceiro(RelAcompanhamentoFinanceiroFiltro filtro,
                                                                                                         out int totalRegistros)
        {
            List<RelAcompanhamentoFinanceiroDTO> listaRelAcompanhamentoFinanceiro = new List<RelAcompanhamentoFinanceiroDTO>();
            totalRegistros = 0;

            if (filtro.BaseadoPor == 0)
            {
                listaRelAcompanhamentoFinanceiro = ListarPeloFiltroRelAcompanhamentoFinanceiroPorTitulo(filtro);
            }
            else
            {
                listaRelAcompanhamentoFinanceiro = ListarPeloFiltroRelAcompanhamentoFinanceiroExecutado(filtro);
            }

            totalRegistros = listaRelAcompanhamentoFinanceiro.Count();

            listaRelAcompanhamentoFinanceiro = OrdenaListaRelAcompanhamentoFinanceiroDTO(filtro, listaRelAcompanhamentoFinanceiro);

            int pageCount = filtro.PaginationParameters.PageSize;
            int pageIndex = filtro.PaginationParameters.PageIndex;

            listaRelAcompanhamentoFinanceiro = listaRelAcompanhamentoFinanceiro.Skip(pageCount * pageIndex).Take(pageCount).To<List<RelAcompanhamentoFinanceiroDTO>>();

            return listaRelAcompanhamentoFinanceiro;

        }

        public FileDownloadDTO ExportarRelAcompanhamentoFinanceiro(RelAcompanhamentoFinanceiroFiltro filtro,
                                                                   FormatoExportacaoArquivo formato)
        {
            if (!EhPermitidoImprimirRelAcompanhamentoFinanceiro())
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return null;
            }

            List<RelAcompanhamentoFinanceiroDTO> listaRelAcompanhamentoFinanceiro = new List<RelAcompanhamentoFinanceiroDTO>();

            if (filtro.BaseadoPor == 0)
            {
                listaRelAcompanhamentoFinanceiro = ListarPeloFiltroRelAcompanhamentoFinanceiroPorTitulo(filtro);
            }
            else
            {
                listaRelAcompanhamentoFinanceiro = ListarPeloFiltroRelAcompanhamentoFinanceiroExecutado(filtro);
            }

            listaRelAcompanhamentoFinanceiro = listaRelAcompanhamentoFinanceiro.OrderBy(l => l.CodigoClasse).ToList<RelAcompanhamentoFinanceiroDTO>();

            FileDownloadDTO arquivo = new FileDownloadDTO("Rel. Acompanhamento financeiro", null, formato);

            var parametros = parametrosFinanceiroRepository.Obter();
            CentroCusto centroCusto = new CentroCusto();
            if (filtro.CentroCusto != null)
            {
                centroCusto = centroCustoRepository.ObterPeloCodigo(filtro.CentroCusto.Codigo, l => l.ListaCentroCustoEmpresa);
            }

            var caminhoImagem = PrepararIconeRelatorio(centroCusto, parametros);
            var nomeEmpresa = ObterNomeEmpresa(centroCusto, parametros);
            string classesFiltro = "";
            if (filtro.ListaClasse.Count > 0)
            {
                classesFiltro = filtro.ListaClasse[0].Codigo;
                for (int i = 1; i <= filtro.ListaClasse.Count - 1; i++)
                {
                    classesFiltro = classesFiltro + "," + filtro.ListaClasse.ToList()[i].Codigo;
                }
            }
            else
            {
                classesFiltro = "Todas";
            }

            DateTime dataOrcamentoInicial = new DateTime();
            var orcamento = orcamentoRepository.ObterPrimeiroOrcamentoPeloCentroCusto(filtro.CentroCusto.Codigo);
            if (orcamento != null)
            {
                if (orcamento.Data.HasValue)
                { 
                    dataOrcamentoInicial = orcamento.Data.Value.Date;
                }
            }

            DateTime dataOrcamentoFinal = new DateTime();
            orcamento = orcamentoRepository.ObterUltimoOrcamentoPeloCentroCusto(filtro.CentroCusto.Codigo);
            if (orcamento != null)
            {
                if (orcamento.Data.HasValue)
                { 
                    dataOrcamentoFinal = orcamento.Data.Value.Date;
                }
            }

            string tipo = "";
            if (filtro.EhValorCorrigido)
            {
                tipo = tipo + " - Valor corrigido";
            }
            else
            {
                if (filtro.IndiceId.HasValue)
                {
                    tipo = tipo + " - Índice";
                }
            }

            string descricaoIndice = "";
            string descricaoIndiceOrcamento = "";
            string descricaoIndicePeriodo = "";
            int defasagem = filtro.Defasagem.HasValue ? (filtro.Defasagem.Value * -1) : 0;

            if ((filtro.IndiceId.HasValue) && (filtro.IndiceId.Value > 0))
            {
                DateTime dataDefasada = new DateTime();
                decimal cotacaoReajuste = 1;

                var cotacao = cotacaoValoresRepository.ListarPeloFiltro(l => l.IndiceFinanceiroId == filtro.IndiceId.Value, l => l.IndiceFinanceiro).FirstOrDefault();
                if (cotacao != null)
                {
                    if (cotacao.IndiceFinanceiro != null)
                    {
                        descricaoIndice = cotacao.IndiceFinanceiro.Indice;
                    }
                }

                if (orcamento != null)
                {
                    dataDefasada = dataOrcamentoFinal.AddMonths(defasagem);
                    cotacaoReajuste = cotacaoValoresRepository.RecuperaCotacao(filtro.IndiceId.Value, dataDefasada.Date);
                    descricaoIndiceOrcamento = dataDefasada.Date.ToShortDateString() + " - " + Math.Round(cotacaoReajuste, 5).ToString();
                }

                if (filtro.DataFinal.HasValue)
                {
                    dataDefasada = filtro.DataFinal.Value.Date.AddMonths(defasagem);
                    cotacaoReajuste = cotacaoValoresRepository.RecuperaCotacao(filtro.IndiceId.Value, dataDefasada.Date);
                    descricaoIndicePeriodo = dataDefasada.Date.ToShortDateString() + " - " + Math.Round(cotacaoReajuste, 5).ToString(); 
                }
            }

            switch (filtro.BaseadoPor)
            {
                case 0:
                    relAcompanhamentoFinanceiro objRelTituloFinan = new relAcompanhamentoFinanceiro();
                    objRelTituloFinan.SetDataSource(RelAcompanhamentoFinanceiroToDataTable(listaRelAcompanhamentoFinanceiro));

                    objRelTituloFinan.SetParameterValue("dataInicial", filtro.DataInicial.Value.ToString("dd/MM/yyyy"));
                    objRelTituloFinan.SetParameterValue("dataFinal", filtro.DataFinal.Value.ToString("dd/MM/yyyy"));
                    objRelTituloFinan.SetParameterValue("centroCustoDescricao", centroCusto != null ? (centroCusto.Codigo + " - " + centroCusto.Descricao) : "");
                    objRelTituloFinan.SetParameterValue("classes", classesFiltro);
                    objRelTituloFinan.SetParameterValue("orcamentoInicial", dataOrcamentoInicial.ToShortDateString());
                    objRelTituloFinan.SetParameterValue("orcamentoFinal", dataOrcamentoFinal.ToShortDateString());
                    objRelTituloFinan.SetParameterValue("descricaoIndice", descricaoIndice);
                    objRelTituloFinan.SetParameterValue("nomeEmpresa", nomeEmpresa);
                    objRelTituloFinan.SetParameterValue("defasagem", defasagem);
                    objRelTituloFinan.SetParameterValue("descricaoIndiceOrcamento", descricaoIndiceOrcamento);
                    objRelTituloFinan.SetParameterValue("descricaoIndicePeriodo", descricaoIndicePeriodo);
                    objRelTituloFinan.SetParameterValue("tipo", tipo);

                    objRelTituloFinan.SetParameterValue("caminhoImagem", caminhoImagem);

                    arquivo = new FileDownloadDTO("Rel. Acompanhamento financeiro por título",
                                                    objRelTituloFinan.ExportToStream((ExportFormatType)formato),
                                                    formato);

                    break;

                case 1:
                    relAcompanhamentoFinanceiroExecutado objRelTituloFinanExecutado = new relAcompanhamentoFinanceiroExecutado();
                    objRelTituloFinanExecutado.SetDataSource(RelAcompanhamentoFinanceiroToDataTable(listaRelAcompanhamentoFinanceiro));

                    objRelTituloFinanExecutado.SetParameterValue("dataInicial", filtro.DataInicial.Value.ToString("dd/MM/yyyy"));
                    objRelTituloFinanExecutado.SetParameterValue("dataFinal", filtro.DataFinal.Value.ToString("dd/MM/yyyy"));
                    objRelTituloFinanExecutado.SetParameterValue("centroCustoDescricao", centroCusto != null ? (centroCusto.Codigo + " - " + centroCusto.Descricao) : "");
                    objRelTituloFinanExecutado.SetParameterValue("classes", classesFiltro);
                    objRelTituloFinanExecutado.SetParameterValue("orcamentoInicial", dataOrcamentoInicial.ToShortDateString());
                    objRelTituloFinanExecutado.SetParameterValue("orcamentoFinal", dataOrcamentoFinal.ToShortDateString());
                    objRelTituloFinanExecutado.SetParameterValue("descricaoIndice", descricaoIndice);
                    objRelTituloFinanExecutado.SetParameterValue("nomeEmpresa", nomeEmpresa);
                    objRelTituloFinanExecutado.SetParameterValue("defasagem", defasagem);
                    objRelTituloFinanExecutado.SetParameterValue("descricaoIndiceOrcamento", descricaoIndiceOrcamento);
                    objRelTituloFinanExecutado.SetParameterValue("descricaoIndicePeriodo", descricaoIndicePeriodo);
                    objRelTituloFinanExecutado.SetParameterValue("tipo", tipo);

                    objRelTituloFinanExecutado.SetParameterValue("caminhoImagem", caminhoImagem);

                    arquivo = new FileDownloadDTO("Rel. Acompanhamento financeiro por título",
                                                    objRelTituloFinanExecutado.ExportToStream((ExportFormatType)formato),
                                                    formato);
                    break;


                default:
                    break;
            }

            if (System.IO.File.Exists(caminhoImagem))
            {
                System.IO.File.Delete(caminhoImagem);
            }

            return arquivo;
        }


        #endregion

        #region "Métodos privados"

        private void AdicionarPercentualExecutadoEhComprometidoNaClassePaiRelAcompanhamentoFinanceiro(string codigoClassePai, RelAcompanhamentoFinanceiroDTO registro, List<RelAcompanhamentoFinanceiroDTO> listaRelAcompanhamentoFinanceiro)
        {
            if (!string.IsNullOrEmpty(codigoClassePai))
            {
                RelAcompanhamentoFinanceiroDTO registroPai = listaRelAcompanhamentoFinanceiro.Where(l => l.CodigoClasse == codigoClassePai).FirstOrDefault();
                decimal totalValorExecutado = registroPai.DespesaAcumulada;
                decimal totalValorComprometido = registroPai.ComprometidoFuturo;

                decimal totalPercentualExecutado = 0;
                if ((totalValorExecutado > 0) && (registro.PercentualExecutado > 0))
                {
                    totalPercentualExecutado = ((registro.DespesaAcumulada * 100) / totalValorExecutado);
                    totalPercentualExecutado = ((totalPercentualExecutado * registro.PercentualExecutado) / 100);
                    totalPercentualExecutado = Math.Round(totalPercentualExecutado, 5);
                }

                decimal totalPercentualComprometido = 0;
                if ((totalValorComprometido > 0) && (registro.PercentualComprometido > 0))
                {
                    totalPercentualComprometido = ((registro.ComprometidoFuturo * 100) / totalValorComprometido);
                    totalPercentualComprometido = ((totalPercentualComprometido * registro.PercentualComprometido) / 100);
                    totalPercentualComprometido = Math.Round(totalPercentualComprometido, 5);
                }

                registroPai.PercentualExecutado = registroPai.PercentualExecutado + totalPercentualExecutado;
                registroPai.PercentualComprometido = registroPai.PercentualComprometido + totalPercentualComprometido;

                registroPai.AssinalarRegistro = false;
                if (((registroPai.DespesaAcumulada == 0) && (registroPai.PercentualExecutado > 0)) ||
                    ((registroPai.DespesaAcumulada > 0) && (registroPai.PercentualExecutado == 0)))
                {
                    registroPai.AssinalarRegistro = true;
                }

                AdicionarPercentualExecutadoEhComprometidoNaClassePaiRelAcompanhamentoFinanceiro(registroPai.CodigoClassePai, registro, listaRelAcompanhamentoFinanceiro);
            }
        }

        private void AdicionarValorNaClassePaiRelAcompanhamentoFinanceiroOutrasColunas(string codigoClassePai, RelAcompanhamentoFinanceiroDTO registro, List<RelAcompanhamentoFinanceiroDTO> listaRelAcompanhamentoFinanceiro, bool ehExecutado)
        {
            if (!string.IsNullOrEmpty(codigoClassePai))
            {
                RelAcompanhamentoFinanceiroDTO registroPai = listaRelAcompanhamentoFinanceiro.Where(l => l.CodigoClasse == codigoClassePai).FirstOrDefault();
                if (ehExecutado)
                {
                    registroPai.ComprometidoFuturo = registroPai.ComprometidoFuturo + registro.ComprometidoFuturo;
                }
                registroPai.ResultadoAcrescimo = registroPai.ResultadoAcrescimo + registro.ResultadoAcrescimo;
                registroPai.ResultadoSaldo = registroPai.ResultadoSaldo + registro.ResultadoSaldo;
                registroPai.Conclusao = registroPai.Conclusao + registro.Conclusao;

                AdicionarValorNaClassePaiRelAcompanhamentoFinanceiroOutrasColunas(registroPai.CodigoClassePai, registro, listaRelAcompanhamentoFinanceiro, ehExecutado);
            }
        }

        private decimal ObterPercentualExecutado(string codigoCentroCusto, string codigoClasse, DateTime data)
        {
            decimal percentual = 0;

            List<CronogramaFisicoFinanceiro> listaCronogramaFisico = cronogramaFisicoFinanceiroRepository.ListarPeloFiltro(l => l.CodigoCentroCusto == codigoCentroCusto, l => l.ListaCronogramaFisicoFinanceiroDetalhe).To<List<CronogramaFisicoFinanceiro>>();
            if (listaCronogramaFisico.Count > 0)
            {
                CronogramaFisicoFinanceiro cronogramaFisico = listaCronogramaFisico.Where(l => l.DataElaboracao <= data).Last();
                if (cronogramaFisico.ListaCronogramaFisicoFinanceiroDetalhe.Count > 0)
                {
                    CronogramaFisicoFinanceiroDetalhe cronogramaFisicoDetalhe = cronogramaFisico.ListaCronogramaFisicoFinanceiroDetalhe.Where(l => l.CodigoClasse == codigoClasse).FirstOrDefault();
                    if (cronogramaFisicoDetalhe != null)
                    {
                        percentual = cronogramaFisicoDetalhe.Percentual;
                    }
                }
            }

            return percentual;
        }

        private List<RelAcompanhamentoFinanceiroDTO> OrdenaListaRelAcompanhamentoFinanceiroDTO(RelAcompanhamentoFinanceiroFiltro filtro, List<RelAcompanhamentoFinanceiroDTO> listaRelAcompanhamentoFinanceiro)
        {
            int pageCount = filtro.PaginationParameters.PageSize;
            int pageIndex = filtro.PaginationParameters.PageIndex;

            switch (filtro.PaginationParameters.OrderBy)
            {
                case "classe":
                    if (filtro.PaginationParameters.Ascending) { listaRelAcompanhamentoFinanceiro = listaRelAcompanhamentoFinanceiro.OrderBy(l => l.CodigoClasse).ToList<RelAcompanhamentoFinanceiroDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelAcompanhamentoFinanceiro = listaRelAcompanhamentoFinanceiro.OrderByDescending(l => l.CodigoClasse).ToList<RelAcompanhamentoFinanceiroDTO>(); }
                    break;
                case "classeDescricao":
                    if (filtro.PaginationParameters.Ascending) { listaRelAcompanhamentoFinanceiro = listaRelAcompanhamentoFinanceiro.OrderBy(l => l.DescricaoClasse).ToList<RelAcompanhamentoFinanceiroDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelAcompanhamentoFinanceiro = listaRelAcompanhamentoFinanceiro.OrderByDescending(l => l.DescricaoClasse).ToList<RelAcompanhamentoFinanceiroDTO>(); }
                    break;
                case "orcamentoInicial":
                    if (filtro.PaginationParameters.Ascending) { listaRelAcompanhamentoFinanceiro = listaRelAcompanhamentoFinanceiro.OrderBy(l => l.OrcamentoInicial).ToList<RelAcompanhamentoFinanceiroDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelAcompanhamentoFinanceiro = listaRelAcompanhamentoFinanceiro.OrderByDescending(l => l.OrcamentoInicial).ToList<RelAcompanhamentoFinanceiroDTO>(); }
                    break;
                case "orcamentoAtual":
                    if (filtro.PaginationParameters.Ascending) { listaRelAcompanhamentoFinanceiro = listaRelAcompanhamentoFinanceiro.OrderBy(l => l.OrcamentoAtual).ToList<RelAcompanhamentoFinanceiroDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelAcompanhamentoFinanceiro = listaRelAcompanhamentoFinanceiro.OrderByDescending(l => l.OrcamentoAtual).ToList<RelAcompanhamentoFinanceiroDTO>(); }
                    break;
                case "despesaPeriodo":
                    if (filtro.PaginationParameters.Ascending) { listaRelAcompanhamentoFinanceiro = listaRelAcompanhamentoFinanceiro.OrderBy(l => l.DespesaPeriodo).ToList<RelAcompanhamentoFinanceiroDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelAcompanhamentoFinanceiro = listaRelAcompanhamentoFinanceiro.OrderByDescending(l => l.DespesaPeriodo).ToList<RelAcompanhamentoFinanceiroDTO>(); }
                    break;
                case "despesaAcumulada":
                    if (filtro.PaginationParameters.Ascending) { listaRelAcompanhamentoFinanceiro = listaRelAcompanhamentoFinanceiro.OrderBy(l => l.DespesaAcumulada).ToList<RelAcompanhamentoFinanceiroDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelAcompanhamentoFinanceiro = listaRelAcompanhamentoFinanceiro.OrderByDescending(l => l.DespesaAcumulada).ToList<RelAcompanhamentoFinanceiroDTO>(); }
                    break;
                case "percentualExecutado":
                    if (filtro.PaginationParameters.Ascending) { listaRelAcompanhamentoFinanceiro = listaRelAcompanhamentoFinanceiro.OrderBy(l => l.PercentualExecutado).ToList<RelAcompanhamentoFinanceiroDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelAcompanhamentoFinanceiro = listaRelAcompanhamentoFinanceiro.OrderByDescending(l => l.PercentualExecutado).ToList<RelAcompanhamentoFinanceiroDTO>(); }
                    break;
                case "comprometidoPendente":
                    if (filtro.PaginationParameters.Ascending) { listaRelAcompanhamentoFinanceiro = listaRelAcompanhamentoFinanceiro.OrderBy(l => l.ComprometidoPendente).ToList<RelAcompanhamentoFinanceiroDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelAcompanhamentoFinanceiro = listaRelAcompanhamentoFinanceiro.OrderByDescending(l => l.ComprometidoPendente).ToList<RelAcompanhamentoFinanceiroDTO>(); }
                    break;
                case "comprometidoFuturo":
                    if (filtro.PaginationParameters.Ascending) { listaRelAcompanhamentoFinanceiro = listaRelAcompanhamentoFinanceiro.OrderBy(l => l.ComprometidoFuturo).ToList<RelAcompanhamentoFinanceiroDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelAcompanhamentoFinanceiro = listaRelAcompanhamentoFinanceiro.OrderByDescending(l => l.ComprometidoFuturo).ToList<RelAcompanhamentoFinanceiroDTO>(); }
                    break;
                case "percentualComprometido":
                    if (filtro.PaginationParameters.Ascending) { listaRelAcompanhamentoFinanceiro = listaRelAcompanhamentoFinanceiro.OrderBy(l => l.PercentualComprometido).ToList<RelAcompanhamentoFinanceiroDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelAcompanhamentoFinanceiro = listaRelAcompanhamentoFinanceiro.OrderByDescending(l => l.PercentualComprometido).ToList<RelAcompanhamentoFinanceiroDTO>(); }
                    break;
                case "resultadoAcrescimo":
                    if (filtro.PaginationParameters.Ascending) { listaRelAcompanhamentoFinanceiro = listaRelAcompanhamentoFinanceiro.OrderBy(l => l.ResultadoAcrescimo).ToList<RelAcompanhamentoFinanceiroDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelAcompanhamentoFinanceiro = listaRelAcompanhamentoFinanceiro.OrderByDescending(l => l.ResultadoAcrescimo).ToList<RelAcompanhamentoFinanceiroDTO>(); }
                    break;
                case "resultadoSaldo":
                    if (filtro.PaginationParameters.Ascending) { listaRelAcompanhamentoFinanceiro = listaRelAcompanhamentoFinanceiro.OrderBy(l => l.ResultadoSaldo).ToList<RelAcompanhamentoFinanceiroDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelAcompanhamentoFinanceiro = listaRelAcompanhamentoFinanceiro.OrderByDescending(l => l.ResultadoSaldo).ToList<RelAcompanhamentoFinanceiroDTO>(); }
                    break;
                case "descricaoClasseFechada":
                    if (filtro.PaginationParameters.Ascending) { listaRelAcompanhamentoFinanceiro = listaRelAcompanhamentoFinanceiro.OrderBy(l => l.DescricaoClasseFechada).ToList<RelAcompanhamentoFinanceiroDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelAcompanhamentoFinanceiro = listaRelAcompanhamentoFinanceiro.OrderByDescending(l => l.DescricaoClasseFechada).ToList<RelAcompanhamentoFinanceiroDTO>(); }
                    break;
                case "conclusao":
                    if (filtro.PaginationParameters.Ascending) { listaRelAcompanhamentoFinanceiro = listaRelAcompanhamentoFinanceiro.OrderBy(l => l.Conclusao).ToList<RelAcompanhamentoFinanceiroDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelAcompanhamentoFinanceiro = listaRelAcompanhamentoFinanceiro.OrderByDescending(l => l.Conclusao).ToList<RelAcompanhamentoFinanceiroDTO>(); }
                    break;
                default:
                    break;

            }

            return listaRelAcompanhamentoFinanceiro;
        }

        private void AdicionarValorNaClassePaiRelAcompanhamentoFinanceiro(decimal valor, Classe classe, List<RelAcompanhamentoFinanceiroDTO> listaRelAcompanhamentoFinanceiro,string tipoColuna)
        {
            if (classe != null)
            {
                RelAcompanhamentoFinanceiroDTO relat = new RelAcompanhamentoFinanceiroDTO();

                if (!listaRelAcompanhamentoFinanceiro.Any(l => l.CodigoClasse == classe.Codigo))
                {
                    relat.CodigoClasse = classe.Codigo;
                    relat.DescricaoClasse = classe.Descricao;
                    relat.CodigoClassePai = classe.CodigoPai;
                    relat.ClassePossuiFilhos = false;
                    if ((classe.ListaFilhos != null) && (classe.ListaFilhos.Count > 0))
                    {
                        relat.ClassePossuiFilhos = true;
                    }

                    switch (tipoColuna)
                    {
                        case "OrcamentoInicial":
                            relat.OrcamentoInicial = valor;
                            break;
                        case "OrcamentoAtual":
                            relat.OrcamentoAtual = valor;
                            break;
                        case "DespesaPeriodo":
                            relat.DespesaPeriodo = valor;
                            break;
                        case "DespesaAcumulada":
                            relat.DespesaAcumulada = valor;
                            break;
                        case "ComprometidoPendente":
                            relat.ComprometidoPendente = valor;
                            break;
                        case "ComprometidoFuturo":
                            relat.ComprometidoFuturo = valor;
                            break;
                        default:
                            break;
                    }

                    listaRelAcompanhamentoFinanceiro.Add(relat);

                }
                else
                {
                    relat = listaRelAcompanhamentoFinanceiro.Where(l => l.CodigoClasse == classe.Codigo).FirstOrDefault();

                    switch (tipoColuna)
                    {
                        case "OrcamentoInicial":
                            relat.OrcamentoInicial = relat.OrcamentoInicial + valor;
                            break;
                        case "OrcamentoAtual":
                            relat.OrcamentoAtual = relat.OrcamentoAtual + valor;
                            break;
                        case "DespesaPeriodo":
                            relat.DespesaPeriodo = relat.DespesaPeriodo + valor;
                            break;
                        case "DespesaAcumulada":
                            relat.DespesaAcumulada = relat.DespesaAcumulada + valor;
                            break;
                        case "ComprometidoPendente":
                            relat.ComprometidoPendente = relat.ComprometidoPendente + valor;
                            break;
                        case "ComprometidoFuturo":
                            relat.ComprometidoFuturo = relat.ComprometidoFuturo + valor;
                            break;
                        default:
                            break;
                    }

                }

                AdicionarValorNaClassePaiRelAcompanhamentoFinanceiro(valor, classe.ClassePai, listaRelAcompanhamentoFinanceiro, tipoColuna);
            }
        }

        private Specification<Apropriacao> MontarSpecificationDespesaPorPeriodoTituloPagarRelAcompanhamentoFinanceiro(RelAcompanhamentoFinanceiroFiltro filtro)
        {
            var specification = (Specification<Apropriacao>)new TrueSpecification<Apropriacao>();

            if (filtro.CentroCusto != null)
            {
                specification &= ApropriacaoSpecification.PertenceAoCentroCustoIniciadoPor(filtro.CentroCusto.Codigo);
            }

            specification &= (ApropriacaoSpecification.EhSituacaoAPagarBaixado(true)) ||
                (ApropriacaoSpecification.EhSituacaoAPagarPago(true)) ||
                (ApropriacaoSpecification.EhSituacaoAPagarEmitido(true));

            specification &= ApropriacaoSpecification.DataPeriodoPorEmissaoTituloPagarMaiorOuIgualRelAcompanhamentoFinanceiro(filtro.DataInicial);
            specification &= ApropriacaoSpecification.DataPeriodoPorEmissaoTituloPagarMenorOuIgualRelAcompanhamentoFinanceiro(filtro.DataFinal);

            return specification;
        }

        private Specification<Apropriacao> MontarSpecificationDespesaAcumuladaTituloPagarRelAcompanhamentoFinanceiro(RelAcompanhamentoFinanceiroFiltro filtro)
        {
            var specification = (Specification<Apropriacao>)new TrueSpecification<Apropriacao>();

            if (filtro.CentroCusto != null)
            {
                specification &= ApropriacaoSpecification.PertenceAoCentroCustoIniciadoPor(filtro.CentroCusto.Codigo);
            }

            specification &= (ApropriacaoSpecification.EhSituacaoAPagarBaixado(true)) ||
                (ApropriacaoSpecification.EhSituacaoAPagarPago(true)) ||
                (ApropriacaoSpecification.EhSituacaoAPagarEmitido(true));

            specification &= ApropriacaoSpecification.DataPeriodoPorEmissaoTituloPagarMenorOuIgualRelAcompanhamentoFinanceiro(filtro.DataFinal);

            return specification;
        }

        private Specification<Apropriacao> MontarSpecificationDespesaAcumuladaTituloPagarRelAcompanhamentoFinanceiroExecutado(RelAcompanhamentoFinanceiroFiltro filtro)
        {
            var specification = (Specification<Apropriacao>)new TrueSpecification<Apropriacao>();

            if (filtro.CentroCusto != null)
            {
                specification &= ApropriacaoSpecification.PertenceAoCentroCustoIniciadoPor(filtro.CentroCusto.Codigo);
            }

            specification &= (ApropriacaoSpecification.EhSituacaoAPagarBaixado(true)) ||
                (ApropriacaoSpecification.EhSituacaoAPagarPago(true)) ||
                (ApropriacaoSpecification.EhSituacaoAPagarEmitido(true));

            specification &= ApropriacaoSpecification.EhTipoTituloDiferenteDeTituloPai();

            return specification;
        }

        private Specification<Apropriacao> MontarSpecificationDespesaAcumuladaTituloPagarAguardandoEhLiberadosRelAcompanhamentoFinanceiroExecutado(RelAcompanhamentoFinanceiroFiltro filtro)
        {
            var specification = (Specification<Apropriacao>)new TrueSpecification<Apropriacao>();

            if (filtro.CentroCusto != null)
            {
                specification &= ApropriacaoSpecification.PertenceAoCentroCustoIniciadoPor(filtro.CentroCusto.Codigo);
            }

            specification &= (ApropriacaoSpecification.EhSituacaoAPagarLiberado(true)) ||
                (ApropriacaoSpecification.EhSituacaoAPagarAguardandoLiberacao(true));

            specification &= ApropriacaoSpecification.EhTipoTituloDiferenteDeTituloPai();

            return specification;
        }

        private Specification<Apropriacao> MontarSpecificationComprometidoPendenteTituloPagarRelAcompanhamentoFinanceiro(RelAcompanhamentoFinanceiroFiltro filtro)
        {
            var specification = (Specification<Apropriacao>)new TrueSpecification<Apropriacao>();

            if (filtro.CentroCusto != null)
            {
                specification &= ApropriacaoSpecification.PertenceAoCentroCustoIniciadoPor(filtro.CentroCusto.Codigo);
            }

            specification &= (ApropriacaoSpecification.EhSituacaoAPagarProvisionado(true)) ||
                (ApropriacaoSpecification.EhSituacaoAPagarLiberado(true)) ||
                (ApropriacaoSpecification.EhSituacaoAPagarAguardandoLiberacao(true));

            specification &= ApropriacaoSpecification.DataPeriodoPorVencimentoTituloPagarMenorOuIgualRelAcompanhamentoFinanceiro(filtro.DataFinal);

            specification &= ApropriacaoSpecification.EhTipoTituloDiferenteDeTituloPai();

            return specification;
        }

        private Specification<Apropriacao> MontarSpecificationComprometidoFuturoTituloPagarRelAcompanhamentoFinanceiro(RelAcompanhamentoFinanceiroFiltro filtro)
        {
            var specification = (Specification<Apropriacao>)new TrueSpecification<Apropriacao>();

            if (filtro.CentroCusto != null)
            {
                specification &= ApropriacaoSpecification.PertenceAoCentroCustoIniciadoPor(filtro.CentroCusto.Codigo);
            }

            specification &= 
                ((
                    (
                        (ApropriacaoSpecification.EhSituacaoAPagarBaixado(true)) ||
                        (ApropriacaoSpecification.EhSituacaoAPagarPago(true)) ||
                        (ApropriacaoSpecification.EhSituacaoAPagarEmitido(true))
                    ) &&
                    (
                        ApropriacaoSpecification.DataPeriodoPorEmissaoTituloPagarMaiorRelAcompanhamentoFinanceiro(filtro.DataFinal)
                    )
                )
                ||
                (
                    (
                        (ApropriacaoSpecification.EhSituacaoAPagarProvisionado(true)) ||
                        (ApropriacaoSpecification.EhSituacaoAPagarLiberado(true)) ||
                        (ApropriacaoSpecification.EhSituacaoAPagarAguardandoLiberacao(true))
                    ) && 
                    (ApropriacaoSpecification.DataPeriodoPorVencimentoTituloPagarMaiorRelAcompanhamentoFinanceiro(filtro.DataFinal)) &&
                    (ApropriacaoSpecification.EhTipoTituloDiferenteDeTituloPai())
                ));

            return specification;
        }


        private Specification<Apropriacao> MontarSpecificationDespesaPorPeriodoMovimentoRelAcompanhamentoFinanceiro(RelAcompanhamentoFinanceiroFiltro filtro)
        {
            var specification = (Specification<Apropriacao>)new TrueSpecification<Apropriacao>();

            if (filtro.CentroCusto != null)
            {
                specification &= ApropriacaoSpecification.PertenceAoCentroCustoIniciadoPor(filtro.CentroCusto.Codigo);
            }

            specification &= (ApropriacaoSpecification.EhSituacaoMovimentoLancado(true)) ||
                (ApropriacaoSpecification.EhSituacaoMovimentoConferido(true));

            specification &= ApropriacaoSpecification.DataPeriodoMovimentoMaiorOuIgual(filtro.DataInicial);
            specification &= ApropriacaoSpecification.DataPeriodoMovimentoMenorOuIgual(filtro.DataFinal);

            specification &= ApropriacaoSpecification.EhTipoMovimentoOperacaoDebito();
            specification &= ApropriacaoSpecification.EhTipoMovimentoCadastradoManualmente();

            return specification;
        }

        private Specification<Apropriacao> MontarSpecificationDespesaAcumuladaMovimentoRelAcompanhamentoFinanceiro(RelAcompanhamentoFinanceiroFiltro filtro)
        {
            var specification = (Specification<Apropriacao>)new TrueSpecification<Apropriacao>();

            if (filtro.CentroCusto != null)
            {
                specification &= ApropriacaoSpecification.PertenceAoCentroCustoIniciadoPor(filtro.CentroCusto.Codigo);
            }

            specification &= (ApropriacaoSpecification.EhSituacaoMovimentoLancado(true)) ||
                (ApropriacaoSpecification.EhSituacaoMovimentoConferido(true));

            specification &= ApropriacaoSpecification.DataPeriodoMovimentoMenorOuIgual(filtro.DataFinal);

            specification &= ApropriacaoSpecification.EhTipoMovimentoOperacaoDebito();
            specification &= ApropriacaoSpecification.EhTipoMovimentoCadastradoManualmente();

            return specification;
        }

        private Specification<Apropriacao> MontarSpecificationDespesaAcumuladaMovimentoRelAcompanhamentoFinanceiroExecutado(RelAcompanhamentoFinanceiroFiltro filtro)
        {
            var specification = (Specification<Apropriacao>)new TrueSpecification<Apropriacao>();

            if (filtro.CentroCusto != null)
            {
                specification &= ApropriacaoSpecification.PertenceAoCentroCustoIniciadoPor(filtro.CentroCusto.Codigo);
            }

            specification &= (ApropriacaoSpecification.EhSituacaoMovimentoLancado(true)) ||
                (ApropriacaoSpecification.EhSituacaoMovimentoConferido(true));

            specification &= ApropriacaoSpecification.EhTipoMovimentoOperacaoDebito();
            specification &= ApropriacaoSpecification.EhTipoMovimentoCadastradoManualmente();

            return specification;
        }


        private Specification<Apropriacao> MontarSpecificationContasAPagarPendentesRelApropriacaoPorClasse(RelApropriacaoPorClasseFiltro filtro, int? idUsuario)
        {
            var specification = (Specification<Apropriacao>)new TrueSpecification<Apropriacao>();

            if (usuarioAppService.UsuarioPossuiCentroCustoDefinidoNoModulo(idUsuario, Resource.Sigim.NomeModulo.Financeiro))
            {
                specification &= ApropriacaoSpecification.UsuarioPossuiAcessoAoCentroCusto(idUsuario, Resource.Sigim.NomeModulo.Financeiro);
            }
            else
            {
                specification &= ApropriacaoSpecification.EhCentroCustoAtivo();
            }

            if (filtro.CentroCusto != null){
                specification &= ApropriacaoSpecification.PertenceAoCentroCustoIniciadoPor(filtro.CentroCusto.Codigo);
            }

            specification &= ApropriacaoSpecification.EhTipoTituloDiferenteDeTituloPai();

            if (filtro.EhSituacaoAPagarProvisionado || filtro.EhSituacaoAPagarAguardandoLiberacao || filtro.EhSituacaoAPagarLiberado || filtro.EhSituacaoAPagarCancelado)
            {
                specification &= (ApropriacaoSpecification.EhSituacaoAPagarProvisionado(filtro.EhSituacaoAPagarProvisionado)) ||
                    (ApropriacaoSpecification.EhSituacaoAPagarAguardandoLiberacao(filtro.EhSituacaoAPagarAguardandoLiberacao)) ||
                    (ApropriacaoSpecification.EhSituacaoAPagarLiberado(filtro.EhSituacaoAPagarLiberado)) ||
                    (ApropriacaoSpecification.EhSituacaoAPagarCancelado(filtro.EhSituacaoAPagarCancelado));
            }

            string tipoPesquisa = "";
            if (filtro.EhTipoPesquisaPorCompetencia)
            {
                tipoPesquisa = "V";
            }
            if (filtro.EhTipoPesquisaPorEmissao)
            {
                tipoPesquisa = "E";
            }

            specification &= ApropriacaoSpecification.DataPeriodoTituloPagarMaiorOuIgualPendentesRelApropriacaoPorClasse(tipoPesquisa, filtro.DataInicial);
            specification &= ApropriacaoSpecification.DataPeriodoTituloPagarMenorOuIgualPendentesRelApropriacaoPorClasse(tipoPesquisa, filtro.DataFinal);

            if (filtro.OpcoesRelatorio.HasValue)
            {
                //if (filtro.OpcoesRelatorio.Value != (int)OpcoesRelatorioApropriacaoPorClasse.Sintetico)
                if (filtro.OpcoesRelatorio.Value == (int)OpcoesRelatorioApropriacaoPorClasse.Analitico)
                {
                    if (filtro.ListaClasseDespesa.Count > 0)
                    {
                        string[] arrayCodigoClasse = PopulaArrayComCodigosDeClassesSelecionadas(filtro.ListaClasseDespesa);

                        if (arrayCodigoClasse.Length > 0)
                        {
                            specification &= ApropriacaoSpecification.SaoClassesExistentes(arrayCodigoClasse);
                        }
                    }
                }
            }

            return specification;
        }

        private Specification<Apropriacao> MontarSpecificationContasAReceberPendentesRelApropriacaoPorClasse(RelApropriacaoPorClasseFiltro filtro, int? idUsuario)
        {
            var specification = (Specification<Apropriacao>)new TrueSpecification<Apropriacao>();

            if (usuarioAppService.UsuarioPossuiCentroCustoDefinidoNoModulo(idUsuario, Resource.Sigim.NomeModulo.Financeiro))
            {
                specification &= ApropriacaoSpecification.UsuarioPossuiAcessoAoCentroCusto(idUsuario, Resource.Sigim.NomeModulo.Financeiro);
            }
            else
            {
                specification &= ApropriacaoSpecification.EhCentroCustoAtivo();
            }

            if (filtro.CentroCusto != null)
            {
                specification &= ApropriacaoSpecification.PertenceAoCentroCustoIniciadoPor(filtro.CentroCusto.Codigo);
            }

            specification &= ApropriacaoSpecification.EhTipoTituloDiferenteDeTituloPai();

            if (filtro.EhSituacaoAReceberProvisionado || filtro.EhSituacaoAReceberAFatura || filtro.EhSituacaoAReceberFaturado || filtro.EhSituacaoAReceberCancelado)
            {
                specification &= (ApropriacaoSpecification.EhSituacaoAReceberProvisionado(filtro.EhSituacaoAReceberProvisionado)) ||
                    (ApropriacaoSpecification.EhSituacaoAReceberAFaturar(filtro.EhSituacaoAReceberAFatura)) ||
                    (ApropriacaoSpecification.EhSituacaoAReceberFaturado(filtro.EhSituacaoAReceberFaturado)) ||
                    (ApropriacaoSpecification.EhSituacaoAReceberCancelado(filtro.EhSituacaoAReceberCancelado));
            }

            string tipoPesquisa = "";
            if (filtro.EhTipoPesquisaPorCompetencia)
            {
                tipoPesquisa = "V";
            }
            if (filtro.EhTipoPesquisaPorEmissao)
            {
                tipoPesquisa = "E";
            }

            specification &= ApropriacaoSpecification.DataPeriodoTituloReceberMaiorOuIgualPendentesRelApropriacaoPorClasse(tipoPesquisa, filtro.DataInicial);
            specification &= ApropriacaoSpecification.DataPeriodoTituloReceberMenorOuIgualPendentesRelApropriacaoPorClasse(tipoPesquisa, filtro.DataFinal);

            if (filtro.OpcoesRelatorio.HasValue)
            {
                //if (filtro.OpcoesRelatorio.Value != (int)OpcoesRelatorioApropriacaoPorClasse.Sintetico)
                if (filtro.OpcoesRelatorio.Value == (int)OpcoesRelatorioApropriacaoPorClasse.Analitico)
                {
                    if (filtro.ListaClasseReceita.Count > 0)
                    {
                        string[] arrayCodigoClasse = PopulaArrayComCodigosDeClassesSelecionadas(filtro.ListaClasseReceita);

                        if (arrayCodigoClasse.Length > 0)
                        {
                            specification &= ApropriacaoSpecification.SaoClassesExistentes(arrayCodigoClasse);
                        }
                    }
                }
            }

            return specification;
        }

        public void MontaArrayClasseExistentes(Classe classeSelecionada, List<Classe> listaClassesSelecionadas)
        {
            if (classeSelecionada.ListaFilhos.Count == 0)
            {
                if (!listaClassesSelecionadas.Any(l => l.Codigo == classeSelecionada.Codigo))
                {
                    listaClassesSelecionadas.Add(classeSelecionada);
                }
            }
            else
            {
                foreach (var classeFilha in classeSelecionada.ListaFilhos)
                {
                    MontaArrayClasseExistentes(classeFilha, listaClassesSelecionadas);
                }
            }
        }

        private void GeraListaRelApropriacaoPorClassePagamentosPendentesEhPagos(List<Apropriacao> listaApropriacao, List<ApropriacaoClasseCCRelatorio> listaApropriacaoClasseRelatorio)
        {
            foreach (var groupApropriacaoClasse in listaApropriacao.OrderBy(l => l.CodigoClasse).GroupBy(l => new { l.CodigoClasse, l.TituloPagarId.HasValue }))
            {
                string tipoClasse = "";

                if (groupApropriacaoClasse.Count() > 0)
                {
                    tipoClasse = "S";
                }

                Classe classe = groupApropriacaoClasse.Select(l => l.Classe).FirstOrDefault().To<Classe>();

                ApropriacaoClasseCCRelatorio apropriacaoClasseRelatorio = new ApropriacaoClasseCCRelatorio();

                bool recuperouApropriacao = false;
                if (listaApropriacaoClasseRelatorio.Any(l => (l.Classe.Codigo == classe.Codigo && l.TipoClasseCC == "S")))
                {
                    apropriacaoClasseRelatorio = listaApropriacaoClasseRelatorio.Where(l => l.Classe.Codigo == classe.Codigo && l.TipoClasseCC == "S").FirstOrDefault();
                    recuperouApropriacao = true;
                }
                else
                {
                    apropriacaoClasseRelatorio.TipoClasseCC = tipoClasse;
                    apropriacaoClasseRelatorio.Classe = classe;
                    apropriacaoClasseRelatorio.ValorApropriado = 0;
                    apropriacaoClasseRelatorio.TipoCodigo = "PG";
                }

                decimal valorApropriado = groupApropriacaoClasse.Sum(l => l.Valor);

                apropriacaoClasseRelatorio.ValorApropriado = apropriacaoClasseRelatorio.ValorApropriado + valorApropriado;

                if (!recuperouApropriacao)
                {
                    listaApropriacaoClasseRelatorio.Add(apropriacaoClasseRelatorio);
                }

                if (apropriacaoClasseRelatorio.Classe.ClassePai != null)
                {
                    AdicionaRegistroRelatorioPai(apropriacaoClasseRelatorio, listaApropriacaoClasseRelatorio, valorApropriado, apropriacaoClasseRelatorio.TipoClasseCC);
                }
            }
        }

        private void GeraListaRelApropriacaoPorClasseRecebimentosPendentesEhRecebidos(List<Apropriacao> listaApropriacao, List<ApropriacaoClasseCCRelatorio> listaApropriacaoClasseRelatorio)
        {
            foreach (var groupApropriacaoClasse in listaApropriacao.OrderBy(l => l.CodigoClasse).GroupBy(l => new { l.CodigoClasse, l.TituloPagarId.HasValue }))
            {
                string tipoClasse = "";

                if (groupApropriacaoClasse.Count() > 0)
                {
                    tipoClasse = "E";
                }

                Classe classe = groupApropriacaoClasse.Select(l => l.Classe).FirstOrDefault().To<Classe>();

                ApropriacaoClasseCCRelatorio apropriacaoClasseRelatorio = new ApropriacaoClasseCCRelatorio();

                bool recuperouApropriacao = false;
                if (listaApropriacaoClasseRelatorio.Any(l => (l.Classe.Codigo == classe.Codigo) && (l.TipoClasseCC == "E")))
                {
                    apropriacaoClasseRelatorio = listaApropriacaoClasseRelatorio.Where(l => l.Classe.Codigo == classe.Codigo && l.TipoClasseCC == "E").FirstOrDefault();
                    recuperouApropriacao = true;
                }
                else
                {
                    apropriacaoClasseRelatorio.TipoClasseCC = tipoClasse;
                    apropriacaoClasseRelatorio.Classe = classe;
                    apropriacaoClasseRelatorio.ValorApropriado = 0;
                    apropriacaoClasseRelatorio.TipoCodigo = "RC";
                }

                decimal valorApropriado = groupApropriacaoClasse.Sum(l => l.Valor);

                apropriacaoClasseRelatorio.ValorApropriado = apropriacaoClasseRelatorio.ValorApropriado + valorApropriado;

                if (!recuperouApropriacao)
                {
                    listaApropriacaoClasseRelatorio.Add(apropriacaoClasseRelatorio);
                }

                if (apropriacaoClasseRelatorio.Classe.ClassePai != null)
                {
                    AdicionaRegistroRelatorioPai(apropriacaoClasseRelatorio, listaApropriacaoClasseRelatorio, valorApropriado, apropriacaoClasseRelatorio.TipoClasseCC);
                }
            }
        }

        private void GeraListaRelApropriacaoPorClasseMovimentoDebito(List<Apropriacao> listaApropriacao, List<ApropriacaoClasseCCRelatorio> listaApropriacaoClasseRelatorio)
        {
            foreach (var groupApropriacaoClasse in listaApropriacao.OrderBy(l => l.CodigoClasse).GroupBy(l => new { l.CodigoClasse, l.TituloPagarId.HasValue }))
            {
                string tipoClasse = "";

                if (groupApropriacaoClasse.Count() > 0)
                {
                    tipoClasse = "S";
                }

                Classe classe = groupApropriacaoClasse.Select(l => l.Classe).FirstOrDefault().To<Classe>();

                ApropriacaoClasseCCRelatorio apropriacaoClasseRelatorio = new ApropriacaoClasseCCRelatorio();

                bool recuperouApropriacao = false;
                if (listaApropriacaoClasseRelatorio.Any(l => (l.Classe.Codigo == classe.Codigo && l.TipoClasseCC == "S")))
                {
                    apropriacaoClasseRelatorio = listaApropriacaoClasseRelatorio.Where(l => l.Classe.Codigo == classe.Codigo && l.TipoClasseCC == "S").FirstOrDefault();
                    recuperouApropriacao = true;
                }
                else
                {
                    apropriacaoClasseRelatorio.TipoClasseCC = tipoClasse;
                    apropriacaoClasseRelatorio.Classe = classe;
                    apropriacaoClasseRelatorio.ValorApropriado = 0;
                    apropriacaoClasseRelatorio.TipoCodigo = "MV";
                }

                decimal valorApropriado = groupApropriacaoClasse.Sum(l => l.Valor);

                apropriacaoClasseRelatorio.ValorApropriado = apropriacaoClasseRelatorio.ValorApropriado + valorApropriado;

                if (!recuperouApropriacao)
                {
                    listaApropriacaoClasseRelatorio.Add(apropriacaoClasseRelatorio);
                }

                if (apropriacaoClasseRelatorio.Classe.ClassePai != null)
                {
                    AdicionaRegistroRelatorioPai(apropriacaoClasseRelatorio, listaApropriacaoClasseRelatorio, valorApropriado, apropriacaoClasseRelatorio.TipoClasseCC);
                }
            }
        }

        private void GeraListaRelApropriacaoPorClasseMovimentoCredito(List<Apropriacao> listaApropriacao, List<ApropriacaoClasseCCRelatorio> listaApropriacaoClasseRelatorio)
        {
            foreach (var groupApropriacaoClasse in listaApropriacao.OrderBy(l => l.CodigoClasse).GroupBy(l => new { l.CodigoClasse, l.TituloPagarId.HasValue }))
            {
                string tipoClasse = "";

                if (groupApropriacaoClasse.Count() > 0)
                {
                    tipoClasse = "E";
                }

                Classe classe = groupApropriacaoClasse.Select(l => l.Classe).FirstOrDefault().To<Classe>();

                ApropriacaoClasseCCRelatorio apropriacaoClasseRelatorio = new ApropriacaoClasseCCRelatorio();

                bool recuperouApropriacao = false;
                if (listaApropriacaoClasseRelatorio.Any(l => (l.Classe.Codigo == classe.Codigo && l.TipoClasseCC == "E")))
                {
                    apropriacaoClasseRelatorio = listaApropriacaoClasseRelatorio.Where(l => l.Classe.Codigo == classe.Codigo && l.TipoClasseCC == "E").FirstOrDefault();
                    recuperouApropriacao = true;
                }
                else
                {
                    apropriacaoClasseRelatorio.TipoClasseCC = tipoClasse;
                    apropriacaoClasseRelatorio.Classe = classe;
                    apropriacaoClasseRelatorio.ValorApropriado = 0;
                    apropriacaoClasseRelatorio.TipoCodigo = "MV";
                }

                decimal valorApropriado = groupApropriacaoClasse.Sum(l => l.Valor);

                apropriacaoClasseRelatorio.ValorApropriado = apropriacaoClasseRelatorio.ValorApropriado + valorApropriado;

                if (!recuperouApropriacao)
                {
                    listaApropriacaoClasseRelatorio.Add(apropriacaoClasseRelatorio);
                }

                if (apropriacaoClasseRelatorio.Classe.ClassePai != null)
                {
                    AdicionaRegistroRelatorioPai(apropriacaoClasseRelatorio, listaApropriacaoClasseRelatorio, valorApropriado, apropriacaoClasseRelatorio.TipoClasseCC);
                }
            }
        }

        private void GeraListaRelApropriacaoPorClasseCreditoCobranca(List<TituloDetalheCredCob> listaTituloDetalheCredCob, List<ApropriacaoClasseCCRelatorio> listaApropriacaoClasseRelatorio)
        {
            foreach (var groupApropriacaoClasse in listaTituloDetalheCredCob.Where(l => l.VerbaCobranca.CodigoClasse != null).OrderBy(l => l.VerbaCobranca.CodigoClasse).GroupBy(l => l.VerbaCobranca.CodigoClasse))
            {
                string tipoClasse = "";

                if (groupApropriacaoClasse.Count() > 0)
                {
                    tipoClasse = "E";
                }

                Classe classe = groupApropriacaoClasse.Select(l => l.VerbaCobranca.Classe).FirstOrDefault().To<Classe>();

                ApropriacaoClasseCCRelatorio apropriacaoClasseRelatorio = new ApropriacaoClasseCCRelatorio();

                bool recuperouApropriacao = false;
                if (listaApropriacaoClasseRelatorio.Any(l => (l.Classe.Codigo == classe.Codigo && l.TipoClasseCC == "E")))
                {
                    apropriacaoClasseRelatorio = listaApropriacaoClasseRelatorio.Where(l => l.Classe.Codigo == classe.Codigo && l.TipoClasseCC == "E").FirstOrDefault();
                    recuperouApropriacao = true;
                }
                else
                {
                    apropriacaoClasseRelatorio.TipoClasseCC = tipoClasse;
                    apropriacaoClasseRelatorio.Classe = classe;
                    apropriacaoClasseRelatorio.ValorApropriado = 0;
                    apropriacaoClasseRelatorio.TipoCodigo = "CC";
                }

                decimal valorApropriado = 0;
                valorApropriado = groupApropriacaoClasse.Where(l => l.Situacao == "P").Sum(l => l.ValorDevido);
                valorApropriado = valorApropriado + groupApropriacaoClasse.Where(l => l.Situacao == "Q").Sum(l => l.ValorBaixa.Value);

                apropriacaoClasseRelatorio.ValorApropriado = apropriacaoClasseRelatorio.ValorApropriado + valorApropriado;

                if (!recuperouApropriacao)
                {
                    listaApropriacaoClasseRelatorio.Add(apropriacaoClasseRelatorio);
                }

                if (apropriacaoClasseRelatorio.Classe.ClassePai != null)
                {
                    AdicionaRegistroRelatorioPai(apropriacaoClasseRelatorio, listaApropriacaoClasseRelatorio, valorApropriado, apropriacaoClasseRelatorio.TipoClasseCC);
                }
            }
        }

        private void GeraListaRelApropriacaoPorClasseCreditoCobrancaTituloMovimento(List<TituloMovimento> listaTituloMovimento, List<ApropriacaoClasseCCRelatorio> listaApropriacaoClasseRelatorio)
        {
            foreach (var groupApropriacaoClasse in listaTituloMovimento.Where(l => l.TituloCredCob.VerbaCobranca.CodigoClasse != null).OrderBy(l => l.TituloCredCob.VerbaCobranca.CodigoClasse).GroupBy(l => l.TituloCredCob.VerbaCobranca.CodigoClasse))
            {
                string tipoClasse = "";

                if (groupApropriacaoClasse.Count() > 0)
                {
                    tipoClasse = "E";
                }

                Classe classe = groupApropriacaoClasse.Select(l => l.TituloCredCob.VerbaCobranca.Classe).FirstOrDefault().To<Classe>();

                ApropriacaoClasseCCRelatorio apropriacaoClasseRelatorio = new ApropriacaoClasseCCRelatorio();

                bool recuperouApropriacao = false;
                if (listaApropriacaoClasseRelatorio.Any(l => (l.Classe.Codigo == classe.Codigo && l.TipoClasseCC == "E")))
                {
                    apropriacaoClasseRelatorio = listaApropriacaoClasseRelatorio.Where(l => l.Classe.Codigo == classe.Codigo && l.TipoClasseCC == "E").FirstOrDefault();
                    recuperouApropriacao = true;
                }
                else
                {
                    apropriacaoClasseRelatorio.TipoClasseCC = tipoClasse;
                    apropriacaoClasseRelatorio.Classe = classe;
                    apropriacaoClasseRelatorio.ValorApropriado = 0;
                    apropriacaoClasseRelatorio.TipoCodigo = "CC";
                }

                decimal valorApropriado = 0;
                valorApropriado = groupApropriacaoClasse.Sum(l => l.TituloCredCob.ValorBaixa.Value);

                apropriacaoClasseRelatorio.ValorApropriado = apropriacaoClasseRelatorio.ValorApropriado + valorApropriado;

                if (!recuperouApropriacao)
                {
                    listaApropriacaoClasseRelatorio.Add(apropriacaoClasseRelatorio);
                }

                if (apropriacaoClasseRelatorio.Classe.ClassePai != null)
                {
                    AdicionaRegistroRelatorioPai(apropriacaoClasseRelatorio, listaApropriacaoClasseRelatorio, valorApropriado, apropriacaoClasseRelatorio.TipoClasseCC);
                }
            }
        }

        private Specification<Apropriacao> MontarSpecificationContasAPagarPagosRelApropriacaoPorClasse(RelApropriacaoPorClasseFiltro filtro, int? idUsuario)
        {
            var specification = (Specification<Apropriacao>)new TrueSpecification<Apropriacao>();

            if (usuarioAppService.UsuarioPossuiCentroCustoDefinidoNoModulo(idUsuario, Resource.Sigim.NomeModulo.Financeiro))
            {
                specification &= ApropriacaoSpecification.UsuarioPossuiAcessoAoCentroCusto(idUsuario, Resource.Sigim.NomeModulo.Financeiro);
            }
            else
            {
                specification &= ApropriacaoSpecification.EhCentroCustoAtivo();
            }

            if (filtro.CentroCusto != null)
            {
                specification &= ApropriacaoSpecification.PertenceAoCentroCustoIniciadoPor(filtro.CentroCusto.Codigo);
            }

            specification &= ApropriacaoSpecification.EhTipoTituloDiferenteDeTituloPai();

            if (filtro.EhSituacaoAPagarEmitido || filtro.EhSituacaoAPagarPago || filtro.EhSituacaoAPagarBaixado)
            {
                specification &= (ApropriacaoSpecification.EhSituacaoAPagarEmitido(filtro.EhSituacaoAPagarEmitido)) ||
                    (ApropriacaoSpecification.EhSituacaoAPagarBaixado(filtro.EhSituacaoAPagarBaixado)) ||
                    (ApropriacaoSpecification.EhSituacaoAPagarPago(filtro.EhSituacaoAPagarPago));
            }

            string tipoPesquisa = "";
            if (filtro.EhTipoPesquisaPorCompetencia)
            {
                tipoPesquisa = "V";
            }
            if (filtro.EhTipoPesquisaPorEmissao)
            {
                tipoPesquisa = "E";
            }

            specification &= ApropriacaoSpecification.DataPeriodoTituloPagarMaiorOuIgualPagosRelApropriacaoPorClasse(tipoPesquisa, filtro.DataInicial, filtro.EhSituacaoAPagarEmitido ,filtro.EhSituacaoAPagarPago ,filtro.EhSituacaoAPagarBaixado);
            specification &= ApropriacaoSpecification.DataPeriodoTituloPagarMenorOuIgualPagosRelApropriacaoPorClasse(tipoPesquisa, filtro.DataFinal, filtro.EhSituacaoAPagarEmitido ,filtro.EhSituacaoAPagarPago ,filtro.EhSituacaoAPagarBaixado);

            if (filtro.OpcoesRelatorio.HasValue)
            {
                //if (filtro.OpcoesRelatorio.Value != (int)OpcoesRelatorioApropriacaoPorClasse.Sintetico)
                if (filtro.OpcoesRelatorio.Value == (int)OpcoesRelatorioApropriacaoPorClasse.Analitico)
                {
                    if (filtro.ListaClasseDespesa.Count > 0)
                    {
                        string[] arrayCodigoClasse = PopulaArrayComCodigosDeClassesSelecionadas(filtro.ListaClasseDespesa);

                        if (arrayCodigoClasse.Length > 0)
                        {
                            specification &= ApropriacaoSpecification.SaoClassesExistentes(arrayCodigoClasse);
                        }
                    }
                }
            }

            return specification;
        }

        private Specification<Apropriacao> MontarSpecificationContasAReceberRecebidosRelApropriacaoPorClasse(RelApropriacaoPorClasseFiltro filtro, int? idUsuario)
        {
            var specification = (Specification<Apropriacao>)new TrueSpecification<Apropriacao>();

            if (usuarioAppService.UsuarioPossuiCentroCustoDefinidoNoModulo(idUsuario, Resource.Sigim.NomeModulo.Financeiro))
            {
                specification &= ApropriacaoSpecification.UsuarioPossuiAcessoAoCentroCusto(idUsuario, Resource.Sigim.NomeModulo.Financeiro);
            }
            else
            {
                specification &= ApropriacaoSpecification.EhCentroCustoAtivo();
            }

            if (filtro.CentroCusto != null)
            {
                specification &= ApropriacaoSpecification.PertenceAoCentroCustoIniciadoPor(filtro.CentroCusto.Codigo);
            }

            specification &= ApropriacaoSpecification.EhTipoTituloDiferenteDeTituloPai();

            if (filtro.EhSituacaoAReceberPreDatado || filtro.EhSituacaoAReceberRecebido || filtro.EhSituacaoAReceberQuitado)
            {
                specification &= (ApropriacaoSpecification.EhSituacaoAReceberPreDatado(filtro.EhSituacaoAReceberPreDatado)) ||
                    (ApropriacaoSpecification.EhSituacaoAReceberRecebido(filtro.EhSituacaoAReceberRecebido)) ||
                    (ApropriacaoSpecification.EhSituacaoAReceberQuitado(filtro.EhSituacaoAReceberQuitado));
            }

            string tipoPesquisa = "";
            if (filtro.EhTipoPesquisaPorCompetencia)
            {
                tipoPesquisa = "V";
            }
            if (filtro.EhTipoPesquisaPorEmissao)
            {
                tipoPesquisa = "E";
            }

            specification &= ApropriacaoSpecification.DataPeriodoTituloReceberMaiorOuIgualRecebidosRelApropriacaoPorClasse(tipoPesquisa, filtro.DataInicial, filtro.EhSituacaoAReceberPreDatado, filtro.EhSituacaoAReceberRecebido, filtro.EhSituacaoAReceberQuitado);
            specification &= ApropriacaoSpecification.DataPeriodoTituloReceberMenorOuIgualRecebidosRelApropriacaoPorClasse(tipoPesquisa, filtro.DataFinal, filtro.EhSituacaoAReceberPreDatado, filtro.EhSituacaoAReceberRecebido, filtro.EhSituacaoAReceberQuitado);

            if (filtro.OpcoesRelatorio.HasValue)
            {
                //if (filtro.OpcoesRelatorio.Value != (int)OpcoesRelatorioApropriacaoPorClasse.Sintetico)
                if (filtro.OpcoesRelatorio.Value == (int)OpcoesRelatorioApropriacaoPorClasse.Analitico)
                {
                    if (filtro.ListaClasseReceita.Count > 0)
                    {

                        string[] arrayCodigoClasse = PopulaArrayComCodigosDeClassesSelecionadas(filtro.ListaClasseReceita);

                        if (arrayCodigoClasse.Length > 0)
                        {
                            specification &= ApropriacaoSpecification.SaoClassesExistentes(arrayCodigoClasse);
                        }
                    }
                }
            }

            return specification;
        }

        private Specification<Apropriacao> MontarSpecificationMovimentoRelApropriacaoPorClasse(RelApropriacaoPorClasseFiltro filtro, int? idUsuario,TipoMovimentoRelatorioApropriacaoPorClasse tipoMovimento)
        {
            var specification = (Specification<Apropriacao>)new TrueSpecification<Apropriacao>();

            if (usuarioAppService.UsuarioPossuiCentroCustoDefinidoNoModulo(idUsuario, Resource.Sigim.NomeModulo.Financeiro))
            {
                specification &= ApropriacaoSpecification.UsuarioPossuiAcessoAoCentroCusto(idUsuario, Resource.Sigim.NomeModulo.Financeiro);
            }
            else
            {
                specification &= ApropriacaoSpecification.EhCentroCustoAtivo();
            }

            if (filtro.CentroCusto != null)
            {
                specification &= ApropriacaoSpecification.PertenceAoCentroCustoIniciadoPor(filtro.CentroCusto.Codigo);
            }

            if (tipoMovimento == TipoMovimentoRelatorioApropriacaoPorClasse.MovimentoDebito ||
                tipoMovimento == TipoMovimentoRelatorioApropriacaoPorClasse.MovimentoDebitoCaixa)
            {
                specification &= ApropriacaoSpecification.EhValorDoMovimentoNegativo();
            }

            if (tipoMovimento == TipoMovimentoRelatorioApropriacaoPorClasse.MovimentoDebito || 
                tipoMovimento == TipoMovimentoRelatorioApropriacaoPorClasse.MovimentoCredito)
            {
                specification &= ApropriacaoSpecification.PossuiContaCorrenteNoMovimento();
            }

            if (tipoMovimento == TipoMovimentoRelatorioApropriacaoPorClasse.MovimentoDebitoCaixa ||
                tipoMovimento == TipoMovimentoRelatorioApropriacaoPorClasse.MovimentoCreditoCaixa)
            {
                specification &= ApropriacaoSpecification.PossuiCaixaNoMovimento();
            }

            if (tipoMovimento == TipoMovimentoRelatorioApropriacaoPorClasse.MovimentoCredito ||
                tipoMovimento == TipoMovimentoRelatorioApropriacaoPorClasse.MovimentoCreditoCaixa)
            {
                specification &= ApropriacaoSpecification.EhValorDoMovimentoPositivo();
                specification &= ApropriacaoSpecification.EhMovimentoDiferenteDeCredCob();
            }

            specification &= ApropriacaoSpecification.EhSituacaoMovimentoDiferenteDeEstornado();

            specification &= ApropriacaoSpecification.DataPeriodoMovimentoMaiorOuIgual(filtro.DataInicial);
            specification &= ApropriacaoSpecification.DataPeriodoMovimentoMenorOuIgual(filtro.DataFinal);

            if (filtro.OpcoesRelatorio.HasValue)
            {
                //if (filtro.OpcoesRelatorio.Value != (int)OpcoesRelatorioApropriacaoPorClasse.Sintetico)
                if (filtro.OpcoesRelatorio.Value == (int)OpcoesRelatorioApropriacaoPorClasse.Analitico)
                {

                    string[] arrayCodigoClasse = new string[0];
                    if (tipoMovimento == TipoMovimentoRelatorioApropriacaoPorClasse.MovimentoDebito ||
                        tipoMovimento == TipoMovimentoRelatorioApropriacaoPorClasse.MovimentoDebitoCaixa)
                    {
                        if (filtro.ListaClasseDespesa.Count > 0)
                        {
                            arrayCodigoClasse = PopulaArrayComCodigosDeClassesSelecionadas(filtro.ListaClasseDespesa);
                        }
                    }

                    if (tipoMovimento == TipoMovimentoRelatorioApropriacaoPorClasse.MovimentoCredito ||
                        tipoMovimento == TipoMovimentoRelatorioApropriacaoPorClasse.MovimentoCreditoCaixa)
                    {
                        if (filtro.ListaClasseReceita.Count > 0)
                        {
                            arrayCodigoClasse = PopulaArrayComCodigosDeClassesSelecionadas(filtro.ListaClasseReceita);
                        }

                    }

                    if (arrayCodigoClasse.Length > 0)
                    {
                        specification &= ApropriacaoSpecification.SaoClassesExistentes(arrayCodigoClasse);
                    }
                }
            }

            return specification;
        }

        private string[] PopulaArrayComCodigosDeClassesSelecionadas(List<ClasseDTO> listaClasses){
            List<Classe> listaClassesSelecionadas = new List<Classe>();

            foreach (var classeSelecionada in listaClasses)
            {
                var classe = classeRepository.ObterPeloCodigo(classeSelecionada.Codigo, l => l.ListaFilhos);

                MontaArrayClasseExistentes(classe, listaClassesSelecionadas);
            }

            int i = 0;
            string[] arrayCodigoClasse = new string[listaClassesSelecionadas.Count()];
            foreach (var classe in listaClassesSelecionadas)
            {
                arrayCodigoClasse[i] = classe.Codigo;
                i += 1;
            }

            return arrayCodigoClasse;

        }

        private DataTable CriaDataTableApropriacaoClasseCCRelatorio()
        {
            DataTable dta = new DataTable();

            DataColumn tipoClasseCC = new DataColumn("tipoClasseCC");
            DataColumn codigoClasse = new DataColumn("codigoClasse");
            DataColumn descricaoClasse = new DataColumn("descricaoClasse");
            DataColumn valorApropriado = new DataColumn("valorApropriado", System.Type.GetType("System.Decimal"));
            DataColumn tipoCodigo = new DataColumn("tipoCodigo");
            DataColumn girErro = new DataColumn("girErro");

            dta.Columns.Add(tipoClasseCC);
            dta.Columns.Add(codigoClasse);
            dta.Columns.Add(descricaoClasse);
            dta.Columns.Add(valorApropriado);
            dta.Columns.Add(tipoCodigo);
            dta.Columns.Add(girErro);

            return dta;
        }

        private DataTable RelApropriacaoPorClasseAnaliticoToDataTable(List<ApropriacaoClasseCCRelatorio> listaApropriacaoClasseRelatorio)
        {
            DataTable dta = new DataTable();
            DataColumn tipoClasseCC = new DataColumn("tipoClasseCC");
            DataColumn codigoClasse = new DataColumn("codigoClasse");
            DataColumn descricaoClasse = new DataColumn("descricaoClasse");
            DataColumn valorApropriado = new DataColumn("valorApropriado", System.Type.GetType("System.Decimal"));
            DataColumn tipoCodigo = new DataColumn("tipoCodigo");
            DataColumn girErro = new DataColumn("girErro");

            dta.Columns.Add(tipoClasseCC);
            dta.Columns.Add(codigoClasse);
            dta.Columns.Add(descricaoClasse);
            dta.Columns.Add(valorApropriado);
            dta.Columns.Add(tipoCodigo);
            dta.Columns.Add(girErro);

            foreach (ApropriacaoClasseCCRelatorio apropriacaoClasseCCRelatorio in listaApropriacaoClasseRelatorio.OrderBy(l => l.Classe.Codigo))
            {
                DataRow row = dta.NewRow();
                row[tipoClasseCC] = apropriacaoClasseCCRelatorio.TipoClasseCC ;
                row[codigoClasse] = apropriacaoClasseCCRelatorio.Classe.Codigo;
                row[descricaoClasse] = apropriacaoClasseCCRelatorio.Classe.Descricao;
                row[valorApropriado] = apropriacaoClasseCCRelatorio.ValorApropriado;
                row[tipoCodigo] = apropriacaoClasseCCRelatorio.TipoCodigo;
                row[girErro] = "";
                dta.Rows.Add(row);
            }

            return dta;
        }

        private void AdicionaRegistroRelatorioPai(ApropriacaoClasseCCRelatorio apropriacaoClasseRelatorioFilho, List<ApropriacaoClasseCCRelatorio> listaApropriacaoClasseRelatorio, decimal valorApropriado, string tipoClasse)
        {
            if (apropriacaoClasseRelatorioFilho.Classe.ClassePai != null)
            {
                ApropriacaoClasseCCRelatorio apropriacaoClasseRelatorio = new ApropriacaoClasseCCRelatorio();
                bool recuperouApropriacao = false;
                if (listaApropriacaoClasseRelatorio.Any(l => ((l.Classe.Codigo == apropriacaoClasseRelatorioFilho.Classe.CodigoPai) && l.TipoClasseCC == tipoClasse)))
                {
                    apropriacaoClasseRelatorio = listaApropriacaoClasseRelatorio.Where(l => l.Classe.Codigo == apropriacaoClasseRelatorioFilho.Classe.CodigoPai && l.TipoClasseCC == tipoClasse).FirstOrDefault();
                    recuperouApropriacao = true;
                }
                else
                {
                    apropriacaoClasseRelatorio.TipoClasseCC = apropriacaoClasseRelatorioFilho.TipoClasseCC;
                    apropriacaoClasseRelatorio.Classe = apropriacaoClasseRelatorioFilho.Classe.ClassePai;
                    apropriacaoClasseRelatorio.ValorApropriado = 0;
                    apropriacaoClasseRelatorio.TipoCodigo = "M";
                }

                apropriacaoClasseRelatorio.ValorApropriado = apropriacaoClasseRelatorio.ValorApropriado + valorApropriado;

                if (!recuperouApropriacao)
                {
                    listaApropriacaoClasseRelatorio.Add(apropriacaoClasseRelatorio);
                }

                if (apropriacaoClasseRelatorio.Classe.ClassePai != null)
                {
                    AdicionaRegistroRelatorioPai(apropriacaoClasseRelatorio, listaApropriacaoClasseRelatorio, valorApropriado, apropriacaoClasseRelatorio.TipoClasseCC);
                }
            }
        }

        private string MontarStringSituacaoAPagar(RelApropriacaoPorClasseFiltro filtro)
        {
            string strSituacaoPagarSelecao = "";
            string strSituacaoPagarSelecaoSemDelimitadorNoFinal = "";
            const string delimitador = ",";

            if (filtro.EhSituacaoAPagarProvisionado)
            {
                strSituacaoPagarSelecao = strSituacaoPagarSelecao + SituacaoTituloPagar.Provisionado.ObterDescricao() + delimitador;
            }
            if (filtro.EhSituacaoAPagarAguardandoLiberacao)
            {
                strSituacaoPagarSelecao = strSituacaoPagarSelecao + SituacaoTituloPagar.AguardandoLiberacao.ObterDescricao() + delimitador;
            }
            if (filtro.EhSituacaoAPagarLiberado)
            {
                strSituacaoPagarSelecao = strSituacaoPagarSelecao + SituacaoTituloPagar.Liberado.ObterDescricao() + delimitador;
            }
            if (filtro.EhSituacaoAPagarEmitido)
            {
                strSituacaoPagarSelecao = strSituacaoPagarSelecao + SituacaoTituloPagar.Emitido.ObterDescricao() + delimitador;
            }
            if (filtro.EhSituacaoAPagarPago)
            {
                strSituacaoPagarSelecao = strSituacaoPagarSelecao + SituacaoTituloPagar.Pago.ObterDescricao() + delimitador;
            }
            if (filtro.EhSituacaoAPagarBaixado)
            {
                strSituacaoPagarSelecao = strSituacaoPagarSelecao + SituacaoTituloPagar.Baixado.ObterDescricao() + delimitador;
            }
            if (filtro.EhSituacaoAPagarCancelado)
            {
                strSituacaoPagarSelecao = strSituacaoPagarSelecao + SituacaoTituloPagar.Cancelado.ObterDescricao() + delimitador;
            }

            if (strSituacaoPagarSelecao.Length > 0)
            {
                strSituacaoPagarSelecao = strSituacaoPagarSelecao.Substring(0, strSituacaoPagarSelecao.Length - 1);
                strSituacaoPagarSelecaoSemDelimitadorNoFinal = strSituacaoPagarSelecao;
            }

            return strSituacaoPagarSelecaoSemDelimitadorNoFinal;
        }

        private string MontaStringSituacaoAReceber(RelApropriacaoPorClasseFiltro filtro)
        {
            string strSituacaoReceberSelecao = "";
            string strSituacaoPagarSelecaoSemDelimitadorNoFinal = "";
            const string delimitador = ",";

            if (filtro.EhSituacaoAReceberProvisionado)
            {
                strSituacaoReceberSelecao = strSituacaoReceberSelecao + SituacaoTituloReceber.Provisionado.ObterDescricao() + delimitador;
            }
            if (filtro.EhSituacaoAReceberAFatura)
            {
                strSituacaoReceberSelecao = strSituacaoReceberSelecao + SituacaoTituloReceber.Afaturar.ObterDescricao() + delimitador;
            }
            if (filtro.EhSituacaoAReceberFaturado)
            {
                strSituacaoReceberSelecao = strSituacaoReceberSelecao + SituacaoTituloReceber.Faturado.ObterDescricao() + delimitador;
            }
            if (filtro.EhSituacaoAReceberPreDatado)
            {
                strSituacaoReceberSelecao = strSituacaoReceberSelecao + SituacaoTituloReceber.Predatado.ObterDescricao() + delimitador;
            }
            if (filtro.EhSituacaoAReceberRecebido)
            {
                strSituacaoReceberSelecao = strSituacaoReceberSelecao + SituacaoTituloReceber.Recebido.ObterDescricao() + delimitador;
            }
            if (filtro.EhSituacaoAReceberQuitado)
            {
                strSituacaoReceberSelecao = strSituacaoReceberSelecao + SituacaoTituloReceber.Quitado.ObterDescricao() + delimitador;
            }
            if (filtro.EhSituacaoAReceberCancelado)
            {
                strSituacaoReceberSelecao = strSituacaoReceberSelecao + SituacaoTituloReceber.Cancelado.ObterDescricao() + delimitador;
            }

            if (strSituacaoReceberSelecao.Length > 0)
            {
                strSituacaoReceberSelecao = strSituacaoReceberSelecao.Substring(0, strSituacaoReceberSelecao.Length - 1);
                strSituacaoPagarSelecaoSemDelimitadorNoFinal = strSituacaoReceberSelecao;
            }


            return strSituacaoPagarSelecaoSemDelimitadorNoFinal;
        }

        private string MontaStringTipoPesquisa(RelApropriacaoPorClasseFiltro filtro)
        {
            string strTipoData = "";

            if (filtro.EhTipoPesquisaPorCompetencia)
            {
                strTipoData = "por competência";
            }
            if (filtro.EhTipoPesquisaPorEmissao)
            {
                strTipoData = "por emissão de documento";
            }

            return strTipoData;
        }

        private DataTable RelAcompanhamentoFinanceiroToDataTable(List<RelAcompanhamentoFinanceiroDTO> listaRelAcompanhamentoFinanceiro)
        {
            DataTable dta = new DataTable();
            DataColumn classe = new DataColumn("classe");
            DataColumn descricaoClasse = new DataColumn("descricaoClasse");
            DataColumn orcamentoInicial = new DataColumn("orcamentoInicial", System.Type.GetType("System.Decimal"));
            DataColumn orcamentoAtual = new DataColumn("orcamentoAtual", System.Type.GetType("System.Decimal"));
            DataColumn despesaPeriodo = new DataColumn("despesaPeriodo", System.Type.GetType("System.Decimal"));
            DataColumn despesaAcumulada = new DataColumn("despesaAcumulada", System.Type.GetType("System.Decimal"));
            DataColumn comprometidoPendente = new DataColumn("comprometidoPendente", System.Type.GetType("System.Decimal"));
            DataColumn comprometidoFuturo = new DataColumn("comprometidoFuturo", System.Type.GetType("System.Decimal"));
            DataColumn resultadoAcrescimo = new DataColumn("resultadoAcrescimo", System.Type.GetType("System.Decimal"));
            DataColumn resultadoSaldo = new DataColumn("resultadoSaldo", System.Type.GetType("System.Decimal"));
            DataColumn conclusao = new DataColumn("conclusao", System.Type.GetType("System.Decimal"));
            DataColumn classeFechada = new DataColumn("classeFechada", System.Type.GetType("System.Boolean"));
            DataColumn classeFilha = new DataColumn("classeFilha");
            DataColumn percentualExecutado = new DataColumn("percentualExecutado", System.Type.GetType("System.Decimal"));
            DataColumn percentualComprometido = new DataColumn("percentualComprometido", System.Type.GetType("System.Decimal"));

            DataColumn girErro = new DataColumn("girErro");

            dta.Columns.Add(classe);
            dta.Columns.Add(descricaoClasse);
            dta.Columns.Add(orcamentoInicial);
            dta.Columns.Add(orcamentoAtual);
            dta.Columns.Add(despesaPeriodo);
            dta.Columns.Add(despesaAcumulada);
            dta.Columns.Add(comprometidoPendente);
            dta.Columns.Add(comprometidoFuturo);
            dta.Columns.Add(resultadoAcrescimo);
            dta.Columns.Add(resultadoSaldo);
            dta.Columns.Add(classeFechada);
            dta.Columns.Add(classeFilha);
            dta.Columns.Add(conclusao);
            dta.Columns.Add(percentualExecutado);
            dta.Columns.Add(percentualComprometido);

            dta.Columns.Add(girErro);

            foreach (var item in listaRelAcompanhamentoFinanceiro)
            {
                DataRow row = dta.NewRow();

                row[classe] = item.CodigoClasse;
                row[descricaoClasse] = item.DescricaoClasse;
                row[orcamentoInicial] = item.OrcamentoInicial;
                row[orcamentoAtual] = item.OrcamentoAtual;
                row[despesaPeriodo] = item.DespesaPeriodo;
                row[despesaAcumulada] = item.DespesaAcumulada;
                row[comprometidoPendente] = item.ComprometidoPendente;
                row[comprometidoFuturo] = item.ComprometidoFuturo;
                row[resultadoAcrescimo] = item.ResultadoAcrescimo;
                row[resultadoSaldo] = item.ResultadoSaldo;
                row[classeFechada] = false;
                if (item.DescricaoClasseFechada == "F")
                {
                    row[classeFechada] = true;
                }
                row[classeFilha] = "";
                if (!item.ClassePossuiFilhos)
                {
                    row[classeFilha] = "S";
                }
                row[conclusao] = item.Conclusao;
                row[percentualExecutado] = item.PercentualExecutado;
                row[percentualComprometido] = item.PercentualComprometido;
                row[girErro] = "";

                dta.Rows.Add(row);
            }

            return dta;
        }

        private bool ValidaProcessamentoRelAcompanhamentoFinanceiro(RelAcompanhamentoFinanceiroFiltro filtro)
        {
            if (!filtro.DataInicial.HasValue)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Data inicial"), TypeMessage.Warning);
                return false;
            }

            if (!filtro.DataFinal.HasValue)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Data final"), TypeMessage.Warning);
                return false;
            }

            if (filtro.DataInicial.Value.Date > filtro.DataFinal.Value.Date)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.DataMaximaMenorQueMinima, "Data final"), TypeMessage.Warning);
                return false;
            }

            if (filtro.CentroCusto == null)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Centro de custo"), TypeMessage.Warning);
                return false;
            }

            if ((filtro.IndiceId.HasValue && filtro.IndiceId.Value > 0) && (!filtro.Defasagem.HasValue))
            {
                messageQueue.Add(string.Format("Informe a defasagem.", "Defasagem"), TypeMessage.Warning);
                return false;

            }

            if ((!filtro.IndiceId.HasValue ) && (filtro.EhValorCorrigido))
            {
                messageQueue.Add(string.Format("Informe o índice.", "Índice"), TypeMessage.Warning);
                return false;
            }

            return true;
        }


        private List<RelAcompanhamentoFinanceiroDTO> ListarPeloFiltroRelAcompanhamentoFinanceiroPorTitulo(RelAcompanhamentoFinanceiroFiltro filtro)
        {

            List<RelAcompanhamentoFinanceiroDTO> listaRelAcompanhamentoFinanceiro = new List<RelAcompanhamentoFinanceiroDTO>();

            if (!ValidaProcessamentoRelAcompanhamentoFinanceiro(filtro))
            {
                return listaRelAcompanhamentoFinanceiro;
            }

            decimal cotacaoReajuste = 1;
            int defasagem = filtro.Defasagem.HasValue ? filtro.Defasagem.Value : 0;
            if ((filtro.EhValorCorrigido) && (filtro.IndiceId.HasValue))
            {
                DateTime dataReajuste = filtro.DataFinal.Value.Date.AddMonths(defasagem);
                cotacaoReajuste = cotacaoValoresRepository.RecuperaCotacao(filtro.IndiceId.Value, dataReajuste.Date);
            }

            List<CentroCusto> listaCentroCusto = centroCustoRepository.ListarPeloFiltro(l => ((l.Codigo.Contains(filtro.CentroCusto.Codigo)) && (!l.ListaFilhos.Any()))).ToList<CentroCusto>();
            bool descartar;

            #region "Orcamento inicial"

            foreach (var centroCusto in listaCentroCusto)
            {
                var orcamentoPrimeiro = orcamentoRepository.ObterPrimeiroOrcamentoPeloCentroCusto(centroCusto.Codigo, l => l.Obra, l => l.ListaOrcamentoClasse, l => l.ListaOrcamentoComposicao.Select(c => c.Classe));

                if (orcamentoPrimeiro != null)
                {
                    foreach (var orcamentoComposicao in orcamentoPrimeiro.ListaOrcamentoComposicao)
                    {
                        if (filtro.ListaClasse.Count > 0)
                        {
                            descartar = true;
                            foreach (var classe in filtro.ListaClasse)
                            {
                                if (orcamentoComposicao.CodigoClasse.StartsWith(classe.Codigo))
                                {
                                    descartar = false;
                                }
                            }
                            if (descartar)
                            {
                                continue;
                            }
                        }

                        decimal valorIndice = moduloSigimAppService.CalculaValorIndice(filtro.IndiceId, defasagem, orcamentoPrimeiro.Data.Value, orcamentoComposicao.Preco.Value);
                        decimal valor = (valorIndice * orcamentoComposicao.Quantidade.Value * cotacaoReajuste);

                        RelAcompanhamentoFinanceiroDTO relat = new RelAcompanhamentoFinanceiroDTO();

                        if (!listaRelAcompanhamentoFinanceiro.Any(l => l.CodigoClasse == orcamentoComposicao.CodigoClasse))
                        {
                            relat.CodigoClasse = orcamentoComposicao.CodigoClasse;
                            relat.DescricaoClasse = orcamentoComposicao.Classe.Descricao;
                            relat.OrcamentoInicial = valor;
                            relat.CodigoClassePai = orcamentoComposicao.Classe.CodigoPai;
                            relat.ClassePossuiFilhos = false;
                            if ((orcamentoComposicao.Classe.ListaFilhos != null) && (orcamentoComposicao.Classe.ListaFilhos.Count > 0))
                            {
                                relat.ClassePossuiFilhos = true;
                            }

                            listaRelAcompanhamentoFinanceiro.Add(relat);
                        }
                        else
                        {
                            relat = listaRelAcompanhamentoFinanceiro.Where(l => l.CodigoClasse == orcamentoComposicao.CodigoClasse).FirstOrDefault();
                            relat.OrcamentoInicial = relat.OrcamentoInicial + valor;
                        }
                        AdicionarValorNaClassePaiRelAcompanhamentoFinanceiro(valor, orcamentoComposicao.Classe.ClassePai, listaRelAcompanhamentoFinanceiro, "OrcamentoInicial");
                    }
                }
            }
            #endregion

            #region "Orçamento Atual"

            foreach (var centroCusto in listaCentroCusto)
            {
                var orcamentoUltimo = orcamentoRepository.ObterUltimoOrcamentoPeloCentroCusto(centroCusto.Codigo, l => l.Obra, l => l.ListaOrcamentoClasse, l => l.ListaOrcamentoComposicao.Select(c => c.Classe));

                if (orcamentoUltimo != null)
                {
                    foreach (var orcamentoComposicao in orcamentoUltimo.ListaOrcamentoComposicao)
                    {
                        if (filtro.ListaClasse.Count > 0)
                        {
                            descartar = true;
                            foreach (var classe in filtro.ListaClasse)
                            {
                                if (orcamentoComposicao.CodigoClasse.StartsWith(classe.Codigo))
                                {
                                    descartar = false;
                                }
                            }
                            if (descartar)
                            {
                                continue;
                            }
                        }

                        decimal valorIndice = moduloSigimAppService.CalculaValorIndice(filtro.IndiceId, defasagem, orcamentoUltimo.Data.Value, orcamentoComposicao.Preco.Value);
                        decimal valor = (valorIndice * orcamentoComposicao.Quantidade.Value * cotacaoReajuste);

                        RelAcompanhamentoFinanceiroDTO relat = new RelAcompanhamentoFinanceiroDTO();

                        if (!listaRelAcompanhamentoFinanceiro.Any(l => l.CodigoClasse == orcamentoComposicao.CodigoClasse))
                        {
                            relat.CodigoClasse = orcamentoComposicao.CodigoClasse;
                            relat.DescricaoClasse = orcamentoComposicao.Classe.Descricao;
                            relat.OrcamentoAtual = valor;
                            relat.CodigoClassePai = orcamentoComposicao.Classe.CodigoPai;
                            relat.ClassePossuiFilhos = false;
                            if ((orcamentoComposicao.Classe.ListaFilhos != null) && (orcamentoComposicao.Classe.ListaFilhos.Count > 0))
                            {
                                relat.ClassePossuiFilhos = true;
                            }

                            OrcamentoClasse orcamentoClasse = orcamentoUltimo.ListaOrcamentoClasse.Where(l => l.ClasseCodigo == orcamentoComposicao.CodigoClasse).FirstOrDefault();
                            relat.DescricaoClasseFechada = orcamentoClasse.Fechada.HasValue ? (orcamentoClasse.Fechada.Value ? "F" : "") : "";

                            listaRelAcompanhamentoFinanceiro.Add(relat);
                        }
                        else
                        {
                            relat = listaRelAcompanhamentoFinanceiro.Where(l => l.CodigoClasse == orcamentoComposicao.CodigoClasse).FirstOrDefault();

                            relat.OrcamentoAtual = relat.OrcamentoAtual + valor;

                            OrcamentoClasse orcamentoClasse = orcamentoUltimo.ListaOrcamentoClasse.Where(l => l.ClasseCodigo == orcamentoComposicao.CodigoClasse).FirstOrDefault();
                            relat.DescricaoClasseFechada = orcamentoClasse.Fechada.HasValue ? (orcamentoClasse.Fechada.Value ? "F" : "") : "";

                        }

                        AdicionarValorNaClassePaiRelAcompanhamentoFinanceiro(valor, orcamentoComposicao.Classe.ClassePai, listaRelAcompanhamentoFinanceiro, "OrcamentoAtual");
                    }
                }
            }
            #endregion

            #region "Despesa por periodo"

            var specification = (Specification<Apropriacao>)new TrueSpecification<Apropriacao>();
            specification = MontarSpecificationDespesaPorPeriodoTituloPagarRelAcompanhamentoFinanceiro(filtro);

            var listaApropriacao = apropriacaoRepository.ListarPeloFiltro(specification,
                                                                          l => l.CentroCusto,
                                                                          l => l.Classe,
                                                                          l => l.TituloPagar).To<List<Apropriacao>>();

            foreach (var apropriacao in listaApropriacao)
            {
                if (filtro.ListaClasse.Count > 0)
                {
                    descartar = true;
                    foreach (var classe in filtro.ListaClasse)
                    {
                        if (apropriacao.CodigoClasse.StartsWith(classe.Codigo))
                        {
                            descartar = false;
                        }
                    }
                    if (descartar)
                    {
                        continue;
                    }
                }

                decimal valorIndice = moduloSigimAppService.CalculaValorIndice(filtro.IndiceId, defasagem, apropriacao.TituloPagar.DataEmissao.Value, apropriacao.Valor);
                decimal valor = valorIndice * cotacaoReajuste;

                RelAcompanhamentoFinanceiroDTO relat = new RelAcompanhamentoFinanceiroDTO();

                if (!listaRelAcompanhamentoFinanceiro.Any(l => l.CodigoClasse == apropriacao.CodigoClasse))
                {
                    relat.CodigoClasse = apropriacao.CodigoClasse;
                    relat.DescricaoClasse = apropriacao.Classe.Descricao;
                    relat.DespesaPeriodo = valor;
                    relat.CodigoClassePai = apropriacao.Classe.CodigoPai;
                    relat.ClassePossuiFilhos = false;
                    if ((apropriacao.Classe.ListaFilhos != null) && (apropriacao.Classe.ListaFilhos.Count > 0))
                    {
                        relat.ClassePossuiFilhos = true;
                    }

                    listaRelAcompanhamentoFinanceiro.Add(relat);
                }
                else
                {
                    relat = listaRelAcompanhamentoFinanceiro.Where(l => l.CodigoClasse == apropriacao.CodigoClasse).FirstOrDefault();
                    relat.DespesaPeriodo = relat.DespesaPeriodo + valor;
                }

                AdicionarValorNaClassePaiRelAcompanhamentoFinanceiro(valor, apropriacao.Classe.ClassePai, listaRelAcompanhamentoFinanceiro, "DespesaPeriodo");
            }

            specification = (Specification<Apropriacao>)new TrueSpecification<Apropriacao>();
            specification = MontarSpecificationDespesaPorPeriodoMovimentoRelAcompanhamentoFinanceiro(filtro);
            listaApropriacao = apropriacaoRepository.ListarPeloFiltro(specification,
                                                                          l => l.CentroCusto,
                                                                          l => l.Classe,
                                                                          l => l.Movimento.TipoMovimento).To<List<Apropriacao>>();
            foreach (var apropriacao in listaApropriacao)
            {
                if ((filtro.ListaClasse.Count > 0) && (!filtro.ListaClasse.Any(l => l.Codigo == apropriacao.CodigoClasse))) continue;

                decimal valorIndice = moduloSigimAppService.CalculaValorIndice(filtro.IndiceId, defasagem, apropriacao.Movimento.DataMovimento.Date, apropriacao.Valor);
                decimal valor = valorIndice * cotacaoReajuste;

                RelAcompanhamentoFinanceiroDTO relat = new RelAcompanhamentoFinanceiroDTO();

                if (!listaRelAcompanhamentoFinanceiro.Any(l => l.CodigoClasse == apropriacao.CodigoClasse))
                {
                    relat.CodigoClasse = apropriacao.CodigoClasse;
                    relat.DescricaoClasse = apropriacao.Classe.Descricao;
                    relat.DespesaPeriodo = valor;
                    relat.CodigoClassePai = apropriacao.Classe.CodigoPai;
                    relat.ClassePossuiFilhos = false;
                    if ((apropriacao.Classe.ListaFilhos != null) && (apropriacao.Classe.ListaFilhos.Count > 0))
                    {
                        relat.ClassePossuiFilhos = true;
                    }

                    listaRelAcompanhamentoFinanceiro.Add(relat);
                }
                else
                {
                    relat = listaRelAcompanhamentoFinanceiro.Where(l => l.CodigoClasse == apropriacao.CodigoClasse).FirstOrDefault();
                    relat.DespesaPeriodo = relat.DespesaPeriodo + valor;
                }

                AdicionarValorNaClassePaiRelAcompanhamentoFinanceiro(valor, apropriacao.Classe.ClassePai, listaRelAcompanhamentoFinanceiro, "DespesaPeriodo");
            }

            #endregion

            #region "Despesa acumulada"

            specification = (Specification<Apropriacao>)new TrueSpecification<Apropriacao>();
            specification = MontarSpecificationDespesaAcumuladaTituloPagarRelAcompanhamentoFinanceiro(filtro);

            listaApropriacao = apropriacaoRepository.ListarPeloFiltro(specification,
                                                                      l => l.CentroCusto,
                                                                      l => l.Classe,
                                                                      l => l.TituloPagar).To<List<Apropriacao>>();

            foreach (var apropriacao in listaApropriacao)
            {
                if (filtro.ListaClasse.Count > 0)
                {
                    descartar = true;
                    foreach (var classe in filtro.ListaClasse)
                    {
                        if (apropriacao.CodigoClasse.StartsWith(classe.Codigo))
                        {
                            descartar = false;
                        }
                    }
                    if (descartar)
                    {
                        continue;
                    }
                }

                decimal valorIndice = moduloSigimAppService.CalculaValorIndice(filtro.IndiceId, defasagem, apropriacao.TituloPagar.DataEmissao.Value, apropriacao.Valor);
                decimal valor = valorIndice * cotacaoReajuste;

                RelAcompanhamentoFinanceiroDTO relat = new RelAcompanhamentoFinanceiroDTO();

                if (!listaRelAcompanhamentoFinanceiro.Any(l => l.CodigoClasse == apropriacao.CodigoClasse))
                {
                    relat.CodigoClasse = apropriacao.CodigoClasse;
                    relat.DescricaoClasse = apropriacao.Classe.Descricao;
                    relat.DespesaAcumulada = valor;
                    relat.CodigoClassePai = apropriacao.Classe.CodigoPai;
                    relat.ClassePossuiFilhos = false;
                    if ((apropriacao.Classe.ListaFilhos != null) && (apropriacao.Classe.ListaFilhos.Count > 0))
                    {
                        relat.ClassePossuiFilhos = true;
                    }

                    listaRelAcompanhamentoFinanceiro.Add(relat);
                }
                else
                {
                    relat = listaRelAcompanhamentoFinanceiro.Where(l => l.CodigoClasse == apropriacao.CodigoClasse).FirstOrDefault();
                    relat.DespesaAcumulada = relat.DespesaAcumulada + valor;
                }

                AdicionarValorNaClassePaiRelAcompanhamentoFinanceiro(valor, apropriacao.Classe.ClassePai, listaRelAcompanhamentoFinanceiro, "DespesaAcumulada");
            }

            specification = (Specification<Apropriacao>)new TrueSpecification<Apropriacao>();
            specification = MontarSpecificationDespesaAcumuladaMovimentoRelAcompanhamentoFinanceiro(filtro);
            listaApropriacao = apropriacaoRepository.ListarPeloFiltro(specification,
                                                                          l => l.CentroCusto,
                                                                          l => l.Classe,
                                                                          l => l.Movimento.TipoMovimento).To<List<Apropriacao>>();
            foreach (var apropriacao in listaApropriacao)
            {
                if (filtro.ListaClasse.Count > 0)
                {
                    descartar = true;
                    foreach (var classe in filtro.ListaClasse)
                    {
                        if (apropriacao.CodigoClasse.StartsWith(classe.Codigo))
                        {
                            descartar = false;
                        }
                    }
                    if (descartar)
                    {
                        continue;
                    }
                }

                decimal valorIndice = moduloSigimAppService.CalculaValorIndice(filtro.IndiceId, defasagem, apropriacao.Movimento.DataMovimento.Date, apropriacao.Valor);
                decimal valor = valorIndice * cotacaoReajuste;

                RelAcompanhamentoFinanceiroDTO relat = new RelAcompanhamentoFinanceiroDTO();

                if (!listaRelAcompanhamentoFinanceiro.Any(l => l.CodigoClasse == apropriacao.CodigoClasse))
                {
                    relat.CodigoClasse = apropriacao.CodigoClasse;
                    relat.DescricaoClasse = apropriacao.Classe.Descricao;
                    relat.DespesaAcumulada = valor;
                    relat.CodigoClassePai = apropriacao.Classe.CodigoPai;
                    relat.ClassePossuiFilhos = false;
                    if ((apropriacao.Classe.ListaFilhos != null) && (apropriacao.Classe.ListaFilhos.Count > 0))
                    {
                        relat.ClassePossuiFilhos = true;
                    }

                    listaRelAcompanhamentoFinanceiro.Add(relat);
                }
                else
                {
                    relat = listaRelAcompanhamentoFinanceiro.Where(l => l.CodigoClasse == apropriacao.CodigoClasse).FirstOrDefault();
                    relat.DespesaAcumulada = relat.DespesaAcumulada + valor;
                }

                AdicionarValorNaClassePaiRelAcompanhamentoFinanceiro(valor, apropriacao.Classe.ClassePai, listaRelAcompanhamentoFinanceiro, "DespesaAcumulada");
            }

            #endregion

            #region "Comprometido pendente"

            specification = (Specification<Apropriacao>)new TrueSpecification<Apropriacao>();
            specification = MontarSpecificationComprometidoPendenteTituloPagarRelAcompanhamentoFinanceiro(filtro);

            listaApropriacao = apropriacaoRepository.ListarPeloFiltro(specification,
                                                                      l => l.CentroCusto,
                                                                      l => l.Classe,
                                                                      l => l.TituloPagar).To<List<Apropriacao>>();

            foreach (var apropriacao in listaApropriacao)
            {
                if (filtro.ListaClasse.Count > 0)
                {
                    descartar = true;
                    foreach (var classe in filtro.ListaClasse)
                    {
                        if (apropriacao.CodigoClasse.StartsWith(classe.Codigo))
                        {
                            descartar = false;
                        }
                    }
                    if (descartar)
                    {
                        continue;
                    }
                }

                decimal valorIndice = moduloSigimAppService.CalculaValorIndice(filtro.IndiceId, defasagem, apropriacao.TituloPagar.DataVencimento, apropriacao.Valor);
                decimal valor = valorIndice * cotacaoReajuste;

                RelAcompanhamentoFinanceiroDTO relat = new RelAcompanhamentoFinanceiroDTO();

                if (!listaRelAcompanhamentoFinanceiro.Any(l => l.CodigoClasse == apropriacao.CodigoClasse))
                {
                    relat.CodigoClasse = apropriacao.CodigoClasse;
                    relat.DescricaoClasse = apropriacao.Classe.Descricao;
                    relat.ComprometidoPendente = valor;
                    relat.CodigoClassePai = apropriacao.Classe.CodigoPai;
                    relat.ClassePossuiFilhos = false;
                    if ((apropriacao.Classe.ListaFilhos != null) && (apropriacao.Classe.ListaFilhos.Count > 0))
                    {
                        relat.ClassePossuiFilhos = true;
                    }

                    listaRelAcompanhamentoFinanceiro.Add(relat);
                }
                else
                {
                    relat = listaRelAcompanhamentoFinanceiro.Where(l => l.CodigoClasse == apropriacao.CodigoClasse).FirstOrDefault();
                    relat.ComprometidoPendente = relat.ComprometidoPendente + valor;
                }

                AdicionarValorNaClassePaiRelAcompanhamentoFinanceiro(valor, apropriacao.Classe.ClassePai, listaRelAcompanhamentoFinanceiro, "ComprometidoPendente");
            }

            #endregion

            #region "Comprometido futuro"

            specification = (Specification<Apropriacao>)new TrueSpecification<Apropriacao>();
            specification = MontarSpecificationComprometidoFuturoTituloPagarRelAcompanhamentoFinanceiro(filtro);

            listaApropriacao = apropriacaoRepository.ListarPeloFiltro(specification,
                                                                      l => l.CentroCusto,
                                                                      l => l.Classe,
                                                                      l => l.TituloPagar).To<List<Apropriacao>>();

            foreach (var apropriacao in listaApropriacao)
            {
                if (filtro.ListaClasse.Count > 0)
                {
                    descartar = true;
                    foreach (var classe in filtro.ListaClasse)
                    {
                        if (apropriacao.CodigoClasse.StartsWith(classe.Codigo))
                        {
                            descartar = false;
                        }
                    }
                    if (descartar)
                    {
                        continue;
                    }
                }

                decimal valorIndice = moduloSigimAppService.CalculaValorIndice(filtro.IndiceId, defasagem, apropriacao.TituloPagar.DataVencimento, apropriacao.Valor);
                decimal valor = valorIndice * cotacaoReajuste;

                RelAcompanhamentoFinanceiroDTO relat = new RelAcompanhamentoFinanceiroDTO();

                if (!listaRelAcompanhamentoFinanceiro.Any(l => l.CodigoClasse == apropriacao.CodigoClasse))
                {
                    relat.CodigoClasse = apropriacao.CodigoClasse;
                    relat.DescricaoClasse = apropriacao.Classe.Descricao;
                    relat.ComprometidoFuturo = valor;
                    relat.CodigoClassePai = apropriacao.Classe.CodigoPai;
                    relat.ClassePossuiFilhos = false;
                    if ((apropriacao.Classe.ListaFilhos != null) && (apropriacao.Classe.ListaFilhos.Count > 0))
                    {
                        relat.ClassePossuiFilhos = true;
                    }

                    listaRelAcompanhamentoFinanceiro.Add(relat);
                }
                else
                {
                    relat = listaRelAcompanhamentoFinanceiro.Where(l => l.CodigoClasse == apropriacao.CodigoClasse).FirstOrDefault();
                    relat.ComprometidoFuturo = relat.ComprometidoFuturo + valor;
                }

                AdicionarValorNaClassePaiRelAcompanhamentoFinanceiro(valor, apropriacao.Classe.ClassePai, listaRelAcompanhamentoFinanceiro, "ComprometidoFuturo");
            }

            #endregion

            #region "Resultado Acrescimo, ResultadoSaldo e Conclusão"

            foreach (RelAcompanhamentoFinanceiroDTO reg in listaRelAcompanhamentoFinanceiro.Where(l => l.ClassePossuiFilhos == false).OrderBy(l => l.CodigoClasse))
            {
                decimal resultadoAcrescimo = reg.OrcamentoAtual - (Math.Round(reg.DespesaAcumulada, 2) + Math.Round(reg.ComprometidoPendente, 2) + Math.Round(reg.ComprometidoFuturo, 2));
                reg.ResultadoAcrescimo = 0;
                if (resultadoAcrescimo < 0)
                {
                    reg.ResultadoAcrescimo = resultadoAcrescimo;
                }
                decimal resultadoSaldo = reg.OrcamentoAtual - (Math.Round(reg.DespesaAcumulada, 2) + Math.Round(reg.ComprometidoPendente, 2) + Math.Round(reg.ComprometidoFuturo, 2));
                reg.ResultadoSaldo = 0;
                if (resultadoSaldo > 0)
                {
                    reg.ResultadoSaldo = resultadoSaldo;
                }
                decimal conclusao = 0;
                conclusao = Math.Round(reg.DespesaAcumulada, 2) + Math.Round(reg.ComprometidoPendente, 2) + Math.Round(reg.ComprometidoFuturo, 2);
                if (reg.DescricaoClasseFechada == "F")
                {
                    reg.Conclusao = conclusao;
                }
                else
                {
                    if (conclusao > reg.OrcamentoAtual)
                    {
                        reg.Conclusao = conclusao;
                    }
                    else
                    {
                        reg.Conclusao = reg.OrcamentoAtual;
                    }
                }

                AdicionarValorNaClassePaiRelAcompanhamentoFinanceiroOutrasColunas(reg.CodigoClassePai, reg, listaRelAcompanhamentoFinanceiro, false);

            }

            #endregion

            return listaRelAcompanhamentoFinanceiro;
        }

        private List<RelAcompanhamentoFinanceiroDTO> ListarPeloFiltroRelAcompanhamentoFinanceiroExecutado(RelAcompanhamentoFinanceiroFiltro filtro)
        {
            List<RelAcompanhamentoFinanceiroDTO> listaRelAcompanhamentoFinanceiro = new List<RelAcompanhamentoFinanceiroDTO>();

            if (!ValidaProcessamentoRelAcompanhamentoFinanceiro(filtro))
            {
                return listaRelAcompanhamentoFinanceiro; 
            }

            decimal cotacaoReajuste = 1;
            int defasagem = filtro.Defasagem.HasValue ? filtro.Defasagem.Value : 0;
            if ((filtro.EhValorCorrigido) && (filtro.IndiceId.HasValue))
            {
                DateTime dataReajuste = filtro.DataFinal.Value.Date.AddMonths(defasagem);
                cotacaoReajuste = cotacaoValoresRepository.RecuperaCotacao(filtro.IndiceId.Value, dataReajuste.Date);
            }

            List<CentroCusto> listaCentroCusto = centroCustoRepository.ListarPeloFiltro(l => ((l.Codigo.Contains(filtro.CentroCusto.Codigo)) && (!l.ListaFilhos.Any()))).ToList<CentroCusto>();
            bool descartar = false;

            #region "Orcamento inicial"

            foreach (var centroCusto in listaCentroCusto)
            {
                var orcamentoPrimeiro = orcamentoRepository.ObterPrimeiroOrcamentoPeloCentroCusto(centroCusto.Codigo, l => l.Obra, l => l.ListaOrcamentoClasse, l => l.ListaOrcamentoComposicao.Select(c => c.Classe));

                if (orcamentoPrimeiro != null)
                {
                    foreach (var orcamentoComposicao in orcamentoPrimeiro.ListaOrcamentoComposicao)
                    {
                        if (filtro.ListaClasse.Count > 0)
                        {
                            descartar = true;
                            foreach (var classe in filtro.ListaClasse)
                            {
                                if (orcamentoComposicao.CodigoClasse.StartsWith(classe.Codigo))
                                {
                                    descartar = false;
                                }
                            }
                            if (descartar)
                            {
                                continue;
                            }
                        }

                        decimal valorIndice = moduloSigimAppService.CalculaValorIndice(filtro.IndiceId, defasagem, orcamentoPrimeiro.Data.Value, orcamentoComposicao.Preco.Value);
                        decimal valor = (valorIndice * orcamentoComposicao.Quantidade.Value * cotacaoReajuste);

                        RelAcompanhamentoFinanceiroDTO relat = new RelAcompanhamentoFinanceiroDTO();

                        if (!listaRelAcompanhamentoFinanceiro.Any(l => l.CodigoClasse == orcamentoComposicao.CodigoClasse))
                        {
                            relat.CodigoClasse = orcamentoComposicao.CodigoClasse;
                            relat.DescricaoClasse = orcamentoComposicao.Classe.Descricao;
                            relat.OrcamentoInicial = valor;
                            relat.CodigoClassePai = orcamentoComposicao.Classe.CodigoPai;
                            relat.ClassePossuiFilhos = false;
                            if ((orcamentoComposicao.Classe.ListaFilhos != null) && (orcamentoComposicao.Classe.ListaFilhos.Count > 0))
                            {
                                relat.ClassePossuiFilhos = true;
                            }

                            listaRelAcompanhamentoFinanceiro.Add(relat);
                        }
                        else
                        {
                            relat = listaRelAcompanhamentoFinanceiro.Where(l => l.CodigoClasse == orcamentoComposicao.CodigoClasse).FirstOrDefault();
                            relat.OrcamentoInicial = relat.OrcamentoInicial + valor;
                        }
                        AdicionarValorNaClassePaiRelAcompanhamentoFinanceiro(valor, orcamentoComposicao.Classe.ClassePai, listaRelAcompanhamentoFinanceiro, "OrcamentoInicial");
                    }
                }
            }
            #endregion

            #region "Orçamento Atual"

            foreach (var centroCusto in listaCentroCusto)
            {
                var orcamentoUltimo = orcamentoRepository.ObterUltimoOrcamentoPeloCentroCusto(centroCusto.Codigo, l => l.Obra, l => l.ListaOrcamentoClasse, l => l.ListaOrcamentoComposicao.Select(c => c.Classe));

                if (orcamentoUltimo != null)
                {
                    foreach (var orcamentoComposicao in orcamentoUltimo.ListaOrcamentoComposicao)
                    {

                        if (filtro.ListaClasse.Count > 0)
                        {
                            descartar = true;
                            foreach (var classe in filtro.ListaClasse)
                            {
                                if (orcamentoComposicao.CodigoClasse.StartsWith(classe.Codigo))
                                {
                                    descartar = false;
                                }
                            }
                            if (descartar)
                            {
                                continue;
                            }
                        }

                        decimal valorIndice = moduloSigimAppService.CalculaValorIndice(filtro.IndiceId, defasagem, orcamentoUltimo.Data.Value, orcamentoComposicao.Preco.Value);
                        decimal valor = (valorIndice * orcamentoComposicao.Quantidade.Value * cotacaoReajuste);

                        RelAcompanhamentoFinanceiroDTO relat = new RelAcompanhamentoFinanceiroDTO();

                        if (!listaRelAcompanhamentoFinanceiro.Any(l => l.CodigoClasse == orcamentoComposicao.CodigoClasse))
                        {
                            relat.CodigoClasse = orcamentoComposicao.CodigoClasse;
                            relat.DescricaoClasse = orcamentoComposicao.Classe.Descricao;
                            relat.OrcamentoAtual = valor;
                            relat.CodigoClassePai = orcamentoComposicao.Classe.CodigoPai;
                            relat.ClassePossuiFilhos = false;
                            if ((orcamentoComposicao.Classe.ListaFilhos != null) && (orcamentoComposicao.Classe.ListaFilhos.Count > 0))
                            {
                                relat.ClassePossuiFilhos = true;
                            }

                            OrcamentoClasse orcamentoClasse = orcamentoUltimo.ListaOrcamentoClasse.Where(l => l.ClasseCodigo == orcamentoComposicao.CodigoClasse).FirstOrDefault();
                            relat.DescricaoClasseFechada = orcamentoClasse.Fechada.HasValue ? (orcamentoClasse.Fechada.Value ? "F" : "") : "";

                            listaRelAcompanhamentoFinanceiro.Add(relat);
                        }
                        else
                        {
                            relat = listaRelAcompanhamentoFinanceiro.Where(l => l.CodigoClasse == orcamentoComposicao.CodigoClasse).FirstOrDefault();

                            relat.OrcamentoAtual = relat.OrcamentoAtual + valor;

                            OrcamentoClasse orcamentoClasse = orcamentoUltimo.ListaOrcamentoClasse.Where(l => l.ClasseCodigo == orcamentoComposicao.CodigoClasse).FirstOrDefault();
                            relat.DescricaoClasseFechada = orcamentoClasse.Fechada.HasValue ? (orcamentoClasse.Fechada.Value ? "F" : "") : "";

                        }

                        AdicionarValorNaClassePaiRelAcompanhamentoFinanceiro(valor, orcamentoComposicao.Classe.ClassePai, listaRelAcompanhamentoFinanceiro, "OrcamentoAtual");
                    }
                }
            }
            #endregion

            #region "Despesa acumulada"

            var specification = (Specification<Apropriacao>)new TrueSpecification<Apropriacao>();
            specification = MontarSpecificationDespesaAcumuladaTituloPagarRelAcompanhamentoFinanceiroExecutado(filtro);

            var listaApropriacao = apropriacaoRepository.ListarPeloFiltro(specification,
                                                                          l => l.CentroCusto,
                                                                          l => l.Classe,
                                                                          l => l.TituloPagar).To<List<Apropriacao>>();

            foreach (var apropriacao in listaApropriacao)
            {
                if (filtro.ListaClasse.Count > 0)
                {
                    descartar = true;
                    foreach (var classe in filtro.ListaClasse)
                    {
                        if (apropriacao.CodigoClasse.StartsWith(classe.Codigo))
                        {
                            descartar = false;
                        }
                    }
                    if (descartar)
                    {
                        continue;
                    }
                }

                decimal valorIndice = moduloSigimAppService.CalculaValorIndice(filtro.IndiceId, defasagem, apropriacao.TituloPagar.DataEmissao.Value, apropriacao.Valor);
                decimal valor = valorIndice * cotacaoReajuste;

                RelAcompanhamentoFinanceiroDTO relat = new RelAcompanhamentoFinanceiroDTO();

                if (!listaRelAcompanhamentoFinanceiro.Any(l => l.CodigoClasse == apropriacao.CodigoClasse))
                {
                    relat.CodigoClasse = apropriacao.CodigoClasse;
                    relat.DescricaoClasse = apropriacao.Classe.Descricao;
                    relat.DespesaAcumulada = valor;
                    relat.CodigoClassePai = apropriacao.Classe.CodigoPai;
                    relat.ClassePossuiFilhos = false;
                    if ((apropriacao.Classe.ListaFilhos != null) && (apropriacao.Classe.ListaFilhos.Count > 0))
                    {
                        relat.ClassePossuiFilhos = true;
                    }

                    listaRelAcompanhamentoFinanceiro.Add(relat);
                }
                else
                {
                    relat = listaRelAcompanhamentoFinanceiro.Where(l => l.CodigoClasse == apropriacao.CodigoClasse).FirstOrDefault();
                    relat.DespesaAcumulada = relat.DespesaAcumulada + valor;
                }

                AdicionarValorNaClassePaiRelAcompanhamentoFinanceiro(valor, apropriacao.Classe.ClassePai, listaRelAcompanhamentoFinanceiro, "DespesaAcumulada");
            }

            specification = (Specification<Apropriacao>)new TrueSpecification<Apropriacao>();
            specification = MontarSpecificationDespesaAcumuladaTituloPagarAguardandoEhLiberadosRelAcompanhamentoFinanceiroExecutado(filtro);

            listaApropriacao = apropriacaoRepository.ListarPeloFiltro(specification,
                                                                      l => l.CentroCusto,
                                                                      l => l.Classe,
                                                                      l => l.TituloPagar).To<List<Apropriacao>>();

            foreach (var apropriacao in listaApropriacao)
            {
                if (filtro.ListaClasse.Count > 0)
                {
                    descartar = true;
                    foreach (var classe in filtro.ListaClasse)
                    {
                        if (apropriacao.CodigoClasse.StartsWith(classe.Codigo))
                        {
                            descartar = false;
                        }
                    }
                    if (descartar)
                    {
                        continue;
                    }
                }

                decimal valorIndice = moduloSigimAppService.CalculaValorIndice(filtro.IndiceId, defasagem, apropriacao.TituloPagar.DataVencimento, apropriacao.Valor);
                decimal valor = valorIndice * cotacaoReajuste;

                RelAcompanhamentoFinanceiroDTO relat = new RelAcompanhamentoFinanceiroDTO();

                if (!listaRelAcompanhamentoFinanceiro.Any(l => l.CodigoClasse == apropriacao.CodigoClasse))
                {
                    relat.CodigoClasse = apropriacao.CodigoClasse;
                    relat.DescricaoClasse = apropriacao.Classe.Descricao;
                    relat.DespesaAcumulada = valor;
                    relat.CodigoClassePai = apropriacao.Classe.CodigoPai;
                    relat.ClassePossuiFilhos = false;
                    if ((apropriacao.Classe.ListaFilhos != null) && (apropriacao.Classe.ListaFilhos.Count > 0))
                    {
                        relat.ClassePossuiFilhos = true;
                    }

                    listaRelAcompanhamentoFinanceiro.Add(relat);
                }
                else
                {
                    relat = listaRelAcompanhamentoFinanceiro.Where(l => l.CodigoClasse == apropriacao.CodigoClasse).FirstOrDefault();
                    relat.DespesaAcumulada = relat.DespesaAcumulada + valor;
                }

                AdicionarValorNaClassePaiRelAcompanhamentoFinanceiro(valor, apropriacao.Classe.ClassePai, listaRelAcompanhamentoFinanceiro, "DespesaAcumulada");
            }

            specification = (Specification<Apropriacao>)new TrueSpecification<Apropriacao>();
            specification = MontarSpecificationDespesaAcumuladaMovimentoRelAcompanhamentoFinanceiroExecutado(filtro);
            listaApropriacao = apropriacaoRepository.ListarPeloFiltro(specification,
                                                                          l => l.CentroCusto,
                                                                          l => l.Classe,
                                                                          l => l.Movimento.TipoMovimento).To<List<Apropriacao>>();
            foreach (var apropriacao in listaApropriacao)
            {
                if (filtro.ListaClasse.Count > 0)
                {
                    descartar = true;
                    foreach (var classe in filtro.ListaClasse)
                    {
                        if (apropriacao.CodigoClasse.StartsWith(classe.Codigo))
                        {
                            descartar = false;
                        }
                    }
                    if (descartar)
                    {
                        continue;
                    }
                }

                decimal valorIndice = moduloSigimAppService.CalculaValorIndice(filtro.IndiceId, defasagem, apropriacao.Movimento.DataMovimento.Date, apropriacao.Valor);
                decimal valor = valorIndice * cotacaoReajuste;

                RelAcompanhamentoFinanceiroDTO relat = new RelAcompanhamentoFinanceiroDTO();

                if (!listaRelAcompanhamentoFinanceiro.Any(l => l.CodigoClasse == apropriacao.CodigoClasse))
                {
                    relat.CodigoClasse = apropriacao.CodigoClasse;
                    relat.DescricaoClasse = apropriacao.Classe.Descricao;
                    relat.DespesaAcumulada = valor;
                    relat.CodigoClassePai = apropriacao.Classe.CodigoPai;
                    relat.ClassePossuiFilhos = false;
                    if ((apropriacao.Classe.ListaFilhos != null) && (apropriacao.Classe.ListaFilhos.Count > 0))
                    {
                        relat.ClassePossuiFilhos = true;
                    }

                    listaRelAcompanhamentoFinanceiro.Add(relat);
                }
                else
                {
                    relat = listaRelAcompanhamentoFinanceiro.Where(l => l.CodigoClasse == apropriacao.CodigoClasse).FirstOrDefault();
                    relat.DespesaAcumulada = relat.DespesaAcumulada + valor;
                }

                AdicionarValorNaClassePaiRelAcompanhamentoFinanceiro(valor, apropriacao.Classe.ClassePai, listaRelAcompanhamentoFinanceiro, "DespesaAcumulada");
            }

            #endregion

            #region "Outras Colunas"

            foreach (RelAcompanhamentoFinanceiroDTO reg in listaRelAcompanhamentoFinanceiro.Where(l => l.ClassePossuiFilhos == false).OrderBy(l => l.CodigoClasse))
            {
                decimal percentualExecutado = 0;
                percentualExecutado = ObterPercentualExecutado(filtro.CentroCusto.Codigo, reg.CodigoClasse, filtro.DataFinal.Value.Date);

                decimal percentualRestante = 100 - percentualExecutado;
                decimal comprometidoFuturo = 0;
                if (percentualExecutado > 0)
                {
                    if (reg.DespesaAcumulada == 0)
                    {
                        comprometidoFuturo = reg.OrcamentoAtual;
                        percentualRestante = 100;
                    }
                    else
                    {
                        comprometidoFuturo = ((Math.Round(reg.DespesaAcumulada, 2) * percentualRestante) / percentualExecutado);
                    }
                }
                else
                {
                    comprometidoFuturo = reg.OrcamentoAtual;
                }

                decimal resultadoAcrescimo = 0;
                resultadoAcrescimo = reg.OrcamentoAtual - (Math.Round(reg.DespesaAcumulada, 2) + comprometidoFuturo);
                if (resultadoAcrescimo > 0)
                {
                    resultadoAcrescimo = 0;
                }

                decimal resultadoSaldo = 0;
                resultadoSaldo = reg.OrcamentoAtual - (Math.Round(reg.DespesaAcumulada, 2) + comprometidoFuturo);
                if (resultadoSaldo < 0)
                {
                    resultadoSaldo = 0;
                }

                decimal conclusao = 0;
                conclusao = Math.Round(reg.DespesaAcumulada, 2) + comprometidoFuturo;
                if (reg.DescricaoClasseFechada == "F")
                {
                    conclusao = Math.Round(reg.DespesaAcumulada, 2);
                }
                else
                {
                    if (reg.OrcamentoAtual > conclusao)
                    {
                        conclusao = reg.OrcamentoAtual;
                    }
                }
                reg.ComprometidoFuturo = comprometidoFuturo;
                reg.ResultadoAcrescimo = resultadoAcrescimo;
                reg.ResultadoSaldo = resultadoSaldo;
                reg.Conclusao = conclusao;
                reg.PercentualExecutado = percentualExecutado;
                reg.PercentualComprometido = percentualRestante;

                reg.AssinalarRegistro = false;
                if (((reg.DespesaAcumulada == 0) && (reg.PercentualExecutado > 0)) ||
                    ((reg.DespesaAcumulada > 0) && (reg.PercentualExecutado == 0)))
                {
                    reg.AssinalarRegistro = true;
                }

                AdicionarValorNaClassePaiRelAcompanhamentoFinanceiroOutrasColunas(reg.CodigoClassePai, reg, listaRelAcompanhamentoFinanceiro, true);

            }

            foreach (RelAcompanhamentoFinanceiroDTO reg in listaRelAcompanhamentoFinanceiro.Where(l => l.ClassePossuiFilhos == false).OrderBy(l => l.CodigoClasse))
            {
                AdicionarPercentualExecutadoEhComprometidoNaClassePaiRelAcompanhamentoFinanceiro(reg.CodigoClassePai, reg, listaRelAcompanhamentoFinanceiro);
            }

            #endregion

            return listaRelAcompanhamentoFinanceiro;
        }

        #endregion

    }
}
