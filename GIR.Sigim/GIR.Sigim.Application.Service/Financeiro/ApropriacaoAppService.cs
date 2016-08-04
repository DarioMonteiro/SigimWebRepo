using System;
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

            if (!ValidarFiltroRelApropriacaoPorClasse(filtro))
            {
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
                                                        l => l.TituloPagar.Cliente,
                                                        l => l.TituloPagar.TipoDocumento).To<List<Apropriacao>>();
                PopulaApropriacaoClasseRelatorio(listaApropriacao, listaApropriacaoClasseRelatorio, "PagamentosPendentesEhPagos");
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
                                                        l => l.TituloPagar.Cliente,
                                                        l => l.TituloPagar.TipoDocumento).To<List<Apropriacao>>();
                PopulaApropriacaoClasseRelatorio(listaApropriacao, listaApropriacaoClasseRelatorio, "PagamentosPendentesEhPagos");
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
                                                        l => l.TituloReceber.Cliente,
                                                        l => l.TituloReceber.TipoDocumento).To<List<Apropriacao>>();
                PopulaApropriacaoClasseRelatorio(listaApropriacao, listaApropriacaoClasseRelatorio, "RecebimentosPendentesEhRecebidos");
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
                                                        l => l.TituloReceber.Cliente,
                                                        l => l.TituloReceber.TipoDocumento).To<List<Apropriacao>>();
                PopulaApropriacaoClasseRelatorio(listaApropriacao, listaApropriacaoClasseRelatorio, "RecebimentosPendentesEhRecebidos");
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
                                                        l => l.Movimento.ContaCorrente.Banco,
                                                        l => l.Movimento.ContaCorrente.Agencia).To<List<Apropriacao>>();
                PopulaApropriacaoClasseRelatorio(listaApropriacao, listaApropriacaoClasseRelatorio, "MovimentoDebito");
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
                                                        l => l.Movimento.Caixa).To<List<Apropriacao>>();
                PopulaApropriacaoClasseRelatorio(listaApropriacao, listaApropriacaoClasseRelatorio, "MovimentoDebitoCaixa");
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
                                                        l => l.Movimento.ContaCorrente.Banco,
                                                        l => l.Movimento.ContaCorrente.Agencia).To<List<Apropriacao>>();

                PopulaApropriacaoClasseRelatorio(listaApropriacao, listaApropriacaoClasseRelatorio, "MovimentoCredito");
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
                                                        l => l.Movimento.Caixa).To<List<Apropriacao>>();

                PopulaApropriacaoClasseRelatorio(listaApropriacao, listaApropriacaoClasseRelatorio, "MovimentoCreditoCaixa");
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
                                                              l => l.VerbaCobranca.Classe).To<List<TituloCredCob>>();
                    listaTituloCredCob =
                     tituloCredCobRepository.ListarPeloFiltro(specification,
                                                              l => l.VendaSerie.Renegociacao,
                                                              l => l.VendaSerie.IndiceCorrecao,
                                                              l => l.VendaSerie.IndiceAtrasoCorrecao,
                                                              l => l.VendaSerie.IndiceReajuste,
                                                              l => l.Indice).To<List<TituloCredCob>>();

                    List<TituloDetalheCredCob> listaTituloDetalheCredCob = tituloCredCobAppService.RecTit(listaTituloCredCob, DateTime.Now.Date, false, false);
                    PopulaApropriacaoClasseRelatorio(listaTituloDetalheCredCob, listaApropriacaoClasseRelatorio, "CreditoCobranca");
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
                                                                l => l.MovimentoFinanceiro.ContaCorrente.Banco,
                                                                l => l.MovimentoFinanceiro.ContaCorrente.Agencia).To<List<TituloMovimento>>();

                    PopulaApropriacaoClasseRelatorio(listaTituloMovimento, listaApropriacaoClasseRelatorio, "CreditoCobrancaTituloMovimento");
                }

            }

            listaApropriacaoClasseRelatorio = AgrupaRelApropriacaoPorClasseRelatorio(filtro.OpcoesRelatorio.Value, listaApropriacaoClasseRelatorio);

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
            DataTable dtaRelatorio = new DataTable();

            List<ApropriacaoClasseCCRelatorio> listaApropriacaoClasseRelatorio = listaApropriacaoClasseRelatorioDTO.To<List<ApropriacaoClasseCCRelatorio>>();

            dtaRelatorio = RelApropriacaoPorClasseToDataTable(filtro.OpcoesRelatorio.Value, listaApropriacaoClasseRelatorio);

            var parametros = parametrosFinanceiroRepository.Obter();
            var centroCusto = centroCustoRepository.ObterPeloCodigo(filtro.CentroCusto.Codigo, l => l.ListaCentroCustoEmpresa);
            var caminhoImagem = PrepararIconeRelatorio(centroCusto, parametros);
            var nomeEmpresa = ObterNomeEmpresa(centroCusto, parametros);

            string situacaoPagarSelecao = MontarStringSituacaoAPagar(filtro);
            string situacaoReceberSelecao = MontaStringSituacaoAReceber(filtro);
            string tipoData = MontaStringTipoPesquisa(filtro);

            FileDownloadDTO arquivo = new FileDownloadDTO("Rel. Apropriação por classe ", null, formato);

            switch (filtro.OpcoesRelatorio.Value)
            {
                case (int)OpcoesRelatorioApropriacaoPorClasse.Sintetico:
                    relApropriacaoPorClasseSintetico objRelSint = new relApropriacaoPorClasseSintetico();

                    objRelSint.SetDataSource(dtaRelatorio);

                    objRelSint.SetParameterValue("DataInicial", filtro.DataInicial.Value.ToString("dd/MM/yyyy"));
                    objRelSint.SetParameterValue("DataFinal", filtro.DataFinal.Value.ToString("dd/MM/yyyy"));
                    objRelSint.SetParameterValue("CentroCusto", centroCusto != null ? centroCusto.Codigo + "-" + centroCusto.Descricao : "");
                    objRelSint.SetParameterValue("Tipo", "S");
                    objRelSint.SetParameterValue("nomeEmpresa", nomeEmpresa);
                    objRelSint.SetParameterValue("SituacaoPagar", situacaoPagarSelecao.Trim());
                    objRelSint.SetParameterValue("SituacaoReceber", situacaoReceberSelecao.Trim());
                    objRelSint.SetParameterValue("TipoData", tipoData);
                    objRelSint.SetParameterValue("caminhoImagem", caminhoImagem);

                    arquivo = new FileDownloadDTO("Rel. Apropriação por classe sintético", objRelSint.ExportToStream((ExportFormatType)formato), formato);

                    break;

                case (int)OpcoesRelatorioApropriacaoPorClasse.Analitico:
                    relApropriacaoPorClasseSintetico objRelAna = new relApropriacaoPorClasseSintetico();

                    objRelAna.SetDataSource(dtaRelatorio);

                    objRelAna.SetParameterValue("DataInicial", filtro.DataInicial.Value.ToString("dd/MM/yyyy"));
                    objRelAna.SetParameterValue("DataFinal", filtro.DataFinal.Value.ToString("dd/MM/yyyy"));
                    objRelAna.SetParameterValue("CentroCusto", centroCusto != null ? centroCusto.Codigo + "-" + centroCusto.Descricao : "");
                    objRelAna.SetParameterValue("Tipo", "A");
                    objRelAna.SetParameterValue("nomeEmpresa", nomeEmpresa);
                    objRelAna.SetParameterValue("SituacaoPagar", situacaoPagarSelecao.Trim());
                    objRelAna.SetParameterValue("SituacaoReceber", situacaoReceberSelecao.Trim());
                    objRelAna.SetParameterValue("TipoData", tipoData);
                    objRelAna.SetParameterValue("caminhoImagem", caminhoImagem);

                    arquivo = new FileDownloadDTO("Rel. Apropriação por classe analítico", objRelAna.ExportToStream((ExportFormatType)formato), formato);

                    break;
                case (int)OpcoesRelatorioApropriacaoPorClasse.AnaliticoDetalhado:
                    relApropriacaoPorClasse objRelAnaDet = new relApropriacaoPorClasse();

                    objRelAnaDet.SetDataSource(dtaRelatorio);

                    objRelAnaDet.SetParameterValue("DataInicial", filtro.DataInicial.Value.ToString("dd/MM/yyyy"));
                    objRelAnaDet.SetParameterValue("DataFinal", filtro.DataFinal.Value.ToString("dd/MM/yyyy"));
                    objRelAnaDet.SetParameterValue("CentroCusto", centroCusto != null ? centroCusto.Codigo + "-" + centroCusto.Descricao : "");
                    objRelAnaDet.SetParameterValue("nomeEmpresa", nomeEmpresa);
                    objRelAnaDet.SetParameterValue("SituacaoPagar", situacaoPagarSelecao.Trim());
                    objRelAnaDet.SetParameterValue("SituacaoReceber", situacaoReceberSelecao.Trim());
                    objRelAnaDet.SetParameterValue("TipoData", tipoData);
                    objRelAnaDet.SetParameterValue("caminhoImagem", caminhoImagem);

                    arquivo = new FileDownloadDTO("Rel. Apropriação por classe analítico detalhado", objRelAnaDet.ExportToStream((ExportFormatType)formato), formato);

                    break;
                case (int)OpcoesRelatorioApropriacaoPorClasse.AnaliticoDetalhadoFornecedor:
                    relApropriacaoPorClasseTeste objRelAnaDetFornec = new relApropriacaoPorClasseTeste();

                    objRelAnaDetFornec.SetDataSource(dtaRelatorio);

                    objRelAnaDetFornec.SetParameterValue("DataInicial", filtro.DataInicial.Value.ToString("dd/MM/yyyy"));
                    objRelAnaDetFornec.SetParameterValue("DataFinal", filtro.DataFinal.Value.ToString("dd/MM/yyyy"));
                    objRelAnaDetFornec.SetParameterValue("CentroCusto", centroCusto != null ? centroCusto.Codigo + "-" + centroCusto.Descricao : "");
                    objRelAnaDetFornec.SetParameterValue("nomeEmpresa", nomeEmpresa);
                    objRelAnaDetFornec.SetParameterValue("SituacaoPagar", situacaoPagarSelecao.Trim());
                    objRelAnaDetFornec.SetParameterValue("SituacaoReceber", situacaoReceberSelecao.Trim());
                    objRelAnaDetFornec.SetParameterValue("TipoData", tipoData);
                    objRelAnaDetFornec.SetParameterValue("caminhoImagem", caminhoImagem);

                    arquivo = new FileDownloadDTO("Rel. Apropriação por classe analítico detalhado por fornecedor", objRelAnaDetFornec.ExportToStream((ExportFormatType)formato), formato);

                    break;
            }

            if (System.IO.File.Exists(caminhoImagem))
            {
                System.IO.File.Delete(caminhoImagem);
            }

            return arquivo;

        }

        public bool EhPermitidoImprimirRelAcompanhamentoFinanceiro()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.RelatorioAcompanhamentoFinanceiroImprimir);
        }

        public List<RelAcompanhamentoFinanceiroDTO> ListarPeloFiltroRelAcompanhamentoFinanceiro(RelAcompanhamentoFinanceiroFiltro filtro)
        {
            List<RelAcompanhamentoFinanceiroDTO> listaRelAcompanhamentoFinanceiro = new List<RelAcompanhamentoFinanceiroDTO>();

            if (filtro.BaseadoPor == 0)
            {
                listaRelAcompanhamentoFinanceiro = ListarPeloFiltroRelAcompanhamentoFinanceiroPorTitulo(filtro);
            }
            else
            {
                listaRelAcompanhamentoFinanceiro = ListarPeloFiltroRelAcompanhamentoFinanceiroExecutado(filtro);
            }

            return listaRelAcompanhamentoFinanceiro;
        }

        public List<RelAcompanhamentoFinanceiroDTO> PaginarPeloFiltroRelAcompanhamentoFinanceiro(RelAcompanhamentoFinanceiroFiltro filtro,
                                                                                                 List<RelAcompanhamentoFinanceiroDTO> listaRelAcompanhamentoFinanceiro,
                                                                                                                            out int totalRegistros)
        {
            List<RelAcompanhamentoFinanceiroDTO> lista = new List<RelAcompanhamentoFinanceiroDTO>();

            totalRegistros = listaRelAcompanhamentoFinanceiro.Count();

            listaRelAcompanhamentoFinanceiro = OrdenaListaRelAcompanhamentoFinanceiroDTO(filtro, listaRelAcompanhamentoFinanceiro);

            int pageCount = filtro.PaginationParameters.PageSize;
            int pageIndex = filtro.PaginationParameters.PageIndex;

            lista = listaRelAcompanhamentoFinanceiro.Skip(pageCount * pageIndex).Take(pageCount).To<List<RelAcompanhamentoFinanceiroDTO>>();

            return lista;
        }

        public FileDownloadDTO ExportarRelAcompanhamentoFinanceiro(RelAcompanhamentoFinanceiroFiltro filtro,
                                                                   List<RelAcompanhamentoFinanceiroDTO> listaRelAcompanhamentoFinanceiro,
                                                                   FormatoExportacaoArquivo formato)
        {
            if (!EhPermitidoImprimirRelAcompanhamentoFinanceiro())
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return null;
            }

            FileDownloadDTO arquivo = new FileDownloadDTO("Rel. Acompanhamento financeiro", new System.IO.MemoryStream(), formato);

            if (listaRelAcompanhamentoFinanceiro == null)
            {
                return arquivo;
            }

            listaRelAcompanhamentoFinanceiro = listaRelAcompanhamentoFinanceiro.OrderBy(l => l.CodigoClasse).ToList<RelAcompanhamentoFinanceiroDTO>();


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

            specification &= ApropriacaoSpecification.EhTipoTituloDiferenteDeTituloPagarPai();

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

            specification &= ApropriacaoSpecification.EhTipoTituloDiferenteDeTituloPagarPai();

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

            specification &= ApropriacaoSpecification.EhTipoTituloDiferenteDeTituloPagarPai();

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
                    (ApropriacaoSpecification.EhTipoTituloDiferenteDeTituloPagarPai())
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

            specification &= ApropriacaoSpecification.EhTipoTituloDiferenteDeTituloPagarPai();

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

            if (filtro.ListaClasseDespesa.Count > 0)
            {
                string[] arrayCodigoClasse = PopulaArrayComCodigosDeClassesSelecionadas(filtro.ListaClasseDespesa);

                if (arrayCodigoClasse.Length > 0)
                {
                    specification &= ApropriacaoSpecification.SaoClassesExistentes(arrayCodigoClasse);
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

            specification &= ApropriacaoSpecification.EhTipoTituloDiferenteDeTituloReceberPai();

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

            if (filtro.ListaClasseReceita.Count > 0)
            {
                string[] arrayCodigoClasse = PopulaArrayComCodigosDeClassesSelecionadas(filtro.ListaClasseReceita);

                if (arrayCodigoClasse.Length > 0)
                {
                    specification &= ApropriacaoSpecification.SaoClassesExistentes(arrayCodigoClasse);
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

        private void PopulaApropriacaoClasseRelatorio(List<Apropriacao> listaApropriacao, List<ApropriacaoClasseCCRelatorio> listaApropriacaoClasseRelatorio,string situacaoRelatorio)
        {

            foreach (var apropriacao in listaApropriacao)
            {
                ApropriacaoClasseCCRelatorio  apropriacaoClasseCCRelatorio = new ApropriacaoClasseCCRelatorio();

                apropriacaoClasseCCRelatorio.ValorApropriado = apropriacao.Valor;
                apropriacaoClasseCCRelatorio.Classe = apropriacao.Classe;
                apropriacaoClasseCCRelatorio.CentroCusto = apropriacao.CentroCusto;
                switch (situacaoRelatorio)
                {
                    case "PagamentosPendentesEhPagos":
                        apropriacaoClasseCCRelatorio.TipoClasseCC = "S";
                        apropriacaoClasseCCRelatorio.TipoCodigo = "PG";
                        apropriacaoClasseCCRelatorio.CorrentistaConta = apropriacao.TituloPagar.Id + " " + apropriacao.TituloPagar.Cliente.Nome;
                        apropriacaoClasseCCRelatorio.NomeCliente = apropriacao.TituloPagar.Cliente.Nome;
                        apropriacaoClasseCCRelatorio.SiglaDocumento = "";
                        if (apropriacao.TituloPagar.TipoDocumento != null)
                        {
                            apropriacaoClasseCCRelatorio.SiglaDocumento = apropriacao.TituloPagar.TipoDocumento.Sigla;
                        }
                        apropriacaoClasseCCRelatorio.Documento = apropriacao.TituloPagar.Documento;
                        apropriacaoClasseCCRelatorio.Identificacao = apropriacao.TituloPagar.Identificacao;
                        apropriacaoClasseCCRelatorio.DataVencimento = apropriacao.TituloPagar.DataVencimento;
                        apropriacaoClasseCCRelatorio.Situacao = apropriacao.TituloPagar.Situacao.ObterDescricao();
                        //apropriacaoClasseCCRelatorio.DataEmissao = apropriacao.TituloPagar.DataEmissaoDocumento;
                        apropriacaoClasseCCRelatorio.DataEmissao = apropriacao.TituloPagar.DataEmissao;
                        apropriacaoClasseCCRelatorio.DataPagamento = apropriacao.TituloPagar.DataPagamento;
                        apropriacaoClasseCCRelatorio.DataBaixa = apropriacao.TituloPagar.DataBaixa;
                        break;
                    case "RecebimentosPendentesEhRecebidos":
                        apropriacaoClasseCCRelatorio.TipoClasseCC = "E";
                        apropriacaoClasseCCRelatorio.TipoCodigo = "RC";
                        apropriacaoClasseCCRelatorio.CorrentistaConta = apropriacao.TituloReceber.Id + " " + apropriacao.TituloReceber.Cliente.Nome;
                        apropriacaoClasseCCRelatorio.NomeCliente = apropriacao.TituloReceber.Cliente.Nome;
                        apropriacaoClasseCCRelatorio.SiglaDocumento = "";
                        if (apropriacao.TituloReceber.TipoDocumento != null)
                        {
                            apropriacaoClasseCCRelatorio.SiglaDocumento = apropriacao.TituloReceber.TipoDocumento.Sigla;
                        }
                        apropriacaoClasseCCRelatorio.Documento = apropriacao.TituloReceber.Documento;
                        apropriacaoClasseCCRelatorio.Identificacao = apropriacao.TituloReceber.Identificacao;
                        apropriacaoClasseCCRelatorio.DataVencimento = apropriacao.TituloReceber.DataVencimento;
                        apropriacaoClasseCCRelatorio.Situacao = apropriacao.TituloReceber.Situacao.ObterDescricao();
                        apropriacaoClasseCCRelatorio.DataEmissao = apropriacao.TituloReceber.DataEmissaoDocumento;
                        apropriacaoClasseCCRelatorio.DataPagamento = apropriacao.TituloReceber.DataRecebimento;
                        apropriacaoClasseCCRelatorio.DataBaixa = apropriacao.TituloReceber.DataBaixa;
                        break;
                    case "MovimentoDebito":
                        apropriacaoClasseCCRelatorio.TipoClasseCC = "S";
                        apropriacaoClasseCCRelatorio.TipoCodigo = "MV";
                        apropriacaoClasseCCRelatorio.CorrentistaConta = apropriacao.Movimento.ContaCorrente.Banco.Id + " - " + apropriacao.Movimento.ContaCorrente.Agencia.AgenciaCodigo + "-" + apropriacao.Movimento.ContaCorrente.Agencia.DVAgencia + " / " + apropriacao.Movimento.ContaCorrente.ContaCodigo + "-" + apropriacao.Movimento.ContaCorrente.DVConta;
                        apropriacaoClasseCCRelatorio.NomeCliente = "";
                        apropriacaoClasseCCRelatorio.SiglaDocumento = "";
                        apropriacaoClasseCCRelatorio.Documento = apropriacao.Movimento.Documento;
                        apropriacaoClasseCCRelatorio.Identificacao = apropriacao.Movimento.Referencia;
                        apropriacaoClasseCCRelatorio.DataVencimento = apropriacao.Movimento.DataMovimento;
                        apropriacaoClasseCCRelatorio.Situacao = apropriacao.Movimento.Situacao;
                        apropriacaoClasseCCRelatorio.DataEmissao = apropriacao.Movimento.DataMovimento;
                        apropriacaoClasseCCRelatorio.DataPagamento = apropriacao.Movimento.DataMovimento;
                        apropriacaoClasseCCRelatorio.DataBaixa = apropriacao.Movimento.DataMovimento;
                        break;
                    case "MovimentoDebitoCaixa":
                        apropriacaoClasseCCRelatorio.TipoClasseCC = "S";
                        apropriacaoClasseCCRelatorio.TipoCodigo = "MV";
                        apropriacaoClasseCCRelatorio.CorrentistaConta = apropriacao.Movimento.Caixa.Descricao + "-" + apropriacao.Movimento.Caixa.Id;
                        apropriacaoClasseCCRelatorio.NomeCliente = "";
                        apropriacaoClasseCCRelatorio.SiglaDocumento = "";
                        apropriacaoClasseCCRelatorio.Documento = apropriacao.Movimento.Documento;
                        apropriacaoClasseCCRelatorio.Identificacao = apropriacao.Movimento.Referencia;
                        apropriacaoClasseCCRelatorio.DataVencimento = apropriacao.Movimento.DataMovimento;
                        apropriacaoClasseCCRelatorio.Situacao = apropriacao.Movimento.Situacao;
                        apropriacaoClasseCCRelatorio.DataEmissao = apropriacao.Movimento.DataMovimento;
                        apropriacaoClasseCCRelatorio.DataPagamento = apropriacao.Movimento.DataMovimento;
                        apropriacaoClasseCCRelatorio.DataBaixa = apropriacao.Movimento.DataMovimento;
                        break;
                    case "MovimentoCredito":
                        apropriacaoClasseCCRelatorio.TipoClasseCC = "E";
                        apropriacaoClasseCCRelatorio.TipoCodigo = "MV";
                        apropriacaoClasseCCRelatorio.CorrentistaConta = apropriacao.Movimento.ContaCorrente.Banco.Id + " - " + apropriacao.Movimento.ContaCorrente.Agencia.AgenciaCodigo + "-" + apropriacao.Movimento.ContaCorrente.Agencia.DVAgencia + " / " + apropriacao.Movimento.ContaCorrente.ContaCodigo + "-" + apropriacao.Movimento.ContaCorrente.DVConta;
                        apropriacaoClasseCCRelatorio.NomeCliente = "";
                        apropriacaoClasseCCRelatorio.SiglaDocumento = "";
                        apropriacaoClasseCCRelatorio.Documento = apropriacao.Movimento.Documento;
                        apropriacaoClasseCCRelatorio.Identificacao = apropriacao.Movimento.Referencia;
                        apropriacaoClasseCCRelatorio.DataVencimento = apropriacao.Movimento.DataMovimento;
                        apropriacaoClasseCCRelatorio.Situacao = apropriacao.Movimento.Situacao;
                        apropriacaoClasseCCRelatorio.DataEmissao = apropriacao.Movimento.DataMovimento;
                        apropriacaoClasseCCRelatorio.DataPagamento = apropriacao.Movimento.DataMovimento;
                        apropriacaoClasseCCRelatorio.DataBaixa = apropriacao.Movimento.DataMovimento;
                        break;
                    case "MovimentoCreditoCaixa":
                        apropriacaoClasseCCRelatorio.TipoClasseCC = "E";
                        apropriacaoClasseCCRelatorio.TipoCodigo = "MV";
                        apropriacaoClasseCCRelatorio.CorrentistaConta = apropriacao.Movimento.Caixa.Descricao + "-" + apropriacao.Movimento.Caixa.Id;
                        apropriacaoClasseCCRelatorio.NomeCliente = "";
                        apropriacaoClasseCCRelatorio.SiglaDocumento = "";
                        apropriacaoClasseCCRelatorio.Documento = apropriacao.Movimento.Documento;
                        apropriacaoClasseCCRelatorio.Identificacao = apropriacao.Movimento.Referencia;
                        apropriacaoClasseCCRelatorio.DataVencimento = apropriacao.Movimento.DataMovimento;
                        apropriacaoClasseCCRelatorio.Situacao = apropriacao.Movimento.Situacao;
                        apropriacaoClasseCCRelatorio.DataEmissao = apropriacao.Movimento.DataMovimento;
                        apropriacaoClasseCCRelatorio.DataPagamento = apropriacao.Movimento.DataMovimento;
                        apropriacaoClasseCCRelatorio.DataBaixa = apropriacao.Movimento.DataMovimento;
                        break;
                }

                listaApropriacaoClasseRelatorio.Add(apropriacaoClasseCCRelatorio);
            }

        }

        private void PopulaApropriacaoClasseRelatorio(List<TituloDetalheCredCob> listaTituloDetalheCredCob, List<ApropriacaoClasseCCRelatorio> listaApropriacaoClasseRelatorio, string situacaoRelatorio)
        {
            foreach (var tituloDetalheCredCob in listaTituloDetalheCredCob)
            {
                if (tituloDetalheCredCob.VerbaCobranca.Classe == null) continue;

                ApropriacaoClasseCCRelatorio apropriacaoClasseCCRelatorio = new ApropriacaoClasseCCRelatorio();

                if (tituloDetalheCredCob.Situacao == "P")
                {
                    apropriacaoClasseCCRelatorio.ValorApropriado = tituloDetalheCredCob.ValorDevido;
                }
                if (tituloDetalheCredCob.Situacao == "Q")
                {
                    apropriacaoClasseCCRelatorio.ValorApropriado = tituloDetalheCredCob.ValorBaixa.HasValue ? tituloDetalheCredCob.ValorBaixa.Value : 0;
                }

                apropriacaoClasseCCRelatorio.Classe = tituloDetalheCredCob.VerbaCobranca.Classe;
                apropriacaoClasseCCRelatorio.CentroCusto = tituloDetalheCredCob.Contrato.Unidade.Bloco.CentroCusto;
                if (situacaoRelatorio == "CreditoCobranca")
                {
                    apropriacaoClasseCCRelatorio.TipoClasseCC = "E";
                    apropriacaoClasseCCRelatorio.TipoCodigo = "CC";
                    string nomeCliente = tituloDetalheCredCob.Contrato.ListaVendaParticipante.Where(l => l.TipoParticipanteId.Value == 1).FirstOrDefault().Cliente.Nome;
                    apropriacaoClasseCCRelatorio.CorrentistaConta = tituloDetalheCredCob.Id + " " + nomeCliente;
                    apropriacaoClasseCCRelatorio.NomeCliente = nomeCliente;
                    apropriacaoClasseCCRelatorio.SiglaDocumento = tituloDetalheCredCob.Id.Value.ToString();
                    apropriacaoClasseCCRelatorio.Documento = "CREDCOB";
                    apropriacaoClasseCCRelatorio.Identificacao = "Crédito e Cobrança";
                    apropriacaoClasseCCRelatorio.DataVencimento = tituloDetalheCredCob.DataVencimento;
                    apropriacaoClasseCCRelatorio.Situacao = tituloDetalheCredCob.Situacao;
                    apropriacaoClasseCCRelatorio.DataEmissao = tituloDetalheCredCob.DataVencimento;
                    apropriacaoClasseCCRelatorio.DataPagamento = tituloDetalheCredCob.DataPagamento;
                    apropriacaoClasseCCRelatorio.DataBaixa = tituloDetalheCredCob.DataPagamento;
                }

                listaApropriacaoClasseRelatorio.Add(apropriacaoClasseCCRelatorio);
            }
        }

        private void PopulaApropriacaoClasseRelatorio(List<TituloMovimento> listaTituloMovimento, List<ApropriacaoClasseCCRelatorio> listaApropriacaoClasseRelatorio, string situacaoRelatorio)
        {

            foreach (var tituloMovimento in listaTituloMovimento)
            {
                if (tituloMovimento.TituloCredCob.VerbaCobranca.Classe == null) continue;

                ApropriacaoClasseCCRelatorio apropriacaoClasseCCRelatorio = new ApropriacaoClasseCCRelatorio();

                apropriacaoClasseCCRelatorio.ValorApropriado = tituloMovimento.TituloCredCob.ValorBaixa.HasValue ? tituloMovimento.TituloCredCob.ValorBaixa.Value : 0;
                apropriacaoClasseCCRelatorio.Classe = tituloMovimento.TituloCredCob.VerbaCobranca.Classe;
                apropriacaoClasseCCRelatorio.CentroCusto = tituloMovimento.TituloCredCob.Contrato.Unidade.Bloco.CentroCusto;
                if (situacaoRelatorio == "CreditoCobrancaTituloMovimento")
                {
                    apropriacaoClasseCCRelatorio.TipoClasseCC = "E";
                    apropriacaoClasseCCRelatorio.TipoCodigo = "CC";
                    apropriacaoClasseCCRelatorio.CorrentistaConta = tituloMovimento.MovimentoFinanceiro.ContaCorrente.Banco.Id + "-" + tituloMovimento.MovimentoFinanceiro.ContaCorrente.Agencia.AgenciaCodigo + "/" + tituloMovimento.MovimentoFinanceiro.ContaCorrente.ContaCodigo + "-" + tituloMovimento.MovimentoFinanceiro.ContaCorrente.DVConta;
                    apropriacaoClasseCCRelatorio.NomeCliente = "";
                    apropriacaoClasseCCRelatorio.SiglaDocumento = "";
                    apropriacaoClasseCCRelatorio.Documento = tituloMovimento.MovimentoFinanceiro.Documento;
                    apropriacaoClasseCCRelatorio.Identificacao = tituloMovimento.MovimentoFinanceiro.Referencia;
                    apropriacaoClasseCCRelatorio.DataVencimento = tituloMovimento.MovimentoFinanceiro.DataConferencia.Value;
                    apropriacaoClasseCCRelatorio.Situacao = tituloMovimento.MovimentoFinanceiro.Situacao;
                    apropriacaoClasseCCRelatorio.DataEmissao = tituloMovimento.MovimentoFinanceiro.DataConferencia.Value;
                    apropriacaoClasseCCRelatorio.DataPagamento = tituloMovimento.MovimentoFinanceiro.DataConferencia.Value;
                    apropriacaoClasseCCRelatorio.DataBaixa = tituloMovimento.MovimentoFinanceiro.DataConferencia.Value;
                }

                listaApropriacaoClasseRelatorio.Add(apropriacaoClasseCCRelatorio);
            }
        }

        private List<ApropriacaoClasseCCRelatorio> AgrupaRelApropriacaoPorClasseRelatorio(int opcaoRelatorio, List<ApropriacaoClasseCCRelatorio> listaApropriacaoClasseRelatorio)
        {
            List<ApropriacaoClasseCCRelatorio> listaAgrupada = new List<ApropriacaoClasseCCRelatorio>();

            switch(opcaoRelatorio)
            {
                case 1:
                    listaAgrupada = AgrupaRelApropriacaoPorClasseRelatorioSintetico(listaApropriacaoClasseRelatorio);
                    break;
                case 2:
                    listaAgrupada = AgrupaRelApropriacaoPorClasseRelatorioAnalitico(listaApropriacaoClasseRelatorio);
                    break;
                case 3:
                    listaAgrupada = AgrupaRelApropriacaoPorClasseRelatorioAnaliticoDetalhado(listaApropriacaoClasseRelatorio);
                    break;
                case 4:
                    listaAgrupada = AgrupaRelApropriacaoPorClasseRelatorioAnaliticoDetalhadoFornecedor(listaApropriacaoClasseRelatorio);
                    break;
            }

            return listaAgrupada;
        }

        private List<ApropriacaoClasseCCRelatorio> AgrupaRelApropriacaoPorClasseRelatorioSintetico(List<ApropriacaoClasseCCRelatorio> listaApropriacaoClasseRelatorio)
        {
            List<ApropriacaoClasseCCRelatorio> listaAgrupada = new List<ApropriacaoClasseCCRelatorio>();

            var novaLista = listaApropriacaoClasseRelatorio.OrderBy(l => l.TipoClasseCC).ThenBy(l => l.Classe.Codigo)
                                                            .GroupBy(l => new { l.TipoClasseCC, l.Classe })
                                                            .Select(r => new
                                                            {
                                                                TipoClasseCC = r.Key.TipoClasseCC,
                                                                Classe = r.Key.Classe,
                                                                Lista = r.ToList<ApropriacaoClasseCCRelatorio>()
                                                            });

            foreach (var groupApropriacaoClasse in novaLista)
            {
                ApropriacaoClasseCCRelatorio registro = new ApropriacaoClasseCCRelatorio();
                decimal valor = 0;

                registro.TipoClasseCC = groupApropriacaoClasse.TipoClasseCC;
                registro.Classe = groupApropriacaoClasse.Classe;
                valor = groupApropriacaoClasse.Lista.Sum(l => l.ValorApropriado);
                registro.ValorApropriado = valor;

                listaAgrupada.Add(registro);

                AdicionaRegistroRelatorioSinteticoPai(registro, listaAgrupada, valor);
            }

            //remove todos os registros que possuem classes com filhos
            listaAgrupada.RemoveAll(l => l.Classe.CodigoPai != null);

            return listaAgrupada;

        }

        private List<ApropriacaoClasseCCRelatorio> AgrupaRelApropriacaoPorClasseRelatorioAnalitico(List<ApropriacaoClasseCCRelatorio> listaApropriacaoClasseRelatorio)
        {
            List<ApropriacaoClasseCCRelatorio> listaAgrupada = new List<ApropriacaoClasseCCRelatorio>();

            decimal valor = 0;

            foreach (var groupApropriacaoClasse in listaApropriacaoClasseRelatorio.OrderBy(l => l.Classe.Codigo).ThenBy(l => l.TipoClasseCC).GroupBy(l => new { l.Classe, l.TipoClasseCC }))
            {
                ApropriacaoClasseCCRelatorio registro = new ApropriacaoClasseCCRelatorio();
                registro.TipoClasseCC = groupApropriacaoClasse.Key.TipoClasseCC;
                registro.Classe = groupApropriacaoClasse.Key.Classe;
                valor = groupApropriacaoClasse.Sum(l => l.ValorApropriado);
                registro.ValorApropriado = valor;
                registro.TipoCodigo = "";

                listaAgrupada.Add(registro);

                AdicionaRegistroRelatorioAnaliticoPai(registro, listaAgrupada, valor);
            }

            return listaAgrupada;
        }

        private List<ApropriacaoClasseCCRelatorio> AgrupaRelApropriacaoPorClasseRelatorioAnaliticoDetalhado(List<ApropriacaoClasseCCRelatorio> listaApropriacaoClasseRelatorio)
        {
            List<ApropriacaoClasseCCRelatorio> listaAgrupada = new List<ApropriacaoClasseCCRelatorio>();

            decimal valor = 0;

            foreach (ApropriacaoClasseCCRelatorio apropriacaoClasse in listaApropriacaoClasseRelatorio.OrderBy(l => l.Classe.Codigo).ThenBy(l => l.TipoClasseCC))
            {
                ApropriacaoClasseCCRelatorio registro = new ApropriacaoClasseCCRelatorio();
                registro.TipoClasseCC = apropriacaoClasse.TipoClasseCC;
                registro.Classe = apropriacaoClasse.Classe;
                valor = apropriacaoClasse.ValorApropriado;
                registro.ValorApropriado = valor;
                registro.CorrentistaConta = apropriacaoClasse.CorrentistaConta;
                registro.SiglaDocumento = apropriacaoClasse.SiglaDocumento;
                registro.Documento = apropriacaoClasse.Documento;
                registro.Identificacao = apropriacaoClasse.Identificacao;

                listaAgrupada.Add(registro);

                AdicionaRegistroRelatorioAnaliticoDetalhadoPai(registro, listaAgrupada, valor);
            }

            return listaAgrupada;
        }

        private List<ApropriacaoClasseCCRelatorio> AgrupaRelApropriacaoPorClasseRelatorioAnaliticoDetalhadoFornecedor(List<ApropriacaoClasseCCRelatorio> listaApropriacaoClasseRelatorio)
        {
            List<ApropriacaoClasseCCRelatorio> listaAgrupada = new List<ApropriacaoClasseCCRelatorio>();

            var novaLista = listaApropriacaoClasseRelatorio.OrderBy(l => l.TipoClasseCC).ThenBy(l => l.Classe.Codigo).ThenBy(l => l.NomeCliente)
                                                            .GroupBy(l => new { l.TipoClasseCC, l.Classe, l.NomeCliente })
                                                            .Select(r => new { TipoClasseCC = r.Key.TipoClasseCC,
                                                                               Classe = r.Key.Classe,
                                                                               NomeCliente = r.Key.NomeCliente,
                                                                               Lista = r.ToList<ApropriacaoClasseCCRelatorio>() });

            foreach (var groupApropriacaoClasse in novaLista)
            {
                ApropriacaoClasseCCRelatorio registro = new ApropriacaoClasseCCRelatorio();
                decimal valor = 0;

                if (!string.IsNullOrEmpty(groupApropriacaoClasse.NomeCliente))
                {
                    registro.TipoClasseCC = groupApropriacaoClasse.TipoClasseCC;
                    registro.Classe = groupApropriacaoClasse.Classe;
                    valor = groupApropriacaoClasse.Lista.Sum(l => l.ValorApropriado);
                    registro.ValorApropriado = valor;
                    registro.CorrentistaConta = "";
                    registro.SiglaDocumento = "";
                    registro.Documento = "";
                    registro.Identificacao = "";
                    registro.NomeCliente = groupApropriacaoClasse.NomeCliente;

                    listaAgrupada.Add(registro);

                    AdicionaRegistroRelatorioAnaliticoDetalhadoFornecedorPai(registro, listaAgrupada, valor);
                }
                else 
                {
                    foreach (ApropriacaoClasseCCRelatorio item in groupApropriacaoClasse.Lista)
                    {
                        registro = new ApropriacaoClasseCCRelatorio();

                        registro.TipoClasseCC = item.TipoClasseCC;
                        registro.Classe = item.Classe;
                        valor = item.ValorApropriado;
                        registro.ValorApropriado = valor;
                        registro.CorrentistaConta = item.CorrentistaConta;
                        registro.SiglaDocumento = item.SiglaDocumento;
                        registro.Documento = item.Documento;
                        registro.Identificacao = item.Identificacao;
                        registro.NomeCliente = item.NomeCliente;

                        listaAgrupada.Add(registro);

                        AdicionaRegistroRelatorioAnaliticoDetalhadoFornecedorPai(registro, listaAgrupada, valor);
                    } 
                }
            }

            return listaAgrupada;
        }

        private void AdicionaRegistroRelatorioSinteticoPai(ApropriacaoClasseCCRelatorio apropriacaoClasseRelatorioFilho, List<ApropriacaoClasseCCRelatorio> listaApropriacaoClasseRelatorio, decimal valor)
        {
            if (apropriacaoClasseRelatorioFilho.Classe.ClassePai != null)
            {
                ApropriacaoClasseCCRelatorio apropriacaoClasseRelatorio = new ApropriacaoClasseCCRelatorio();
                bool recuperouApropriacao = false;
                if (listaApropriacaoClasseRelatorio.Any(l => ((l.Classe.Codigo == apropriacaoClasseRelatorioFilho.Classe.CodigoPai) && l.TipoClasseCC == apropriacaoClasseRelatorioFilho.TipoClasseCC)))
                {
                    apropriacaoClasseRelatorio = listaApropriacaoClasseRelatorio.Where(l => l.Classe.Codigo == apropriacaoClasseRelatorioFilho.Classe.CodigoPai && l.TipoClasseCC == apropriacaoClasseRelatorioFilho.TipoClasseCC).FirstOrDefault();
                    recuperouApropriacao = true;
                }
                else
                {
                    apropriacaoClasseRelatorio.TipoClasseCC = apropriacaoClasseRelatorioFilho.TipoClasseCC;
                    apropriacaoClasseRelatorio.Classe = apropriacaoClasseRelatorioFilho.Classe.ClassePai;
                    apropriacaoClasseRelatorio.ValorApropriado = 0;
                }

                apropriacaoClasseRelatorio.ValorApropriado = apropriacaoClasseRelatorio.ValorApropriado + valor;

                if (!recuperouApropriacao)
                {
                    listaApropriacaoClasseRelatorio.Add(apropriacaoClasseRelatorio);
                }

                if (apropriacaoClasseRelatorio.Classe.ClassePai != null)
                {
                    AdicionaRegistroRelatorioSinteticoPai(apropriacaoClasseRelatorio, listaApropriacaoClasseRelatorio, valor);
                }
            }
        }

        private void AdicionaRegistroRelatorioAnaliticoPai(ApropriacaoClasseCCRelatorio apropriacaoClasseRelatorioFilho, List<ApropriacaoClasseCCRelatorio> listaApropriacaoClasseRelatorio, decimal valor)
        {
            if (apropriacaoClasseRelatorioFilho.Classe.ClassePai != null)
            {
                ApropriacaoClasseCCRelatorio apropriacaoClasseRelatorio = new ApropriacaoClasseCCRelatorio();
                bool recuperouApropriacao = false;
                if (listaApropriacaoClasseRelatorio.Any(l => ((l.Classe.Codigo == apropriacaoClasseRelatorioFilho.Classe.CodigoPai) && l.TipoClasseCC == apropriacaoClasseRelatorioFilho.TipoClasseCC)))
                {
                    apropriacaoClasseRelatorio = listaApropriacaoClasseRelatorio.Where(l => l.Classe.Codigo == apropriacaoClasseRelatorioFilho.Classe.CodigoPai && l.TipoClasseCC == apropriacaoClasseRelatorioFilho.TipoClasseCC).FirstOrDefault();
                    recuperouApropriacao = true;
                }
                else
                {
                    apropriacaoClasseRelatorio.TipoClasseCC = apropriacaoClasseRelatorioFilho.TipoClasseCC;
                    apropriacaoClasseRelatorio.Classe = apropriacaoClasseRelatorioFilho.Classe.ClassePai;
                    apropriacaoClasseRelatorio.ValorApropriado = 0;
                    apropriacaoClasseRelatorio.TipoCodigo = "M";
                }

                apropriacaoClasseRelatorio.ValorApropriado = apropriacaoClasseRelatorio.ValorApropriado + valor;

                if (!recuperouApropriacao)
                {
                    listaApropriacaoClasseRelatorio.Add(apropriacaoClasseRelatorio);
                }

                if (apropriacaoClasseRelatorio.Classe.ClassePai != null)
                {
                    AdicionaRegistroRelatorioAnaliticoPai(apropriacaoClasseRelatorio, listaApropriacaoClasseRelatorio, valor);
                }
            }
        }

        private void AdicionaRegistroRelatorioAnaliticoDetalhadoPai(ApropriacaoClasseCCRelatorio apropriacaoClasseRelatorioFilho, List<ApropriacaoClasseCCRelatorio> listaApropriacaoClasseRelatorio, decimal valor)
        {
            if (apropriacaoClasseRelatorioFilho.Classe.ClassePai != null)
            {
                ApropriacaoClasseCCRelatorio apropriacaoClasseRelatorio = new ApropriacaoClasseCCRelatorio();
                bool recuperouApropriacao = false;
                if (listaApropriacaoClasseRelatorio.Any(l => ((l.Classe.Codigo == apropriacaoClasseRelatorioFilho.Classe.CodigoPai) && l.TipoClasseCC == apropriacaoClasseRelatorioFilho.TipoClasseCC)))
                {
                    apropriacaoClasseRelatorio = listaApropriacaoClasseRelatorio.Where(l => l.Classe.Codigo == apropriacaoClasseRelatorioFilho.Classe.CodigoPai && l.TipoClasseCC == apropriacaoClasseRelatorioFilho.TipoClasseCC).FirstOrDefault();
                    recuperouApropriacao = true;
                }
                else
                {
                    apropriacaoClasseRelatorio.TipoClasseCC = apropriacaoClasseRelatorioFilho.TipoClasseCC;
                    apropriacaoClasseRelatorio.Classe = apropriacaoClasseRelatorioFilho.Classe.ClassePai;
                    apropriacaoClasseRelatorio.ValorApropriado = 0;
                    apropriacaoClasseRelatorio.CorrentistaConta = "";
                    apropriacaoClasseRelatorio.SiglaDocumento = "";
                    apropriacaoClasseRelatorio.Documento = "";
                    apropriacaoClasseRelatorio.Identificacao = "";
                }

                apropriacaoClasseRelatorio.ValorApropriado = apropriacaoClasseRelatorio.ValorApropriado + valor;

                if (!recuperouApropriacao)
                {
                    listaApropriacaoClasseRelatorio.Add(apropriacaoClasseRelatorio);
                }

                if (apropriacaoClasseRelatorio.Classe.ClassePai != null)
                {
                    AdicionaRegistroRelatorioAnaliticoDetalhadoPai(apropriacaoClasseRelatorio, listaApropriacaoClasseRelatorio, valor);
                }
            }
        }

        private void AdicionaRegistroRelatorioAnaliticoDetalhadoFornecedorPai(ApropriacaoClasseCCRelatorio apropriacaoClasseRelatorioFilho, List<ApropriacaoClasseCCRelatorio> listaApropriacaoClasseRelatorio, decimal valor)
        {
            if (apropriacaoClasseRelatorioFilho.Classe.ClassePai != null)
            {
                ApropriacaoClasseCCRelatorio apropriacaoClasseRelatorio = new ApropriacaoClasseCCRelatorio();
                bool recuperouApropriacao = false;
                if (listaApropriacaoClasseRelatorio.Any(l => ((l.Classe.Codigo == apropriacaoClasseRelatorioFilho.Classe.CodigoPai) && l.TipoClasseCC == apropriacaoClasseRelatorioFilho.TipoClasseCC)))
                {
                    apropriacaoClasseRelatorio = listaApropriacaoClasseRelatorio.Where(l => l.Classe.Codigo == apropriacaoClasseRelatorioFilho.Classe.CodigoPai && l.TipoClasseCC == apropriacaoClasseRelatorioFilho.TipoClasseCC).FirstOrDefault();
                    recuperouApropriacao = true;
                }
                else
                {
                    apropriacaoClasseRelatorio.TipoClasseCC = apropriacaoClasseRelatorioFilho.TipoClasseCC;
                    apropriacaoClasseRelatorio.Classe = apropriacaoClasseRelatorioFilho.Classe.ClassePai;
                    apropriacaoClasseRelatorio.ValorApropriado = 0;
                    apropriacaoClasseRelatorio.CorrentistaConta = "";
                    apropriacaoClasseRelatorio.SiglaDocumento = "";
                    apropriacaoClasseRelatorio.Documento = "";
                    apropriacaoClasseRelatorio.Identificacao = "";
                    apropriacaoClasseRelatorio.NomeCliente = "";
                }

                apropriacaoClasseRelatorio.ValorApropriado = apropriacaoClasseRelatorio.ValorApropriado + valor;

                if (!recuperouApropriacao)
                {
                    listaApropriacaoClasseRelatorio.Add(apropriacaoClasseRelatorio);
                }

                if (apropriacaoClasseRelatorio.Classe.ClassePai != null)
                {
                    AdicionaRegistroRelatorioAnaliticoDetalhadoFornecedorPai(apropriacaoClasseRelatorio, listaApropriacaoClasseRelatorio, valor);
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

            specification &= ApropriacaoSpecification.EhTipoTituloDiferenteDeTituloPagarPai();

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

            if (filtro.ListaClasseDespesa.Count > 0)
            {
                string[] arrayCodigoClasse = PopulaArrayComCodigosDeClassesSelecionadas(filtro.ListaClasseDespesa);

                if (arrayCodigoClasse.Length > 0)
                {
                    specification &= ApropriacaoSpecification.SaoClassesExistentes(arrayCodigoClasse);
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

            specification &= ApropriacaoSpecification.EhTipoTituloDiferenteDeTituloReceberPai();

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

            if (filtro.ListaClasseReceita.Count > 0)
            {

                string[] arrayCodigoClasse = PopulaArrayComCodigosDeClassesSelecionadas(filtro.ListaClasseReceita);

                if (arrayCodigoClasse.Length > 0)
                {
                    specification &= ApropriacaoSpecification.SaoClassesExistentes(arrayCodigoClasse);
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

        private DataTable RelApropriacaoPorClasseToDataTable(int opcaoRelatorio, List<ApropriacaoClasseCCRelatorio> listaApropriacaoClasseRelatorio)
        {
            DataTable dta = new DataTable();
            DataColumn tipoClasseCC = new DataColumn("tipoClasseCC");
            DataColumn codigoClasse = new DataColumn("codigoClasse");
            DataColumn descricaoClasse = new DataColumn("descricaoClasse");
            DataColumn tipoCodigo = new DataColumn("tipoCodigo");
            DataColumn correntistaConta = new DataColumn("correntistaConta");
            DataColumn siglaTipoDocumento = new DataColumn("siglaTipoDocumento");
            DataColumn documento = new DataColumn("documento");
            DataColumn identificacao = new DataColumn("identificacao");
            DataColumn valorApropriado = new DataColumn("valorApropriado", System.Type.GetType("System.Decimal"));
            DataColumn nomeCliente = new DataColumn("nomeCliente");
            DataColumn girErro = new DataColumn("girErro");

            dta.Columns.Add(tipoClasseCC);
            dta.Columns.Add(codigoClasse);
            dta.Columns.Add(descricaoClasse);
            switch (opcaoRelatorio)
            {
                case 1:
                    dta.Columns.Add(tipoCodigo);
                    break;
                case 2:
                    dta.Columns.Add(tipoCodigo);
                    break;
                case 3:
                    dta.Columns.Add(correntistaConta);
                    dta.Columns.Add(siglaTipoDocumento);
                    dta.Columns.Add(documento);
                    dta.Columns.Add(identificacao);
                    break;
                case 4:
                    dta.Columns.Add(correntistaConta);
                    dta.Columns.Add(siglaTipoDocumento);
                    dta.Columns.Add(documento);
                    dta.Columns.Add(identificacao);
                    dta.Columns.Add(nomeCliente);
                    break;

            }
            dta.Columns.Add(valorApropriado);
            dta.Columns.Add(girErro);

            foreach (ApropriacaoClasseCCRelatorio apropriacaoClasseCCRelatorio in listaApropriacaoClasseRelatorio.OrderBy(l => l.Classe.Codigo))
            {
                DataRow row = dta.NewRow();
                row[tipoClasseCC] = apropriacaoClasseCCRelatorio.TipoClasseCC;
                row[codigoClasse] = apropriacaoClasseCCRelatorio.Classe.Codigo;
                row[descricaoClasse] = apropriacaoClasseCCRelatorio.Classe.Descricao;
                row[valorApropriado] = apropriacaoClasseCCRelatorio.ValorApropriado;
                switch (opcaoRelatorio)
                {
                    case 2:
                        row[tipoCodigo] = apropriacaoClasseCCRelatorio.TipoCodigo;
                        break;
                    case 3:
                        row[correntistaConta] = apropriacaoClasseCCRelatorio.CorrentistaConta;
                        row[siglaTipoDocumento] = apropriacaoClasseCCRelatorio.SiglaDocumento;
                        row[documento] = apropriacaoClasseCCRelatorio.Documento;
                        row[identificacao] = apropriacaoClasseCCRelatorio.Identificacao;
                        break;
                    case 4:
                        row[correntistaConta] = apropriacaoClasseCCRelatorio.CorrentistaConta;
                        row[siglaTipoDocumento] = apropriacaoClasseCCRelatorio.SiglaDocumento;
                        row[documento] = apropriacaoClasseCCRelatorio.Documento;
                        row[identificacao] = apropriacaoClasseCCRelatorio.Identificacao;
                        row[nomeCliente] = apropriacaoClasseCCRelatorio.NomeCliente;
                        break;
                }

                row[girErro] = "";
                dta.Rows.Add(row);
            }

            return dta;
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

        private bool ValidarFiltroRelApropriacaoPorClasse(RelApropriacaoPorClasseFiltro filtro)
        {
            if (filtro.DataInicial.HasValue && filtro.DataFinal.HasValue) 
            {
                if (filtro.DataInicial.Value > filtro.DataFinal.Value)
                {
                    messageQueue.Add("Data final menor que a data inicial", TypeMessage.Error);
                    return false;
                }
            }

            return true;
        }

        #endregion

    }
}
