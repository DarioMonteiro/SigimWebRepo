using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Entity.Contrato;

namespace GIR.Sigim.Domain.Entity.Estoque
{
    public class Movimento : BaseEntity
    {
        public int? EstoqueId { get; set; }
        public Estoque Estoque { get; set; }
        public int? ClienteFornecedorId { get; set; }
        public ClienteFornecedor ClienteFornecedor { get; set; }
        public int? MovimentoPaiId { get; set; }
        public Movimento MovimentoPai { get; set; }
        public TipoMovimentoEstoque TipoMovimento { get; set; }
        public Nullable<DateTime> Data { get; set; }
        public int? EntradaMaterialId { get; set; }
        public EntradaMaterial EntradaMaterial { get; set; }
        //ResponsavelSolicitacao
        //ResponsavelAutorizacao
        public string ResponsavelRetirada { get; set; }
        public bool? EhTransferenciaDefinitiva { get; set; }
        public string Observacao { get; set; }
        public int? TipoDocumentoId { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public string Documento { get; set; }
        public string Referencia { get; set; }
        public Nullable<DateTime> DataEmissao { get; set; }
        public Nullable<DateTime> DataEntrega { get; set; }
        public DateTime DataOperacao { get; set; }
        public string LoginUsuarioOperacao { get; set; }
        public bool? EhMovimentoTemporario { get; set; }
        public int? ContratoId { get; set; }
        public Domain.Entity.Contrato.Contrato Contrato { get; set; }

        public ICollection<Movimento> ListaFilhos { get; set; }
        public ICollection<MovimentoItem> ListaMovimentoItem { get; set; }

        public Movimento()
        {
            this.ListaFilhos = new HashSet<Movimento>();
            this.ListaMovimentoItem = new HashSet<MovimentoItem>();
        }
    }
}