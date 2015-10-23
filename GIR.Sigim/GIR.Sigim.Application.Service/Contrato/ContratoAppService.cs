using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Contrato;
using GIR.Sigim.Application.Reports.Contrato;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros.Contrato;
using GIR.Sigim.Application.Service.Admin;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Domain.Repository.Contrato;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Domain.Specification.Contrato;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Application.Service.Financeiro;
using CrystalDecisions.Shared;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Application.Enums;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Entity.Estoque;
using GIR.Sigim.Domain.Repository.Estoque;

namespace GIR.Sigim.Application.Service.Contrato
{
    public class ContratoAppService : BaseAppService , IContratoAppService
    {
        #region Declaração

        private IContratoRepository  contratoRepository;
        private IUsuarioAppService usuarioAppService;
        private IParametrosContratoRepository parametrosContratoRepository;
        private ILogOperacaoAppService logOperacaoAppService;
        private IContratoRetificacaoItemMedicaoAppService contratoRetificacaoItemMedicaoAppService;
        private IBloqueioContabilAppService bloqueioContabilAppService;
        private ITituloPagarRepository tituloPagarRepository;
        private ITituloReceberRepository tituloReceberRepository;
        private ITituloPagarAppService tituloPagarAppService;
        private IImpostoFinanceiroRepository impostoFinanceiroRepository;
        private IParametrosFinanceiroRepository parametrosFinanceiroRepository;
        private IFeriadoRepository feriadoRepository;
        private IApropriacaoRepository apropriacaoRepository;
        private IImpostoPagarRepository impostoPagarRepository;
        private IImpostoReceberRepository impostoReceberRepository;
        private IContratoRetificacaoProvisaoRepository contratoRetificacaoProvisaoRepository;
        private IEstoqueRepository estoqueRepository;
        private IContratoRetencaoRepository contratoRetencaoRepository;

        #endregion

        #region Construtor

        public ContratoAppService(IContratoRepository contratoRepository, 
                                  IUsuarioAppService usuarioAppService, 
                                  ILogOperacaoAppService logOperacaoAppService,
                                  IContratoRetificacaoItemMedicaoAppService contratoRetificacaoItemMedicaoAppService,
                                  IBloqueioContabilAppService bloqueioContabilAppService,
                                  IParametrosContratoRepository parametrosContratoRepository,
                                  ITituloPagarRepository tituloPagarRepository,
                                  ITituloReceberRepository tituloReceberRepository,
                                  ITituloPagarAppService tituloPagarAppService,
                                  IImpostoFinanceiroRepository impostoFinanceiroRepository,
                                  IParametrosFinanceiroRepository parametrosFinanceiroRepository,
                                  IFeriadoRepository feriadoRepository,
                                  IApropriacaoRepository apropriacaoRepository,
                                  IImpostoPagarRepository impostoPagarRepository,
                                  IImpostoReceberRepository impostoReceberRepository,
                                  IContratoRetificacaoProvisaoRepository contratoRetificacaoProvisaoRepository,
                                  IEstoqueRepository estoqueRepository,
                                  IContratoRetencaoRepository contratoRetencaoRepository,
                                  MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.contratoRepository = contratoRepository;
            this.usuarioAppService = usuarioAppService;
            this.logOperacaoAppService = logOperacaoAppService;
            this.contratoRetificacaoItemMedicaoAppService = contratoRetificacaoItemMedicaoAppService;
            this.bloqueioContabilAppService = bloqueioContabilAppService;
            this.parametrosContratoRepository = parametrosContratoRepository;
            this.tituloPagarRepository = tituloPagarRepository;
            this.tituloReceberRepository = tituloReceberRepository;
            this.tituloPagarAppService = tituloPagarAppService;
            this.impostoFinanceiroRepository = impostoFinanceiroRepository;
            this.parametrosFinanceiroRepository = parametrosFinanceiroRepository;
            this.feriadoRepository = feriadoRepository;
            this.apropriacaoRepository = apropriacaoRepository;
            this.impostoPagarRepository = impostoPagarRepository;
            this.impostoReceberRepository = impostoReceberRepository;
            this.contratoRetificacaoProvisaoRepository = contratoRetificacaoProvisaoRepository;
            this.estoqueRepository = estoqueRepository;
            this.contratoRetencaoRepository = contratoRetencaoRepository;
        }

        #endregion

        #region Métodos IContratoAppService

        public List<ContratoDTO> ListarPeloFiltro(MedicaoContratoFiltro filtro,int? idUsuario, out int totalRegistros)
        {
            var specification = (Specification<Domain.Entity.Contrato.Contrato>)new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            if (usuarioAppService.UsuarioPossuiCentroCustoDefinidoNoModulo(idUsuario, Resource.Sigim.NomeModulo.Contrato))
            {
                specification &= ContratoSpecification.UsuarioPossuiAcessoAoCentroCusto(idUsuario, Resource.Sigim.NomeModulo.Contrato);
            }
            else
            {
                specification &= ContratoSpecification.EhCentroCustoAtivo();
            }

            if (filtro.Id.HasValue)
                specification &= ContratoSpecification.MatchingId(filtro.Id);
            else
            {
                specification &= ContratoSpecification.PertenceAoCentroCustoIniciadoPor(filtro.CentroCusto.Codigo);
                specification &= ContratoSpecification.PertenceAoContratante(filtro.Contratante.Id);
                specification &= ContratoSpecification.PertenceAoContratado(filtro.Contratado.Id);
            }

            return contratoRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros,
                l => l.CentroCusto,
                l => l.ContratoDescricao.ListaContrato,
                l => l.Contratante.ListaContratoContratante,
                l => l.Contratado.ListaContratoContratado).To<List<ContratoDTO>>();
        }

        public List<ContratoDTO> ListarPeloFiltro(LiberacaoContratoFiltro filtro, int? idUsuario, out int totalRegistros)
        {
            var specification = (Specification<Domain.Entity.Contrato.Contrato>)new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            bool ehMinuta = false;
            bool ehAguardandoAssinatura = false;
            bool ehAssinado = true;
            bool ehRetificacao = false;
            bool ehSuspenso = false;
            bool ehConcluido = true;
            bool ehCancelado = false;

            if (ehMinuta || ehAguardandoAssinatura || ehAssinado || ehRetificacao || ehSuspenso || ehConcluido || ehCancelado)
            {
                specification &= ((ehMinuta ? ContratoSpecification.EhMinuta() : new FalseSpecification<Domain.Entity.Contrato.Contrato>())
                    || (ehAguardandoAssinatura ? ContratoSpecification.EhAguardandoAssinatura() : new FalseSpecification<Domain.Entity.Contrato.Contrato>())
                    || (ehAssinado ? ContratoSpecification.EhAssinado() : new FalseSpecification<Domain.Entity.Contrato.Contrato>())
                    || (ehRetificacao ? ContratoSpecification.EhRetificacao() : new FalseSpecification<Domain.Entity.Contrato.Contrato>())
                    || (ehSuspenso ? ContratoSpecification.EhSuspenso() : new FalseSpecification<Domain.Entity.Contrato.Contrato>())
                    || (ehConcluido ? ContratoSpecification.EhConcluido() : new FalseSpecification<Domain.Entity.Contrato.Contrato>())
                    || (ehCancelado ? ContratoSpecification.EhCancelado() : new FalseSpecification<Domain.Entity.Contrato.Contrato>()));
            }

            if (usuarioAppService.UsuarioPossuiCentroCustoDefinidoNoModulo(idUsuario, Resource.Sigim.NomeModulo.Contrato))
            {
                specification &= ContratoSpecification.UsuarioPossuiAcessoAoCentroCusto(idUsuario, Resource.Sigim.NomeModulo.Contrato);
            }
            else
            {
                specification &= ContratoSpecification.EhCentroCustoAtivo();
            }

            if (filtro.Id.HasValue)
                specification &= ContratoSpecification.MatchingId(filtro.Id);
            else
            {
                specification &= ContratoSpecification.PertenceAoCentroCustoIniciadoPor(filtro.CentroCusto.Codigo);
                specification &= ContratoSpecification.PertenceAoContratante(filtro.Contratante.Id);
                specification &= ContratoSpecification.PertenceAoContratado(filtro.Contratado.Id);
            }

            return contratoRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros,
                l => l.CentroCusto,
                l => l.ContratoDescricao.ListaContrato,
                l => l.Contratante.ListaContratoContratante,
                l => l.Contratado.ListaContratoContratado,
                l => l.ListaContratoRetificacaoItemMedicao).To<List<ContratoDTO>>();
        }

        public ContratoDTO ObterPeloId(int? id, int? idUsuario)
        {
            var specification = (Specification<Domain.Entity.Contrato.Contrato>)new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            if (idUsuario.HasValue)
            {
                if (usuarioAppService.UsuarioPossuiCentroCustoDefinidoNoModulo(idUsuario, Resource.Sigim.NomeModulo.Contrato))
                {
                    specification &= ContratoSpecification.UsuarioPossuiAcessoAoCentroCusto(idUsuario, Resource.Sigim.NomeModulo.Contrato);
                }
                else
                {
                    specification &= ContratoSpecification.EhCentroCustoAtivo();
                }
            }

            return contratoRepository.ObterPeloId(id, 
                                                  specification, 
                                                  l => l.CentroCusto, 
                                                  l => l.Contratado.PessoaFisica, 
                                                  l => l.Contratado.PessoaJuridica , 
                                                  l => l.ContratoDescricao, 
                                                  l => l.ListaContratoRetificacao,
                                                  l => l.ListaContratoRetificacaoItem.Select(d => d.Servico),
                                                  l => l.ListaContratoRetificacaoItemMedicao.Select(d => d.TipoDocumento),
                                                  l => l.ListaContratoRetificacaoItemCronograma,
                                                  l => l.ListaContratoRetificacaoProvisao,
                                                  l => l.ListaContratoRetencao.Select(d => d.ListaContratoRetencaoLiberada),
                                                  l => l.ListaAvaliacaoFornecedor).To<ContratoDTO>();
        }

        public bool EhContratoAssinado(ContratoDTO dto)
        {
            bool ehContratoAssinado =  PodeSerUmContratoAssinado(dto.Situacao);
            if (!ehContratoAssinado)
            {
                messageQueue.Add(Application.Resource.Contrato.ErrorMessages.ContratoNaoAssinado, TypeMessage.Error);
            }

            return ehContratoAssinado;
        }

        public bool EhContratoExistente(ContratoDTO dto)
        {
            if (dto == null)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
                return false;
            }

            if (!dto.Id.HasValue)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
                return false;
            }

            return true;
        }

        public bool EhContratoComCentroCustoAtivo(ContratoDTO dto)
        {
            if (!dto.CentroCusto.Ativo)
            {
                messageQueue.Add(Application.Resource.Financeiro.ErrorMessages.CentroCustoInativo, TypeMessage.Error);
                return false;
            }

            return true;
        }

        public List<ContratoRetificacaoProvisaoDTO> ObterListaCronogramaPorContratoEhRetificacaoItem(int contratoId, int contratoRetificacaoItemId)
        {
            var contrato = contratoRepository.ObterPeloId(contratoId, 
                                                          l => l.ListaContratoRetificacaoItem.Select(i => i.Servico),
                                                          l => l.ListaContratoRetificacaoItem.Select(i => i.RetencaoTipoCompromisso),
                                                          l => l.ListaContratoRetificacaoProvisao,
                                                          l => l.ListaContratoRetificacaoItemCronograma,
                                                          l => l.ListaContratoRetificacaoItemMedicao);
            return contrato.ListaContratoRetificacaoProvisao
                .Where(l => l.ContratoRetificacaoItemId == contratoRetificacaoItemId)
                .To<List<ContratoRetificacaoProvisaoDTO>>();
        }

        public List<ContratoRetificacaoItemMedicaoDTO> ObtemMedicaoPorSequencialItem(int contratoId, int sequencialItem)
        {
            var contrato = contratoRepository.ObterPeloId(contratoId,
                                                          l => l.ListaContratoRetificacaoItemMedicao.Select(i => i.TipoDocumento));

            return contrato.ListaContratoRetificacaoItemMedicao
                    .Where(l => l.SequencialItem == sequencialItem).OrderBy(l => l.DataVencimento)
                    .To<List<ContratoRetificacaoItemMedicaoDTO>>();
        }

        public ContratoRetificacaoItemMedicaoDTO ObtemMedicaoPorId(int contratoId, int contratoRetificacaoItemMedicaoId)
        {
            var contrato = contratoRepository.ObterPeloId(contratoId,
                                                          l => l.ListaContratoRetificacaoItemCronograma,
                                                          l => l.ListaContratoRetificacaoItemMedicao.Select(i => i.MultiFornecedor));

            var medicao =  contrato.ListaContratoRetificacaoItemMedicao
                            .Where(l => l.Id.Value == contratoRetificacaoItemMedicaoId).SingleOrDefault().To<ContratoRetificacaoItemMedicaoDTO>();

            return medicao;
        }

        public bool ExisteContratoRetificacaoProvisao(List<ContratoRetificacaoProvisaoDTO> listaContratoRetificacaoProvisao)
        {
            if (listaContratoRetificacaoProvisao == null)
            {
                messageQueue.Add(Resource.Contrato.ErrorMessages.RetificacaoItemSemProvisionamento, TypeMessage.Error);
                return false;
            }
            if (listaContratoRetificacaoProvisao.Count == 0)
            {
                messageQueue.Add(Resource.Contrato.ErrorMessages.RetificacaoItemSemProvisionamento, TypeMessage.Error);
                return false;
            }

            return true;
        }

        public bool ExisteMedicao(ContratoRetificacaoItemMedicaoDTO dto)
        {
            if (dto == null)
            {
                messageQueue.Add(Resource.Contrato.ErrorMessages.MedicaoNaoEncontrada, TypeMessage.Error);
                return false;
            }

            return true;
        }

        public bool EhValidoParametrosVisualizacaoMedicao(int? contratoId,
                                                          int? tipoDocumentoId,
                                                          string numeroDocumento,
                                                          Nullable<DateTime> dataEmissao,
                                                          int? contratadoId)
        {
            if (!contratadoId.HasValue)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Contrato"), TypeMessage.Error);
                return false;
            }

            if (contratadoId == 0)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Contrato"), TypeMessage.Error);
                return false;
            }

            if (!tipoDocumentoId.HasValue)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Tipo"), TypeMessage.Error);
                return false;
            }

            if (tipoDocumentoId == 0)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Tipo"), TypeMessage.Error);
                return false;
            }

            if (string.IsNullOrEmpty(numeroDocumento))
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Nº"), TypeMessage.Error);
                return false;
            }

            if (!dataEmissao.HasValue)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Data emissão"), TypeMessage.Error);
                return false;
            }

            if (!contratadoId.HasValue)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Contratado"), TypeMessage.Error);
                return false;
            }

            return true;

        }

        public List<ContratoRetificacaoItemMedicaoDTO> RecuperaMedicaoPorDadosDaNota(int contratoId,
                                                                                     int tipoDocumentoId,
                                                                                     string numeroDocumento,
                                                                                     DateTime dataEmissao,
                                                                                     int? contratadoId)
        {
            List<ContratoRetificacaoItemMedicaoDTO> listaMedicao =
                    RecuperaMedicaoPorContratoEhDadosDaNota(contratoId,tipoDocumentoId,numeroDocumento,dataEmissao,contratadoId,
                                                            l => l.ListaContratoRetificacaoItem.Select(i => i.Servico),
                                                            l => l.ListaContratoRetificacaoItemCronograma,
                                                            l => l.ListaContratoRetificacaoItemImposto.Select(ip => ip.ImpostoFinanceiro),
                                                            l => l.ListaContratoRetificacaoItemMedicao).To<List<ContratoRetificacaoItemMedicaoDTO>>();

            return listaMedicao;
        }

        public bool ExisteNumeroDocumento(Nullable<DateTime> dataEmissao, string numeroDocumento, int? contratadoId)
        {
            bool existe = false;

            if (!string.IsNullOrEmpty(numeroDocumento) && (contratadoId.HasValue))
            {
                List<Domain.Entity.Contrato.Contrato> listaContrato;

                string numeroNotaFiscal = RetiraZerosIniciaisNumeroDocumento(numeroDocumento);

                //traz uma lista de contrato que possuem na lista de medições do contrato pelo menos um documento existente
                listaContrato = contratoRepository.ListarPeloFiltro(l => 
                                                                        l.ListaContratoRetificacaoItemMedicao
                                                                        .Any(s => s.NumeroDocumento.EndsWith(numeroNotaFiscal) && 
                                                                        (
                                                                            (dataEmissao == null) ||
                                                                            ((dataEmissao != null) && (s.DataEmissao.Year == dataEmissao.Value.Year))
                                                                        ) &&
                                                                        (
                                                                            (s.MultiFornecedorId == contratadoId) ||
                                                                            (s.MultiFornecedorId == null && l.ContratadoId == contratadoId)
                                                                        )),
                                                                    l => l.ListaContratoRetificacaoItemMedicao).To<List<Domain.Entity.Contrato.Contrato>>();
                if (listaContrato.Count() > 0)
                {
                    string numeroDeZerosIniciais;
                    foreach (var contrato in listaContrato)
                    {
                        //descarta as mediçoes que possuem o documento diferente do procurado
                        var lista = contrato.ListaContratoRetificacaoItemMedicao.Where(l => l.DataEmissao == dataEmissao && l.NumeroDocumento.EndsWith(numeroNotaFiscal) == true).To<List<ContratoRetificacaoItemMedicao>>();
                        foreach (var medicao in lista)
                        {
                            //verifica se o documento foi digitado com zeros na frente
                            var quantidadeDeZerosIniciais = medicao.NumeroDocumento.Length - numeroNotaFiscal.Length;
                            numeroDeZerosIniciais = medicao.NumeroDocumento.Substring(0, quantidadeDeZerosIniciais);
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

        public FileDownloadDTO ExportarMedicao(int contratoId,
                                               int? contratadoId,
                                               int tipoDocumentoId,
                                               string numeroDocumento,
                                               DateTime dataEmissao,
                                               string valorContratadoItem,
                                               FormatoExportacaoArquivo formato,
                                               OrigemChamada origemChamada)
        {
            if (origemChamada == OrigemChamada.MedicaoContrato)
            {
                if (!UsuarioLogado.IsInRole(Funcionalidade.MedicaoImprimir))
                {
                    messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                    return null;
                }
            }

            if (origemChamada == OrigemChamada.LiberacaoContrato)
            {
                if (!UsuarioLogado.IsInRole(Funcionalidade.LiberacaoImprimirMedicao))
                {
                    messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                    return null;
                }
            }

            var listaMedicao = RecuperaMedicaoPorContratoEhDadosDaNota(contratoId,
                                                                       tipoDocumentoId,
                                                                       numeroDocumento,
                                                                       dataEmissao,
                                                                       contratadoId,
                                                                       l => l.CentroCusto.ListaCentroCustoEmpresa,
                                                                       l => l.ContratoDescricao,
                                                                       l => l.Contratado.PessoaFisica,
                                                                       l => l.Contratado.PessoaJuridica,
                                                                       l => l.Contratante.PessoaFisica,
                                                                       l => l.Contratante.PessoaJuridica,
                                                                       l => l.ListaContratoRetificacao,
                                                                       l => l.ListaContratoRetificacaoItemMedicao.Select(i => i.MultiFornecedor.PessoaFisica),
                                                                       l => l.ListaContratoRetificacaoItemMedicao.Select(i => i.MultiFornecedor.PessoaJuridica),
                                                                       l => l.ListaContratoRetificacaoItemMedicao.Select(i => i.TipoDocumento),
                                                                       l => l.ListaContratoRetificacaoItemMedicao.Select(i => i.TituloPagar),
                                                                       l => l.ListaContratoRetificacaoItemMedicao.Select(i => i.TituloReceber),
                                                                       l => l.ListaContratoRetificacaoItem.Select(i => i.Servico),
                                                                       l => l.ListaContratoRetificacaoItem.Select(i => i.Classe),
                                                                       l => l.ListaContratoRetificacaoItemCronograma).To<List<ContratoRetificacaoItemMedicao>>();

            var listaImposto = RecuperaImpostoMedicaoPorContratoDadosDaNota(contratoId,
                                                                            tipoDocumentoId,
                                                                            numeroDocumento,
                                                                            dataEmissao,
                                                                            contratadoId,
                                                                            l => l.ListaContratoRetificacaoItemImposto.Select(i => i.ImpostoFinanceiro),
                                                                            l => l.ListaContratoRetificacaoItem,
                                                                            l => l.ListaContratoRetificacaoItemMedicao);

            decimal retencaoContrato = 0;
            foreach (var retificacao in listaMedicao.Select(l => l.ContratoRetificacao).Where(l => l.RetencaoContratual.HasValue))
            {
                if (retificacao.RetencaoContratual.Value > 0)
                {
                    retencaoContrato = retificacao.RetencaoContratual.Value;
                }
            }

            decimal retencaoItem = 0;
            foreach (var retificacaoItem in listaMedicao.Select(l => l.ContratoRetificacaoItem).Where(l => l.RetencaoItem.HasValue))
            {
                if (retificacaoItem.RetencaoItem.Value > 0)
                {
                    retencaoItem = retificacaoItem.RetencaoItem.Value;
                }
            }

            decimal percentualRetencao = retencaoItem > 0 ? retencaoItem : (retencaoContrato > 0 ? retencaoContrato : 0);

            relMedicao objRel = new relMedicao();

            DataTable dtaImpostoMedido = ImpostoToDataTable(listaImposto, listaMedicao);

            decimal valorImposto = 0;
            decimal valorIndireto = 0;
            decimal valorTotalImposto = 0;
            foreach (DataRow linha in dtaImpostoMedido.Rows)
            {
                if (linha["valorImposto"] != DBNull.Value){
                    valorImposto = (decimal)linha["valorImposto"];
                }
                linha["valorImposto"] = valorImposto;
                if (linha["valorTotalMedidoIndireto"] != DBNull.Value){
                    valorIndireto = (decimal) linha["valorTotalMedidoIndireto"];
                }
                linha["valorTotalMedidoIndireto"] = valorIndireto;
                bool retido = false;
                if (linha["retido"] != DBNull.Value){
                    retido = (bool)linha["retido"];
                }
                bool indireto = false;
                if (linha["indireto"] != DBNull.Value){
                    indireto = (bool)linha["indireto"];
                }
                if (retido || (indireto && valorIndireto > 4999.99m)){
                    valorTotalImposto += valorImposto;
                }
            }
        
            objRel.SetDataSource(MedicaoToDataTable(listaMedicao));
            objRel.Subreports["contratoImposto"].Database.Tables["Contrato_contratoImpostoMedidoRelatorio"].SetDataSource(dtaImpostoMedido);

            var parametros = parametrosContratoRepository.Obter();
            var centroCusto = listaMedicao.ElementAt(0).Contrato.CentroCusto;
            var caminhoImagem = PrepararIconeRelatorio(centroCusto, parametros);

            var nomeEmpresa = ObterNomeEmpresa(centroCusto, parametros);
            objRel.SetParameterValue("nomeEmpresa", nomeEmpresa);
            objRel.SetParameterValue("parCentroCusto", centroCusto.Codigo);
            objRel.SetParameterValue("parDescricaoCentroCusto", centroCusto.Descricao);
            string contratado = listaMedicao.ElementAt(0).Contrato.Contratado.Nome;
            if (listaMedicao.ElementAt(0).Contrato.Contratado.TipoPessoa == "F")
            {
                contratado += " - " + listaMedicao.ElementAt(0).Contrato.Contratado.PessoaFisica.Cpf;
            }
            if (listaMedicao.ElementAt(0).Contrato.Contratado.TipoPessoa == "J")
            {
                contratado += " - " + listaMedicao.ElementAt(0).Contrato.Contratado.PessoaJuridica.Cnpj;
            }
            objRel.SetParameterValue("parContratado", contratado);
            objRel.SetParameterValue("parContrato", listaMedicao.ElementAt(0).ContratoId);
            objRel.SetParameterValue("parDescricaoContrato", listaMedicao.ElementAt(0).Contrato.ContratoDescricao.Descricao);
            objRel.SetParameterValue("parRetencao", percentualRetencao);
            objRel.SetParameterValue("parValorContratadoItem", valorContratadoItem);
            objRel.SetParameterValue("parValorTotalImposto", valorTotalImposto);

            string multiFornecedor = "";
            if (listaMedicao.ElementAt(0).MultiFornecedorId > 0)
            {
                multiFornecedor = listaMedicao.ElementAt(0).MultiFornecedor.Nome;
                if (listaMedicao.ElementAt(0).MultiFornecedor.TipoPessoa == "F")
                {
                    multiFornecedor += " - " + listaMedicao.ElementAt(0).MultiFornecedor.PessoaFisica.Cpf;
                }
                if (listaMedicao.ElementAt(0).MultiFornecedor.TipoPessoa == "J")
                {
                    multiFornecedor += " - " + listaMedicao.ElementAt(0).MultiFornecedor.PessoaJuridica.Cnpj;
                }

            }
            objRel.SetParameterValue("parMultifornecedor", multiFornecedor);

            string assinaturaEletronicaMedicao = listaMedicao.ElementAt(0).UsuarioMedicao;
            string caminhoImagemAssinaturaMedicao = "";
            if (!string.IsNullOrEmpty(assinaturaEletronicaMedicao))
            {
                var usuarioMedicao = usuarioAppService.ObterUsuarioPorLogin(assinaturaEletronicaMedicao);
                if (usuarioMedicao != null && usuarioMedicao.AssinaturaEletronica != null && usuarioMedicao.AssinaturaEletronica.Length > 0)
                {
                    caminhoImagemAssinaturaMedicao = DiretorioImagemRelatorio + Guid.NewGuid().ToString() + ".bmp";
                    System.Drawing.Image imagemAssinaturaMedicao = usuarioMedicao.AssinaturaEletronica.ToImage();
                    imagemAssinaturaMedicao.Save(caminhoImagemAssinaturaMedicao, System.Drawing.Imaging.ImageFormat.Bmp);
                }
            }
            objRel.SetParameterValue("assinaturaEletronicaMedicao", caminhoImagemAssinaturaMedicao);

            string assinaturaEletronicaLiberacao = listaMedicao.ElementAt(0).UsuarioLiberacao;
            string caminhoImagemAssinaturaLiberacao = "";
            if (!string.IsNullOrEmpty(assinaturaEletronicaLiberacao))
            {
                var usuarioLiberacao = usuarioAppService.ObterUsuarioPorLogin(assinaturaEletronicaLiberacao);
                if (usuarioLiberacao != null && usuarioLiberacao.AssinaturaEletronica != null && usuarioLiberacao.AssinaturaEletronica.Length > 0)
                {
                    caminhoImagemAssinaturaLiberacao = DiretorioImagemRelatorio + Guid.NewGuid().ToString() + ".bmp";
                    System.Drawing.Image imagemAssinaturaLiberacao = usuarioLiberacao.AssinaturaEletronica.ToImage();
                    imagemAssinaturaLiberacao.Save(caminhoImagemAssinaturaLiberacao, System.Drawing.Imaging.ImageFormat.Bmp);
                }
            }
            objRel.SetParameterValue("assinaturaEletronicaLiberacao", caminhoImagemAssinaturaLiberacao);

            objRel.SetParameterValue("caminhoImagem", caminhoImagem);

            FileDownloadDTO arquivo = new FileDownloadDTO(
                "Medicao",
                objRel.ExportToStream((ExportFormatType)formato),
                formato);

            if (System.IO.File.Exists(caminhoImagem))
                System.IO.File.Delete(caminhoImagem);
            return arquivo;
        }

        public bool ExcluirMedicao(int? contratoId,int? contratoRetificacaoItemMedicaoId)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.MedicaoDeletar))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            var contrato = contratoRepository.ObterPeloId(contratoId,l => l.ListaContratoRetificacaoItemMedicao);
            if (!EhValidoExcluirMedicao(contrato,contratoRetificacaoItemMedicaoId))
            {
                return false;
            }

            try
            {
                ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao = contrato.ListaContratoRetificacaoItemMedicao.Where(l => l.Id == contratoRetificacaoItemMedicaoId).SingleOrDefault();
                contrato.ListaContratoRetificacaoItemMedicao.Remove(contratoRetificacaoItemMedicao);              
                contratoRepository.RemoverItemMedicao(contratoRetificacaoItemMedicao);

                contratoRepository.Alterar(contrato);
                contratoRepository.UnitOfWork.Commit();
                GravarLogOperacaoMedicao(contratoRetificacaoItemMedicao, "DELETE");

                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception exception)
            {
                QueueExeptionMessages(exception);
                messageQueue.Add(Resource.Sigim.ErrorMessages.ExclusaoErro, TypeMessage.Error);
                return false;
            }
        }

        public bool SalvarMedicao(ContratoRetificacaoItemMedicaoDTO dto)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.MedicaoGravar))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            if ((dto == null)) throw new ArgumentNullException("dto");

            bool novoRegistro = false;

            Domain.Entity.Contrato.Contrato contrato = contratoRepository.ObterPeloId(dto.ContratoId,
                                                                                      l => l.ListaContratoRetificacao,
                                                                                      l => l.ListaContratoRetificacaoItem,
                                                                                      l => l.ListaContratoRetificacaoItemMedicao, 
                                                                                      l => l.CentroCusto);

            ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao = InseriuMedicao(contrato, dto);
            novoRegistro = contratoRetificacaoItemMedicao.Id == null ? true : false;

            try
            {
                if (!EhValidoSalvarMedicao(contratoRetificacaoItemMedicao))
                {
                    return false;
                }

                if (Validator.IsValid(contratoRetificacaoItemMedicao, out validationErrors))
                {
                    contratoRepository.Alterar(contrato);
                    contratoRepository.UnitOfWork.Commit();
                    GravarLogOperacaoMedicao(contratoRetificacaoItemMedicao, novoRegistro ? "INSERT" : "UPDATE");

                    messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                    return true;
                }
                else
                {
                    messageQueue.AddRange(validationErrors, TypeMessage.Error);
                }
            }
            catch (Exception exception)
            {

                QueueExeptionMessages(exception);
            }

            return false;
        }

        public bool EhPermitidoSalvarMedicao()
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.MedicaoGravar))
                return false;

            return true;
        }

        public bool EhPermitidoDeletarMedicao()
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.MedicaoDeletar))
                return false;

            return true;
        }

        public bool EhPermitidoImprimirMedicao()
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.MedicaoImprimir))
                return false;

            return true;
        }

        public bool EhPermitidoAprovarLiberarLiberacao()
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.LiberacaoAprovarLiberar))
                return false;

            return true;
        }

        public bool EhPermitidoAprovarLiberacao()
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.LiberacaoAprovar))
                return false;

            return true;
        }

        public bool EhPermitidoLiberarLiberacao()
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.LiberacaoLiberar))
                return false;

            return true;
        }

        public bool EhPermitidoCancelarLiberacao()
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.LiberacaoCancelar))
                return false;

            return true;
        }

        public bool EhPermitidoAssociarNFLiberacao()
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.LiberacaoAssociarNF))
                return false;

            return true;
        }

        public bool EhPermitidoAlterarvencimentoLiberacao()
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.LiberacaoAlterarVencimento))
                return false;

            return true;
        }

        public bool EhPermitidoImprimirMedicaoLiberacao()
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.LiberacaoImprimirMedicao))
                return false;

            return true;
        }

        public List<ContratoDTO> PesquisarContratosPeloFiltro(ContratoPesquisaFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<Domain.Entity.Contrato.Contrato>)new TrueSpecification<Domain.Entity.Contrato.Contrato>();
            int? inicio;
            int? fim;

            bool EhTipoSelecaoContem = filtro.TipoSelecao == TipoPesquisa.Contem;
            switch (filtro.Campo)
            {
                case "centroCusto":
                    specification &= EhTipoSelecaoContem ? ContratoSpecification.CentroCustoContem(filtro.TextoInicio)
                        : ContratoSpecification.CentroCustoNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                    break;
                case "descricaoContrato":
                    specification &= EhTipoSelecaoContem ? ContratoSpecification.DescricaoContratoContem(filtro.TextoInicio)
                        : ContratoSpecification.DescricaoContratoNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                    break;
                case "contratante":
                    specification &= EhTipoSelecaoContem ? ContratoSpecification.ContratanteContem(filtro.TextoInicio)
                        : ContratoSpecification.ContratanteNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                    break;
                case "contratado":
                    specification &= EhTipoSelecaoContem ? ContratoSpecification.ContratadoContem(filtro.TextoInicio)
                        : ContratoSpecification.ContratadoNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                    break;
                case "dataAssinatura":
                    Nullable<DateTime> datInicio = !string.IsNullOrEmpty(filtro.TextoInicio) ? Convert.ToDateTime(filtro.TextoInicio) : (Nullable<DateTime>)null;
                    Nullable<DateTime> datFim = !string.IsNullOrEmpty(filtro.TextoFim) ? Convert.ToDateTime(filtro.TextoFim) : (Nullable<DateTime>)null;
                    specification &= EhTipoSelecaoContem ? ContratoSpecification.DataAssinaturaContem(datInicio)
                        : ContratoSpecification.DataAssinaturaNoIntervalo(datInicio, datFim);
                    break;

                case "descricaoSituacao":
                    List<ItemListaDTO> listaSituacao = typeof(SituacaoContrato).ToItemListaDTO();
                    bool ehMinuta = false;
                    bool ehAguardandoAssinatura = false;
                    bool ehAssinado = false;
                    bool ehRetificacao = false;
                    bool ehSuspenso = false;
                    bool ehConcluido = false;
                    bool ehCancelado = false;

                    if (EhTipoSelecaoContem)
                    {
                        if (!string.IsNullOrEmpty(filtro.TextoInicio)){

                            foreach (ItemListaDTO item in listaSituacao.OrderBy(l => l.Id))
                            {
                                SituacaoContrato situacao = (SituacaoContrato)item.Id;

                                if (item.Descricao.ToUpper().Contains(filtro.TextoInicio.ToUpper()))
                                {
                                    switch (situacao){
                                        case SituacaoContrato.Minuta:
                                            ehMinuta = true;
                                            break;
                                        case SituacaoContrato.AguardandoAssinatura:
                                            ehAguardandoAssinatura = true;
                                            break;
                                        case SituacaoContrato.Assinado:
                                            ehAssinado = true;
                                            break;
                                        case SituacaoContrato.Retificacao:
                                            ehRetificacao = true;
                                            break;
                                        case SituacaoContrato.Suspenso:
                                            ehSuspenso = true;
                                            break;
                                        case SituacaoContrato.Concluido:
                                            ehConcluido = true;
                                            break;
                                        case SituacaoContrato.Cancelado:
                                            ehCancelado = true;
                                            break;
                                    }
                                }
                            }

                            if (ehMinuta || ehAguardandoAssinatura || ehAssinado || ehRetificacao || ehSuspenso || ehConcluido || ehCancelado)
                            {
                                specification &= ((ehMinuta ? ContratoSpecification.EhMinuta() : new FalseSpecification<Domain.Entity.Contrato.Contrato>())
                                    || (ehAguardandoAssinatura ? ContratoSpecification.EhAguardandoAssinatura() : new FalseSpecification<Domain.Entity.Contrato.Contrato>())
                                    || (ehAssinado ? ContratoSpecification.EhAssinado() : new FalseSpecification<Domain.Entity.Contrato.Contrato>())
                                    || (ehRetificacao ? ContratoSpecification.EhRetificacao() : new FalseSpecification<Domain.Entity.Contrato.Contrato>())
                                    || (ehSuspenso ? ContratoSpecification.EhSuspenso() : new FalseSpecification<Domain.Entity.Contrato.Contrato>())
                                    || (ehConcluido ? ContratoSpecification.EhConcluido() : new FalseSpecification<Domain.Entity.Contrato.Contrato>())
                                    || (ehCancelado ? ContratoSpecification.EhCancelado() : new FalseSpecification<Domain.Entity.Contrato.Contrato>()));
                            }
                        }
                    }
                    else
                    {
                        if ((!string.IsNullOrEmpty(filtro.TextoInicio)) || (!string.IsNullOrEmpty(filtro.TextoFim)))
                        {
                            if (!string.IsNullOrEmpty(filtro.TextoInicio))
                            {
                                foreach (ItemListaDTO item in listaSituacao.OrderBy(l => l.Descricao))
                                {
                                    SituacaoContrato situacao = (SituacaoContrato)item.Id;

                                    if ((item.Descricao.ToUpper().CompareTo(filtro.TextoInicio.ToUpper()) >= 0))
                                    {
                                        switch (situacao)
                                        {
                                            case SituacaoContrato.Minuta:
                                                ehMinuta = true;
                                                break;
                                            case SituacaoContrato.AguardandoAssinatura:
                                                ehAguardandoAssinatura = true;
                                                break;
                                            case SituacaoContrato.Assinado:
                                                ehAssinado = true;
                                                break;
                                            case SituacaoContrato.Retificacao:
                                                ehRetificacao = true;
                                                break;
                                            case SituacaoContrato.Suspenso:
                                                ehSuspenso = true;
                                                break;
                                            case SituacaoContrato.Concluido:
                                                ehConcluido = true;
                                                break;
                                            case SituacaoContrato.Cancelado:
                                                ehCancelado = true;
                                                break;
                                        }
                                    }
                                    if ((!string.IsNullOrEmpty(filtro.TextoFim)) && (item.Descricao.ToUpper().Substring(0, filtro.TextoFim.Length).CompareTo(filtro.TextoFim.ToUpper()) > 0))
                                    {
                                        switch (situacao)
                                        {
                                            case SituacaoContrato.Minuta:
                                                ehMinuta = false;
                                                break;
                                            case SituacaoContrato.AguardandoAssinatura:
                                                ehAguardandoAssinatura = false;
                                                break;
                                            case SituacaoContrato.Assinado:
                                                ehAssinado = false;
                                                break;
                                            case SituacaoContrato.Retificacao:
                                                ehRetificacao = false;
                                                break;
                                            case SituacaoContrato.Suspenso:
                                                ehSuspenso = false;
                                                break;
                                            case SituacaoContrato.Concluido:
                                                ehConcluido = false;
                                                break;
                                            case SituacaoContrato.Cancelado:
                                                ehCancelado = false;
                                                break;
                                        }
                                    }
                                }
                            }
                            if (ehMinuta || ehAguardandoAssinatura || ehAssinado || ehRetificacao || ehSuspenso || ehConcluido || ehCancelado)
                            {
                                specification &= ((ehMinuta ? ContratoSpecification.EhMinuta() : new FalseSpecification<Domain.Entity.Contrato.Contrato>())
                                    || (ehAguardandoAssinatura ? ContratoSpecification.EhAguardandoAssinatura() : new FalseSpecification<Domain.Entity.Contrato.Contrato>())
                                    || (ehAssinado ? ContratoSpecification.EhAssinado() : new FalseSpecification<Domain.Entity.Contrato.Contrato>())
                                    || (ehRetificacao ? ContratoSpecification.EhRetificacao() : new FalseSpecification<Domain.Entity.Contrato.Contrato>())
                                    || (ehSuspenso ? ContratoSpecification.EhSuspenso() : new FalseSpecification<Domain.Entity.Contrato.Contrato>())
                                    || (ehConcluido ? ContratoSpecification.EhConcluido() : new FalseSpecification<Domain.Entity.Contrato.Contrato>())
                                    || (ehCancelado ? ContratoSpecification.EhCancelado() : new FalseSpecification<Domain.Entity.Contrato.Contrato>()));
                            }
                        }
                    }
                    break;
                case "fornecedor":
                    specification &= EhTipoSelecaoContem ? ContratoSpecification.ContratadoContem(filtro.TextoInicio)
                        : ContratoSpecification.ContratadoNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                    break;
                case "id":
                default:
                    inicio = !string.IsNullOrEmpty(filtro.TextoInicio) ? Convert.ToInt32(filtro.TextoInicio) : (int?)null;
                    fim = !string.IsNullOrEmpty(filtro.TextoFim) ? Convert.ToInt32(filtro.TextoFim) : (int?)null;
                    specification &= EhTipoSelecaoContem ? ContratoSpecification.MatchingId(inicio)
                        : ContratoSpecification.IdNoIntervalo(inicio, fim);
                    break;
            }

            return contratoRepository.Pesquisar(specification,
                                                filtro.PageIndex,
                                                filtro.PageSize,
                                                filtro.OrderBy,
                                                filtro.Ascending,
                                                out totalRegistros,
                                                l => l.CentroCusto,
                                                l => l.ContratoDescricao,
                                                l => l.Contratante,
                                                l => l.Contratado).To<List<ContratoDTO>>();
        }

        public bool PodeConcluirContrato(ContratoDTO contrato)
        {
            decimal valorTotalProvisionado = 0;

            List<ContratoRetificacaoProvisaoDTO> listaContratoRetificacaoProvisao = null;

            ContratoRetificacaoDTO contratoRetificacao = contrato.ListaContratoRetificacao.Last();

            if (!PodeSerUmContratoAssinado(contrato.Situacao))
            {
                return false;
            }

            if (contratoRetificacao == null)
            {
                return false;
            }

            if (!contratoRetificacao.Aprovada)
            {
                return false;
            }

            listaContratoRetificacaoProvisao = contrato.ListaContratoRetificacaoProvisao.Where(l => l.ContratoRetificacaoId == contratoRetificacao.Id && l.ContratoRetificacaoItemId.HasValue).ToList();

            if (listaContratoRetificacaoProvisao == null)
            {
                return false;
            }

            if (listaContratoRetificacaoProvisao.Count > 0)
            {
                return false;
            }

            valorTotalProvisionado  = contrato.ListaContratoRetificacaoProvisao.Where(l => l.ContratoRetificacaoId == contratoRetificacao.Id && l.ContratoRetificacaoItemId == null).Select(l => l.Valor).FirstOrDefault();

            if (valorTotalProvisionado > 0)
            {
                return false;
            }

            return true;
        }

        public void RecuperarMedicoesALiberar(ContratoDTO contrato, ContratoRetificacaoItemDTO contratoRetificacaoItemSelecionado, ResumoLiberacaoDTO resumo, out List<ItemLiberacaoDTO> listaItemLiberacao)
        {
            decimal valorTotalItem = 0;
            decimal valorTotalItemSemRetencao = 0;
            decimal valorTotalProvisionado = 0;
            decimal valorTotalMedido = 0;
            decimal totalAdiantado = 0;
            decimal totalValorAdiantadoDescontado = 0;
            decimal valorTotalAguardando = 0;
            decimal valorTotalRetido = 0;
            decimal valorRetido = 0;
            decimal valorTotalRetencaoLiberada = 0;
            decimal valorTotalLiberado = 0;
            List<ContratoRetificacaoProvisaoDTO> listaContratoRetificacaoProvisao = null;
            List<ContratoRetificacaoItemMedicaoDTO> listaContratoRetificacaoItemMedicao = null;
            List<ContratoRetencaoLiberadaDTO> listaContratoRetencaoLiberada = null;
            List<ItemLiberacaoDTO> listaItemLiberacaoAux = new List<ItemLiberacaoDTO>();

            ContratoRetificacaoDTO contratoRetificacao = contrato.ListaContratoRetificacao.Last();

            if (!contratoRetificacaoItemSelecionado.Id.HasValue)
            {
                listaContratoRetificacaoItemMedicao = contrato.ListaContratoRetificacaoItemMedicao;
                listaContratoRetificacaoProvisao = contrato.ListaContratoRetificacaoProvisao.Where(l => l.ContratoRetificacaoId == contratoRetificacao.Id && l.ContratoRetificacaoItemId.HasValue).ToList();
                listaContratoRetencaoLiberada = contrato.ListaContratoRetencao.SelectMany(l => l.ListaContratoRetencaoLiberada).ToList();
                if (contrato.ValorContrato.HasValue) valorTotalItem = contrato.ValorContrato.Value;
            }
            else
            {
                listaContratoRetificacaoItemMedicao = contrato.ListaContratoRetificacaoItemMedicao.Where(l => l.SequencialItem == contratoRetificacaoItemSelecionado.Sequencial).ToList();
                listaContratoRetificacaoProvisao = contrato.ListaContratoRetificacaoProvisao.Where(l => l.ContratoRetificacaoItemId == contratoRetificacaoItemSelecionado.Id).ToList();
                listaContratoRetencaoLiberada = contrato.ListaContratoRetencao.
                                                SelectMany(l => l.ListaContratoRetencaoLiberada.
                                                                Where(m => m.ContratoRetencao.ContratoRetificacaoItem.Sequencial == contratoRetificacaoItemSelecionado.Sequencial)).ToList();
                if (contratoRetificacaoItemSelecionado.ValorItem.HasValue) valorTotalItem = contratoRetificacaoItemSelecionado.ValorItem.Value;
            }
            valorTotalItemSemRetencao = valorTotalItem;



            decimal retencaoContratual = 0;
            if (contratoRetificacao.RetencaoContratual.HasValue)
            {
                retencaoContratual = contratoRetificacao.RetencaoContratual.Value;
            }
            decimal valorContratoRetido = 0;
            if (retencaoContratual > 0)
            {
                valorTotalItemSemRetencao = valorTotalItem - (valorTotalItem * retencaoContratual) / 100;
                valorContratoRetido = valorTotalItem - valorTotalItemSemRetencao;
            }
            else
            {
                if (!contratoRetificacaoItemSelecionado.Id.HasValue)
                {
                    valorContratoRetido = (from item in contratoRetificacao.ListaContratoRetificacaoItem
                                           select (((((item.ValorItem.HasValue ? item.ValorItem.Value : 0) * (item.BaseRetencaoItem.HasValue ? item.BaseRetencaoItem.Value : 0)) / 100) * (item.RetencaoItem.HasValue ? item.RetencaoItem.Value : 0)) / 100)).Sum();
                }
                else
                {
                    valorContratoRetido = (from item in contratoRetificacao.ListaContratoRetificacaoItem
                                           where (item.Id == contratoRetificacaoItemSelecionado.Id)
                                           select (((((item.ValorItem.HasValue ? item.ValorItem.Value : 0) * (item.BaseRetencaoItem.HasValue ? item.BaseRetencaoItem.Value : 0)) / 100) * (item.RetencaoItem.HasValue ? item.RetencaoItem.Value : 0)) / 100)).Sum();
                }
                valorTotalItemSemRetencao = valorTotalItem - valorContratoRetido;
            }

            if (listaContratoRetificacaoProvisao.Count > 0)
            {
                foreach (var provisao in listaContratoRetificacaoProvisao)
                {
                    ItemLiberacaoDTO itemLiberacao = new ItemLiberacaoDTO();
                    if (provisao.SequencialItem.HasValue) itemLiberacao.SequencialItem = provisao.SequencialItem.Value;
                    itemLiberacao.DataVencimento = new DateTime(1900, 1, 1, 0, 0, 0);
                    if (provisao.ContratoRetificacaoItemCronograma != null)
                    {
                        itemLiberacao.DataVencimento = provisao.ContratoRetificacaoItemCronograma.DataVencimento;
                    }

                    itemLiberacao.ResponsavelLiberacao = !string.IsNullOrEmpty(provisao.UsuarioAntecipacao) ? provisao.UsuarioAntecipacao : "";
                    itemLiberacao.TipoDocumento = "";
                    itemLiberacao.NumeroDocumento = "";
                    bool ehPagamentoAntecipado = provisao.PagamentoAntecipado.HasValue ? provisao.PagamentoAntecipado.Value : false;
                    itemLiberacao.EhPagamentoAntecipado = ehPagamentoAntecipado;
                    if (ehPagamentoAntecipado)
                    {
                        itemLiberacao.NumeroDocumento = RecuperaDocumentoAntecipacao(contrato, provisao.SequencialCronograma, provisao.SequencialItem, provisao.ContratoRetificacaoId);
                        itemLiberacao.DescricaoSituacao = "Antecipado";
                        if (provisao.DataAntecipacao.HasValue) itemLiberacao.DataLiberacao = provisao.DataAntecipacao.Value;

                        totalAdiantado = totalAdiantado + provisao.Valor;
                        if (provisao.ValorAdiantadoDescontado.HasValue)
                        {
                            totalValorAdiantadoDescontado = totalValorAdiantadoDescontado + provisao.ValorAdiantadoDescontado.Value;
                        }
                        itemLiberacao.CorTexto = "color:purple";
                    }
                    else
                    {
                        itemLiberacao.DescricaoSituacao = "Provisionado";
                        valorTotalProvisionado = valorTotalProvisionado + provisao.Valor;
                        itemLiberacao.CorTexto = "color:blue";
                    }
                    itemLiberacao.CodigoSituacao = (int)SituacaoMedicao.Provisionado;
                    itemLiberacao.Valor = provisao.Valor;
                    itemLiberacao.ValorRetido = 0;
                    itemLiberacao.ResponsavelMedicao = "";
                    if (contrato.TipoContrato == 0){
                        itemLiberacao.TituloId = provisao.TituloPagarId.HasValue ? provisao.TituloPagarId.Value : 0;
                    }
                    else{
                        itemLiberacao.TituloId = provisao.TituloReceberId.HasValue ? provisao.TituloReceberId.Value : 0;
                    }
                    itemLiberacao.Avaliacao = "";
                    itemLiberacao.DescricaoEvento = "";
                    if (provisao.ContratoRetificacaoItemCronograma != null)
                    {
                        itemLiberacao.DescricaoEvento = !string.IsNullOrEmpty(provisao.ContratoRetificacaoItemCronograma.Descricao) ? provisao.ContratoRetificacaoItemCronograma.Descricao : "";
                    }
                    itemLiberacao.ContratoRetificacaoProvisaoId = provisao.Id;
                    itemLiberacao.Ordem = 1;

                    listaItemLiberacaoAux.Add(itemLiberacao);
                }
            }

            if (listaContratoRetificacaoItemMedicao.Count > 0)
            {
                foreach (var medicao in listaContratoRetificacaoItemMedicao)
                {
                    ItemLiberacaoDTO itemLiberacao = new ItemLiberacaoDTO();
                    itemLiberacao.SequencialItem = medicao.SequencialItem;
                    itemLiberacao.DataMedicao = medicao.DataMedicao;
                    itemLiberacao.DataVencimento = medicao.DataVencimento;
                    itemLiberacao.TipoDocumento = medicao.TipoDocumento.Sigla;
                    itemLiberacao.NumeroDocumento = medicao.NumeroDocumento;
                    itemLiberacao.DataEmissao = medicao.DataEmissao;
                    itemLiberacao.CodigoSituacao = (int)medicao.Situacao;
                    itemLiberacao.DescricaoSituacao = medicao.Situacao.ObterDescricao();
                    itemLiberacao.Valor = medicao.Valor;
                    valorRetido = medicao.ValorRetido.HasValue ? medicao.ValorRetido.Value : 0;
                    itemLiberacao.ValorRetido = valorRetido;
                    itemLiberacao.ResponsavelMedicao = medicao.UsuarioMedicao;
                    if (contrato.TipoContrato == 0)
                    {
                        itemLiberacao.TituloId = medicao.TituloPagarId.HasValue ? medicao.TituloPagarId.Value : 0;
                    }
                    else
                    {
                        itemLiberacao.TituloId = medicao.TituloReceberId.HasValue ? medicao.TituloReceberId.Value : 0;
                    }
                    itemLiberacao.DataLiberacao = medicao.DataLiberacao;
                    itemLiberacao.ResponsavelLiberacao = !string.IsNullOrEmpty(medicao.UsuarioLiberacao) ? medicao.UsuarioLiberacao : "";
                    itemLiberacao.DataCadastro = medicao.DataCadastro;
                    if (medicao.Situacao == SituacaoMedicao.Liberado)
                    {
                        itemLiberacao.CorTexto = "color:black";
                    }
                    else
                    {
                        itemLiberacao.CorTexto = "color:red";
                    }
                    itemLiberacao.Avaliacao = "";
                    if (!string.IsNullOrEmpty(medicao.NumeroDocumento))
                    {
                        bool existeAvaliacao = contrato.ListaAvaliacaoFornecedor.Any(l => l.ContratoId == medicao.ContratoId && l.Documento == medicao.NumeroDocumento);
                        itemLiberacao.Avaliacao = existeAvaliacao ? "S" : "";
                    }

                    itemLiberacao.DescricaoEvento = "";
                    if (medicao.ContratoRetificacaoItemCronograma != null)
                    {
                        itemLiberacao.DescricaoEvento = !string.IsNullOrEmpty(medicao.ContratoRetificacaoItemCronograma.Descricao) ? medicao.ContratoRetificacaoItemCronograma.Descricao : "";
                    }

                    valorTotalMedido = valorTotalMedido + medicao.Valor;
                    bool ehSituacaoMedicaoLiberado = contratoRetificacaoItemMedicaoAppService.EhSituacaoMedicaoLiberado(medicao);
                    if (ehSituacaoMedicaoLiberado)
                    {
                        valorTotalLiberado = valorTotalLiberado + medicao.Valor;
                    }
                    else
                    {
                        valorTotalAguardando = valorTotalAguardando + medicao.Valor;
                    }
                    valorRetido = 0;
                    if (medicao.ValorRetido.HasValue) valorRetido = medicao.ValorRetido.Value;
                    valorTotalRetido = valorTotalRetido + valorRetido;

                    itemLiberacao.ContratoRetificacaoItemMedicaoId = medicao.Id;
                    itemLiberacao.Ordem = 2;

                    listaItemLiberacaoAux.Add(itemLiberacao);
                }
            }

            if (listaContratoRetencaoLiberada.Count > 0)
            {
                foreach (var retLib in listaContratoRetencaoLiberada)
                {
                    ItemLiberacaoDTO itemLiberacao = new ItemLiberacaoDTO();
                    itemLiberacao.SequencialItem = retLib.ContratoRetencao.ContratoRetificacaoItem.Sequencial;
                    itemLiberacao.DataVencimento = retLib.DataVencimento;
                    itemLiberacao.TipoDocumento = retLib.ContratoRetencao.ContratoRetificacaoItemMedicao.TipoDocumento.Sigla;
                    itemLiberacao.NumeroDocumento = retLib.ContratoRetencao.ContratoRetificacaoItemMedicao.NumeroDocumento;
                    itemLiberacao.DescricaoSituacao = "Liberado";
                    itemLiberacao.CodigoSituacao = (int)SituacaoMedicao.Retencao;
                    itemLiberacao.Valor = retLib.ValorLiberado;
                    itemLiberacao.ValorRetido = 0;
                    itemLiberacao.ResponsavelMedicao = "";
                    if (contrato.TipoContrato == 0)
                    {
                        itemLiberacao.TituloId = retLib.TituloPagarId.HasValue ? retLib.TituloPagarId.Value : 0;
                    }
                    else
                    {
                        itemLiberacao.TituloId = retLib.TituloReceberId.HasValue ? retLib.TituloReceberId.Value : 0;
                    }
                    itemLiberacao.DataLiberacao = retLib.DataLiberacao;
                    itemLiberacao.ResponsavelLiberacao = retLib.UsuarioLiberacao;
                    itemLiberacao.DescricaoEvento = "";
                    itemLiberacao.CorTexto = "color:green";
                    itemLiberacao.Avaliacao = "";
                    itemLiberacao.Ordem = 3;
                    itemLiberacao.ContratoRetificacaoItemMedicaoId = retLib.Id;

                    valorTotalRetencaoLiberada = valorTotalRetencaoLiberada + retLib.ValorLiberado;

                    listaItemLiberacaoAux.Add(itemLiberacao);
                }
            }

            resumo.ValorContratado = valorTotalItem;
            resumo.ValorRetidoContrato = valorContratoRetido;
            resumo.ValorProvisionado = valorTotalProvisionado;
            resumo.ValorMedido = valorTotalMedido;
            resumo.Diferenca = valorTotalItem - valorTotalProvisionado - valorTotalMedido - totalAdiantado + totalValorAdiantadoDescontado;
            resumo.AguardandoLiberacao = valorTotalAguardando;
            resumo.Retido = valorTotalRetido - valorTotalRetencaoLiberada;
            resumo.Liberado = valorTotalLiberado;
            resumo.RetencaoLiberada = valorTotalRetencaoLiberada;
            resumo.Saldo = valorTotalItem - valorTotalMedido;
            listaItemLiberacao = listaItemLiberacaoAux.OrderBy(l => l.Ordem).ThenBy(l => l.DataVencimento).ToList();

            int posicaoLista = 0;
            foreach (ItemLiberacaoDTO item in listaItemLiberacao)
            {
                item.PosicaoLista = posicaoLista;
                posicaoLista = posicaoLista + 1;
                item.Selecionado = false;
                item.DataSelecionado = 0;
            }
        }

        public bool EhPermitidoHabilitarBotoes(ContratoDTO dto)
        {
            bool ehPermitidoHabilitarBotoes = PodeSerUmContratoCancelado(dto.Situacao) || PodeSerUmContratoConcluido(dto.Situacao) || PodeSerUmContratoSuspenso(dto.Situacao);

            return !ehPermitidoHabilitarBotoes;
        }

        public bool EhUltimoContratoRetificacao(int? contratoId,int? contratoRetificacaoId)
        {
            if (!contratoId.HasValue)
            {              
                return false;
            }

            var specification = (Specification<Domain.Entity.Contrato.Contrato>)new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            Domain.Entity.Contrato.Contrato contrato = contratoRepository.ObterPeloId(contratoId,
                                                                                      specification,
                                                                                      l => l.ListaContratoRetificacao);
            if (contrato.ListaContratoRetificacao == null)
            {
                return false;
            }

            if (contrato.ListaContratoRetificacao.Last().Id != contratoRetificacaoId)
            {
                return false;
            }

            return true;
        }

        public bool AtualizarSituacaoParaConcluido(int? contratoId)
        {
            bool retorno = false;
            if (!contratoId.HasValue)
            {
                return retorno;
            }

            ContratoDTO contratoDTO = ObterPeloId(contratoId, null);

            if (!PodeConcluirContrato(contratoDTO))
            {
                return retorno;
            }

            var specification = (Specification<Domain.Entity.Contrato.Contrato>)new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            Domain.Entity.Contrato.Contrato contrato = contratoRepository.ObterPeloId(contratoId,specification);

            try
            {
                contrato.Situacao = SituacaoContrato.Concluido;

                if (Validator.IsValid(contrato, out validationErrors))
                {
                    contratoRepository.Alterar(contrato);
                    contratoRepository.UnitOfWork.Commit();

                    messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                    retorno = true;
                    return retorno;
                }
                else
                {
                    messageQueue.AddRange(validationErrors, TypeMessage.Error);
                    return retorno;
                }
            }
            catch (Exception exception)
            {
                QueueExeptionMessages(exception);
            }

            return retorno;
        }

        public bool AprovarListaItemLiberacao(int contratoId,List<ItemLiberacaoDTO> listaItemLiberacaoDTO)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.LiberacaoAprovar))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            bool retorno = false;
            if (listaItemLiberacaoDTO == null)
            {
                messageQueue.Add("Nenhuma liberação foi selecionada", TypeMessage.Error);
                return retorno;
            }

            if (listaItemLiberacaoDTO.All(l => l.Selecionado == false))
            {
                messageQueue.Add("Nenhuma liberação foi selecionada", TypeMessage.Error);
                return retorno;
            }

            if (listaItemLiberacaoDTO.Any(l => l.Selecionado == true && l.CodigoSituacao != (int)SituacaoMedicao.AguardandoAprovacao)){
                messageQueue.Add("Existe(m) medição(ões) com situação diferente de Aguardando aprovação !", TypeMessage.Info);
                return retorno;
            }

            var specification = (Specification<Domain.Entity.Contrato.Contrato>)new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            Domain.Entity.Contrato.Contrato contrato = contratoRepository.ObterPeloId(contratoId, specification,l => l.ListaContratoRetificacaoItemMedicao);

            bool modificou = false;
            foreach (ItemLiberacaoDTO item in listaItemLiberacaoDTO.Where(l => l.Selecionado == true && l.CodigoSituacao == (int)SituacaoMedicao.AguardandoAprovacao))
            {
                ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao = contrato.ListaContratoRetificacaoItemMedicao.Where(l => l.Id == item.ContratoRetificacaoItemMedicaoId).FirstOrDefault();
                if (contratoRetificacaoItemMedicao.Situacao != SituacaoMedicao.AguardandoAprovacao)
                {
                    messageQueue.Add("Existe um ou mais itens da lista desatualizados recupere o contrato novamente !", TypeMessage.Info);
                    modificou = true;
                    break;
                }
            }

            if (!modificou)
            {
                return retorno;
            }

            try
            {
                if (Validator.IsValid(contrato, out validationErrors))
                {
                    contratoRepository.Alterar(contrato);
                    contratoRepository.UnitOfWork.Commit();

                    messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                    retorno = true;
                    return retorno;
                }
                else
                {
                    messageQueue.AddRange(validationErrors, TypeMessage.Error);
                    return retorno;
                }
            }
            catch (Exception exception)
            {
                QueueExeptionMessages(exception);
            }


            return retorno;
        }

        public bool ValidarImpressaoMedicaoPelaLiberacao(int? contratoId, List<ItemLiberacaoDTO> listaItemLiberacaoDTO, out int? contratoRetificacaoItemMedicaoId)
        {
            contratoRetificacaoItemMedicaoId = null;

            if (listaItemLiberacaoDTO == null)
            {
                messageQueue.Add("Nenhuma liberação foi selecionada", TypeMessage.Error);
                return false;
            }

            if (listaItemLiberacaoDTO.All(l => l.Selecionado == false))
            {
                messageQueue.Add("Nenhuma liberação foi selecionada", TypeMessage.Error);
                return false;
            }

            ItemLiberacaoDTO itemLiberacao = listaItemLiberacaoDTO.Where(l => l.Selecionado == true).FirstOrDefault();
            if (itemLiberacao.CodigoSituacao > (int)SituacaoMedicao.Liberado)
            {
                messageQueue.Add("Não é permitido imprimir a medição do item selecionado", TypeMessage.Error);
                return false;
            }

            var specification = (Specification<Domain.Entity.Contrato.Contrato>)new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            Domain.Entity.Contrato.Contrato contrato = contratoRepository.ObterPeloId(contratoId,
                                                                                      specification,
                                                                                      l => l.ListaContratoRetificacao,
                                                                                      l => l.ListaContratoRetificacaoItem,
                                                                                      l => l.ListaContratoRetificacaoItemMedicao);

            ContratoRetificacaoItemMedicao medicao = contrato.ListaContratoRetificacaoItemMedicao.Where(l => l.Id == itemLiberacao.ContratoRetificacaoItemMedicaoId).FirstOrDefault();
            if (medicao == null)
            {
                messageQueue.Add("Medição inexistente", TypeMessage.Error);
                return false;
            }

            contratoRetificacaoItemMedicaoId = medicao.Id;

            return true;
        }


        public FileDownloadDTO ImprimirMedicaoPelaLiberacao(FormatoExportacaoArquivo formato,int? contratoId, int? contratoRetificacaoItemMedicaoId)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.LiberacaoImprimirMedicao))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return null;
            }

            var specification = (Specification<Domain.Entity.Contrato.Contrato>)new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            Domain.Entity.Contrato.Contrato contrato = contratoRepository.ObterPeloId(contratoId, 
                                                                                      specification, 
                                                                                      l => l.ListaContratoRetificacao,
                                                                                      l => l.ListaContratoRetificacaoItem,
                                                                                      l => l.ListaContratoRetificacaoItemMedicao);

            ContratoRetificacaoItemMedicao medicao = contrato.ListaContratoRetificacaoItemMedicao.Where(l => l.Id == contratoRetificacaoItemMedicaoId).FirstOrDefault();

            int contratadoId = medicao.MultiFornecedorId != null ? medicao.MultiFornecedorId.Value : contrato.ContratadoId;
            decimal valorContrato = contrato.ValorContrato.HasValue ? contrato.ValorContrato.Value : 0;

            FileDownloadDTO arquivo = ExportarMedicao(contratoId.Value,
                                                      contratadoId,
                                                      medicao.TipoDocumentoId,
                                                      medicao.NumeroDocumento,
                                                      medicao.DataEmissao,
                                                      valorContrato.ToString(),
                                                      formato,
                                                      OrigemChamada.LiberacaoContrato);
            return arquivo;
        }

        public bool ValidarTrocaDataVencimentoListaItemLiberacao(int contratoId, Nullable<DateTime> dataVencimento, List<ItemLiberacaoDTO> listaItemLiberacaoDTO)
        {
            bool retorno = false;

            if (!UsuarioLogado.IsInRole(Funcionalidade.LiberacaoAlterarVencimento))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            if (listaItemLiberacaoDTO == null)
            {
                messageQueue.Add("Nenhuma liberação foi selecionada", TypeMessage.Error);
                return retorno;
            }

            if (!dataVencimento.HasValue)
            {
                messageQueue.Add("Data de vencimento não foi informada", TypeMessage.Error);
                return retorno;
            }

            DateTime hoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            if (dataVencimento < hoje)
            {
                messageQueue.Add("Data de vencimento menor que a data de hoje", TypeMessage.Error);
                return retorno;
            }

            if (listaItemLiberacaoDTO.All(l => l.Selecionado == false))
            {
                messageQueue.Add("Nenhuma liberação foi selecionada", TypeMessage.Error);
                return retorno;
            }

            if (listaItemLiberacaoDTO.Any(l => l.Selecionado == true && l.CodigoSituacao != (int)SituacaoMedicao.Provisionado))
            {
                messageQueue.Add("Existe uma ou mais medições com situação diferente de Provisionado !", TypeMessage.Info);
                return retorno;
            }

            var specificationContrato = (Specification<Domain.Entity.Contrato.Contrato>)new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            Domain.Entity.Contrato.Contrato contrato = contratoRepository.ObterPeloId(contratoId, specificationContrato, l => l.ListaContratoRetificacaoProvisao, l => l.ListaContratoRetificacaoItemCronograma);

            bool falhouTeste = false;
            foreach (ItemLiberacaoDTO item in listaItemLiberacaoDTO.Where(l => l.Selecionado == true && l.CodigoSituacao == (int)SituacaoMedicao.Provisionado))
            {
                ContratoRetificacaoProvisao contratoRetificacaoProvisao = contrato.ListaContratoRetificacaoProvisao.Where(l => l.Id == item.ContratoRetificacaoProvisaoId).FirstOrDefault();
                if (contratoRetificacaoProvisao == null)
                {
                    messageQueue.Add("Existe um ou mais itens da lista desatualizados recupere o contrato novamente !", TypeMessage.Info);
                    falhouTeste = true;
                    break;
                }
            }

            if (falhouTeste)
            {
                return retorno;
            }

            retorno = true;

            return retorno;
        }

        public bool TrocarDataVencimentoListaItemLiberacao(int contratoId, Nullable<DateTime> dataVencimento, List<ItemLiberacaoDTO> listaItemLiberacaoDTO)
        {
            bool retorno = false;

            if (!UsuarioLogado.IsInRole(Funcionalidade.LiberacaoAlterarVencimento))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            if (!ValidarTrocaDataVencimentoListaItemLiberacao(contratoId,dataVencimento, listaItemLiberacaoDTO))
            {
                return retorno;
            }

            var specificationContrato = (Specification<Domain.Entity.Contrato.Contrato>)new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            Domain.Entity.Contrato.Contrato contrato = contratoRepository.ObterPeloId(contratoId, specificationContrato, l => l.ListaContratoRetificacaoProvisao, l => l.ListaContratoRetificacaoItemCronograma);

            bool trocou = false;
            foreach (ItemLiberacaoDTO item in listaItemLiberacaoDTO.Where(l => l.Selecionado == true && l.CodigoSituacao == (int)SituacaoMedicao.Provisionado))
            {
                ContratoRetificacaoProvisao contratoRetificacaoProvisao = contrato.ListaContratoRetificacaoProvisao.Where(l => l.Id == item.ContratoRetificacaoProvisaoId).FirstOrDefault();
                if (contratoRetificacaoProvisao != null)
                {
                    ContratoRetificacaoItemCronograma contratoRetificacaoItemCronograma = contrato.ListaContratoRetificacaoItemCronograma.Where(l => l.Id == contratoRetificacaoProvisao.ContratoRetificacaoItemCronogramaId).FirstOrDefault();
                    contratoRetificacaoItemCronograma.DataVencimento = dataVencimento.Value;
                    if (contrato.TipoContrato == TipoContrato.ContratoAPagar && contratoRetificacaoProvisao.TituloPagarId.HasValue)
                    {
                        TituloPagar tituloPagar = tituloPagarRepository.ListarPeloFiltro(l => l.Situacao < SituacaoTituloPagar.Emitido && l.Id == contratoRetificacaoProvisao.TituloPagarId.Value).FirstOrDefault();
                        tituloPagar.DataVencimento = dataVencimento.Value;
                    }
                    else
                    {
                        if (contrato.TipoContrato == TipoContrato.contratoAReceber && contratoRetificacaoProvisao.TituloReceberId.HasValue)
                        {
                            TituloReceber tituloReceber = tituloReceberRepository.ListarPeloFiltro(l => l.Situacao < SituacaoTituloReceber.Predatado && l.Id == contratoRetificacaoProvisao.TituloReceberId.Value).FirstOrDefault();
                            tituloReceber.DataVencimento = dataVencimento.Value;
                        }
                    }
                    trocou = true;
                }
                else
                {
                    break;
                }
            }

            if (!trocou)
            {
                messageQueue.Add("Existe um ou mais itens da lista desatualizados recupere o contrato novamente !", TypeMessage.Info);
                return retorno;
            }

            try
            {
                if (Validator.IsValid(contrato, out validationErrors))
                {
                    contratoRepository.Alterar(contrato);
                    contratoRepository.UnitOfWork.Commit();

                    messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                    retorno = true;
                    return retorno;
                }
                else
                {
                    messageQueue.AddRange(validationErrors, TypeMessage.Error);
                    return retorno;
                }
            }
            catch (Exception exception)
            {
                QueueExeptionMessages(exception);
            }

            return retorno;
        }

        public bool ValidarAssociacaoNotaFiscalListaItemLiberacao(int contratoId, List<ItemLiberacaoDTO> listaItemLiberacaoDTO, out ItemLiberacaoDTO itemLiberacao)
        {
            bool retorno = false;
            itemLiberacao = new ItemLiberacaoDTO();

            if (listaItemLiberacaoDTO == null)
            {
                messageQueue.Add("Nenhuma liberação foi selecionada", TypeMessage.Error);
                return retorno;
            }

            if (listaItemLiberacaoDTO.All(l => l.Selecionado == false))
            {
                messageQueue.Add("Nenhuma liberação foi selecionada", TypeMessage.Error);
                return retorno;
            }

            var specification = (Specification<Domain.Entity.Contrato.Contrato>)new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            Domain.Entity.Contrato.Contrato contrato = contratoRepository.ObterPeloId(contratoId, specification, l => l.ListaContratoRetificacaoItemMedicao);

            //RETORNA O ULTIMO ITEM SELECIONADO
            ItemLiberacaoDTO item = listaItemLiberacaoDTO.Where(l => l.Selecionado == true && l.CodigoSituacao < (int)SituacaoMedicao.Liberado).OrderByDescending(l => l.DataSelecionado).FirstOrDefault();

            if (item == null)
            {
                messageQueue.Add("Nenhuma liberação foi selecionada !", TypeMessage.Info);
                return retorno;
            }

            if (item.Selecionado == true && item.CodigoSituacao > (int)SituacaoMedicao.AguardandoLiberacao)
            {
                messageQueue.Add("O item escolhido não possui permissão para essa funcionalidade !", TypeMessage.Info);
                return retorno;
            }

            ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicaoSelecionado = contrato.ListaContratoRetificacaoItemMedicao.Where(l => l.Id == item.ContratoRetificacaoItemMedicaoId).FirstOrDefault();
            if (contratoRetificacaoItemMedicaoSelecionado.Situacao > SituacaoMedicao.AguardandoLiberacao)
            {
                messageQueue.Add("Existe um ou mais itens da lista desatualizados recupere o contrato novamente !", TypeMessage.Info);
                return retorno;
            }

            itemLiberacao = item;
            retorno = true;

            return retorno;
        }

        public bool AssociarNotaFiscalListaItemLiberacao(int? contratoId, int? contratoRetificacaoItemMedicaoId,int? tipoDocumentoId, string numeroDocumento, Nullable<DateTime> dataEmissao, Nullable<DateTime> dataVencimento)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.LiberacaoAssociarNF))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            bool retorno = false;

            if (!ValidacaoAssociacaoNotaFiscalListaItemLiberacao(contratoRetificacaoItemMedicaoId,tipoDocumentoId, numeroDocumento, dataEmissao, dataVencimento))
            {
                return retorno;
            }

            var specificationContrato = (Specification<Domain.Entity.Contrato.Contrato>)new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            Domain.Entity.Contrato.Contrato contrato = contratoRepository.ObterPeloId(contratoId, specificationContrato, l => l.ListaContratoRetificacaoItemMedicao);
            ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicaoSelecionado = contrato.ListaContratoRetificacaoItemMedicao.Where(l => l.Id == contratoRetificacaoItemMedicaoId).FirstOrDefault();
            int tipoDocumentoIdSelecionado = contratoRetificacaoItemMedicaoSelecionado.TipoDocumentoId;
            string numeroDocumentoSelecionado = contratoRetificacaoItemMedicaoSelecionado.NumeroDocumento;
            
            bool associou = false;
            foreach (ContratoRetificacaoItemMedicao medicao in contrato.ListaContratoRetificacaoItemMedicao.Where(l => l.TipoDocumentoId == tipoDocumentoIdSelecionado &&
                                                                                                                       l.NumeroDocumento == numeroDocumentoSelecionado &&
                                                                                                                       l.Situacao < SituacaoMedicao.Liberado))
            {
                medicao.TipoDocumentoId = tipoDocumentoId.Value;
                medicao.NumeroDocumento = numeroDocumento;
                medicao.DataEmissao = dataEmissao.Value;
                medicao.DataVencimento = dataVencimento.Value;
                associou = true;
            }

            if (!associou)
            {
                messageQueue.Add("Existe um ou mais itens da lista desatualizados recupere o contrato novamente !", TypeMessage.Info);
                return retorno;
            }

            try
            {
                if (Validator.IsValid(contrato, out validationErrors))
                {
                    contratoRepository.Alterar(contrato);
                    contratoRepository.UnitOfWork.Commit();

                    messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                    retorno = true;
                    return retorno;
                }
                else
                {
                    messageQueue.AddRange(validationErrors, TypeMessage.Error);
                    return retorno;
                }
            }
            catch (Exception exception)
            {
                QueueExeptionMessages(exception);
            }

            return retorno;
        }

        public bool AprovarLiberarListaItemLiberacao(int contratoId, List<ItemLiberacaoDTO> listaItemLiberacaoDTO , OperacaoLiberarMedicao operacao)
        {
            if (operacao == OperacaoLiberarMedicao.AprovarLiberar)
            {
                if (!UsuarioLogado.IsInRole(Funcionalidade.LiberacaoAprovarLiberar))
                {
                    messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                    return false;
                }
            }

            if (operacao == OperacaoLiberarMedicao.Liberar)
            {
                if (!UsuarioLogado.IsInRole(Funcionalidade.LiberacaoLiberar))
                {
                    messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                    return false;
                }
            }

            bool retorno = false;
            if (listaItemLiberacaoDTO == null)
            {
                messageQueue.Add("Nenhuma liberação foi selecionada", TypeMessage.Error);
                return retorno;
            }

            if (listaItemLiberacaoDTO.All(l => l.Selecionado == false))
            {
                messageQueue.Add("Nenhuma liberação foi selecionada", TypeMessage.Error);
                return retorno;
            }

            if (operacao == OperacaoLiberarMedicao.AprovarLiberar)
            {
                if (listaItemLiberacaoDTO.Any(l => l.Selecionado == true && l.CodigoSituacao > (int)SituacaoMedicao.AguardandoLiberacao))
                {
                    messageQueue.Add("Existe(m) medição(ões) com situação diferente de Aguardando aprovação ou Aguardando liberação!", TypeMessage.Info);
                    return retorno;
                }
            }
            else
            {
                if (operacao == OperacaoLiberarMedicao.Liberar)
                {
                    if (listaItemLiberacaoDTO.Any(l => l.Selecionado == true && l.CodigoSituacao != (int)SituacaoMedicao.AguardandoLiberacao))
                    {
                        messageQueue.Add("Existe(m) medição(ões) com situação diferente de Aguardando liberação!", TypeMessage.Info);
                        return retorno;
                    }
                }
            }

            var specification = (Specification<Domain.Entity.Contrato.Contrato>)new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            Domain.Entity.Contrato.Contrato contrato = contratoRepository.ObterPeloId(contratoId, specification, l => l.Contratado,
                                                                                                                 l => l.Contratante,
                                                                                                                 l => l.ContratoDescricao,
                                                                                                                 l => l.ListaContratoRetificacaoItem,
                                                                                                                 l => l.ListaContratoRetificacaoItemCronograma,
                                                                                                                 l => l.ListaContratoRetificacaoItemMedicao.Select(m => m.MultiFornecedor), 
                                                                                                                 l => l.ListaContratoRetificacaoItemMedicaoNF.Select(m => m.Material),
                                                                                                                 l => l.ListaContratoRetificacaoProvisao.Select(p => p.TituloPagar).Select(a => a.ListaApropriacao),
                                                                                                                 l => l.ListaContratoRetificacaoProvisao.Select(p => p.TituloPagar).Select(a => a.ListaImpostoPagar.Select(i => i.TituloPagarImposto)),
                                                                                                                 l => l.ListaContratoRetificacaoProvisao.Select(p => p.TituloPagar).Select(a => a.ListaImpostoPagar.Select(i => i.TituloPagarImposto.ListaApropriacao)),
                                                                                                                 l => l.ListaContratoRetificacaoProvisao.Select(p => p.TituloPagar).Select(a => a.ListaImpostoPagar.Select(i => i.TituloPagarImposto.ListaFilhos)),
                                                                                                                 l => l.ListaContratoRetificacaoProvisao.Select(p => p.TituloPagar).Select(a => a.ListaTituloPagarAdiantamento.Select(i => i.ListaApropriacaoAdiantamento)),
                                                                                                                 l => l.ListaContratoRetificacaoProvisao.Select(r => r.TituloReceber).Select(a => a.ListaApropriacao),
                                                                                                                 l => l.ListaContratoRetificacaoProvisao.Select(r => r.TituloReceber).Select(a => a.ListaImpostoReceber), 
                                                                                                                 l => l.ListaContratoRetificacaoItemImposto.Select(i => i.ImpostoFinanceiro).Select(c => c.Cliente),
                                                                                                                 l => l.ListaContratoRetencao,
                                                                                                                 l => l.ListaMovimento);
            ParametrosContrato parametros = parametrosContratoRepository.Obter();
            bool geraTituloSituacaoAguardandoLiberacao = parametros.GeraTituloAguardando.HasValue ? parametros.GeraTituloAguardando.Value : false;
            ParametrosFinanceiro parametrosFinanceiro = parametrosFinanceiroRepository.Obter();
            bool geraTituloImposto = parametrosFinanceiro.GeraTituloImposto.HasValue ? parametrosFinanceiro.GeraTituloImposto.Value : false;

            string msg = "";
            bool ehValido = true;
            foreach (ItemLiberacaoDTO item in listaItemLiberacaoDTO.Where(l => l.Selecionado == true && l.CodigoSituacao < (int)SituacaoMedicao.Liberado))
            {
                ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao = contrato.ListaContratoRetificacaoItemMedicao.Where(l => l.Id == item.ContratoRetificacaoItemMedicaoId).FirstOrDefault();
                Nullable<DateTime> dataBloqueio;

                if (contratoRetificacaoItemMedicao.Situacao > SituacaoMedicao.AguardandoLiberacao)
                {
                    msg = "Existe um ou mais itens da lista desatualizados recupere o contrato novamente !";
                    messageQueue.Add(msg, TypeMessage.Error);
                    ehValido = false;
                    break;
                }


                if (bloqueioContabilAppService.OcorreuBloqueioContabil(contrato.CodigoCentroCusto,
                                                                       contratoRetificacaoItemMedicao.DataEmissao,
                                                                       out dataBloqueio))
                {
                    msg = string.Format(Resource.Sigim.ErrorMessages.BloqueioContabilEncontrado, dataBloqueio.Value.ToShortDateString(), contrato.CodigoCentroCusto);
                    messageQueue.Add(msg, TypeMessage.Error);
                    ehValido = false;
                    break;
                }

                ClienteFornecedor cliente = ObtemClienteFornecedor(contrato,contratoRetificacaoItemMedicao);

                if (tituloPagarAppService.ExisteNumeroDocumento(contratoRetificacaoItemMedicao.DataEmissao, contratoRetificacaoItemMedicao.DataVencimento, contratoRetificacaoItemMedicao.NumeroDocumento, cliente.Id))
                {
                    msg = "Documento " + contratoRetificacaoItemMedicao.NumeroDocumento + " existente no Financeiro !";
                    messageQueue.Add(msg, TypeMessage.Error);
                    ehValido = false;
                    break;
                }
            }

            if (!ehValido)
            {
                return retorno;
            }

            try
            {
                List<ContratoRetificacaoItemMedicao> listaMedicaoSelecionadas = new List<ContratoRetificacaoItemMedicao>();

                CriarListaDeMedicaoSelecionadas(listaItemLiberacaoDTO, contrato, listaMedicaoSelecionadas, OperacaoLiberarMedicao.AprovarLiberar);
                AtualizarContratoRetencao(listaMedicaoSelecionadas, contrato);
                ReprovisionarContratoProvisao(listaMedicaoSelecionadas, contrato);
                GerarTitulosApropriadosLiberacao(listaMedicaoSelecionadas, contrato, geraTituloSituacaoAguardandoLiberacao, geraTituloImposto);
                GerarEntradaDeEstoque(listaMedicaoSelecionadas, contrato);

                if (Validator.IsValid(contrato, out validationErrors))
                {
                    contratoRepository.Alterar(contrato);
                    contratoRepository.UnitOfWork.Commit();

                    GravarLogOperacaoLiberacao(contrato, "INSERT");

                    messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                    retorno = true;
                    return retorno;
                }
                else
                {
                    messageQueue.AddRange(validationErrors, TypeMessage.Error);
                    return retorno;
                }
            }
            catch (Exception exception)
            {
                QueueExeptionMessages(exception);
            }

            return retorno;
        }

        public bool CancelarListaItemLiberacao(int contratoId, List<ItemLiberacaoDTO> listaItemLiberacaoDTO)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.LiberacaoCancelar))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            bool retorno = false;
            if (listaItemLiberacaoDTO == null)
            {
                messageQueue.Add("Nenhuma liberação foi selecionada", TypeMessage.Error);
                return retorno;
            }

            if (listaItemLiberacaoDTO.All(l => l.Selecionado == false))
            {
                messageQueue.Add("Nenhuma liberação foi selecionada", TypeMessage.Error);
                return retorno;
            }

            if (listaItemLiberacaoDTO.Any(l => l.Selecionado == true && l.CodigoSituacao != (int)SituacaoMedicao.Liberado))
            {
                messageQueue.Add("Existe(m) medição(ões) com situação diferente de liberada!", TypeMessage.Info);
                return retorno;
            }

            var specification = (Specification<Domain.Entity.Contrato.Contrato>)new TrueSpecification<Domain.Entity.Contrato.Contrato>();


            Domain.Entity.Contrato.Contrato contrato = contratoRepository.ObterPeloId(contratoId, specification, l => l.Contratado,
                                                                                                                 l => l.Contratante,
                                                                                                                 l => l.ContratoDescricao,
                                                                                                                 l => l.ListaContratoRetificacaoItem,
                                                                                                                 l => l.ListaContratoRetificacaoItemCronograma,
                                                                                                                 l => l.ListaContratoRetificacaoItemMedicao.Select(m => m.TituloPagar.ListaFilhos),
                                                                                                                 l => l.ListaContratoRetificacaoItemMedicao.Select(m => m.TituloPagar.ListaApropriacao),
                                                                                                                 l => l.ListaContratoRetificacaoItemMedicao.Select(m => m.TituloPagar.ListaImpostoPagar.Select(i => i.TituloPagarImposto)),
                                                                                                                 l => l.ListaContratoRetificacaoItemMedicao.Select(m => m.TituloPagar.ListaImpostoPagar.Select(i => i.TituloPagarImposto.ListaApropriacao)),
                                                                                                                 l => l.ListaContratoRetificacaoItemMedicao.Select(m => m.TituloPagar.ListaImpostoPagar.Select(i => i.TituloPagarImposto.ListaFilhos)),
                                                                                                                 l => l.ListaContratoRetificacaoItemMedicao.Select(m => m.TituloReceber.ListaFilhos),
                                                                                                                 l => l.ListaContratoRetificacaoItemMedicao.Select(m => m.TituloReceber.ListaApropriacao),
                                                                                                                 l => l.ListaContratoRetificacaoItemMedicao.Select(m => m.TituloReceber.ListaImpostoReceber),
                                                                                                                 l => l.ListaContratoRetificacaoProvisao.Select(p => p.TituloPagar).Select(a => a.ListaApropriacao),
                                                                                                                 l => l.ListaContratoRetificacaoProvisao.Select(p => p.TituloPagar).Select(a => a.ListaImpostoPagar.Select(i => i.TituloPagarImposto)),
                                                                                                                 l => l.ListaContratoRetificacaoProvisao.Select(p => p.TituloPagar).Select(a => a.ListaImpostoPagar.Select(i => i.TituloPagarImposto.ListaApropriacao)),
                                                                                                                 l => l.ListaContratoRetificacaoProvisao.Select(p => p.TituloPagar).Select(a => a.ListaImpostoPagar.Select(i => i.TituloPagarImposto.ListaFilhos)),
                                                                                                                 l => l.ListaContratoRetificacaoProvisao.Select(p => p.TituloPagar).Select(a => a.ListaTituloPagarAdiantamento.Select(pa => pa.ListaApropriacaoAdiantamento)),
                                                                                                                 l => l.ListaContratoRetificacaoProvisao.Select(r => r.TituloReceber).Select(a => a.ListaApropriacao),
                                                                                                                 l => l.ListaContratoRetificacaoProvisao.Select(r => r.TituloReceber).Select(a => a.ListaImpostoReceber),
                                                                                                                 l => l.ListaContratoRetificacaoItemImposto.Select(i => i.ImpostoFinanceiro),
                                                                                                                 l => l.ListaContratoRetencao.Select(rl => rl.ListaContratoRetencaoLiberada));

            string msg = "";
            bool ehValido = true;
            foreach (ItemLiberacaoDTO item in listaItemLiberacaoDTO.Where(l => l.Selecionado == true && l.CodigoSituacao == (int)SituacaoMedicao.Liberado))
            {
                ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao = contrato.ListaContratoRetificacaoItemMedicao.Where(l => l.Id == item.ContratoRetificacaoItemMedicaoId).FirstOrDefault();
                Nullable<DateTime> dataBloqueio;

                if (contratoRetificacaoItemMedicao.Situacao != SituacaoMedicao.Liberado)
                {
                    msg = "Existe um ou mais itens da lista desatualizados recupere o contrato novamente !";
                    messageQueue.Add(msg, TypeMessage.Error);
                    ehValido = false;
                    break;
                }

                if (bloqueioContabilAppService.OcorreuBloqueioContabil(contrato.CodigoCentroCusto,
                                                                       contratoRetificacaoItemMedicao.DataEmissao,
                                                                       out dataBloqueio))
                {
                    msg = string.Format(Resource.Sigim.ErrorMessages.BloqueioContabilEncontrado, dataBloqueio.Value.ToShortDateString(), contrato.CodigoCentroCusto);
                    messageQueue.Add(msg, TypeMessage.Error);
                    ehValido = false;
                    break;
                }

                bool existeRetencaoComLiberacao = VerificaExistenciaRetencaoLiberada(contrato, contratoRetificacaoItemMedicao);
                if (existeRetencaoComLiberacao)
                {
                    msg = "Existe(m) retenção(ões) já liberadas" ;
                    messageQueue.Add(msg, TypeMessage.Error);
                    ehValido = false;
                    break;
                }

                if (contrato.TipoContrato == TipoContrato.ContratoAPagar)
                {
                    if (JaHouvePagamentoDeTitulo(contratoRetificacaoItemMedicao.TituloPagar))
                    {
                        ehValido = false;
                        break;
                    }
                }
                else
                {
                    if (JaHouveRecebimentoDeTitulo(contratoRetificacaoItemMedicao.TituloReceber))
                    {
                        ehValido = false;
                        break;
                    }
                }
            }

            if (!ehValido)
            {
                return retorno;
            }

            try
            {
                List<ContratoRetificacaoItemMedicao> listaMedicaoSelecionadas = new List<ContratoRetificacaoItemMedicao>();

                CriarListaDeMedicaoSelecionadas(listaItemLiberacaoDTO, contrato, listaMedicaoSelecionadas, OperacaoLiberarMedicao.Cancelar);
                DeletarContratoRetencao(listaMedicaoSelecionadas, contrato);
                DeletarTitulosMedicao(listaMedicaoSelecionadas, contrato);
                EstornarContratoProvisao(listaMedicaoSelecionadas, contrato);
                EstornaMedicao(listaMedicaoSelecionadas, contrato);

                if (Validator.IsValid(contrato, out validationErrors))
                {
                    contratoRepository.Alterar(contrato);
                    contratoRepository.UnitOfWork.Commit();

                    GravarLogOperacaoCancelarLiberacao(contrato, "UPDATE");

                    messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                    retorno = true;
                    return retorno;
                }
                else
                {
                    messageQueue.AddRange(validationErrors, TypeMessage.Error);
                    return retorno;
                }
            }
            catch (Exception exception)
            {
                QueueExeptionMessages(exception);
            }

            return retorno;
        }

        #endregion

        #region Métodos Privados

        private void EstornarContratoProvisao(List<ContratoRetificacaoItemMedicao> listaMedicaoSelecionadas, Domain.Entity.Contrato.Contrato contrato)
        {
            ContratoRetificacao ultimoContratoRetificacao = contrato.ListaContratoRetificacao.Last();

            foreach (ContratoRetificacaoItemMedicao item in listaMedicaoSelecionadas)
            {
                ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao = contrato.ListaContratoRetificacaoItemMedicao.Where(l => l.Id == item.Id).FirstOrDefault();

                ContratoRetificacaoProvisao contratoRetificacaoProvisao = contrato.ListaContratoRetificacaoProvisao.Where(l => l.SequencialItem == contratoRetificacaoItemMedicao.SequencialItem &&
                                                                                                                               l.SequencialCronograma == contratoRetificacaoItemMedicao.SequencialCronograma).FirstOrDefault();

                bool? temPagamentoAntecipado = contratoRetificacaoItemMedicao.ContratoRetificacaoItemCronograma.EhPagamentoAntecipado;
                temPagamentoAntecipado = temPagamentoAntecipado.HasValue ? temPagamentoAntecipado.Value : false;

                #region "Trata Estorno do contrato de provisão para cronogramas que possuem adiantamento"
                if (temPagamentoAntecipado.Value)
                {
                    //#region "Estorna medição"

                    //contratoRetificacaoItemMedicao.DataLiberacao = null;
                    //contratoRetificacaoItemMedicao.UsuarioLiberacao = null;
                    //contratoRetificacaoItemMedicao.Situacao = SituacaoMedicao.AguardandoLiberacao;

                    //#endregion

                    #region "Estorna o valor medido no valor adiantado do provisionamento"

                    decimal valorDescontado = contratoRetificacaoProvisao.ValorAdiantadoDescontado.HasValue ? contratoRetificacaoProvisao.ValorAdiantadoDescontado.Value : 0;

                    contratoRetificacaoProvisao.ValorAdiantadoDescontado = valorDescontado - contratoRetificacaoItemMedicao.Valor;

                    #endregion

                    #region "Cancela o título de adiantamento da contabilidade"

                    TituloPagarAdiantamento tituloPagarAdiantamento = contratoRetificacaoProvisao.TituloPagar.
                                                                            ListaTituloPagarAdiantamento.Where(l =>
                                                                                                               l.DataEmissaoDocumento == contratoRetificacaoItemMedicao.DataEmissao &&
                                                                                                               l.TipoDocumentoId == contratoRetificacaoItemMedicao.TipoDocumentoId &&
                                                                                                               l.Documento == contratoRetificacaoItemMedicao.NumeroDocumento &&
                                                                                                               l.ValorAdiantamento == contratoRetificacaoItemMedicao.Valor).FirstOrDefault();
                    if (tituloPagarAdiantamento != null)
                    {
                        DeletarTituloPagarAdiantamentoEhApropriacoes(tituloPagarAdiantamento);
                    }

                    #endregion
                }
                #endregion

                #region "Trata Estorno do contrato de provisão para cronogramas que NÃO possuem adiantamento"

                if (!temPagamentoAntecipado.Value)
                {
                    if (contratoRetificacaoProvisao != null)
                    {
                        EstornaProvisionamentoParcial(contrato, contratoRetificacaoItemMedicao, ultimoContratoRetificacao, contratoRetificacaoProvisao);
                    }
                    else
                    {
                        EstornaProvisionamentoTotal(contrato, contratoRetificacaoItemMedicao, ultimoContratoRetificacao, contratoRetificacaoProvisao);
                    }
                }

                #endregion
            }
        }

        private void EstornaProvisionamentoParcial(Domain.Entity.Contrato.Contrato contrato, ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao, ContratoRetificacao ultimoContratoRetificacao, ContratoRetificacaoProvisao contratoRetificacaoProvisao)
        {
            if (contratoRetificacaoProvisao != null)
            {
                #region "Estorna valor e quantidade da medição na provisão"

                contratoRetificacaoProvisao.Quantidade = contratoRetificacaoProvisao.Quantidade + contratoRetificacaoItemMedicao.Quantidade;
                contratoRetificacaoProvisao.Valor = contratoRetificacaoProvisao.Valor + contratoRetificacaoItemMedicao.Valor;

                #endregion

                #region "Calcula o novo valor retido na provisão"

                decimal valorRetidoNovo = 0;
                if (contratoRetificacaoItemMedicao.ValorRetido.HasValue)
                {
                    decimal percentualRetencao = ((contratoRetificacaoItemMedicao.ValorRetido.Value / contratoRetificacaoItemMedicao.Valor) * 100);
                    valorRetidoNovo = Math.Round(((contratoRetificacaoProvisao.Valor * percentualRetencao) / 100), 2);
                }

                #endregion

                #region "Aponta a medição cancelada para as mesmas informações da provisão"

                if (contratoRetificacaoProvisao.ContratoRetificacaoId != ultimoContratoRetificacao.Id)
                {
                    //*********************************************************
                    //VER COM A ROSANNE SE O TRATAMENTO ABAIXO ESTÁ CORRETO
                    //*********************************************************

                    contratoRetificacaoItemMedicao.ContratoRetificacao = contratoRetificacaoProvisao.ContratoRetificacao;
                    contratoRetificacaoItemMedicao.ContratoRetificacaoId = contratoRetificacaoProvisao.ContratoRetificacaoId;
                    contratoRetificacaoItemMedicao.ContratoRetificacaoItem = contratoRetificacaoProvisao.ContratoRetificacaoItem;
                    contratoRetificacaoItemMedicao.ContratoRetificacaoItemId = contratoRetificacaoProvisao.ContratoRetificacaoItemId.Value;
                    contratoRetificacaoItemMedicao.ContratoRetificacaoItemCronograma = contratoRetificacaoProvisao.ContratoRetificacaoItemCronograma;
                    contratoRetificacaoItemMedicao.ContratoRetificacaoItemCronogramaId = contratoRetificacaoProvisao.ContratoRetificacaoItemCronogramaId.Value;
                }

                #endregion

                #region "Atualiza os titulo, apropriação e deleta imposto da provisão"

                if (contrato.TipoContrato == TipoContrato.ContratoAPagar)
                {
                    #region "Atualiza os titulo a pagar e a apropriação da provisão e deleta impostoPagar, titulo e Apropriações"

                    if (contratoRetificacaoProvisao.TituloPagar.Situacao < SituacaoTituloPagar.Emitido)
                    {
                        contratoRetificacaoProvisao.TituloPagar.TipoDocumentoId = null;
                        contratoRetificacaoProvisao.TituloPagar.TipoDocumento = null;
                        contratoRetificacaoProvisao.TituloPagar.ValorTitulo = contratoRetificacaoProvisao.Valor;
                        contratoRetificacaoProvisao.TituloPagar.Retencao = valorRetidoNovo;
                        contratoRetificacaoProvisao.TituloPagar.Situacao = SituacaoTituloPagar.Provisionado;

                        foreach (Apropriacao apropriacao in contratoRetificacaoProvisao.TituloPagar.ListaApropriacao)
                        {
                            apropriacao.Valor = Math.Round((contratoRetificacaoProvisao.Valor - valorRetidoNovo), 2);
                            apropriacao.Percentual = Math.Round((((contratoRetificacaoProvisao.Valor - valorRetidoNovo) / contratoRetificacaoProvisao.Valor) * 100), 5);
                        }

                        DeletarTituloPagarEhApropriacoesComDesdobradosDeImpostoPagar(contratoRetificacaoProvisao.TituloPagar);
                        RemoverImpostosDoTituloAPagar(contratoRetificacaoProvisao.TituloPagar);
                    }

                    #endregion
                }
                else
                {
                    #region "Atualiza os titulo a receber e a apropriação da provisão e deleta impostoReceber"

                    if (contratoRetificacaoProvisao.TituloReceber.Situacao < SituacaoTituloReceber.Predatado)
                    {
                        contratoRetificacaoProvisao.TituloReceber.TipoDocumentoId = null;
                        contratoRetificacaoProvisao.TituloReceber.TipoDocumento = null;
                        contratoRetificacaoProvisao.TituloReceber.ValorTitulo = contratoRetificacaoProvisao.Valor;
                        contratoRetificacaoProvisao.TituloReceber.Retencao = valorRetidoNovo;
                        contratoRetificacaoProvisao.TituloReceber.Situacao = SituacaoTituloReceber.Provisionado;

                        foreach (Apropriacao apropriacao in contratoRetificacaoProvisao.TituloReceber.ListaApropriacao)
                        {
                            apropriacao.Valor = Math.Round((contratoRetificacaoProvisao.Valor - valorRetidoNovo), 2);
                            apropriacao.Percentual = Math.Round((((contratoRetificacaoProvisao.Valor - valorRetidoNovo) / contratoRetificacaoProvisao.Valor) * 100), 5);
                        }

                        RemoverImpostosDoTituloAReceber(contratoRetificacaoProvisao.TituloReceber);
                    }

                    #endregion
                }
                #endregion

                #region "Cria o(s) imposto(s) da provisão"

                decimal valorImpostoRetidoTotal = 0;
                foreach (ContratoRetificacaoItemImposto contratoRetificacaoItemImposto in contrato.ListaContratoRetificacaoItemImposto.Where(l => l.ContratoRetificacaoItemId == contratoRetificacaoProvisao.ContratoRetificacaoItemId))
                {
                    #region "Calcula base de cálculo e imposto"

                    decimal valorBaseCalculo = (contratoRetificacaoProvisao.Valor * contratoRetificacaoItemImposto.PercentualBaseCalculo) / 100;
                    decimal valorImposto = (valorBaseCalculo * contratoRetificacaoItemImposto.ImpostoFinanceiro.Aliquota) / 100;

                    #endregion
                    if (valorBaseCalculo > 0)
                    {
                        bool ehRetido = contratoRetificacaoItemImposto.ImpostoFinanceiro.EhRetido.HasValue ? contratoRetificacaoItemImposto.ImpostoFinanceiro.EhRetido.Value : false;
                        if (ehRetido)
                        {
                            valorImpostoRetidoTotal = valorImpostoRetidoTotal + valorImposto;
                        }

                        if (contrato.TipoContrato == TipoContrato.ContratoAPagar)
                        {
                            #region "Calcula imposto a pagar"

                            ImpostoPagar impostoPagar = null;
                            if (contratoRetificacaoProvisao.TituloPagar.ListaImpostoPagar.Count > 0 && contratoRetificacaoProvisao.TituloPagar.ListaImpostoPagar.All(l => !l.Id.HasValue))
                            {
                                impostoPagar = contratoRetificacaoProvisao.TituloPagar.ListaImpostoPagar.Where(l => l.ImpostoFinanceiroId == contratoRetificacaoItemImposto.ImpostoFinanceiroId).FirstOrDefault();
                                if (impostoPagar != null)
                                {
                                    impostoPagar.BaseCalculo = valorBaseCalculo;
                                    impostoPagar.ValorImposto = valorImposto;
                                }
                                else
                                {
                                    impostoPagar = new ImpostoPagar();
                                    PreencherImpostoPagar(impostoPagar,
                                                          contratoRetificacaoItemImposto.ImpostoFinanceiroId,
                                                          contratoRetificacaoProvisao.TituloPagar,
                                                          valorBaseCalculo,
                                                          valorImposto,
                                                          null);

                                    contratoRetificacaoProvisao.TituloPagar.ListaImpostoPagar.Add(impostoPagar);
                                }
                            }
                            else
                            {
                                impostoPagar = new ImpostoPagar();
                                PreencherImpostoPagar(impostoPagar,
                                                      contratoRetificacaoItemImposto.ImpostoFinanceiroId,
                                                      contratoRetificacaoProvisao.TituloPagar,
                                                      valorBaseCalculo,
                                                      valorImposto,
                                                      null);

                                contratoRetificacaoProvisao.TituloPagar.ListaImpostoPagar.Add(impostoPagar);
                            }

                            #endregion
                        }
                        else
                        {
                            #region "Calcula imposto a receber"

                            ImpostoReceber impostoReceber = null;
                            if (contratoRetificacaoProvisao.TituloReceber.ListaImpostoReceber.Count > 0 && contratoRetificacaoProvisao.TituloReceber.ListaImpostoReceber.All(l => !l.Id.HasValue))
                            {
                                impostoReceber = contratoRetificacaoProvisao.TituloReceber.ListaImpostoReceber.Where(l => l.ImpostoFinanceiroId == contratoRetificacaoItemImposto.ImpostoFinanceiroId).FirstOrDefault();
                                if (impostoReceber != null)
                                {
                                    impostoReceber.BaseCalculo = valorBaseCalculo;
                                    impostoReceber.ValorImposto = valorImposto;
                                }
                                else
                                {
                                    impostoReceber = new ImpostoReceber();
                                    PreencherImpostoReceber(impostoReceber,
                                                            contratoRetificacaoItemImposto.ImpostoFinanceiroId,
                                                            contratoRetificacaoProvisao.TituloReceber,
                                                            valorBaseCalculo,
                                                            valorImposto);
                                    contratoRetificacaoProvisao.TituloReceber.ListaImpostoReceber.Add(impostoReceber);
                                }
                            }
                            else
                            {
                                impostoReceber = new ImpostoReceber();
                                PreencherImpostoReceber(impostoReceber,
                                                        contratoRetificacaoItemImposto.ImpostoFinanceiroId,
                                                        contratoRetificacaoProvisao.TituloReceber,
                                                        valorBaseCalculo,
                                                        valorImposto);
                                contratoRetificacaoProvisao.TituloReceber.ListaImpostoReceber.Add(impostoReceber);
                            }

                            #endregion
                        }
                    }
                }
                if (contrato.TipoContrato == TipoContrato.ContratoAPagar)
                {
                    if (valorImpostoRetidoTotal > 0)
                    {
                        contratoRetificacaoProvisao.TituloPagar.ValorImposto = valorImpostoRetidoTotal;
                    }
                }
                else
                {
                    if (valorImpostoRetidoTotal > 0)
                    {
                        contratoRetificacaoProvisao.TituloReceber.ValorImposto = valorImpostoRetidoTotal;
                    }
                }
                #endregion

            }
        }

        private void EstornaProvisionamentoTotal(Domain.Entity.Contrato.Contrato contrato, ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao,ContratoRetificacao ultimoContratoRetificacao, ContratoRetificacaoProvisao contratoRetificacaoProvisao)
        {
            if (contratoRetificacaoProvisao == null)
            {
                ClienteFornecedor cliente = ObtemClienteFornecedor(contrato, null);
                string identificacao = MontaIdentificacaoTitulo(contrato);
                DateTime dataEmissao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                DateTime dataVencimento = contratoRetificacaoItemMedicao.DataVencimento;
                if (dataVencimento < dataEmissao)
                {
                    dataVencimento = dataEmissao;
                }

                #region "Calcula os valores do provisionamento a ser criado"

                decimal valorRetido = contratoRetificacaoItemMedicao.ValorRetido.HasValue ? contratoRetificacaoItemMedicao.ValorRetido.Value : 0;
                decimal valorApropriado = contratoRetificacaoItemMedicao.Valor - valorRetido;
                decimal percentualApropriado = (valorApropriado / contratoRetificacaoItemMedicao.Valor) * 100;

                #endregion

                #region "Aponta a medição cancelada para as mesmas informações da provisão"

                if (ultimoContratoRetificacao.Id != contratoRetificacaoItemMedicao.ContratoRetificacaoId)
                {
                    ContratoRetificacaoItem ultimoContratoRetificacaoItem =
                        contrato.ListaContratoRetificacaoItem.Where(l =>
                                                                    l.ContratoRetificacaoId == ultimoContratoRetificacao.Id &&
                                                                    l.Sequencial == contratoRetificacaoItemMedicao.SequencialItem).FirstOrDefault();
                    ContratoRetificacaoItemCronograma ultimoContratoRetificacaoItemCronograma =
                        contrato.ListaContratoRetificacaoItemCronograma.Where(l =>
                                                                              l.ContratoRetificacaoId == ultimoContratoRetificacao.Id &&
                                                                              l.ContratoRetificacaoItemId == ultimoContratoRetificacaoItem.Id &&
                                                                              l.Sequencial == contratoRetificacaoItemMedicao.SequencialCronograma).FirstOrDefault();

                    //*********************************************************
                    //VER COM A ROSANNE SE O TRATAMENTO ABAIXO ESTÁ CORRETO
                    //*********************************************************

                    contratoRetificacaoItemMedicao.ContratoRetificacao = ultimoContratoRetificacao;
                    contratoRetificacaoItemMedicao.ContratoRetificacaoId = ultimoContratoRetificacao.Id.Value;
                    contratoRetificacaoItemMedicao.ContratoRetificacaoItem = ultimoContratoRetificacaoItem;
                    contratoRetificacaoItemMedicao.ContratoRetificacaoItemId = ultimoContratoRetificacaoItem.Id.Value;
                    contratoRetificacaoItemMedicao.ContratoRetificacaoItemCronograma = ultimoContratoRetificacaoItemCronograma;
                    contratoRetificacaoItemMedicao.ContratoRetificacaoItemCronogramaId = ultimoContratoRetificacaoItemCronograma.Id.Value;
                }

                #endregion

                #region "Cria os títulos e apropriação a pagar e receber do provisionamento"

                if (contrato.TipoContrato == TipoContrato.ContratoAPagar)
                {
                    #region "Cria os títulos e apropriação a pagar do provisionamento"

                    TituloPagar tituloPagarProvisao = new TituloPagar();
                    PreencherTituloPagar(tituloPagarProvisao,
                                         cliente.Id,
                                         identificacao,
                                         SituacaoTituloPagar.Provisionado,
                                         null,
                                         null,
                                         dataVencimento,
                                         dataEmissao,
                                         contratoRetificacaoItemMedicao.Valor,
                                         null,
                                         null,
                                         contratoRetificacaoItemMedicao.ValorRetido,
                                         null);

                    Apropriacao apropriacaoProvisao = new Apropriacao();
                    PreencherApropriacaoTitulo(apropriacaoProvisao,
                                               contratoRetificacaoItemMedicao.ContratoRetificacaoItem.CodigoClasse,
                                               contrato.CodigoCentroCusto,
                                               tituloPagarProvisao,
                                               null,
                                               percentualApropriado,
                                               valorApropriado);

                    tituloPagarProvisao.ListaApropriacao.Add(apropriacaoProvisao);

                    #endregion

                    #region "Cria impostos a pagar do provisionamento"

                    decimal valorImpostoRetidoTotal = 0;
                    foreach (ContratoRetificacaoItemImposto contratoRetificacaoItemImposto in contrato.ListaContratoRetificacaoItemImposto.Where(l => l.ContratoRetificacaoItemId == contratoRetificacaoItemMedicao.ContratoRetificacaoItemId))
                    {
                        #region "Calcula base de cálculo e imposto"

                        decimal valorBaseCalculo = (contratoRetificacaoItemMedicao.Valor * contratoRetificacaoItemImposto.PercentualBaseCalculo) / 100;
                        decimal valorImposto = (valorBaseCalculo * contratoRetificacaoItemImposto.ImpostoFinanceiro.Aliquota) / 100;
                        bool ehRetido = contratoRetificacaoItemImposto.ImpostoFinanceiro.EhRetido.HasValue ? contratoRetificacaoItemImposto.ImpostoFinanceiro.EhRetido.Value : false;
                        if (ehRetido)
                        {
                            valorImpostoRetidoTotal = valorImpostoRetidoTotal + valorImposto;
                        }

                        #endregion

                        #region "Calcula imposto a pagar"

                        ImpostoPagar impostoPagar = new ImpostoPagar();
                        PreencherImpostoPagar(impostoPagar,
                                              contratoRetificacaoItemImposto.ImpostoFinanceiroId,
                                              tituloPagarProvisao, 
                                              valorBaseCalculo, 
                                              valorImposto, 
                                              null);
                        if (valorImpostoRetidoTotal > 0)
                        {
                            tituloPagarProvisao.ValorImposto = valorImpostoRetidoTotal;
                        }

                        tituloPagarProvisao.ListaImpostoPagar.Add(impostoPagar);

                        #endregion
                    }

                    #endregion

                    #region "Cria a nova provisão"

                    contratoRetificacaoProvisao = new ContratoRetificacaoProvisao();

                    PreencherContratoRetificacaoProvisao(contratoRetificacaoProvisao,
                                                         contrato.Id.Value,
                                                         ultimoContratoRetificacao.Id.Value,
                                                         contratoRetificacaoItemMedicao.ContratoRetificacaoItemId,
                                                         contratoRetificacaoItemMedicao.ContratoRetificacaoItemCronogramaId,
                                                         contratoRetificacaoItemMedicao.SequencialItem,
                                                         contratoRetificacaoItemMedicao.SequencialCronograma,
                                                         tituloPagarProvisao,
                                                         null,
                                                         contratoRetificacaoItemMedicao.Valor,
                                                         contratoRetificacaoItemMedicao.Quantidade);

                    contrato.ListaContratoRetificacaoProvisao.Add(contratoRetificacaoProvisao);

                    #endregion
                }
                else
                {
                    #region "Cria os títulos e apropriação a receber do provisionamento"

                    TituloReceber tituloReceberProvisao = new TituloReceber();
                    PreencherTituloReceber(tituloReceberProvisao,
                                           cliente.Id,
                                           identificacao,
                                           SituacaoTituloReceber.Provisionado,
                                           null,
                                           null,
                                           dataVencimento,
                                           dataEmissao,
                                           contratoRetificacaoItemMedicao.Valor,
                                           null,
                                           null,
                                           contratoRetificacaoItemMedicao.ValorRetido,
                                           null);

                    Apropriacao apropriacaoProvisao = new Apropriacao();
                    PreencherApropriacaoTitulo(apropriacaoProvisao,
                                               contratoRetificacaoItemMedicao.ContratoRetificacaoItem.CodigoClasse,
                                               contrato.CodigoCentroCusto,
                                               null,
                                               tituloReceberProvisao,
                                               percentualApropriado,
                                               valorApropriado);

                    tituloReceberProvisao.ListaApropriacao.Add(apropriacaoProvisao);

                    #endregion

                    #region "Cria impostos receber do provisionamento"

                    decimal valorImpostoRetidoTotal = 0;
                    foreach (ContratoRetificacaoItemImposto contratoRetificacaoItemImposto in contrato.ListaContratoRetificacaoItemImposto.Where(l => l.ContratoRetificacaoItemId == contratoRetificacaoItemMedicao.ContratoRetificacaoItemId))
                    {
                        #region "Calcula base de cálculo e imposto"

                        decimal valorBaseCalculo = (contratoRetificacaoItemMedicao.Valor * contratoRetificacaoItemImposto.PercentualBaseCalculo) / 100;
                        decimal valorImposto = (valorBaseCalculo * contratoRetificacaoItemImposto.ImpostoFinanceiro.Aliquota) / 100;
                        bool ehRetido = contratoRetificacaoItemImposto.ImpostoFinanceiro.EhRetido.HasValue ? contratoRetificacaoItemImposto.ImpostoFinanceiro.EhRetido.Value : false;
                        if (ehRetido)
                        {
                            valorImpostoRetidoTotal = valorImpostoRetidoTotal + valorImposto;
                        }

                        #endregion

                        #region "Calcula imposto a receber"

                        ImpostoReceber impostoReceber = new ImpostoReceber();
                        PreencherImpostoReceber(impostoReceber, contratoRetificacaoItemImposto.ImpostoFinanceiroId, tituloReceberProvisao, valorBaseCalculo, valorImposto);
                        tituloReceberProvisao.ValorImposto = null;
                        if (valorImpostoRetidoTotal > 0)
                        {
                            tituloReceberProvisao.ValorImposto = valorImpostoRetidoTotal;
                        }

                        tituloReceberProvisao.ListaImpostoReceber.Add(impostoReceber);

                        #endregion
                    }

                    #endregion

                    #region "Cria a nova provisão"

                    contratoRetificacaoProvisao = new ContratoRetificacaoProvisao();

                    PreencherContratoRetificacaoProvisao(contratoRetificacaoProvisao,
                                                         contrato.Id.Value,
                                                         ultimoContratoRetificacao.Id.Value,
                                                         contratoRetificacaoItemMedicao.ContratoRetificacaoItemId,
                                                         contratoRetificacaoItemMedicao.ContratoRetificacaoItemCronogramaId,
                                                         contratoRetificacaoItemMedicao.SequencialItem,
                                                         contratoRetificacaoItemMedicao.SequencialCronograma,
                                                         null,
                                                         tituloReceberProvisao,
                                                         contratoRetificacaoItemMedicao.Valor,
                                                         contratoRetificacaoItemMedicao.Quantidade);

                    contrato.ListaContratoRetificacaoProvisao.Add(contratoRetificacaoProvisao);

                    #endregion

                }

                #endregion
            }
        }

        private void PreencherContratoRetificacaoProvisao(ContratoRetificacaoProvisao contratoRetificacaoProvisao,
                                                          int contratoId,
                                                          int contratoRetificacaoId,
                                                          int contratoRetificacaoItemId,
                                                          int contratoRetificacaoItemCronogramaId,
                                                          int sequencialItem,
                                                          int sequencialCronograma,
                                                          TituloPagar tituloPagar,
                                                          TituloReceber tituloReceber,
                                                          decimal valor,
                                                          decimal quantidade){
                contratoRetificacaoProvisao.ContratoId = contratoId;
                contratoRetificacaoProvisao.ContratoRetificacaoId = contratoRetificacaoId;
                contratoRetificacaoProvisao.ContratoRetificacaoItemId = contratoRetificacaoItemId;
                contratoRetificacaoProvisao.SequencialItem = sequencialItem;
                contratoRetificacaoProvisao.ContratoRetificacaoItemCronogramaId = contratoRetificacaoItemCronogramaId;
                contratoRetificacaoProvisao.SequencialCronograma = sequencialCronograma;
                contratoRetificacaoProvisao.TituloPagar = tituloPagar;
                contratoRetificacaoProvisao.TituloReceber = tituloReceber;
                contratoRetificacaoProvisao.Valor = valor;
                contratoRetificacaoProvisao.Quantidade = quantidade;
        }

        private void EstornaMedicao(List<ContratoRetificacaoItemMedicao> listaMedicaoSelecionadas, Domain.Entity.Contrato.Contrato contrato)
        {
            foreach (ContratoRetificacaoItemMedicao item in listaMedicaoSelecionadas)
            {
                ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao = contrato.ListaContratoRetificacaoItemMedicao.Where(l => l.Id == item.Id).FirstOrDefault();
                contratoRetificacaoItemMedicao.DataLiberacao = null;
                contratoRetificacaoItemMedicao.UsuarioLiberacao = null;
                contratoRetificacaoItemMedicao.Situacao = SituacaoMedicao.AguardandoLiberacao;
            }
        }

        private void DeletarTitulosMedicao(List<ContratoRetificacaoItemMedicao> listaMedicaoSelecionadas, Domain.Entity.Contrato.Contrato contrato)
        {
            if (contrato.TipoContrato == TipoContrato.ContratoAPagar)
            {
                foreach (ContratoRetificacaoItemMedicao item in listaMedicaoSelecionadas.Where(l => l.TituloPagarId.HasValue).GroupBy(l => l.TituloPagarId.Value).Select(o => o.First()).ToList())
                {
                    ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao = contrato.ListaContratoRetificacaoItemMedicao.Where(l => l.Id == item.Id).FirstOrDefault();
                    DeletarTituloPagarEhApropriacoesComDesdobradosDeImpostoPagar(contratoRetificacaoItemMedicao.TituloPagar);
                    RemoverImpostosDoTituloAPagar(contratoRetificacaoItemMedicao.TituloPagar);
                    DeletarTituloPagarEhApropriacoesComDesdobrados(contratoRetificacaoItemMedicao.TituloPagar);
                }
            }
            else
            {
                foreach (ContratoRetificacaoItemMedicao item in listaMedicaoSelecionadas.GroupBy(l => l.TituloReceberId.Value).Select(o => o.First()).ToList())
                {
                    ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao = contrato.ListaContratoRetificacaoItemMedicao.Where(l => l.Id == item.Id).FirstOrDefault();
                    RemoverImpostosDoTituloAReceber(contratoRetificacaoItemMedicao.TituloReceber);
                    DeletarTituloReceberEhApropriacoesComDesdobrados(contratoRetificacaoItemMedicao.TituloReceber);
                }
            }
        }

        private void DeletarTituloPagarEhApropriacoesComDesdobradosDeImpostoPagar(TituloPagar tituloPagar)
        {
            if (tituloPagar != null)
            {
                foreach (var item in tituloPagar.ListaImpostoPagar)
                {
                    if (item.TituloPagarImpostoId.HasValue && item.TituloPagarImposto.TipoTitulo == TipoTitulo.Pai)
                    {
                        DeletarTituloPagarDesdobradoEhApropriacoes(item.TituloPagarImposto.ListaFilhos.ToList());
                    }
                    else
                    {
                        DeletarTituloPagarEhApropriacoes(item.TituloPagarImposto);
                    }
                }
            }
        }

        private void DeletarTituloPagarEhApropriacoesComDesdobrados(TituloPagar tituloPagar)
        {
            if (tituloPagar != null)
            {
                if (tituloPagar.TipoTitulo == TipoTitulo.Pai)
                {
                    DeletarTituloPagarDesdobradoEhApropriacoes(tituloPagar.ListaFilhos.ToList());
                }
                else
                {
                    DeletarTituloPagarEhApropriacoes(tituloPagar);
                }
            }
        }

        private void DeletarTituloReceberEhApropriacoesComDesdobrados(TituloReceber tituloReceber)
        {
            if (tituloReceber != null)
            {
                if (tituloReceber.TipoTitulo == TipoTitulo.Pai)
                {
                    DeletarTituloReceberDesdobradoEhApropriacoes(tituloReceber.ListaFilhos.ToList());
                }
                else
                {
                    DeletarTituloReceberEhApropriacoes(tituloReceber);
                }
            }
        }

        private void DeletarTituloPagarDesdobradoEhApropriacoes(List<TituloPagar> listaTituloPagarFilhos)
        {
            foreach (var titulo in listaTituloPagarFilhos)
            {
                if (titulo.TipoTitulo == TipoTitulo.Pai)
                {
                    DeletarTituloPagarDesdobradoEhApropriacoes(titulo.ListaFilhos.ToList());
                }

                DeletarTituloPagarEhApropriacoes(titulo);
            }
        }

        private void DeletarTituloReceberDesdobradoEhApropriacoes(List<TituloReceber> listaTituloReceberFilhos)
        {
            foreach (var titulo in listaTituloReceberFilhos)
            {
                if (titulo.TipoTitulo == TipoTitulo.Pai)
                {
                    DeletarTituloReceberDesdobradoEhApropriacoes(titulo.ListaFilhos.ToList());
                }

                DeletarTituloReceberEhApropriacoes(titulo);
            }
        }

        private void DeletarTituloPagarEhApropriacoes(TituloPagar titulo)
        {
            if (titulo != null)
            {
                RemoverApropriacoesDoTituloAPagar(titulo);
                RemoverTituloPagar(titulo);
            }
        }

        private void DeletarTituloPagarAdiantamentoEhApropriacoes(TituloPagarAdiantamento tituloPagarAdiantamento)
        {
            if (tituloPagarAdiantamento != null)
            {
                RemoverApropriacoesDoTituloAPagarAdiantamento(tituloPagarAdiantamento);
                RemoverTituloPagarAdiantamento(tituloPagarAdiantamento);
            }
        }

        private void DeletarTituloReceberEhApropriacoes(TituloReceber titulo)
        {
            if (titulo != null)
            {
                RemoverApropriacoesDoTituloAReceber(titulo);
                RemoverTituloReceber(titulo);
            }
        }

        private void RemoverTituloPagar(TituloPagar titulo)
        {
            if (titulo != null)
            {
                tituloPagarRepository.RemoverTituloPagar(titulo);
            }
        }

        private void RemoverTituloPagarAdiantamento(TituloPagarAdiantamento tituloAdiantamento)
        {
            if (tituloAdiantamento != null)
            {
                tituloPagarRepository.RemoverTituloPagarAdiantamento(tituloAdiantamento);
            }
        }

        private void RemoverTituloReceber(TituloReceber titulo)
        {
            if (titulo != null)
            {
                tituloReceberRepository.RemoverTituloReceber(titulo);
            }
        }

        private void RemoverApropriacoesDoTituloAPagar(TituloPagar titulo)
        {
            if (titulo.ListaApropriacao.All(l => l.Id.HasValue))
            {
                for (int i = titulo.ListaApropriacao.Count() - 1; i >= 0; i--)
                {
                    var apropriacao = titulo.ListaApropriacao.ToList()[i];
                    tituloPagarRepository.RemoverApropriacao(apropriacao);
                }
            }
        }

        private void RemoverApropriacoesDoTituloAPagarAdiantamento(TituloPagarAdiantamento tituloAdiantamento)
        {
            if (tituloAdiantamento.ListaApropriacaoAdiantamento.All(l => l.Id.HasValue))
            {
                for (int i = tituloAdiantamento.ListaApropriacaoAdiantamento.Count() - 1; i >= 0; i--)
                {
                    ApropriacaoAdiantamento apropriacaoAdiantamento = tituloAdiantamento.ListaApropriacaoAdiantamento.ToList()[i];
                    tituloPagarRepository.RemoverApropriacaoAdiantamento(apropriacaoAdiantamento);
                }
            }
        }

        private void RemoverApropriacoesDoTituloAReceber(TituloReceber titulo)
        {
            if (titulo.ListaApropriacao.All(l => l.Id.HasValue))
            {
                for (int i = titulo.ListaApropriacao.Count() - 1; i >= 0; i--)
                {
                    var apropriacao = titulo.ListaApropriacao.ToList()[i];
                    tituloReceberRepository.RemoverApropriacao(apropriacao);
                }
            }
        }

        private void RemoverImpostosDoTituloAPagar(TituloPagar titulo)
        {
            if (titulo.ListaImpostoPagar.All(l => l.Id.HasValue))
            {
                for (int i = titulo.ListaImpostoPagar.Count() - 1; i >= 0; i--)
                {
                    var impostoPagar = titulo.ListaImpostoPagar.ToList()[i];
                    impostoPagarRepository.Remover(impostoPagar);
                }
            }
        }

        private void RemoverImpostosDoTituloAReceber(TituloReceber titulo)
        {
            if (titulo.ListaImpostoReceber.All(l => l.Id.HasValue))
            {
                for (int i = titulo.ListaImpostoReceber.Count() - 1; i >= 0; i--)
                {
                    var impostoReceber = titulo.ListaImpostoReceber.ToList()[i];
                    impostoReceberRepository.Remover(impostoReceber);
                }
            }
        }

        private bool JaHouvePagamentoDeTitulo(TituloPagar tituloPagarMedicao)
        {
            int? tituloPagarId;
            if (PossuiTituloPago(tituloPagarMedicao, out tituloPagarId))
            {
                var msg = string.Format(Resource.Contrato.ErrorMessages.TituloEstaPago, tituloPagarId.ToString());
                messageQueue.Add(msg, TypeMessage.Error);
                return true;
            }

            if (PossuiTituloImpostoPago(tituloPagarMedicao, out tituloPagarId))
            {
                var msg = string.Format(Resource.Contrato.ErrorMessages.TituloImpostoEstaPago, tituloPagarId.ToString());
                messageQueue.Add(msg, TypeMessage.Error);
                return true;
            }

            return false;
        }

        private bool JaHouveRecebimentoDeTitulo(TituloReceber tituloReceberMedicao)
        {
            int? tituloReceberId;
            if (PossuiTituloRecebido(tituloReceberMedicao, out tituloReceberId))
            {
                var msg = string.Format(Resource.Contrato.ErrorMessages.TituloEstaRecebido, tituloReceberId.ToString());
                messageQueue.Add(msg, TypeMessage.Error);
                return true;
            }

            return false;
        }

        private bool PossuiTituloPago(TituloPagar tituloPagar, out int? tituloPagarId)
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

        private bool PossuiTituloRecebido(TituloReceber tituloReceber, out int? tituloReceberId)
        {
            tituloReceberId = null;
            if (tituloReceber != null)
            {
                if (tituloReceber.TipoTitulo == TipoTitulo.Pai)
                {
                    if (PossuiTituloDesdobradoRecebido(tituloReceber.ListaFilhos, out tituloReceberId))
                        return true;
                }
                else if (EhTituloRecebido(tituloReceber, out tituloReceberId))
                    return true;
            }

            return false;
        }

        private bool PossuiTituloImpostoPago(TituloPagar tituloPagar, out int? tituloPagarId)
        {
            tituloPagarId = null;
            if (tituloPagar != null)
            {
                foreach (var item in tituloPagar.ListaImpostoPagar)
                {
                    if (item.TituloPagarImpostoId.HasValue && item.TituloPagarImposto.TipoTitulo == TipoTitulo.Pai)
                    {
                        if (PossuiTituloDesdobradoPago(item.TituloPagarImposto.ListaFilhos, out tituloPagarId))
                            return true;
                    }
                    else if (EhTituloPago(item.TituloPagarImposto, out tituloPagarId))
                        return true;
                }
            }

            return false;
        }

        private bool PossuiTituloDesdobradoPago(ICollection<TituloPagar> listaFilhos, out int? tituloPagarId)
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

        private bool PossuiTituloDesdobradoRecebido(ICollection<TituloReceber> listaFilhos, out int? tituloReceberId)
        {
            tituloReceberId = null;
            foreach (var titulo in listaFilhos)
            {
                if (titulo.TipoTitulo == TipoTitulo.Pai)
                {
                    if (PossuiTituloDesdobradoRecebido(titulo.ListaFilhos, out tituloReceberId))
                        return true;
                }
                else if (EhTituloRecebido(titulo, out tituloReceberId))
                    return true;
            }

            return false;
        }

        private static bool EhTituloPago(TituloPagar titulo, out int? tituloPagarId)
        {
            tituloPagarId = null;
            if ((titulo != null) &&
                ((titulo.Situacao == SituacaoTituloPagar.Emitido) ||
                 (titulo.Situacao == SituacaoTituloPagar.Pago) ||
                 (titulo.Situacao == SituacaoTituloPagar.Baixado)))
            {
                tituloPagarId = titulo.Id;
                return true;
            }

            return false;
        }

        private static bool EhTituloRecebido(TituloReceber titulo, out int? tituloReceberId)
        {
            tituloReceberId = null;
            if ((titulo != null) &&
                ((titulo.Situacao == SituacaoTituloReceber.Predatado) ||
                 (titulo.Situacao == SituacaoTituloReceber.Recebido) ||
                 (titulo.Situacao == SituacaoTituloReceber.Quitado)))
            {
                tituloReceberId = titulo.Id;
                return true;
            }

            return false;
        }

        private bool VerificaExistenciaRetencaoLiberada(Domain.Entity.Contrato.Contrato contrato, ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao)
        {
            bool existeRetencaoComLiberacao = contrato.ListaContratoRetencao.Where(l => l.ContratoRetificacaoItemMedicaoId == contratoRetificacaoItemMedicao.Id).Any(l => l.ListaContratoRetencaoLiberada.Any(rl => rl.ContratoRetencaoId == l.Id));
            if (!existeRetencaoComLiberacao)
            {
                ContratoRetificacaoItemMedicao medicaoLiberadasComMesmoTitulo = contrato.ListaContratoRetificacaoItemMedicao.Where(l => l.TituloPagarId == contratoRetificacaoItemMedicao.TituloPagarId && l.Id != contratoRetificacaoItemMedicao.Id && l.Situacao == SituacaoMedicao.Liberado).FirstOrDefault();
                if (medicaoLiberadasComMesmoTitulo != null)
                {
                    existeRetencaoComLiberacao = contrato.ListaContratoRetencao.Where(l => l.ContratoRetificacaoItemMedicaoId == medicaoLiberadasComMesmoTitulo.Id).Any(l => l.ListaContratoRetencaoLiberada.Any(rl => rl.ContratoRetencaoId == l.Id));
                }
            }

            return existeRetencaoComLiberacao;
        }

        private void GerarEntradaDeEstoque(List<ContratoRetificacaoItemMedicao> listaMedicaoSelecionadas, Domain.Entity.Contrato.Contrato contrato)
        {
            Estoque estoque = estoqueRepository.ObterEstoqueAtivoPeloCentroCusto(contrato.CodigoCentroCusto, l => l.ListaEstoqueMaterial);

            if (estoque != null)
            {
                List<ContratoRetificacaoItemMedicao> medicaoGrupoEstoque = listaMedicaoSelecionadas.GroupBy(l => new { l.TipoDocumentoId, l.NumeroDocumento, l.DataEmissao }).Select(o => o.First()).ToList();

                foreach (ContratoRetificacaoItemMedicao medicaoItemEstoque in medicaoGrupoEstoque)
                {
                    ClienteFornecedor cliente = ObtemClienteFornecedor(contrato, medicaoItemEstoque);

                    List<ContratoRetificacaoItemMedicaoNF> medicaoNFGrupoEstoque = contrato.ListaContratoRetificacaoItemMedicaoNF.
                                                                Where(l => l.NumeroDocumento == medicaoItemEstoque.NumeroDocumento &&
                                                                           l.TipoDocumentoId == medicaoItemEstoque.TipoDocumentoId &&
                                                                           l.DataEmissao == medicaoItemEstoque.DataEmissao &&
                                                                           l.Material.EhControladoPorEstoque.Value == true).
                                                                GroupBy(l => new
                                                                {
                                                                    l.TipoDocumentoId,
                                                                    l.NumeroDocumento,
                                                                    l.DataEmissao
                                                                }).SelectMany(o => o).ToList();

                    if (medicaoNFGrupoEstoque.Count > 0)
                    {
                        #region "Trata a entrada no estoque"

                        DateTime dataEntrega = medicaoNFGrupoEstoque.Where(l =>
                                                                           l.NumeroDocumento == medicaoItemEstoque.NumeroDocumento &&
                                                                           l.TipoDocumentoId == medicaoItemEstoque.TipoDocumentoId &&
                                                                           l.DataEmissao == medicaoItemEstoque.DataEmissao).
                                                                           Select(l => l.DataEntrega).First();

                        Movimento movimentoEntrada = new Movimento();

                        PreencherMovimentoEstoque(movimentoEntrada,
                                                  cliente.Id.Value,
                                                  contrato.Id.Value,
                                                  medicaoItemEstoque.TipoDocumentoId,
                                                  medicaoItemEstoque.NumeroDocumento,
                                                  medicaoItemEstoque.DataEmissao,
                                                  dataEntrega,
                                                  estoque.Id.Value,
                                                  TipoMovimentoEstoque.EntradaContrato);
                        #endregion

                        #region "Trata a saida no estoque"

                        Movimento movimentoSaida = new Movimento();

                        PreencherMovimentoEstoque(movimentoSaida,
                                                  cliente.Id.Value,
                                                  contrato.Id.Value,
                                                  medicaoItemEstoque.TipoDocumentoId,
                                                  medicaoItemEstoque.NumeroDocumento,
                                                  medicaoItemEstoque.DataEmissao,
                                                  dataEntrega,
                                                  estoque.Id.Value,
                                                  TipoMovimentoEstoque.SaidaContrato);
                        #endregion

                        foreach (ContratoRetificacaoItemMedicaoNF medicaoNFItemEstoque in medicaoNFGrupoEstoque)
                        {
                            bool ehMovimentoTemporario = false;
                            EstoqueMaterial estoqueMaterial = estoque.ListaEstoqueMaterial.Where(l => l.MaterialId == medicaoNFItemEstoque.MaterialId).SingleOrDefault();

                            #region "Trata item movimento entrada no estoque"

                            MovimentoItem movimentoEntradaItem = new MovimentoItem();

                            PreencherMovimentoItemEstoque(movimentoEntradaItem, movimentoEntrada, medicaoNFItemEstoque);

                            movimentoEntrada.ListaMovimentoItem.Add(movimentoEntradaItem);

                            #endregion

                            #region "Trata atualização de saldo no estoque - Entrada"

                            if (estoqueMaterial == null)
                            {
                                estoqueMaterial = new EstoqueMaterial();
                                estoqueMaterial.MaterialId = medicaoNFItemEstoque.MaterialId;
                                estoqueMaterial.EstoqueId = estoque.Id;
                                estoqueMaterial.Quantidade = 0;
                                estoqueMaterial.QuantidadeTemporaria = 0;
                                estoqueMaterial.Valor = 0;
                                estoque.ListaEstoqueMaterial.Add(estoqueMaterial);
                            }

                            ehMovimentoTemporario = movimentoEntrada.EhMovimentoTemporario.HasValue ? movimentoEntrada.EhMovimentoTemporario.Value : false;

                            if (!ehMovimentoTemporario)
                            {
                                estoqueMaterial.Valor = movimentoEntradaItem.Valor;
                                estoqueMaterial.Quantidade += movimentoEntradaItem.Quantidade;
                            }
                            else
                            {
                                //NA INSTRUÇÃO ABAIXO FOI COLOCADO A MULTIPLICAÇÃO DA QUANTIDADE POR (-1), 
                                //POIS O MOVIMENTO É GERADO NEGATIVO, PORÉM O CÁLCULO DO SALDO É FEITO DE FORMA CONTRÁRIA,
                                //UMA VEZ QUE A 'RETIRADA' ENTRA NO ESTOQUE TEMPORÁRIO COMO POSITIVO
                                estoqueMaterial.QuantidadeTemporaria += (movimentoEntradaItem.Quantidade * -1);
                            }

                            #endregion

                            #region "Trata item movimento saida no estoque"

                            MovimentoItem movimentoSaidaItem = new MovimentoItem();

                            PreencherMovimentoItemEstoque(movimentoSaidaItem, movimentoSaida, medicaoNFItemEstoque);

                            movimentoSaida.ListaMovimentoItem.Add(movimentoSaidaItem);

                            #endregion

                            #region "Trata atualização de saldo no estoque - Saida"

                            if (estoqueMaterial == null)
                            {
                                estoqueMaterial = new EstoqueMaterial();
                                estoqueMaterial.MaterialId = medicaoNFItemEstoque.MaterialId;
                                estoqueMaterial.EstoqueId = estoque.Id;
                                estoqueMaterial.Quantidade = 0;
                                estoqueMaterial.QuantidadeTemporaria = 0;
                                estoqueMaterial.Valor = 0;
                                estoque.ListaEstoqueMaterial.Add(estoqueMaterial);
                            }

                            ehMovimentoTemporario = movimentoSaida.EhMovimentoTemporario.HasValue ? movimentoSaida.EhMovimentoTemporario.Value : false;

                            if (!ehMovimentoTemporario)
                            {
                                estoqueMaterial.Quantidade += movimentoSaidaItem.Quantidade;
                            }
                            else
                            {
                                //NA INSTRUÇÃO ABAIXO FOI COLOCADO A MULTIPLICAÇÃO DA QUANTIDADE POR (-1), 
                                //POIS O MOVIMENTO É GERADO NEGATIVO, PORÉM O CÁLCULO DO SALDO É FEITO DE FORMA CONTRÁRIA,
                                //UMA VEZ QUE A 'RETIRADA' ENTRA NO ESTOQUE TEMPORÁRIO COMO POSITIVO
                                estoqueMaterial.QuantidadeTemporaria += (movimentoSaidaItem.Quantidade * -1);
                            }

                            #endregion

                        }

                        contrato.ListaMovimento.Add(movimentoEntrada);
                        contrato.ListaMovimento.Add(movimentoSaida);

                        estoqueRepository.Alterar(estoque);

                    }
                }
            }
        }

        private void GerarTitulosApropriadosLiberacao(List<ContratoRetificacaoItemMedicao> listaMedicaoSelecionadas, Domain.Entity.Contrato.Contrato contrato, bool geraTituloAguardandoLiberacao, bool geraTituloImposto)
        {
            string identificacaoTitulo = MontaIdentificacaoTitulo(contrato);

            List<ContratoRetificacaoItemMedicao> medicaoGrupoTitulos = new List<ContratoRetificacaoItemMedicao>();

            #region "Agrupa as medições a serem liberadas por documento distinguindo os que possuem adiantamento ou não"

            bool temPagamentoAntecipado = !listaMedicaoSelecionadas.All(l => (l.ContratoRetificacaoItemCronograma.EhPagamentoAntecipado.HasValue && l.ContratoRetificacaoItemCronograma.EhPagamentoAntecipado.Value == false) || (!l.ContratoRetificacaoItemCronograma.EhPagamentoAntecipado.HasValue));

            if (!temPagamentoAntecipado)
            {
                medicaoGrupoTitulos = listaMedicaoSelecionadas.GroupBy(l => new { l.MultiFornecedorId, l.TipoDocumentoId, l.NumeroDocumento, l.DataVencimento, l.DataEmissao}).Select(o => o.First()).ToList();
            }
            else 
            {
                medicaoGrupoTitulos = listaMedicaoSelecionadas.Where(l => (l.ContratoRetificacaoItemCronograma.EhPagamentoAntecipado.HasValue && l.ContratoRetificacaoItemCronograma.EhPagamentoAntecipado.Value == false) || (!l.ContratoRetificacaoItemCronograma.EhPagamentoAntecipado.HasValue)).GroupBy(l => new { l.MultiFornecedorId, l.TipoDocumentoId, l.NumeroDocumento, l.DataVencimento, l.DataEmissao }).Select(o => o.First()).ToList();
            }

            #endregion

            //Na lista de medições selecionada podem existir medições com pagamento antecipado, 
            //mas não possuir nenhuma medição para ser liberada sem pagamento antecipado,
            //então essa condição (temPagamentoAntecipado == true and medicaoGrupoTitulos.Count == 0)
            //não processará o foreach abaixo 

            #region "Processamento das medições selecionadas que não possuem pagamento antecipado"

            foreach (var medicaoItemTitulo in medicaoGrupoTitulos)
            {
                #region "Agrupa as medições a serem liberadas por documento"

                List<ContratoRetificacaoItemMedicao> listaItensMedicaoGrupoTitulos;
                if (!temPagamentoAntecipado)
                {
                    listaItensMedicaoGrupoTitulos = listaMedicaoSelecionadas.Where(l =>
                                                                                   l.MultiFornecedorId == medicaoItemTitulo.MultiFornecedorId &&
                                                                                   l.TipoDocumentoId == medicaoItemTitulo.TipoDocumentoId &&
                                                                                   l.NumeroDocumento == medicaoItemTitulo.NumeroDocumento &&
                                                                                   l.DataVencimento == medicaoItemTitulo.DataVencimento &&
                                                                                   l.DataEmissao == medicaoItemTitulo.DataEmissao).ToList();
                }
                else
                {
                    listaItensMedicaoGrupoTitulos = listaMedicaoSelecionadas.Where(l =>
                                                                                   l.MultiFornecedorId == medicaoItemTitulo.MultiFornecedorId &&
                                                                                   l.TipoDocumentoId == medicaoItemTitulo.TipoDocumentoId &&
                                                                                   l.NumeroDocumento == medicaoItemTitulo.NumeroDocumento &&
                                                                                   l.DataVencimento == medicaoItemTitulo.DataVencimento &&
                                                                                   l.DataEmissao == medicaoItemTitulo.DataEmissao &&
                                                                                   ((l.ContratoRetificacaoItemCronograma.EhPagamentoAntecipado.HasValue && 
                                                                                     l.ContratoRetificacaoItemCronograma.EhPagamentoAntecipado.Value == false) || 
                                                                                    (!l.ContratoRetificacaoItemCronograma.EhPagamentoAntecipado.HasValue))
                                                                                   ).ToList();
                }


                decimal valorTitulo = listaItensMedicaoGrupoTitulos.Sum(o => o.Valor);
                decimal? valorDesconto = listaItensMedicaoGrupoTitulos.Where(o => o.Desconto.HasValue && o.Desconto.Value > 0).Sum(o => o.Desconto);
                decimal? valorRetido = listaItensMedicaoGrupoTitulos.Where(o => o.ValorRetido.HasValue && o.ValorRetido.Value > 0).Sum(o => o.ValorRetido);
                string motivoDesconto = listaItensMedicaoGrupoTitulos.Where(o => o.MotivoDesconto != "").LastOrDefault() != null ? listaItensMedicaoGrupoTitulos.Where(o => o.MotivoDesconto != "").LastOrDefault().MotivoDesconto : null;

                #endregion

                ClienteFornecedor cliente = ObtemClienteFornecedor(contrato, medicaoItemTitulo);

                if (contrato.TipoContrato == TipoContrato.ContratoAPagar)
                {
                    TituloPagar tituloPagarLiberacao = new TituloPagar();

                    #region "Cria Titulo Pagar"

                    PreencherTituloPagar(tituloPagarLiberacao,
                                         cliente.Id,
                                         identificacaoTitulo,
                                         geraTituloAguardandoLiberacao ? SituacaoTituloPagar.AguardandoLiberacao : SituacaoTituloPagar.Liberado,
                                         medicaoItemTitulo.TipoDocumentoId,
                                         medicaoItemTitulo.NumeroDocumento,
                                         medicaoItemTitulo.DataVencimento,
                                         medicaoItemTitulo.DataEmissao,
                                         valorTitulo,
                                         valorDesconto,
                                         motivoDesconto,
                                         valorRetido,
                                         null);
                    foreach (ContratoRetificacaoItemMedicao medicao in listaItensMedicaoGrupoTitulos)
                    {
                        ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao = contrato.ListaContratoRetificacaoItemMedicao.Where(l => l.Id == medicao.Id).FirstOrDefault();
                        contratoRetificacaoItemMedicao.TituloPagar = tituloPagarLiberacao;
                        contratoRetificacaoItemMedicao.Situacao = SituacaoMedicao.Liberado;
                        contratoRetificacaoItemMedicao.DataLiberacao = DateTime.Now;
                        contratoRetificacaoItemMedicao.UsuarioLiberacao = UsuarioLogado.Login;
                    }
                    #endregion

                    List<ContratoRetificacaoItemImposto> impostosGrupo = listaItensMedicaoGrupoTitulos.SelectMany(l => l.ContratoRetificacaoItem.ListaContratoRetificacaoItemImposto).OrderBy(ii => ii.ImpostoFinanceiroId).GroupBy(l => l.ImpostoFinanceiroId).Select(o => o.First()).ToList();

                    #region "Cria Imposto Pagar e Titulo Pagar de Imposto"

                    decimal totalImposto = 0;
                    foreach (var impostoFinanceiroItem in impostosGrupo)
                    {
                        List<ContratoRetificacaoItemMedicao> listaMedicaoAgrupadaPorImposto = listaItensMedicaoGrupoTitulos.Where(l => l.ContratoRetificacaoItem.ListaContratoRetificacaoItemImposto.Any(i => i.ImpostoFinanceiroId == impostoFinanceiroItem.ImpostoFinanceiroId)).ToList();

                        decimal valor = listaMedicaoAgrupadaPorImposto.Sum(l => l.Valor);

                        ContratoRetificacaoItemImposto contratoRetificacaoItemImposto = listaMedicaoAgrupadaPorImposto.Select(l => l.ContratoRetificacaoItem).FirstOrDefault().ListaContratoRetificacaoItemImposto.Where(ii => ii.ImpostoFinanceiroId == impostoFinanceiroItem.ImpostoFinanceiroId).FirstOrDefault();

                        ImpostoFinanceiro impostoFinanceiro = contratoRetificacaoItemImposto.ImpostoFinanceiro;

                        decimal baseCalculo = (valor * contratoRetificacaoItemImposto.PercentualBaseCalculo) / 100;

                        if (baseCalculo > 0 && impostoFinanceiro != null && impostoFinanceiro.Aliquota > 0)
                        {
                            #region "Cria Imposto Pagar"

                            decimal valorImposto = Math.Round(((baseCalculo * impostoFinanceiro.Aliquota) / 100), 2);

                            bool ehRetido = impostoFinanceiro.EhRetido.HasValue ? impostoFinanceiro.EhRetido.Value : false;
                            bool ehIndireto = impostoFinanceiro.Indireto.HasValue ? impostoFinanceiro.Indireto.Value : false;
                            if (ehRetido || ehIndireto)
                            {
                                if (valorImposto > 0)
                                {
                                    totalImposto = totalImposto + valorImposto;
                                    tituloPagarLiberacao.ValorImposto = totalImposto;
                                }
                            }

                            ImpostoPagar impostoPagarTituloLiberacao = new ImpostoPagar();
                            PreencherImpostoPagar(impostoPagarTituloLiberacao,
                                                  impostoFinanceiro.Id.Value,
                                                  tituloPagarLiberacao,
                                                  baseCalculo,
                                                  valorImposto,
                                                  null);

                            tituloPagarLiberacao.ListaImpostoPagar.Add(impostoPagarTituloLiberacao);

                            #endregion

                            #region "Cria Titulo Pagar de Imposto"

                            if (geraTituloImposto)
                            {
                                string ordem = (impostosGrupo.IndexOf(impostoFinanceiroItem) + 1).ToString();
                                string identificacaoImposto = MontaIdentificacaoTituloImposto(impostoFinanceiro.Sigla, cliente.Nome);
                                string documentoImposto = MontaDocumentoTituloImposto(medicaoItemTitulo.NumeroDocumento, ordem);

                                Nullable<DateTime> dataVencimentoImposto = null;
                                if ((ehRetido || ehIndireto) && impostoFinanceiro.ClienteId.HasValue && impostoFinanceiro.Periodicidade.HasValue)
                                {
                                    dataVencimentoImposto = CalcularDataVencimentoImposto(impostoFinanceiro, tituloPagarLiberacao.DataEmissaoDocumento, tituloPagarLiberacao.DataVencimento);
                                }

                                if (dataVencimentoImposto.HasValue && impostoFinanceiro.ClienteId.HasValue)
                                {
                                    TituloPagar tituloImpostoPagar = new TituloPagar();
                                    PreencherTituloPagar(tituloImpostoPagar,
                                                         impostoFinanceiro.Cliente.Id,
                                                         identificacaoImposto,
                                                         geraTituloAguardandoLiberacao ? SituacaoTituloPagar.AguardandoLiberacao : SituacaoTituloPagar.Liberado,
                                                         medicaoItemTitulo.TipoDocumentoId,
                                                         documentoImposto,
                                                         dataVencimentoImposto.Value,
                                                         medicaoItemTitulo.DataEmissao,
                                                         valorImposto,
                                                         0,
                                                         null,
                                                         null,
                                                         impostoFinanceiro.TipoCompromissoId);

                                    impostoPagarTituloLiberacao.TituloPagarImposto = tituloImpostoPagar;
                                }
                            }
                            #endregion

                        }
                    }
                    #endregion

                    var medicaoGrupoApropriacao = listaItensMedicaoGrupoTitulos.GroupBy(l => new { l.MultiFornecedorId,
                                                                                                   l.TipoDocumentoId,
                                                                                                   l.NumeroDocumento,
                                                                                                   l.DataVencimento,
                                                                                                   l.DataEmissao,
                                                                                                   l.ContratoRetificacaoItem.CodigoClasse,
                                                                                                   l.Contrato.CodigoCentroCusto
                                                                                                 }).Select(o => o.First()).ToList();

                    foreach (var medicaoItemApropriacao in medicaoGrupoApropriacao.OrderBy(l => l.Contrato.CodigoCentroCusto).ThenBy(l => l.ContratoRetificacaoItem.CodigoClasse))
                    {
                        #region "Cria Apropriação dos titulos a pagar "

                        List<ContratoRetificacaoItemMedicao> listaItensMedicaoGrupoApropriacao;
                        if (!temPagamentoAntecipado)
                        {
                            listaItensMedicaoGrupoApropriacao = listaMedicaoSelecionadas.Where(l =>
                                                                                               l.MultiFornecedorId == medicaoItemApropriacao.MultiFornecedorId &&
                                                                                               l.TipoDocumentoId == medicaoItemApropriacao.TipoDocumentoId &&
                                                                                               l.NumeroDocumento == medicaoItemApropriacao.NumeroDocumento &&
                                                                                               l.DataVencimento == medicaoItemApropriacao.DataVencimento &&
                                                                                               l.DataEmissao == medicaoItemApropriacao.DataEmissao &&
                                                                                               l.ContratoRetificacaoItem.CodigoClasse == medicaoItemApropriacao.ContratoRetificacaoItem.CodigoClasse &&
                                                                                               l.Contrato.CodigoCentroCusto == medicaoItemApropriacao.Contrato.CodigoCentroCusto
                                                                                              ).ToList();
                        }
                        else
                        {
                            listaItensMedicaoGrupoApropriacao = listaMedicaoSelecionadas.Where(l =>
                                                                                               l.MultiFornecedorId == medicaoItemApropriacao.MultiFornecedorId &&
                                                                                               l.TipoDocumentoId == medicaoItemApropriacao.TipoDocumentoId &&
                                                                                               l.NumeroDocumento == medicaoItemApropriacao.NumeroDocumento &&
                                                                                               l.DataVencimento == medicaoItemApropriacao.DataVencimento &&
                                                                                               l.DataEmissao == medicaoItemApropriacao.DataEmissao &&
                                                                                               l.ContratoRetificacaoItem.CodigoClasse == medicaoItemApropriacao.ContratoRetificacaoItem.CodigoClasse &&
                                                                                               l.Contrato.CodigoCentroCusto == medicaoItemApropriacao.Contrato.CodigoCentroCusto &&
                                                                                               ((l.ContratoRetificacaoItemCronograma.EhPagamentoAntecipado.HasValue &&
                                                                                                 l.ContratoRetificacaoItemCronograma.EhPagamentoAntecipado.Value == false) ||
                                                                                                (!l.ContratoRetificacaoItemCronograma.EhPagamentoAntecipado.HasValue))
                                                                                              ).ToList();
                        }

                        decimal valorAgrupado = listaItensMedicaoGrupoApropriacao.Sum(o => o.Valor);
                        decimal? valorDescontoAgrupado = listaItensMedicaoGrupoApropriacao.Where(o => o.Desconto.HasValue && o.Desconto.Value > 0).Sum(o => o.Desconto);
                        decimal? valorRetidoAgrupado = listaItensMedicaoGrupoApropriacao.Where(o => o.ValorRetido.HasValue && o.ValorRetido.Value > 0).Sum(o => o.ValorRetido);

                        decimal valorApropriacao = valorAgrupado;
                        if (valorRetido.HasValue) valorApropriacao = valorApropriacao - valorRetidoAgrupado.Value;
                        if (valorDesconto.HasValue) valorApropriacao = valorApropriacao - valorDescontoAgrupado.Value;

                        impostosGrupo = listaItensMedicaoGrupoApropriacao.SelectMany(l => l.ContratoRetificacaoItem.ListaContratoRetificacaoItemImposto).OrderBy(ii => ii.ImpostoFinanceiroId).GroupBy(l => l.ImpostoFinanceiroId).Select(o => o.First()).ToList();

                        decimal valorImposto = 0;
                        foreach (var impostoFinanceiroItem in impostosGrupo)
                        {
                            List<ContratoRetificacaoItemMedicao> listaMedicaoAgrupadaPorImposto = listaItensMedicaoGrupoApropriacao.Where(l => l.ContratoRetificacaoItem.ListaContratoRetificacaoItemImposto.Any(i => i.ImpostoFinanceiroId == impostoFinanceiroItem.ImpostoFinanceiroId)).ToList();

                            ContratoRetificacaoItemImposto contratoRetificacaoItemImposto = listaMedicaoAgrupadaPorImposto.Select(l => l.ContratoRetificacaoItem).FirstOrDefault().ListaContratoRetificacaoItemImposto.Where(ii => ii.ImpostoFinanceiroId == impostoFinanceiroItem.ImpostoFinanceiroId).FirstOrDefault();

                            ImpostoFinanceiro impostoFinanceiro = contratoRetificacaoItemImposto.ImpostoFinanceiro;

                            decimal baseCalculo = (valorAgrupado * contratoRetificacaoItemImposto.PercentualBaseCalculo) / 100;

                            if (baseCalculo > 0)
                            {
                                bool ehRetido = impostoFinanceiro.EhRetido.HasValue ? impostoFinanceiro.EhRetido.Value : false;
                                bool ehIndireto = impostoFinanceiro.Indireto.HasValue ? impostoFinanceiro.Indireto.Value : false;
                                if (ehRetido || ehIndireto)
                                {
                                    valorImposto = valorImposto + Math.Round(((baseCalculo * impostoFinanceiro.Aliquota) / 100), 2);
                                }
                            }
                        }

                        decimal valorAgrupadoTitulos = listaItensMedicaoGrupoTitulos.Sum(l => l.Valor);
                        decimal percentualApropriado = Math.Round((((valorApropriacao - valorImposto) / valorAgrupadoTitulos) * 100), 5);
                        decimal valorApropriado = valorApropriacao - valorImposto;

                        Apropriacao apropriacaoTituloPagarLiberacao = new Apropriacao();
                        PreencherApropriacaoTitulo(apropriacaoTituloPagarLiberacao,
                                                   medicaoItemApropriacao.ContratoRetificacaoItem.CodigoClasse,
                                                   medicaoItemApropriacao.Contrato.CodigoCentroCusto,
                                                   tituloPagarLiberacao,
                                                   null,
                                                   percentualApropriado,
                                                   valorApropriado);


                        tituloPagarLiberacao.ListaApropriacao.Add(apropriacaoTituloPagarLiberacao);

                        #endregion
                    }

                    #region "Cria Apropriação dos titulos de impostos a pagar "

                    if (geraTituloImposto && (tituloPagarLiberacao.ListaApropriacao.Sum(l => l.Percentual) < 100) && tituloPagarLiberacao.ListaApropriacao.Count > 0)
                    {
                        decimal valorLiquido = tituloPagarLiberacao.ValorTitulo;
                        if (tituloPagarLiberacao.ValorImposto.HasValue) valorLiquido = valorLiquido - tituloPagarLiberacao.ValorImposto.Value;
                        if (tituloPagarLiberacao.Retencao.HasValue) valorLiquido = valorLiquido - tituloPagarLiberacao.Retencao.Value;

                        foreach (var apropriacaoTitulo in tituloPagarLiberacao.ListaApropriacao.OrderBy(l => l.CodigoCentroCusto).ThenBy(l => l.CodigoClasse))
                        {
                            decimal percentual = Math.Round(((apropriacaoTitulo.Valor / valorLiquido) * 100), 5);

                            foreach (ImpostoPagar impostoPagar in tituloPagarLiberacao.ListaImpostoPagar.OrderBy(l => l.ImpostoFinanceiroId))
                            {

                                if (impostoPagar.TituloPagarImposto != null)
                                {
                                    decimal valorApropriado = Math.Round(((impostoPagar.TituloPagarImposto.ValorTitulo * percentual) / 100), 2);

                                    Apropriacao apropriacaoTituloImpostoPagar = new Apropriacao();
                                    PreencherApropriacaoTitulo(apropriacaoTituloImpostoPagar,
                                                               apropriacaoTitulo.CodigoClasse,
                                                               apropriacaoTitulo.CodigoCentroCusto,
                                                               impostoPagar.TituloPagarImposto,
                                                               null,
                                                               percentual,
                                                               valorApropriado);

                                    impostoPagar.TituloPagarImposto.ListaApropriacao.Add(apropriacaoTituloImpostoPagar);
                                }
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    TituloReceber tituloReceberLiberacao = new TituloReceber();

                    #region "Cria Titulo Receber"

                    PreencherTituloReceber(tituloReceberLiberacao,
                                           cliente.Id,
                                           identificacaoTitulo,
                                           geraTituloAguardandoLiberacao ? SituacaoTituloReceber.Afaturar : SituacaoTituloReceber.Faturado,
                                           medicaoItemTitulo.TipoDocumentoId,
                                           medicaoItemTitulo.NumeroDocumento,
                                           medicaoItemTitulo.DataVencimento,
                                           medicaoItemTitulo.DataEmissao,
                                           valorTitulo,
                                           valorDesconto,
                                           null,
                                           valorRetido,
                                           null);

                    foreach (ContratoRetificacaoItemMedicao medicao in listaItensMedicaoGrupoTitulos)
                    {
                        ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao = contrato.ListaContratoRetificacaoItemMedicao.Where(l => l.Id == medicao.Id).FirstOrDefault();
                        contratoRetificacaoItemMedicao.TituloReceber = tituloReceberLiberacao;
                        contratoRetificacaoItemMedicao.Situacao = SituacaoMedicao.Liberado;
                        contratoRetificacaoItemMedicao.DataLiberacao = DateTime.Now;
                        contratoRetificacaoItemMedicao.UsuarioLiberacao = UsuarioLogado.Login;
                    }

                    #endregion

                    List<ContratoRetificacaoItemImposto> impostosGrupo = listaItensMedicaoGrupoTitulos.SelectMany(l => l.ContratoRetificacaoItem.ListaContratoRetificacaoItemImposto).OrderBy(ii => ii.ImpostoFinanceiroId).GroupBy(l => l.ImpostoFinanceiroId).Select(o => o.First()).ToList();

                    #region "Cria Imposto Receber"

                    decimal totalImposto = 0;
                    foreach (var impostoFinanceiroItem in impostosGrupo)
                    {
                        List<ContratoRetificacaoItemMedicao> listaMedicaoAgrupadaPorImposto = listaItensMedicaoGrupoTitulos.Where(l => l.ContratoRetificacaoItem.ListaContratoRetificacaoItemImposto.Any(i => i.ImpostoFinanceiroId == impostoFinanceiroItem.ImpostoFinanceiroId)).ToList();

                        decimal valor = listaMedicaoAgrupadaPorImposto.Sum(l => l.Valor);

                        ContratoRetificacaoItemImposto contratoRetificacaoItemImposto = listaMedicaoAgrupadaPorImposto.Select(l => l.ContratoRetificacaoItem).FirstOrDefault().ListaContratoRetificacaoItemImposto.Where(ii => ii.ImpostoFinanceiroId == impostoFinanceiroItem.ImpostoFinanceiroId).FirstOrDefault();

                        ImpostoFinanceiro impostoFinanceiro = contratoRetificacaoItemImposto.ImpostoFinanceiro;

                        decimal baseCalculo = (valor * contratoRetificacaoItemImposto.PercentualBaseCalculo) / 100;

                        if (baseCalculo > 0 && impostoFinanceiro != null && impostoFinanceiro.Aliquota > 0)
                        {
                            #region "Cria Imposto Receber"

                            decimal valorImposto = Math.Round(((baseCalculo * impostoFinanceiro.Aliquota) / 100), 2);

                            bool ehRetido = impostoFinanceiro.EhRetido.HasValue ? impostoFinanceiro.EhRetido.Value : false;
                            if (ehRetido)
                            {
                                if (valorImposto > 0)
                                {
                                    totalImposto = totalImposto + valorImposto;
                                    tituloReceberLiberacao.ValorImposto = valorImposto;
                                }
                            }

                            ImpostoReceber impostoReceberTituloLiberacao = new ImpostoReceber();
                            PreencherImpostoReceber(impostoReceberTituloLiberacao,
                                                    impostoFinanceiro.Id.Value,
                                                    tituloReceberLiberacao,
                                                    baseCalculo,
                                                    valorImposto);

                            tituloReceberLiberacao.ListaImpostoReceber.Add(impostoReceberTituloLiberacao);

                            #endregion
                        }
                    }

                    #endregion

                    var medicaoGrupoApropriacao = listaItensMedicaoGrupoTitulos.GroupBy(l => new { l.MultiFornecedorId,
                                                                                                   l.TipoDocumentoId,
                                                                                                   l.NumeroDocumento,
                                                                                                   l.DataVencimento,
                                                                                                   l.DataEmissao,
                                                                                                   l.ContratoRetificacaoItem.CodigoClasse,
                                                                                                   l.Contrato.CodigoCentroCusto
                                                                                                  }).Select(o => o.First()).ToList();

                    foreach (var medicaoItemApropriacao in medicaoGrupoApropriacao.OrderBy(l => l.Contrato.CodigoCentroCusto).ThenBy(l => l.ContratoRetificacaoItem.CodigoClasse))
                    {
                        #region "Cria Apropriação dos titulos a Receber"

                        var listaItensMedicaoGrupoApropriacao = listaMedicaoSelecionadas.Where(l =>
                                                                                               l.MultiFornecedorId == medicaoItemApropriacao.MultiFornecedorId &&
                                                                                               l.TipoDocumentoId == medicaoItemApropriacao.TipoDocumentoId &&
                                                                                               l.NumeroDocumento == medicaoItemApropriacao.NumeroDocumento &&
                                                                                               l.DataVencimento == medicaoItemApropriacao.DataVencimento &&
                                                                                               l.DataEmissao == medicaoItemApropriacao.DataEmissao &&
                                                                                               l.ContratoRetificacaoItem.CodigoClasse == medicaoItemApropriacao.ContratoRetificacaoItem.CodigoClasse &&
                                                                                               l.Contrato.CodigoCentroCusto == medicaoItemApropriacao.Contrato.CodigoCentroCusto
                                                                                               ).ToList();

                        decimal valorAgrupado = listaItensMedicaoGrupoApropriacao.Sum(o => o.Valor);
                        decimal? valorDescontoAgrupado = listaItensMedicaoGrupoApropriacao.Where(o => o.Desconto.HasValue && o.Desconto.Value > 0).Sum(o => o.Desconto);
                        decimal? valorRetidoAgrupado = listaItensMedicaoGrupoApropriacao.Where(o => o.ValorRetido.HasValue && o.ValorRetido.Value > 0).Sum(o => o.ValorRetido);

                        decimal valorApropriacao = valorAgrupado;
                        if (valorRetido.HasValue) valorApropriacao = valorApropriacao - valorRetidoAgrupado.Value;
                        if (valorDesconto.HasValue) valorApropriacao = valorApropriacao - valorDescontoAgrupado.Value;

                        impostosGrupo = listaItensMedicaoGrupoApropriacao.SelectMany(l => l.ContratoRetificacaoItem.ListaContratoRetificacaoItemImposto).OrderBy(ii => ii.ImpostoFinanceiroId).GroupBy(l => l.ImpostoFinanceiroId).Select(o => o.First()).ToList();

                        decimal valorImposto = 0;
                        foreach (var impostoFinanceiroItem in impostosGrupo)
                        {
                            List<ContratoRetificacaoItemMedicao> listaMedicaoAgrupadaPorImposto = listaItensMedicaoGrupoApropriacao.Where(l => l.ContratoRetificacaoItem.ListaContratoRetificacaoItemImposto.Any(i => i.ImpostoFinanceiroId == impostoFinanceiroItem.ImpostoFinanceiroId)).ToList();

                            ContratoRetificacaoItemImposto contratoRetificacaoItemImposto = listaMedicaoAgrupadaPorImposto.Select(l => l.ContratoRetificacaoItem).FirstOrDefault().ListaContratoRetificacaoItemImposto.Where(ii => ii.ImpostoFinanceiroId == impostoFinanceiroItem.ImpostoFinanceiroId).FirstOrDefault();

                            ImpostoFinanceiro impostoFinanceiro = contratoRetificacaoItemImposto.ImpostoFinanceiro;

                            decimal baseCalculo = (valorAgrupado * contratoRetificacaoItemImposto.PercentualBaseCalculo) / 100;

                            if (baseCalculo > 0)
                            {
                                bool ehRetido = impostoFinanceiro.EhRetido.HasValue ? impostoFinanceiro.EhRetido.Value : false;
                                if (ehRetido)
                                {
                                    valorImposto = valorImposto + Math.Round(((baseCalculo * impostoFinanceiro.Aliquota) / 100), 2);
                                }
                            }
                        }

                        decimal valorAgrupadoTitulos = listaItensMedicaoGrupoTitulos.Sum(l => l.Valor);
                        decimal percentualApropriado = Math.Round((((valorApropriacao - valorImposto) / valorAgrupadoTitulos) * 100), 5);
                        decimal valorApropriado = valorApropriacao - valorImposto;

                        Apropriacao apropriacaoTituloReceberLiberacao = new Apropriacao();
                        PreencherApropriacaoTitulo(apropriacaoTituloReceberLiberacao,
                                                   medicaoItemApropriacao.ContratoRetificacaoItem.CodigoClasse,
                                                   medicaoItemApropriacao.Contrato.CodigoCentroCusto,
                                                   null,
                                                   medicaoItemApropriacao.TituloReceber,
                                                   percentualApropriado,
                                                   valorApropriado);


                        tituloReceberLiberacao.ListaApropriacao.Add(apropriacaoTituloReceberLiberacao);

                        #endregion
                    }
                }
            }

            #endregion
        }

        private Nullable<DateTime> CalcularDataVencimentoImposto(ImpostoFinanceiro impostoFinanceiro, Nullable<DateTime> dataEmissaoDocumento, Nullable<DateTime> dataVencimento)
        {
            Nullable<DateTime> dataVencimentoImposto = null;
            if (impostoFinanceiro.Periodicidade != null && dataEmissaoDocumento.HasValue && dataVencimento.HasValue)
            {
                DateTime dataGeradora = dataVencimento.Value;

                if (impostoFinanceiro.FatoGerador == FatoGeradorImpostoFinanceiro.EmissaoDocumento)
                    dataGeradora = dataEmissaoDocumento.Value;

                dataGeradora = dataGeradora.AddMonths(1);
                if (impostoFinanceiro.Periodicidade == PeriodicidadeImpostoFinanceiro.Quinzenal)
                {
                    if (dataGeradora.Day > 15)
                        dataVencimentoImposto = new DateTime(dataGeradora.Year, dataGeradora.Month, 15);
                    else
                        dataVencimentoImposto = new DateTime(dataGeradora.Year, dataGeradora.Month, DateTime.DaysInMonth(dataGeradora.Year, dataGeradora.Month));
                }
                else if (impostoFinanceiro.Periodicidade == PeriodicidadeImpostoFinanceiro.Mensal)
                {
                    int ultimoDiaDoMes = DateTime.DaysInMonth(dataGeradora.Year, dataGeradora.Month);
                    if (impostoFinanceiro.DiaVencimento.HasValue)
                    {
                        if (ultimoDiaDoMes < impostoFinanceiro.DiaVencimento.Value)
                        {
                            dataVencimentoImposto = new DateTime(dataGeradora.Year, dataGeradora.Month, ultimoDiaDoMes);
                        }
                        else
                        {
                            dataVencimentoImposto = new DateTime(dataGeradora.Year, dataGeradora.Month, impostoFinanceiro.DiaVencimento.Value);
                        }
                    }
                    else
                    {
                        dataVencimentoImposto = new DateTime(dataGeradora.Year, dataGeradora.Month, ultimoDiaDoMes);
                    }
                }

                var listaFeriados = feriadoRepository.ListarPeloFiltro(l => l.Ativo == true && l.Data.Value.Year == dataVencimentoImposto.Value.Year);
                while (dataVencimentoImposto.Value.DayOfWeek == DayOfWeek.Saturday
                    || dataVencimentoImposto.Value.DayOfWeek == DayOfWeek.Sunday
                    || listaFeriados.Any(l => l.Data.Value == dataVencimentoImposto.Value))
                {
                    if (impostoFinanceiro.FimDeSemana == FimDeSemanaImpostoFinanceiro.Antecipa)
                    {
                        dataVencimentoImposto = dataVencimentoImposto.Value.AddDays(-1);
                    }
                    else if (impostoFinanceiro.FimDeSemana == FimDeSemanaImpostoFinanceiro.Posterga)
                    {
                        dataVencimentoImposto = dataVencimentoImposto.Value.AddDays(1);
                    }
                }
            }

            return dataVencimentoImposto;
        }

        private string MontaIdentificacaoTituloImposto(string sigla, string nome)
        {
            string identificacaoImposto = sigla + " - " + nome;
            if (identificacaoImposto.Length > 50)
            {
                identificacaoImposto = identificacaoImposto.Substring(1, 50);
            }

            return identificacaoImposto;
        }

        private string MontaDocumentoTituloImposto(string numeroDocumento , string ordem )
        {
            string documentoImposto = "";
            if (!string.IsNullOrEmpty(numeroDocumento))
            {
                documentoImposto = numeroDocumento + "/" + ordem;
                if (documentoImposto.Length > 10)
                {
                    documentoImposto.Substring(1, 10);
                }
            }

            return documentoImposto;
        }

        private string MontaIdentificacaoTitulo(Domain.Entity.Contrato.Contrato contrato)
        {
            string numeroContrato10Caracteres = contrato.Id.Value.ToString();
            if (contrato.Id.Value.ToString().Length > 10)
            {
                numeroContrato10Caracteres = contrato.Id.Value.ToString().Substring(0, 10);
            }
            string descricaoContrato40Caracteres = contrato.ContratoDescricao.Descricao;
            if (contrato.ContratoDescricao.Descricao.Length > 40)
            {
                descricaoContrato40Caracteres = contrato.ContratoDescricao.Descricao.Substring(0, 40);
            }

            string identificacao = "Contrato : " + numeroContrato10Caracteres + " - " + descricaoContrato40Caracteres;

            return identificacao;
        }

        private void PreencherApropriacaoTituloAdiantamento(ApropriacaoAdiantamento apropriacaoAdiantamento, TituloPagarAdiantamento tituloPagarAdiantamento, Domain.Entity.Contrato.Contrato contrato, ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao)
        {
            apropriacaoAdiantamento.CodigoClasse = contratoRetificacaoItemMedicao.ContratoRetificacaoItem.CodigoClasse;
            apropriacaoAdiantamento.CodigoCentroCusto = contrato.CodigoCentroCusto;
            apropriacaoAdiantamento.TituloPagarAdiantamento = tituloPagarAdiantamento;
            apropriacaoAdiantamento.Valor = contratoRetificacaoItemMedicao.Valor;
            apropriacaoAdiantamento.Percentual = 100m;
        }

        private void PreencherMovimentoEstoque(Movimento movimento, 
                                               int clienteId,
                                               int contratoId,
                                               int tipoDocumentoId,
                                               string numeroDocumento, 
                                               DateTime dataEmissao, 
                                               DateTime dataEntrega,
                                               int estoqueId, 
                                               TipoMovimentoEstoque tipoMovimento)
        {
            string observacao = "";
            if (tipoMovimento == TipoMovimentoEstoque.SaidaContrato) observacao = "Motivo cancelamento de liberação";

            movimento.EstoqueId = estoqueId;
            movimento.ClienteFornecedorId = clienteId;
            movimento.TipoMovimento = tipoMovimento;
            DateTime dataAtualSemHora = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            movimento.Data = dataAtualSemHora;
            movimento.Observacao = observacao;
            movimento.TipoDocumentoId = tipoDocumentoId;
            movimento.Documento = numeroDocumento;
            movimento.DataEmissao = dataEmissao;
            movimento.DataEntrega = dataEntrega;
            movimento.ContratoId = contratoId;
            movimento.DataOperacao = DateTime.Now;
            movimento.LoginUsuarioOperacao = UsuarioLogado.Login;
            movimento.EhMovimentoTemporario = false;
        }

        private void PreencherMovimentoItemEstoque(MovimentoItem movimentoItem, Movimento movimento, ContratoRetificacaoItemMedicaoNF contratoRetificacaoItemMedicaoNF)
        {
            movimentoItem.MaterialId = contratoRetificacaoItemMedicaoNF.MaterialId;
            movimentoItem.CodigoClasse = contratoRetificacaoItemMedicaoNF.CodigoClasse;
            movimentoItem.Quantidade = contratoRetificacaoItemMedicaoNF.Quantidade;
            movimentoItem.Valor = contratoRetificacaoItemMedicaoNF.ValorUnitario;
            if (movimento.TipoMovimento == TipoMovimentoEstoque.SaidaContrato)
            {
                movimentoItem.Quantidade = contratoRetificacaoItemMedicaoNF.Quantidade * -1;
                movimentoItem.Valor = 0;
            }
            movimentoItem.Movimento = movimento;
        }

        private void PreencherApropriacaoTitulo(Apropriacao apropriacao, string codigoClasse, string codigoCentroCusto, TituloPagar tituloPagar, TituloReceber tituloReceber, decimal percentual, decimal valor)
        {
            apropriacao.Id = null;
            apropriacao.CodigoClasse = codigoClasse;
            apropriacao.CodigoCentroCusto = codigoCentroCusto;
            apropriacao.TituloPagarId = null;
            apropriacao.TituloPagar = tituloPagar;
            apropriacao.TituloReceberId = null;
            apropriacao.TituloReceber = tituloReceber;
            apropriacao.Percentual = percentual;
            apropriacao.Valor = valor;
        }

        private void PreencherImpostoReceber(ImpostoReceber impostoReceber, int impostoFinanceiroId, TituloReceber tituloReceber, decimal baseCalculo, decimal valorImposto)
        {
            impostoReceber.Id = null;
            impostoReceber.ImpostoFinanceiroId = impostoFinanceiroId;
            impostoReceber.TituloReceberId = null;
            impostoReceber.TituloReceber = tituloReceber;
            impostoReceber.BaseCalculo = baseCalculo;
            impostoReceber.ValorImposto = valorImposto;
        }

        private void PreencherImpostoPagar(ImpostoPagar impostoPagar, int impostoFinanceiroId, TituloPagar tituloPagar, decimal baseCalculo, decimal valorImposto, TituloPagar tituloPagarImposto)
        {
            impostoPagar.Id = null;
            impostoPagar.ImpostoFinanceiroId = impostoFinanceiroId;
            impostoPagar.TituloPagarId = null;
            impostoPagar.TituloPagar = tituloPagar;
            impostoPagar.BaseCalculo = baseCalculo;
            impostoPagar.ValorImposto = valorImposto;
            impostoPagar.TituloPagarImpostoId = null;
            impostoPagar.TituloPagarImposto = tituloPagarImposto;
        }

        private void PreencherTituloPagarAdiantamento(TituloPagarAdiantamento tituloPagarAdiantamento, Domain.Entity.Contrato.Contrato contrato, ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao, ContratoRetificacaoProvisao contratoRetificacaoProvisao)
        {
            ClienteFornecedor cliente = ObtemClienteFornecedor(contrato, contratoRetificacaoItemMedicao);
            string identificacao = MontaIdentificacaoTitulo(contrato);

            tituloPagarAdiantamento.ClienteId = cliente.Id;
            tituloPagarAdiantamento.Identificacao = identificacao;
            tituloPagarAdiantamento.TipoDocumentoId = contratoRetificacaoItemMedicao.TipoDocumentoId;
            tituloPagarAdiantamento.Documento = contratoRetificacaoItemMedicao.NumeroDocumento;
            tituloPagarAdiantamento.DataEmissaoDocumento = contratoRetificacaoItemMedicao.DataEmissao;
            tituloPagarAdiantamento.ValorAdiantamento = contratoRetificacaoItemMedicao.Valor;
            tituloPagarAdiantamento.LoginUsuarioCadastro = UsuarioLogado.Login;
            tituloPagarAdiantamento.DataCadastro = DateTime.Now;
            tituloPagarAdiantamento.TituloPagarId = contratoRetificacaoProvisao.TituloPagarId;
        }


        private void PreencherTituloPagar(TituloPagar tituloPagar, 
                                          int? clienteId, 
                                          string identificacao,
                                          SituacaoTituloPagar situacao, 
                                          int? tipoDocumentoId, 
                                          string numeroDocumento,
                                          Nullable<DateTime> dataVencimento,
                                          Nullable<DateTime> dataEmissaoDocumento,
                                          decimal valorTitulo,
                                          decimal? valorDesconto,
                                          string motivoDesconto,
                                          decimal? valorRetido,
                                          int? tipoCompromissoId){
            tituloPagar.Id = null;
            tituloPagar.ClienteId =  clienteId.Value;
            tituloPagar.TipoCompromissoId = tipoCompromissoId;
            tituloPagar.Identificacao = identificacao;
            tituloPagar.Situacao = situacao;
            tituloPagar.TipoDocumentoId = tipoDocumentoId;
            tituloPagar.Documento = numeroDocumento;
            tituloPagar.DataVencimento = dataVencimento.Value;
            tituloPagar.DataEmissaoDocumento = dataEmissaoDocumento.Value;
            tituloPagar.TipoTitulo = TipoTitulo.Normal;
            tituloPagar.TituloPaiId = 0;
            tituloPagar.Parcela = 0;
            tituloPagar.ValorTitulo = valorTitulo;
            tituloPagar.ValorImposto = 0;
            tituloPagar.Desconto = valorDesconto;
            if (valorDesconto.HasValue && valorDesconto.Value > 0)
            {
                tituloPagar.DataLimiteDesconto = dataVencimento.Value;
            }
            else
            {
                tituloPagar.DataLimiteDesconto = null;
            }
            tituloPagar.Multa = 0;
            tituloPagar.EhMultaPercentual = false;
            tituloPagar.TaxaPermanencia = 0;
            tituloPagar.EhTaxaPermanenciaPercentual = false;
            tituloPagar.MotivoDesconto = motivoDesconto;
            tituloPagar.LoginUsuarioCadastro = UsuarioLogado.Login;
            tituloPagar.DataCadastro = DateTime.Now;
            tituloPagar.LoginUsuarioSituacao = UsuarioLogado.Login;
            tituloPagar.DataSituacao = DateTime.Now;
            tituloPagar.Retencao = valorRetido;
            tituloPagar.SistemaOrigem = "CTR";
        }

        private void PreencherTituloReceber(TituloReceber tituloReceber,
                                            int? clienteId,
                                            string identificacao,
                                            SituacaoTituloReceber situacao,
                                            int? tipoDocumentoId,
                                            string numeroDocumento,
                                            Nullable<DateTime> dataVencimento,
                                            Nullable<DateTime> dataEmissaoDocumento,
                                            decimal valorTitulo,
                                            decimal? valorDesconto,
                                            string motivoDesconto,
                                            decimal? valorRetido,
                                            int? tipoCompromissoId)
        {
            tituloReceber.Id = null;
            tituloReceber.ClienteId = clienteId.Value;
            tituloReceber.TipoCompromissoId = tipoCompromissoId;
            tituloReceber.Identificacao = identificacao;
            tituloReceber.Situacao = situacao;
            tituloReceber.TipoDocumentoId = tipoDocumentoId;
            tituloReceber.Documento = numeroDocumento;
            tituloReceber.DataVencimento = dataVencimento.Value;
            tituloReceber.DataEmissaoDocumento = dataEmissaoDocumento.Value;
            tituloReceber.TipoTitulo = TipoTitulo.Normal;
            tituloReceber.TituloPaiId = 0;
            tituloReceber.Parcela = 0;
            tituloReceber.ValorTitulo = valorTitulo;
            tituloReceber.ValorImposto = 0;
            tituloReceber.Desconto = valorDesconto;
            tituloReceber.DataLimiteDesconto = dataVencimento.Value;
            tituloReceber.Multa = 0;
            tituloReceber.EhMultaPercentual = false;
            tituloReceber.TaxaPermanencia = 0;
            tituloReceber.EhTaxaPermanenciaPercentual = false;
            tituloReceber.MotivoDesconto = motivoDesconto;
            tituloReceber.LoginUsuarioCadastro = UsuarioLogado.Login;
            tituloReceber.DataCadastro = DateTime.Now;
            tituloReceber.LoginUsuarioSituacao = UsuarioLogado.Login;
            tituloReceber.DataSituacao = DateTime.Now;
            tituloReceber.Retencao = valorRetido;
            tituloReceber.SistemaOrigem = "CTR";
        }

        private void CriarListaDeMedicaoSelecionadas(List<ItemLiberacaoDTO> listaItemLiberacaoDTO, Domain.Entity.Contrato.Contrato contrato, List<ContratoRetificacaoItemMedicao> listaMedicaoSelecionadas, OperacaoLiberarMedicao operacao){
            if (operacao == OperacaoLiberarMedicao.AprovarLiberar)
            {
                foreach (ItemLiberacaoDTO item in listaItemLiberacaoDTO.Where(l => l.Selecionado == true && l.CodigoSituacao < (int)SituacaoMedicao.Liberado))
                {
                    ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao = contrato.ListaContratoRetificacaoItemMedicao.Where(l => l.Id == item.ContratoRetificacaoItemMedicaoId).FirstOrDefault();
                    listaMedicaoSelecionadas.Add(contratoRetificacaoItemMedicao);
                }
            }
            else
            {
                if (operacao == OperacaoLiberarMedicao.Cancelar)
                {
                    foreach (ItemLiberacaoDTO item in listaItemLiberacaoDTO.Where(l => l.Selecionado == true && l.CodigoSituacao == (int)SituacaoMedicao.Liberado))
                    {
                        ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao = contrato.ListaContratoRetificacaoItemMedicao.Where(l => l.Id == item.ContratoRetificacaoItemMedicaoId).FirstOrDefault();
                        List<ContratoRetificacaoItemMedicao> listaContratoRetificacaoItemMedicao = new List<ContratoRetificacaoItemMedicao>();

                        if (contrato.TipoContrato == TipoContrato.ContratoAPagar)
                        {
                            if (contratoRetificacaoItemMedicao.TituloPagarId == null)
                            {
                                listaMedicaoSelecionadas.Add(contratoRetificacaoItemMedicao);
                            }
                            else
                            {
                                bool jaFoiInserido = listaMedicaoSelecionadas.Any(l => l.TituloPagarId == contratoRetificacaoItemMedicao.TituloPagarId);
                                if (!jaFoiInserido)
                                {
                                    listaContratoRetificacaoItemMedicao = contrato.ListaContratoRetificacaoItemMedicao.Where(l => l.TituloPagarId == contratoRetificacaoItemMedicao.TituloPagarId).ToList();
                                    listaMedicaoSelecionadas.AddRange(listaContratoRetificacaoItemMedicao);
                                }
                            }
                        }
                        else
                        {
                            if (contratoRetificacaoItemMedicao.TituloReceberId == null)
                            {
                                listaMedicaoSelecionadas.Add(contratoRetificacaoItemMedicao);
                            }
                            else
                            {
                                bool jaFoiInserido = listaMedicaoSelecionadas.Any(l => l.TituloReceberId == contratoRetificacaoItemMedicao.TituloReceberId);
                                if (!jaFoiInserido)
                                {
                                    listaContratoRetificacaoItemMedicao = contrato.ListaContratoRetificacaoItemMedicao.Where(l => l.TituloReceberId == contratoRetificacaoItemMedicao.TituloReceberId).ToList();
                                    listaMedicaoSelecionadas.AddRange(listaContratoRetificacaoItemMedicao);
                                }
                            }
                        }
                    }
                }
            }
        }

        private ClienteFornecedor ObtemClienteFornecedor(Domain.Entity.Contrato.Contrato contrato, ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao)
        {
            ClienteFornecedor cliente = new ClienteFornecedor();
            if (contrato.TipoContrato == TipoContrato.ContratoAPagar)
            {
                cliente = contrato.Contratado;
            }
            else
            {
                cliente = contrato.Contratante;
            }
            if (contratoRetificacaoItemMedicao != null)
            {
                if (contratoRetificacaoItemMedicao.MultiFornecedorId.HasValue && contratoRetificacaoItemMedicao.MultiFornecedorId > 0)
                {
                    cliente = contratoRetificacaoItemMedicao.MultiFornecedor;
                }
            }

            return cliente;
        }

        private void AtualizarContratoRetencao(List<ContratoRetificacaoItemMedicao> listaMedicaoSelecionadas, Domain.Entity.Contrato.Contrato contrato)
        {
            foreach (ContratoRetificacaoItemMedicao item in listaMedicaoSelecionadas)
            {
                ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao = contrato.ListaContratoRetificacaoItemMedicao.Where(l => l.Id == item.Id).FirstOrDefault();

                if (contratoRetificacaoItemMedicao.ValorRetido > 0)
                {
                    ContratoRetencao contratoRetencao = new ContratoRetencao();
                    contratoRetencao.ContratoId = contratoRetificacaoItemMedicao.ContratoId;
                    contratoRetencao.ContratoRetificacaoId = contratoRetificacaoItemMedicao.ContratoRetificacaoId;
                    contratoRetencao.ContratoRetificacaoItemId = contratoRetificacaoItemMedicao.ContratoRetificacaoItemId;
                    contratoRetencao.ContratoRetificacaoItemMedicaoId = contratoRetificacaoItemMedicao.Id.Value;
                    contratoRetencao.ValorRetencao = contratoRetificacaoItemMedicao.ValorRetido;
                    contrato.ListaContratoRetencao.Add(contratoRetencao);
                }
            }
        }

        private void DeletarContratoRetencao(List<ContratoRetificacaoItemMedicao> listaMedicaoSelecionadas, Domain.Entity.Contrato.Contrato contrato)
        {
            foreach (ContratoRetificacaoItemMedicao item in listaMedicaoSelecionadas)
            {
                if (contrato.ListaContratoRetencao.Any(cr => cr.ContratoRetificacaoItemMedicaoId == item.Id))
                {
                    ContratoRetencao contratoRetencao = contrato.ListaContratoRetencao.Where(cr => cr.ContratoRetificacaoItemMedicaoId == item.Id).FirstOrDefault();
                    if (contratoRetencao != null)
                    {
                        contratoRetencaoRepository.Remover(contratoRetencao);
                    }
                }
            }
        }

        private void ReprovisionarContratoProvisao(List<ContratoRetificacaoItemMedicao> listaMedicaoSelecionadas, Domain.Entity.Contrato.Contrato contrato)
        {
            ContratoRetificacao ultimoContratoRetificacao = contrato.ListaContratoRetificacao.Last();

            foreach (ContratoRetificacaoItemMedicao item in listaMedicaoSelecionadas)
            {
                ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao = contrato.ListaContratoRetificacaoItemMedicao.Where(l => l.Id == item.Id).FirstOrDefault();

                ContratoRetificacaoProvisao contratoRetificacaoProvisao = contrato.ListaContratoRetificacaoProvisao.Where(l => l.SequencialItem == contratoRetificacaoItemMedicao.SequencialItem && l.SequencialCronograma == contratoRetificacaoItemMedicao.SequencialCronograma).FirstOrDefault();

                bool ehPagamentoAntecipado = contratoRetificacaoProvisao.PagamentoAntecipado.HasValue ? contratoRetificacaoProvisao.PagamentoAntecipado.Value : false;
                if (ehPagamentoAntecipado)
                {
                    #region "Antecipação - Trata titulo, apropriação e armazena o valor adiantado no contrato provisao"

                    if (contrato.TipoContrato == TipoContrato.ContratoAPagar)
                    {
                        TituloPagarAdiantamento tituloPagarAdiantamento = new TituloPagarAdiantamento();
                        PreencherTituloPagarAdiantamento(tituloPagarAdiantamento, contrato, contratoRetificacaoItemMedicao, contratoRetificacaoProvisao);
                        contratoRetificacaoProvisao.TituloPagar.ListaTituloPagarAdiantamento.Add(tituloPagarAdiantamento);

                        ApropriacaoAdiantamento apropriacaoAdiantamento = new ApropriacaoAdiantamento();
                        PreencherApropriacaoTituloAdiantamento(apropriacaoAdiantamento, tituloPagarAdiantamento, contrato, contratoRetificacaoItemMedicao);
                        contratoRetificacaoItemMedicao.DataLiberacao = DateTime.Now;
                        contratoRetificacaoItemMedicao.UsuarioLiberacao = UsuarioLogado.Login;
                        contratoRetificacaoItemMedicao.Situacao = SituacaoMedicao.Liberado;
                        tituloPagarAdiantamento.ListaApropriacaoAdiantamento.Add(apropriacaoAdiantamento);

                        decimal valorDescontado = contratoRetificacaoProvisao.ValorAdiantadoDescontado.HasValue ? contratoRetificacaoProvisao.ValorAdiantadoDescontado.Value : 0;
                        valorDescontado = valorDescontado + contratoRetificacaoItemMedicao.Valor;

                        contratoRetificacaoProvisao.ValorAdiantadoDescontado = valorDescontado;
                    }

                    #endregion
                }
                else
                {
                    #region "Sem antecipação - Trata título, apropriação, imposto  do contrato provisao"

                    bool ehProvisionamentoParcial = ReCalcularProvisao(contratoRetificacaoProvisao, contratoRetificacaoItemMedicao);

                    decimal valorRetencao = 0;

                    if (ehProvisionamentoParcial)
                    {
                        ContratoRetificacaoItem contratoRetificacaoItem = contratoRetificacaoItemMedicao.ContratoRetificacaoItem;

                        valorRetencao = CalcularValorRetencao(ultimoContratoRetificacao, contratoRetificacaoProvisao, contratoRetificacaoItem);

                        if (contrato.TipoContrato == TipoContrato.ContratoAPagar)
                        {
                            ReApropriarContratoProvisaoParcialAPagar(contratoRetificacaoProvisao, valorRetencao);
                            ReCalcularImpostoProvisaoParcialAPagar(contrato, contratoRetificacaoProvisao);
                        }
                        else
                        {
                            ReApropriarContratoProvisaoParcialAReceber(contratoRetificacaoProvisao, valorRetencao);
                            ReCalcularImpostoProvisaoParcialAReceber(contrato, contratoRetificacaoProvisao);
                        }
                    }
                    else
                    {
                        if (contrato.TipoContrato == TipoContrato.ContratoAPagar)
                        {
                            if (contratoRetificacaoProvisao.TituloPagar.Situacao < SituacaoTituloPagar.Emitido)
                            {
                                DeletarTituloPagarEhApropriacoesComDesdobradosDeImpostoPagar(contratoRetificacaoProvisao.TituloPagar);
                                RemoverImpostosDoTituloAPagar(contratoRetificacaoProvisao.TituloPagar);
                                DeletarTituloPagarEhApropriacoesComDesdobrados(contratoRetificacaoProvisao.TituloPagar);
                            }
                        }
                        else
                        {
                            if (contratoRetificacaoProvisao.TituloReceber.Situacao < SituacaoTituloReceber.Predatado)
                            {
                                RemoverImpostosDoTituloAReceber(contratoRetificacaoProvisao.TituloReceber);
                                DeletarTituloReceberEhApropriacoesComDesdobrados(contratoRetificacaoProvisao.TituloReceber);
                            }
                        }

                        RemoverContratoProvisao(contrato, contratoRetificacaoProvisao);
                    }

                    #endregion
                }               
            }
        }

        private void RemoverContratoProvisao(Domain.Entity.Contrato.Contrato contrato, ContratoRetificacaoProvisao contratoRetificacaoProvisao)
        {
            if (contrato.ListaContratoRetificacaoProvisao.Any(l => l.Id == contratoRetificacaoProvisao.Id))
            {
                contratoRetificacaoProvisaoRepository.Remover(contratoRetificacaoProvisao);
            }
        }

        void ReCalcularImpostoProvisaoParcialAReceber(Domain.Entity.Contrato.Contrato contrato, ContratoRetificacaoProvisao contratoRetificacaoProvisao)
        {
            if (contrato.ListaContratoRetificacaoItemImposto.Any(l => l.ContratoRetificacaoItemId == contratoRetificacaoProvisao.ContratoRetificacaoItemId))
            {
                decimal valorImpostoRetidoTotal = 0;

                foreach (ContratoRetificacaoItemImposto contratoRetificacaoItemImposto in contrato.ListaContratoRetificacaoItemImposto.Where(l => l.ContratoRetificacaoItemId == contratoRetificacaoProvisao.ContratoRetificacaoItemId))
                {
                    ImpostoFinanceiro impostoFinanceiro = impostoFinanceiroRepository.ObterPeloId(contratoRetificacaoItemImposto.ImpostoFinanceiroId);

                    decimal valorBaseCalculo = ((contratoRetificacaoProvisao.Valor * contratoRetificacaoItemImposto.PercentualBaseCalculo) / 100);
                    if (valorBaseCalculo > 0)
                    {
                        decimal valorImposto = Math.Round(((valorBaseCalculo * impostoFinanceiro.Aliquota) / 100), 2);
                        bool ehRetido = impostoFinanceiro.EhRetido.HasValue ? impostoFinanceiro.EhRetido.Value : false;
                        if (ehRetido)
                        {
                            valorImpostoRetidoTotal = valorImpostoRetidoTotal + valorImposto;
                        }
                        ImpostoReceber impostoReceber = null;
                        if (contratoRetificacaoProvisao.TituloReceber.ListaImpostoReceber.Count > 0 && contratoRetificacaoProvisao.TituloReceber.ListaImpostoReceber.All(l => !l.Id.HasValue))
                        {
                            impostoReceber = contratoRetificacaoProvisao.TituloReceber.ListaImpostoReceber.Where(l => l.ImpostoFinanceiroId == impostoFinanceiro.Id).FirstOrDefault();
                            if (impostoReceber != null)
                            {
                                impostoReceber.BaseCalculo = valorBaseCalculo;
                                impostoReceber.ValorImposto = valorImposto;
                            }
                            else
                            {
                                impostoReceber = new ImpostoReceber();
                                PreencherImpostoReceber(impostoReceber,
                                                        impostoFinanceiro.Id.Value,
                                                        contratoRetificacaoProvisao.TituloReceber,
                                                        valorBaseCalculo,
                                                        valorImposto);

                                contratoRetificacaoProvisao.TituloReceber.ListaImpostoReceber.Add(impostoReceber);
                            }
                        }
                        else
                        {
                            impostoReceber = new ImpostoReceber();
                            PreencherImpostoReceber(impostoReceber,
                                                    impostoFinanceiro.Id.Value,
                                                    contratoRetificacaoProvisao.TituloReceber,
                                                    valorBaseCalculo,
                                                    valorImposto);

                            contratoRetificacaoProvisao.TituloReceber.ListaImpostoReceber.Add(impostoReceber);
                        }
                    }
                }
                contratoRetificacaoProvisao.TituloReceber.ValorImposto = null;
                if (valorImpostoRetidoTotal > 0)
                {
                    contratoRetificacaoProvisao.TituloReceber.ValorImposto = valorImpostoRetidoTotal;
                }
            }
        }

        void ReCalcularImpostoProvisaoParcialAPagar(Domain.Entity.Contrato.Contrato contrato, ContratoRetificacaoProvisao contratoRetificacaoProvisao)
        {
            if (contrato.ListaContratoRetificacaoItemImposto.Any(l => l.ContratoRetificacaoItemId == contratoRetificacaoProvisao.ContratoRetificacaoItemId))
            {
                decimal valorImpostoRetidoTotal = 0;

                foreach (ContratoRetificacaoItemImposto contratoRetificacaoItemImposto in contrato.ListaContratoRetificacaoItemImposto.Where(l => l.ContratoRetificacaoItemId == contratoRetificacaoProvisao.ContratoRetificacaoItemId))
                {
                    ImpostoFinanceiro impostoFinanceiro = impostoFinanceiroRepository.ObterPeloId(contratoRetificacaoItemImposto.ImpostoFinanceiroId);

                    decimal valorBaseCalculo = ((contratoRetificacaoProvisao.Valor * contratoRetificacaoItemImposto.PercentualBaseCalculo) / 100);
                    if (valorBaseCalculo > 0)
                    {
                        decimal valorImposto = Math.Round(((valorBaseCalculo * impostoFinanceiro.Aliquota) / 100), 2);
                        bool ehRetido = impostoFinanceiro.EhRetido.HasValue ? impostoFinanceiro.EhRetido.Value : false;
                        if (ehRetido)
                        {
                            valorImpostoRetidoTotal = valorImpostoRetidoTotal + valorImposto;
                        }
                        ImpostoPagar impostoPagar = null;
                        if (contratoRetificacaoProvisao.TituloPagar.ListaImpostoPagar.Count > 0 && contratoRetificacaoProvisao.TituloPagar.ListaImpostoPagar.All(l => !l.Id.HasValue))
                        {
                            impostoPagar = contratoRetificacaoProvisao.TituloPagar.ListaImpostoPagar.Where(l => l.ImpostoFinanceiroId == impostoFinanceiro.Id).FirstOrDefault();
                            if (impostoPagar != null)
                            {
                                impostoPagar.BaseCalculo = valorBaseCalculo;
                                impostoPagar.ValorImposto = valorImposto;
                            }
                            else
                            {
                                impostoPagar = new ImpostoPagar();

                                PreencherImpostoPagar(impostoPagar,
                                                      impostoFinanceiro.Id.Value,
                                                      contratoRetificacaoProvisao.TituloPagar,
                                                      valorBaseCalculo,
                                                      valorImposto,
                                                      null);

                                contratoRetificacaoProvisao.TituloPagar.ListaImpostoPagar.Add(impostoPagar);
                            }
                        }
                        else
                        {
                            impostoPagar = new ImpostoPagar();

                            PreencherImpostoPagar(impostoPagar, 
                                                    impostoFinanceiro.Id.Value,
                                                    contratoRetificacaoProvisao.TituloPagar,
                                                    valorBaseCalculo,
                                                    valorImposto, 
                                                    null);

                            contratoRetificacaoProvisao.TituloPagar.ListaImpostoPagar.Add(impostoPagar);

                        }
                    }
                }
                contratoRetificacaoProvisao.TituloPagar.ValorImposto = null;
                if (valorImpostoRetidoTotal > 0)
                {
                    contratoRetificacaoProvisao.TituloPagar.ValorImposto = valorImpostoRetidoTotal;
                }
            }
        }

        void ReApropriarContratoProvisaoParcialAReceber(ContratoRetificacaoProvisao contratoRetificacaoProvisao, decimal valorRetencao)
        {
            decimal valorApropriacao = contratoRetificacaoProvisao.Valor - valorRetencao;
            decimal percentualApropriacao = Math.Round(((valorApropriacao / contratoRetificacaoProvisao.Valor) * 100), 5);

            if (contratoRetificacaoProvisao.TituloReceber.Situacao < SituacaoTituloReceber.Predatado)
            {
                contratoRetificacaoProvisao.TituloReceber.ValorTitulo = contratoRetificacaoProvisao.Valor;
                contratoRetificacaoProvisao.TituloReceber.Retencao = valorRetencao;
            }

            foreach (Apropriacao apropriacao in contratoRetificacaoProvisao.TituloReceber.ListaApropriacao)
            {
                apropriacao.Valor = valorApropriacao;
                apropriacao.Percentual = percentualApropriacao;
            }

            if (contratoRetificacaoProvisao.TituloReceber.Situacao < SituacaoTituloReceber.Predatado)
            {
                RemoverImpostosDoTituloAReceber(contratoRetificacaoProvisao.TituloReceber);
            }
        }

        void ReApropriarContratoProvisaoParcialAPagar(ContratoRetificacaoProvisao contratoRetificacaoProvisao, decimal valorRetencao)
        {
            decimal valorApropriacao = contratoRetificacaoProvisao.Valor - valorRetencao;
            decimal percentualApropriacao = Math.Round(((valorApropriacao / contratoRetificacaoProvisao.Valor) * 100), 5);

            if (contratoRetificacaoProvisao.TituloPagar.Situacao < SituacaoTituloPagar.Emitido)
            {
                contratoRetificacaoProvisao.TituloPagar.ValorTitulo = contratoRetificacaoProvisao.Valor;
                contratoRetificacaoProvisao.TituloPagar.Retencao = valorRetencao;
            }

            foreach (Apropriacao apropriacao in contratoRetificacaoProvisao.TituloPagar.ListaApropriacao)
            {
                apropriacao.Valor = valorApropriacao;
                apropriacao.Percentual = percentualApropriacao;
            }

            if (contratoRetificacaoProvisao.TituloPagar.Situacao < SituacaoTituloPagar.Emitido)
            {
                DeletarTituloPagarEhApropriacoesComDesdobradosDeImpostoPagar(contratoRetificacaoProvisao.TituloPagar);
                RemoverImpostosDoTituloAPagar(contratoRetificacaoProvisao.TituloPagar);
            }
        }

        private decimal CalcularValorRetencao(ContratoRetificacao ultimoContratoRetificacao, ContratoRetificacaoProvisao contratoRetificacaoProvisao, ContratoRetificacaoItem contratoRetificacaoItem)
        {
            decimal retencaoContratual = ultimoContratoRetificacao.RetencaoContratual.HasValue ? ultimoContratoRetificacao.RetencaoContratual.Value : 0;
            decimal retencaoItem = 0;
            decimal baseRetencaoItem = 0;
            decimal retencaoPrazoItem = 0;
            decimal valorRetencao = 0;

            if (retencaoContratual == 0)
            {
                retencaoItem = contratoRetificacaoItem.RetencaoItem.HasValue ? contratoRetificacaoItem.RetencaoItem.Value : 0;
                baseRetencaoItem = contratoRetificacaoItem.BaseRetencaoItem.HasValue ? contratoRetificacaoItem.BaseRetencaoItem.Value : 0;
                retencaoPrazoItem = contratoRetificacaoItem.RetencaoPrazoResgate.HasValue ? contratoRetificacaoItem.RetencaoPrazoResgate.Value : 0;
            }

            if ((retencaoContratual > 0) || (retencaoItem > 0))
            {
                valorRetencao = Math.Round(((contratoRetificacaoProvisao.Valor * retencaoContratual) / 100), 2);

                if (retencaoItem > 0)
                {
                    decimal valorTituloRetencao = (contratoRetificacaoProvisao.Valor * baseRetencaoItem) / 100;
                    valorRetencao = Math.Round(((valorTituloRetencao * retencaoItem) / 100), 2);
                }
            }
            return valorRetencao;
        }

        private bool ReCalcularProvisao(ContratoRetificacaoProvisao contratoRetificacaoProvisao, ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao)
        {

            decimal valorProvisaoNova = contratoRetificacaoProvisao.Valor - contratoRetificacaoItemMedicao.Valor;
            decimal quantidadeNovaProvisao = contratoRetificacaoProvisao.Quantidade - contratoRetificacaoItemMedicao.Quantidade;

            bool ehProvisionamentoParcial = valorProvisaoNova > 0 ? true : false;

            if (ehProvisionamentoParcial)
            {
                contratoRetificacaoProvisao.Valor = valorProvisaoNova;
                contratoRetificacaoProvisao.Quantidade = quantidadeNovaProvisao;
            }
            
            return ehProvisionamentoParcial; 
        }

        private ContratoRetificacaoItemMedicao InseriuMedicao(Domain.Entity.Contrato.Contrato contrato, ContratoRetificacaoItemMedicaoDTO dto)
        {
            ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao;
            contratoRetificacaoItemMedicao = contrato.ListaContratoRetificacaoItemMedicao.Where(l => l.Id == dto.Id).FirstOrDefault() ?? new ContratoRetificacaoItemMedicao();

            ContratoRetificacao contratoRetificacao = contrato.ListaContratoRetificacao.Where(l => l.Id == dto.ContratoRetificacaoId).FirstOrDefault();
            ContratoRetificacaoItem contratoRetificacaoItem = contrato.ListaContratoRetificacaoItem.Where(l => l.Id == dto.ContratoRetificacaoItemId).FirstOrDefault();

            contratoRetificacaoItemMedicao.ContratoId = dto.ContratoId;
            contratoRetificacaoItemMedicao.Contrato = contrato;
            contratoRetificacaoItemMedicao.ContratoRetificacaoId = dto.ContratoRetificacaoId;
            contratoRetificacaoItemMedicao.ContratoRetificacaoItemId = dto.ContratoRetificacaoItemId;
            contratoRetificacaoItemMedicao.ContratoRetificacaoItem = contratoRetificacaoItem;
            contratoRetificacaoItemMedicao.ContratoRetificacaoItemCronogramaId = dto.ContratoRetificacaoItemCronogramaId;
            contratoRetificacaoItemMedicao.SequencialItem = dto.SequencialItem;
            contratoRetificacaoItemMedicao.SequencialCronograma = dto.SequencialCronograma;
            if (contratoRetificacaoItemMedicao.Id.HasValue && contratoRetificacaoItemMedicao.Situacao < SituacaoMedicao.Liberado)
            {
                contratoRetificacaoItemMedicao.Situacao = SituacaoMedicao.AguardandoAprovacao;
            }
            contratoRetificacaoItemMedicao.TipoDocumentoId = dto.TipoDocumentoId;
            contratoRetificacaoItemMedicao.NumeroDocumento = dto.NumeroDocumento;
            contratoRetificacaoItemMedicao.DataMedicao = dto.DataMedicao;
            contratoRetificacaoItemMedicao.DataEmissao = dto.DataEmissao;
            contratoRetificacaoItemMedicao.DataVencimento = dto.DataVencimento;
            contratoRetificacaoItemMedicao.Quantidade = dto.Quantidade;
            contratoRetificacaoItemMedicao.Valor = dto.Valor;
            contratoRetificacaoItemMedicao.MultiFornecedorId = dto.MultiFornecedor.Id;
            contratoRetificacaoItemMedicao.Observacao = dto.Observacao;
            contratoRetificacaoItemMedicao.Desconto = dto.Desconto;
            contratoRetificacaoItemMedicao.MotivoDesconto = dto.MotivoDesconto;

            contratoRetificacaoItemMedicao.TipoCompraCodigo = dto.TipoCompraCodigo;
            contratoRetificacaoItemMedicao.CifFobId = dto.CifFobId;
            contratoRetificacaoItemMedicao.NaturezaOperacaoCodigo = dto.NaturezaOperacaoCodigo;
            contratoRetificacaoItemMedicao.SerieNFId = dto.SerieNFId;
            contratoRetificacaoItemMedicao.CSTCodigo = dto.CSTCodigo;
            contratoRetificacaoItemMedicao.CodigoContribuicaoCodigo = dto.CodigoContribuicaoCodigo;
            contratoRetificacaoItemMedicao.CodigoBarras = dto.CodigoBarras;

            decimal valorRetido = 0.0m;
            decimal percentualRetencao = 0.0m;
            decimal percentualBaseCalculo = 0.0m;

            if (contratoRetificacao.RetencaoContratual.HasValue)
            {
                percentualRetencao = contratoRetificacao.RetencaoContratual.Value;
                percentualBaseCalculo = 100.0m;
            }

            if (contratoRetificacaoItem.RetencaoItem.HasValue)
            {
                percentualRetencao = contratoRetificacaoItem.RetencaoItem.Value;
                percentualBaseCalculo = 0;
                if (contratoRetificacaoItem.BaseRetencaoItem.HasValue) percentualBaseCalculo = contratoRetificacaoItem.BaseRetencaoItem.Value;
            }

            if (percentualRetencao > 0.0m)
            {
                decimal valorBaseCalculo = ((dto.Valor * percentualBaseCalculo) / 100);
                valorRetido = decimal.Round(((valorBaseCalculo * percentualRetencao) / 100), 2);
            }

            if (valorRetido > 0)
            {
                dto.ValorRetido = valorRetido;
                contratoRetificacaoItemMedicao.ValorRetido = valorRetido;
            }

            if (!contratoRetificacaoItemMedicao.Id.HasValue)
            {
                contratoRetificacaoItemMedicao.DataCadastro = DateTime.Now;
                contratoRetificacaoItemMedicao.UsuarioMedicao = UsuarioLogado.Login;
                contratoRetificacaoItemMedicao.Situacao = SituacaoMedicao.AguardandoAprovacao;

                contrato.ListaContratoRetificacaoItemMedicao.Add(contratoRetificacaoItemMedicao);    
            }

            return contratoRetificacaoItemMedicao;
        }

        private bool EhValidoSalvarMedicao(ContratoRetificacaoItemMedicao medicao)
        {
            ParametrosContrato parametros = parametrosContratoRepository.Obter();

            if (medicao != null)
            {

                if (medicao.Situacao > SituacaoMedicao.AguardandoLiberacao)
                {
                    messageQueue.Add(Application.Resource.Contrato.ErrorMessages.SituacaoNaoPermitida, TypeMessage.Error);
                    return false;
                }

                DateTime DataLimiteMedicao = DateTime.Now;

                if (parametros != null && (medicao.DataVencimento != null) && (medicao.DataMedicao != null))
                {
                    if (parametros.DiasPagamento.HasValue)
                    {

                        DataLimiteMedicao = DateTime.Now.AddDays((parametros.DiasPagamento.Value * -1));

                        if (parametros.DiasPagamento.Value > 0)
                        {
                            int numeroDias = (int)medicao.DataVencimento.Subtract(medicao.DataMedicao).TotalDays;
                            if (numeroDias < parametros.DiasPagamento.Value)
                            {
                                messageQueue.Add(Application.Resource.Contrato.ErrorMessages.DataVencimentoForaDoLimite, TypeMessage.Error);
                                return false;
                            }
                        }
                    }
                }

                if (ComparaDatas(medicao.DataMedicao.ToShortDateString(), DataLimiteMedicao.ToShortDateString()) < 0)
                {
                    messageQueue.Add(Application.Resource.Contrato.ErrorMessages.DataMedicaoMenorQueDataLimite, TypeMessage.Error);
                    return false;
                }

                if (medicao.ContratoRetificacaoItem != null)
                {
                    if (medicao.ContratoRetificacaoItem.NaturezaItem == NaturezaItem.PrecoGlobal)
                    {
                        if (medicao.Valor == 0)
                        {
                            messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.CampoObrigatorio, "Valor medição atual"), TypeMessage.Error);
                            return false;
                        }

                        decimal valorTotalLiberado = medicao.Contrato.ObterValorTotalLiberado(medicao.SequencialItem, medicao.SequencialCronograma);
                        decimal valorTotalMedido = medicao.Contrato.ObterValorTotalMedido(medicao.SequencialItem, medicao.SequencialCronograma);
                        decimal valorItem = 0;
                        if (medicao.ContratoRetificacaoItem.ValorItem.HasValue)
                        {
                            valorItem = medicao.ContratoRetificacaoItem.ValorItem.Value;
                        }

                        decimal valorMedidoEfetivo = 0;
                        if (valorTotalMedido > medicao.Valor) 
                        {
                            valorMedidoEfetivo = valorTotalMedido - medicao.Valor;
                        }
                        else 
                        {
                            valorMedidoEfetivo = medicao.Valor - valorTotalMedido;
                        }
                        decimal valorPendente = valorItem - (valorMedidoEfetivo + valorTotalLiberado);

                        if (medicao.Valor > valorPendente)
                        {
                            messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.ValorMaiorQue, "Valor medição atual", "Valor pendente"), TypeMessage.Error);
                            return false;
                        }
                    }
                    else if (medicao.ContratoRetificacaoItem.NaturezaItem == NaturezaItem.PrecoUnitario)
                    {

                        decimal quantidadeTotalLiberada = medicao.Contrato.ObterQuantidadeTotalLiberada(medicao.SequencialItem, medicao.SequencialCronograma);
                        decimal quantidadeTotalMedida = medicao.Contrato.ObterQuantidadeTotalMedida(medicao.SequencialItem, medicao.SequencialCronograma);

                        decimal quantidadeMedidaEfetiva = 0;
                        if (quantidadeTotalMedida > medicao.Quantidade)
                        {
                            quantidadeMedidaEfetiva = quantidadeTotalMedida - medicao.Quantidade;
                        }
                        else
                        {
                            quantidadeMedidaEfetiva = medicao.Quantidade - quantidadeTotalMedida;
                        }

                        decimal quantidadePendente = medicao.ContratoRetificacaoItem.Quantidade - (quantidadeMedidaEfetiva + quantidadeTotalLiberada);

                        if (medicao.Quantidade > quantidadePendente)
                        {
                            messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.ValorMaiorQue, "Quantidade medição atual", "Quantidade pendente"), TypeMessage.Error);
                            return false;
                        }
                    }
                }

                Nullable<DateTime> dataBloqueio;

                if (bloqueioContabilAppService.OcorreuBloqueioContabil(medicao.Contrato.CentroCusto.Codigo,
                                                                        medicao.DataEmissao,
                                                                        out dataBloqueio))
                {
                    string msg = string.Format(Resource.Sigim.ErrorMessages.BloqueioContabilEncontrado, dataBloqueio.Value.ToShortDateString(), medicao.Contrato.CentroCusto.Codigo);
                    messageQueue.Add(msg, TypeMessage.Error);
                    return false;
                }

                if (parametros == null) return true;

                if (!parametros.DadosSped.HasValue) return true;

                if (parametros.DadosSped.Value)
                {
                    bool ok = true;

                    if (string.IsNullOrEmpty(medicao.TipoCompraCodigo))
                    {
                        messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Tipo compra"), TypeMessage.Error);
                        ok = false;
                    }

                    if ((!medicao.CifFobId.HasValue) || (medicao.CifFobId.Value == 0))
                    {
                        messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "CIF/FOB"), TypeMessage.Error);
                        ok = false;
                    }

                    if ((string.IsNullOrEmpty(medicao.NaturezaOperacaoCodigo)))
                    {
                        messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Natureza de operação"), TypeMessage.Error);
                        ok = false;
                    }

                    if ((!medicao.SerieNFId.HasValue) || (medicao.SerieNFId.Value == 0))
                    {
                        messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Série"), TypeMessage.Error);
                        ok = false;
                    }

                    if ((string.IsNullOrEmpty(medicao.CSTCodigo)))
                    {
                        messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "CST"), TypeMessage.Error);
                        ok = false;
                    }

                    if ((string.IsNullOrEmpty(medicao.CodigoContribuicaoCodigo)))
                    {
                        messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Contribuição"), TypeMessage.Error);
                        ok = false;
                    }

                    if (!ok){
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        private void GravarLogOperacaoCancelarLiberacao(Domain.Entity.Contrato.Contrato contrato, string operacao)
        {
            string descricaoOperacao = "";
            string nomeRotina = "";
            if (operacao == "UPDATE")
            {
                descricaoOperacao = "Cancela liberação da medição";
                nomeRotina = "ContratoAppService.CancelarListaItemLiberacao";
            }

            foreach (ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao in contrato.ListaContratoRetificacaoItemMedicao)
            {
                logOperacaoAppService.Gravar(descricaoOperacao,
                                             nomeRotina,
                                             "Contrato.contratoRetificacaoItemMedicao",
                                             operacao,
                                             contratoRetificacaoItemMedicaoAppService.MedicaoToXML(contratoRetificacaoItemMedicao));
            }
        }


        private void GravarLogOperacaoLiberacao(Domain.Entity.Contrato.Contrato contrato, string operacao)
        {
            string descricaoOperacao = "";
            string nomeRotina = "";
            if (operacao == "INSERT")
            {
                descricaoOperacao = "Atualização da liberação da medição";
                nomeRotina = "ContratoAppService.AprovarLiberarListaItemLiberacao";
            }

            foreach (ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao in contrato.ListaContratoRetificacaoItemMedicao)
            {
                logOperacaoAppService.Gravar(descricaoOperacao,
                                             nomeRotina,
                                             "Contrato.contratoRetificacaoItemMedicao",
                                             operacao,
                                             contratoRetificacaoItemMedicaoAppService.MedicaoToXML(contratoRetificacaoItemMedicao));
            }
        }

        private void GravarLogOperacaoMedicao(ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao, string operacao)
        {
            string descricaoOperacao = "";
            string nomeRotina = "";
            if (operacao == "INSERT" || operacao == "UPDATE")
            {
                descricaoOperacao = "Atualização da medição";
                nomeRotina = "ContratoAppService.SalvarMedicao";
                //nomeRotina = "Contrato.contratoRetificacaoItemMedicao_Atualiza";
            }
            if (operacao == "DELETE")
            {
                descricaoOperacao = "Cancelamento da medição";
                nomeRotina = "ContratoAppService.ExcluirMedicao";
                //nomeRotina = "Contrato.contratoRetificacaoItemMedicao_Deleta";
            }

            logOperacaoAppService.Gravar(descricaoOperacao,
                                         nomeRotina,
                                         "Contrato.contratoRetificacaoItemMedicao",
                                         operacao,
                                         contratoRetificacaoItemMedicaoAppService.MedicaoToXML(contratoRetificacaoItemMedicao));
        }

        private bool EhValidoExcluirMedicao(Domain.Entity.Contrato.Contrato contrato,int? contratoRetificacaoItemMedicaoId)
        {
            if (!contratoRetificacaoItemMedicaoId.HasValue)
            {
                messageQueue.Add(Application.Resource.Contrato.ErrorMessages.SelecioneUmaMedicao, TypeMessage.Error);
                return false;
            }

            if (contratoRetificacaoItemMedicaoId.Value == 0)
            {
                messageQueue.Add(Application.Resource.Contrato.ErrorMessages.SelecioneUmaMedicao, TypeMessage.Error);
                return false;
            }

            if (contrato == null)
            {
                messageQueue.Add(Application.Resource.Contrato.ErrorMessages.MedicaoNaoEncontrada, TypeMessage.Error);
                return false;
            }

            if (contrato.ListaContratoRetificacaoItemMedicao == null)
            {
                messageQueue.Add(Application.Resource.Contrato.ErrorMessages.MedicaoNaoEncontrada, TypeMessage.Error);
                return false;
            }

            if (!contrato.ListaContratoRetificacaoItemMedicao.Any(l => l.Id == contratoRetificacaoItemMedicaoId))
            {
                messageQueue.Add(Application.Resource.Contrato.ErrorMessages.MedicaoNaoEncontrada, TypeMessage.Error);
                return false;
            }

            return true;
        }

        private bool PodeSerUmContratoAssinado(SituacaoContrato situacao)
        {
            if (situacao != SituacaoContrato.Assinado)
            {
                return false;
            }

            return true;
        }

        private bool PodeSerUmContratoCancelado(SituacaoContrato situacao)
        {
            if (situacao != SituacaoContrato.Cancelado)
            {
                return false;
            }

            return true;
        }

        private bool PodeSerUmContratoConcluido(SituacaoContrato situacao)
        {
            if (situacao != SituacaoContrato.Concluido)
            {
                return false;
            }

            return true;
        }

        private bool PodeSerUmContratoSuspenso(SituacaoContrato situacao)
        {
            if (situacao != SituacaoContrato.Suspenso)
            {
                return false;
            }

            return true;
        }

        private List<ContratoRetificacaoItemMedicao> RecuperaMedicaoPorContratoEhDadosDaNota(int contratoId,
                                                                                             int tipoDocumentoId,
                                                                                             string numeroDocumento,
                                                                                             DateTime dataEmissao,
                                                                                             int? contratadoId,
                                                                                             params Expression<Func<Domain.Entity.Contrato.Contrato, object>>[] includes)
        {
            List<ContratoRetificacaoItemMedicao> listaMedicao;
            var contrato = contratoRepository.ObterPeloId(contratoId,includes);


            if ((contratadoId.HasValue) && (contratadoId.Value > 0) && (contrato != null))
            {
                listaMedicao =
                contrato.ListaContratoRetificacaoItemMedicao.Where(i => i.TipoDocumentoId == tipoDocumentoId &&
                                                                        i.NumeroDocumento == numeroDocumento &&
                                                                        i.DataEmissao == dataEmissao &&
                                                                        ((i.MultiFornecedorId == contratadoId) ||
                                                                        (i.MultiFornecedorId == null &&
                                                                         i.Contrato.ContratadoId == contratadoId))
                                                                   ).To<List<ContratoRetificacaoItemMedicao>>();
            }
            else
            {
                listaMedicao =
                contrato.ListaContratoRetificacaoItemMedicao.Where(i => i.TipoDocumentoId == tipoDocumentoId &&
                                                                        i.NumeroDocumento == numeroDocumento &&
                                                                        i.DataEmissao == dataEmissao
                                                                   ).To<List<ContratoRetificacaoItemMedicao>>();
            }

            return listaMedicao;
        }

        private List<ContratoRetificacaoItemImposto> RecuperaImpostoMedicaoPorContratoDadosDaNota(int contratoId,
                                                                                                  int tipoDocumentoId,
                                                                                                  string numeroDocumento,
                                                                                                  DateTime dataEmissao,
                                                                                                  int? fornecedorId,
                                                                                                  params Expression<Func<Domain.Entity.Contrato.Contrato, object>>[] includes)
        {
            var contrato = contratoRepository.ObterPeloId(contratoId, includes);
            IEnumerable<ContratoRetificacaoItemImposto> resultJoin;

            if ((fornecedorId.HasValue) && (fornecedorId.Value > 0) && (contrato != null))
            {
                resultJoin =
                            from i in contrato.ListaContratoRetificacaoItemImposto
                            join m in contrato.ListaContratoRetificacaoItemMedicao 
                            on i.ContratoRetificacaoItemId equals m.ContratoRetificacaoItemId 
                            where ((i.ImpostoFinanceiro.EhRetido == true || i.ImpostoFinanceiro.Indireto == true) &&
                                   (m.TipoDocumentoId == tipoDocumentoId &&
                                    m.NumeroDocumento == numeroDocumento &&
                                    m.DataEmissao == dataEmissao &&
                                    ((m.MultiFornecedorId == fornecedorId) ||
                                     (m.MultiFornecedorId == null &&
                                      m.Contrato.ContratadoId == fornecedorId))))
                            select i;

            }
            else
            {
                resultJoin =
                            from i in contrato.ListaContratoRetificacaoItemImposto
                            join m in contrato.ListaContratoRetificacaoItemMedicao
                            on i.ContratoRetificacaoItemId equals m.ContratoRetificacaoItemId
                            where ((i.ImpostoFinanceiro.EhRetido == true || i.ImpostoFinanceiro.Indireto == true) &&
                                   (m.TipoDocumentoId == tipoDocumentoId &&
                                    m.NumeroDocumento == numeroDocumento &&
                                    m.DataEmissao == dataEmissao))
                            select i;
            }

            List<ContratoRetificacaoItemImposto> listaImposto = resultJoin.Distinct().To<List<ContratoRetificacaoItemImposto>>();

            return listaImposto; 

        }

        private DataTable MedicaoToDataTable(List<ContratoRetificacaoItemMedicao> listaMedicao)
        {
            DataTable dta = new DataTable();
            DataColumn codigo = new DataColumn("codigo");
            DataColumn contrato = new DataColumn("contrato");
            DataColumn contratoRetificacao = new DataColumn("contratoRetificacao");
            DataColumn contratoRetificacaoItem = new DataColumn("contratoRetificacaoItem");
            DataColumn sequencialItem = new DataColumn("sequencialItem");
            DataColumn contratoRetificacaoItemCronograma = new DataColumn("contratoRetificacaoItemCronograma");
            DataColumn sequencialCronograma = new DataColumn("sequencialCronograma");
            DataColumn tipoDocumento = new DataColumn("tipoDocumento");
            DataColumn tipoDcumentoDescricao = new DataColumn("tipoDcumentoDescricao");
            DataColumn tipoDocumentoSigla = new DataColumn("tipoDocumentoSigla");
            DataColumn numeroDocumento = new DataColumn("numeroDocumento");
            DataColumn dataVencimento = new DataColumn("dataVencimento", System.Type.GetType("System.DateTime"));
            DataColumn dataEmissao = new DataColumn("dataEmissao", System.Type.GetType("System.DateTime"));
            DataColumn dataMedicao = new DataColumn("dataMedicao", System.Type.GetType("System.DateTime"));
            DataColumn usuarioMedicao = new DataColumn("usuarioMedicao");
            DataColumn valor = new DataColumn("valor", System.Type.GetType("System.Decimal"));
            DataColumn quantidade = new DataColumn("quantidade", System.Type.GetType("System.Decimal"));
            DataColumn valorRetido = new DataColumn("valorRetido", System.Type.GetType("System.Decimal"));
            DataColumn observacao = new DataColumn("observacao");
            DataColumn tituloPagar = new DataColumn("tituloPagar", System.Type.GetType("System.String"));
            DataColumn tituloReceber = new DataColumn("tituloReceber", System.Type.GetType("System.String"));
            DataColumn dataLiberacao = new DataColumn("dataLiberacao", System.Type.GetType("System.DateTime"));
            DataColumn usuarioLiberacao = new DataColumn("usuarioLiberacao");
            DataColumn situacao = new DataColumn("situacao");
            DataColumn multifornecedor = new DataColumn("multifornecedor");
            DataColumn tipoContrato = new DataColumn("tipoContrato", System.Type.GetType("System.Int32"));
            DataColumn contratado = new DataColumn("contratado");
            DataColumn contratante = new DataColumn("contratante");
            DataColumn valorTotalMedidoLiberadoContrato = new DataColumn("valorTotalMedidoLiberadoContrato");
            DataColumn valorContrato = new DataColumn("valorContrato", System.Type.GetType("System.Decimal"));
            DataColumn saldoContrato = new DataColumn("saldoContrato", System.Type.GetType("System.Decimal"));
            DataColumn classe = new DataColumn("classe");
            DataColumn codigoDescricaoClasse = new DataColumn("codigoDescricaoClasse");
            DataColumn precoUnitario = new DataColumn("precoUnitario", System.Type.GetType("System.Decimal"));
            DataColumn descricaoCronograma = new DataColumn("descricaoCronograma");
            DataColumn quantidadeCronograma = new DataColumn("quantidadeCronograma", System.Type.GetType("System.Decimal"));
            DataColumn valorCronograma = new DataColumn("valorCronograma", System.Type.GetType("System.Decimal"));
            DataColumn unidadeMedida = new DataColumn("unidadeMedida");
            DataColumn descricaoItem = new DataColumn("descricaoItem");
            DataColumn complementoDescricaoItem = new DataColumn("complementoDescricaoItem");
            DataColumn descricaoSituacaoMedicao = new DataColumn("descricaoSituacaoMedicao");
            DataColumn naturezaItem = new DataColumn("naturezaItem", System.Type.GetType("System.Int32"));
            DataColumn valorTotalMedido = new DataColumn("valorTotalMedido", System.Type.GetType("System.Decimal"));
            DataColumn quantidadeTotalMedida = new DataColumn("quantidadeTotalMedida", System.Type.GetType("System.Decimal"));
            DataColumn valorTotalLiberado = new DataColumn("valorTotalLiberado", System.Type.GetType("System.Decimal"));
            DataColumn quantidadeTotalLiberada = new DataColumn("quantidadeTotalLiberada", System.Type.GetType("System.Decimal"));
            DataColumn valorTotalMedidoLiberado = new DataColumn("valorTotalMedidoLiberado", System.Type.GetType("System.Decimal"));
            DataColumn quantidadeTotalMedidaLiberada = new DataColumn("quantidadeTotalMedidaLiberada", System.Type.GetType("System.Decimal"));
            DataColumn valorImpostoRetido = new DataColumn("valorImpostoRetido", System.Type.GetType("System.Decimal"));
            DataColumn valorTotalMedidoNota = new DataColumn("valorTotalMedidoNota", System.Type.GetType("System.Decimal"));
            DataColumn valorImpostoIndiretoMedicao = new DataColumn("valorImpostoIndiretoMedicao", System.Type.GetType("System.Decimal"));
            DataColumn valorTotalMedidoIndireto = new DataColumn("valorTotalMedidoIndireto", System.Type.GetType("System.Decimal"));
            DataColumn nomeContratado = new DataColumn("nomeContratado");
            DataColumn nomeContratante = new DataColumn("nomeContratante");
            DataColumn codigoDescricaoCentroCusto = new DataColumn("codigoDescricaoCentroCusto");
            DataColumn centroCusto = new DataColumn("centroCusto");
            DataColumn descricaoContratoDescricao = new DataColumn("descricaoContratoDescricao");
            DataColumn valorItem = new DataColumn("valorItem", System.Type.GetType("System.Decimal"));
            DataColumn valorImpostoRetidoMedicao = new DataColumn("valorImpostoRetidoMedicao", System.Type.GetType("System.Decimal"));
            DataColumn descricaoSituacaoTituloPagar = new DataColumn("descricaoSituacaoTituloPagar");
            DataColumn descricaoSituacaoTituloReceber = new DataColumn("descricaoSituacaoTituloReceber");
            DataColumn CPFCNPJContratado = new DataColumn("CPFCNPJContratado");
            DataColumn CPFCNPJContratante = new DataColumn("CPFCNPJContratante");
            DataColumn CPFCNPJMultifornecedor = new DataColumn("CPFCNPJMultifornecedor");
            DataColumn girErro = new DataColumn("girErro");
            DataColumn desconto = new DataColumn("desconto", System.Type.GetType("System.Decimal"));

            dta.Columns.Add(codigo);
            dta.Columns.Add(contrato);
            dta.Columns.Add(contratoRetificacao);
            dta.Columns.Add(contratoRetificacaoItem);
            dta.Columns.Add(sequencialItem);
            dta.Columns.Add(contratoRetificacaoItemCronograma);
            dta.Columns.Add(sequencialCronograma);
            dta.Columns.Add(tipoDocumento);
            dta.Columns.Add(tipoDcumentoDescricao);
            dta.Columns.Add(tipoDocumentoSigla);
            dta.Columns.Add(numeroDocumento);
            dta.Columns.Add(dataVencimento);
            dta.Columns.Add(dataEmissao);
            dta.Columns.Add(dataMedicao);
            dta.Columns.Add(usuarioMedicao);
            dta.Columns.Add(valor);
            dta.Columns.Add(quantidade);
            dta.Columns.Add(valorRetido);
            dta.Columns.Add(valorTotalMedidoNota);
            dta.Columns.Add(valorImpostoIndiretoMedicao);
            dta.Columns.Add(valorTotalMedidoIndireto);
            dta.Columns.Add(observacao);
            dta.Columns.Add(tituloPagar);
            dta.Columns.Add(tituloReceber);
            dta.Columns.Add(dataLiberacao);
            dta.Columns.Add(usuarioLiberacao);
            dta.Columns.Add(situacao);
            dta.Columns.Add(multifornecedor);
            dta.Columns.Add(tipoContrato);
            dta.Columns.Add(contratado);
            dta.Columns.Add(contratante);
            dta.Columns.Add(valorTotalMedidoLiberadoContrato);
            dta.Columns.Add(valorContrato);
            dta.Columns.Add(saldoContrato);
            dta.Columns.Add(classe);
            dta.Columns.Add(codigoDescricaoClasse);
            dta.Columns.Add(precoUnitario);
            dta.Columns.Add(descricaoCronograma);
            dta.Columns.Add(quantidadeCronograma);
            dta.Columns.Add(valorCronograma);
            dta.Columns.Add(unidadeMedida);
            dta.Columns.Add(descricaoItem);
            dta.Columns.Add(complementoDescricaoItem);
            dta.Columns.Add(descricaoSituacaoMedicao);
            dta.Columns.Add(naturezaItem);
            dta.Columns.Add(valorTotalMedido);
            dta.Columns.Add(quantidadeTotalMedida);
            dta.Columns.Add(valorTotalLiberado);
            dta.Columns.Add(quantidadeTotalLiberada);
            dta.Columns.Add(valorTotalMedidoLiberado);
            dta.Columns.Add(quantidadeTotalMedidaLiberada);
            dta.Columns.Add(valorImpostoRetido);
            dta.Columns.Add(nomeContratado);
            dta.Columns.Add(nomeContratante);
            dta.Columns.Add(codigoDescricaoCentroCusto);
            dta.Columns.Add(centroCusto);
            dta.Columns.Add(descricaoContratoDescricao);
            dta.Columns.Add(valorItem);
            dta.Columns.Add(valorImpostoRetidoMedicao);
            dta.Columns.Add(descricaoSituacaoTituloPagar);
            dta.Columns.Add(descricaoSituacaoTituloReceber);
            dta.Columns.Add(CPFCNPJContratado);
            dta.Columns.Add(CPFCNPJContratante);
            dta.Columns.Add(CPFCNPJMultifornecedor);
            dta.Columns.Add(girErro);
            dta.Columns.Add(desconto);

            foreach (var medicao in listaMedicao)
            {
                DataRow row = dta.NewRow();

                row[codigo] = medicao.Id;
                row[contrato] = medicao.ContratoId;
                row[contratoRetificacao] = medicao.ContratoRetificacaoId;
                row[contratoRetificacaoItem] = medicao.ContratoRetificacaoItemId;
                row[sequencialItem] = medicao.SequencialItem;
                row[contratoRetificacaoItemCronograma] = medicao.ContratoRetificacaoItemCronogramaId;
                row[sequencialCronograma] = medicao.SequencialCronograma;
                row[tipoDocumento] = medicao.TipoDocumentoId;
                row[tipoDcumentoDescricao] = medicao.TipoDocumento.Descricao;
                row[tipoDocumentoSigla] = medicao.TipoDocumento.Sigla;
                row[numeroDocumento] = medicao.NumeroDocumento;
                row[dataVencimento] = medicao.DataVencimento.ToString("dd/MM/yyyy");
                row[dataEmissao] = medicao.DataEmissao.ToString("dd/MM/yyyy");
                row[dataMedicao] = medicao.DataMedicao.ToString("dd/MM/yyyy");
                row[usuarioMedicao] = medicao.UsuarioMedicao;
                row[valor] = medicao.Valor;
                row[quantidade] = medicao.Quantidade;
                if (medicao.ValorRetido.HasValue)
                {
                    row[valorRetido] = medicao.ValorRetido.Value;
                }
                else
                {
                    row[valorRetido] = 0M;
                }
                row[valorTotalMedidoNota] = medicao.Contrato.ObterValorTotalMedidoNota(medicao.ContratoId,medicao.NumeroDocumento,medicao.TipoDocumentoId,medicao.DataVencimento);
                row[valorImpostoIndiretoMedicao] = medicao.Contrato.ObterValorTotalImpostoIndiretoMedicao(medicao.SequencialItem,medicao.SequencialCronograma,medicao.ContratoRetificacaoItemId,medicao.Id);
                row[valorTotalMedidoIndireto] = medicao.Contrato.ObterValorTotalMedidoIndireto(medicao.ContratoId, medicao.NumeroDocumento, medicao.TipoDocumentoId, medicao.DataVencimento);
                row[observacao] = medicao.Observacao;
                if (medicao.TituloPagarId.HasValue)
                {
                    row[tituloPagar] = medicao.TituloPagarId.Value;
                }
                else
                {
                    row[tituloPagar] = DBNull.Value;
                }

                if (medicao.TituloReceberId.HasValue)
                {
                    row[tituloReceber] = medicao.TituloReceberId.Value.ToString();
                }
                else
                {
                    row[tituloReceber] = DBNull.Value;
                }

                if (medicao.DataLiberacao.HasValue)
                {
                    row[dataLiberacao] = medicao.DataLiberacao.Value.ToString("dd/MM/yyyy");
                }
                else
                {
                    row[dataLiberacao] = DBNull.Value;
                }
                row[usuarioLiberacao] = medicao.UsuarioLiberacao;
                row[situacao] = medicao.Situacao;
                row[multifornecedor] = medicao.MultiFornecedorId;
                row[tipoContrato] = medicao.Contrato.TipoContrato;
                row[contratado] = medicao.Contrato.ContratadoId;
                row[contratante] = medicao.Contrato.ContratanteId;
                decimal valorTotalMedidoLiberadoContratoAux = medicao.Contrato.ObterValorTotalMedidoLiberadoContrato(medicao.ContratoId);
                row[valorTotalMedidoLiberadoContrato] = valorTotalMedidoLiberadoContratoAux;
                decimal valorContratoAux = medicao.Contrato.ValorContrato.HasValue ? medicao.Contrato.ValorContrato.Value : 0;
                row[valorContrato] = valorContratoAux;               
                row[saldoContrato] = (valorContratoAux - valorTotalMedidoLiberadoContratoAux);
                row[classe] = medicao.ContratoRetificacaoItem.CodigoClasse;
                string codigoDescricaoClasseAux = medicao.ContratoRetificacaoItem.CodigoClasse + " - " + medicao.ContratoRetificacaoItem.Classe.Descricao;
                row[codigoDescricaoClasse] = codigoDescricaoClasseAux;
                row[precoUnitario] = medicao.ContratoRetificacaoItem.PrecoUnitario;
                row[descricaoCronograma] = medicao.ContratoRetificacaoItemCronograma.Descricao;
                row[quantidadeCronograma] = medicao.ContratoRetificacaoItemCronograma.Quantidade;
                row[valorCronograma] = medicao.ContratoRetificacaoItemCronograma.Valor;
                row[unidadeMedida] = medicao.ContratoRetificacaoItem.Servico.SiglaUnidadeMedida;
                row[descricaoItem] = medicao.ContratoRetificacaoItem.Servico.Descricao;
                row[complementoDescricaoItem] = medicao.ContratoRetificacaoItem.ComplementoDescricao;
                row[descricaoSituacaoMedicao] = medicao.Situacao.ObterDescricao(); ;
                row[naturezaItem] = (int)medicao.ContratoRetificacaoItem.NaturezaItem; 
                row[valorTotalMedido] = medicao.Contrato.ObterValorTotalMedido(medicao.SequencialItem,medicao.SequencialCronograma);
                row[quantidadeTotalMedida] = medicao.Contrato.ObterQuantidadeTotalMedida(medicao.SequencialItem,medicao.SequencialCronograma);
                row[valorTotalLiberado] = medicao.Contrato.ObterValorTotalLiberado(medicao.SequencialItem,medicao.SequencialCronograma);
                row[quantidadeTotalLiberada] = medicao.Contrato.ObterQuantidadeTotalLiberada(medicao.SequencialItem,medicao.SequencialCronograma);
                row[valorTotalMedidoLiberado] = medicao.Contrato.ObterValorTotalMedidoLiberado(medicao.SequencialItem,medicao.SequencialCronograma);
                row[quantidadeTotalMedidaLiberada] = medicao.Contrato.ObterQuantidadeTotalMedidaLiberada(medicao.SequencialItem,medicao.SequencialCronograma);
                row[valorImpostoRetido] = medicao.Contrato.ObterValorImpostoRetido(medicao.SequencialItem,medicao.SequencialCronograma,medicao.ContratoRetificacaoItemId);
                row[nomeContratado] = medicao.Contrato.Contratado.Nome;
                row[nomeContratante] = medicao.Contrato.Contratante.Nome;
                string codigoDescricaoCentroCustoAux = medicao.Contrato.CodigoCentroCusto + " - " + medicao.Contrato.CentroCusto.Descricao;
                row[codigoDescricaoCentroCusto] = codigoDescricaoCentroCustoAux;
                row[centroCusto] = medicao.Contrato.CodigoCentroCusto;
                row[descricaoContratoDescricao] = medicao.Contrato.ContratoDescricao;
                row[valorItem] = medicao.ContratoRetificacaoItem.ValorItem.HasValue ? medicao.ContratoRetificacaoItem.ValorItem.Value : 0;
                row[valorImpostoRetidoMedicao] = medicao.Contrato.ObterValorImpostoRetidoMedicao(medicao.SequencialItem, medicao.SequencialCronograma, medicao.ContratoRetificacaoItemId, medicao.Id);
                if (medicao.TituloPagarId.HasValue)
                {
                    row[descricaoSituacaoTituloPagar] = medicao.TituloPagar.Situacao.ObterDescricao();
                }
                else
                {
                    row[descricaoSituacaoTituloPagar] = "";
                }
                if (medicao.TituloReceberId.HasValue)
                {
                    row[descricaoSituacaoTituloReceber] = medicao.TituloReceber.Situacao.ObterDescricao();
                }
                else
                {
                    row[descricaoSituacaoTituloReceber] = "";
                }
                row[CPFCNPJContratado] = "";
                if (medicao.Contrato.Contratado.TipoPessoa == "F")
                {
                    if (medicao.Contrato.Contratado.PessoaFisica != null)
                    {
                        row[CPFCNPJContratado] = medicao.Contrato.Contratado.PessoaFisica.Cpf;
                    }
                }
                else if (medicao.Contrato.Contratado.TipoPessoa == "J")
                {
                    if (medicao.Contrato.Contratado.PessoaJuridica != null)
                    {
                        row[CPFCNPJContratado] = medicao.Contrato.Contratado.PessoaJuridica.Cnpj;
                    }
                }
                row[CPFCNPJContratante] = "";
                if (medicao.Contrato.Contratante.TipoPessoa == "F")
                {
                    if (medicao.Contrato.Contratante.PessoaFisica != null)
                    {
                        row[CPFCNPJContratante] = medicao.Contrato.Contratante.PessoaFisica.Cpf;
                    }
                }
                else if (medicao.Contrato.Contratante.TipoPessoa == "J")
                {
                    if (medicao.Contrato.Contratante.PessoaJuridica != null)
                    {
                        row[CPFCNPJContratante] = medicao.Contrato.Contratante.PessoaJuridica.Cnpj;
                    }
                }
                row[CPFCNPJMultifornecedor] = "";
                if (medicao.MultiFornecedor != null)
                {
                    if (medicao.MultiFornecedor.TipoPessoa == "F")
                    {
                        if (medicao.MultiFornecedor.PessoaFisica != null)
                        {
                            row[CPFCNPJMultifornecedor] = medicao.MultiFornecedor.PessoaFisica.Cpf;
                        }
                    }
                    else if (medicao.MultiFornecedor.TipoPessoa == "J")
                    {
                        if (medicao.MultiFornecedor.PessoaJuridica != null)
                        {
                            row[CPFCNPJMultifornecedor] = medicao.MultiFornecedor.PessoaJuridica.Cnpj;
                        }
                    }
                }
                row[girErro] = "";
                row[desconto] = medicao.Desconto.HasValue ? medicao.Desconto.Value : 0;
                dta.Rows.Add(row);
            }

            return dta;
        }

        private DataTable ImpostoToDataTable(List<ContratoRetificacaoItemImposto> listaImposto,
                                             List<ContratoRetificacaoItemMedicao> listaMedicao)
        {
            DataTable dta = new DataTable();
            DataColumn codigo = new DataColumn("codigo");
            DataColumn contrato = new DataColumn("contrato");
            DataColumn contratoRetificacao = new DataColumn("contratoRetificacao");
            DataColumn contratoRetificacaoItem = new DataColumn("contratoRetificacaoItem");
            DataColumn impostoFinanceiro = new DataColumn("impostoFinanceiro", System.Type.GetType("System.Int32"));
            DataColumn percentualBaseCalculo = new DataColumn("percentualBaseCalculo", System.Type.GetType("System.Decimal"));
            DataColumn aliquota = new DataColumn("aliquota", System.Type.GetType("System.Decimal"));
            DataColumn retido = new DataColumn("retido", System.Type.GetType("System.Boolean"));
            DataColumn descricaoImposto = new DataColumn("descricaoImposto");
            DataColumn valorTotalMedido = new DataColumn("valorTotalMedido", System.Type.GetType("System.Decimal"));
            DataColumn sigla = new DataColumn("sigla");
            DataColumn sequencialItem = new DataColumn("sequencialItem");
            DataColumn descricaoItem = new DataColumn("descricaoItem");
            DataColumn valorImposto = new DataColumn("valorImposto", System.Type.GetType("System.Decimal"));
            DataColumn valorTotalMedidoIndireto = new DataColumn("valorTotalMedidoIndireto", System.Type.GetType("System.Decimal"));
            DataColumn indireto = new DataColumn("indireto", System.Type.GetType("System.Boolean"));
            DataColumn girErro = new DataColumn("girErro");

            dta.Columns.Add(codigo);
            dta.Columns.Add(contrato);
            dta.Columns.Add(contratoRetificacao);
            dta.Columns.Add(contratoRetificacaoItem);
            dta.Columns.Add(impostoFinanceiro);
            dta.Columns.Add(percentualBaseCalculo);
            dta.Columns.Add(aliquota);
            dta.Columns.Add(retido);
            dta.Columns.Add(descricaoImposto);
            dta.Columns.Add(valorTotalMedido);
            dta.Columns.Add(sigla);
            dta.Columns.Add(sequencialItem);
            dta.Columns.Add(descricaoItem);
            dta.Columns.Add(valorImposto);
            dta.Columns.Add(valorTotalMedidoIndireto);
            dta.Columns.Add(indireto);
            dta.Columns.Add(girErro);

            var resultJoin = from i in listaImposto
                             join m in listaMedicao
                             on i.ContratoRetificacaoItemId equals m.ContratoRetificacaoItemId
                             select new
                             {
                                 Imposto = i,
                                 Medicao = m,
                                 ValorImposto = (m.Valor * (i.PercentualBaseCalculo / 100) * (i.ImpostoFinanceiro.Aliquota / 100))
                             };


            foreach (var item in resultJoin)
            {
                DataRow row = dta.NewRow();

                row[codigo] = item.Imposto.Id;
                row[contrato] = item.Imposto.ContratoId;
                row[contratoRetificacao] = item.Imposto.ContratoRetificacaoId;
                row[contratoRetificacaoItem] = item.Imposto.ContratoRetificacaoItemId;
                row[impostoFinanceiro] = item.Imposto.ImpostoFinanceiroId;
                row[percentualBaseCalculo] = item.Imposto.PercentualBaseCalculo;
                row[aliquota] = item.Imposto.ImpostoFinanceiro.Aliquota;
                row[retido] = item.Imposto.ImpostoFinanceiro.EhRetido;
                row[descricaoImposto] = item.Imposto.ImpostoFinanceiro.Descricao;
                //Esse campo está no rpt mais vem da procedure Contrato.contratoRetificacaoItemImposto_RecuperaPorContratoDadosNota do desktop
                //row[valorTotalMedido] = null;
                //Esse campo está no rpt mais não vem da procedure Contrato.contratoRetificacaoItemImposto_RecuperaPorContratoDadosNota do desktop
                row[sigla] = item.Imposto.ImpostoFinanceiro.Sigla;
                //Esse campo está no rpt mais não vem da procedure Contrato.contratoRetificacaoItemImposto_RecuperaPorContratoDadosNota do desktop
                row[sequencialItem] = item.Imposto.ContratoRetificacaoItem.Sequencial;
                row[descricaoItem] = item.Imposto.ContratoRetificacaoItem.Servico.Descricao;
                //Esse campo está no rpt mais não vem da procedure Contrato.contratoRetificacaoItemImposto_RecuperaPorContratoDadosNota do desktop
                row[valorImposto] = Math.Round(item.ValorImposto,2);
                row[valorTotalMedidoIndireto] = item.Medicao.Contrato.ObterValorTotalMedidoIndireto(item.Medicao.ContratoId,
                                                                                                    item.Medicao.NumeroDocumento,
                                                                                                    item.Medicao.TipoDocumentoId,
                                                                                                    item.Medicao.DataVencimento);
                row[indireto] = false;
                if (item.Imposto.ImpostoFinanceiro.Indireto.HasValue)
                {
                    row[indireto] = item.Imposto.ImpostoFinanceiro.Indireto.Value;
                }
                //row[girErro] = "";

                dta.Rows.Add(row);

            }

            return dta;
        }

        private string RecuperaDocumentoAntecipacao(ContratoDTO contrato, int? sequencialCronograma, int? sequencialItem, int? contratoRetificacaoId)
        {
            string documentoAntecipacao = "";
            if (contrato.ListaContratoRetificacaoItemMedicao.Count > 0 && sequencialCronograma.HasValue && sequencialItem.HasValue && contratoRetificacaoId.HasValue)
            {
                List<ContratoRetificacaoItemMedicaoDTO> listaMedicao =
                    contrato.ListaContratoRetificacaoItemMedicao.Where(l => l.SequencialCronograma == sequencialCronograma.Value && l.SequencialItem == sequencialItem.Value && l.ContratoRetificacaoId <= contratoRetificacaoId.Value).ToList();
                foreach (var medicao in listaMedicao)
                {
                    documentoAntecipacao = documentoAntecipacao + medicao.NumeroDocumento + "/";
                }
                if (documentoAntecipacao.Length > 0)
                {
                    documentoAntecipacao = documentoAntecipacao.Substring(0, documentoAntecipacao.Length - 1);
                }
            }
            return documentoAntecipacao;
        }

        private bool ValidacaoAssociacaoNotaFiscalListaItemLiberacao(int? contratoRetificacaoItemMedicaoId,int? tipoDocumentoId,string numeroDocumento,Nullable<DateTime> dataEmissao,Nullable<DateTime> dataVencimento){
            bool retorno = false;

            if (!contratoRetificacaoItemMedicaoId.HasValue)
            {
                messageQueue.Add("Nenhum item foi selecionado.", TypeMessage.Error);
                return retorno;
            }

            if (!tipoDocumentoId.HasValue)
            {
                messageQueue.Add("Informe o campo tipo da nota.", TypeMessage.Error);
                return retorno;
            }

            if (string.IsNullOrEmpty(numeroDocumento))
            {
                messageQueue.Add("Informe o campo número do documento.", TypeMessage.Error);
                return retorno;
            }

            if (!dataEmissao.HasValue)
            {
                messageQueue.Add("Informe o campo data de emissão.", TypeMessage.Error);
                return retorno;
            }

            if (!dataVencimento.HasValue)
            {
                messageQueue.Add("Informe o campo data de vencimento.", TypeMessage.Error);
                return retorno;
            }

            retorno = true;
            return retorno;
        }

        #endregion

    }

}
