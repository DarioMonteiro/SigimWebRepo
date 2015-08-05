using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Enums;

namespace GIR.Sigim.Application.Filtros.Sigim
{
    public class MaterialPesquisaFiltro : PaginationParameters
    {
        public string Campo { get; set; }
        public TipoPesquisa TipoSelecao { get; set; }
        public string TextoInicio { get; set; }
        public string TextoFim { get; set; }
        public string CodigoCentroCusto { get; set; }

        public MaterialPesquisaFiltro()
        {
            Ascending = true;
            OrderBy = "descricao";
        }
    }
}