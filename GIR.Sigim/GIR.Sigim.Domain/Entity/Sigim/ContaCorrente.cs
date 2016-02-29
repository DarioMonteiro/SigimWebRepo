using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class ContaCorrente : BaseEntity
    {
        public int? BancoId { get; set; }
        public Banco Banco { get; set; }
        public int? AgenciaId { get; set; }
        public Agencia Agencia { get; set; }
        public string ContaCodigo { get; set; }
        public string DVConta { get; set; }
        public string CodigoEmpresa { get; set; }
        public string NomeCedente { get; set; }
        public string CNPJCedente { get; set; }
        public string Descricao { get; set; }
        public string Complemento { get; set; }
        public byte ? Tipo { get; set; }
        public string Situacao { get; set; }

        public int? BancoLayoutCobranca { get; set; }
        public int? BancoLayoutPagamento { get; set; }
        public string CodigoCentroCusto { get; set; }
        public CentroCusto CentroCusto { get; set; }       
        public string ContaContabil { get; set; }
        public string Carteira { get; set; }
        public bool? ImprimeBoleto { get; set; }
        public int? ContaCorrentePenhor { get; set; }
        public string ContaContabilCredCob { get; set; }
        public string EnderecoCedente { get; set; }

        public ICollection<MovimentoFinanceiro> ListaMovimentoFinanceiro { get; set; }

        public ContaCorrente()
        {
            this.ListaMovimentoFinanceiro = new HashSet<MovimentoFinanceiro>();
        }
        
    }
}