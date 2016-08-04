using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Enums;

namespace GIR.Sigim.Application.Filtros.Orcamento
{
    public class OrcamentoPesquisaFiltro : PaginationParameters
    {
        public int? EmpresaId { get; set; }
        public int? ObraId { get; set; }
        public string Campo { get; set; }
        public TipoPesquisa TipoSelecao { get; set; }
        public string TextoInicio { get; set; }
        public string TextoFim { get; set; }

        public OrcamentoPesquisaFiltro()
        {
            Ascending = true;
            OrderBy = "sequencial";
        }

    }
}
