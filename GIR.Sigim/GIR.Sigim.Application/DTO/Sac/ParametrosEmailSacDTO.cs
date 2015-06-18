using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.DTO.Sac
{
    public class ParametrosEmailSacDTO : BaseDTO
    {
        [Required]
        [Display(Name = "Tipo")]
        public int? Tipo { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Setor")]
        public int? SetorId { get; set; }
        public SetorDTO Setor { get; set; }

        [Display(Name = "Anexo")]
        public bool Anexo { get; set; }

        [Display(Name = "Parametros")]
        public int? ParametrosId { get; set; }
        public ParametrosSacDTO ParametrosSac { get; set; }
     }
}