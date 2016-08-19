using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Comercial;

namespace GIR.Sigim.Application.Filtros.Comercial
{
    public class RelStatusVendaFiltro : BaseFiltro
    {
        [Display(Name = "Incorporador")]
        public int IncorporadorId { get; set; }
        public IncorporadorDTO Incorporador { get; set; }

        [Display(Name = "Empreendimento")]
        public int? EmpreendimentoId { get; set; }
        public EmpreendimentoDTO Empreendimento { get; set; }

        [Display(Name = "Bloco")]
        public int? BlocoId { get; set; }
        public BlocoDTO Bloco { get; set; }

        [Display(Name = "Proposta")]
        public bool SituacaoProposta { get; set; }

        [Display(Name = "Assinada")]
        public bool SituacaoAssinada { get; set; }

        [Display(Name = "Cancelada")]
        public bool SituacaoCancelada { get; set; }

        [Display(Name = "Rescindida")]
        public bool SituacaoRescindida { get; set; }

        [Display(Name = "Quitada")]
        public bool SituacaoQuitada { get; set; }

        [Display(Name = "Escriturada")]
        public bool SituacaoEscriturada { get; set; }

        [Display(Name = "Todas")]
        public bool SituacaoTodas { get; set; }

        [Display(Name = "Aprovado")]
        public int? Aprovado { get; set; }

        public int MoedaConversao { get; set; }

    }
}