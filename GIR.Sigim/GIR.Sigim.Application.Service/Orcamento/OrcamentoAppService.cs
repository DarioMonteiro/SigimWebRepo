using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Orcamento;
using GIR.Sigim.Domain.Entity.Orcamento;
using GIR.Sigim.Domain.Repository.Orcamento;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Application.Filtros.Financeiro;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Application.Filtros.Orcamento;
using GIR.Sigim.Application.Enums;
using GIR.Sigim.Domain.Specification.Orcamento;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Application.Reports.Orcamento;
using CrystalDecisions.Shared;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Domain.Entity.Sigim;


namespace GIR.Sigim.Application.Service.Orcamento
{
    public class OrcamentoAppService : BaseAppService, IOrcamentoAppService
    {
        #region Declaração

        private IOrcamentoRepository orcamentoRepository;
        private IParametrosOrcamentoRepository parametrosOrcamentoRepository;
        private ICentroCustoRepository centroCustoRepository;
        private ICotacaoValoresRepository cotacaoValoresRepository;
        private IIndiceFinanceiroRepository indiceFinanceiroRepository;

        #endregion

        #region Construtor

        public OrcamentoAppService(IOrcamentoRepository orcamentoRepository, 
                                   IParametrosOrcamentoRepository parametrosOrcamentoRepository, 
                                   ICentroCustoRepository centroCustoRepository,
                                   ICotacaoValoresRepository cotacaoValoresRepository,
                                   IIndiceFinanceiroRepository indiceFinanceiroRepository,
                                   MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.orcamentoRepository = orcamentoRepository;
            this.parametrosOrcamentoRepository = parametrosOrcamentoRepository;
            this.centroCustoRepository = centroCustoRepository;
            this.cotacaoValoresRepository = cotacaoValoresRepository;
            this.indiceFinanceiroRepository = indiceFinanceiroRepository;
        }

        #endregion

        #region Métodos IOrcamentoAppService

        public OrcamentoDTO ObterPeloId(int? id)
        {
            return orcamentoRepository.ObterPeloId(id).To<OrcamentoDTO>();
        }

        public OrcamentoDTO ObterUltimoOrcamentoPeloCentroCustoClasseOrcamento(string codigoCentroCusto)
        {
            return orcamentoRepository.ObterUltimoOrcamentoPeloCentroCustoClasseOrcamento(codigoCentroCusto).To<OrcamentoDTO>();
        }

        public OrcamentoDTO ObterUltimoOrcamentoPeloCentroCusto(string codigoCentroCusto)
        {
            return orcamentoRepository.ObterUltimoOrcamentoPeloCentroCusto(codigoCentroCusto).To<OrcamentoDTO>();
        }

        public bool EhPermitidoImprimirRelOrcamento()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.RelatorioOrcamentoImprimir);
        }

        public List<OrcamentoDTO> PesquisarOrcamentosPeloFiltro(OrcamentoPesquisaFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<Domain.Entity.Orcamento.Orcamento>)new TrueSpecification<Domain.Entity.Orcamento.Orcamento>();
            int? inicio;
            int? fim;

            specification &= OrcamentoSpecification.MatchingEmpresa(filtro.EmpresaId);
            specification &= OrcamentoSpecification.MatchingObra(filtro.ObraId);

            bool EhTipoSelecaoContem = filtro.TipoSelecao == TipoPesquisa.Contem;
            bool EhTipoSelecaoIntervalo = filtro.TipoSelecao == TipoPesquisa.Intervalo;
            switch (filtro.Campo)
            {
                case "sequencial":
                    if (EhTipoSelecaoIntervalo)
                    {
                        inicio = !string.IsNullOrEmpty(filtro.TextoInicio) ? Convert.ToInt32(filtro.TextoInicio) : (int?)null;
                        fim = !string.IsNullOrEmpty(filtro.TextoFim) ? Convert.ToInt32(filtro.TextoFim) : (int?)null;
                        specification &= OrcamentoSpecification.SequencialNoIntervalo(inicio, fim);
                    }
                    break;
                case "descricao":
                    specification &= EhTipoSelecaoContem ? OrcamentoSpecification.DescricaoContem(filtro.TextoInicio)
                        : OrcamentoSpecification.DescricaoNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                    break;
                case "obra":
                    specification &= EhTipoSelecaoContem ? OrcamentoSpecification.ObraNumeroContem(filtro.TextoInicio)
                        : OrcamentoSpecification.ObraNumeroNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                    break;
                case "empresa":
                    specification &= EhTipoSelecaoContem ? OrcamentoSpecification.EmpresaNumeroContem(filtro.TextoInicio)
                        : OrcamentoSpecification.EmpresaNumeroNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                    break;
                case "centroCusto":
                    specification &= EhTipoSelecaoContem ? OrcamentoSpecification.CentroCustoContem(filtro.TextoInicio)
                        : OrcamentoSpecification.CentroCustoNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                    break;
                case "situacao":
                    specification &= EhTipoSelecaoContem ? OrcamentoSpecification.SituacaoContem(filtro.TextoInicio)
                        : OrcamentoSpecification.SituacaoNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                    break;
                case "data":
                    Nullable<DateTime> datInicio = !string.IsNullOrEmpty(filtro.TextoInicio) ? Convert.ToDateTime(filtro.TextoInicio) : (Nullable<DateTime>)null;
                    Nullable<DateTime> datFim = !string.IsNullOrEmpty(filtro.TextoFim) ? Convert.ToDateTime(filtro.TextoFim) : (Nullable<DateTime>)null;
                    specification &= EhTipoSelecaoContem ? OrcamentoSpecification.DataContem(datInicio)
                        : OrcamentoSpecification.DataNoIntervalo(datInicio, datFim);
                    break;
                //case "simplificado":
                //    specification &= EhTipoSelecaoContem ? OrcamentoSpecification.SimplificadoContem(filtro.TextoInicio)
                //        : OrcamentoSpecification.SimplificadoNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                //    break;

                default:
                    break;
            }

            return orcamentoRepository.Pesquisar(specification,
                                                filtro.PageIndex,
                                                filtro.PageSize,
                                                filtro.OrderBy,
                                                filtro.Ascending,
                                                out totalRegistros,
                                                l => l.Obra.CentroCusto,
                                                l => l.Empresa.ClienteFornecedor).To<List<OrcamentoDTO>>();
        }

        public OrcamentoDTO GerarRelatorioOrcamento(RelOrcamentoFiltro filtro)
        {

            if (!EhPermitidoImprimirRelOrcamento())
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return null;
            }

            if (!ValidarFiltroRelOrcamento(filtro))
            {
                return null;
            }

            OrcamentoDTO orcamentoDTO = null;

            GIR.Sigim.Domain.Entity.Orcamento.Orcamento orcamento 
                = orcamentoRepository.ObterPeloId(filtro.Orcamento.Id.Value,
                                                  l => l.Empresa.ClienteFornecedor,
                                                  l => l.Obra.CentroCusto,
                                                  l => l.ListaOrcamentoComposicao.Select(c => c.Composicao));

            if (orcamento != null)
            {
                decimal percentualBDI = orcamento.Obra.BDIPercentual.HasValue ? orcamento.Obra.BDIPercentual.Value : 0;
                percentualBDI = percentualBDI / 100;
                decimal cotacao = 1;
                decimal cotacaoAtual = 1;
                decimal preco = 0;
                decimal quantidade = 0;
                decimal BDI = 0;
                decimal precoSemBDI = 0;
                decimal precoTotal = 0;
                decimal precoTotalSemBDI = 0;
                decimal BDITotal = 0;
                int defasagem = filtro.Defasagem.HasValue ? filtro.Defasagem.Value : 0;
                DateTime dataBase = orcamento.Data.HasValue ? orcamento.Data.Value : new DateTime(1990,1,1);
                dataBase = dataBase.Date.AddMonths(-1 * defasagem);
                DateTime dataAtual = DateTime.Now.Date;
                dataAtual = dataAtual.Date.AddMonths(-1 * defasagem);
                string nomeIndice = "Não informado";

                if (filtro.IndiceId.HasValue)
                {
                    IndiceFinanceiro indiceFinanceiro = indiceFinanceiroRepository.ObterPeloId(filtro.IndiceId.Value);
                    nomeIndice = indiceFinanceiro.Descricao;
                    cotacao = cotacaoValoresRepository.RecuperaCotacao(filtro.IndiceId.Value, dataBase.Date);

                    if (filtro.EhValorCorrigido)
                    {
                        cotacaoAtual = cotacaoValoresRepository.RecuperaCotacao(filtro.IndiceId.Value, dataAtual.Date);
                    }
                }

                foreach (var orcamentoComposicao in orcamento.ListaOrcamentoComposicao)
                {
                    preco = orcamentoComposicao.Preco.HasValue ? orcamentoComposicao.Preco.Value : 0;
                    quantidade = orcamentoComposicao.Preco.HasValue ? orcamentoComposicao.Preco.Value : 0;

                    if (cotacao != 0)
                    {
                        preco = preco / cotacao;
                    }

                    if (cotacaoAtual != 0)
                    {
                        preco = preco / cotacaoAtual;
                    }

                    preco = Math.Round(preco, 5);
                    BDI = preco * percentualBDI;
                    BDI = Math.Round(BDI, 4);
                    precoSemBDI = preco;
                    if (filtro.EhBDI)
                    {
                        preco += BDI;
                    }

                    orcamentoComposicao.Preco = preco;
                    precoTotal = precoTotal + Math.Round((preco * quantidade), 5);
                    precoTotalSemBDI = precoTotalSemBDI + Math.Round((precoSemBDI * quantidade), 5);
                }

                BDITotal = precoTotalSemBDI * percentualBDI;
                filtro.BDITotal = Math.Round(BDITotal, 4);
                filtro.PrecoTotal = precoTotal;
                filtro.NomeIndice = nomeIndice;
                filtro.CotacaoBase = Math.Round(cotacao,5);
                filtro.CotacaoAtual = Math.Round(cotacaoAtual,5);
                filtro.Defasagem = defasagem;
                filtro.DataBase = dataBase;
                filtro.DataAtual = dataAtual;
                filtro.AreaConstrucaoAreaReal = orcamento.Obra.AreaConstrucaoAreaReal.HasValue ? orcamento.Obra.AreaConstrucaoAreaReal.Value : 0;
                filtro.AreaConstrucaoAreaEquivalente = orcamento.Obra.AreaConstrucaoAreaEquivalente.HasValue ? orcamento.Obra.AreaConstrucaoAreaEquivalente.Value : 0;

                orcamentoDTO = orcamento.To<OrcamentoDTO>();
            }


            return orcamentoDTO;

        }

        public FileDownloadDTO ExportarRelOrcamento(RelOrcamentoFiltro filtro, OrcamentoDTO orcamentoDTO, FormatoExportacaoArquivo formato)
        {
            DataTable dtaRelatorio = new DataTable();

            dtaRelatorio = RelOrcamentoToDataTable(orcamentoDTO);

            ParametrosOrcamento parametros = parametrosOrcamentoRepository.Obter();
            var centroCusto = centroCustoRepository.ObterPeloCodigo(orcamentoDTO.Obra.CentroCusto.Codigo, l => l.ListaCentroCustoEmpresa);
            var caminhoImagem = PrepararIconeRelatorio(centroCusto, parametros);
            var nomeEmpresa = ObterNomeEmpresa(centroCusto, parametros);

            FileDownloadDTO arquivo = new FileDownloadDTO("Rel. Orçamento ", null, formato);

            relOrcamento objRelOrcamento = new relOrcamento();

            objRelOrcamento.SetDataSource(dtaRelatorio);

            objRelOrcamento.SetParameterValue("ComBDI", filtro.EhBDI);
            objRelOrcamento.SetParameterValue("SemDetalhamento", filtro.EhSemDetalhamento);
            objRelOrcamento.SetParameterValue("TotalBDI", filtro.BDITotal);
            objRelOrcamento.SetParameterValue("ValorTotal", filtro.PrecoTotal);
            objRelOrcamento.SetParameterValue("NomeIndice", filtro.NomeIndice);
            objRelOrcamento.SetParameterValue("Defasagem", filtro.Defasagem);
            objRelOrcamento.SetParameterValue("DataBase", filtro.DataBase);
            objRelOrcamento.SetParameterValue("CotacaoBase", filtro.CotacaoBase);
            objRelOrcamento.SetParameterValue("DataAtual", filtro.DataAtual);
            objRelOrcamento.SetParameterValue("CotacaoAtual", filtro.CotacaoAtual);
            objRelOrcamento.SetParameterValue("ValorCorrigido", filtro.EhValorCorrigido);
            objRelOrcamento.SetParameterValue("AreaConstrucaoAreaReal", filtro.AreaConstrucaoAreaReal);
            objRelOrcamento.SetParameterValue("AreaConstrucaoAreaEquivalente", filtro.AreaConstrucaoAreaEquivalente);
            //objRelOrcamento.SetParameterValue("nomeEmpresa", nomeEmpresa);
            objRelOrcamento.SetParameterValue("caminhoImagem", caminhoImagem);

            arquivo = new FileDownloadDTO("Rel. Orçamento", objRelOrcamento.ExportToStream((ExportFormatType)formato), formato);


            if (System.IO.File.Exists(caminhoImagem))
            {
                System.IO.File.Delete(caminhoImagem);
            }

            return arquivo;

        }

        #endregion

        #region "Métodos privados"

        private string ObterNomeEmpresa(CentroCusto centroCusto, ParametrosOrcamento parametros)
        {
            if (centroCusto != null)
            {
                var centroCustoEmpresa = centroCusto.ListaCentroCustoEmpresa.FirstOrDefault();
                if (centroCustoEmpresa != null)
                    return centroCustoEmpresa.Cliente.Nome;
            }
            return !string.IsNullOrEmpty(parametros.EmpresaNomeRaiz) ? parametros.EmpresaNomeRaiz : string.Empty;
        }

        private string PrepararIconeRelatorio(CentroCusto centroCusto, ParametrosOrcamento parametros)
        {
            var caminhoImagem = string.Empty;
            var iconeRelatorio = ObterIconeRelatorio(centroCusto) ?? parametros.IconeRelatorio;

            if (iconeRelatorio == null)
            {
                string diretorio = AppDomain.CurrentDomain.BaseDirectory + "/Content/img/sigim_AzulCopia.png";

                System.Uri uri = new System.Uri(diretorio);

                using (System.Drawing.Image image = System.Drawing.Image.FromFile(diretorio))
                {
                    using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                    {
                        image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
                        iconeRelatorio = memoryStream.ToArray();
                    }
                }
            }

            if (iconeRelatorio != null)
            {
                caminhoImagem = DiretorioImagemRelatorio + Guid.NewGuid().ToString() + ".bmp";
                System.Drawing.Image imagem = iconeRelatorio.ToImage();
                imagem.Save(caminhoImagem, System.Drawing.Imaging.ImageFormat.Bmp);
            }

            return caminhoImagem;
        }

        private bool ValidarFiltroRelOrcamento(RelOrcamentoFiltro filtro)
        {
            if ( (!filtro.Orcamento.Id.HasValue) || (filtro.Orcamento.Id.HasValue) && (filtro.Orcamento.Id == 0))
            {
                messageQueue.Add("Selecione o orçamento", TypeMessage.Warning);
                return false;
            }

            if ((filtro.EhValorCorrigido) && ((!filtro.IndiceId.HasValue) || (filtro.IndiceId.HasValue) && (filtro.IndiceId == 0)))
            {
                messageQueue.Add("Informe o índice", TypeMessage.Warning);
                return false;

            }

            return true;
        }

        private DataTable RelOrcamentoToDataTable(OrcamentoDTO orcamentoDTO)
        {
            DataTable dta = new DataTable();
            DataColumn numeroNomeEmpresa = new DataColumn("numeroNomeEmpresa");
            DataColumn numeroDescricaoObra = new DataColumn("numeroDescricaoObra");
            DataColumn orcamento = new DataColumn("orcamento");
            DataColumn descricaoOrcamento = new DataColumn("descricaoOrcamento");
            DataColumn composicao = new DataColumn("composicao");
            DataColumn descricaoComposicao = new DataColumn("descricaoComposicao");
            DataColumn unidadeMedida = new DataColumn("unidadeMedida");
            DataColumn classe = new DataColumn("classe");
            DataColumn quantidade = new DataColumn("quantidade", System.Type.GetType("System.Decimal"));
            DataColumn preco = new DataColumn("preco", System.Type.GetType("System.Decimal"));
            DataColumn girErro = new DataColumn("girErro");

            dta.Columns.Add(numeroNomeEmpresa);
            dta.Columns.Add(numeroDescricaoObra);
            dta.Columns.Add(orcamento);
            dta.Columns.Add(descricaoOrcamento);
            dta.Columns.Add(composicao);
            dta.Columns.Add(descricaoComposicao);
            dta.Columns.Add(unidadeMedida);
            dta.Columns.Add(classe);
            dta.Columns.Add(quantidade);
            dta.Columns.Add(preco);
            dta.Columns.Add(girErro);

            foreach (OrcamentoComposicaoDTO orcamentoComposicao in orcamentoDTO.ListaOrcamentoComposicao)
            {
                DataRow row = dta.NewRow();
                row[numeroNomeEmpresa] = orcamentoDTO.Empresa.NumeroNomeEmpresa;
                row[numeroDescricaoObra] = orcamentoDTO.Obra.NumeroDescricao;
                row[orcamento] = orcamentoDTO.Id;
                row[descricaoOrcamento] = orcamentoDTO.Descricao;
                row[composicao] = orcamentoComposicao.Composicao.Id;
                row[descricaoComposicao] = orcamentoComposicao.Composicao.Descricao;
                row[unidadeMedida] = orcamentoComposicao.Composicao.UnidadeMedidaSigla;
                row[classe] = orcamentoComposicao.codigoClasse;
                row[quantidade] = orcamentoComposicao.Quantidade.Value;
                row[preco] = orcamentoComposicao.Preco.Value;
                row[girErro] = "";
                dta.Rows.Add(row);
            }

            return dta;
        }


        #endregion
    }
}