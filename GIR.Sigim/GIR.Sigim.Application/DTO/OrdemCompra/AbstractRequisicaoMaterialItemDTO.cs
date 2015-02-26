using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.DTO.OrdemCompra
{
    public abstract class AbstractRequisicaoMaterialItemDTO : BaseDTO
    {
        //public int? MaterialId { get; set; }
        public MaterialDTO Material { get; set; }
        //public string CodigoClasse { get; set; }
        public ClasseDTO Classe { get; set; }
        public int Sequencial { get; set; }
        public string Complemento { get; set; }
        public string UnidadeMedida { get; set; }
        public decimal Quantidade { get; set; }
        public decimal QuantidadeAprovada { get; set; }
        public Nullable<DateTime> DataMinima { get; set; }
        public Nullable<DateTime> DataMaxima { get; set; }

        public AbstractRequisicaoMaterialItemDTO()
        {
            this.Material = new MaterialDTO();
            this.Classe = new ClasseDTO();
        }
    }
}