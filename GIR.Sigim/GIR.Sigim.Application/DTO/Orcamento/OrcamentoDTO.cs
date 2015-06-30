using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Orcamento
{
    public class OrcamentoDTO : BaseDTO
    {
        public ObraDTO Obra { get; set; }
        public int? Sequencial { get; set; }
        public string Descricao { get; set; }
        public Nullable<DateTime> Data { get; set; }
        public bool Ativo { get; set; }
        public bool EhControlado { get; set; }
        public ICollection<OrcamentoComposicaoDTO> ListaOrcamentoComposicao { get; set; }

        public OrcamentoDTO()
        {
            this.ListaOrcamentoComposicao = new HashSet<OrcamentoComposicaoDTO>();
        }
    }
}