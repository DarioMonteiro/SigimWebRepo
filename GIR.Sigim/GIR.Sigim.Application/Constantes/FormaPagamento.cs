using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.Constantes
{
    public class FormaPagamento
    {

        #region Hashtable

        public System.Collections.Hashtable MenuFormaPagamento;

        #endregion

        #region Contantes

        public const string FormaPagamentoAutomatico = "FORMAPAGAMENTO_AUTOMATICO";
        public const string FormaPagamentoBordero = "FORMAPAGAMENTO_BORDERO";
        public const string FormaPagamentoBorderoEletronico = "FORMAPAGAMENTO_BORDERO_ELETRONICO";
        public const string FormaPagamentoCheque = "FORMAPAGAMENTO_CHEQUE";
        public const string FormaPagamentoDinheiro = "FORMAPAGAMENTO_DINHEIRO";
        public const string FormaPagamentoOperacaoBancaria = "FORMAPAGAMENTO_OPERACAO_BANCARIA";

        #endregion

        #region "Construtor"

        public FormaPagamento()
        {
            FormaPagamentoMenu();
        }

        #endregion

        #region "Métodos Privados"

        private void FormaPagamentoMenu()
        {

            MenuFormaPagamento = new System.Collections.Hashtable();

            MenuFormaPagamento.Add(FormaPagamentoAutomatico, "Automático");
            MenuFormaPagamento.Add(FormaPagamentoBordero, "Borderô");
            MenuFormaPagamento.Add(FormaPagamentoBorderoEletronico, "Borderô eletrônico");
            MenuFormaPagamento.Add(FormaPagamentoCheque, "Cheque");
            MenuFormaPagamento.Add(FormaPagamentoDinheiro, "Dinheiro");
            MenuFormaPagamento.Add(FormaPagamentoOperacaoBancaria, "Operação bancária");

        }

        #endregion
    }
}
