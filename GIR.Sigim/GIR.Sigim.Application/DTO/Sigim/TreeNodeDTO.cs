using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Sigim
{
    public class TreeNodeDTO
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public TreeNodeDTO NodePai { get; set; }
        public ICollection<TreeNodeDTO> ListaFilhos { get; set; }

        public TreeNodeDTO()
        {
            this.ListaFilhos = new HashSet<TreeNodeDTO>();
        }
    }
}