using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Admin;

namespace GIR.Sigim.Domain.Entity.OrdemCompra
{
    public abstract class AbstractRequisicaoMaterial : BaseEntity
    {
        public DateTime Data { get; set; }
        public string Observacao { get; set; }
        public DateTime DataCadastro { get; set; }
        //TODO: Criar relação com a classe Usuario
        public string LoginUsuarioCadastro { get; set; }
        public Nullable<DateTime> DataCancelamento { get; set; }
        //TODO: Criar relação com a classe Usuario
        public string LoginUsuarioCancelamento { get; set; }
        public string MotivoCancelamento { get; set; }
    }
}