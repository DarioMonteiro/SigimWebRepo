using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Filtros;    


namespace GIR.Sigim.Application.DTO.Orcamento
{
    public class OrcamentoDTO : BaseDTO
    {
        public int EmpresaId { get; set; }
        public EmpresaDTO Empresa { get; set; }
        public int ObraId { get; set; }
        public ObraDTO Obra { get; set; }
        public int? Sequencial { get; set; }
        public string Descricao { get; set; }
        public Nullable<DateTime> Data { get; set; }
        public string Situacao { get; set; }
        public string SituacaoDescricao {
            get {
                    string situacaoDescricao = "";

                    switch (Situacao)
                    {
                        case "E":
                            situacaoDescricao = "Em elaboração";
                            break;
                        case "A":
                            situacaoDescricao = "Ativo";
                            break;
                        case "C":
                            situacaoDescricao = "Concluído";
                            break;
                        default:
                            situacaoDescricao = "";
                            break;
                    }

                    return situacaoDescricao;
            }
        }

        public bool EhControlado { get; set; }
        public ICollection<OrcamentoComposicaoDTO> ListaOrcamentoComposicao { get; set; }

        public PaginationParameters PaginationParameters { get; set; }


        public OrcamentoDTO()
        {
            this.ListaOrcamentoComposicao = new HashSet<OrcamentoComposicaoDTO>();
            PaginationParameters = new PaginationParameters();
            PaginationParameters.UniqueIdentifier = "_" + Guid.NewGuid().ToString().Replace("-", string.Empty);
        }
    }
}