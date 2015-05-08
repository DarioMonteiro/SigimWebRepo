using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Domain.Entity.Contrato
{
    public class ContratoRetificacaoItemCronograma : BaseEntity
    {
        public int ContratoId { get; set; }
        public Contrato Contrato { get; set; }
        public int ContratoRetificacaoId { get; set; }
        public ContratoRetificacao ContratoRetificacao { get; set; }
        public int ContratoRetificacaoItemId { get; set; }
        public ContratoRetificacaoItem ContratoRetificacaoItem { get; set; }
        public int Sequencial { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public DateTime DataVencimento { get; set; }
        public decimal Quantidade { get; set; }
        public decimal PercentualExecucao { get; set; }
        public decimal Valor { get; set; }

        public ICollection<ContratoRetificacaoProvisao> ListaContratoRetificacaoProvisao { get; set; }
        public ICollection<ContratoRetificacaoItemMedicao> ListaContratoRetificacaoItemMedicao { get; set; }

        public ContratoRetificacaoItemCronograma()
        {
            ListaContratoRetificacaoProvisao = new HashSet<ContratoRetificacaoProvisao>();
            ListaContratoRetificacaoItemMedicao = new HashSet<ContratoRetificacaoItemMedicao>();
        }
    }
}
