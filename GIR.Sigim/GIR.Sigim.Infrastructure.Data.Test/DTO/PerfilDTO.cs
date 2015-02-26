using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Infrastructure.Data.Test.DTO
{
    public class PerfilDTO
    {
        public int? Id { get; set; }
        public string Descricao { get; set; }
        public ModuloDTO Modulo { get; set; }
        public ICollection<FuncionalidadeDTO> ListaFuncionalidade { get; set; }

        public PerfilDTO()
        {
            this.ListaFuncionalidade = new HashSet<FuncionalidadeDTO>();
        }
    }
}