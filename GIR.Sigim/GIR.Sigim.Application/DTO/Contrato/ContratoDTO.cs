using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;  
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;  
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Application.Filtros;    

namespace GIR.Sigim.Application.DTO.Contrato
{
    public class ContratoDTO : BaseDTO
    {
        public string CodigoCentroCusto { get; set; }
        public CentroCustoDTO CentroCusto { get; set; }
        public string DescricaoCentroCusto 
        {
            get { return CentroCusto != null ? CentroCusto.Codigo + " - " + CentroCusto.Descricao : ""; }
        }
        public int? LicitacaoId { get; set; }
        public LicitacaoDTO Licitacao { get; set; }
        public int ContratanteId { get; set; }
        public ClienteFornecedorDTO Contratante { get; set; }
        public int ContratadoId { get; set; }
        public ClienteFornecedorDTO Contratado { get; set; }
        public int? IntervenienteId { get; set; }
        public ClienteFornecedorDTO Interveniente { get; set; }
        public int ContratoDescricaoId { get; set; }
        public LicitacaoDescricaoDTO ContratoDescricao { get; set; }
        public SituacaoContrato Situacao { get; set; }
        public string SituacaoDescricao
        {
            get { return this.Situacao.ObterDescricao(); }
        }
        public Nullable<DateTime> DataAssinatura { get; set; }
        public string DocumentoOrigem { get; set; }
        public string NumeroEmpenho { get; set; }
        public decimal? ValorContrato { get; set; }
        public DateTime DataCadastro { get; set; }
        public string UsuarioCadastro { get; set; }
        public Nullable<DateTime> DataCancela { get; set; }
        public string UsuarioCancela { get; set; }
        public string MotivoCancela { get; set; }
        public int TipoContrato { get; set; }
        public bool TemMedicaoALiberar { get; set; }
        public string DescricaoMedicaoLiberada
        {
            get { return this.TemMedicaoALiberar ? "Sim" : "Não"; }
        }
        public PaginationParameters PaginationParameters { get; set; }

        public List<ContratoRetificacaoDTO> ListaContratoRetificacao { get; set; }
        public List<ContratoRetificacaoItemDTO> ListaContratoRetificacaoItem { get; set; }
        public List<ContratoRetificacaoItemMedicaoDTO> ListaContratoRetificacaoItemMedicao { get; set; }
        public List<ContratoRetificacaoItemCronogramaDTO> ListaContratoRetificacaoItemCronograma { get; set; }
        public List<ContratoRetificacaoItemImpostoDTO> ListaContratoRetificacaoItemImposto { get; set; }
        public List<ContratoRetificacaoProvisaoDTO> ListaContratoRetificacaoProvisao { get; set; }
        public List<ContratoRetencaoDTO> ListaContratoRetencao { get; set; }


        public ContratoDTO()
        {
            this.CentroCusto = new CentroCustoDTO();
            this.Situacao = SituacaoContrato.Minuta;
            this.ListaContratoRetificacao = new List<ContratoRetificacaoDTO>(); 
            this.ListaContratoRetificacaoItem = new List<ContratoRetificacaoItemDTO>();
            this.ListaContratoRetificacaoItemMedicao = new List<ContratoRetificacaoItemMedicaoDTO>();
            this.ListaContratoRetificacaoItemCronograma = new List<ContratoRetificacaoItemCronogramaDTO>();
            this.ListaContratoRetificacaoItemImposto = new List<ContratoRetificacaoItemImpostoDTO>();
            this.ListaContratoRetificacaoProvisao = new List<ContratoRetificacaoProvisaoDTO>();
            this.ListaContratoRetencao = new List<ContratoRetencaoDTO>();

            PaginationParameters = new PaginationParameters();
            PaginationParameters.UniqueIdentifier = "_" + Guid.NewGuid().ToString().Replace("-", string.Empty);

        }
    }
}
