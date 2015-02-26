using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Application.DTO.Sigim
{
    public class BancoLayoutDTO : BaseDTO
    {
        public string Descricao { get; set; }
        public short Padrao { get; set; }
        public TipoLayout Tipo { get; set; }
        public bool DesconsideraPosicao { get; set; }
        public int? BancoId { get; set; }
        public BancoDTO Banco { get; set; }
    }
}