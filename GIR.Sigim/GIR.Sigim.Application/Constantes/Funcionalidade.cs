using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GIR.Sigim.Application.Constantes
{
    public class Funcionalidade
    {

        #region Hashtable

        public System.Collections.Hashtable MenuAdmin;
        public System.Collections.Hashtable MenuContrato;
        public System.Collections.Hashtable MenuComercial;
        public System.Collections.Hashtable MenuFinanceiro;
        public System.Collections.Hashtable MenuOrdemCompra;
        public System.Collections.Hashtable MenuSac;

        #endregion

        #region Admin

        public const string PerfilAcessar = "PERFIL_ACESSAR";
        public const string PerfilGravar = "PERFIL_GRAVAR";
        public const string PerfilDeletar = "PERFIL_DELETAR";
        public const string PerfilImprimir = "PERFIL_IMPRIMIR";

        public const string UsuarioFuncionalidadeAcessar = "USUARIOFUNCIONALIDADE_ACESSAR";
        public const string UsuarioFuncionalidadeGravar = "USUARIOFUNCIONALIDADE_GRAVAR";
        public const string UsuarioFuncionalidadeDeletar = "USUARIOFUNCIONALIDADE_DELETAR";
        public const string UsuarioFuncionalidadeImprimir = "USUARIOFUNCIONALIDADE_IMPRIMIR";

        #endregion

        #region Contrato

        public const string LiberacaoAcessar = "LIBERACAO_ACESSAR";

        public const string LiberacaoAprovarLiberar = "LIBERACAO_APROVARLIBERAR";
        public const string LiberacaoAprovar = "LIBERACAO_APROVAR";
        public const string LiberacaoLiberar = "LIBERACAO_LIBERAR";
        public const string LiberacaoCancelar = "LIBERACAO_CANCELAR";
        public const string LiberacaoAssociarNF = "LIBERACAO_ASSOCIARNF";
        public const string LiberacaoAlterarVencimento = "LIBERACAO_ALTERARVENCIMENTO";
        public const string LiberacaoImprimirMedicao = "LIBERACAO_IMPRIMIRMEDICAO";

        public const string MedicaoAcessar = "MEDICAO_ACESSAR";
        public const string MedicaoGravar = "MEDICAO_GRAVAR";
        public const string MedicaoDeletar = "MEDICAO_DELETAR";
        public const string MedicaoImprimir = "MEDICAO_IMPRIMIR";

        public const string RelNotasFiscaisLiberadasAcessar = "REL_NF_LIBERADAS_ACESSAR";
        public const string RelNotasFiscaisLiberadasImprimir = "REL_NF_LIBERADAS_IMPRIMIR";
        
        #endregion

        #region Comercial

        public const string RelStatusVendaAcessar = "REL_STATUS_VENDA_ACESSAR";
        public const string RelStatusVendaImprimir = "REL_STATUS_VENDA_IMPRIMIR";

        #endregion

        #region Financeiro

        public const string ParametroFinanceiroAcessar = "PARAMETRO_FINANCEIRO_ACESSAR";
        public const string ParametroFinanceiroGravar = "PARAMETRO_FINANCEIRO_GRAVAR";

        public const string ParametroUsuarioFinanceiroAcessar = "PARAMETRO_USUARIO_FINANCEIRO_ACESSAR";
        public const string ParametroUsuarioFinanceiroGravar = "PARAMETRO_USUARIO_FINANCEIRO_GRAVAR";

        public const string CaixaAcessar = "CAIXA_ACESSAR";
        public const string CaixaGravar = "CAIXA_GRAVAR";
        public const string CaixaDeletar = "CAIXA_DELETAR";
        public const string CaixaImprimir = "CAIXA_IMPRIMIR";

        public const string ImpostoFinanceiroAcessar = "IMPOSTOFINANCEIRO_ACESSAR";
        public const string ImpostoFinanceiroGravar = "IMPOSTOFINANCEIRO_GRAVAR";
        public const string ImpostoFinanceiroDeletar = "IMPOSTOFINANCEIRO_DELETAR";
        public const string ImpostoFinanceiroImprimir = "IMPOSTOFINANCEIRO_IMPRIMIR";

        public const string FormaRecebimentoAcessar = "FORMARECEBIMENTO_ACESSAR";
        public const string FormaRecebimentoGravar = "FORMARECEBIMENTO_GRAVAR";
        public const string FormaRecebimentoDeletar = "FORMARECEBIMENTO_DELETAR";
        public const string FormaRecebimentoImprimir = "FORMARECEBIMENTO_IMPRIMIR";

        public const string MotivoCancelamentoAcessar = "MOTIVOCANCELAMENTO_ACESSAR";
        public const string MotivoCancelamentoGravar = "MOTIVOCANCELAMENTO_GRAVAR";
        public const string MotivoCancelamentoDeletar = "MOTIVOCANCELAMENTO_DELETAR";
        public const string MotivoCancelamentoImprimir = "MOTIVOCANCELAMENTO_IMPRIMIR";

        public const string RateioAutomaticoAcessar = "RATEIOAUTOMATICO_ACESSAR";
        public const string RateioAutomaticoGravar = "RATEIOAUTOMATICO_GRAVAR";
        public const string RateioAutomaticoDeletar = "RATEIOAUTOMATICO_DELETAR";
        public const string RateioAutomaticoImprimir = "RATEIOAUTOMATICO_IMPRIMIR";

        public const string TabelaBasicaFinanceiroAcessar = "TABELABASICA_FINANCEIRO_ACESSAR";
        public const string TabelaBasicaFinanceiroGravar = "TABELABASICA_FINANCEIRO_GRAVAR";
        public const string TabelaBasicaFinanceiroDeletar = "TABELABASICA_FINANCEIRO_DELETAR";
        public const string TabelaBasicaFinanceiroImprimir = "TABELABASICA_FINANCEIRO_IMPRIMIR";

        public const string TaxaAdministracaoAcessar = "TAXAADMINISTRACAO_ACESSAR";
        public const string TaxaAdministracaoGravar = "TAXAADMINISTRACAO_GRAVAR";
        public const string TaxaAdministracaoDeletar = "TAXAADMINISTRACAO_DELETAR";
        public const string TaxaAdministracaoImprimir = "TAXAADMINISTRACAO_IMPRIMIR";

        public const string TipoCompromissoAcessar = "TIPOCOMPROMISSO_ACESSAR";
        public const string TipoCompromissoGravar = "TIPOCOMPROMISSO_GRAVAR";
        public const string TipoCompromissoDeletar = "TIPOCOMPROMISSO_DELETAR";
        public const string TipoCompromissoImprimir = "TIPOCOMPROMISSO_IMPRIMIR";

        public const string TipoDocumentoAcessar = "TIPODOCUMENTO_ACESSAR";
        public const string TipoDocumentoGravar = "TIPODOCUMENTO_GRAVAR";
        public const string TipoDocumentoDeletar = "TIPODOCUMENTO_DELETAR";
        public const string TipoDocumentoImprimir = "TIPODOCUMENTO_IMPRIMIR";

        public const string TipoMovimentoAcessar = "TIPOMOVIMENTO_ACESSAR";
        public const string TipoMovimentoGravar = "TIPOMOVIMENTO_GRAVAR";
        public const string TipoMovimentoDeletar = "TIPOMOVIMENTO_DELETAR";
        public const string TipoMovimentoImprimir = "TIPOMOVIMENTO_IMPRIMIR";

        public const string TipoRateioAcessar = "TIPORATEIO_ACESSAR";
        public const string TipoRateioGravar = "TIPORATEIO_GRAVAR";
        public const string TipoRateioDeletar = "TIPORATEIO_DELETAR";
        public const string TipoRateioImprimir = "TIPORATEIO_IMPRIMIR";

        public const string BancoAcessar = "BANCO_ACESSAR";
        public const string BancoGravar = "BANCO_GRAVAR";
        public const string BancoDeletar = "BANCO_DELETAR";
        public const string BancoImprimir = "BANCO_IMPRIMIR";

        public const string AgenciaAcessar = "AGENCIA_ACESSAR";
        public const string AgenciaGravar = "AGENCIA_GRAVAR";
        public const string AgenciaDeletar = "AGENCIA_DELETAR";
        public const string AgenciaImprimir = "AGENCIA_IMPRIMIR";

        public const string ContaCorrenteAcessar = "CONTACORRENTE_ACESSAR";
        public const string ContaCorrenteGravar = "CONTACORRENTE_GRAVAR";
        public const string ContaCorrenteDeletar = "CONTACORRENTE_DELETAR";
        //public const string ContaCorrenteImprimir = "CONTACORRENTE_IMPRIMIR";

        public const string RelatorioApropriacaoPorClasseAcessar = "REL_APROPRIACAO_POR_CLASSE_ACESSAR";
        public const string RelatorioApropriacaoPorClasseImprimir = "REL_APROPRIACAO_POR_CLASSE_IMPRIMIR";

        public const string RelatorioContasAPagarTitulosAcessar = "REL_CONTASAPAGAR_TITULOS_ACESSAR";
        #endregion

        #region OrdemCompra

        public const string ParametroOrdemCompraAcessar = "PARAMETRO_ORDEMCOMPRA_ACESSAR";
        public const string ParametroOrdemCompraGravar = "PARAMETRO_ORDEMCOMPRA_GRAVAR";

        public const string ParametroUsuarioOrdemCompraAcessar = "PARAMETRO_USUARIO_ORDEMCOMPRA_ACESSAR";
        public const string ParametroUsuarioOrdemCompraGravar = "PARAMETRO_USUARIO_ORDEMCOMPRA_GRAVAR";

        public const string OrdemCompraUnidadeMedidaAcessar = "ORDEMCOMPRA_UNIDADEMEDIDA_ACESSAR";
        public const string OrdemCompraUnidadeMedidaGravar = "ORDEMCOMPRA_UNIDADEMEDIDA_GRAVAR";
        public const string OrdemCompraUnidadeMedidaDeletar = "ORDEMCOMPRA_UNIDADEMEDIDA_DELETAR";
        public const string OrdemCompraUnidadeMedidaImprimir = "ORDEMCOMPRA_UNIDADEMEDIDA_IMPRIMIR";

        //public const string OrdemCompraMaterialAcessar = "ORDEMCOMPRA_MATERIAL_ACESSAR";
        //public const string OrdemCompraMaterialGravar = "ORDEMCOMPRA_MATERIAL_GRAVAR";
        //public const string OrdemCompraMaterialDeletar = "ORDEMCOMPRA_MATERIAL_DELETAR";
        //public const string OrdemCompraMaterialImprimir = "ORDEMCOMPRA_MATERIAL_IMPRIMIR";

        public const string PreRequisicaoMaterialAcessar = "PREREQUISICAOMATERIAL_ACESSAR";
        public const string PreRequisicaoMaterialGravar = "PREREQUISICAOMATERIAL_GRAVAR";
        public const string PreRequisicaoMaterialCancelar = "PREREQUISICAOMATERIAL_CANCELAR";
        public const string PreRequisicaoMaterialImprimir = "PREREQUISICAOMATERIAL_IMPRIMIR";
        public const string PreRequisicaoMaterialAprovar = "PREREQUISICAOMATERIAL_APROVAR";

        public const string RequisicaoMaterialAcessar = "REQUISICAOMATERIAL_ACESSAR";
        public const string RequisicaoMaterialGravar = "REQUISICAOMATERIAL_GRAVAR";
        public const string RequisicaoMaterialCancelar = "REQUISICAOMATERIAL_CANCELAR";
        public const string RequisicaoMaterialImprimir = "REQUISICAOMATERIAL_IMPRIMIR";
        public const string RequisicaoMaterialAprovar = "REQUISICAOMATERIAL_APROVAR";
        public const string RequisicaoMaterialCancelarAprovacao = "REQUISICAOMATERIAL_CANCELAR_APROVACAO";

        public const string EntradaMaterialAcessar = "ENTRADAMATERIAL_ACESSAR";
        public const string EntradaMaterialGravar = "ENTRADAMATERIAL_GRAVAR";
        public const string EntradaMaterialCancelar = "ENTRADAMATERIAL_CANCELAR";
        public const string EntradaMaterialImprimir = "ENTRADAMATERIAL_IMPRIMIR";
        public const string EntradaMaterialLiberar = "ENTRADAMATERIAL_LIBERAR";

        public const string RelatorioItensOrdemCompraAcessar = "REL_ITENS_ORDEMCOMPRA_ACESSAR";
        public const string RelatorioItensOrdemCompraImprimir = "REL_ITENS_ORDEMCOMPRA_IMPRIMIR";

        #endregion

        #region Sac

        public const string ParametroSacAcessar = "PARAMETRO_SAC_ACESSAR";
        public const string ParametroSacGravar = "PARAMETRO_SAC_GRAVAR";

        public const string SetorSacAcessar = "SETOR_SAC_ACESSAR";
        public const string SetorSacGravar = "SETOR_SAC_GRAVAR";
        public const string SetorSacDeletar = "SETOR_SAC_DELETAR";
        //public const string SetorSacImprimir = "SETOR_SAC_IMPRIMIR";

        #endregion

        public Funcionalidade()
        {
            FuncionalidadeAdmin();
            FuncionalidadeComercial();
            FuncionalidadeContrato();
            FuncionalidadeFinanceiro();
            FuncionalidadeOrdemCompra();
            //FuncionalidadeSac();
        }

        private void FuncionalidadeAdmin()
        {
            MenuAdmin = new System.Collections.Hashtable();

            MenuAdmin.Add(PerfilAcessar, "Perfil - acessar");
            MenuAdmin.Add(PerfilGravar, "Perfil - gravar");
            MenuAdmin.Add(PerfilDeletar, "Perfil - deletar");
            //MenuAdmin.Add(PerfilImprimir, "Perfil - imprimir");

            MenuAdmin.Add(UsuarioFuncionalidadeAcessar, "Usuário funcionalidades - acessar");
            MenuAdmin.Add(UsuarioFuncionalidadeGravar, "Usuário funcionalidades - gravar");
            MenuAdmin.Add(UsuarioFuncionalidadeDeletar, "Usuário funcionalidades - deletar");
            //MenuAdmin.Add(UsuarioFuncionalidadeImprimir, "Usuário funcionalidades - imprimir");
        }

        private void FuncionalidadeContrato()
        {
            MenuContrato = new System.Collections.Hashtable();

            MenuContrato.Add(LiberacaoAcessar, "Liberação - acessar");
            MenuContrato.Add(LiberacaoAprovarLiberar, "Liberação - aprovar e liberar");
            MenuContrato.Add(LiberacaoAprovar, "Liberação - aprovar");
            MenuContrato.Add(LiberacaoLiberar, "Liberação - liberar");
            MenuContrato.Add(LiberacaoCancelar, "Liberação - cancelar liberação");
            MenuContrato.Add(LiberacaoAssociarNF, "Liberação - associar nota fiscal");
            MenuContrato.Add(LiberacaoAlterarVencimento, "Liberação - alterar data de vencimento");
            MenuContrato.Add(LiberacaoImprimirMedicao, "Liberação - imprimir dados da medição");

            MenuContrato.Add(MedicaoAcessar, "Medição - acessar");
            MenuContrato.Add(MedicaoGravar, "Medição - gravar");
            MenuContrato.Add(MedicaoDeletar, "Medição - deletar");
            MenuContrato.Add(MedicaoImprimir, "Medição - imprimir");

            MenuContrato.Add(RelNotasFiscaisLiberadasAcessar, "Relatório de notas fiscais liberadas - acessar");
            MenuContrato.Add(RelNotasFiscaisLiberadasImprimir, "Relatório de notas fiscais liberadas - imprimir");
        }

        private void FuncionalidadeComercial()
        {
            MenuComercial = new System.Collections.Hashtable();

            MenuComercial.Add(RelStatusVendaAcessar , "Relatório de status da venda - acessar");
            MenuComercial.Add(RelStatusVendaImprimir, "Relatório de status da venda - imprimir");
        }

        private void FuncionalidadeFinanceiro()
        {
            MenuFinanceiro = new System.Collections.Hashtable();

            //MenuFinanceiro.Add(ParametroFinanceiroAcessar, "Parâmetros - acessar");
            //MenuFinanceiro.Add(ParametroFinanceiroGravar, "Parâmetros - gravar");

            //MenuFinanceiro.Add(ParametroUsuarioFinanceiroAcessar, "Parâmetros do usuário - acessar");
            //MenuFinanceiro.Add(ParametroUsuarioFinanceiroGravar, "Parâmetros do usuário - gravar");

            //MenuFinanceiro.Add(CaixaAcessar, "Caixa - acessar");
            //MenuFinanceiro.Add(CaixaGravar, "Caixa - gravar");
            //MenuFinanceiro.Add(CaixaDeletar, "Caixa - deletar");
            //MenuFinanceiro.Add(CaixaImprimir, "Caixa - imprimir");

            //MenuFinanceiro.Add(ImpostoFinanceiroAcessar, "Imposto financeiro - acessar");
            //MenuFinanceiro.Add(ImpostoFinanceiroGravar, "Imposto financeiro - gravar");
            //MenuFinanceiro.Add(ImpostoFinanceiroDeletar, "Imposto financeiro - deletar");
            //MenuFinanceiro.Add(ImpostoFinanceiroImprimir, "Imposto financeiro - imprimir");

            //MenuFinanceiro.Add(FormaRecebimentoAcessar, "Forma de recebimento - acessar");
            //MenuFinanceiro.Add(FormaRecebimentoGravar, "Forma de recebimento - gravar");
            //MenuFinanceiro.Add(FormaRecebimentoDeletar, "Forma de recebimento - deletar");
            //MenuFinanceiro.Add(FormaRecebimentoImprimir, "Forma de recebimento - imprimir");

            //MenuFinanceiro.Add(MotivoCancelamentoAcessar, "Motivo de cancelamento - acessar");
            //MenuFinanceiro.Add(MotivoCancelamentoGravar, "Motivo de cancelamento - gravar");
            //MenuFinanceiro.Add(MotivoCancelamentoDeletar, "Motivo de cancelamento - deletar");
            //MenuFinanceiro.Add(MotivoCancelamentoImprimir, "Motivo de cancelamento - imprimir");

            //MenuFinanceiro.Add(RateioAutomaticoAcessar, "Rateio automático - acessar");
            //MenuFinanceiro.Add(RateioAutomaticoGravar, "Rateio automático - gravar");
            //MenuFinanceiro.Add(RateioAutomaticoDeletar, "Rateio automático - deletar");
            //MenuFinanceiro.Add(RateioAutomaticoImprimir, "Rateio automático - imprimir");

            //MenuFinanceiro.Add(TabelaBasicaFinanceiroAcessar, "Tabelas básicas - acessar");
            //MenuFinanceiro.Add(TabelaBasicaFinanceiroGravar, "Tabelas básicas - gravar");
            //MenuFinanceiro.Add(TabelaBasicaFinanceiroDeletar, "Tabelas básicas - deletar");
            //MenuFinanceiro.Add(TabelaBasicaFinanceiroImprimir, "Tabelas básicas - imprimir");

            //MenuFinanceiro.Add(TaxaAdministracaoAcessar, "Taxa de administração - acessar");
            //MenuFinanceiro.Add(TaxaAdministracaoGravar, "Taxa de administração - gravar");
            //MenuFinanceiro.Add(TaxaAdministracaoDeletar, "Taxa de administração - deletar");
            //MenuFinanceiro.Add(TaxaAdministracaoImprimir, "Taxa de administração - imprimir");

            //MenuFinanceiro.Add(TipoCompromissoAcessar, "Tipo de compromisso - acessar");
            //MenuFinanceiro.Add(TipoCompromissoGravar, "Tipo de compromisso - gravar");
            //MenuFinanceiro.Add(TipoCompromissoDeletar, "Tipo de compromisso - deletar");
            //MenuFinanceiro.Add(TipoCompromissoImprimir, "Tipo de compromisso - imprimir");

            //MenuFinanceiro.Add(TipoDocumentoAcessar, "Tipo de documento - acessar");
            //MenuFinanceiro.Add(TipoDocumentoGravar, "Tipo de documento - gravar");
            //MenuFinanceiro.Add(TipoDocumentoDeletar, "Tipo de documento - deletar");
            //MenuFinanceiro.Add(TipoDocumentoImprimir, "Tipo de documento - Imprimir");

            //MenuFinanceiro.Add(TipoMovimentoAcessar, "Tipo movimento - acessar");
            //MenuFinanceiro.Add(TipoMovimentoGravar, "Tipo movimento - gravar");
            //MenuFinanceiro.Add(TipoMovimentoDeletar, "Tipo movimento - deletar");
            //MenuFinanceiro.Add(TipoMovimentoImprimir, "Tipo movimento - Imprimir");

            //MenuFinanceiro.Add(TipoRateioAcessar, "Tipo rateio - acessar");
            //MenuFinanceiro.Add(TipoRateioGravar, "Tipo rateio - gravar");
            //MenuFinanceiro.Add(TipoRateioDeletar, "Tipo rateio - deletar");
            //MenuFinanceiro.Add(TipoRateioImprimir, "Tipo rateio - Imprimir");

            //MenuFinanceiro.Add(BancoAcessar, "Banco - acessar");
            //MenuFinanceiro.Add(BancoGravar, "Banco - gravar");
            //MenuFinanceiro.Add(BancoDeletar, "Banco - deletar");
            //MenuFinanceiro.Add(BancoImprimir, "Banco - Imprimir");

            //MenuFinanceiro.Add(AgenciaAcessar, "Agência - acessar");
            //MenuFinanceiro.Add(AgenciaGravar, "Agência - gravar");
            //MenuFinanceiro.Add(AgenciaDeletar, "Agência - deletar");
            //MenuFinanceiro.Add(AgenciaImprimir, "Agência - imprimir");

            //MenuFinanceiro.Add(ContaCorrenteAcessar, "Conta corrente - acessar");
            //MenuFinanceiro.Add(ContaCorrenteGravar, "Conta corrente - gravar");
            //MenuFinanceiro.Add(ContaCorrenteDeletar, "Conta corrente - deletar");
            ////MenuFinanceiro.Add(ContaCorrenteImprimir, "Conta corrente - imprimir");

            MenuFinanceiro.Add(RelatorioApropriacaoPorClasseAcessar, "Relatório de apropriação por classe - acessar");
            MenuFinanceiro.Add(RelatorioApropriacaoPorClasseImprimir, "Relatório de apropriação por classe - imprimir");


            //MenuFinanceiro.Add(RelatorioContasAPagarTitulosAcessar, "Relatório de títulos a pagar - acessar");
        }

        private void FuncionalidadeOrdemCompra()
        {
            MenuOrdemCompra = new System.Collections.Hashtable();

            ////MenuOrdemCompra.Add(OrdemCompraMaterialAcessar, "Material - acessar");
            ////MenuOrdemCompra.Add(OrdemCompraMaterialGravar, "Material - gravar");
            ////MenuOrdemCompra.Add(OrdemCompraMaterialDeletar, "Material - deletar");
            ////MenuOrdemCompra.Add(OrdemCompraMaterialImprimir, "Material - Imprimir");

            //MenuOrdemCompra.Add(OrdemCompraUnidadeMedidaAcessar, "Unidade de medida - acessar");
            //MenuOrdemCompra.Add(OrdemCompraUnidadeMedidaGravar, "Unidade de medida - gravar");
            //MenuOrdemCompra.Add(OrdemCompraUnidadeMedidaDeletar, "Unidade de medida - deletar");
            //MenuOrdemCompra.Add(OrdemCompraUnidadeMedidaImprimir, "Unidade de medida - imprimir");

            //MenuOrdemCompra.Add(ParametroOrdemCompraAcessar, "Parâmetros - acessar");
            //MenuOrdemCompra.Add(ParametroOrdemCompraGravar, "Parâmetros - gravar");

            //MenuOrdemCompra.Add(ParametroUsuarioOrdemCompraAcessar, "Parâmetros do usuário - acessar");
            //MenuOrdemCompra.Add(ParametroUsuarioOrdemCompraGravar, "Parâmetros do usuário - gravar");

            MenuOrdemCompra.Add(PreRequisicaoMaterialAcessar, "Pré-Requisição de material - acessar");
            MenuOrdemCompra.Add(PreRequisicaoMaterialGravar, "Pré-Requisição de material - gravar");
            MenuOrdemCompra.Add(PreRequisicaoMaterialCancelar, "Pré-Requisição de material - cancelar");
            MenuOrdemCompra.Add(PreRequisicaoMaterialImprimir, "Pré-Requisição de material - imprimir");
            MenuOrdemCompra.Add(PreRequisicaoMaterialAprovar, "Pré-Requisição de material - aprovar");

            MenuOrdemCompra.Add(RequisicaoMaterialAcessar, "Requisição de material - acessar");
            MenuOrdemCompra.Add(RequisicaoMaterialGravar, "Requisição de material - gravar");
            MenuOrdemCompra.Add(RequisicaoMaterialCancelar, "Requisição de material - cancelar");
            MenuOrdemCompra.Add(RequisicaoMaterialImprimir, "Requisição de material - imprimir");
            MenuOrdemCompra.Add(RequisicaoMaterialAprovar, "Requisição de material - aprovar");
            MenuOrdemCompra.Add(RequisicaoMaterialCancelarAprovacao, "Requisição de material - cancelar aprovação");

            MenuOrdemCompra.Add(EntradaMaterialAcessar, "Entrada de material - acessar");
            MenuOrdemCompra.Add(EntradaMaterialGravar, "Entrada de material - gravar");
            MenuOrdemCompra.Add(EntradaMaterialCancelar, "Entrada de material - cancelar");
            MenuOrdemCompra.Add(EntradaMaterialImprimir, "Entrada de material - imprimir");

            MenuOrdemCompra.Add(RelatorioItensOrdemCompraAcessar, "Relatório de itens de ordem de compra - acessar");
            MenuOrdemCompra.Add(RelatorioItensOrdemCompraImprimir, "Relatório de itens de ordem de compra - imprimir");
        }

        //private void FuncionalidadeSac()
        //{
        //    MenuSac = new System.Collections.Hashtable();

        //    MenuSac.Add(ParametroSacAcessar, "Parâmetros - acessar");
        //    MenuSac.Add(ParametroSacGravar, "Parâmetros - gravar");

        //    MenuSac.Add(SetorSacAcessar, "Setor - acessar");
        //    MenuSac.Add(SetorSacGravar, "Setor - gravar");
        //    MenuSac.Add(SetorSacDeletar, "Setor - deletar");
        //    //MenuSac.Add(SetorSacImprimir, "Setor - imprimir");
        //}
    }
}