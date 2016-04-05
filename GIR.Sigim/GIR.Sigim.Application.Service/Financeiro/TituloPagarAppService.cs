using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.Filtros.Financeiro;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Application.Service.Admin;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Domain.Specification.Financeiro;
using GIR.Sigim.Application.Enums;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Reports.Financeiro;
using CrystalDecisions.Shared;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public class TituloPagarAppService : BaseAppService, ITituloPagarAppService
    {
        #region Declaração

        private ITituloPagarRepository tituloPagarRepository;
        private IUsuarioAppService usuarioAppService;
        private IModuloSigimAppService moduloSigimAppService;
        private IParametrosFinanceiroRepository parametrosFinanceiroRepository;
        private ICentroCustoRepository centroCustoRepository;

        #endregion

        #region Construtor

        public TituloPagarAppService(ITituloPagarRepository tituloPagarRepository, 
                                     IUsuarioAppService usuarioAppService,
                                     IModuloSigimAppService moduloSigimAppService,
                                     IParametrosFinanceiroRepository parametrosFinanceiroRepository,
                                     ICentroCustoRepository centroCustoRepository,
                                     MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.tituloPagarRepository = tituloPagarRepository;
            this.usuarioAppService = usuarioAppService;
            this.moduloSigimAppService = moduloSigimAppService;
            this.parametrosFinanceiroRepository = parametrosFinanceiroRepository;
            this.centroCustoRepository = centroCustoRepository;
        }

        #endregion

        #region Métodos ITituloPagarAppService

        public bool ExisteNumeroDocumento(Nullable<DateTime> DataEmissao, Nullable<DateTime> DataVencimento, string NumeroDocumento, int? ClienteId)
        {
            bool existe = false;

            if (!string.IsNullOrEmpty(NumeroDocumento) && (ClienteId.HasValue) && (DataEmissao.HasValue))
            {
                List<TituloPagar> listaTituloPagar;
                string numeroNotaFiscal = RetiraZerosIniciaisNumeroDocumento(NumeroDocumento);

                listaTituloPagar =
                    tituloPagarRepository.ListarPeloFiltro(l => l.ClienteId == ClienteId &&
                                                            l.Documento.EndsWith(NumeroDocumento) &&
                                                            l.DataEmissaoDocumento.Year == DataEmissao.Value.Year &&
                                                            ((DataVencimento == null) || ((DataVencimento != null) && (l.DataVencimento == DataVencimento)))).ToList<TituloPagar>();
                if (listaTituloPagar.Count() > 0)
                {
                    string numeroDeZerosIniciais;

                    foreach (var item in listaTituloPagar)
                    {
                        if ((item.Situacao != SituacaoTituloPagar.Cancelado) && (item.TipoTitulo != TipoTitulo.Pai))
                        {
                            var quantidadeDeZerosIniciais = item.Documento.Length - numeroNotaFiscal.Length;
                            numeroDeZerosIniciais = item.Documento.Substring(0, quantidadeDeZerosIniciais);
                            if (string.IsNullOrEmpty(numeroDeZerosIniciais))
                            {
                                numeroDeZerosIniciais = "0";
                            }
                            int resultado;
                            if (int.TryParse(numeroDeZerosIniciais, out resultado))
                            {
                                if (Convert.ToInt32(resultado) == 0)
                                {
                                    existe = true;
                                    break;
                                }
                            }

                        }
                    }
                }
            }

            return existe;
        }

        public bool EhPermitidoImprimirRelContasPagarTitulo()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.RelatorioContasAPagarTitulosImprimir);
        }

        public List<RelContasPagarTitulosDTO> ListarPeloFiltroRelContasPagarTitulos(RelContasPagarTitulosFiltro filtro, 
                                                                                    int? usuarioId, 
                                                                                    out int totalRegistros, 
                                                                                    out decimal totalValorTitulo, 
                                                                                    out decimal totalValorLiquido,
                                                                                    out decimal totalValorApropriado)
        {
            bool situacaoPagamentoPendente = filtro.EhSituacaoAPagarProvisionado || filtro.EhSituacaoAPagarAguardandoLiberacao || filtro.EhSituacaoAPagarLiberado || filtro.EhSituacaoAPagarCancelado;
            bool situacaoPagamentoPago = filtro.EhSituacaoAPagarEmitido || filtro.EhSituacaoAPagarPago || filtro.EhSituacaoAPagarBaixado;

            List<TituloPagar> listaTitulosPagar = new List<TituloPagar>();

            var specificationTeste1 = (Specification<TituloPagar>)new TrueSpecification<TituloPagar>();

            if ((situacaoPagamentoPendente) || ((!situacaoPagamentoPendente) && (!situacaoPagamentoPago)))
            {
                specificationTeste1 &= MontarSpecificationSituacaoPendentesRelContasPagarTitulos(filtro, usuarioId);
            }

            var specificationTeste2 = (Specification<TituloPagar>)new TrueSpecification<TituloPagar>();

            if ((situacaoPagamentoPago) || ((!situacaoPagamentoPendente) && (!situacaoPagamentoPago)))
            {
                specificationTeste2 &= MontarSpecificationSituacaoPagosRelContasPagarTitulos(filtro, usuarioId);
            }

            listaTitulosPagar =
             tituloPagarRepository.ListarPeloFiltroComUnion(specificationTeste1,
                                                            specificationTeste2,
                                                            l => l.Cliente.PessoaFisica,
                                                            l => l.Cliente.PessoaJuridica,
                                                            l => l.Movimento.ContaCorrente.Agencia,
                                                            l => l.Movimento.Caixa,
                                                            l => l.TipoCompromisso,
                                                            l => l.TipoDocumento,
                                                            l => l.ListaApropriacao.Select(a => a.CentroCusto),
                                                            l => l.ListaApropriacao.Select(a => a.Classe),
                                                            l => l.MotivoCancelamento).To<List<TituloPagar>>();

            var listaRelContasPagarTitulos = PopulaListaRelContasPagarTitulosDTO(filtro, 
                                                                                 listaTitulosPagar,
                                                                                 usuarioId,
                                                                                 out totalValorTitulo, 
                                                                                 out totalValorLiquido,
                                                                                 out totalValorApropriado);

            totalRegistros = listaRelContasPagarTitulos.Count();

            if ((filtro.EhTotalizadoPor.HasValue) && (filtro.EhTotalizadoPor.Value == 4))
            {
                filtro.PaginationParameters.OrderBy = "dataSelecao";
            }

            listaRelContasPagarTitulos = OrdenaListaRelContasPagarTitulosDTO(filtro, listaRelContasPagarTitulos);

            int pageCount = filtro.PaginationParameters.PageSize;
            int pageIndex = filtro.PaginationParameters.PageIndex;

            listaRelContasPagarTitulos = listaRelContasPagarTitulos.Skip(pageCount * pageIndex).Take(pageCount).To<List<RelContasPagarTitulosDTO>>();

            return listaRelContasPagarTitulos;

        }

        public FileDownloadDTO ExportarRelContasPagarTitulos(RelContasPagarTitulosFiltro filtro,
                                                             int? usuarioId,
                                                             FormatoExportacaoArquivo formato)
        {
            if (!EhPermitidoImprimirRelContasPagarTitulo())
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return null;
            }

            decimal totalValorTitulo = 0; 
            decimal totalValorLiquido = 0;
            decimal totalValorApropriado = 0;

            bool situacaoPagamentoPendente = filtro.EhSituacaoAPagarProvisionado || filtro.EhSituacaoAPagarAguardandoLiberacao || filtro.EhSituacaoAPagarLiberado || filtro.EhSituacaoAPagarCancelado;
            bool situacaoPagamentoPago = filtro.EhSituacaoAPagarEmitido || filtro.EhSituacaoAPagarPago || filtro.EhSituacaoAPagarBaixado;

            List<TituloPagar> listaTitulosPagar = new List<TituloPagar>();

            var specificationTeste1 = (Specification<TituloPagar>)new TrueSpecification<TituloPagar>();

            if ((situacaoPagamentoPendente) || ((!situacaoPagamentoPendente) && (!situacaoPagamentoPago)))
            {
                specificationTeste1 &= MontarSpecificationSituacaoPendentesRelContasPagarTitulos(filtro, usuarioId);
            }

            var specificationTeste2 = (Specification<TituloPagar>)new TrueSpecification<TituloPagar>();

            if ((situacaoPagamentoPago) || ((!situacaoPagamentoPendente) && (!situacaoPagamentoPago)))
            {
                specificationTeste2 &= MontarSpecificationSituacaoPagosRelContasPagarTitulos(filtro, usuarioId);
            }

            listaTitulosPagar =
             tituloPagarRepository.ListarPeloFiltroComUnion(specificationTeste1,
                                                            specificationTeste2,
                                                            l => l.Cliente.PessoaFisica,
                                                            l => l.Cliente.PessoaJuridica,
                                                            l => l.Movimento.ContaCorrente.Agencia,
                                                            l => l.Movimento.Caixa,
                                                            l => l.TipoCompromisso,
                                                            l => l.TipoDocumento,
                                                            l => l.ListaApropriacao.Select(a => a.CentroCusto),
                                                            l => l.ListaApropriacao.Select(a => a.Classe),
                                                            l => l.MotivoCancelamento).To<List<TituloPagar>>();

            var listaRelContasPagarTitulos = PopulaListaRelContasPagarTitulosDTO(filtro,
                                                                                 listaTitulosPagar,
                                                                                 usuarioId,
                                                                                 out totalValorTitulo,
                                                                                 out totalValorLiquido,
                                                                                 out totalValorApropriado);

            if ((filtro.EhTotalizadoPor.HasValue) && (filtro.EhTotalizadoPor.Value == 4))
            {
                listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.DataSelecao).ToList<RelContasPagarTitulosDTO>();
            }


            FileDownloadDTO arquivo = new FileDownloadDTO("Rel. Contas a pagar títulos", null, formato);

            var parametros = parametrosFinanceiroRepository.Obter();
            CentroCusto centroCusto = new CentroCusto();
            if (filtro.CentroCusto != null)
            {
                centroCusto = centroCustoRepository.ObterPeloCodigo(filtro.CentroCusto.Codigo, l => l.ListaCentroCustoEmpresa);
            }

            var caminhoImagem = PrepararIconeRelatorio(centroCusto, parametros);
            var nomeEmpresa = ObterNomeEmpresa(centroCusto, parametros);

            if (filtro.EhTotalizadoPor.HasValue)
            {
                switch (filtro.EhTotalizadoPor.Value)
                {
                    case 0:
                        relTituloPagar objRelGeral = new relTituloPagar();
                        objRelGeral.SetDataSource(RelContasPagarTitulosToDataTable(listaRelContasPagarTitulos));

                        objRelGeral.SetParameterValue("dataInicial", filtro.DataInicial.Value.ToString("dd/MM/yyyy"));
                        objRelGeral.SetParameterValue("dataFinal", filtro.DataFinal.Value.ToString("dd/MM/yyyy"));
                        objRelGeral.SetParameterValue("valorTotalBruto", totalValorTitulo);
                        objRelGeral.SetParameterValue("valorTotalLiquido", totalValorLiquido);
                        objRelGeral.SetParameterValue("NomeEmpresa", nomeEmpresa);
                        objRelGeral.SetParameterValue("tipoPesquisa", (filtro.EhPorCompetencia ? "V" : ""));
                        objRelGeral.SetParameterValue("semApropriacao", filtro.EhSemApropriacao);
                        objRelGeral.SetParameterValue("caminhoImagem", caminhoImagem);

                        arquivo = new FileDownloadDTO("Rel. Contas a pagar títulos",
                                                      objRelGeral.ExportToStream((ExportFormatType)formato),
                                                      formato);

                        break;

                    case 1:
                        relTituloPagarFornecedor objRelFornecedor = new relTituloPagarFornecedor();
                        objRelFornecedor.SetDataSource(RelContasPagarTitulosToDataTable(listaRelContasPagarTitulos.OrderBy(l => l.ClienteId).ThenBy(l => l.TituloId).ToList<RelContasPagarTitulosDTO>()));

                        objRelFornecedor.SetParameterValue("dataInicial", filtro.DataInicial.Value.ToString("dd/MM/yyyy"));
                        objRelFornecedor.SetParameterValue("dataFinal", filtro.DataFinal.Value.ToString("dd/MM/yyyy"));
                        objRelFornecedor.SetParameterValue("valorTotalBruto", totalValorTitulo);
                        objRelFornecedor.SetParameterValue("valorTotalLiquido", totalValorLiquido);
                        objRelFornecedor.SetParameterValue("NomeEmpresa", nomeEmpresa);
                        objRelFornecedor.SetParameterValue("tipoPesquisa", (filtro.EhPorCompetencia ? "V" : ""));
                        objRelFornecedor.SetParameterValue("caminhoImagem", caminhoImagem);

                        arquivo = new FileDownloadDTO("Rel. Contas a pagar títulos",
                                                      objRelFornecedor.ExportToStream((ExportFormatType)formato),
                                                      formato);
                        break;

                    case 2:
                        relTituloPagarClasse objRelClasse = new relTituloPagarClasse();
                        objRelClasse.SetDataSource(RelContasPagarTitulosToDataTable(listaRelContasPagarTitulos));

                        objRelClasse.SetParameterValue("dataInicial", filtro.DataInicial.Value.ToString("dd/MM/yyyy"));
                        objRelClasse.SetParameterValue("dataFinal", filtro.DataFinal.Value.ToString("dd/MM/yyyy"));
                        objRelClasse.SetParameterValue("valorTotalBruto", totalValorTitulo);
                        objRelClasse.SetParameterValue("valorTotalLiquido", totalValorLiquido);
                        objRelClasse.SetParameterValue("NomeEmpresa", nomeEmpresa);
                        objRelClasse.SetParameterValue("tipoPesquisa", (filtro.EhPorCompetencia ? "V" : ""));
                        objRelClasse.SetParameterValue("caminhoImagem", caminhoImagem);

                        arquivo = new FileDownloadDTO("Rel. Contas a pagar títulos",
                                                      objRelClasse.ExportToStream((ExportFormatType)formato),
                                                      formato);
                        break;

                    case 3:
                        relTituloPagarDataSituacao objRelDataSituacao = new relTituloPagarDataSituacao();
                        objRelDataSituacao.SetDataSource(RelContasPagarTitulosToDataTable(listaRelContasPagarTitulos));

                        objRelDataSituacao.SetParameterValue("dataInicial", filtro.DataInicial.Value.ToString("dd/MM/yyyy"));
                        objRelDataSituacao.SetParameterValue("dataFinal", filtro.DataFinal.Value.ToString("dd/MM/yyyy"));
                        objRelDataSituacao.SetParameterValue("valorTotalBruto", totalValorTitulo);
                        objRelDataSituacao.SetParameterValue("valorTotalLiquido", totalValorLiquido);
                        objRelDataSituacao.SetParameterValue("NomeEmpresa", nomeEmpresa);
                        objRelDataSituacao.SetParameterValue("tipoPesquisa", (filtro.EhPorCompetencia ? "V" : ""));
                        objRelDataSituacao.SetParameterValue("caminhoImagem", caminhoImagem);

                        arquivo = new FileDownloadDTO("Rel. Contas a pagar títulos",
                                                      objRelDataSituacao.ExportToStream((ExportFormatType)formato),
                                                      formato);

                        break;

                    case 4:
                        string parCentroCusto = "Todos";
                        if (centroCusto != null)
                        {
                            parCentroCusto = centroCusto.Codigo + " - " + centroCusto.Descricao;
                        }

                        string parClasse = "Todas";
                        if (!string.IsNullOrEmpty(filtro.Classe.Codigo))
                        {
                            parClasse = filtro.Classe.Codigo + " - " + filtro.Classe.Descricao;
                        }

                        string parCorrentista = "Todos";
                        if (filtro.ClienteFornecedor.Id.HasValue)
                        {
                            parCorrentista = filtro.ClienteFornecedor.Nome;
                        }

                        relTituloPagarDataSintetico objRelDataSituacaoSintetico = new relTituloPagarDataSintetico();
                        objRelDataSituacaoSintetico.SetDataSource(RelContasPagarTitulosToDataTable(listaRelContasPagarTitulos));

                        objRelDataSituacaoSintetico.SetParameterValue("dataInicial", filtro.DataInicial.Value.ToString("dd/MM/yyyy"));
                        objRelDataSituacaoSintetico.SetParameterValue("dataFinal", filtro.DataFinal.Value.ToString("dd/MM/yyyy"));
                        objRelDataSituacaoSintetico.SetParameterValue("NomeEmpresa", nomeEmpresa);
                        objRelDataSituacaoSintetico.SetParameterValue("tipoPesquisa", (filtro.EhPorCompetencia ? "V" : ""));
                        objRelDataSituacaoSintetico.SetParameterValue("centroCusto", parCentroCusto);
                        objRelDataSituacaoSintetico.SetParameterValue("classe", parClasse);
                        objRelDataSituacaoSintetico.SetParameterValue("correntista", parCorrentista);
                        objRelDataSituacaoSintetico.SetParameterValue("caminhoImagem", caminhoImagem);

                        arquivo = new FileDownloadDTO("Rel. Contas a pagar títulos",
                                                      objRelDataSituacaoSintetico.ExportToStream((ExportFormatType)formato),
                                                      formato);

                        break;


                    default:
                        break;
                }
            }

            if (System.IO.File.Exists(caminhoImagem))
                System.IO.File.Delete(caminhoImagem);


            return arquivo;
        }



        #endregion

        #region "Métodos privados"

        private DataTable RelContasPagarTitulosToDataTable(List<RelContasPagarTitulosDTO> listaRelContasPagarTitulos)
        {
            DataTable dta = new DataTable();
            DataColumn codigo = new DataColumn("codigo", System.Type.GetType("System.Int32"));
            DataColumn codigoCliente = new DataColumn("codigoCliente", System.Type.GetType("System.Int32"));
            DataColumn nomeCliente = new DataColumn("nomeCliente");
            DataColumn descricaoTipoCompromisso = new DataColumn("descricaoTipoCompromisso");
            DataColumn identificacao = new DataColumn("identificacao");
            DataColumn descricaoSituacao = new DataColumn("descricaoSituacao");
            DataColumn siglaTipoDocumento = new DataColumn("siglaTipoDocumento");
            DataColumn documento = new DataColumn("documento");
            DataColumn documentoCompleto = new DataColumn("documentoCompleto");
            DataColumn dataVencimento = new DataColumn("dataVencimento", System.Type.GetType("System.DateTime"));
            DataColumn dataCadastro = new DataColumn("dataCadastro", System.Type.GetType("System.DateTime"));
            DataColumn dataEmissaoDocumento = new DataColumn("dataEmissaoDocumento", System.Type.GetType("System.DateTime"));
            DataColumn valorTitulo = new DataColumn("valorTitulo", System.Type.GetType("System.Decimal"));
            DataColumn valorLiquido = new DataColumn("valorLiquido", System.Type.GetType("System.Decimal"));
            DataColumn dataEmissao = new DataColumn("dataEmissao", System.Type.GetType("System.DateTime"));
            DataColumn dataPagamento = new DataColumn("dataPagamento", System.Type.GetType("System.DateTime"));
            DataColumn dataBaixa = new DataColumn("dataBaixa", System.Type.GetType("System.DateTime"));
            DataColumn valorPago = new DataColumn("valorPago", System.Type.GetType("System.Decimal"));
            DataColumn codigoClasse = new DataColumn("codigoClasse");
            DataColumn descricaoClasse = new DataColumn("descricaoClasse");
            DataColumn codigoDescricaoClasse = new DataColumn("codigoDescricaoClasse");
            DataColumn codigoCentroCusto = new DataColumn("codigoCentroCusto");
            DataColumn descricaoCentroCusto = new DataColumn("descricaoCentroCusto");
            DataColumn codigoDescricaoCentroCusto = new DataColumn("codigoDescricaoCentroCusto");
            DataColumn valorApropriado = new DataColumn("valorApropriado", System.Type.GetType("System.Decimal"));
            DataColumn descricaoFormaPagamento = new DataColumn("descricaoFormaPagamento");
            DataColumn agenciaConta = new DataColumn("agenciaConta");
            DataColumn documentoPagamento = new DataColumn("documentoPagamento");
            DataColumn numeroCheque = new DataColumn("numeroCheque");
            DataColumn numeroChequeFormatado = new DataColumn("numeroChequeFormatado");
            DataColumn dataSituacao = new DataColumn("dataSituacao");
            DataColumn girErro = new DataColumn("girErro");

            dta.Columns.Add(codigo);
            dta.Columns.Add(codigoCliente);
            dta.Columns.Add(nomeCliente);
            dta.Columns.Add(descricaoTipoCompromisso);
            dta.Columns.Add(identificacao);
            dta.Columns.Add(descricaoSituacao);
            dta.Columns.Add(siglaTipoDocumento);
            dta.Columns.Add(documento);
            dta.Columns.Add(documentoCompleto);
            dta.Columns.Add(dataVencimento);
            dta.Columns.Add(dataCadastro);
            dta.Columns.Add(dataEmissaoDocumento);
            dta.Columns.Add(valorTitulo);
            dta.Columns.Add(valorLiquido);
            dta.Columns.Add(dataEmissao);
            dta.Columns.Add(dataPagamento);
            dta.Columns.Add(dataBaixa);
            dta.Columns.Add(valorPago);
            dta.Columns.Add(codigoClasse);
            dta.Columns.Add(descricaoClasse);
            dta.Columns.Add(codigoDescricaoClasse);
            dta.Columns.Add(codigoCentroCusto);
            dta.Columns.Add(descricaoCentroCusto);
            dta.Columns.Add(codigoDescricaoCentroCusto);
            dta.Columns.Add(valorApropriado);
            dta.Columns.Add(descricaoFormaPagamento);
            dta.Columns.Add(agenciaConta);
            dta.Columns.Add(documentoPagamento);
            dta.Columns.Add(numeroCheque);
            dta.Columns.Add(numeroChequeFormatado);
            dta.Columns.Add(dataSituacao);
            dta.Columns.Add(girErro);

            foreach (var item in listaRelContasPagarTitulos)
            {
                DataRow row = dta.NewRow();

                row[codigo] = item.TituloId;
                if (item.ClienteId.HasValue)
                {
                    row[codigoCliente] = item.ClienteId.Value;
                }
                row[nomeCliente] = item.NomeCliente;
                row[descricaoTipoCompromisso] = item.TipoCompromissoDescricao;
                row[identificacao] = item.Identificacao;
                row[descricaoSituacao] = item.SituacaoTituloDescricao;
                //row[siglaTipoDocumento] = null;
                //row[documento] = null;
                row[documentoCompleto] = item.DocumentoCompleto;
                row[dataVencimento] = item.DataVencimento.Date;
                if (item.DataCadastro.HasValue)
                {
                    row[dataCadastro] = item.DataCadastro.Value.Date.ToShortDateString();
                }
                row[dataEmissaoDocumento] = item.DataEmissaoDocumento;
                row[valorTitulo] = item.ValorTitulo;
                row[valorLiquido] = item.ValorLiquido;
                if (item.DataEmissao.HasValue)
                {
                    row[dataEmissao] = item.DataEmissao.Value.Date.ToShortDateString();
                }
                if (item.DataPagamento.HasValue)
                {
                    row[dataPagamento] = item.DataPagamento.Value.Date.ToShortDateString();
                }
                if (item.DataBaixa.HasValue)
                {
                    row[dataBaixa] = item.DataBaixa.Value.Date.ToShortDateString();
                }
                //row[valorPago] = null;
                row[codigoClasse] = item.CodigoClasse;
                //row[descricaoClasse] = null;
                row[codigoDescricaoClasse] = item.CodigoDescricaoClasse;
                row[codigoCentroCusto] = item.CodigoCentroCusto;
                //row[descricaoCentroCusto] = null;
                row[codigoDescricaoCentroCusto] = item.CodigoDescricaoCentroCusto;
                row[valorApropriado] = item.ValorApropriado;
                row[descricaoFormaPagamento] = item.FormaPagamentoDescricao;
                row[agenciaConta] = item.AgenciaContaCorrente;
                row[documentoPagamento] = item.DocumentoPagamento;

                row[documentoPagamento] = item.DocumentoPagamento;
                //row[numeroCheque] = null;
                //row[numeroChequeFormatado] = null;
                if (item.DataSelecao.HasValue)
                {
                    row[dataSituacao] = item.DataSelecao.Value.Date.ToShortDateString();
                }

                row[girErro] = "";

                dta.Rows.Add(row);
            }

            return dta;
        }

        private List<RelContasPagarTitulosDTO> OrdenaListaRelContasPagarTitulosDTO(RelContasPagarTitulosFiltro filtro, List<RelContasPagarTitulosDTO> listaRelContasPagarTitulos)
        {
            int pageCount = filtro.PaginationParameters.PageSize;
            int pageIndex = filtro.PaginationParameters.PageIndex;

            switch (filtro.PaginationParameters.OrderBy)
            {
                case "tituloId":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.TituloId).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.TituloId).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "dataVencimento":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.DataVencimento).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.DataVencimento).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "dataEmissaoDocumento":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.DataEmissaoDocumento).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.DataEmissaoDocumento).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "documentoCompleto":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.DocumentoCompleto).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.DocumentoCompleto).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "valorTitulo":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.ValorTitulo).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.ValorTitulo).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "valorLiquido":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.ValorLiquido).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.ValorLiquido).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "nomeCliente":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.NomeCliente).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.NomeCliente).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "identificacao":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.Identificacao).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.Identificacao).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "descricaoFormaPagamento":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.FormaPagamentoDescricao).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.FormaPagamentoDescricao).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "documentoPagamento":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.DocumentoPagamento).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.DocumentoPagamento).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "agenciaConta":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.AgenciaContaCorrente).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.AgenciaContaCorrente).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "descricaoTipoCompromisso":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.TipoCompromissoDescricao).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.TipoCompromissoDescricao).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "descricaoSituacao":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.SituacaoTituloDescricao).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.SituacaoTituloDescricao).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "dataSelecao":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.DataSelecao).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.DataSelecao).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "dataEmissao":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.DataEmissao).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.DataEmissao).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "dataPagamento":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.DataPagamento).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.DataPagamento).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "dataBaixa":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.DataBaixa).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.DataBaixa).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "cpfCnpj":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.CPFCNPJ).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.CPFCNPJ).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "operadorCadastro":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.LoginUsuarioCadastro).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.LoginUsuarioCadastro).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "valorApropriacao":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.ValorApropriado).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.ValorApropriado).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "classe":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.CodigoDescricaoClasse).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.CodigoDescricaoClasse).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "centroCusto":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.CodigoDescricaoCentroCusto).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.CodigoDescricaoCentroCusto).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                default:
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.TituloId).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.TituloId).ToList<RelContasPagarTitulosDTO>(); }
                    break;
            }

            return listaRelContasPagarTitulos;
        }

        private List<RelContasPagarTitulosDTO> PopulaListaRelContasPagarTitulosDTO(RelContasPagarTitulosFiltro filtro, List<TituloPagar> listaTitulosPagar,int? usuarioId, out decimal totalizadoValorTitulo, out decimal totalizadoValorLiquido, out decimal totalizadoValorApropriado)
        {
            bool situacaoPagamentoPendente = filtro.EhSituacaoAPagarProvisionado || filtro.EhSituacaoAPagarAguardandoLiberacao || filtro.EhSituacaoAPagarLiberado || filtro.EhSituacaoAPagarCancelado;
            bool situacaoPagamentoPago = filtro.EhSituacaoAPagarEmitido || filtro.EhSituacaoAPagarPago || filtro.EhSituacaoAPagarBaixado;

            string flagDataSituacao = "";
            if ((filtro.EhSituacaoAPagarEmitido) || ((!situacaoPagamentoPendente) && (!situacaoPagamentoPago)))
            {
                flagDataSituacao = "Emissao";
            }
            if (!filtro.EhSituacaoAPagarEmitido && filtro.EhSituacaoAPagarPago)
            {
                flagDataSituacao = "Pagamento";
            }
            if (!filtro.EhSituacaoAPagarEmitido && !filtro.EhSituacaoAPagarPago && filtro.EhSituacaoAPagarBaixado)
            {
                flagDataSituacao = "Baixa";
            }

            List<RelContasPagarTitulosDTO> listaRelContasPagarTitulos = new List<RelContasPagarTitulosDTO>();

            foreach (var tituloPagar in listaTitulosPagar)
            {
                RelContasPagarTitulosDTO relat = new RelContasPagarTitulosDTO();

                situacaoPagamentoPendente = false;
                situacaoPagamentoPago = false;

                relat.TituloId = tituloPagar.Id.Value;
                relat.DataVencimento = tituloPagar.DataVencimento;
                relat.DataEmissaoDocumento = tituloPagar.DataEmissaoDocumento;
                relat.DocumentoCompleto = "";
                if (tituloPagar.TipoDocumentoId.HasValue)
                {
                    relat.DocumentoCompleto = tituloPagar.TipoDocumento.Sigla + " " + tituloPagar.Documento;
                }
                relat.ValorTitulo = tituloPagar.ValorTitulo;

                relat.ClienteId = tituloPagar.ClienteId;
                relat.NomeCliente = tituloPagar.Cliente.Nome;
                relat.Identificacao = tituloPagar.Identificacao;
                relat.FormaPagamentoDescricao = "";
                if (tituloPagar.FormaPagamento.HasValue)
                {
                    switch ((FormaPagamento)tituloPagar.FormaPagamento.Value)
                    {
                        case FormaPagamento.Automatico:
                            relat.FormaPagamentoDescricao = FormaPagamento.Automatico.ObterDescricao();
                            break;
                        case FormaPagamento.Bordero:
                            relat.FormaPagamentoDescricao = FormaPagamento.Bordero.ObterDescricao();
                            break;
                        case FormaPagamento.BorderoEletrônico:
                            relat.FormaPagamentoDescricao = FormaPagamento.BorderoEletrônico.ObterDescricao();
                            break;
                        case FormaPagamento.Cheque:
                            relat.FormaPagamentoDescricao = FormaPagamento.Cheque.ObterDescricao();
                            break;
                        case FormaPagamento.Dinheiro:
                            relat.FormaPagamentoDescricao = FormaPagamento.Dinheiro.ObterDescricao();
                            break;
                        case FormaPagamento.OperacaoBancaria:
                            relat.FormaPagamentoDescricao = FormaPagamento.OperacaoBancaria.ObterDescricao();
                            break;
                        default:
                            relat.FormaPagamentoDescricao = "";
                            break;
                    }
                }
                relat.DocumentoPagamento = "";
                if (tituloPagar.MovimentoId.HasValue)
                {
                    relat.DocumentoPagamento = tituloPagar.Movimento.Documento;
                }
                relat.AgenciaContaCorrente = "";
                if ((tituloPagar.Movimento != null) && (tituloPagar.Movimento.ContaCorrente != null))
                {
                    ContaCorrente contaCorrente = tituloPagar.Movimento.ContaCorrente;
                    relat.AgenciaContaCorrente =
                        contaCorrente.Agencia.AgenciaCodigo + "-" + contaCorrente.Agencia.DVAgencia + " / " + contaCorrente.ContaCodigo + "-" + contaCorrente.DVConta;
                }
                relat.TipoCompromissoDescricao = "";
                if (tituloPagar.TipoCompromisso != null)
                {
                    relat.TipoCompromissoDescricao = tituloPagar.TipoCompromisso.Descricao;
                }
                relat.SituacaoTituloDescricao = tituloPagar.Situacao.ObterDescricao();
                relat.DataSelecao = null;

                switch (tituloPagar.Situacao)
                {
                    case SituacaoTituloPagar.AguardandoLiberacao:
                        relat.DataSelecao = tituloPagar.DataVencimento;
                        situacaoPagamentoPendente = true;
                        break;
                    case SituacaoTituloPagar.Provisionado:
                        relat.DataSelecao = tituloPagar.DataVencimento;
                        situacaoPagamentoPendente = true;
                        break;
                    case SituacaoTituloPagar.Liberado:
                        relat.DataSelecao = tituloPagar.DataVencimento;
                        situacaoPagamentoPendente = true;
                        break;
                    case SituacaoTituloPagar.Cancelado:
                        relat.DataSelecao = tituloPagar.DataVencimento;
                        situacaoPagamentoPendente = true;
                        break;
                    case SituacaoTituloPagar.Emitido:
                        if (flagDataSituacao == "Emissao") relat.DataSelecao = tituloPagar.DataEmissao;
                        if (flagDataSituacao == "Pagamento") relat.DataSelecao = tituloPagar.DataPagamento;
                        if (flagDataSituacao == "Baixa") relat.DataSelecao = tituloPagar.DataBaixa;
                        situacaoPagamentoPago = true;
                        break;
                    case SituacaoTituloPagar.Pago:
                        if (flagDataSituacao == "Emissao") relat.DataSelecao = tituloPagar.DataEmissao;
                        if (flagDataSituacao == "Pagamento") relat.DataSelecao = tituloPagar.DataPagamento;
                        if (flagDataSituacao == "Baixa") relat.DataSelecao = tituloPagar.DataBaixa;
                        situacaoPagamentoPago = true;
                        break;
                    case SituacaoTituloPagar.Baixado:
                        if (flagDataSituacao == "Emissao") relat.DataSelecao = tituloPagar.DataEmissao;
                        if (flagDataSituacao == "Pagamento") relat.DataSelecao = tituloPagar.DataPagamento;
                        if (flagDataSituacao == "Baixa") relat.DataSelecao = tituloPagar.DataBaixa;
                        situacaoPagamentoPago = true;
                        break;
                    default:
                        relat.FormaPagamentoDescricao = "";
                        break;
                }

                if (situacaoPagamentoPendente)
                {
                    relat.ValorLiquido = CalculaValorLiquido(tituloPagar,DateTime.Now.Date);
                }
                else
                {
                    decimal valorPago = tituloPagar.ValorPago.HasValue ? tituloPagar.ValorPago.Value : 0;
                    relat.ValorLiquido = valorPago;
                }

                relat.DataEmissao = null;
                if (tituloPagar.DataEmissao.HasValue)
                {
                    relat.DataEmissao = tituloPagar.DataEmissao.Value.Date;
                }
                relat.DataPagamento = null;
                if (tituloPagar.DataPagamento.HasValue)
                {
                    relat.DataPagamento = tituloPagar.DataPagamento.Value.Date;
                }
                relat.DataBaixa = null;
                if (tituloPagar.DataBaixa.HasValue)
                {
                    relat.DataBaixa = tituloPagar.DataBaixa.Value.Date;
                }
                relat.MotivoCancelamentoDescricao = "";
                if (tituloPagar.MotivoCancelamento != null)
                {
                    relat.MotivoCancelamentoDescricao = tituloPagar.MotivoCancelamento.Descricao;
                }
                if (tituloPagar.SistemaOrigem != "FINAN")
                {
                    relat.MotivoCancelamentoDescricao = tituloPagar.MotivoCancelamentoInterface;
                }
                relat.CPFCNPJ = "";
                if (tituloPagar.Cliente != null)
                {
                    if (tituloPagar.Cliente.TipoPessoa == "F")
                    {
                        relat.CPFCNPJ = tituloPagar.Cliente.PessoaFisica.Cpf;
                    }
                    else
                    {
                        if (tituloPagar.Cliente.TipoPessoa == "J")
                        {
                            relat.CPFCNPJ = tituloPagar.Cliente.PessoaJuridica.Cnpj;
                            if ((filtro.VisualizarClientePor.HasValue) && (filtro.VisualizarClientePor.Value == 1))
                            {
                                relat.NomeCliente = tituloPagar.Cliente.PessoaJuridica.NomeFantasia;
                            }
                        }
                    }
                }
                relat.LoginUsuarioCadastro = tituloPagar.LoginUsuarioCadastro;
                relat.DataCadastro = null;
                if (tituloPagar.DataCadastro.HasValue)
                {
                    relat.DataCadastro = tituloPagar.DataCadastro.Value.Date;
                }

                if (!filtro.EhSemApropriacao)
                {
                    if (tituloPagar.ListaApropriacao.Count > 0)
                    {
                        decimal totalValorApropriado = 0;
                        foreach (var apropriacao in tituloPagar.ListaApropriacao)
                        {
                            if (!string.IsNullOrEmpty(filtro.Classe.Codigo))
                            {
                                if (filtro.Classe.Codigo != apropriacao.Classe.Codigo)
                                {
                                    continue;
                                }
                            }
                            if (!string.IsNullOrEmpty(filtro.CentroCusto.Codigo))
                            {
                                if (filtro.CentroCusto.Codigo != apropriacao.CentroCusto.Codigo)
                                {
                                    continue;
                                }
                                if (usuarioAppService.UsuarioPossuiCentroCustoDefinidoNoModulo(usuarioId, Resource.Sigim.NomeModulo.Financeiro))
                                {
                                    if (!apropriacao.CentroCusto.ListaUsuarioCentroCusto.Any(c =>
                                        c.UsuarioId == usuarioId && c.Modulo.Nome == Resource.Sigim.NomeModulo.Financeiro && c.CentroCusto.Situacao == "A"))
                                    {
                                        continue;
                                    }
                                }
                            }

                            if ((filtro.EhTotalizadoPor.HasValue) && (filtro.EhTotalizadoPor.Value == 4))
                            {
                                totalValorApropriado = totalValorApropriado + apropriacao.Valor;
                            }
                            else
                            {
                                relat.ValorApropriado = apropriacao.Valor;
                                relat.CodigoClasse = apropriacao.Classe.Codigo;
                                relat.CodigoDescricaoClasse = apropriacao.Classe.Codigo + " - " + apropriacao.Classe.Descricao;
                                relat.CodigoCentroCusto = apropriacao.CentroCusto.Codigo;
                                relat.CodigoDescricaoCentroCusto = apropriacao.CentroCusto.Codigo + " - " + apropriacao.CentroCusto.Descricao;

                                listaRelContasPagarTitulos.Add(relat);
                            }
                        }
                        if ((filtro.EhTotalizadoPor.HasValue) && (filtro.EhTotalizadoPor.Value == 4))
                        {
                            relat.ValorApropriado = totalValorApropriado;
                            listaRelContasPagarTitulos.Add(relat);
                        }

                    }
                    else
                    {
                        relat.ValorApropriado = 0;
                        relat.CodigoClasse = "";
                        relat.CodigoDescricaoClasse = "";
                        relat.CodigoCentroCusto = "";
                        relat.CodigoDescricaoCentroCusto = "";

                        listaRelContasPagarTitulos.Add(relat);
                    }
                }
                else
                {
                    relat.ValorApropriado = 0;
                    relat.CodigoClasse = "";
                    relat.CodigoDescricaoClasse = "";
                    relat.CodigoCentroCusto = "";
                    relat.CodigoDescricaoCentroCusto = "";

                    listaRelContasPagarTitulos.Add(relat);
                }

            }

            totalizadoValorTitulo = listaRelContasPagarTitulos.Sum(l => l.ValorTitulo);
            totalizadoValorLiquido = listaRelContasPagarTitulos.Sum(l => l.ValorLiquido);
            totalizadoValorApropriado = listaRelContasPagarTitulos.Sum(l => l.ValorApropriado);

            return listaRelContasPagarTitulos;
        }

        private decimal CalculaValorLiquido(TituloPagar titulo, DateTime dataEmissao)
        {
            decimal valorLiquido = 0;

            if (!ValidaData(titulo.DataVencimento.ToShortDateString()))
            {
                return valorLiquido;
            }

            decimal valorImposto = titulo.ValorImposto.HasValue ? titulo.ValorImposto.Value : 0;

            valorLiquido = titulo.ValorTitulo - valorImposto;

            decimal valorMultaCalculada = 0;
            decimal taxaPermanenciaCalculada = 0;
            decimal multa = titulo.Multa.HasValue ? titulo.Multa.Value : 0;
            decimal taxaPermanencia = titulo.TaxaPermanencia.HasValue ? titulo.TaxaPermanencia.Value : 0;
            int atraso = moduloSigimAppService.ObtemQuantidadeDeDias(titulo.DataVencimento,dataEmissao);
            int fator;
            decimal valorTaxa = 0;
            decimal retencao = titulo.Retencao.HasValue ? titulo.Retencao.Value : 0;
            decimal desconto = titulo.Desconto.HasValue ? titulo.Desconto.Value : 0;

            if (!moduloSigimAppService.OperacaoEmDia(titulo.DataVencimento, dataEmissao))
            {
                if ((titulo.EhMultaPercentual.HasValue) && (titulo.EhMultaPercentual.Value))
                {
                    valorMultaCalculada = ((titulo.ValorTitulo * multa) / 100);
                    valorMultaCalculada = Math.Round(valorMultaCalculada,2);
                }
                else
                {
                    valorMultaCalculada = Math.Round(multa,2);
                }

                if ((titulo.EhTaxaPermanenciaPercentual.HasValue) && (titulo.EhTaxaPermanenciaPercentual.Value))
                {
                    fator = 30;
                    valorTaxa = (taxaPermanencia / fator) * atraso;
                    taxaPermanenciaCalculada = ((titulo.ValorTitulo * valorTaxa) / 100);
                }
                else
                {
                    fator = 1;
                    valorTaxa = (taxaPermanencia / fator) * atraso;
                    taxaPermanenciaCalculada = Math.Round(valorTaxa,2);
                }
                valorLiquido = valorLiquido -  retencao + valorMultaCalculada + taxaPermanenciaCalculada;
                if (titulo.DataLimiteDesconto.HasValue)
                {
                    if (titulo.DataLimiteDesconto.Value.Date >= dataEmissao)
                    {
                        valorLiquido = Math.Round((valorLiquido - desconto),2);
                    }
                }
            }
            else
            {
                if (titulo.DataLimiteDesconto.HasValue)
                {
                    if (titulo.DataLimiteDesconto.Value.Date >= dataEmissao)
                    {
                        valorLiquido = Math.Round((valorLiquido - desconto),2);
                    }
                }
                valorLiquido = Math.Round((valorLiquido - retencao),2);
            }

            return valorLiquido;
        }

        private Specification<TituloPagar> MontarSpecificationSituacaoPendentesRelContasPagarTitulos(RelContasPagarTitulosFiltro filtro, int? idUsuario)
        {
            bool situacaoPagamentoPendente = filtro.EhSituacaoAPagarProvisionado || filtro.EhSituacaoAPagarAguardandoLiberacao || filtro.EhSituacaoAPagarLiberado || filtro.EhSituacaoAPagarCancelado;
            bool situacaoPagamentoPago = filtro.EhSituacaoAPagarEmitido || filtro.EhSituacaoAPagarPago || filtro.EhSituacaoAPagarBaixado;

            var specification = (Specification<TituloPagar>)new TrueSpecification<TituloPagar>();

            specification &= TituloPagarSpecification.DataPeriodoMaiorOuIgualSituacaoPendentesRelContasPagarTitulos(filtro.DataInicial);
            specification &= TituloPagarSpecification.DataPeriodoMenorOuIgualSituacaoPendentesRelContasPagarTitulos(filtro.DataFinal);

            if ((!situacaoPagamentoPendente) && (!situacaoPagamentoPago))
            {
                specification &= (TituloPagarSpecification.EhSituacaoAPagarProvisionado(true)) ||
                    (TituloPagarSpecification.EhSituacaoAPagarAguardandoLiberacao(true)) ||
                    (TituloPagarSpecification.EhSituacaoAPagarLiberado(true));
            }
            else
            {
                if (filtro.EhSituacaoAPagarProvisionado || filtro.EhSituacaoAPagarAguardandoLiberacao || filtro.EhSituacaoAPagarLiberado || filtro.EhSituacaoAPagarCancelado)
                {
                    specification &= (TituloPagarSpecification.EhSituacaoAPagarProvisionado(filtro.EhSituacaoAPagarProvisionado)) ||
                        (TituloPagarSpecification.EhSituacaoAPagarAguardandoLiberacao(filtro.EhSituacaoAPagarAguardandoLiberacao)) ||
                        (TituloPagarSpecification.EhSituacaoAPagarLiberado(filtro.EhSituacaoAPagarLiberado)) ||
                        (TituloPagarSpecification.EhSituacaoAPagarCancelado(filtro.EhSituacaoAPagarCancelado));
                }
            }

            specification &= TituloPagarSpecification.PertenceAoCentroCustoIniciadoPor(filtro.CentroCusto.Codigo);

            specification &= TituloPagarSpecification.PertenceAClasseIniciadaPor(filtro.Classe.Codigo);

            specification &= TituloPagarSpecification.MatchingClienteId(filtro.ClienteFornecedor.Id);

            specification &= TituloPagarSpecification.IdentificacaoContem(filtro.Identificacao);

            string documento = !string.IsNullOrEmpty(filtro.Documento) ? RetiraZerosIniciaisNumeroDocumento(filtro.Documento) : null;

            specification &= TituloPagarSpecification.DocumentoContem(documento);

            specification &= TituloPagarSpecification.MatchingTipoCompromissoId(filtro.TipoCompromissoId);

            specification &= TituloPagarSpecification.PertenceAhFormaPagamento(filtro.FormaPagamentoCodigo);

            specification &= TituloPagarSpecification.DocumentoPagamentoContem(filtro.DocumentoPagamento);

            specification &= TituloPagarSpecification.EhTipoTituloDiferenteDeTituloPai();

            specification &= TituloPagarSpecification.ValorTituloMaiorOuIgualRelContasPagarTitulos(filtro.ValorTituloInicial);
            specification &= TituloPagarSpecification.ValorTituloMenorOuIgualRelContasPagarTitulos(filtro.ValorTituloFinal);

            specification &= TituloPagarSpecification.MatchingMovimentoBancoId(filtro.BancoId);

            specification &= TituloPagarSpecification.MatchingMovimentoContaCorrenteId(filtro.ContaCorrenteId);

            specification &= TituloPagarSpecification.MatchingMovimentoCaixaId(filtro.CaixaId);

            return specification;
        }

        private Specification<TituloPagar> MontarSpecificationSituacaoPagosRelContasPagarTitulos(RelContasPagarTitulosFiltro filtro, int? idUsuario)
        {
            bool situacaoPagamentoPendente = filtro.EhSituacaoAPagarProvisionado || filtro.EhSituacaoAPagarAguardandoLiberacao || filtro.EhSituacaoAPagarLiberado || filtro.EhSituacaoAPagarCancelado;
            bool situacaoPagamentoPago = filtro.EhSituacaoAPagarEmitido || filtro.EhSituacaoAPagarPago || filtro.EhSituacaoAPagarBaixado;

            string tipoPesquisa = filtro.EhPorCompetencia ? "V" : "";

            var specification = (Specification<TituloPagar>)new TrueSpecification<TituloPagar>();

            SituacaoTituloPagar situacaoTitulo = SituacaoTituloPagar.Emitido;

            if ((!situacaoPagamentoPendente) && (!situacaoPagamentoPago))
            {
                specification &= (TituloPagarSpecification.EhSituacaoAPagarEmitido(true)) ||
                    (TituloPagarSpecification.EhSituacaoAPagarPago(true)) ||
                    (TituloPagarSpecification.EhSituacaoAPagarBaixado(true));
            }
            else
            {
                if (filtro.EhSituacaoAPagarPago && (!filtro.EhSituacaoAPagarEmitido && !filtro.EhSituacaoAPagarBaixado))
                {
                    situacaoTitulo = SituacaoTituloPagar.Pago;
                }
                else 
                {
                    if (filtro.EhSituacaoAPagarBaixado && (!filtro.EhSituacaoAPagarEmitido && !filtro.EhSituacaoAPagarPago))
                    {
                        situacaoTitulo = SituacaoTituloPagar.Baixado;
                    }
                }

                if (filtro.EhSituacaoAPagarEmitido || filtro.EhSituacaoAPagarPago || filtro.EhSituacaoAPagarBaixado)
                {
                    specification &= (TituloPagarSpecification.EhSituacaoAPagarEmitido(filtro.EhSituacaoAPagarEmitido)) ||
                        (TituloPagarSpecification.EhSituacaoAPagarPago(filtro.EhSituacaoAPagarPago)) ||
                        (TituloPagarSpecification.EhSituacaoAPagarBaixado(filtro.EhSituacaoAPagarBaixado));
                }
            }

            specification &= TituloPagarSpecification.DataPeriodoMaiorOuIgualSituacaoPagosRelContasPagarTitulos(situacaoTitulo,tipoPesquisa,filtro.DataInicial);
            specification &= TituloPagarSpecification.DataPeriodoMenorOuIgualSituacaoPagosRelContasPagarTitulos(situacaoTitulo,tipoPesquisa,filtro.DataFinal);

            specification &= TituloPagarSpecification.PertenceAoCentroCustoIniciadoPor(filtro.CentroCusto.Codigo);

            specification &= TituloPagarSpecification.PertenceAClasseIniciadaPor(filtro.Classe.Codigo);

            specification &= TituloPagarSpecification.MatchingClienteId(filtro.ClienteFornecedor.Id);

            specification &= TituloPagarSpecification.IdentificacaoContem(filtro.Identificacao);

            string documento = !string.IsNullOrEmpty(filtro.Documento) ? RetiraZerosIniciaisNumeroDocumento(filtro.Documento) : null;

            specification &= TituloPagarSpecification.DocumentoContem(documento);

            specification &= TituloPagarSpecification.MatchingTipoCompromissoId(filtro.TipoCompromissoId);

            specification &= TituloPagarSpecification.PertenceAhFormaPagamento(filtro.FormaPagamentoCodigo);

            specification &= TituloPagarSpecification.DocumentoPagamentoContem(filtro.DocumentoPagamento);

            specification &= TituloPagarSpecification.EhTipoTituloDiferenteDeTituloPai();

            specification &= TituloPagarSpecification.ValorTituloMaiorOuIgualRelContasPagarTitulos(filtro.ValorTituloInicial);
            specification &= TituloPagarSpecification.ValorTituloMenorOuIgualRelContasPagarTitulos(filtro.ValorTituloFinal);

            specification &= TituloPagarSpecification.MatchingMovimentoBancoId(filtro.BancoId);

            specification &= TituloPagarSpecification.MatchingMovimentoContaCorrenteId(filtro.ContaCorrenteId);

            specification &= TituloPagarSpecification.MatchingMovimentoCaixaId(filtro.CaixaId);

            return specification;
        }


        #endregion
    }
}
