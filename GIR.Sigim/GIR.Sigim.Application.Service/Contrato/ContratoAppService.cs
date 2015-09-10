﻿using System;
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
        private ITituloPagarAppService tituloPagarAppService;
        private IBloqueioContabilAppService bloqueioContabilAppService;

        #endregion

        #region Construtor

        public ContratoAppService(IContratoRepository contratoRepository, 
                                  IUsuarioAppService usuarioAppService, 
                                  ILogOperacaoAppService logOperacaoAppService,
                                  IContratoRetificacaoItemMedicaoAppService contratoRetificacaoItemMedicaoAppService,
                                  ITituloPagarAppService tituloPagarAppService,
                                  IBloqueioContabilAppService bloqueioContabilAppService,
                                  IParametrosContratoRepository parametrosContratoRepository,
                                  MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.contratoRepository = contratoRepository;
            this.usuarioAppService = usuarioAppService;
            this.logOperacaoAppService = logOperacaoAppService;
            this.contratoRetificacaoItemMedicaoAppService = contratoRetificacaoItemMedicaoAppService;
            this.tituloPagarAppService = tituloPagarAppService;
            this.bloqueioContabilAppService = bloqueioContabilAppService;
            this.parametrosContratoRepository = parametrosContratoRepository;
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
                                               FormatoExportacaoArquivo formato)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.MedicaoImprimir))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return null;
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
                GravarLogOperacao(contratoRetificacaoItemMedicao, "DELETE");

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
                    GravarLogOperacao(contratoRetificacaoItemMedicao, novoRegistro ? "INSERT" : "UPDATE");

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
                    itemLiberacao.DataVencimento = provisao.ContratoRetificacaoItemCronograma.DataVencimento;

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
                    itemLiberacao.DescricaoEvento = provisao.ContratoRetificacaoItemCronograma.Descricao;
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
                    itemLiberacao.DescricaoEvento = medicao.ContratoRetificacaoItemCronograma.Descricao;

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
            bool retorno = false;
            if (listaItemLiberacaoDTO == null)
            {
                messageQueue.Add("Nenhuma liberação foi selecionada", TypeMessage.Error);
                return false;
            }

            if (listaItemLiberacaoDTO.All(l => l.Selecionado == false))
            {
                messageQueue.Add("Nenhuma liberação foi selecionada", TypeMessage.Error);
                return retorno;
            }

            if (listaItemLiberacaoDTO.Any(l => l.Selecionado == true && l.CodigoSituacao != (int)SituacaoMedicao.AguardandoAprovacao)){
                messageQueue.Add("Existe(m) medição(ões) com situacao diferente de Aguardando aprovação !", TypeMessage.Info);
                return retorno;
            }

            var specification = (Specification<Domain.Entity.Contrato.Contrato>)new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            Domain.Entity.Contrato.Contrato contrato = contratoRepository.ObterPeloId(contratoId, specification,l => l.ListaContratoRetificacaoItemMedicao);

            bool modificou = false;
            foreach (ItemLiberacaoDTO item in listaItemLiberacaoDTO.Where(l => l.Selecionado == true && l.CodigoSituacao == (int)SituacaoMedicao.AguardandoAprovacao))
            {
                ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao = contrato.ListaContratoRetificacaoItemMedicao.Where(l => l.Id == item.ContratoRetificacaoItemMedicaoId).FirstOrDefault();
                if (contratoRetificacaoItemMedicao.Situacao == SituacaoMedicao.AguardandoAprovacao)
                {
                    modificou = true;
                    contratoRetificacaoItemMedicao.Situacao = SituacaoMedicao.AguardandoLiberacao;
                }
            }

            if (!modificou)
            {
                messageQueue.Add("A(s) medição(ões) escolhidas estão desatualizadas recupere o contrato novamente !", TypeMessage.Info);
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
                                                      formato);

            return arquivo;
        }

        #endregion

        #region Métodos Privados

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

        private void GravarLogOperacao(ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao, string operacao)
        {
            string descricaoOperacao = "";
            string nomeRotina = "";
            if (operacao == "INSERT" || operacao == "UPDATE")
            {
                descricaoOperacao = "Atualização da medição";
                nomeRotina = "Contrato.contratoRetificacaoItemMedicao_Atualiza";
            }
            if (operacao == "DELETE")
            {
                descricaoOperacao = "Cancelamento da medição";
                nomeRotina = "Contrato.contratoRetificacaoItemMedicao_Deleta";
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

        #endregion
    }
}
