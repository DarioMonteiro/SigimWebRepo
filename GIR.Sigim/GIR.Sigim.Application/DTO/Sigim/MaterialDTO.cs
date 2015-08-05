using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Filtros;

namespace GIR.Sigim.Application.DTO.Sigim
{
    public class MaterialDTO : BaseDTO
    {
        public string Descricao { get; set; }
        public string SiglaUnidadeMedida { get; set; }
        public string CodigoMaterialClasseInsumo { get; set; }
        public decimal? PrecoUnitario { get; set; }
        public Nullable<DateTime> DataCadastro { get; set; }
        public Nullable<DateTime> DataAlteracao { get; set; }
        public Nullable<DateTime> DataPreco { get; set; }
        public decimal? QuantidadeMinima { get; set; }
        public bool? EhControladoPorEstoque { get; set; }
        public string ContaContabil { get; set; }
        public string DescricaoTipoTabela { get; set; }
        public int? AnoMes { get; set; }
        public string CodigoExterno { get; set; }
        public string CodigoSituacaoMercadoria { get; set; }
        public string CodigoNCM { get; set; }
        public string TipoMaterial { get; set; }
        public PaginationParameters PaginationParameters { get; set; }

        public MaterialDTO()
        {
            PaginationParameters = new PaginationParameters();
            PaginationParameters.UniqueIdentifier = "_" + Guid.NewGuid().ToString().Replace("-", string.Empty);
        }
    }
}