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
        public int? BancoIdPesquisado { get; set; }
        public AgenciaDTO Agencia { get; set; }       
        public SelectList ListaBanco { get; set; }
        public SelectList ListaUnidadeFederacao { get; set; }
        public bool PodeSalvar { get; set; }
        public bool PodeDeletar { get; set; }
        public bool PodeImprimir { get; set; }
        public bool PodeAcessarContaCorrente { get; set; }
        public bool EhValidoImprimir { get; set; }

        public AgenciaCadastroViewModel()
        {
               
        }
        
        
    }
}