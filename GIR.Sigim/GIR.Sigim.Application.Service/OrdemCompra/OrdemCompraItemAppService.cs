using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalDecisions.Shared;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Application.DTO.OrdemCompra;
using GIR.Sigim.Application.Filtros.OrdemCompras;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Domain.Repository.OrdemCompra;
using GIR.Sigim.Application.Service.Admin;
using GIR.Sigim.Domain.Specification.OrdemCompra;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Reports.OrdemCompra;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Application.Constantes;

namespace GIR.Sigim.Application.Service.OrdemCompra
{
    public class OrdemCompraItemAppService: BaseAppService, IOrdemCompraItemAppService
    {

        #region Declaração
            private IUsuarioAppService usuarioAppService;
            private IParametrosOrdemCompraRepository parametrosOrdemCompraRepository;
            private IOrdemCompraItemRepository ordemCompraItemRepository;
            private ICentroCustoRepository centroCustoRepository;
        #endregion

        #region Construtor

        public OrdemCompraItemAppService(IUsuarioAppService usuarioAppService,
                                         IParametrosOrdemCompraRepository parametrosOrdemCompraRepository,
                                         IOrdemCompraItemRepository ordemCompraItemRepository,
                                         ICentroCustoRepository centroCustoRepository,
                                         MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.usuarioAppService = usuarioAppService;
            this.parametrosOrdemCompraRepository = parametrosOrdemCompraRepository;
            this.ordemCompraItemRepository = ordemCompraItemRepository;
            this.centroCustoRepository = centroCustoRepository;
        }

        #endregion

        #region Métodos IOrdemCompraItemAppService

        public List<RelOCItensOrdemCompraDTO> ListarPeloFiltroRelOCItensOrdemCompra(RelOcItensOrdemCompraFiltro filtro, int? usuarioId, out int totalRegistros)
        {
            var specification = MontarSpecificationRelOCItensOrdemCompra(filtro, usuarioId);

            var listaOrdemCompraItem =
             ordemCompraItemRepository.ListarPeloFiltroComPaginacao(specification,
                                                                    filtro.PaginationParameters.PageIndex,
                                                                    filtro.PaginationParameters.PageSize,
                                                                    filtro.PaginationParameters.OrderBy,
                                                                    filtro.PaginationParameters.Ascending,
                                                                    out totalRegistros,
                                                                    l => l.Classe,
                                                                    l => l.OrdemCompra.CentroCusto,
                                                                    l => l.OrdemCompra.ClienteFornecedor,
                                                                    l => l.Material.MaterialClasseInsumo).To<List<OrdemCompraItem>>();
            List<RelOCItensOrdemCompraDTO> listaRelOCItensOrdemCompra = new List<RelOCItensOrdemCompraDTO>();
            foreach (var item in listaOrdemCompraItem)
            {
                RelOCItensOrdemCompraDTO relat = new RelOCItensOrdemCompraDTO();

                relat.OrdemCompra = item.OrdemCompra.To<OrdemCompraDTO>();
                relat.OrdemCompraItem = item.To<OrdemCompraItemDTO>();

                listaRelOCItensOrdemCompra.Add(relat);
            }

            return listaRelOCItensOrdemCompra;

        }

        private Specification<OrdemCompraItem> MontarSpecificationRelOCItensOrdemCompra(RelOcItensOrdemCompraFiltro filtro, int? usuarioId)
        {
            var specification = (Specification<OrdemCompraItem>)new TrueSpecification<OrdemCompraItem>();

            if (usuarioAppService.UsuarioPossuiCentroCustoDefinidoNoModulo(usuarioId, Resource.Sigim.NomeModulo.OrdemCompra))
                specification &= OrdemCompraItemSpecification.UsuarioPossuiAcessoAoCentroCusto(usuarioId, Resource.Sigim.NomeModulo.OrdemCompra);

            if (filtro.OrdemCompra.Id.HasValue)
                specification &= OrdemCompraItemSpecification.PertenceAhOrdemCompraId(filtro.OrdemCompra.Id);
            else
            {
                specification &= OrdemCompraItemSpecification.DataOrdemCompraMaiorOuIgual(filtro.DataInicial);
                specification &= OrdemCompraItemSpecification.DataOrdemCompraMenorOuIgual(filtro.DataFinal);
                specification &= OrdemCompraItemSpecification.PertenceAoCentroCustoIniciadoPor(filtro.CentroCusto.Codigo);
                specification &= OrdemCompraItemSpecification.PertenceAhClasseIniciadaPor(filtro.Classe.Codigo);
                specification &= OrdemCompraItemSpecification.PertenceAhClasseInsumoIniciadaPor(filtro.ClasseInsumo.Codigo);
                specification &= OrdemCompraItemSpecification.ClienteFornecedorPertenceAhItemOC(filtro.ClienteFornecedor.Id);
                specification &= OrdemCompraItemSpecification.PertenceMaterialId(filtro.Material.Id);
                specification &= OrdemCompraItemSpecification.EhExibirSomenteComSaldo(filtro.EhExibirSomentecomSaldo);

                if (filtro.EhFechada || filtro.EhLiberada || filtro.EhPendente)
                {
                    specification &= ((filtro.EhFechada ? OrdemCompraItemSpecification.EhFechada() : new FalseSpecification<OrdemCompraItem>())
                        || ((filtro.EhLiberada) ? OrdemCompraItemSpecification.EhLiberada() : new FalseSpecification<OrdemCompraItem>())
                        || ((filtro.EhPendente) ? OrdemCompraItemSpecification.EhPendente() : new FalseSpecification<OrdemCompraItem>()));
                }

            }

            return specification;
        }

        public FileDownloadDTO ExportarRelOCItensOrdemCompra(RelOcItensOrdemCompraFiltro filtro,
                                                             int? usuarioId,
                                                             FormatoExportacaoArquivo formato)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.RelatorioItensOrdemCompraImprimir))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return null;
            }

            var specification = MontarSpecificationRelOCItensOrdemCompra(filtro, usuarioId);

            var listaOrdemCompraItem =
             ordemCompraItemRepository.ListarPeloFiltro(specification,
                                                        l => l.Classe,
                                                        l => l.OrdemCompra.CentroCusto,
                                                        l => l.OrdemCompra.ClienteFornecedor,
                                                        l => l.Material.MaterialClasseInsumo).To<List<OrdemCompraItem>>();

            relOrdemCompraItem objRel = new relOrdemCompraItem();

            objRel.SetDataSource(RelItensOrdemCompraToDataTable(listaOrdemCompraItem));

            string periodo = " Período de: " + filtro.DataInicial.Value.ToString("dd/MM/yyyy") + " até " + filtro.DataFinal.Value.ToString("dd/MM/yyyy");

            string parametroInsumo = "Insumo: ";
            if (filtro.Material.Id.HasValue)
            {
                parametroInsumo += filtro.Material.Id.ToString() + " - " + filtro.Material.Descricao;
            }
            else{
                parametroInsumo += "Todos";
            }

            var parametros = parametrosOrdemCompraRepository.Obter();
            var centroCusto = centroCustoRepository.ObterPeloCodigo(filtro.CentroCusto.Codigo, l => l.ListaCentroCustoEmpresa);
            string parametroCentroCusto = "Centro de Custo: ";
            if (centroCusto != null){
                parametroCentroCusto += centroCusto.Codigo + " - " + centroCusto.Descricao;
            }
            else{
                parametroCentroCusto += "Todos";
            }

            var caminhoImagem = PrepararIconeRelatorio(centroCusto, parametros);

            var nomeEmpresa = ObterNomeEmpresa(centroCusto, parametros);
            objRel.SetParameterValue("nomeEmpresa", nomeEmpresa);
            objRel.SetParameterValue("parCentroCusto", parametroCentroCusto);
            objRel.SetParameterValue("parPeriodo", periodo);
            objRel.SetParameterValue("parInsumo", parametroInsumo);
            objRel.SetParameterValue("caminhoImagem", caminhoImagem);

            FileDownloadDTO arquivo = new FileDownloadDTO(
                "Rel. Itens ordem de compra",
                objRel.ExportToStream((ExportFormatType)formato),
                formato);

            if (System.IO.File.Exists(caminhoImagem))
                System.IO.File.Delete(caminhoImagem);
            return arquivo;
        }

        public bool EhPermitidoImprimir()
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.RelatorioItensOrdemCompraImprimir))
                return false;

            return true;
        }


        #endregion

        #region Métodos privados

        private DataTable RelItensOrdemCompraToDataTable(List<OrdemCompraItem> listaOrdemCompraItem)
        {
            DataTable dta = new DataTable();
            DataColumn codigo = new DataColumn("codigo", System.Type.GetType("System.Int32"));
            DataColumn ordemCompra = new DataColumn("ordemCompra", System.Type.GetType("System.Int32"));
            DataColumn requisicaoMaterial = new DataColumn("requisicaoMaterial");
            DataColumn cotacaoItem = new DataColumn("cotacaoItem");
            DataColumn material = new DataColumn("material", System.Type.GetType("System.Int32"));
            DataColumn descricaoMaterial = new DataColumn("descricaoMaterial");
            DataColumn classe = new DataColumn("classe");
            DataColumn descricaoClasse = new DataColumn("descricaoClasse");
            DataColumn sequencial = new DataColumn("sequencial");
            DataColumn complementoDescricao = new DataColumn("complementoDescricao");
            DataColumn unidadeMedida = new DataColumn("unidadeMedida");
            DataColumn quantidade = new DataColumn("quantidade", System.Type.GetType("System.Decimal"));
            DataColumn quantidadeEntregue = new DataColumn("quantidadeEntregue", System.Type.GetType("System.Decimal"));
            DataColumn valorUnitario = new DataColumn("valorUnitario", System.Type.GetType("System.Decimal"));
            DataColumn percentualIPI = new DataColumn("percentualIPI", System.Type.GetType("System.Decimal"));
            DataColumn percentualDesconto = new DataColumn("percentualDesconto", System.Type.GetType("System.Decimal"));
            DataColumn valorTotalComImposto = new DataColumn("valorTotalComImposto", System.Type.GetType("System.Decimal"));
            DataColumn valorTotalItem = new DataColumn("valorTotalItem", System.Type.GetType("System.Decimal"));
            DataColumn prazoEntrega = new DataColumn("prazoEntrega", System.Type.GetType("System.Decimal"));
            DataColumn situacaoOrdemCompra = new DataColumn("situacaoOrdemCompra");
            DataColumn descricaoSituacaoOrdemCompra = new DataColumn("descricaoSituacaoOrdemCompra");
            DataColumn codigoFornecedor = new DataColumn("codigoFornecedor");
            DataColumn nomeFornecedor = new DataColumn("nomeFornecedor");
            DataColumn dataOrdemCompra = new DataColumn("dataOrdemCompra", System.Type.GetType("System.DateTime"));
            DataColumn centroCusto = new DataColumn("centroCusto");
            DataColumn girErro = new DataColumn("girErro");


            dta.Columns.Add(codigo);
            dta.Columns.Add(ordemCompra);
            dta.Columns.Add(requisicaoMaterial);
            dta.Columns.Add(cotacaoItem);
            dta.Columns.Add(material);
            dta.Columns.Add(descricaoMaterial);
            dta.Columns.Add(classe);
            dta.Columns.Add(descricaoClasse);
            dta.Columns.Add(sequencial);
            dta.Columns.Add(complementoDescricao);
            dta.Columns.Add(unidadeMedida);
            dta.Columns.Add(quantidade);
            dta.Columns.Add(quantidadeEntregue);
            dta.Columns.Add(valorUnitario);
            dta.Columns.Add(percentualIPI);
            dta.Columns.Add(percentualDesconto);
            dta.Columns.Add(valorTotalComImposto);
            dta.Columns.Add(valorTotalItem);
            dta.Columns.Add(prazoEntrega);
            dta.Columns.Add(situacaoOrdemCompra);
            dta.Columns.Add(descricaoSituacaoOrdemCompra);
            dta.Columns.Add(codigoFornecedor);
            dta.Columns.Add(nomeFornecedor);
            dta.Columns.Add(dataOrdemCompra);
            dta.Columns.Add(centroCusto);
            dta.Columns.Add(girErro);

            foreach (var itemOC in listaOrdemCompraItem)
            {
                DataRow row = dta.NewRow();

                row[codigo] = itemOC.Id;
                row[ordemCompra] = itemOC.OrdemCompraId;
                row[requisicaoMaterial] = itemOC.RequisicaoMaterialItemId;
                row[cotacaoItem] = itemOC.CotacaoItemId;
                row[material] = itemOC.Material.Id;
                row[descricaoMaterial] = itemOC.Material.Descricao;
                row[classe] = itemOC.CodigoClasse + " - " + itemOC.Classe.Descricao;
                row[descricaoClasse] = itemOC.Classe;
                row[sequencial] = itemOC.Sequencial;
                row[complementoDescricao] = itemOC.Complemento;
                row[unidadeMedida] = itemOC.Material.SiglaUnidadeMedida;
                if (!itemOC.Quantidade.HasValue) row[quantidade] = 0M;
                row[quantidade] = itemOC.Quantidade;
                if (!itemOC.QuantidadeEntregue.HasValue) row[quantidadeEntregue] = 0M;
                else row[quantidadeEntregue] = itemOC.QuantidadeEntregue;
                if (!itemOC.ValorUnitario.HasValue) row[valorUnitario] = 0M;
                else row[valorUnitario] = itemOC.ValorUnitario;
                if (!itemOC.PercentualIPI.HasValue) row[percentualIPI] = 0M;
                else row[percentualIPI] = itemOC.PercentualIPI;
                if (!itemOC.PercentualDesconto.HasValue) row[percentualDesconto] = 0M;
                else row[percentualDesconto] = itemOC.PercentualDesconto;
                if (!itemOC.ValorTotalComImposto.HasValue) row[valorTotalComImposto] = 0M;
                else row[valorTotalComImposto] = itemOC.ValorTotalComImposto;
                if (!itemOC.ValorTotalItem.HasValue) row[valorTotalItem] = 0M;
                else row[valorTotalItem] = itemOC.ValorTotalItem;
                if (!itemOC.OrdemCompra.PrazoEntrega.HasValue) row[prazoEntrega] = DBNull.Value;
                else row[prazoEntrega] = itemOC.OrdemCompra.PrazoEntrega;
                row[situacaoOrdemCompra] = (int)itemOC.OrdemCompra.Situacao;
                row[descricaoSituacaoOrdemCompra] = itemOC.OrdemCompra.Situacao.ObterDescricao();
                row[codigoFornecedor] = itemOC.OrdemCompra.ClienteFornecedor.Id;
                row[nomeFornecedor] = itemOC.OrdemCompra.ClienteFornecedor.Nome;
                row[dataOrdemCompra] = itemOC.OrdemCompra.Data.ToString("dd/MM/yyyy");
                row[centroCusto] = itemOC.OrdemCompra.CentroCusto.Codigo;
                row[girErro] = "";

                dta.Rows.Add(row);
            }

            return dta;
        }

        #endregion

    }

}
