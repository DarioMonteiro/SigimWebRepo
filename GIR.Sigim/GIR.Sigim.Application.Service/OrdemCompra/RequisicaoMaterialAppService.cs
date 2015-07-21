using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CrystalDecisions.Shared;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.OrdemCompra;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros.OrdemCompras;
using GIR.Sigim.Application.Reports.OrdemCompra;
using GIR.Sigim.Application.Service.Admin;
using GIR.Sigim.Application.Service.Financeiro;
using GIR.Sigim.Application.Service.Sigim;
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
    public class RequisicaoMaterialAppService : BaseAppService, IRequisicaoMaterialAppService
    {
        private IRequisicaoMaterialRepository requisicaoMaterialRepository;
        private IUsuarioAppService usuarioAppService;
        private IParametrosOrdemCompraRepository parametrosOrdemCompraRepository;
        private ILogOperacaoAppService logOperacaoAppService;
        private ICentroCustoRepository centroCustoRepository;

        public RequisicaoMaterialAppService(
            IRequisicaoMaterialRepository requisicaoMaterialRepository,
            IUsuarioAppService usuarioAppService,
            IParametrosOrdemCompraRepository parametrosOrdemCompraRepository,
            ILogOperacaoAppService logOperacaoAppService,
            ICentroCustoRepository centroCustoRepository,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.requisicaoMaterialRepository = requisicaoMaterialRepository;
            this.usuarioAppService = usuarioAppService;
            this.parametrosOrdemCompraRepository = parametrosOrdemCompraRepository;
            this.logOperacaoAppService = logOperacaoAppService;
            this.centroCustoRepository = centroCustoRepository;
        }

        #region IRequisicaoMaterialAppService Members

        public List<RequisicaoMaterialDTO> ListarPeloFiltro(RequisicaoMaterialFiltro filtro, int? idUsuario, out int totalRegistros)
        {
            var specification = (Specification<RequisicaoMaterial>)new TrueSpecification<RequisicaoMaterial>();

            if (usuarioAppService.UsuarioPossuiCentroCustoDefinidoNoModulo(idUsuario, Resource.Sigim.NomeModulo.OrdemCompra))
                specification &= RequisicaoMaterialSpecification.UsuarioPossuiAcessoAoCentroCusto(idUsuario, Resource.Sigim.NomeModulo.OrdemCompra);

            if (filtro.Id.HasValue)
                specification &= RequisicaoMaterialSpecification.MatchingId(filtro.Id);
            else
            {
                specification &= RequisicaoMaterialSpecification.DataMaiorOuIgual(filtro.DataInicial);
                specification &= RequisicaoMaterialSpecification.DataMenorOuIgual(filtro.DataFinal);
                specification &= RequisicaoMaterialSpecification.PertenceAoCentroCustoIniciadoPor(filtro.CentroCusto.Codigo);

                if (filtro.EhAprovada || filtro.EhCancelada || filtro.EhFechada || filtro.EhRequisitada)
                {
                    specification &= ((filtro.EhAprovada ? RequisicaoMaterialSpecification.EhAprovada() : new FalseSpecification<RequisicaoMaterial>())
                        || ((filtro.EhCancelada) ? RequisicaoMaterialSpecification.EhCancelada() : new FalseSpecification<RequisicaoMaterial>())
                        || ((filtro.EhFechada) ? RequisicaoMaterialSpecification.EhFechada() : new FalseSpecification<RequisicaoMaterial>())
                        || ((filtro.EhRequisitada) ? RequisicaoMaterialSpecification.EhRequisitada() : new FalseSpecification<RequisicaoMaterial>()));
                }
            }

            return requisicaoMaterialRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros,
                l => l.ListaItens.Select(c => c.RequisicaoMaterial)).To<List<RequisicaoMaterialDTO>>();
        }

        public RequisicaoMaterialDTO ObterPeloId(int? id)
        {
            return ObterPeloIdEUsuario(id,
                UsuarioLogado.Id,
                l => l.ListaItens.Select(s => s.ListaCotacaoItem),
                l => l.ListaItens.Select(s => s.ListaOrdemCompraItem),
                l => l.ListaItens.Select(s => s.Material.MaterialClasseInsumo),
                l => l.ListaItens.Select(s => s.ListaOrcamentoInsumoRequisitado.Select(o => o.Composicao)),
                l => l.ListaItens.Select(s => s.ListaOrcamentoInsumoRequisitado.Select(o => o.Material))).To<RequisicaoMaterialDTO>();
        }

        public bool Salvar(RequisicaoMaterialDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var requisicaoMaterial = requisicaoMaterialRepository.ObterPeloId(dto.Id, l => l.ListaItens.Select(o => o.ListaOrcamentoInsumoRequisitado));
            if (requisicaoMaterial == null)
            {
                requisicaoMaterial = new RequisicaoMaterial();
                requisicaoMaterial.Situacao = SituacaoRequisicaoMaterial.Requisitada;
                requisicaoMaterial.DataCadastro = DateTime.Now;
                requisicaoMaterial.LoginUsuarioCadastro = UsuarioLogado.Login;
                novoItem = true;
            }

            if (!PodeSerSalvaNaSituacaoAtual(requisicaoMaterial.Situacao))
            {
                var msg = string.Format(Resource.OrdemCompra.ErrorMessages.RequisicaoSituacaoInvalida, requisicaoMaterial.Situacao.ObterDescricao());
                messageQueue.Add(msg, TypeMessage.Error);
                return false;
            }

            requisicaoMaterial.Data = dto.Data;
            requisicaoMaterial.CodigoCentroCusto = dto.CentroCusto.Codigo;
            requisicaoMaterial.Observacao = dto.Observacao;
            ProcessarItens(dto, requisicaoMaterial);

            AjustarSituacaoRequisicao(requisicaoMaterial);

            if (Validator.IsValid(requisicaoMaterial, out validationErrors))
            {
                try
                {
                    if (novoItem)
                        requisicaoMaterialRepository.Inserir(requisicaoMaterial);
                    else
                        requisicaoMaterialRepository.Alterar(requisicaoMaterial);

                    requisicaoMaterialRepository.UnitOfWork.Commit();
                    GravarLogOperacao(requisicaoMaterial, novoItem ? "INSERT" : "UPDATE");

                    dto.Id = requisicaoMaterial.Id;
                    messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                    return true;
                }
                catch (Exception exception)
                {
                    QueueExeptionMessages(exception);
                }
            }
            else
                messageQueue.AddRange(validationErrors, TypeMessage.Error);

            return false;
        }

        public bool Aprovar(int? id)
        {
            var requisicaoMaterial = ObterPeloIdEUsuario(id, UsuarioLogado.Id);

            if (requisicaoMaterial == null)
            {
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
                return false;
            }

            if (!PodeAprovarNaSituacaoAtual(requisicaoMaterial.Situacao))
            {
                var msg = string.Format(Resource.OrdemCompra.ErrorMessages.RequisicaoSituacaoInvalida, requisicaoMaterial.Situacao.ObterDescricao());
                messageQueue.Add(msg, TypeMessage.Error);
                return false;
            }

            requisicaoMaterial.Situacao = SituacaoRequisicaoMaterial.Aprovada;
            requisicaoMaterial.DataAprovacao = DateTime.Now;
            requisicaoMaterial.LoginUsuarioAprovacao = UsuarioLogado.Login;

            requisicaoMaterialRepository.Alterar(requisicaoMaterial);
            requisicaoMaterialRepository.UnitOfWork.Commit();
            //TODO: Enviar e-mail
            messageQueue.Add(Resource.OrdemCompra.SuccessMessages.AprovacaoRealizadaComSucesso, TypeMessage.Success);
            return true;
        }

        public bool CancelarAprovacao(int? id)
        {
            var requisicaoMaterial = ObterPeloIdEUsuario(id, UsuarioLogado.Id);

            if (requisicaoMaterial == null)
            {
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
                return false;
            }

            if (!PodeCancelarAprovacaoNaSituacaoAtual(requisicaoMaterial.Situacao))
            {
                var msg = string.Format(Resource.OrdemCompra.ErrorMessages.RequisicaoSituacaoInvalida, requisicaoMaterial.Situacao.ObterDescricao());
                messageQueue.Add(msg, TypeMessage.Error);
                return false;
            }

            if (!PodeCancelarComItensAtuais(requisicaoMaterial.ListaItens.To<List<RequisicaoMaterialItemDTO>>()))
            {
                messageQueue.Add(Resource.OrdemCompra.ErrorMessages.RequisicaoComItensNaoRequisitados, TypeMessage.Error);
                return false;
            }

            requisicaoMaterial.Situacao = SituacaoRequisicaoMaterial.Requisitada;

            requisicaoMaterialRepository.Alterar(requisicaoMaterial);
            requisicaoMaterialRepository.UnitOfWork.Commit();
            messageQueue.Add(Resource.OrdemCompra.SuccessMessages.AprovacaoCanceladaComSucesso, TypeMessage.Success);
            return true;
        }

        public bool CancelarRequisicao(int? id, string motivo)
        {
            if (string.IsNullOrEmpty(motivo.Trim()))
            {
                messageQueue.Add(Resource.OrdemCompra.ErrorMessages.InformeMotivoCancelamentoRequisicao, TypeMessage.Error);
                return false;
            }

            var requisicaoMaterial = ObterPeloIdEUsuario(id, UsuarioLogado.Id, l => l.ListaItens.Select(o => o.ListaOrcamentoInsumoRequisitado));

            if (requisicaoMaterial == null)
            {
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
                return false;
            }

            if (!PodeCancelarNaSituacaoAtual(requisicaoMaterial.Situacao))
            {
                var msg = string.Format(Resource.OrdemCompra.ErrorMessages.PreRequisicaoSituacaoInvalida, requisicaoMaterial.Situacao.ObterDescricao());
                messageQueue.Add(msg, TypeMessage.Error);
                return false;
            }

            if (!PodeCancelarComItensAtuais(requisicaoMaterial.ListaItens.To<List<RequisicaoMaterialItemDTO>>()))
            {
                messageQueue.Add(Resource.OrdemCompra.ErrorMessages.RequisicaoComItensNaoRequisitados, TypeMessage.Error);
                return false;
            }

            requisicaoMaterial.MotivoCancelamento = motivo;
            requisicaoMaterial.Situacao = SituacaoRequisicaoMaterial.Cancelada;
            requisicaoMaterial.DataCancelamento = DateTime.Now;
            requisicaoMaterial.LoginUsuarioCancelamento = UsuarioLogado.Login;
            foreach (var item in requisicaoMaterial.ListaItens)
            {
                item.Situacao = SituacaoRequisicaoMaterialItem.Cancelado;
                item.ListaOrcamentoInsumoRequisitado.Clear();
            }

            requisicaoMaterialRepository.Alterar(requisicaoMaterial);
            requisicaoMaterialRepository.UnitOfWork.Commit();
            messageQueue.Add(Resource.OrdemCompra.SuccessMessages.CancelamentoComSucesso, TypeMessage.Success);
            return true;
        }

        public FileDownloadDTO Exportar(int? id, FormatoExportacaoArquivo formato)
        {
            var requisicao = ObterPeloIdEUsuario(id, UsuarioLogado.Id, l => l.ListaItens.Select(o => o.Material.MaterialClasseInsumo));
            relRequisicaoMaterial objRel = new relRequisicaoMaterial();
            objRel.SetDataSource(RequisicaoToDataTable(requisicao));
            objRel.Subreports["requisicaoMaterialItem"].SetDataSource(RequisicaoItemToDataTable(requisicao.ListaItens.ToList()));

            var parametros = parametrosOrdemCompraRepository.Obter();
            var centroCusto = centroCustoRepository.ObterPeloCodigo(requisicao.CodigoCentroCusto, l => l.ListaCentroCustoEmpresa);

            var caminhoImagem = PrepararIconeRelatorio(centroCusto, parametros);
            objRel.SetParameterValue("caminhoImagem", caminhoImagem);

            var nomeEmpresa = ObterNomeEmpresa(centroCusto, parametros);
            objRel.SetParameterValue("nomeEmpresa", nomeEmpresa);

            FileDownloadDTO arquivo = new FileDownloadDTO(
                "RequisicaoMaterial_" + id.ToString(),
                objRel.ExportToStream((ExportFormatType)formato),
                formato);

            RemoverIconeRelatorio(caminhoImagem);

            return arquivo;
        }

        public bool EhPermitidoSalvar(RequisicaoMaterialDTO dto)
        {
            return PodeSerSalvaNaSituacaoAtual(dto.Situacao);
        }

        public bool EhPermitidoCancelar(RequisicaoMaterialDTO dto)
        {
            if (!dto.Id.HasValue)
                return false;

            if (!PodeCancelarNaSituacaoAtual(dto.Situacao))
                return false;

            if (!PodeCancelarComItensAtuais(dto.ListaItens))
                return false;

            return true;
        }

        public bool EhPermitidoImprimir(RequisicaoMaterialDTO dto)
        {
            if (!dto.Id.HasValue)
                return false;

            if (!dto.ListaItens.Any())
                return false;

            return true;
        }

        public bool EhPermitidoAdicionarItem(RequisicaoMaterialDTO dto)
        {
            if (!EhPermitidoSalvar(dto))
                return false;

            if (!PodeAdicionarItemNaSituacaoAtual(dto.Situacao))
                return false;

            return true;
        }

        public bool EhPermitidoCancelarItem(RequisicaoMaterialDTO dto)
        {
            return EhPermitidoSalvar(dto);
        }

        public bool EhPermitidoEditarItem(RequisicaoMaterialDTO dto)
        {
            return EhPermitidoSalvar(dto);
        }

        public bool EhPermitidoAprovarRequisicao(RequisicaoMaterialDTO dto)
        {
            if (!dto.Id.HasValue)
                return false;

            if (!PodeAprovarNaSituacaoAtual(dto.Situacao))
                return false;

            if (!dto.ListaItens.Any())
                return false;

            return true;
        }

        public bool EhPermitidoCancelarAprovacao(RequisicaoMaterialDTO dto)
        {
            if (!dto.Id.HasValue)
                return false;

            if (!PodeCancelarAprovacaoNaSituacaoAtual(dto.Situacao))
                return false;

            if (!PodeCancelarComItensAtuais(dto.ListaItens))
                return false;

            return true;
        }

        public bool EhPermitidoEditarCentroCusto(RequisicaoMaterialDTO dto)
        {
            if (dto.Id.HasValue)
                return false;

            return true;
        }

        #endregion

        #region Métodos Privados

        private RequisicaoMaterial ObterPeloIdEUsuario(int? id, int? idUsuario, params Expression<Func<RequisicaoMaterial, object>>[] includes)
        {
            var specification = (Specification<RequisicaoMaterial>)new TrueSpecification<RequisicaoMaterial>();

            if (usuarioAppService.UsuarioPossuiCentroCustoDefinidoNoModulo(idUsuario, Resource.Sigim.NomeModulo.OrdemCompra))
                specification &= RequisicaoMaterialSpecification.UsuarioPossuiAcessoAoCentroCusto(idUsuario, Resource.Sigim.NomeModulo.OrdemCompra);

            return requisicaoMaterialRepository.ObterPeloId(id, specification, includes);
        }

        private bool PodeSerSalvaNaSituacaoAtual(SituacaoRequisicaoMaterial situacao)
        {
            return situacao == SituacaoRequisicaoMaterial.Requisitada
                || situacao == SituacaoRequisicaoMaterial.Aprovada;
        }

        private bool PodeAdicionarItemNaSituacaoAtual(SituacaoRequisicaoMaterial situacao)
        {
            return situacao == SituacaoRequisicaoMaterial.Requisitada;
        }

        private bool PodeAprovarNaSituacaoAtual(SituacaoRequisicaoMaterial situacao)
        {
            return situacao == SituacaoRequisicaoMaterial.Requisitada;
        }

        private bool PodeCancelarAprovacaoNaSituacaoAtual(SituacaoRequisicaoMaterial situacao)
        {
            return situacao == SituacaoRequisicaoMaterial.Aprovada;
        }

        private bool PodeCancelarNaSituacaoAtual(SituacaoRequisicaoMaterial situacao)
        {
            return situacao == SituacaoRequisicaoMaterial.Requisitada;
        }

        private bool PodeCancelarComItensAtuais(List<RequisicaoMaterialItemDTO> listaItens)
        {
            return listaItens.All(l => l.Situacao == SituacaoRequisicaoMaterialItem.Requisitado || l.Situacao == SituacaoRequisicaoMaterialItem.Cancelado);
        }

        private void ProcessarItens(RequisicaoMaterialDTO dto, RequisicaoMaterial requisicaoMaterial)
        {
            RemoverItens(dto, requisicaoMaterial);
            AlterarItens(dto, requisicaoMaterial);
            AdicionarItens(dto, requisicaoMaterial);
        }

        private void RemoverItens(RequisicaoMaterialDTO dto, RequisicaoMaterial requisicaoMaterial)
        {
            for (int i = requisicaoMaterial.ListaItens.Count - 1; i >= 0; i--)
            {
                var item = requisicaoMaterial.ListaItens.ToList()[i];
                if (!dto.ListaItens.Any(l => l.Id == item.Id))
                {
                    requisicaoMaterial.ListaItens.Remove(item);
                    requisicaoMaterialRepository.RemoverItem(item);
                }
            }
        }

        private void AlterarItens(RequisicaoMaterialDTO dto, RequisicaoMaterial requisicaoMaterial)
        {
            foreach (var item in requisicaoMaterial.ListaItens)
            {
                if (item.Situacao == SituacaoRequisicaoMaterialItem.Requisitado)
                {
                    var itemDTO = dto.ListaItens.Where(l => l.Id == item.Id).SingleOrDefault();

                    if (itemDTO.OrcamentoInsumoRequisitado == null)
                    {
                        item.Material = null;
                        item.MaterialId = itemDTO.Material.Id;
                        item.UnidadeMedida = itemDTO.Material.SiglaUnidadeMedida.Trim();
                        item.Quantidade = itemDTO.Quantidade;
                        item.Classe = null;
                        item.CodigoClasse = itemDTO.Classe.Codigo;
                    }

                    item.Sequencial = itemDTO.Sequencial;
                    item.Complemento = itemDTO.Complemento.Trim();
                    item.QuantidadeAprovada = itemDTO.QuantidadeAprovada;
                    item.DataMaxima = itemDTO.DataMaxima;
                    item.DataMinima = itemDTO.DataMinima;
                    item.Situacao = itemDTO.Situacao;
                    if (item.Situacao == SituacaoRequisicaoMaterialItem.Cancelado)
                        RemoverInsumoRequisitado(item);
                }
            }
        }

        private void RemoverInsumoRequisitado(RequisicaoMaterialItem item)
        {
            for (int i = item.ListaOrcamentoInsumoRequisitado.Count - 1; i >= 0; i--)
            {
                var insumoRequisitado = item.ListaOrcamentoInsumoRequisitado.ToList()[i];
                requisicaoMaterialRepository.RemoverInsumoRequisitado(insumoRequisitado);
            }

            item.ListaOrcamentoInsumoRequisitado.Clear();
        }

        private void AdicionarItens(RequisicaoMaterialDTO dto, RequisicaoMaterial requisicaoMaterial)
        {
            foreach (var item in dto.ListaItens.Where(l => !l.Id.HasValue))
            {
                var itemLista = item.To<RequisicaoMaterialItem>();
                itemLista.RequisicaoMaterial = requisicaoMaterial;
                if (PossuiInsumoRequisitado(item))
                {
                    var orcamentoInsumoRequisitado = new OrcamentoInsumoRequisitado();
                    orcamentoInsumoRequisitado.CodigoCentroCusto = dto.CentroCusto.Codigo;
                    orcamentoInsumoRequisitado.CodigoClasse = item.Classe.Codigo;
                    orcamentoInsumoRequisitado.ComposicaoId = item.OrcamentoInsumoRequisitado.Composicao.Id;
                    orcamentoInsumoRequisitado.MaterialId = item.OrcamentoInsumoRequisitado.Material.Id;
                    orcamentoInsumoRequisitado.Quantidade = item.Quantidade;
                    orcamentoInsumoRequisitado.RequisicaoMaterialItem = itemLista;
                    itemLista.ListaOrcamentoInsumoRequisitado.Add(orcamentoInsumoRequisitado);
                }
                requisicaoMaterial.ListaItens.Add(itemLista);
            }
        }

        private bool PossuiInsumoRequisitado(RequisicaoMaterialItemDTO item)
        {
            return (item.OrcamentoInsumoRequisitado != null)
                && (item.OrcamentoInsumoRequisitado.Material != null)
                && (item.OrcamentoInsumoRequisitado.Material.Id.HasValue);
        }

        private void AjustarSituacaoRequisicao(RequisicaoMaterial requisicao)
        {
            if ((requisicao.ListaItens.Any())
                && (requisicao.ListaItens.All(l => l.Situacao == SituacaoRequisicaoMaterialItem.Fechado
                    || l.Situacao == SituacaoRequisicaoMaterialItem.Cancelado)))
                requisicao.Situacao = SituacaoRequisicaoMaterial.Fechada;
        }

        private DataTable RequisicaoToDataTable(RequisicaoMaterial requisicaoMaterial)
        {
            DataTable dta = new DataTable();
            DataColumn codigo = new DataColumn("codigo");
            DataColumn dataRequisicao = new DataColumn("dataRequisicao");
            DataColumn observacao = new DataColumn("observacao");
            DataColumn dataCadastro = new DataColumn("dataCadastro");
            DataColumn usuarioCadastro = new DataColumn("usuarioCadastro");
            DataColumn dataCancela = new DataColumn("dataCancela");
            DataColumn usuarioCancela = new DataColumn("usuarioCancela");
            DataColumn motivoCancela = new DataColumn("motivoCancela");
            DataColumn dataAprovado = new DataColumn("dataAprovado");
            DataColumn usuarioAprovado = new DataColumn("usuarioAprovado");
            DataColumn centroCusto = new DataColumn("centroCusto");
            DataColumn descricaoCentroCusto = new DataColumn("descricaoCentroCusto");
            DataColumn codigoDescricaoCentroCusto = new DataColumn("codigoDescricaoCentroCusto");
            DataColumn situacaoCentroCusto = new DataColumn("situacaoCentroCusto");
            DataColumn situacao = new DataColumn("situacao");
            DataColumn descricaoSituacao = new DataColumn("descricaoSituacao");

            dta.Columns.Add(codigo);
            dta.Columns.Add(dataRequisicao);
            dta.Columns.Add(observacao);
            dta.Columns.Add(dataCadastro);
            dta.Columns.Add(usuarioCadastro);
            dta.Columns.Add(dataCancela);
            dta.Columns.Add(usuarioCancela);
            dta.Columns.Add(motivoCancela);
            dta.Columns.Add(dataAprovado);
            dta.Columns.Add(usuarioAprovado);
            dta.Columns.Add(centroCusto);
            dta.Columns.Add(descricaoCentroCusto);
            dta.Columns.Add(codigoDescricaoCentroCusto);
            dta.Columns.Add(situacaoCentroCusto);
            dta.Columns.Add(situacao);
            dta.Columns.Add(descricaoSituacao);

            DataRow row = dta.NewRow();
            row[codigo] = requisicaoMaterial.Id;
            row[dataRequisicao] = requisicaoMaterial.Data.ToString("dd/MM/yyyy");
            row[observacao] = requisicaoMaterial.Observacao;
            row[dataCadastro] = requisicaoMaterial.DataCadastro.ToString("dd/MM/yyyy");
            row[usuarioCadastro] = requisicaoMaterial.LoginUsuarioCadastro;
            row[dataCancela] = requisicaoMaterial.DataCancelamento.HasValue ? requisicaoMaterial.DataCancelamento.Value.ToString("dd/MM/yyyy") : string.Empty;
            row[usuarioCancela] = requisicaoMaterial.LoginUsuarioCancelamento;
            row[motivoCancela] = requisicaoMaterial.MotivoCancelamento;
            row[dataAprovado] = requisicaoMaterial.DataAprovacao.HasValue ? requisicaoMaterial.DataAprovacao.Value.ToString("dd/MM/yyyy") : string.Empty;
            row[usuarioAprovado] = requisicaoMaterial.LoginUsuarioAprovacao;
            row[centroCusto] = requisicaoMaterial.CodigoCentroCusto;
            row[descricaoCentroCusto] = requisicaoMaterial.CodigoCentroCusto + " - " + requisicaoMaterial.CentroCusto.Descricao;
            row[codigoDescricaoCentroCusto] = requisicaoMaterial.CodigoCentroCusto + " - " + requisicaoMaterial.CentroCusto.Descricao;
            row[situacaoCentroCusto] = requisicaoMaterial.CentroCusto.Situacao;
            row[situacao] = requisicaoMaterial.Situacao;
            row[descricaoSituacao] = requisicaoMaterial.Situacao.ObterDescricao();
            dta.Rows.Add(row);
            return dta;
        }

        private DataTable RequisicaoItemToDataTable(List<RequisicaoMaterialItem> listaRequisicaoMaterialItem)
        {
            DataTable dta = new DataTable();
            DataColumn codigo = new DataColumn("codigo");
            DataColumn requisicaoMaterial = new DataColumn("requisicaoMaterial");
            DataColumn situacaoRM = new DataColumn("situacaoRM");
            DataColumn descricaoSituacaoRM = new DataColumn("descricaoSituacaoRM");
            DataColumn dataRequisicao = new DataColumn("dataRequisicao");
            DataColumn centroCusto = new DataColumn("centroCusto");
            DataColumn descricaoCentroCusto = new DataColumn("descricaoCentroCusto");
            DataColumn codigoDescricaoCentroCusto = new DataColumn("codigoDescricaoCentroCusto");
            DataColumn situacaoCentroCusto = new DataColumn("situacaoCentroCusto");
            DataColumn dataAprovado = new DataColumn("dataAprovado");
            DataColumn observacaoRM = new DataColumn("observacaoRM");
            DataColumn sequencial = new DataColumn("sequencial");
            DataColumn complementoDescricao = new DataColumn("complementoDescricao");
            DataColumn quantidade = new DataColumn("quantidade");
            DataColumn quantidadeAprovada = new DataColumn("quantidadeAprovada");
            DataColumn dataMinima = new DataColumn("dataMinima");
            DataColumn dataMaxima = new DataColumn("dataMaxima");
            DataColumn preRequisicaoMaterialItem = new DataColumn("preRequisicaoMaterialItem");
            DataColumn material = new DataColumn("material");
            DataColumn descricaoMaterial = new DataColumn("descricaoMaterial");
            DataColumn unidadeMedida = new DataColumn("unidadeMedida");
            DataColumn descricaoUnidadeMedida = new DataColumn("descricaoUnidadeMedida");
            DataColumn codigoDescricaoMaterialClasseInsumo = new DataColumn("codigoDescricaoMaterialClasseInsumo");
            DataColumn classe = new DataColumn("classe");
            DataColumn descricaoClasse = new DataColumn("descricaoClasse");
            DataColumn codigoDescricaoClasse = new DataColumn("codigoDescricaoClasse");
            DataColumn situacao = new DataColumn("situacao");
            DataColumn descricaoSituacao = new DataColumn("descricaoSituacao");
            DataColumn codigoCotacao = new DataColumn("codigoCotacao");
            DataColumn codigoOrdemCompra = new DataColumn("codigoOrdemCompra");
            DataColumn situacaoOC = new DataColumn("situacaoOC");

            dta.Columns.Add(codigo);
            dta.Columns.Add(requisicaoMaterial);
            dta.Columns.Add(situacaoRM);
            dta.Columns.Add(descricaoSituacaoRM);
            dta.Columns.Add(dataRequisicao);
            dta.Columns.Add(centroCusto);
            dta.Columns.Add(descricaoCentroCusto);
            dta.Columns.Add(codigoDescricaoCentroCusto);
            dta.Columns.Add(situacaoCentroCusto);
            dta.Columns.Add(dataAprovado);
            dta.Columns.Add(observacaoRM);
            dta.Columns.Add(sequencial);
            dta.Columns.Add(complementoDescricao);
            dta.Columns.Add(quantidade);
            dta.Columns.Add(quantidadeAprovada);
            dta.Columns.Add(dataMinima);
            dta.Columns.Add(dataMaxima);
            dta.Columns.Add(preRequisicaoMaterialItem);
            dta.Columns.Add(material);
            dta.Columns.Add(descricaoMaterial);
            dta.Columns.Add(unidadeMedida);
            dta.Columns.Add(descricaoUnidadeMedida);
            dta.Columns.Add(codigoDescricaoMaterialClasseInsumo);
            dta.Columns.Add(classe);
            dta.Columns.Add(descricaoClasse);
            dta.Columns.Add(codigoDescricaoClasse);
            dta.Columns.Add(situacao);
            dta.Columns.Add(descricaoSituacao);
            dta.Columns.Add(codigoCotacao);
            dta.Columns.Add(codigoOrdemCompra);
            dta.Columns.Add(situacaoOC);

            foreach (var item in listaRequisicaoMaterialItem)
            {
                DataRow row = dta.NewRow();
                row[codigo] = item.Id;
                row[requisicaoMaterial] = item.RequisicaoMaterialId;
                row[situacaoRM] = item.RequisicaoMaterial.Situacao;
                row[descricaoSituacaoRM] = item.RequisicaoMaterial.Situacao.ObterDescricao();
                row[dataRequisicao] = item.RequisicaoMaterial.Data.ToString("dd/MM/yyyy");
                row[centroCusto] = item.RequisicaoMaterial.CodigoCentroCusto;
                row[descricaoCentroCusto] = item.RequisicaoMaterial.CentroCusto.Descricao;
                row[codigoDescricaoCentroCusto] = item.RequisicaoMaterial.CodigoCentroCusto + " - " + item.RequisicaoMaterial.CentroCusto.Descricao;
                row[situacaoCentroCusto] = item.RequisicaoMaterial.CentroCusto.Situacao;
                row[dataAprovado] = item.RequisicaoMaterial.DataAprovacao.HasValue ? item.RequisicaoMaterial.DataAprovacao.Value.ToString("dd/MM/yyyy") : string.Empty;
                row[observacaoRM] = item.RequisicaoMaterial.Observacao;
                row[sequencial] = item.Sequencial;
                row[complementoDescricao] = item.Complemento;
                row[quantidade] = item.Quantidade;
                row[quantidadeAprovada] = item.QuantidadeAprovada;
                row[dataMinima] = item.DataMinima.HasValue ? item.DataMinima.Value.ToString("dd/MM/yyyy") : string.Empty;
                row[dataMaxima] = item.DataMaxima.HasValue ? item.DataMaxima.Value.ToString("dd/MM/yyyy") : string.Empty;
                row[preRequisicaoMaterialItem] = item.PreRequisicaoMaterialItemId;
                row[material] = item.MaterialId;
                row[descricaoMaterial] = item.Material.Descricao;
                row[unidadeMedida] = item.UnidadeMedida;
                row[descricaoUnidadeMedida] = item.Material.UnidadeMedida.Descricao;
                if (!string.IsNullOrEmpty(item.Material.CodigoMaterialClasseInsumo))
                    row[codigoDescricaoMaterialClasseInsumo] = item.Material.CodigoMaterialClasseInsumo + " - " + item.Material.MaterialClasseInsumo.Descricao;
                row[classe] = item.CodigoClasse;
                row[descricaoClasse] = item.Classe.Descricao;
                row[codigoDescricaoClasse] = item.CodigoClasse + " - " + item.Classe.Descricao;
                row[situacao] = item.Situacao;
                row[descricaoSituacao] = item.Situacao.ObterDescricao();
                row[codigoCotacao] = item.ListaCotacaoItem.Where(l => l.Cotacao.Situacao != SituacaoCotacao.Cancelada).Max(c => c.CotacaoId);
                row[codigoOrdemCompra] = item.ListaOrdemCompraItem.Where(l => l.OrdemCompra.Situacao != SituacaoOrdemCompra.Cancelada).Max(c => c.OrdemCompraId);
                row[situacaoOC] = item.ListaOrcamentoInsumoRequisitado.Any();

                dta.Rows.Add(row);
            }
            return dta;
        }

        private static string RequisicaoToXML(RequisicaoMaterial requisicaoMaterial)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<requisicaoMaterial>");
            sb.Append("<OrdemCompra.requisicaoMaterial>");
            sb.Append("<codigo>" + requisicaoMaterial.Id.ToString() + "</codigo>");
            sb.Append("<centroCusto>" + requisicaoMaterial.CodigoCentroCusto + "</centroCusto>");
            sb.Append("<dataRequisicao>" + requisicaoMaterial.Data.ToString() + "</dataRequisicao>");
            sb.Append("<situacao>" + ((int)requisicaoMaterial.Situacao).ToString() + "</situacao>");
            sb.Append("<observacao>" + requisicaoMaterial.Observacao + "</observacao>");
            sb.Append("<dataCadastro>" + requisicaoMaterial.DataCadastro.ToString() + "</dataCadastro>");
            sb.Append("<usuarioCadastro>" + requisicaoMaterial.LoginUsuarioCadastro + "</usuarioCadastro>");
            sb.Append("<dataAprovado>" + requisicaoMaterial.DataAprovacao.ToString() + "</dataAprovado>");
            sb.Append("<usuarioAprovado>" + requisicaoMaterial.LoginUsuarioAprovacao + "</usuarioAprovado>");
            sb.Append("</OrdemCompra.requisicaoMaterial>");
            sb.Append("</requisicaoMaterial>");

            return sb.ToString();
        }

        private static string RequisicaoItemToXML(List<RequisicaoMaterialItem> listaItens)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<requisicaoMaterialItem>");
            foreach (var item in listaItens)
            {
                sb.Append("<OrdemCompra.requisicaoMaterialItem>");

                sb.Append("<codigo>" + item.Id + "</codigo>");
                sb.Append("<requisicaoMaterial>" + item.RequisicaoMaterialId + "</requisicaoMaterial>");
                sb.Append("<material>" + item.MaterialId + "</material>");
                sb.Append("<classe>" + item.CodigoClasse + "</classe>");
                sb.Append("<sequencial>" + item.Sequencial + "</sequencial>");
                sb.Append("<complementoDescricao>" + item.Complemento + "</complementoDescricao>");
                sb.Append("<unidadeMedida>" + item.UnidadeMedida + "</unidadeMedida>");
                sb.Append("<quantidade>" + item.Quantidade.ToString("0.00000") + "</quantidade>");
                sb.Append("<quantidadeAprovada>" + item.QuantidadeAprovada.ToString("0.00000") + "</quantidadeAprovada>");
                sb.Append("<dataMinima>" + item.DataMinima.ToString() + "</dataMinima>");
                sb.Append("<dataMaxima>" + item.DataMaxima.ToString() + "</dataMaxima>");
                sb.Append("<situacao>" + ((int)item.Situacao).ToString() + "</situacao>");
                sb.Append("<preRequisicaoMaterialItem>" + item.PreRequisicaoMaterialItemId + "</preRequisicaoMaterialItem>");
                sb.Append("</OrdemCompra.requisicaoMaterialItem>");
            }
            sb.Append("</requisicaoMaterialItem>");

            return sb.ToString();
        }

        private void GravarLogOperacao(RequisicaoMaterial requisicaoMaterial, string operacao)
        {
            logOperacaoAppService.Gravar("Atualização da requisição de material",
                "OrdemCompra.requisicaoMaterial_Atualiza",
                "OrdemCompra.requisicaoMaterial",
                operacao,
                RequisicaoToXML(requisicaoMaterial));

            if (requisicaoMaterial.ListaItens.Any())
                logOperacaoAppService.Gravar("Atualização da requisição de material",
                    "OrdemCompra.requisicaoMaterial_Atualiza",
                    "OrdemCompra.requisicaoMaterialItem",
                    operacao,
                    RequisicaoItemToXML(requisicaoMaterial.ListaItens.ToList()));
        }

        #endregion
    }
}