using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Sigim
{
    public class MaterialClasseInsumoDTO : BaseDTO
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public bool? EhMovimentoTemporario { get; set; }
        public int? Sequencial { get; set; }
        public bool? NaoGeraSPED { get; set; }
        public MaterialClasseInsumoDTO ClassePai { get; set; }
        public ICollection<MaterialClasseInsumoDTO> ListaFilhos { get; set; }

        public ICollection<MaterialDTO> ListaMaterial { get; set; }

        public MaterialClasseInsumoDTO()
        {
            this.ListaFilhos = new HashSet<MaterialClasseInsumoDTO>();
            this.ListaMaterial = new HashSet<MaterialDTO>();
        }

    }
}
