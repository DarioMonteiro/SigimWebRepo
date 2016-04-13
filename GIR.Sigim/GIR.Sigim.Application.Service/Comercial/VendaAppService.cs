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
using GIR.Sigim.Domain.Repository.Sigim;
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
        private IParametrosSigimRepository parametrosRepository;


        public VendaAppService(IVendaRepository vendaRepository, IParametrosSigimRepository parametrosRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.vendaRepository = vendaRepository;
            this.parametrosRepository = parametrosRepository;
        }

        #region IVendaRepositoryAppService Members


        private Specification<Venda> MontarSpecificationRelStatusVenda(RelStatusVendaFiltro filtro)
        {
            var specification = (Specification<Venda>)new TrueSpecification<Venda>();

            specification &= VendaSpecification.IgualAoIncorporadorId(filtro.IncorporadorId);
            specification &= VendaSpecification.IgualAoEmpreendimentoId(filtro.EmpreendimentoId);
            specification &= VendaSpecification.IgualAoBlocoId(filtro.BlocoId);
            specification &= VendaSpecification.EhTipoParticipanteTitular();
            if (filtro.Aprovado.HasValue)
            {
                if (filtro.Aprovado.Value == 1)
                {
                    specification &= VendaSpecification.EhAprovado();
                }
                else
                {
                    if (filtro.Aprovado.Value == 2)
                    {
                        specification &= VendaSpecification.NaoEhAprovado();
                    }
                }
            }

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
                                 (filtro.SituacaoRescindida ? VendaSpecification.EhRescindida() : new FalseSpecification<Venda>()) ||
                                 (filtro.SituacaoQuitada ? VendaSpecification.EhQuitada() : new FalseSpecification<Venda>()) ||
                                 (filtro.SituacaoEscriturada ? VendaSpecification.EhEscriturada() : new FalseSpecification<Venda>())
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

            var parametros = parametrosRepository.Obter();
            var caminhoImagem = PrepararIconeRelatorio(null, parametros);

            objRel.SetParameterValue("descricaoMoeda", "");
            objRel.SetParameterValue("caminhoImagem", caminhoImagem);

            FileDownloadDTO arquivo = new FileDownloadDTO(
                "Rel. Status da Venda",
                objRel.ExportToStream((ExportFormatType)formato),
                formato);

            if (System.IO.File.Exists(caminhoImagem)) System.IO.File.Delete(caminhoImagem);

            return arquivo;
        }
        
        #endregion

        #region Métodos Privados

        private DataTable RelStatusVendaToDataTable(List<Venda> lista)
        {
            DataTable dta = new DataTable();
            DataColumn codigoIncorporador = new DataColumn("codigoIncorporador", System.Type.GetType("System.Int64"));
            DataColumn razaoSocialIncorporador = new DataColumn("razaoSocialIncorporador");
            DataColumn codigoEmpreendimento = new DataColumn("codigoEmpreendimento", System.Type.GetType("System.Int64"));
            DataColumn nomeEmpreendimento = new DataColumn("nomeEmpreendimento");
            DataColumn codigoBloco = new DataColumn("codigoBloco", System.Type.GetType("System.Int64"));
            DataColumn nomeBloco = new DataColumn("nomeBloco");
            DataColumn codigoUnidade = new DataColumn("codigoUnidade", System.Type.GetType("System.Int64"));
            DataColumn descricaoUnidade = new DataColumn("descricaoUnidade");
            DataColumn codigoContrato = new DataColumn("codigoContrato", System.Type.GetType("System.Int64"));
            DataColumn descricaoSituacaoContrato = new DataColumn("descricaoSituacaoContrato");
            DataColumn nomeCliente = new DataColumn("nomeCliente");
            DataColumn dataVenda = new DataColumn("dataVenda", System.Type.GetType("System.DateTime"));
            DataColumn precoTabela = new DataColumn("precoTabela", System.Type.GetType("System.Decimal"));
            DataColumn valorDesconto = new DataColumn("valorDesconto", System.Type.GetType("System.Decimal"));
            DataColumn precoPraticado = new DataColumn("precoPraticado", System.Type.GetType("System.Decimal"));
            DataColumn dataAssinatura = new DataColumn("dataAssinatura", System.Type.GetType("System.DateTime"));
            DataColumn dataCancelamento = new DataColumn("dataCancelamento", System.Type.GetType("System.DateTime"));
            DataColumn descricaoTabelaVenda = new DataColumn("descricaoTabelaVenda");
            DataColumn percentualDesconto = new DataColumn("percentualDesconto", System.Type.GetType("System.Decimal"));

            dta.Columns.Add(codigoIncorporador);
            dta.Columns.Add(razaoSocialIncorporador);
            dta.Columns.Add(codigoEmpreendimento);
            dta.Columns.Add(nomeEmpreendimento);
            dta.Columns.Add(codigoBloco);
            dta.Columns.Add(nomeBloco);
            dta.Columns.Add(codigoUnidade);
            dta.Columns.Add(descricaoUnidade);
            dta.Columns.Add(codigoContrato);
            dta.Columns.Add(descricaoSituacaoContrato);
            dta.Columns.Add(nomeCliente);
            dta.Columns.Add(dataVenda);
            dta.Columns.Add(precoTabela);
            dta.Columns.Add(valorDesconto);
            dta.Columns.Add(precoPraticado);
            dta.Columns.Add(dataAssinatura);
            dta.Columns.Add(dataCancelamento);
            dta.Columns.Add(descricaoTabelaVenda);
            dta.Columns.Add(percentualDesconto);
       
            foreach (var item in lista)
            {
                DataRow row = dta.NewRow();

                VendaDTO objeto = item.To<VendaDTO>();

                row[codigoIncorporador] = objeto.Contrato.Unidade.Empreendimento.IncorporadorId;
                row[razaoSocialIncorporador] = objeto.Contrato.Unidade.Empreendimento.Incorporador.RazaoSocial;
                row[codigoEmpreendimento] = objeto.Contrato.Unidade.EmpreendimentoId;
                row[nomeEmpreendimento] = objeto.Contrato.Unidade.Empreendimento.Nome;
                row[codigoBloco] = objeto.Contrato.Unidade.BlocoId;
                row[nomeBloco] = objeto.Contrato.Unidade.Bloco.Nome;
                row[codigoUnidade] = objeto.Contrato.Unidade.Id;
                row[descricaoUnidade] = objeto.Contrato.Unidade.Descricao;
                row[codigoContrato] = objeto.Contrato.Id;
                row[descricaoSituacaoContrato] = objeto.Contrato.DescricaoSituacaoContrato;
                row[nomeCliente] = objeto.Contrato.ListaVendaParticipante.Where(l => l.TipoParticipanteId == GIR.Sigim.Domain.Constantes.Comercial.ContratoTipoParticipanteTitular).FirstOrDefault().Cliente.Nome;
                row[dataVenda] = objeto.Contrato.Venda.DataVenda;
                row[precoTabela] = objeto.Contrato.Venda.PrecoTabela;
                if (!objeto.Contrato.Venda.ValorDesconto.HasValue) row[valorDesconto] = 0M; else row[valorDesconto] = objeto.Contrato.Venda.ValorDesconto;
                row[precoPraticado] = objeto.Contrato.Venda.PrecoPraticado;
                if (!objeto.Contrato.Venda.DataAssinatura.HasValue) row[dataAssinatura] = DBNull.Value; else row[dataAssinatura] = objeto.Contrato.Venda.DataAssinatura;
                if (!objeto.Contrato.Venda.DataCancelamento.HasValue) row[dataCancelamento] = DBNull.Value; else row[dataCancelamento] = objeto.Contrato.Venda.DataCancelamento;
                row[descricaoTabelaVenda] = objeto.Contrato.Venda.TabelaVenda.Nome;
                row[percentualDesconto] = objeto.Contrato.Venda.PercentualDesconto;

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