using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;

namespace GIR.Sigim.Application.DTO.Orcamento
{
    public class ObraDTO : BaseDTO
    {
        public int? EmpresaId { get; set; }
        public EmpresaDTO Empresa { get; set; }
        public string Numero { get; set; }
        public string Descricao { get; set; }
        public string CodigoCentroCusto { get; set; }
        public CentroCustoDTO CentroCusto { get; set; }
        public bool? OrcamentoSimplificado { get; set; }
        public string DescricaoOrcamentoSimplificado
        {
            get
            {
                string descricaoOrcamentoSimplificado = !OrcamentoSimplificado.HasValue ?  "Não" : ((OrcamentoSimplificado.Value == true) ? "Sim" : "Não");

                return descricaoOrcamentoSimplificado;
            }
        }

        public string NumeroDescricao
        {
            get
            {
                string numeroDescricao = "";
                if (!string.IsNullOrEmpty(this.Numero))
                {
                    numeroDescricao = this.Numero;
                }
                if (!string.IsNullOrEmpty(this.Descricao))
                {
                    numeroDescricao = numeroDescricao + " - " + this.Descricao;
                }

                return numeroDescricao;
            }
        }
    }
}