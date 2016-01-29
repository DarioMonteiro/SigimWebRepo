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

        #endregion

        #region Construtor

        public ApropriacaoAppService(IUsuarioAppService usuarioAppService,
                                     IParametrosFinanceiroRepository parametrosFinanceiroRepository, 
                                     ICentroCustoRepository centroCustoRepository,
                                     IClasseRepository classeRepository,
                                     IApropriacaoRepository apropriacaoRepository,
                                     ITituloCredCobAppService tituloCredCobAppService,
                                     ITituloCredCobRepository tituloCredCobRepository,
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

        public FileDownloadDTO ExportarRelApropriacaoPorClasse(RelApropriacaoPorClasseFiltro filtro,
                                                               int? usuarioId,
                                                               FormatoExportacaoArquivo formato)
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
                                                              l => l.VerbaCobranca.Classe).To<List<TituloCredCob>>();
                }
                //if (!filtro.EhSituacaoAReceberRecebido || filtro.EhSituacaoAReceberQuitado)
                //{

                //}



            }

            relApropriacaoPorClasseSintetico objRel = new relApropriacaoPorClasseSintetico();

            DataTable dtaRelatorio = CriaDataTableApropriacaoClasseCCRelatorio();

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

        #endregion

        #region "Métodos privados"

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
            if (filtro.TipoPesquisa.Value == (int)TipoPesquisaRelatorioApropriacaoPorClasse.PorCompetencia)
            {
                tipoPesquisa = "V";
            }
            if (filtro.TipoPesquisa.Value == (int)TipoPesquisaRelatorioApropriacaoPorClasse.PorEmissaoDocumento)
            {
                tipoPesquisa = "E";
            }

            specification &= ApropriacaoSpecification.DataPeriodoTituloPagarMaiorOuIgualPendentesRelApropriacaoPorClasse(tipoPesquisa, filtro.DataInicial);
            specification &= ApropriacaoSpecification.DataPeriodoTituloPagarMenorOuIgualPendentesRelApropriacaoPorClasse(tipoPesquisa, filtro.DataFinal);

            if (filtro.OpcoesRelatorio.HasValue)
            {
                if (filtro.OpcoesRelatorio.Value != (int)OpcoesRelatorioApropriacaoPorClasse.Sintetico)
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
            if (filtro.TipoPesquisa.Value == (int)TipoPesquisaRelatorioApropriacaoPorClasse.PorCompetencia)
            {
                tipoPesquisa = "V";
            }
            if (filtro.TipoPesquisa.Value == (int)TipoPesquisaRelatorioApropriacaoPorClasse.PorEmissaoDocumento)
            {
                tipoPesquisa = "E";
            }

            specification &= ApropriacaoSpecification.DataPeriodoTituloReceberMaiorOuIgualPendentesRelApropriacaoPorClasse(tipoPesquisa, filtro.DataInicial);
            specification &= ApropriacaoSpecification.DataPeriodoTituloReceberMenorOuIgualPendentesRelApropriacaoPorClasse(tipoPesquisa, filtro.DataFinal);

            if (filtro.OpcoesRelatorio.HasValue)
            {
                if (filtro.OpcoesRelatorio.Value != (int)OpcoesRelatorioApropriacaoPorClasse.Sintetico)
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
            if (filtro.TipoPesquisa.Value == (int)TipoPesquisaRelatorioApropriacaoPorClasse.PorCompetencia)
            {
                tipoPesquisa = "V";
            }
            if (filtro.TipoPesquisa.Value == (int)TipoPesquisaRelatorioApropriacaoPorClasse.PorEmissaoDocumento)
            {
                tipoPesquisa = "E";
            }

            specification &= ApropriacaoSpecification.DataPeriodoTituloPagarMaiorOuIgualPagosRelApropriacaoPorClasse(tipoPesquisa, filtro.DataInicial, filtro.EhSituacaoAPagarEmitido ,filtro.EhSituacaoAPagarPago ,filtro.EhSituacaoAPagarBaixado);
            specification &= ApropriacaoSpecification.DataPeriodoTituloPagarMenorOuIgualPagosRelApropriacaoPorClasse(tipoPesquisa, filtro.DataFinal, filtro.EhSituacaoAPagarEmitido ,filtro.EhSituacaoAPagarPago ,filtro.EhSituacaoAPagarBaixado);

            if (filtro.OpcoesRelatorio.HasValue)
            {
                if (filtro.OpcoesRelatorio.Value != (int)OpcoesRelatorioApropriacaoPorClasse.Sintetico)
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
            if (filtro.TipoPesquisa.Value == (int)TipoPesquisaRelatorioApropriacaoPorClasse.PorCompetencia)
            {
                tipoPesquisa = "V";
            }
            if (filtro.TipoPesquisa.Value == (int)TipoPesquisaRelatorioApropriacaoPorClasse.PorEmissaoDocumento)
            {
                tipoPesquisa = "E";
            }

            specification &= ApropriacaoSpecification.DataPeriodoTituloReceberMaiorOuIgualRecebidosRelApropriacaoPorClasse(tipoPesquisa, filtro.DataInicial, filtro.EhSituacaoAReceberPreDatado, filtro.EhSituacaoAReceberRecebido, filtro.EhSituacaoAReceberQuitado);
            specification &= ApropriacaoSpecification.DataPeriodoTituloReceberMenorOuIgualRecebidosRelApropriacaoPorClasse(tipoPesquisa, filtro.DataFinal, filtro.EhSituacaoAReceberPreDatado, filtro.EhSituacaoAReceberRecebido, filtro.EhSituacaoAReceberQuitado);

            if (filtro.OpcoesRelatorio.HasValue)
            {
                if (filtro.OpcoesRelatorio.Value != (int)OpcoesRelatorioApropriacaoPorClasse.Sintetico)
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

            specification &= ApropriacaoSpecification.DataPeriodoMovimentoMaiorOuIgualRelApropriacaoPorClasse(filtro.DataInicial);
            specification &= ApropriacaoSpecification.DataPeriodoMovimentoMenorOuIgualRelApropriacaoPorClasse(filtro.DataFinal);

            if (filtro.OpcoesRelatorio.HasValue)
            {
                if (filtro.OpcoesRelatorio.Value != (int)OpcoesRelatorioApropriacaoPorClasse.Sintetico)
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
            if (filtro.TipoPesquisa.Value == (int)TipoPesquisaRelatorioApropriacaoPorClasse.PorCompetencia)
            {
                strTipoData = "por competência";
            }
            if (filtro.TipoPesquisa.Value == (int)TipoPesquisaRelatorioApropriacaoPorClasse.PorEmissaoDocumento)
            {
                strTipoData = "por emissão de documento";
            }

            return strTipoData;
        }


        #endregion

    }
}
