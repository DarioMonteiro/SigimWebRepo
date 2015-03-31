﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Entity.Contrato; 

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class CentroCusto : BaseEntity
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string CentroContabil { get; set; }
        public int? AnoMes { get; set; }
        public int? TipoTabela { get; set; }
        [Obsolete("Esta propriedade será removida em uma versão futura. Caso NÃO esteja codificando em um repositório, utilize a propriedade \"Ativo\"")]
        public string Situacao { get; set; }
        public bool Ativo
        {
            get { return Situacao == "A"; }
            set { Situacao = value ? "A" : "I"; }
        }
        public string CodigoPai { get; set; }
        public CentroCusto CentroCustoPai { get; set; }
        public virtual ICollection<CentroCusto> ListaFilhos { get; set; }
        public ICollection<CentroCustoEmpresa> ListaCentroCustoEmpresa { get; set; }
        public ICollection<ParametrosUsuario> ListaParametrosUsuario { get; set; }
        public ICollection<UsuarioCentroCusto> ListaUsuarioCentroCusto { get; set; }
        public ICollection<PreRequisicaoMaterialItem> ListaPreRequisicaoMaterialItem { get; set; }
        public ICollection<RequisicaoMaterial> ListaRequisicaoMaterial { get; set; }
        public ICollection<Orcamento.Obra> ListaObra { get; set; }
        public ICollection<Contrato.Contrato> ListaContrato { get; set; }
        public ICollection<Contrato.Licitacao> ListaLicitacao { get; set; }

        public CentroCusto()
        {
            this.ListaFilhos = new HashSet<CentroCusto>();
            this.ListaCentroCustoEmpresa = new HashSet<CentroCustoEmpresa>();
            this.ListaParametrosUsuario = new HashSet<ParametrosUsuario>();
            this.ListaUsuarioCentroCusto = new HashSet<UsuarioCentroCusto>();
            this.ListaPreRequisicaoMaterialItem = new HashSet<PreRequisicaoMaterialItem>();
            this.ListaRequisicaoMaterial = new HashSet<RequisicaoMaterial>();
            this.ListaObra = new HashSet<Orcamento.Obra>();
            this.ListaContrato = new HashSet<Contrato.Contrato>();
            this.ListaLicitacao = new HashSet<Contrato.Licitacao>();
        }

        public bool UsuarioPossuiAcesso(int? idUsuario, string modulo)
        {
            return ListaUsuarioCentroCusto.Any(l => l.UsuarioId == idUsuario && l.Modulo.Nome == modulo);
        }
    }
}