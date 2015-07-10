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
        public System.Collections.Hashtable MenuFinanceiro;
        public System.Collections.Hashtable MenuOrdemCompra;
        public System.Collections.Hashtable MenuSac;

        #endregion

        #region Admin

        public const string PerfilAcessar = "PERFIL_ACESSAR";
        public const string PerfilAcessarDescricao = "Perfil - acessar";
        public const string PerfilGravar = "PERFIL_GRAVAR";
        public const string PerfilGravarDescricao = "Perfil - gravar";
        public const string PerfilDeletar = "PERFIL_DELETAR";
        public const string PerfilDeletarDescricao = "Perfil - deletar";
        public const string PerfilImprimir = "PERFIL_IMPRIMIR";
        public const string PerfilImprimirDescricao = "Perfil - imprimir";

        public const string UsuarioFuncionalidadeAcessar = "USUARIOFUNCIONALIDADE_ACESSAR";
        public const string UsuarioFuncionalidadeAcessarDescricao = "Usuário funcionalidades - acessar";
        public const string UsuarioFuncionalidadeGravar = "USUARIOFUNCIONALIDADE_GRAVAR";
        public const string UsuarioFuncionalidadeGravarDescricao = "Usuário funcionalidades - gravar";
        public const string UsuarioFuncionalidadeDeletar = "USUARIOFUNCIONALIDADE_DELETAR";
        public const string UsuarioFuncionalidadeDeletarDescricao = "Usuário funcionalidades - deletar";
        public const string UsuarioFuncionalidadeImprimir = "USUARIOFUNCIONALIDADE_IMPRIMIR";
        public const string UsuarioFuncionalidadeImprimirDescricao = "Usuário funcionalidades - imprimir";

        #endregion

        #region Contrato

        public const string MedicaoAcessar = "MEDICAO_ACESSAR";
        public const string MedicaoAcessarDescricao = "Medição - acessar";
        public const string MedicaoGravar = "MEDICAO_GRAVAR";
        public const string MedicaoGravarDescricao = "Medição - gravar";
        public const string MedicaoDeletar = "MEDICAO_DELETAR";
        public const string MedicaoDeletarDescricao = "Medição - deletar";
        public const string MedicaoImprimir = "MEDICAO_IMPRIMIR";
        public const string MedicaoImprimirDescricao = "Medição - imprimir";

        public const string RelNotasFiscaisLiberadasAcessar = "REL_NF_LIBERADAS_ACESSAR";
        public const string RelNotasFiscaisLiberadasAcessarDescricao = "Relatório de notas fiscais liberadas - acessar";
        public const string RelNotasFiscaisLiberadasImprimir = "REL_NF_LIBERADAS_IMPRIMIR";
        public const string RelNotasFiscaisLiberadasImprimirDescricao = "Relatório de notas fiscais liberadas - imprimir";
        
        #endregion

        #region Financeiro

        public const string ParametroFinanceiroAcessar = "PARAMETRO_FINANCEIRO_ACESSAR";
        public const string ParametroFinanceiroAcessarDescricao = "Parâmetros - acessar";
        public const string ParametroFinanceiroGravar = "PARAMETRO_FINANCEIRO_GRAVAR";
        public const string ParametroFinanceiroGravarDescricao = "Parâmetros - gravar";

        public const string ParametroUsuarioFinanceiroAcessar = "PARAMETROUSUARIO_FINANCEIRO_ACESSAR";
        public const string ParametroUsuarioFinanceiroAcessarDescricao = "Parâmetros do usuário - acessar";
        public const string ParametroUsuarioFinanceiroGravar = "PARAMETROUSUARIO_FINANCEIRO_GRAVAR";
        public const string ParametroUsuarioFinanceiroGravarDescricao = "Parâmetros do usuário - gravar";

        public const string CaixaAcessar = "CAIXA_ACESSAR";
        public const string CaixaAcessarDescricao = "Caixa - acessar";
        public const string CaixaGravar = "CAIXA_GRAVAR";
        public const string CaixaGravarDescricao = "Caixa - gravar";
        public const string CaixaDeletar = "CAIXA_DELETAR";
        public const string CaixaDeletarDescricao = "Caixa - deletar";
        public const string CaixaImprimir = "CAIXA_IMPRIMIR";
        public const string CaixaImprimirDescricao = "Caixa - imprimir";

        public const string ImpostoFinanceiroAcessar = "IMPOSTOFINANCEIRO_ACESSAR";
        public const string ImpostoFinanceiroAcessarDescricao = "Imposto financeiro - acessar";
        public const string ImpostoFinanceiroGravar = "IMPOSTOFINANCEIRO_GRAVAR";
        public const string ImpostoFinanceiroGravarDescricao = "Imposto financeiro - gravar";
        public const string ImpostoFinanceiroDeletar = "IMPOSTOFINANCEIRO_DELETAR";
        public const string ImpostoFinanceiroDeletarDescricao = "Imposto financeiro - deletar";
        public const string ImpostoFinanceiroImprimir = "IMPOSTOFINANCEIRO_IMPRIMIR";
        public const string ImpostoFinanceiroImprimirDescricao = "Imposto financeiro - imprimir";

        public const string FormaRecebimentoAcessar = "FORMARECEBIMENTO_ACESSAR";
        public const string FormaRecebimentoAcessarDescricao = "Forma de recebimento - acessar";
        public const string FormaRecebimentoGravar = "FORMARECEBIMENTO_GRAVAR";
        public const string FormaRecebimentoGravarDescricao = "Forma de recebimento - gravar";
        public const string FormaRecebimentoDeletar = "FORMARECEBIMENTO_DELETAR";
        public const string FormaRecebimentoDeletarDescricao = "Forma de recebimento - deletar";
        public const string FormaRecebimentoImprimir = "FORMARECEBIMENTO_IMPRIMIR";
        public const string FormaRecebimentoImprimirDescricao = "Forma de recebimento - imprimir";

        public const string MotivoCancelamentoAcessar = "MOTIVOCANCELAMENTO_ACESSAR";
        public const string MotivoCancelamentoAcessarDescricao = "Motivo de cancelamento - acessar";
        public const string MotivoCancelamentoGravar = "MOTIVOCANCELAMENTO_GRAVAR";
        public const string MotivoCancelamentoGravarDescricao = "Motivo de cancelamento - gravar";
        public const string MotivoCancelamentoDeletar = "MOTIVOCANCELAMENTO_DELETAR";
        public const string MotivoCancelamentoDeletarDescricao = "Motivo de cancelamento - deletar";
        public const string MotivoCancelamentoImprimir = "UMOTIVOCANCELAMENTO_IMPRIMIR";
        public const string MotivoCancelamentoImprimirDescricao = "Motivo de cancelamento - imprimir";

        public const string RateioAutomaticoAcessar = "RATEIOAUTOMATICO_ACESSAR";
        public const string RateioAutomaticoAcessarDescricao = "Rateio automático - acessar";
        public const string RateioAutomaticoGravar = "RATEIOAUTOMATICO_GRAVAR";
        public const string RateioAutomaticoGravarDescricao = "Rateio automático - gravar";
        public const string RateioAutomaticoDeletar = "RATEIOAUTOMATICO_DELETAR";
        public const string RateioAutomaticoDeletarDescricao = "Rateio automático - deletar";
        public const string RateioAutomaticoImprimir = "RATEIOAUTOMATICO_IMPRIMIR";
        public const string RateioAutomaticoImprimirDescricao = "Rateio automático - imprimir";
            
        public const string TabelaBasicaFinanceiroAcessar = "TABELABASICA_FINANCEIRO_ACESSAR";
        public const string TabelaBasicaFinanceiroAcessarDescricao = "Tabelas básicas - acessar";
        public const string TabelaBasicaFinanceiroGravar = "TABELABASICA_FINANCEIRO_GRAVAR";
        public const string TabelaBasicaFinanceiroGravarDescricao = "Tabelas básicas - gravar";
        public const string TabelaBasicaFinanceiroDeletar = "TABELABASICA_FINANCEIRO_DELETAR";
        public const string TabelaBasicaFinanceiroDeletarDescricao = "Tabelas básicas - deletar";
        public const string TabelaBasicaFinanceiroImprimir = "TABELABASICA_FINANCEIRO_IMPRIMIR";
        public const string TabelaBasicaFinanceiroImprimirDescricao = "Tabelas básicas - imprimir";
   
        public const string TaxaAdministracaoAcessar = "TAXAADMINISTRACAO_ACESSAR";
        public const string TaxaAdministracaoAcessarDescricao = "Taxa de administração - acessar";
        public const string TaxaAdministracaoGravar = "TAXAADMINISTRACAO_GRAVAR";
        public const string TaxaAdministracaoGravarDescricao = "Taxa de administração - gravar";
        public const string TaxaAdministracaoDeletar = "TAXAADMINISTRACAO_DELETAR";
        public const string TaxaAdministracaoDeletarDescricao = "Taxa de administração - deletar";
        public const string TaxaAdministracaoImprimir = "TAXAADMINISTRACAO_IMPRIMIR";
        public const string TaxaAdministracaoImprimirDescricao = "Taxa de administração - imprimir";

        public const string TipoCompromissoAcessar = "TIPOCOMPROMISSO_ACESSAR";
        public const string TipoCompromissoAcessarDescricao = "Tipo de compromisso - acessar";
        public const string TipoCompromissoGravar = "TIPOCOMPROMISSO_GRAVAR";
        public const string TipoCompromissoGravarDescricao = "Tipo de compromisso - gravar";
        public const string TipoCompromissoDeletar = "TIPOCOMPROMISSO_DELETAR";
        public const string TipoCompromissoDeletarDescricao = "Tipo de compromisso - deletar";
        public const string TipoCompromissoImprimir = "TIPOCOMPROMISSO_IMPRIMIR";
        public const string TipoCompromissoImprimirDescricao = "Tipo de compromisso - imprimir";

        public const string TipoDocumentoAcessar = "TIPODOCUMENTO_ACESSAR";
        public const string TipoDocumentoAcessarDescricao = "Tipo de documento - acessar";
        public const string TipoDocumentoGravar = "TIPODOCUMENTO_GRAVAR";
        public const string TipoDocumentoGravarDescricao = "Tipo de documento - gravar";
        public const string TipoDocumentoDeletar = "TIPODOCUMENTO_DELETAR";
        public const string TipoDocumentoDeletarDescricao = "Tipo de documento - deletar";
        public const string TipoDocumentoImprimir = "TIPODOCUMENTO_IMPRIMIR";
        public const string TipoDocumentoImprimirDescricao = "Tipo de documento - Imprimir";

        public const string TipoRateioAcessar = "TIPORATEIO_ACESSAR";
        public const string TipoRateioAcessarDescricao = "Tipo rateio - acessar";
        public const string TipoRateioGravar = "TIPORATEIO_GRAVAR";
        public const string TipoRateioGravarDescricao = "Tipo rateio - gravar";
        public const string TipoRateioDeletar = "TIPORATEIO_DELETAR";
        public const string TipoRateiolDeletarDescricao = "Tipo rateio - deletar";
        public const string TipoRateioImprimir = "TIPORATEIO_IMPRIMIR";
        public const string TipoRateioImprimirDescricao = "Tipo rateio - Imprimir";
            
        public const string BancoAcessar = "BANCO_ACESSAR";
        public const string BancoAcessarDescricao = "Banco - acessar";
        public const string BancoGravar = "BANCO_GRAVAR";
        public const string BancoGravarDescricao = "Banco - gravar";
        public const string BancoDeletar = "BANCO_DELETAR";
        public const string BancoDeletarDescricao = "Banco - deletar";
        public const string BancoImprimir = "BANCO_IMPRIMIR";
        public const string BancoImprimirDescricao = "Banco - Imprimir";

        public const string AgenciaAcessar = "AGENCIA_ACESSAR";
        public const string AgenciaAcessarDescricao = "Agência - acessar";
        public const string AgenciaGravar = "AGENCIA_GRAVAR";
        public const string AgenciaGravarDescricao = "Agência - gravar";
        public const string AgenciaDeletar = "AGENCIA_DELETAR";
        public const string AgenciaDeletarDescricao = "Agência - deletar";
        public const string AgenciaImprimir = "AGENCIA_IMPRIMIR";
        public const string AgenciaImprimirDescricao = "Agência - imprimir";

        public const string ContaCorrenteAcessar = "CONTACORRENTE_ACESSAR";
        public const string ContaCorrenteAcessarDescricao = "Conta corrente - acessar";
        public const string ContaCorrenteGravar = "CONTACORRENTE_GRAVAR";
        public const string ContaCorrenteGravarDescricao = "Conta corrente - gravar";
        public const string ContaCorrenteDeletar = "CONTACORRENTE_DELETAR";
        public const string ContaCorrenteDeletarDescricao = "Conta corrente - deletar";
        public const string ContaCorrenteImprimir = "CONTACORRENTE_IMPRIMIR";
        public const string ContaCorrenteImprimirDescricao = "Conta corrente - imprimir";
           
        #endregion

        #region Comuns

        public const string UnidadeMedidaAcessar = "UNIDADEMEDIDA_ACESSAR";
        public const string UnidadeMedidaAcessarDescricao = "Unidade de medida - acessar";
        public const string UnidadeMedidaGravar = "UNIDADEMEDIDA_GRAVAR";
        public const string UnidadeMedidaGravarDescricao = "Unidade de medida - gravar";
        public const string UnidadeMedidaDeletar = "UNIDADEMEDIDA_DELETAR";
        public const string UnidadeMedidaDeletarDescricao = "Unidade de medida - deletar";
        public const string UnidadeMedidaImprimir = "UNIDADEMEDIDA_IMPRIMIR";
        public const string UnidadeMedidaImprimirDescricao = "Unidade de medida - imprimir";

        public const string MaterialAcessar = "MATERIAL_ACESSAR";
        public const string MaterialAcessarDescricao = "Material - acessar";
        public const string MaterialGravar = "MATERIAL_GRAVAR";
        public const string MaterialGravarDescricao = "Material - gravar";
        public const string MaterialDeletar = "MATERIAL_DELETAR";
        public const string MaterialDeletarDescricao = "Material - deletar";
        public const string MaterialImprimir = "MATERIAL_IMPRIMIR";
        public const string MaterialImprimirDescricao = "Material - Imprimir";

        #endregion

        #region OrdemCompra

        public const string ParametroOrdemCompraAcessar = "PARAMETRO_ORDEMCOMPRA_ACESSAR";
        public const string ParametroOrdemCompraAcessarDescricao = "Parâmetros - acessar";
        public const string ParametroOrdemCompraGravar = "PARAMETRO_ORDEMCOMPRA_GRAVAR";
        public const string ParametroOrdemCompraGravarDescricao = "Parâmetros - gravar";

        public const string ParametroUsuarioOrdemCompraAcessar = "PARAMETRO_USUARIO_ORDEMCOMPRA_ACESSAR";
        public const string ParametroUsuarioOrdemCompraAcessarDescricao = "Parâmetros do usuário - acessar";
        public const string ParametroUsuarioOrdemCompraGravar = "PARAMETRO_USUARIO_ORDEMCOMPRA_GRAVAR";
        public const string ParametroUsuarioOrdemCompraGravarDescricao = "Parâmetros do usuário - gravar";

        public const string PreRequisicaoMaterialAcessar = "PREREQUISICAOMATERIAL_ACESSAR";
        public const string PreRequisicaoMaterialAcessarDescricao = "Pré-Requisição de material - acessar";
        public const string PreRequisicaoMaterialGravar = "PREREQUISICAOMATERIAL_GRAVAR";
        public const string PreRequisicaoMaterialGravarDescricao = "Pré-Requisição de material - gravar";
        public const string PreRequisicaoMaterialCancelar = "PREREQUISICAOMATERIAL_CANCELAR";
        public const string PreRequisicaoMaterialCancelarDescricao = "Pré-Requisição de material - cancelar";
        public const string PreRequisicaoMaterialImprimir = "PREREQUISICAOMATERIAL_IMPRIMIR";
        public const string PreRequisicaoMaterialImprimirDescricao = "Pré-Requisição de material - imprimir";
        public const string PreRequisicaoMaterialAprovar = "PREREQUISICAOMATERIAL_APROVAR";
        public const string PreRequisicaoMaterialAprovarDescricao = "Pré-Requisição de material - aprovar";

        public const string RequisicaoMaterialAcessar = "REQUISICAOMATERIAL_ACESSAR";
        public const string RequisicaoMaterialAcessarDescricao = "Requisição de material - acessar";
        public const string RequisicaoMaterialGravar = "REQUISICAOMATERIAL_GRAVAR";
        public const string RequisicaoMaterialGravarDescricao = "Requisição de material - gravar";
        public const string RequisicaoMaterialCancelar = "REQUISICAOMATERIAL_CANCELAR";
        public const string RequisicaoMaterialCancelarDescricao = "Requisição de material - cancelar";
        public const string RequisicaoMaterialImprimir = "REQUISICAOMATERIAL_IMPRIMIR";
        public const string RequisicaoMaterialImprimirDescricao = "Requisição de material - imprimir";
        public const string RequisicaoMaterialAprovar = "REQUISICAOMATERIAL_APROVAR";
        public const string RequisicaoMaterialAprovarDescricao = "Requisição de material - aprovar";
        public const string RequisicaoMaterialCancelarAprovacao = "REQUISICAOMATERIAL_CANCELAR_APROVACAO";
        public const string RequisicaoMaterialCancelarAprovacaoDescricao = "Requisição de material - cancelar aprovação";

        #endregion

        #region Sac

        public const string ParametroSacAcessar = "PARAMETRO_SAC_ACESSAR";
        public const string ParametroSacAcessarDescricao = "Parâmetros - acessar";
        public const string ParametroSacGravar = "PARAMETRO_SAC_GRAVAR";
        public const string ParametroSacGravarDescricao = "Parâmetros - gravar";

        public const string SetorSacAcessar = "SETOR_SAC_ACESSAR";
        public const string SetorSacAcessarDescricao = "Setor - acessar";
        public const string SetorSacGravar = "SETOR_SAC_GRAVAR";
        public const string SetorSacGravarDescricao = "Setor - gravar";
        public const string SetorSacDeletar = "SETOR_SAC_DELETAR";
        public const string SetorSacDeletarDescricao = "Setor - deletar";
        public const string SetorSacImprimir = "SETOR_SAC_IMPRIMIR";
        public const string SetorSacImprimirDescricao = "Setor - imprimir";

        #endregion

        public Funcionalidade()
        {
            FuncionalidadeAdmin();
            FuncionalidadeContrato();
            FuncionalidadeFinanceiro();
            FuncionalidadeOrdemCompra();
            FuncionalidadeSac();
        }

        private void FuncionalidadeAdmin()
        {
            MenuAdmin = new System.Collections.Hashtable();

            MenuAdmin.Add(PerfilAcessar, PerfilAcessarDescricao);
            MenuAdmin.Add(PerfilGravar, PerfilGravarDescricao);
            MenuAdmin.Add(PerfilDeletar, PerfilDeletarDescricao);
            MenuAdmin.Add(PerfilImprimir, PerfilImprimirDescricao);

            MenuAdmin.Add(UsuarioFuncionalidadeAcessar, UsuarioFuncionalidadeAcessarDescricao);
            MenuAdmin.Add(UsuarioFuncionalidadeGravar, UsuarioFuncionalidadeGravarDescricao);
            MenuAdmin.Add(UsuarioFuncionalidadeDeletar, UsuarioFuncionalidadeDeletarDescricao);
            MenuAdmin.Add(UsuarioFuncionalidadeImprimir, UsuarioFuncionalidadeImprimirDescricao);

        }

        private void FuncionalidadeContrato()
        {
            MenuContrato = new System.Collections.Hashtable();

            MenuContrato.Add(MedicaoAcessar, MedicaoAcessarDescricao);
            MenuContrato.Add(MedicaoGravar, MedicaoGravarDescricao);
            MenuContrato.Add(MedicaoDeletar, MedicaoDeletarDescricao);
            MenuContrato.Add(MedicaoImprimir, MedicaoImprimirDescricao);

            MenuContrato.Add(RelNotasFiscaisLiberadasAcessar, RelNotasFiscaisLiberadasAcessarDescricao);
            MenuContrato.Add(RelNotasFiscaisLiberadasImprimir, RelNotasFiscaisLiberadasImprimirDescricao);

        }

        private void FuncionalidadeFinanceiro()
        {
            MenuFinanceiro = new System.Collections.Hashtable();

            MenuFinanceiro.Add(ParametroFinanceiroAcessar, ParametroFinanceiroAcessarDescricao);
            MenuFinanceiro.Add(ParametroFinanceiroGravar, ParametroFinanceiroGravarDescricao);

            MenuFinanceiro.Add(ParametroUsuarioFinanceiroAcessar, ParametroUsuarioFinanceiroAcessarDescricao);
            MenuFinanceiro.Add(ParametroUsuarioFinanceiroGravar, ParametroUsuarioFinanceiroGravarDescricao);

            MenuFinanceiro.Add(CaixaAcessar, CaixaAcessarDescricao);
            MenuFinanceiro.Add(CaixaGravar, CaixaGravarDescricao);
            MenuFinanceiro.Add(CaixaDeletar, CaixaDeletarDescricao);
            MenuFinanceiro.Add(CaixaImprimir, CaixaImprimirDescricao);

            MenuFinanceiro.Add(ImpostoFinanceiroAcessar, ImpostoFinanceiroAcessarDescricao);
            MenuFinanceiro.Add(ImpostoFinanceiroGravar, ImpostoFinanceiroGravarDescricao);
            MenuFinanceiro.Add(ImpostoFinanceiroDeletar, ImpostoFinanceiroDeletarDescricao);
            MenuFinanceiro.Add(ImpostoFinanceiroImprimir, ImpostoFinanceiroImprimirDescricao);

            MenuFinanceiro.Add(FormaRecebimentoAcessar, FormaRecebimentoAcessarDescricao);
            MenuFinanceiro.Add(FormaRecebimentoGravar, FormaRecebimentoGravarDescricao);
            MenuFinanceiro.Add(FormaRecebimentoDeletar, FormaRecebimentoDeletarDescricao);
            MenuFinanceiro.Add(FormaRecebimentoImprimir, FormaRecebimentoImprimirDescricao);

            MenuFinanceiro.Add(MotivoCancelamentoAcessar, MotivoCancelamentoAcessarDescricao);
            MenuFinanceiro.Add(MotivoCancelamentoGravar, MotivoCancelamentoGravarDescricao);
            MenuFinanceiro.Add(MotivoCancelamentoDeletar, MotivoCancelamentoDeletarDescricao);
            MenuFinanceiro.Add(MotivoCancelamentoImprimir, MotivoCancelamentoImprimirDescricao);

            MenuFinanceiro.Add(RateioAutomaticoAcessar, RateioAutomaticoAcessarDescricao);
            MenuFinanceiro.Add(RateioAutomaticoGravar, RateioAutomaticoGravarDescricao);
            MenuFinanceiro.Add(RateioAutomaticoDeletar, RateioAutomaticoDeletarDescricao);
            MenuFinanceiro.Add(RateioAutomaticoImprimir, RateioAutomaticoImprimirDescricao);

            MenuFinanceiro.Add(TabelaBasicaFinanceiroAcessar, TabelaBasicaFinanceiroAcessarDescricao);
            MenuFinanceiro.Add(TabelaBasicaFinanceiroGravar, TabelaBasicaFinanceiroGravarDescricao);
            MenuFinanceiro.Add(TabelaBasicaFinanceiroDeletar, TabelaBasicaFinanceiroDeletarDescricao);
            MenuFinanceiro.Add(TabelaBasicaFinanceiroImprimir, TabelaBasicaFinanceiroImprimirDescricao);

            MenuFinanceiro.Add(TaxaAdministracaoAcessar, TaxaAdministracaoAcessarDescricao);
            MenuFinanceiro.Add(TaxaAdministracaoGravar, TaxaAdministracaoGravarDescricao);
            MenuFinanceiro.Add(TaxaAdministracaoDeletar, TaxaAdministracaoDeletarDescricao);
            MenuFinanceiro.Add(TaxaAdministracaoImprimir, TaxaAdministracaoImprimirDescricao);

            MenuFinanceiro.Add(TipoCompromissoAcessar, TipoCompromissoAcessarDescricao);
            MenuFinanceiro.Add(TipoCompromissoGravar, TipoCompromissoGravarDescricao);
            MenuFinanceiro.Add(TipoCompromissoDeletar, TipoCompromissoDeletarDescricao);
            MenuFinanceiro.Add(TipoCompromissoImprimir, TipoCompromissoImprimirDescricao);

            MenuFinanceiro.Add(TipoDocumentoAcessar, TipoDocumentoAcessarDescricao);
            MenuFinanceiro.Add(TipoDocumentoGravar, TipoDocumentoGravarDescricao);
            MenuFinanceiro.Add(TipoDocumentoDeletar, TipoDocumentoDeletarDescricao);
            MenuFinanceiro.Add(TipoDocumentoImprimir, TipoDocumentoImprimirDescricao);

            MenuFinanceiro.Add(TipoRateioAcessar, TipoRateioAcessarDescricao);
            MenuFinanceiro.Add(TipoRateioGravar, TipoRateioGravarDescricao);
            MenuFinanceiro.Add(TipoRateioDeletar, TipoRateiolDeletarDescricao);
            MenuFinanceiro.Add(TipoRateioImprimir, TipoRateioImprimirDescricao);

            MenuFinanceiro.Add(BancoAcessar, BancoAcessarDescricao);
            MenuFinanceiro.Add(BancoGravar, BancoGravarDescricao);
            MenuFinanceiro.Add(BancoDeletar, BancoDeletarDescricao);
            MenuFinanceiro.Add(BancoImprimir, BancoImprimirDescricao);

            MenuFinanceiro.Add(AgenciaAcessar, AgenciaAcessarDescricao);
            MenuFinanceiro.Add(AgenciaGravar, AgenciaGravarDescricao);
            MenuFinanceiro.Add(AgenciaDeletar, AgenciaDeletarDescricao);
            MenuFinanceiro.Add(AgenciaImprimir, AgenciaImprimirDescricao);

            MenuFinanceiro.Add(ContaCorrenteAcessar, ContaCorrenteAcessarDescricao);
            MenuFinanceiro.Add(ContaCorrenteGravar, ContaCorrenteGravarDescricao);
            MenuFinanceiro.Add(ContaCorrenteDeletar, ContaCorrenteDeletarDescricao);
            MenuFinanceiro.Add(ContaCorrenteImprimir, ContaCorrenteImprimirDescricao);

        }

        private void FuncionalidadeOrdemCompra()
        {
            MenuOrdemCompra = new System.Collections.Hashtable();

            MenuOrdemCompra.Add(MaterialAcessar, MaterialAcessarDescricao);
            MenuOrdemCompra.Add(MaterialGravar, MaterialGravarDescricao);
            MenuOrdemCompra.Add(MaterialDeletar, MaterialDeletarDescricao);
            MenuOrdemCompra.Add(MaterialImprimir, MaterialImprimirDescricao);

            MenuOrdemCompra.Add(UnidadeMedidaAcessar, UnidadeMedidaAcessarDescricao);
            MenuOrdemCompra.Add(UnidadeMedidaGravar, UnidadeMedidaGravarDescricao);
            MenuOrdemCompra.Add(UnidadeMedidaDeletar, UnidadeMedidaDeletarDescricao);
            MenuOrdemCompra.Add(UnidadeMedidaImprimir, UnidadeMedidaImprimirDescricao);

            MenuOrdemCompra.Add(ParametroOrdemCompraAcessar, ParametroOrdemCompraAcessarDescricao);
            MenuOrdemCompra.Add(ParametroOrdemCompraGravar, ParametroOrdemCompraGravarDescricao);

            MenuOrdemCompra.Add(ParametroUsuarioOrdemCompraAcessar, ParametroUsuarioOrdemCompraAcessarDescricao);
            MenuOrdemCompra.Add(ParametroUsuarioOrdemCompraGravar, ParametroUsuarioOrdemCompraGravarDescricao);

            MenuOrdemCompra.Add(PreRequisicaoMaterialAcessar, PreRequisicaoMaterialAcessarDescricao);
            MenuOrdemCompra.Add(PreRequisicaoMaterialGravar, PreRequisicaoMaterialGravarDescricao);
            MenuOrdemCompra.Add(PreRequisicaoMaterialCancelar, PreRequisicaoMaterialCancelarDescricao);
            MenuOrdemCompra.Add(PreRequisicaoMaterialImprimir, PreRequisicaoMaterialImprimirDescricao);
            MenuOrdemCompra.Add(PreRequisicaoMaterialAprovar, PreRequisicaoMaterialAprovarDescricao);

            MenuOrdemCompra.Add(RequisicaoMaterialAcessar, RequisicaoMaterialAcessarDescricao);
            MenuOrdemCompra.Add(RequisicaoMaterialGravar, RequisicaoMaterialGravarDescricao);
            MenuOrdemCompra.Add(RequisicaoMaterialCancelar, RequisicaoMaterialCancelarDescricao);
            MenuOrdemCompra.Add(RequisicaoMaterialImprimir, RequisicaoMaterialImprimirDescricao);
            MenuOrdemCompra.Add(RequisicaoMaterialAprovar, RequisicaoMaterialAprovarDescricao);
            MenuOrdemCompra.Add(RequisicaoMaterialCancelarAprovacao, RequisicaoMaterialCancelarAprovacaoDescricao);
        }

        private void FuncionalidadeSac()
        {
            MenuSac = new System.Collections.Hashtable();

            MenuSac.Add(ParametroSacAcessar, ParametroSacAcessarDescricao);
            MenuSac.Add(ParametroSacGravar, ParametroSacGravarDescricao);

            MenuSac.Add(SetorSacAcessar, SetorSacAcessarDescricao);
            MenuSac.Add(SetorSacGravar, SetorSacGravarDescricao);
            MenuSac.Add(SetorSacDeletar, SetorSacDeletarDescricao);
            MenuSac.Add(SetorSacImprimir, SetorSacImprimirDescricao);
            
        }

    }
}
