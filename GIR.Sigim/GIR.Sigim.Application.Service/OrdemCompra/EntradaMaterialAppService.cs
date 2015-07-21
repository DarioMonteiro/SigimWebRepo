using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CrystalDecisions.Shared;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.OrdemCompra;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros.OrdemCompras;
using GIR.Sigim.Application.Reports.OrdemCompra;
using GIR.Sigim.Application.Service.Admin;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Orcamento;
using GIR.Sigim.Domain.Entity.OrdemCompra;
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
        private ICentroCustoRepository centroCustoRepository;

        public EntradaMaterialAppService(
            IEntradaMaterialRepository entradaMaterialRepository,
            IUsuarioAppService usuarioAppService,
            IParametrosOrdemCompraRepository parametrosOrdemCompraRepository,
            ICentroCustoRepository centroCustoRepository,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.entradaMaterialRepository = entradaMaterialRepository;
            this.usuarioAppService = usuarioAppService;
            this.parametrosOrdemCompraRepository = parametrosOrdemCompraRepository;
            this.centroCustoRepository = centroCustoRepository;
        }

        #region IEntradaMaterialAppService Members

        public List<EntradaMaterialDTO> ListarPeloFiltro(EntradaMaterialFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<EntradaMaterial>)new TrueSpecification<EntradaMaterial>();

            if (usuarioAppService.UsuarioPossuiCentroCustoDefinidoNoModulo(UsuarioLogado.Id, Resource.Sigim.NomeModulo.OrdemCompra))
                specification &= EntradaMaterialSpecification.UsuarioPossuiAcessoAoCentroCusto(UsuarioLogado.Id, Resource.Sigim.NomeModulo.OrdemCompra);

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
                l => l.ListaItens,
                l => l.ListaFormaPagamento.Select(o => o.TituloPagar),
                l => l.ListaImposto,
                l => l.ListaMovimentoEstoque).To<EntradaMaterialDTO>();
        }

        public bool CancelarEntrada(int? id, string motivo)
        {
            if (string.IsNullOrEmpty(motivo.Trim()))
            {
                messageQueue.Add(Resource.OrdemCompra.ErrorMessages.InformeMotivoCancelamentoEntradaMaterial, TypeMessage.Error);
                return false;
            }

            var entradaMaterial = ObterPeloIdEUsuario(id, UsuarioLogado.Id,
                l => l.ListaItens,
                l => l.ListaFormaPagamento.Select(o => o.TituloPagar),
                l => l.ListaImposto,
                l => l.ListaMovimentoEstoque);

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

            foreach (var formaPagamento in entradaMaterial.ListaFormaPagamento.To<List<EntradaMaterialFormaPagamentoDTO>>())
            {
                int? tituloPagarId;

                if (PossuiTituloPago(formaPagamento.TituloPagar, out tituloPagarId))
                {
                    var msg = string.Format(Resource.OrdemCompra.ErrorMessages.TituloEstaPago, tituloPagarId.ToString());
                    messageQueue.Add(msg, TypeMessage.Error);
                    return false;
                }

                if (PossuiTituloImpostoPago(formaPagamento.TituloPagar, out tituloPagarId))
                {
                    var msg = string.Format(Resource.OrdemCompra.ErrorMessages.TituloImpostoEstaPago, tituloPagarId.ToString());
                    messageQueue.Add(msg, TypeMessage.Error);
                    return false;
                }
            }

            entradaMaterial.MotivoCancelamento = motivo;
            entradaMaterial.Situacao = SituacaoEntradaMaterial.Cancelada;
            entradaMaterial.DataCancelamento = DateTime.Now;
            entradaMaterial.LoginUsuarioCancelamento = UsuarioLogado.Login;

            //TODO: Efetuar todos os passos da PROCEDURE [OrdemCompra].[entradaMaterial_Cancela]

            entradaMaterialRepository.Alterar(entradaMaterial);
            entradaMaterialRepository.UnitOfWork.Commit();
            messageQueue.Add(Resource.OrdemCompra.SuccessMessages.CancelamentoComSucesso, TypeMessage.Success);
            return true;
        }

        public FileDownloadDTO Exportar(int? id, FormatoExportacaoArquivo formato)
        {
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

            var parametros = parametrosOrdemCompraRepository.Obter();
            var centroCusto = centroCustoRepository.ObterPeloCodigo(entradaMaterial.CodigoCentroCusto, l => l.ListaCentroCustoEmpresa);

            var caminhoImagem = PrepararIconeRelatorio(centroCusto, parametros);
            objRel.SetParameterValue("caminhoImagem", caminhoImagem);

            var nomeEmpresa = ObterNomeEmpresa(centroCusto, parametros);
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
            return PodeSerSalvaNaSituacaoAtual(dto.Situacao);
        }

        public bool EhPermitidoCancelar(EntradaMaterialDTO dto)
        {
            if (!dto.Id.HasValue)
                return false;

            if (!PodeCancelarNaSituacaoAtual(dto.Situacao))
                return false;

            if (JaHouvePagamentoEfetuado(dto))
                return false;

            return true;
        }

        public bool EhPermitidoImprimir(EntradaMaterialDTO dto)
        {
            if (!dto.Id.HasValue)
                return false;

            return true;
        }

        public bool ExisteMovimentoNoEstoque(EntradaMaterialDTO dto)
        {
            return dto.ListaMovimentoEstoque.Any();
        }

        #endregion

        #region Métodos Privados

        private EntradaMaterial ObterPeloIdEUsuario(int? id, int? idUsuario, params Expression<Func<EntradaMaterial, object>>[] includes)
        {
            var specification = (Specification<EntradaMaterial>)new TrueSpecification<EntradaMaterial>();

            if (usuarioAppService.UsuarioPossuiCentroCustoDefinidoNoModulo(idUsuario, Resource.Sigim.NomeModulo.OrdemCompra))
                specification &= EntradaMaterialSpecification.UsuarioPossuiAcessoAoCentroCusto(idUsuario, Resource.Sigim.NomeModulo.OrdemCompra);

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

        private bool JaHouvePagamentoEfetuado(EntradaMaterialDTO dto)
        {
            foreach (var formaPagamento in dto.ListaFormaPagamento)
            {
                int? tituloPagarId;

                if (PossuiTituloPago(formaPagamento.TituloPagar, out tituloPagarId))
                    return true;

                if (PossuiTituloImpostoPago(formaPagamento.TituloPagar, out tituloPagarId))
                    return true;
            }

            return false;
        }

        private bool PossuiTituloImpostoPago(TituloPagarDTO tituloPagar, out int? tituloPagarId)
        {
            tituloPagarId = null;
            if (tituloPagar != null)
            {
                foreach (var item in tituloPagar.ListaImpostoPagar)
                {
                    if (item.TituloPagar.TipoTitulo == TipoTitulo.Pai)
                    {
                        if (PossuiTituloDesdobradoPago(item.TituloPagar.ListaFilhos, out tituloPagarId))
                            return true;
                    }
                    else if (EhTituloPago(item.TituloPagar, out tituloPagarId))
                        return true;
                }
            }

            return false;
        }

        private bool PossuiTituloPago(TituloPagarDTO tituloPagar, out int? tituloPagarId)
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

        private bool PossuiTituloDesdobradoPago(List<TituloPagarDTO> listaFilhos, out int? tituloPagarId)
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

        private static bool EhTituloPago(TituloPagarDTO titulo, out int? tituloPagarId)
        {
            tituloPagarId = null;
            if ((titulo.Situacao == SituacaoTituloPagar.Emitido)
                || (titulo.Situacao == SituacaoTituloPagar.Pago)
                || (titulo.Situacao == SituacaoTituloPagar.Baixado))
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

            foreach (var item in listaImposto)
            {
                DataRow row = dta.NewRow();
                row[codigo] = item.Id.Value;
                row[baseCalculo] = item.BaseCalculo;
                row[valorImposto] = item.Valor;
                row[dataVencimento] = item.DataVencimento.HasValue ? item.DataVencimento.Value.ToString("dd/MM/yyyy") : string.Empty;
                row[TituloPagarImposto] = item.TituloPagar;
                row[sigla] = item.ImpostoFinanceiro.Sigla;
                row[descricao] = item.ImpostoFinanceiro.Descricao;
                row[aliquota] = item.ImpostoFinanceiro.Aliquota;
                dta.Rows.Add(row);
            }

            return dta;
        }

        #endregion
    }
}