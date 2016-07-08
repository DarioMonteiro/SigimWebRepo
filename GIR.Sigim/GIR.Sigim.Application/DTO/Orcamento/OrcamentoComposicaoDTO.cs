using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.DTO.Orcamento
{
    public class OrcamentoComposicaoDTO : BaseDTO
    {
        public int? OrcamentoId { get; set; }
        public OrcamentoDTO Orcamento { get; set; }
        public int? ComposicaoId { get; set; }
        public ComposicaoDTO Composicao { get; set; }
        public string codigoClasse { get; set; }
        public ClasseDTO Classe { get; set; }
        public decimal? Quantidade { get; set; }
        public decimal? Preco { get; set; }
        public bool? EhSincronizada { get; set; }
        public string EspecificacaoTecnica { get; set; }
        public ICollection<OrcamentoComposicaoItemDTO> ListaOrcamentoComposicaoItem { get; set; }

        public OrcamentoComposicaoDTO()
        {
            this.ListaOrcamentoComposicaoItem = new HashSet<OrcamentoComposicaoItemDTO>();
        }
    }
}