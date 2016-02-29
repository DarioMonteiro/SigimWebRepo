using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Financeiro
{
    public class ClasseDTO : BaseDTO
    {
        [StringLength(18, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Classe")]
        public string Codigo { get; set; }

        public string Descricao { get; set; }

        [Display(Name = "Classe")]
        public string ClasseDescricao
        {
            get { return this.Codigo + " - " + this.Descricao; }
        }

        public string CentroContabil { get; set; }
        public int? AnoMes { get; set; }
        public int? TipoTabela { get; set; }
        public bool Ativo { get; set; }
        //public ClasseDTO ClassePai { get; set; }
        public ICollection<ClasseDTO> ListaFilhos { get; set; }

        public ClasseDTO()
        {
            this.ListaFilhos = new HashSet<ClasseDTO>();
        }
    }
}