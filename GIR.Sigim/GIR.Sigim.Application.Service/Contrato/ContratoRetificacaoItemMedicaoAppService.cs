using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Domain.Repository.Contrato;
using System.Threading.Tasks;
using CrystalDecisions.Shared;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Application.DTO.Contrato;
using GIR.Sigim.Application.Service.Financeiro;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Reports.Contrato;
using GIR.Sigim.Application.Service.Admin;

namespace GIR.Sigim.Application.Service.Contrato
{
    public class ContratoRetificacaoItemMedicaoAppService : BaseAppService, IContratoRetificacaoItemMedicaoAppService
    {
        #region Declaração

        private IContratoRetificacaoItemMedicaoRepository contratoRetificacaoItemMedicaoRepository;
        private ITituloPagarAppService tituloPagarAppService;
        private IParametrosContratoAppService parametrosContratoAppService;
        private IBloqueioContabilAppService bloqueioContabilAppService;
        private ILogOperacaoAppService logOperacaoAppService;
        private IContratoRetificacaoItemImpostoAppService contratoRetificacaoItemImpostoAppService;
        private IUsuarioAppService usuarioAppService;

        #endregion

        #region Construtor

        public ContratoRetificacaoItemMedicaoAppService(IContratoRetificacaoItemMedicaoRepository contratoRetificacaoItemMedicaoRepository,
                                                        ITituloPagarAppService tituloPagarAppService,
                                                        IParametrosContratoAppService parametrosContratoAppService,
                                                        IBloqueioContabilAppService bloqueioContabilAppService,
                                                        ILogOperacaoAppService logOperacaoAppService,
                                                        IContratoRetificacaoItemImpostoAppService contratoRetificacaoItemImpostoAppService,
                                                        IUsuarioAppService usuarioAppService,
                                                        MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.contratoRetificacaoItemMedicaoRepository = contratoRetificacaoItemMedicaoRepository;
            this.tituloPagarAppService = tituloPagarAppService;
            this.parametrosContratoAppService = parametrosContratoAppService;
            this.bloqueioContabilAppService = bloqueioContabilAppService;
            this.logOperacaoAppService = logOperacaoAppService;
            this.contratoRetificacaoItemImpostoAppService = contratoRetificacaoItemImpostoAppService;
            this.usuarioAppService = usuarioAppService;
        }

        #endregion

        #region Métodos IContratoRetificacaoItemMedicaoAppService

        public bool ExisteNumeroDocumento(Nullable<DateTime> dataEmissao, string numeroDocumento, int? contratadoId)
        {
            bool existe = false;

            if (!string.IsNullOrEmpty(numeroDocumento) && (contratadoId.HasValue))
            {
                List<ContratoRetificacaoItemMedicao> listaContratoRetificacaoItemMedicao;
                string numeroNotaFiscal = RetiraZerosIniciaisNumeroDocumento(numeroDocumento);

                listaContratoRetificacaoItemMedicao =
                contratoRetificacaoItemMedicaoRepository.ListarPeloFiltro((l =>
                                                                                l.NumeroDocumento.EndsWith(numeroNotaFiscal) &&
                                                                                (
                                                                                    (dataEmissao == null) ||
                                                                                    ((dataEmissao != null) && (l.DataEmissao.Year == dataEmissao.Value.Year))
                                                                                ) &&
                                                                                (
                                                                                    (l.MultiFornecedorId == contratadoId) ||
                                                                                    (l.MultiFornecedorId == null && l.Contrato.ContratadoId == contratadoId)
                                                                                )
                                                                            ),
                                                                            l => l.Contrato).ToList<ContratoRetificacaoItemMedicao>();
                if (listaContratoRetificacaoItemMedicao.Count() > 0)
                {
                    string numeroDeZerosIniciais;

                    foreach (var item in listaContratoRetificacaoItemMedicao)
                    {
                        var quantidadeDeZerosIniciais = item.NumeroDocumento.Length - numeroNotaFiscal.Length;
                        numeroDeZerosIniciais = item.NumeroDocumento.Substring(0, quantidadeDeZerosIniciais);
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

            return existe;
        }

        public bool Salvar(ContratoRetificacaoItemMedicaoDTO dto)
        {
            if ((dto == null) )
            {
                throw new ArgumentNullException("dto");
            }

            bool novoRegistro = false;

            ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao = contratoRetificacaoItemMedicaoRepository.ObterPeloId(dto.Id);

            if (contratoRetificacaoItemMedicao == null)
            {
                novoRegistro = true;
                contratoRetificacaoItemMedicao = new ContratoRetificacaoItemMedicao();
                contratoRetificacaoItemMedicao.DataCadastro = DateTime.Now;
                contratoRetificacaoItemMedicao.UsuarioMedicao = UsuarioLogado.Login;
                contratoRetificacaoItemMedicao.Situacao = SituacaoMedicao.AguardandoAprovacao;
            }

            contratoRetificacaoItemMedicao.ContratoId = dto.ContratoId;
 
            if (!EhValidoSalvar(dto))
            {
                return false;
            }

            PopularEntity(dto, contratoRetificacaoItemMedicao);

            if (Validator.IsValid(contratoRetificacaoItemMedicao, out validationErrors))
            {
                try
                {
                    if (novoRegistro)
                    {
                        contratoRetificacaoItemMedicaoRepository.Inserir(contratoRetificacaoItemMedicao);
                    }
                    else
                    {
                        contratoRetificacaoItemMedicaoRepository.Alterar(contratoRetificacaoItemMedicao);
                    }

                    contratoRetificacaoItemMedicaoRepository.UnitOfWork.Commit();

                    GravarLogOperacao(contratoRetificacaoItemMedicao, novoRegistro ? "INSERT" : "UPDATE");

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

        public bool Cancelar(int? contratoRetificacaoItemMedicaoId)
        {
            if (!EhValidoCancelar(contratoRetificacaoItemMedicaoId))
            {
                return false;
            }

            var contratoRetificacaoItemMedicao = contratoRetificacaoItemMedicaoRepository.ObterPeloId(contratoRetificacaoItemMedicaoId);

            try
            {

                contratoRetificacaoItemMedicaoRepository.Remover(contratoRetificacaoItemMedicao);
                contratoRetificacaoItemMedicaoRepository.UnitOfWork.Commit();

                GravarLogOperacao(contratoRetificacaoItemMedicao, "DELETE");

                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.GravacaoErro, TypeMessage.Error);
                return false;
            }
        }

        public FileDownloadDTO Exportar(int? contratadoId,
                                        int contratoId,
                                        int tipoDocumentoId,
                                        string numeroDocumento,
                                        DateTime dataEmissao,
                                        string retencaoContratual,
                                        string valorContratadoItem,
                                        FormatoExportacaoArquivo formato)
        {
            var listaMedicao = contratoRetificacaoItemMedicaoRepository.RecuperaMedicaoPorContratoDadosDaNota(contratoId, 
                                                                                                              tipoDocumentoId, 
                                                                                                              numeroDocumento, 
                                                                                                              dataEmissao, 
                                                                                                              contratadoId,
                                                                                                              l => l.Contrato.CentroCusto,
                                                                                                              l => l.Contrato.ContratoDescricao,
                                                                                                              l => l.Contrato.Contratado.PessoaFisica,
                                                                                                              l => l.Contrato.Contratado.PessoaJuridica,
                                                                                                              l => l.Contrato.Contratante.PessoaFisica,
                                                                                                              l => l.Contrato.Contratante.PessoaJuridica,
                                                                                                              l => l.MultiFornecedor.PessoaFisica,
                                                                                                              l => l.MultiFornecedor.PessoaJuridica,
                                                                                                              l => l.TipoDocumento,
                                                                                                              l => l.TituloPagar,
                                                                                                              l => l.TituloReceber,
                                                                                                              l => l.ContratoRetificacaoItem.Servico,
                                                                                                              l => l.ContratoRetificacaoItem.Classe,
                                                                                                              l => l.ContratoRetificacaoItemCronograma
                                                                                                              ).To<List<ContratoRetificacaoItemMedicao>>();
            relMedicao objRel = new relMedicao();

            //var listaImposto = contratoRetificacaoItemImpostoAppService.RecuperaImpostoPorContratoDadosDaNota(contratoId,
            //                                                                                                  tipoDocumentoId,
            //                                                                                                  numeroDocumento,
            //                                                                                                  dataEmissao,
            //                                                                                                  contratadoId,
            //                                                                                                  l => l.ImpostoFinanceiro,
            //                                                                                                  l => l.ContratoRetificacaoItem,
            //                                                                                                  l => l.ContratoRetificacaoItem.ListaContratoRetificacaoItemMedicao
            //                                                                                                  ).To<List<ContratoRetificacaoItemImposto>>();
            List<ContratoRetificacaoItemImposto> listaImposto = new List<ContratoRetificacaoItemImposto>();

            objRel.SetDataSource(MedicaoToDataTable(listaMedicao));
            objRel.Subreports["contratoImposto"].Database.Tables["Contrato_contratoImpostoMedidoRelatorio"].SetDataSource(ImpostoToDataTable(listaImposto));
            //objRel.Subreports["contratoImposto"].SetDataSource(ImpostoToDataTable(listaImposto));

            var parametros = parametrosContratoAppService.Obter();

            var caminhoImagem = DiretorioImagemRelatorio + Guid.NewGuid().ToString() + ".bmp";
            System.Drawing.Image imagem = parametros.IconeRelatorio.ToImage();
            imagem.Save(caminhoImagem, System.Drawing.Imaging.ImageFormat.Bmp);

            objRel.SetParameterValue("nomeEmpresa", parametros.Cliente.Nome);
            objRel.SetParameterValue("parCentroCusto", listaMedicao.ElementAt(0).Contrato.CentroCusto.Codigo);            
            objRel.SetParameterValue("parDescricaoCentroCusto", listaMedicao.ElementAt(0).Contrato.CentroCusto.Descricao);
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
            objRel.SetParameterValue("parRetencao", retencaoContratual);
            objRel.SetParameterValue("parValorContratadoItem", valorContratadoItem);
            objRel.SetParameterValue("parValorTotalImposto", 0M);

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


        #endregion

        #region Métodos privados

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
            DataColumn valor = new DataColumn("valor",System.Type.GetType("System.Decimal"));
            DataColumn quantidade = new DataColumn("quantidade",System.Type.GetType("System.Decimal"));
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
                //row[dataLiberacao] = medicao.DataLiberacao.HasValue ? medicao.DataLiberacao.Value.ToString("dd/MM/yyyy") : DBNull.Value;
                row[usuarioLiberacao] = medicao.UsuarioLiberacao;
                row[situacao] = medicao.Situacao;
                row[multifornecedor] = medicao.MultiFornecedorId;
                row[tipoContrato] = medicao.Contrato.TipoContrato;
                row[contratado] = medicao.Contrato.ContratadoId;
                row[contratante] = medicao.Contrato.ContratanteId;
                //Esse valor é um campo customizado na procedure viw_contrato , ver como fazer com o wilson 
                row[valorTotalMedidoLiberadoContrato] = -999999999999;
                //Esse valor é um campo customizado na procedure viw_contrato , ver como fazer com o wilson 
                decimal valorContratoAux = medicao.Contrato.ValorContrato.HasValue ? medicao.Contrato.ValorContrato.Value : 0;
                row[valorContrato] = valorContratoAux;
                row[saldoContrato] = (valorContratoAux - (-999999999999));
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
                row[naturezaItem] = (int)medicao.ContratoRetificacaoItem.NaturezaItem; //medicao.ContratoRetificacaoItem.NaturezaItem.ObterDescricao();
                //Esse valor é um campo customizado na procedure viw_contrato , ver como fazer com o wilson 
                row[valorTotalMedido] = -999999999999;
                row[quantidadeTotalMedida] = -999999999999;
                row[valorTotalLiberado] = -999999999999;
                row[quantidadeTotalLiberada] = -999999999999;
                row[valorTotalMedidoLiberado] = -999999999999;
                row[quantidadeTotalMedidaLiberada] = -999999999999;
                row[valorImpostoRetido] = - 999999999999;
                //Esse valor é um campo customizado na procedure viw_contrato , ver como fazer com o wilson 
                row[nomeContratado] = medicao.Contrato.Contratado.Nome;
                row[nomeContratante] = medicao.Contrato.Contratante.Nome;
                string codigoDescricaoCentroCustoAux = medicao.Contrato.CodigoCentroCusto + " - " + medicao.Contrato.CentroCusto.Descricao;
                row[codigoDescricaoCentroCusto] = codigoDescricaoCentroCustoAux;
                row[centroCusto] = medicao.Contrato.CodigoCentroCusto;
                row[descricaoContratoDescricao] = medicao.Contrato.ContratoDescricao;
                row[valorItem] = medicao.ContratoRetificacaoItem.ValorItem.HasValue ? medicao.ContratoRetificacaoItem.ValorItem.Value : 0;
                //Esse valor é um campo customizado na procedure viw_contrato , ver como fazer com o wilson 
                row[valorImpostoRetidoMedicao] = -999999999999;
                //Esse valor é um campo customizado na procedure viw_contrato , ver como fazer com o wilson 
                if (medicao.TituloPagarId.HasValue)
                {
                    row[descricaoSituacaoTituloPagar] = medicao.TituloPagar.Situacao.ObterDescricao();
                }
                else
                {
                    row[descricaoSituacaoTituloPagar] = "";
                }
                if (medicao.TituloReceberId.HasValue) {
                    row[descricaoSituacaoTituloReceber] = medicao.TituloReceber.Situacao.ObterDescricao();
                }
                else{
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

        private DataTable ImpostoToDataTable(List<ContratoRetificacaoItemImposto> listaImposto)
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
            dta.Columns.Add(indireto);
            dta.Columns.Add(girErro);

            //if (listaImposto.Count()==0)
            //{
            //    DataRow row = dta.NewRow();

            //    //row[codigo] = 0;
            //    //row[contrato] = 0;
            //    //row[contratoRetificacao] = 0;
            //    //row[contratoRetificacaoItem] = 0;
            //    row[impostoFinanceiro] = 0;
            //    //row[percentualBaseCalculo] = 0;
            //    row[aliquota] = 0;
            //    //row[retido] = false;
            //    //row[descricaoImposto] = "";
            //    //Esse campo está no rpt mais vem da procedure Contrato.contratoRetificacaoItemImposto_RecuperaPorContratoDadosNota do desktop
            //    //row[valorTotalMedido] = null;
            //    //Esse campo está no rpt mais não vem da procedure Contrato.contratoRetificacaoItemImposto_RecuperaPorContratoDadosNota do desktop
            //    row[sigla] = "";
            //    //Esse campo está no rpt mais não vem da procedure Contrato.contratoRetificacaoItemImposto_RecuperaPorContratoDadosNota do desktop
            //    //row[sequencialItem] = null;
            //    //row[descricaoItem] = null;
            //    //Esse campo está no rpt mais não vem da procedure Contrato.contratoRetificacaoItemImposto_RecuperaPorContratoDadosNota do desktop
            //    decimal valorImpostoAux = 0;
            //    row[valorImposto] = valorImpostoAux;
            //    row[indireto] = false;
            //    //row[girErro] = "";

            //    dta.Rows.Add(row);

            //}

            foreach (var imposto in listaImposto)
            {
                DataRow row = dta.NewRow();

                row[codigo] = imposto.Id;
                row[contrato] = imposto.ContratoId;
                row[contratoRetificacao] = imposto.ContratoRetificacaoId;
                row[contratoRetificacaoItem] = imposto.ContratoRetificacaoItemId;
                row[impostoFinanceiro] = imposto.ImpostoFinanceiroId;
                row[percentualBaseCalculo] = imposto.PercentualBaseCalculo;
                row[aliquota] = imposto.ImpostoFinanceiro.Aliquota;
                row[retido] = imposto.ImpostoFinanceiro.Retido;
                row[descricaoImposto] =  imposto.ImpostoFinanceiro.Descricao;
                //Esse campo está no rpt mais vem da procedure Contrato.contratoRetificacaoItemImposto_RecuperaPorContratoDadosNota do desktop
                //row[valorTotalMedido] = null;
                //Esse campo está no rpt mais não vem da procedure Contrato.contratoRetificacaoItemImposto_RecuperaPorContratoDadosNota do desktop
                row[sigla] = imposto.ImpostoFinanceiro.Sigla;
                //Esse campo está no rpt mais não vem da procedure Contrato.contratoRetificacaoItemImposto_RecuperaPorContratoDadosNota do desktop
                //row[sequencialItem] = null;
                //row[descricaoItem] = null;
                //Esse campo está no rpt mais não vem da procedure Contrato.contratoRetificacaoItemImposto_RecuperaPorContratoDadosNota do desktop
                decimal valorImpostoAux = 0;
                row[valorImposto] = valorImpostoAux;
                row[indireto] = imposto.ImpostoFinanceiro.Indireto;
                row[girErro] = "";

                dta.Rows.Add(row);
            }

            return dta;
        }


        private bool EhValidoSalvar(ContratoRetificacaoItemMedicaoDTO dto)
        {
            int contratadoId = 0;

            //contratadoId = dto.Contrato.ContratadoId;

            if (contratadoId == 0)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Contratado"), TypeMessage.Error);
                return false;
            }

            if (dto.MultiFornecedorId.HasValue){
                contratadoId = dto.MultiFornecedorId.Value;
            }

            if (dto.DataEmissao == null)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio,"Data emissão"), TypeMessage.Error);
                return false;
            }

            if (!ValidaData(dto.DataEmissao.ToShortDateString()))
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoInvalido, "Data emissão"), TypeMessage.Error);
                return false;
            }

            if (dto.DataVencimento == null)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Data vencimento"), TypeMessage.Error);
                return false;
            }

            if (!ValidaData(dto.DataVencimento.ToShortDateString()))
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoInvalido, "Data vencimento"), TypeMessage.Error);
                return false;
            }

            if (ComparaDatas(dto.DataEmissao.ToShortDateString(), dto.DataVencimento.ToShortDateString()) < 0)
            {
                string msg = string.Format(Resource.Contrato.ErrorMessages.DataMaiorQue, "Data emissão", "Data vencimento");
                messageQueue.Add(msg, TypeMessage.Error);
                return false;
            }

            var parametros = parametrosContratoAppService.Obter().To<ParametrosContrato>();

            if (dto.TipoDocumentoId == 0)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Tipo"), TypeMessage.Error);
            }

            if (string.IsNullOrEmpty(dto.NumeroDocumento))
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Documento"), TypeMessage.Error);
                return false;
            }

            bool condicao = !dto.Id.HasValue ? true: ( dto.Id.Value == 0 ? true : false );

            if (condicao)
            {
                if (ExisteNumeroDocumento(dto.DataEmissao,
                                          dto.NumeroDocumento,
                                          contratadoId))
                {
                    messageQueue.Add(Resource.Contrato.ErrorMessages.DocumentoExistente, TypeMessage.Error);
                    return false;
                }

                if (ExisteNumeroDocumentoTituloAPagar(dto.DataEmissao,
                                                      dto.DataVencimento,
                                                      dto.NumeroDocumento,
                                                      contratadoId))
                {
                    messageQueue.Add(Resource.Contrato.ErrorMessages.DocumentoExistente, TypeMessage.Error);
                    return false;
                }
            }

            if (dto.DataMedicao == null)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Data medição"), TypeMessage.Error);
                return false;
            }

            if (!ValidaData(dto.DataMedicao.ToShortDateString()))
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoInvalido, "Data medição"), TypeMessage.Error);
                return false;
            }

            if (ComparaDatas(dto.DataMedicao.ToShortDateString(), dto.DataVencimento.ToShortDateString()) < 0)
            {
                string msg = string.Format(Resource.Contrato.ErrorMessages.DataMaiorQue, "Data medição", "Data vencimento");
                messageQueue.Add(msg, TypeMessage.Error);
                return false;
            }

            DateTime DataLimiteMedicao = DateTime.Now;

            if (parametros != null)
            {
                if (parametros.DiasPagamento.HasValue)
                {

                    DataLimiteMedicao = DateTime.Now.AddDays((parametros.DiasPagamento.Value * -1));

                    if (parametros.DiasPagamento.Value > 0)
                    {
                        int numeroDias = (int)dto.DataVencimento.Subtract(dto.DataMedicao).TotalDays;
                        if (numeroDias < parametros.DiasPagamento.Value)
                        {
                            messageQueue.Add(Application.Resource.Contrato.ErrorMessages.DataVencimentoForaDoLimite, TypeMessage.Error);
                            return false;
                        }
                    }
                }
            }

            if (ComparaDatas(dto.DataMedicao.ToShortDateString(), DataLimiteMedicao.ToShortDateString()) < 0)
            {
                messageQueue.Add(Application.Resource.Contrato.ErrorMessages.DataMedicaoMenorQueDataLimite, TypeMessage.Error);
                return false;
            }

            if (dto.Quantidade == 0)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Quantidade medição atual"), TypeMessage.Error);
                return false;
            }

            if (dto.ContratoRetificacaoItem.NaturezaItem == NaturezaItem.PrecoGlobal)
            {
                if (dto.Valor == 0)
                {
                    messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Valor medição atual"), TypeMessage.Error);
                    return false;
                }
                if (dto.Valor > dto.Totalizadores.ValorPendente)
                {
                    string msg = string.Format(Resource.Contrato.ErrorMessages.ValorMaiorQue, "Valor medição atual", "Valor pendente");
                    messageQueue.Add(msg, TypeMessage.Error);
                    return false;
                }
            }
            else if (dto.ContratoRetificacaoItem.NaturezaItem == NaturezaItem.PrecoUnitario)
            {
                if (dto.Quantidade > dto.Totalizadores.QuantidadePendente)
                {
                    string msg = string.Format(Resource.Contrato.ErrorMessages.ValorMaiorQue, "Quantidade medição atual", "Quantidade pendente");
                    messageQueue.Add(msg, TypeMessage.Error);
                    return false;
                }
            }

            if (dto.Desconto > 0) 
            {
                if (string.IsNullOrEmpty(dto.MotivoDesconto))
                {
                    messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Motivo desconto"), TypeMessage.Error);
                    return false;
                }
                if (dto.Desconto > dto.Valor)
                {
                    string msg = string.Format(Resource.Contrato.ErrorMessages.ValorMaiorQue, "Desconto", "Valor medição atual");
                    messageQueue.Add(msg, TypeMessage.Error);
                    return false;
                }
            }

            Nullable<DateTime> dataBloqueio;
            string codigoCentroCusto = "";
            //codigoCentroCusto = dto.Contrato.CentroCusto.Codigo;

            if (!bloqueioContabilAppService.ValidaBloqueioContabil(codigoCentroCusto, 
                                                                   dto.DataEmissao,
                                                                   out dataBloqueio))
            {
                string msg = string.Format(Resource.Sigim.ErrorMessages.BloqueioContabilEncontrado, dataBloqueio.Value.ToShortDateString(), codigoCentroCusto);
                messageQueue.Add(msg, TypeMessage.Error);
                return false;
            }

            if (parametros == null) return true;

            if (!parametros.DadosSped.HasValue) return true;

            if (parametros.DadosSped.Value)
            {
                if (string.IsNullOrEmpty(dto.TipoCompraCodigo))
                {
                    messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Tipo compra"), TypeMessage.Error);
                    return false;
                }

                if ((!dto.CifFobId.HasValue) || (dto.CifFobId.Value == 0))
                {
                    messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "CIF/FOB"), TypeMessage.Error);
                    return false;
                }

                if ((string.IsNullOrEmpty(dto.NaturezaOperacaoCodigo)))
                {
                    messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Natureza de operação"), TypeMessage.Error);
                    return false;
                }

                if ((!dto.SerieNFId.HasValue) || (dto.SerieNFId.Value == 0))
                {
                    messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Série"), TypeMessage.Error);
                    return false;
                }

                if ((string.IsNullOrEmpty(dto.CSTCodigo)))
                {
                    messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "CST"), TypeMessage.Error);
                    return false;
                }

                if ((string.IsNullOrEmpty(dto.CodigoContribuicaoCodigo)))
                {
                    messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Contribuição"), TypeMessage.Error);
                    return false;
                }
            }

            return true;
        }

        private void PopularEntity(ContratoRetificacaoItemMedicaoDTO dto, ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao)
        {
            contratoRetificacaoItemMedicao.Id = dto.Id;
            contratoRetificacaoItemMedicao.ContratoId = dto.ContratoId;
            contratoRetificacaoItemMedicao.ContratoRetificacaoId = dto.ContratoRetificacaoId;
            contratoRetificacaoItemMedicao.ContratoRetificacaoItemId = dto.ContratoRetificacaoItemId;
            contratoRetificacaoItemMedicao.ContratoRetificacaoItemCronogramaId = dto.ContratoRetificacaoItemCronogramaId;
            contratoRetificacaoItemMedicao.SequencialItem = dto.SequencialItem;
            contratoRetificacaoItemMedicao.SequencialCronograma = dto.SequencialCronograma;
            contratoRetificacaoItemMedicao.Situacao = SituacaoMedicao.AguardandoAprovacao;
            contratoRetificacaoItemMedicao.TipoDocumentoId = dto.TipoDocumentoId;
            contratoRetificacaoItemMedicao.NumeroDocumento = dto.NumeroDocumento;
            contratoRetificacaoItemMedicao.DataMedicao = dto.DataMedicao;
            contratoRetificacaoItemMedicao.DataEmissao = dto.DataEmissao;
            contratoRetificacaoItemMedicao.DataVencimento = dto.DataVencimento;
            contratoRetificacaoItemMedicao.Quantidade = dto.Quantidade;
            contratoRetificacaoItemMedicao.Valor = dto.Valor;
            contratoRetificacaoItemMedicao.MultiFornecedorId = dto.MultiFornecedorId;
            contratoRetificacaoItemMedicao.Observacao = dto.Observacao;
            contratoRetificacaoItemMedicao.Desconto = dto.Desconto;
            contratoRetificacaoItemMedicao.MotivoDesconto = dto.MotivoDesconto;

            decimal valorRetido = CalculaValorRetido(dto);

            if (valorRetido > 0)
            {
                contratoRetificacaoItemMedicao.ValorRetido = valorRetido;
            }

            contratoRetificacaoItemMedicao.TipoCompraCodigo = dto.TipoCompraCodigo;
            contratoRetificacaoItemMedicao.CifFobId = dto.CifFobId;
            contratoRetificacaoItemMedicao.NaturezaOperacaoCodigo = dto.NaturezaOperacaoCodigo;
            contratoRetificacaoItemMedicao.SerieNFId = dto.SerieNFId;
            contratoRetificacaoItemMedicao.CSTCodigo = dto.CSTCodigo;
            contratoRetificacaoItemMedicao.CodigoContribuicaoCodigo = dto.CodigoContribuicaoCodigo;
            contratoRetificacaoItemMedicao.CodigoBarras = dto.CodigoBarras;
        }

        private bool ExisteNumeroDocumentoTituloAPagar(DateTime dataEmissao, DateTime dataVencimento, string numeroDocumento, int contratadoId)
        {

            if (tituloPagarAppService.ExisteNumeroDocumento(dataEmissao,
                                                            dataVencimento,
                                                            numeroDocumento,
                                                            contratadoId))
            {
                return false;
            }

            return false;
        }

        private decimal CalculaValorRetido(ContratoRetificacaoItemMedicaoDTO medicao)
        {
            decimal valorRetido = 0.0m;
            decimal percentualRetencao = 0.0m;
            decimal percentualBaseCalculo = 0.0m;

            if (medicao.ContratoRetificacao.RetencaoContratual.HasValue)
            {
                percentualRetencao = medicao.ContratoRetificacao.RetencaoContratual.Value;
                percentualBaseCalculo = 100.0m;
            }

            if (medicao.ContratoRetificacaoItem.RetencaoItem.HasValue)
            {
                percentualRetencao = medicao.ContratoRetificacaoItem.RetencaoItem.Value;
                percentualBaseCalculo = medicao.ContratoRetificacaoItem.BaseRetencaoItem.Value;
            }

            if (percentualRetencao > 0.0m)
            {
                decimal valorBaseCalculo = ((medicao.Valor * percentualBaseCalculo) / 100);
                valorRetido = decimal.Round(((valorBaseCalculo * percentualRetencao) / 100), 2);
            }

            return valorRetido;
        }

        private bool EhValidoCancelar(int? contratoRetificacaoItemMedicaoId)
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

            return true;
        }

        private string MedicaoToXML(ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<contratoRetificacaoItemMedicao>");
            sb.Append("<Contrato.contratoRetificacaoItemMedicao>");
            sb.Append("<codigo>" + contratoRetificacaoItemMedicao.Id.ToString() + "</codigo>");
            sb.Append("<contrato>" + contratoRetificacaoItemMedicao.ContratoId.ToString() + "</contrato>");
            //sb.Append("<descricaoContrato>" + contratoRetificacaoItemMedicao.Contrato.ContratoDescricao.Descricao + "</descricaoContrato>");
            sb.Append("<contratoRetificacao>" + contratoRetificacaoItemMedicao.ContratoRetificacaoId.ToString() +  "</contratoRetificacao>");
            sb.Append("<contratoRetificacaoItem>" + contratoRetificacaoItemMedicao.ContratoRetificacaoItemId.ToString() + "</contratoRetificacaoItem>");
            sb.Append("<sequencialItem>" + contratoRetificacaoItemMedicao.SequencialItem.ToString() + "</sequencialItem>");
            //sb.Append("<servico>" + contratoRetificacaoItemMedicao.ContratoRetificacaoItem.ServicoId + "</servico>");
            sb.Append("<contratoRetificacaoItemCronograma>" + contratoRetificacaoItemMedicao.ContratoRetificacaoItemCronogramaId.ToString() + "</contratoRetificacaoItemCronograma>");
            sb.Append("<sequencialCronograma>" + contratoRetificacaoItemMedicao.SequencialCronograma.ToString() + "</sequencialCronograma>");
            sb.Append("<tipoDocumento>" + contratoRetificacaoItemMedicao.TipoDocumentoId.ToString() + "</tipoDocumento>");
            //sb.Append("<tipoDocumentoDescricao>" + contratoRetificacaoItemMedicao.TipoDocumento.Descricao + "</tipoDocumentoDescricao>");
            //sb.Append("<tipoDocumentoSigla>" + contratoRetificacaoItemMedicao.TipoDocumento.Sigla + "</tipoDocumentoSigla>");
            sb.Append("<numeroDocumento>" +  contratoRetificacaoItemMedicao.NumeroDocumento + "</numeroDocumento>");
            sb.Append("<multiFornecedor>" + contratoRetificacaoItemMedicao.MultiFornecedorId.ToString() + "</multiFornecedor>");
            sb.Append("<dataCadastro>" + contratoRetificacaoItemMedicao.DataCadastro.ToString() + "</dataCadastro>");
            sb.Append("<dataVencimento>" + contratoRetificacaoItemMedicao.DataVencimento.ToString() + "</dataVencimento>");
            sb.Append("<dataEmissao>" +  contratoRetificacaoItemMedicao.DataEmissao.ToString() + "</dataEmissao>");
            sb.Append("<dataMedicao>" +  contratoRetificacaoItemMedicao.DataMedicao.ToString() + "</dataMedicao>");
            sb.Append("<usuarioMedicao>" + contratoRetificacaoItemMedicao.UsuarioMedicao + "</usuarioMedicao>");
            sb.Append("<valor>" + contratoRetificacaoItemMedicao.Valor.ToString("0.00000") + "</valor>");
            sb.Append("<quantidade>" + contratoRetificacaoItemMedicao.Quantidade.ToString("0.00000") + "</quantidade>");
            if (contratoRetificacaoItemMedicao.ValorRetido.HasValue)
            {
                sb.Append("<valorRetido>" + contratoRetificacaoItemMedicao.ValorRetido.Value.ToString("0.00000") + "</valorRetido>");
            }
            else{
                sb.Append("<valorRetido />");
            }
            sb.Append("<observacao>" + contratoRetificacaoItemMedicao.Observacao + "</observacao>");
            sb.Append("<situacao>" + ((int)contratoRetificacaoItemMedicao.Situacao).ToString()  + "</situacao>");
            if (contratoRetificacaoItemMedicao.Desconto.HasValue)
            {
                sb.Append("<desconto>" +  contratoRetificacaoItemMedicao.Desconto.Value.ToString("0.00000") + "</desconto>");
                sb.Append("<motivoDesconto>" + contratoRetificacaoItemMedicao.MotivoDesconto + "</motivoDesconto>");
            }
            else{
                sb.Append("<desconto />");
                sb.Append("<motivoDesconto />");
            }
            //sb.Append("<situacaoContrato>" + ((int)contratoRetificacaoItemMedicao.Contrato.Situacao).ToString() + "</situacaoContrato>");
            //sb.Append("<tipoContrato>" + contratoRetificacaoItemMedicao.Contrato.TipoContrato.ToString() + "</tipoContrato>");
            //sb.Append("<contratado>" + contratoRetificacaoItemMedicao.Contrato.ContratadoId.ToString() + "</contratado>");
            //sb.Append("<nomeContratado>" + contratoRetificacaoItemMedicao.Contrato.Contratado.Nome + "</nomeContratado>");
            //if (contratoRetificacaoItemMedicao.Contrato.Contratado.TipoPessoa == "F"){
            //    sb.Append("<CPFCNPJContratado>" + contratoRetificacaoItemMedicao.Contrato.Contratado.PessoaFisica.Cpf + "</CPFCNPJContratado>");
            //}
            //else if (contratoRetificacaoItemMedicao.Contrato.Contratado.TipoPessoa == "J"){
            //    sb.Append("<CPFCNPJContratado>" + contratoRetificacaoItemMedicao.Contrato.Contratado.PessoaJuridica.Cnpj + "</CPFCNPJContratado>");
            //}
            //sb.Append("<contratante>" + contratoRetificacaoItemMedicao.Contrato.ContratanteId + "</contratante>");
            //sb.Append("<nomeContratante>" + contratoRetificacaoItemMedicao.Contrato.Contratante.Nome + "</nomeContratante>");
            //if (contratoRetificacaoItemMedicao.Contrato.Contratante.TipoPessoa == "F"){
            //    sb.Append("<CPFCNPJContratante>" + contratoRetificacaoItemMedicao.Contrato.Contratante.PessoaFisica.Cpf + "</CPFCNPJContratante>");
            //}
            //else if (contratoRetificacaoItemMedicao.Contrato.Contratado.TipoPessoa == "J"){
            //    sb.Append("<CPFCNPJContratante>" + contratoRetificacaoItemMedicao.Contrato.Contratante.PessoaJuridica.Cnpj + "</CPFCNPJContratante>");
            //}
            //if (contratoRetificacaoItemMedicao.Contrato.TipoContrato == 0){
            //    sb.Append("<codigoFornecedor>" + contratoRetificacaoItemMedicao.Contrato.ContratadoId + "</codigoFornecedor>");
            //}
            //else{
            //    sb.Append("<codigoFornecedor>" + contratoRetificacaoItemMedicao.Contrato.ContratanteId + "</codigoFornecedor>");
            //}

            //A tag abaixo é um sum do campo valor para todas as medições que ocorreram no contrato, não fiz esse campo"
            //sb.Append("<valorTotalMedidoLiberadoContrato>" +  + "</valorTotalMedidoLiberadoContrato>");
            //decimal valorContrato = 0;
            //if (contratoRetificacaoItemMedicao.Contrato.ValorContrato.HasValue){
            //    sb.Append("<valorContrato>" + contratoRetificacaoItemMedicao.Contrato.ValorContrato.Value.ToString("0.00000") + "</valorContrato>");
            //    valorContrato = contratoRetificacaoItemMedicao.Contrato.ValorContrato.Value;
            //}
            //else{
            //    sb.Append("<valorContrato />");
            //}
            //decimal saldoContrato = 0;
            //saldoContrato = valorContrato - <valorTotalMedidoLiberadoContrato>;
            //sb.Append("<saldoContrato>" + saldoContrato.ToString("0.00000") + "</saldoContrato>");
            //sb.Append("<centroCusto>" + contratoRetificacaoItemMedicao.Contrato.CodigoCentroCusto + "</centroCusto>");
            //sb.Append("<descricaoCentroCusto>" + contratoRetificacaoItemMedicao.Contrato.CentroCusto.Descricao + "</descricaoCentroCusto>");
            //sb.Append("<situacaoCentroCusto>" + contratoRetificacaoItemMedicao.Contrato.CentroCusto.Situacao + "</situacaoCentroCusto>");
            //sb.Append("<classe>" + contratoRetificacaoItemMedicao.ContratoRetificacaoItem.CodigoClasse + "</classe>");
            //sb.Append("<descricaoClasse>" + contratoRetificacaoItemMedicao.ContratoRetificacaoItem.Classe.Descricao + "</descricaoClasse>");
            //sb.Append("<precoUnitario>" + contratoRetificacaoItemMedicao.ContratoRetificacaoItem.PrecoUnitario.ToString("0.00000") + "</precoUnitario>");
            //if (contratoRetificacaoItemMedicao.ContratoRetificacaoItem.ValorItem.HasValue){
            //    sb.Append("<valorItem>" + contratoRetificacaoItemMedicao.ContratoRetificacaoItem.ValorItem.Value.ToString("0.00000") + "</valorItem>");
            //}
            //else{
            //    sb.Append("<valorItem />");
            //}
            //if (contratoRetificacaoItemMedicao.ContratoRetificacaoItem.Quantidade.HasValue){
            //    sb.Append("<quantidadeItem>" + contratoRetificacaoItemMedicao.ContratoRetificacaoItem.Quantidade.Value.ToString("0.00000") + "</valorItem>");
            //}
            //else{
            //    sb.Append("<quantidadeItem />");
            //}
            //sb.Append("<descricaoCronograma>" + contratoRetificacaoItemMedicao.ContratoRetificacaoItemCronograma.Descricao + "</descricaoCronograma>");
            //sb.Append("<quantidadeCronograma>" + contratoRetificacaoItemMedicao.ContratoRetificacaoItemCronograma.Quantidade.ToString("0.00000") + "</quantidadeCronograma>");
            //sb.Append("<valorCronograma>" + contratoRetificacaoItemMedicao.ContratoRetificacaoItemCronograma.Valor.ToString("0.00000") + "</valorCronograma>");
            //sb.Append("<unidadeMedida>" + contratoRetificacaoItemMedicao.ContratoRetificacaoItem.Servico.SiglaUnidadeMedida + "</unidadeMedida>");
            //sb.Append("<descricaoItem>" + contratoRetificacaoItemMedicao.ContratoRetificacaoItem.Servico.Descricao + "</descricaoItem>");
            //sb.Append("<complementoDescricaoItem>" + contratoRetificacaoItemMedicao.ContratoRetificacaoItem.ComplementoDescricao + "</complementoDescricaoItem>");
            sb.Append("<descricaoSituacaoMedicao>" + contratoRetificacaoItemMedicao.Situacao.ObterDescricao() + "</descricaoSituacaoMedicao>");
            //sb.Append("<naturezaItem>" + ((int)contratoRetificacaoItemMedicao.ContratoRetificacaoItem.NaturezaItem).ToString() + "</naturezaItem>");
            sb.Append("<codigoBarras>" + contratoRetificacaoItemMedicao.CodigoBarras + "</codigoBarras>");
            //sb.Append("<valorTotalMedido>" +   + "</valorTotalMedido>");
            //sb.Append("<quantidadeTotalMedida>" +  + "</quantidadeTotalMedida>");
            //sb.Append("<valorTotalLiberado>" +  + "</valorTotalLiberado>");
            //sb.Append("<quantidadeTotalLiberada>" + + "</quantidadeTotalLiberada>");
            //sb.Append("<valorTotalMedidoLiberado>" + + "</valorTotalMedidoLiberado>");
            //sb.Append("<quantidadeTotalMedidaLiberada>" +  + "</quantidadeTotalMedidaLiberada>");
            //sb.Append("<valorImpostoRetido>" + + "</valorImpostoRetido>");
            //sb.Append("<valorImpostoRetidoMedicao>" +  + "</valorImpostoRetidoMedicao>");
            //sb.Append("<valorTotalMedidoNota>" + + "</valorTotalMedidoNota>");
            //sb.Append("<valorImpostoIndiretoMedicao>" +  + "</valorImpostoIndiretoMedicao>");
            //sb.Append("<valorTotalMedidoIndireto>" +  +"</valorTotalMedidoIndireto>");
            sb.Append("</Contrato.contratoRetificacaoItemMedicao>");
            sb.Append("</contratoRetificacaoItemMedicao>");

            return sb.ToString();
        }


        private void GravarLogOperacao(ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao, string operacao)
        {
            string descricaoOperacao= "";
            string nomeRotina="";
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
                MedicaoToXML(contratoRetificacaoItemMedicao));

        }


        
        #endregion
    }
}
