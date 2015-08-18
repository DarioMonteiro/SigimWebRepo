using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GIR.Sigim.Application.DTO.Admin
{
    public class PerfilDTO : BaseDTO 
    {
        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Required]
        [Display(Name = "Módulo")]
        public int ModuloId { get; set; }

        public ModuloDTO Modulo { get; set; }

        public List<PerfilFuncionalidadeDTO> ListaFuncionalidade { get; set; }

        public PerfilDTO()
        {
            this.Modulo = new ModuloDTO();
            this.ListaFuncionalidade = new List<PerfilFuncionalidadeDTO>();
        }
        
    }
}