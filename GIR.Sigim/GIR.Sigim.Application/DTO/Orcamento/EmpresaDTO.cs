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
            get
            {
                string numeroNomeEmpresa = "";
                if (!string.IsNullOrEmpty(this.Numero))
                {
                    numeroNomeEmpresa = this.Numero;
                }
                if (this.ClienteFornecedor != null)
                {
                    if (!string.IsNullOrEmpty(this.ClienteFornecedor.Nome))
                    {
                        numeroNomeEmpresa = numeroNomeEmpresa + " - " + this.ClienteFornecedor.Nome;
                    }
                }

                return numeroNomeEmpresa;
            }
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
