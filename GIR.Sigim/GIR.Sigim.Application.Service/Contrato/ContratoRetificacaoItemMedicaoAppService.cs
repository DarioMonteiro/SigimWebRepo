using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Domain.Repository.Contrato;
using GIR.Sigim.Domain.Repository.Financeiro;
using System.Threading.Tasks;
using CrystalDecisions.Shared;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Application.Filtros.Contrato;
using GIR.Sigim.Application.DTO.Contrato;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Domain.Specification.Contrato;
using GIR.Sigim.Application.Service.Admin;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Reports.Contrato;

namespace GIR.Sigim.Application.Service.Contrato
{
    public class ContratoRetificacaoItemMedicaoAppService : BaseAppService, IContratoRetificacaoItemMedicaoAppService
    {
        #region Declaração

        private IUsuarioAppService usuarioAppService;
        private IParametrosContratoRepository parametrosContratoRepository;
        private IContratoRetificacaoItemMedicaoRepository contratoRetificacaoItemMedicaoRepository;
        private ICentroCustoRepository centroCustoRepository;

        #endregion

        #region Construtor

        public ContratoRetificacaoItemMedicaoAppService(IUsuarioAppService usuarioAppService,
                                                        IParametrosContratoRepository parametrosContratoRepository,
                                                        IContratoRetificacaoItemMedicaoRepository contratoRetificacaoItemMedicaoRepository,
                                                        ICentroCustoRepository centroCustoRepository,
                                                        MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.usuarioAppService = usuarioAppService;
            this.parametrosContratoRepository = parametrosContratoRepository;
            this.contratoRetificacaoItemMedicaoRepository = contratoRetificacaoItemMedicaoRepository;
            this.centroCustoRepository = centroCustoRepository;
        }

        #endregion

        #region Métodos IContratoRetificacaoItemMedicaoAppService

        public string MedicaoToXML(ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<contratoRetificacaoItemMedicao>");
            sb.Append("<Contrato.contratoRetificacaoItemMedicao>");
            sb.Append("<codigo>" + contratoRetificacaoItemMedicao.Id.ToString() + "</codigo>");
            sb.Append("<contrato>" + contratoRetificacaoItemMedicao.ContratoId.ToString() + "</contrato>");
            //sb.Append("<descricaoContrato>" + contratoRetificacaoItemMedicao.Contrato.ContratoDescricao.Descricao + "</descricaoContrato>");
            sb.Append("<contratoRetificacao>" + contratoRetificacaoItemMedicao.ContratoRetificacaoId.ToString() + "</contratoRetificacao>");
            sb.Append("<contratoRetificacaoItem>" + contratoRetificacaoItemMedicao.ContratoRetificacaoItemId.ToString() + "</contratoRetificacaoItem>");
            sb.Append("<sequencialItem>" + contratoRetificacaoItemMedicao.SequencialItem.ToString() + "</sequencialItem>");
            //sb.Append("<servico>" + contratoRetificacaoItemMedicao.ContratoRetificacaoItem.ServicoId + "</servico>");
            sb.Append("<contratoRetificacaoItemCronograma>" + contratoRetificacaoItemMedicao.ContratoRetificacaoItemCronogramaId.ToString() + "</contratoRetificacaoItemCronograma>");
            sb.Append("<sequencialCronograma>" + contratoRetificacaoItemMedicao.SequencialCronograma.ToString() + "</sequencialCronograma>");
            sb.Append("<tipoDocumento>" + contratoRetificacaoItemMedicao.TipoDocumentoId.ToString() + "</tipoDocumento>");
            //sb.Append("<tipoDocumentoDescricao>" + contratoRetificacaoItemMedicao.TipoDocumento.Descricao + "</tipoDocumentoDescricao>");
            //sb.Append("<tipoDocumentoSigla>" + contratoRetificacaoItemMedicao.TipoDocumento.Sigla + "</tipoDocumentoSigla>");
            sb.Append("<numeroDocumento>" + contratoRetificacaoItemMedicao.NumeroDocumento + "</numeroDocumento>");
            sb.Append("<multiFornecedor>" + contratoRetificacaoItemMedicao.MultiFornecedorId.ToString() + "</multiFornecedor>");
            sb.Append("<dataCadastro>" + contratoRetificacaoItemMedicao.DataCadastro.ToString() + "</dataCadastro>");
            sb.Append("<dataVencimento>" + contratoRetificacaoItemMedicao.DataVencimento.ToString() + "</dataVencimento>");
            sb.Append("<dataEmissao>" + contratoRetificacaoItemMedicao.DataEmissao.ToString() + "</dataEmissao>");
            sb.Append("<dataMedicao>" + contratoRetificacaoItemMedicao.DataMedicao.ToString() + "</dataMedicao>");
            sb.Append("<usuarioMedicao>" + contratoRetificacaoItemMedicao.UsuarioMedicao + "</usuarioMedicao>");
            sb.Append("<valor>" + contratoRetificacaoItemMedicao.Valor.ToString("0.00000") + "</valor>");
            sb.Append("<quantidade>" + contratoRetificacaoItemMedicao.Quantidade.ToString("0.00000") + "</quantidade>");
            if (contratoRetificacaoItemMedicao.ValorRetido.HasValue)
            {
                sb.Append("<valorRetido>" + contratoRetificacaoItemMedicao.ValorRetido.Value.ToString("0.00000") + "</valorRetido>");
            }
            else
            {
                sb.Append("<valorRetido />");
            }
            sb.Append("<observacao>" + contratoRetificacaoItemMedicao.Observacao + "</observacao>");
            sb.Append("<situacao>" + ((int)contratoRetificacaoItemMedicao.Situacao).ToString() + "</situacao>");
            if (contratoRetificacaoItemMedicao.Desconto.HasValue)
            {
                sb.Append("<desconto>" + contratoRetificacaoItemMedicao.Desconto.Value.ToString("0.00000") + "</desconto>");
                sb.Append("<motivoDesconto>" + contratoRetificacaoItemMedicao.MotivoDesconto + "</motivoDesconto>");
            }
            else
            {
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

        public List<RelNotaFiscalLiberadaDTO> ListarPeloFiltroRelNotaFiscalLiberada(RelNotaFiscalLiberadaFiltro filtro, int? idUsuario, out int totalRegistros)
        {
            var specification = MontarSpecificationRelNotaFiscalLiberada(filtro, idUsuario);

            var listaMedicao =
             contratoRetificacaoItemMedicaoRepository.ListarPeloFiltroComPaginacao(specification,
                                                                                   filtro.PaginationParameters.PageIndex,
                                                                                   filtro.PaginationParameters.PageSize,
                                                                                   filtro.PaginationParameters.OrderBy,
                                                                                   filtro.PaginationParameters.Ascending,
                                                                                   out totalRegistros,
                                                                                   l => l.Contrato.ContratoDescricao,
                                                                                   l => l.Contrato.CentroCusto,
                                                                                   l => l.Contrato.Contratado.PessoaFisica,
                                                                                   l => l.Contrato.Contratado.PessoaJuridica,
                                                                                   l => l.Contrato.Contratante.PessoaFisica,
                                                                                   l => l.Contrato.Contratante.PessoaJuridica,
                                                                                   l => l.ContratoRetificacaoItem.Classe,
                                                                                   l => l.ContratoRetificacaoItem.Servico,
                                                                                   l => l.ContratoRetificacaoItem.ListaContratoRetificacaoItemImposto.Select(i => i.ImpostoFinanceiro),
                                                                                   l => l.MultiFornecedor).To<List<ContratoRetificacaoItemMedicao>>();

            List<RelNotaFiscalLiberadaDTO> listaRelNotaFiscalLiberadaDTO = new List<RelNotaFiscalLiberadaDTO>();
            foreach (var medicao in listaMedicao)
            {
                RelNotaFiscalLiberadaDTO relat = new RelNotaFiscalLiberadaDTO();

                if (medicao.MultiFornecedorId.HasValue)
                {
                    relat.FornecedorCliente = medicao.MultiFornecedor.To<Application.DTO.Sigim.ClienteFornecedorDTO>();
                    relat.FornecedorClienteId = medicao.MultiFornecedorId;
                }
                else
                {
                    if (medicao.Contrato.TipoContrato == 0)
                    {
                        relat.FornecedorCliente = medicao.Contrato.Contratado.To<Application.DTO.Sigim.ClienteFornecedorDTO>();
                        relat.FornecedorClienteId = medicao.Contrato.ContratadoId;
                    }
                    else
                    {
                        relat.FornecedorCliente = medicao.Contrato.Contratante.To<Application.DTO.Sigim.ClienteFornecedorDTO>();
                        relat.FornecedorClienteId = medicao.Contrato.ContratanteId;
                    }
                }
                relat.NumeroDocumento = medicao.NumeroDocumento;
                relat.Contrato = medicao.Contrato.To<Application.DTO.Contrato.ContratoDTO>(); 
                relat.ContratoId = medicao.Contrato.Id.Value;
                relat.ContratoRetificacaoItem = medicao.ContratoRetificacaoItem.To<ContratoRetificacaoItemDTO>(); 
                relat.ContratoRetificacaoItemId = medicao.ContratoRetificacaoItem.Id.Value;
                relat.DataEmissao = medicao.DataEmissao;
                relat.DataVencimento = medicao.DataVencimento;
                decimal valorImpostoRetidoMedicao = medicao.Contrato.ObterValorImpostoRetidoMedicao(medicao.SequencialItem,
                                                                                                    medicao.SequencialCronograma,
                                                                                                    medicao.ContratoRetificacaoItemId,
                                                                                                    medicao.Id);
                decimal valorRetido = 0;
                decimal desconto = 0;
                if (medicao.ValorRetido.HasValue)
                {
                    valorRetido = medicao.ValorRetido.Value;
                }
                if (valorRetido > valorImpostoRetidoMedicao)
                {
                    desconto = valorRetido - valorImpostoRetidoMedicao;
                }
                else
                {
                    desconto = valorImpostoRetidoMedicao - valorRetido;
                }

                decimal valorDesconto = medicao.Valor - desconto;

                relat.Valor = medicao.Valor;
                relat.ValorClasse = valorDesconto;
                if (medicao.TituloPagarId.HasValue)
                {
                    relat.TituloId = medicao.TituloPagarId.Value;
                }
                if (medicao.TituloReceberId.HasValue)
                {
                    relat.TituloId = medicao.TituloReceberId.Value;
                }
                relat.UsuarioLiberacao = medicao.UsuarioLiberacao;

                listaRelNotaFiscalLiberadaDTO.Add(relat);
            }

            return listaRelNotaFiscalLiberadaDTO;
        }

        private Specification<ContratoRetificacaoItemMedicao> MontarSpecificationRelNotaFiscalLiberada(RelNotaFiscalLiberadaFiltro filtro, int? idUsuario)
        {
            var specification = (Specification<ContratoRetificacaoItemMedicao>)new TrueSpecification<ContratoRetificacaoItemMedicao>();

            if (usuarioAppService.UsuarioPossuiCentroCustoDefinidoNoModulo(idUsuario, Resource.Sigim.NomeModulo.Contrato))
                specification &= ContratoRetificacaoItemMedicaoSpecification.UsuarioPossuiAcessoAoCentroCusto(idUsuario, Resource.Sigim.NomeModulo.Contrato);

            if (filtro.ContratoId.HasValue)
                specification &= ContratoRetificacaoItemMedicaoSpecification.PertenceAoContratoComSituacaoLiberado(filtro.ContratoId);
            else
            {
                specification &= ContratoRetificacaoItemMedicaoSpecification.DataLiberacaoMaiorOuIgual(filtro.DataInicial);
                specification &= ContratoRetificacaoItemMedicaoSpecification.DataLiberacaoMenorOuIgual(filtro.DataFinal);
                specification &= ContratoRetificacaoItemMedicaoSpecification.PertenceAoCentroCustoIniciadoPor(filtro.CentroCusto.Codigo);
                specification &= ContratoRetificacaoItemMedicaoSpecification.DocumentoPertenceAhMedicao(filtro.Documento);
                specification &= ContratoRetificacaoItemMedicaoSpecification.FornecedorClientePertenceAhMedicao(filtro.FornecedorCliente.Id);
            }

            specification &= ContratoRetificacaoItemMedicaoSpecification.SituacaoIgualLiberado();
            return specification;
        }

        public FileDownloadDTO ExportarRelNotaFiscalLiberada(RelNotaFiscalLiberadaFiltro filtro,
                                                             int? usuarioId,
                                                             FormatoExportacaoArquivo formato)
        {

            var specification = MontarSpecificationRelNotaFiscalLiberada(filtro, usuarioId);

            var listaMedicao =
             contratoRetificacaoItemMedicaoRepository.ListarPeloFiltro(specification,
                                                                        l => l.Contrato.ContratoDescricao,
                                                                        l => l.Contrato.CentroCusto.ListaUsuarioCentroCusto.Select(u => u.Modulo),
                                                                        l => l.Contrato.Contratado.PessoaFisica,
                                                                        l => l.Contrato.Contratado.PessoaJuridica,
                                                                        l => l.Contrato.Contratante.PessoaFisica,
                                                                        l => l.Contrato.Contratante.PessoaJuridica,
                                                                        l => l.ContratoRetificacao,
                                                                        l => l.ContratoRetificacaoItem.Classe,
                                                                        l => l.ContratoRetificacaoItem.Servico,
                                                                        l => l.ContratoRetificacaoItem.ListaContratoRetificacaoItemImposto.Select(i => i.ImpostoFinanceiro),
                                                                        l => l.MultiFornecedor,
                                                                        l => l.ContratoRetificacaoItemCronograma,
                                                                        l => l.TipoDocumento,
                                                                        l => l.TituloPagar,
                                                                        l => l.TituloReceber).To<List<ContratoRetificacaoItemMedicao>>();

            relNotaFiscalLiberada objRel = new relNotaFiscalLiberada();



            objRel.SetDataSource(RelNotaFiscalLiberadaToDataTable(listaMedicao));

            string periodo = filtro.DataInicial.Value.ToString("dd/MM/yyyy") + "  a  " + filtro.DataFinal.Value.ToString("dd/MM/yyyy");

            var parametros = parametrosContratoRepository.Obter();
            var centroCusto = centroCustoRepository.ObterPeloCodigo(filtro.CentroCusto.Codigo, l => l.ListaCentroCustoEmpresa);

            var caminhoImagem = PrepararIconeRelatorio(centroCusto, parametros);

            //var caminhoImagem = DiretorioImagemRelatorio + Guid.NewGuid().ToString() + ".bmp";
            //System.Drawing.Image imagem = parametros.IconeRelatorio.ToImage();
            //imagem.Save(caminhoImagem, System.Drawing.Imaging.ImageFormat.Bmp);

            var nomeEmpresa = ObterNomeEmpresa(centroCusto, parametros);
            objRel.SetParameterValue("nomeEmpresa", nomeEmpresa);
            objRel.SetParameterValue("descricaoCentroCusto", centroCusto.Descricao);
            objRel.SetParameterValue("periodo", periodo);
            objRel.SetParameterValue("caminhoImagem", caminhoImagem);

            FileDownloadDTO arquivo = new FileDownloadDTO(
                "Rel. Nota Fiscal Liberada",
                objRel.ExportToStream((ExportFormatType)formato),
                formato);

            if (System.IO.File.Exists(caminhoImagem))
                System.IO.File.Delete(caminhoImagem);
            return arquivo;
        }

        #endregion

        #region Métodos privados

        private DataTable RelNotaFiscalLiberadaToDataTable(List<ContratoRetificacaoItemMedicao> listaMedicao)
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
                row[valorTotalMedidoNota] = medicao.Contrato.ObterValorTotalMedidoNota(medicao.ContratoId, medicao.NumeroDocumento, medicao.TipoDocumentoId, medicao.DataVencimento);
                row[valorImpostoIndiretoMedicao] = medicao.Contrato.ObterValorTotalImpostoIndiretoMedicao(medicao.SequencialItem, medicao.SequencialCronograma, medicao.ContratoRetificacaoItemId, medicao.Id);
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
                row[valorTotalMedido] = medicao.Contrato.ObterValorTotalMedido(medicao.SequencialItem, medicao.SequencialCronograma);
                row[quantidadeTotalMedida] = medicao.Contrato.ObterQuantidadeTotalMedida(medicao.SequencialItem, medicao.SequencialCronograma);
                row[valorTotalLiberado] = medicao.Contrato.ObterValorTotalLiberado(medicao.SequencialItem, medicao.SequencialCronograma);
                row[quantidadeTotalLiberada] = medicao.Contrato.ObterQuantidadeTotalLiberada(medicao.SequencialItem, medicao.SequencialCronograma);
                row[valorTotalMedidoLiberado] = medicao.Contrato.ObterValorTotalMedidoLiberado(medicao.SequencialItem, medicao.SequencialCronograma);
                row[quantidadeTotalMedidaLiberada] = medicao.Contrato.ObterQuantidadeTotalMedidaLiberada(medicao.SequencialItem, medicao.SequencialCronograma);
                row[valorImpostoRetido] = medicao.Contrato.ObterValorImpostoRetido(medicao.SequencialItem, medicao.SequencialCronograma, medicao.ContratoRetificacaoItemId);
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

        
        #endregion
    }
}
