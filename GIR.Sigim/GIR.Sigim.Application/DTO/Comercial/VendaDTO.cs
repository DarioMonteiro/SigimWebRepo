﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Comercial;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.DTO.Comercial
{
    public class VendaDTO : BaseDTO
    {
        public ContratoComercialDTO Contrato { get; set; }
        public Nullable<DateTime> DataVenda { get; set; }
        public int TabelaVendaId { get; set; }
        public TabelaVendaDTO TabelaVenda { get; set; }
        public Decimal PrecoTabela { get; set; }
        public Decimal? ValorDesconto { get; set; }

        public Decimal PercentualDesconto
        {
            get
            {
                if (!ValorDesconto.HasValue) { return 0; }
                if (ValorDesconto.Value == 0) {return 0;}
                if (ValorDesconto != 0) {return ((ValorDesconto.Value * 100) / PrecoTabela);}
                return 0;
            }
        }

        //CASE WHEN ISNULL(VEN.valorDesconto, 0) > 0 THEN ((VEN.valorDesconto * 100) / VEN.precoTabela) ELSE 0 END AS percentualDesconto,

        public Decimal PrecoPraticado { get; set; }
        public String Condicao { get; set; }
        public Decimal PrecoContrato { get; set; }
        public int? IndiceFinanceiroId { get; set; }
        public IndiceFinanceiroDTO IndiceFinanceiro { get; set; }
        public Decimal CotacaoIndiceFinanceiro { get; set; }
        public DateTime DataBaseIndiceFinanceiro { get; set; }
        public Nullable<DateTime> DataAssinaturaAgenda { get; set; }
        public String HoraAssinaturaAgenda { get; set; }
        public Nullable<DateTime> DataAssinatura { get; set; }
        public String HoraAssinatura { get; set; }
        public String NumeroCartorio { get; set; }
        public String NumeroLivroCartorio { get; set; }
        public String NumeroFolhaLivroCartorio { get; set; }
        public String FormaVenda { get; set; }
        public String FormaContrato { get; set; }
        public int? ContaCorrenteId { get; set; }
        public ContaCorrenteDTO ContaCorrente { get; set; }
        public Nullable<DateTime> DataCadastramento { get; set; }
        public Nullable<DateTime> DataQuitacao { get; set; }
        public Nullable<DateTime> DataCancelamento { get; set; }
        public Nullable<Boolean> Aprovado { get; set; }
        public String UsuarioAprovacao { get; set; }
        public Nullable<DateTime> DataAprovacao { get; set; }
        public Decimal? PrecoBaseComissao { get; set; }
        public int? MatrizId { get; set; }
        public int? CorretorMatrizId { get; set; }
        public Decimal? ValorTotalComissao { get; set; }

        public VendaDTO()
        {

        }
    }
}
