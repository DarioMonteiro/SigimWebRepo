using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.DTO.OrdemCompra
{
    public class ParametrosSacDTO : BaseDTO
    {
        [RegularExpression(@"[0-9]*$", ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Prazo de avaliação")]
        public short? DiasDataMinima { get; set; }

        [RegularExpression(@"[0-9]*$", ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Prazo de conclusão")]
        public short? DiasPrazo { get; set; }

        [Display(Name = "Empresa")]
        public int? ClienteId { get; set; }
        public ClienteFornecedorDTO Cliente { get; set; }
        
        [Display(Name = "Ícone para relatórios")]
        public byte[] IconeRelatorio { get; set; }
        public bool RemoverImagem { get; set; }
    }
}