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

        #endregion

        #region Construtor

        public ContratoRetificacaoItemMedicaoAppService(MessageQueue messageQueue)
            : base(messageQueue)
        {
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

        #endregion

        #region Métodos privados
        
        #endregion
    }
}
