using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Domain.Entity.OrdemCompra;
namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class ImpostoFinanceiro : BaseEntity
    {
        public string Sigla { get; set; }
        public string Descricao { get; set; }
        public decimal Aliquota { get; set; }
        public bool? Retido { get; set; }
        public bool? Indireto { get; set; }
        public bool? PagamentoEletronico { get; set; }
        public TipoCompromisso TipoCompromisso { get; set; }
        public int? TipoCompromissoId { get; set; }
        public ClienteFornecedor Cliente { get; set; }
        public int? ClienteId { get; set; }
        public string ContaContabil { get; set; }
        public int? Periodicidade { get; set; }
        public short? DiaVencimento { get; set; }
        public int? FimDeSemana { get; set; }
        public int? FatoGerador { get; set; }

        public ICollection<ContratoRetificacaoItemImposto> ListaContratoRetificacaoItemImposto { get; set; }
        public ICollection<EntradaMaterialImposto> ListaEntradaMaterialImposto { get; set; }

        public ImpostoFinanceiro()
        {
            this.ListaContratoRetificacaoItemImposto = new HashSet<ContratoRetificacaoItemImposto>();
            this.ListaEntradaMaterialImposto = new HashSet<EntradaMaterialImposto>();
        }
    }
}