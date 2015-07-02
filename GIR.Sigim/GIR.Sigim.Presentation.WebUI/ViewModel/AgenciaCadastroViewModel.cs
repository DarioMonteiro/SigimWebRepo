using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros.Sigim;
using GIR.Sigim.Presentation.WebUI.ViewModel;

namespace GIR.Sigim.Presentation.WebUI.ViewModel
{
    public class AgenciaCadastroViewModel
    {
        public AgenciaDTO Agencia { get; set; }

        public int? Id { get; set; }

        public BancoDTO Banco { get; set; }

        [Display(Name = "Agência")]
        public string AgenciaCodigo { get; set; }

        [Display(Name = "DV")]
        public string DVAgencia { get; set; }

        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Display(Name = "Nome contato")]
        public string NomeContato { get; set; }

        [Display(Name = "Telefone contato")]
        public string TelefoneContato { get; set; }

        [Display(Name = "Tipo logradouro")]
        public string TipoLogradouro { get; set; }

        [Display(Name = "Logradouro")]
        public string Logradouro { get; set; }

        [Display(Name = "Cidade")]
        public string Cidade { get; set; }

        [Display(Name = "Número")]
        public string Numero { get; set; }

        [Display(Name = "Complemento")]
        public string Complemento { get; set; }

        public SelectList ListaBanco { get; set; }

        public AgenciaCadastroViewModel()
        {
            //ContaCorrente = new ContaCorrenteDTO();            
        }
    }
}