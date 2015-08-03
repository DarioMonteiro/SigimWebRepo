using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GIR.Sigim.Application.DTO.Admin
{
    public class UsuarioPerfilDTO : BaseDTO 
    {
        [Required]
        [Display(Name = "Usuário")]
        public int UsuarioId { get; set; }
        public UsuarioDTO Usuario { get; set; }

        [Required]
        [Display(Name = "Módulo")]
        public int ModuloId { get; set; }
        public ModuloDTO Modulo { get; set; }

       [Required]
        [Display(Name = "Perfil")]
        public int PerfilId { get; set; }
        public PerfilDTO Perfil { get; set; }

        public UsuarioPerfilDTO()
        {
            this.Usuario = new UsuarioDTO();
            this.Modulo = new ModuloDTO();
            this.Perfil = new PerfilDTO();
        }
        
    }
}