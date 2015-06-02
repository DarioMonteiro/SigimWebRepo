using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Application.Adapter;


namespace GIR.Sigim.Application.DTO.Financeiro
{
    public class AbstractTituloDTO : BaseDTO
    {
        public int ClienteID { get; set; }
        public ClienteFornecedorDTO Cliente { get; set; }
        public int? TipoCompromissoId { get; set; }
        public TipoCompromissoDTO TipoCompromisso { get; set; }
        public string Identificacao { get; set; }
        public SituacaoTituloPagar Situacao { get; set; }
        public string SituacaoDescricao
        {
            get { return this.Situacao.ObterDescricao(); }
        }
        public int? TipoDocumentoId { get; set; }
        public TipoDocumentoDTO TipoDocumento { get; set; }
        public string Documento { get; set; }
        public DateTime DataEmissaoDocumento { get; set; }
        public DateTime DataVencimento { get; set; }
        public TipoTitulo TipoTitulo { get; set; }
        public string TipoTituloDescricao
        {
            get { return this.TipoTitulo.ObterDescricao(); }
        }

        public decimal ValorTitulo { get; set; }

    }
}
