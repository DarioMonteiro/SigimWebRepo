using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Enums;

namespace GIR.Sigim.Application.DTO.Sigim
{
    public class TabelaBasicaDTO : BaseDTO
    {
        [Required]
        [Display(Name = "Tipo tabela")]
        public int TipoTabela { get; set; }

        [Required]
        [StringLength(50, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Automático")]
        public bool? Automatico { get; set; }

        public string AutomaticoDescricao
        {get { return Automatico == true ? "Sim" : "Não"; }}

    }
}