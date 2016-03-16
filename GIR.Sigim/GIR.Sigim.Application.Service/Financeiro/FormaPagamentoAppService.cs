using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.Constantes;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public class FormaPagamentoAppService : BaseAppService, IFormaPagamentoAppService
    {
        public FormaPagamentoAppService(MessageQueue messageQueue)
            : base(messageQueue)
        {
        }

        #region IFormaPagamentoAppService Métodos

        public List<FormaPagamentoDTO> ListaFormaPagamento()
        {
            List<FormaPagamentoDTO> listaFormaPagamento = new List<FormaPagamentoDTO>();
            FormaPagamento formaPagamento = new FormaPagamento();
            System.Collections.Hashtable Menu = new System.Collections.Hashtable();

            Menu = formaPagamento.MenuFormaPagamento;

            listaFormaPagamento = TransformaEmFormaPagamentoDTO(Menu);

            return listaFormaPagamento;

        }

        private List<FormaPagamentoDTO> TransformaEmFormaPagamentoDTO(System.Collections.Hashtable Menu)
        {
            FormaPagamentoDTO formaPagamento = new FormaPagamentoDTO();
            List<FormaPagamentoDTO> listaFormaPagamento = new List<FormaPagamentoDTO>();

            foreach (var item in Menu.Keys)
            {
                formaPagamento = new FormaPagamentoDTO();
                formaPagamento.Codigo = item.ToString();
                formaPagamento.Descricao = Menu[item].ToString();
                listaFormaPagamento.Add(formaPagamento);
            }

            listaFormaPagamento = listaFormaPagamento.OrderBy(l => l.Codigo).ToList<FormaPagamentoDTO>();
            return listaFormaPagamento;

        }

        #endregion

    }
}
