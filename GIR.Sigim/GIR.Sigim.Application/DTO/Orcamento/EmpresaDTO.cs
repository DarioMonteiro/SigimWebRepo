using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.DTO.Orcamento
{
    public class EmpresaDTO : BaseDTO
    {
        public string Numero { get; set; }
        public int ClienteFornecedorId { get; set; }
        public ClienteFornecedorDTO ClienteFornecedor { get; set; }
        public string NumeroNomeEmpresa
        {
            get { return this.Numero + " - " + this.ClienteFornecedor.Nome; }
        }

        public List<ObraDTO> ListaObra { get; set; }
        public List<ObraDTO> ListaObraSemPai { get; set; }

        public EmpresaDTO()
        {
            this.ListaObra = new List<ObraDTO>();
            this.ListaObraSemPai = new List<ObraDTO>();
        }

    }
}
