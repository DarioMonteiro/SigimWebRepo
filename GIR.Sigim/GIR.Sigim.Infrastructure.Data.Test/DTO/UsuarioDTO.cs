using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Infrastructure.Data.Test.DTO
{
    public class UsuarioDTO
    {
        public int? Id { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        //TODO: Inverter para Ativo
        public bool Inativo { get; set; }
        public ICollection<FuncionalidadeDTO> ListaFuncionalidade { get; set; }
        public ICollection<PerfilDTO> ListaPerfil { get; set; }

        public UsuarioDTO()
        {
            this.ListaFuncionalidade = new HashSet<FuncionalidadeDTO>();
            this.ListaPerfil = new HashSet<PerfilDTO>();
        }
    }
}