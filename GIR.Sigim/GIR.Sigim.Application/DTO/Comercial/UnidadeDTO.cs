using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Comercial;

namespace GIR.Sigim.Application.DTO.Comercial
{
    public class UnidadeDTO : BaseDTO
    {
        public string Descricao { get; set; }
        public int? EmpreendimentoId { get; set; }
        public EmpreendimentoDTO Empreendimento { get; set; }
        public int? BlocoId { get; set; }
        public BlocoDTO Bloco { get; set; }

        public Nullable<Decimal> TaxaPermanenciaDiaria { get; set; }
        public Nullable<Decimal> MultaPorAtraso { get; set; } 
        public bool? ConsiderarParametroUnidade { get; set; }

        public List<ContratoComercialDTO> ListaContratoComercial { get; set; }

        public UnidadeDTO()
        {
            this.ListaContratoComercial = new List<ContratoComercialDTO>();
        }

    }
}
