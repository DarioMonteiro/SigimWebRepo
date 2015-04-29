using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Application.DTO.OrdemCompra
{
    public abstract class AbstractRequisicaoMaterialDTO : BaseDTO
    {
        [DataType(DataType.Date, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "InformeDataValida")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Data")]
        public DateTime Data { get; set; }

        [StringLength(255, ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Observação")]
        public string Observacao { get; set; }

        [Display(Name = "Data cadastro")]
        public Nullable<DateTime> DataCadastro { get; set; }

        [Display(Name = "Cadastrado por")]
        public string LoginUsuarioCadastro { get; set; }

        [Display(Name = "Data cancelamento")]
        public Nullable<DateTime> DataCancelamento { get; set; }

        [Display(Name = "Cancelado por")]
        public string LoginUsuarioCancelamento { get; set; }

        [Display(Name = "Motivo do CancelamentoX")]
        public string MotivoCancelamento { get; set; }

        public AbstractRequisicaoMaterialDTO()
        {
            this.Data = DateTime.Now;
        }
    }
}