using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
   public class ParametrosFinanceiro : BaseEntity
   {
       public int? ClienteId { get; set; }
       public ClienteFornecedor Cliente { get; set; }
       public string Responsavel { get; set; }
       public string PracaPagamento { get; set; }
       public string Licenca { get; set; }
       public string CentroCusto { get; set; }
       public string Classe { get; set; }
       public short? SituacaoDefaultPagar { get; set; }
       public short? SituacaoDefaultReceber { get; set; }
       public bool? GeraTituloImposto { get; set; }
       public bool? LeitoraCodigoBarras { get; set; }
       public decimal? ToleranciaRecebimento { get; set; }
       public bool? InterfaceBloqueioTotal { get; set; }
       public bool? InterfaceBloqueioParcial { get; set; }
       public bool? InterfacePermiteApropriacao { get; set; }
       public bool? InterfaceContabil { get; set; }
       public bool? PercentualApropriacao { get; set; }
       public byte[] IconeRelatorio { get; set; }
       public bool? ConfereNFOC { get; set; }
       public bool? ConfereNFCT { get; set; }
       public bool? ImpostoAutomatico { get; set; }
       public bool? ValorLiquidoApropriado { get; set; }
       public bool? ContaCorrenteCentroCusto { get; set; }
       public string BloqueioSituacaoLiberado { get; set; }

       public short Interface
       {
           get
           {
               short retorno;
               retorno = 0;
               if (InterfaceBloqueioTotal == true) retorno = 1;
               if (InterfaceBloqueioParcial == true) retorno = 2;
               if (InterfacePermiteApropriacao == true) retorno = 3;

               return retorno;
           }
           set
           {
               if (value == 1) { InterfaceBloqueioTotal = true;  InterfaceBloqueioParcial = false; InterfacePermiteApropriacao = false; }
               if (value == 2) { InterfaceBloqueioTotal = false; InterfaceBloqueioParcial = true;  InterfacePermiteApropriacao = false; }
               if (value == 3) { InterfaceBloqueioTotal = false; InterfaceBloqueioParcial = false; InterfacePermiteApropriacao = true;  }
           }
       }

       public ParametrosFinanceiro()
       {
       }

   }
    
}
