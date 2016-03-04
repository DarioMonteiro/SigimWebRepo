using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CrystalDecisions.Shared;
using System.Web.Script.Serialization;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO;
using GIR.Sigim.Application.Reports.Comercial;
using GIR.Sigim.Domain.Repository.Admin;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Infrastructure.Crosscutting.Security;
using GIR.Sigim.Application.DTO.Comercial;
using GIR.Sigim.Domain.Entity.Comercial;
using GIR.Sigim.Domain.Repository.Comercial;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Domain.Specification.Comercial;
using GIR.Sigim.Application.Filtros.Comercial;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.Service.Comercial
{
    public class VendaAppService : BaseAppService, IVendaAppService
    {
        private IVendaRepository vendaRepository;
        //private IParametrosRepository parametrosRepository;


        public VendaAppService(IVendaRepository vendaRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.vendaRepository = vendaRepository;
        }

        #region IVendaRepositoryAppService Members


        private Specification<Venda> MontarSpecificationRelStatusVenda(RelStatusVendaFiltro filtro)
        {
            var specification = (Specification<Venda>)new TrueSpecification<Venda>();

            specification &= VendaSpecification.IgualAoIncorporadorId(filtro.IncorporadorId);
            specification &= VendaSpecification.IgualAoEmpreendimentoId(filtro.EmpreendimentoId);
            specification &= VendaSpecification.IgualAoBlocoId(filtro.BlocoId);


            if (filtro.SituacaoTodas)
            {
                specification &= (
                                 (VendaSpecification.EhProposta()) ||
                                 (VendaSpecification.EhAssinada()) ||
                                 (VendaSpecification.EhCancelada()) ||
                                 (VendaSpecification.EhRescindida()) ||
                                 (VendaSpecification.EhQuitada()) ||
                                 (VendaSpecification.EhEscriturada())
                                 );
            }
            else
            {
                specification &= (
                                 (filtro.SituacaoProposta ? VendaSpecification.EhProposta() : new FalseSpecification<Venda>()) ||
                                 (filtro.SituacaoAssinada ? VendaSpecification.EhAssinada() : new FalseSpecification<Venda>()) ||
                                 (filtro.SituacaoCancelada ? VendaSpecification.EhCancelada() : new FalseSpecification<Venda>()) ||
                                 (filtro.SituacaoCancelada ? VendaSpecification.EhRescindida() : new FalseSpecification<Venda>()) ||
                                 (filtro.SituacaoCancelada ? VendaSpecification.EhQuitada() : new FalseSpecification<Venda>()) ||
                                 (filtro.SituacaoCancelada ? VendaSpecification.EhEscriturada() : new FalseSpecification<Venda>())
                                 );
            }

            return specification;
        }

        public List<RelStatusVendaDTO> ListarPeloFiltroRelStatusVenda(RelStatusVendaFiltro filtro, out int totalRegistros)
        {
            var specification = MontarSpecificationRelStatusVenda(filtro);

            var lista = vendaRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros,
                l => l.Contrato,
                l => l.Contrato.ListaVendaParticipante.Select(c => c.Cliente),
                l => l.Contrato.Unidade, 
                l => l.Contrato.Unidade.Bloco,
                l => l.Contrato.Unidade.Empreendimento,
                l => l.Contrato.Unidade.Empreendimento.Incorporador, 
                l => l.TabelaVenda,
                l => l.IndiceFinanceiro
                );

            return lista.To<List<RelStatusVendaDTO>>();
        }

        public bool EhPermitidoImprimirRelStatusVenda()
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.RelStatusVendaImprimir))
                return false;

            return true;
        }

        public FileDownloadDTO ExportarRelStatusVenda(RelStatusVendaFiltro filtro,
                                                      int? usuarioId,
                                                      FormatoExportacaoArquivo formato)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.RelStatusVendaImprimir))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return null;
            }

            var specification = MontarSpecificationRelStatusVenda(filtro);

            var lista = vendaRepository.ListarPeloFiltro(
                specification,
                l => l.Contrato,
                l => l.Contrato.ListaVendaParticipante.Select(c => c.Cliente),
                l => l.Contrato.Unidade,
                l => l.Contrato.Unidade.Bloco,
                l => l.Contrato.Unidade.Empreendimento,
                l => l.Contrato.Unidade.Empreendimento.Incorporador,
                l => l.TabelaVenda,
                l => l.IndiceFinanceiro
                ).To<List<Venda>>();

            relStatusVenda objRel = new relStatusVenda();
            objRel.SetDataSource(RelStatusVendaToDataTable(lista));

            //var parametros = parametrosRepository.Obter();
            var caminhoImagem = PrepararIconeRelatorio(null, parametros);

            objRel.SetParameterValue("descricaoMoeda", "");
            objRel.SetParameterValue("caminhoImagem", caminhoImagem);

            FileDownloadDTO arquivo = new FileDownloadDTO(
                "Rel. Status da Venda",
                objRel.ExportToStream((ExportFormatType)formato),
                formato);

            if (System.IO.File.Exists(caminhoImagem))
                System.IO.File.Delete(caminhoImagem);
            return arquivo;
        }
        
        #endregion

        #region Métodos Privados

        private DataTable RelStatusVendaToDataTable(List<Venda> lista)
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

            foreach (var item in lista)
            {
                DataRow row = dta.NewRow();

                //row[codigo] = item.Id;
                //row[ordemCompra] = item.OrdemCompraId;
                //row[requisicaoMaterial] = item.RequisicaoMaterialItemId;
                //row[cotacaoItem] = item.CotacaoItemId;
                //row[material] = item.Material.Id;
                //row[descricaoMaterial] = item.Material.Descricao;
                //row[classe] = item.CodigoClasse + " - " + item.Classe.Descricao;
                //row[descricaoClasse] = item.Classe;
                //row[sequencial] = item.Sequencial;
                //row[complementoDescricao] = item.Complemento;
                //row[unidadeMedida] = item.Material.SiglaUnidadeMedida;
                //if (!item.Quantidade.HasValue) row[quantidade] = 0M;
                //row[quantidade] = item.Quantidade;
                //if (!item.QuantidadeEntregue.HasValue) row[quantidadeEntregue] = 0M;
                //else row[quantidadeEntregue] = item.QuantidadeEntregue;
                //if (!item.ValorUnitario.HasValue) row[valorUnitario] = 0M;
                //else row[valorUnitario] = item.ValorUnitario;
                //if (!item.PercentualIPI.HasValue) row[percentualIPI] = 0M;
                //else row[percentualIPI] = item.PercentualIPI;
                //if (!item.PercentualDesconto.HasValue) row[percentualDesconto] = 0M;
                //else row[percentualDesconto] = item.PercentualDesconto;
                //if (!item.ValorTotalComImposto.HasValue) row[valorTotalComImposto] = 0M;
                //else row[valorTotalComImposto] = item.ValorTotalComImposto;
                //if (!item.ValorTotalItem.HasValue) row[valorTotalItem] = 0M;
                //else row[valorTotalItem] = item.ValorTotalItem;
                //if (!item.OrdemCompra.PrazoEntrega.HasValue) row[prazoEntrega] = DBNull.Value;
                //else row[prazoEntrega] = item.OrdemCompra.PrazoEntrega;
                //row[situacaoOrdemCompra] = (int)item.OrdemCompra.Situacao;
                //row[descricaoSituacaoOrdemCompra] = item.OrdemCompra.Situacao.ObterDescricao();
                //row[codigoFornecedor] = item.OrdemCompra.ClienteFornecedor.Id;
                //row[nomeFornecedor] = item.OrdemCompra.ClienteFornecedor.Nome;
                //row[dataOrdemCompra] = item.OrdemCompra.Data.ToString("dd/MM/yyyy");
                //row[centroCusto] = item.OrdemCompra.CentroCusto.Codigo;
                //row[girErro] = "";

                dta.Rows.Add(row);
            }

            return dta;
        }

        #endregion
    }
}