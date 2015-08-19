using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Enums;

namespace GIR.Sigim.Application.Filtros.Sigim
{
    public class ClienteFornecedorPesquisaFiltro : PaginationParameters
    {
        public string Campo { get; set; }
        public TipoPesquisa TipoSelecao { get; set; }
        public string TextoInicio { get; set; }
        public string TextoFim { get; set; }

        public ClienteFornecedorPesquisaFiltro()
        {
            Ascending = true;
            OrderBy = "nomeFantasia";
        }

    }
}
