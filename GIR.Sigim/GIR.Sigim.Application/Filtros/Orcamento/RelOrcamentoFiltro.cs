using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Orcamento;

namespace GIR.Sigim.Application.Filtros.Orcamento
{
    public class RelOrcamentoFiltro : BaseFiltro
    {
        [Display(Name = "Empresa")]
        public int? EmpresaId { get; set; }

        [Display(Name = "Obra")]
        public int? ObraId { get; set; }

        public OrcamentoDTO Orcamento { get; set; }

        [Display(Name = "Indice")]
        public int? IndiceId { get; set; }

        [Display(Name = "BDI")]
        public bool EhBDI { get; set; }

        [Display(Name = "Classe")]
        public bool EhClasse { get; set; }

        [Display(Name = "Valor corrigido")]
        public bool EhValorCorrigido { get; set; }

        [Display(Name = "Sem detalhamento")]
        public bool EhSemDetalhamento { get; set; }

        [RegularExpression(@"[0-9]*$", ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Defasagem")]
        public int? Defasagem { get; set; }

        public string NomeIndice { get; set; }
        public Decimal BDITotal { get; set; }
        public Decimal PrecoTotal { get; set; }
        public DateTime DataBase { get; set; }
        public Decimal CotacaoBase { get; set; }
        public DateTime DataAtual { get; set; }
        public Decimal CotacaoAtual { get; set; }
        public Decimal AreaConstrucaoAreaReal { get; set; }
        public Decimal AreaConstrucaoAreaEquivalente { get; set; }

        public List<ClasseDTO> ListaClasse { get; set; }

        public RelOrcamentoFiltro()
        {
            this.ListaClasse = new List<ClasseDTO>();
        }
    }
}
