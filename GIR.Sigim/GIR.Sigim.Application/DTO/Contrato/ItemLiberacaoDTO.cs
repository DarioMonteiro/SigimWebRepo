using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;

namespace GIR.Sigim.Application.DTO.Contrato
{
    public class ItemLiberacaoDTO : BaseDTO
    {
        public int? ContratoRetificacaoProvisaoId { get; set; }
        public int? ContratoRetificacaoItemMedicaoId { get; set; }
        public int SequencialItem { get; set; }
        public Nullable<DateTime> DataMedicao { get; set; }
        public DateTime DataVencimento { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public Nullable<DateTime> DataEmissao { get; set; }
        public int CodigoSituacao { get; set; }
        public string DescricaoSituacao { get; set; }
        public decimal Valor { get; set; }
        public decimal? ValorRetido { get; set; }
        public string ResponsavelMedicao { get; set; }
        public int TituloId { get; set; }
        public Nullable<DateTime> DataLiberacao { get; set; }
        public string ResponsavelLiberacao { get; set; }
        public string DescricaoEvento { get; set; }
        public Nullable<DateTime> DataCadastro { get; set; }
        public string Avaliacao { get; set; }
        public bool EhPagamentoAntecipado { get; set; }
        public string CorTexto { get; set; }
        public string CorLinhaSelecionada
        {
            get
            {
                return "background-color:yellow";
            }
        }
        public int Ordem { get; set; }
        public int PosicaoLista { get; set; }
        public bool Selecionado { get; set; }
    }
}
