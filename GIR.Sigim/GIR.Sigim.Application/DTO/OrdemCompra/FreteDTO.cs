using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Application.Filtros;

namespace GIR.Sigim.Application.DTO.OrdemCompra
{
    public class FreteDTO : BaseDTO
    {
        public DateTime Data { get; set; }
        public decimal Valor { get; set; }
        public int? TituloPagarId { get; set; }
        public ClienteFornecedorDTO Transportadora { get; set; }
    }
}