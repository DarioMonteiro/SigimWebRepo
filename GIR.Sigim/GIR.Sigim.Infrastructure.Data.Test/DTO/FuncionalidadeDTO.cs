using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Infrastructure.Data.Test.DTO
{
    public class FuncionalidadeDTO
    {
        public int? Id { get; set; }
        public string Descricao { get; set; }
        public string ChaveAcesso { get; set; }
        public bool Ativo { get; set; }
        public int ModuloId { get; set; }
        public ModuloDTO Modulo { get; set; }
    }
}