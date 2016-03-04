using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Comercial
{
    public class Incorporador : BaseEntity
    {
        public string RazaoSocial { get; set; }
        public string TipoPessoa { get; set; }
        public string Cnpj { get; set; }
        public string InscricaoMunicipal { get; set; }
        public string InscricaoEstadual { get; set; }
        public string CodigoSUFRAMA { get; set; }
        
        public ICollection<Empreendimento> ListaEmpreendimento { get; set; }
        
        public Incorporador()
        {
            this.ListaEmpreendimento = new HashSet<Empreendimento>();
        }

    }
}
