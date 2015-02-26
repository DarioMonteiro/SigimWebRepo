using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.OrdemCompra
{
    public class InterfaceCotacao
    {
        public int? Modelo { get; set; }
        public int? CodigoCliente { get; set; }
        public int? CodigoLogin { get; set; }
        public int? CodigoRegiao { get; set; }
        public int? CodigoComprador { get; set; }
        public int? CodigoTelefone { get; set; }
        public int? CodigoCentroCusto { get; set; }
        public int? CodigoTipo { get; set; }
    }
}