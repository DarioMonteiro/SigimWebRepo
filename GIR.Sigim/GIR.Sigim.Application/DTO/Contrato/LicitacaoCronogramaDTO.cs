﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;

namespace GIR.Sigim.Application.DTO.Contrato
{
    public class LicitacaoCronogramaDTO : BaseDTO
    {

        public string CodigoCentroCusto { get; set; }
        public virtual CentroCustoDTO CentroCusto { get; set; }
        public int LicitacaoDescricaoId { get; set; }
        public LicitacaoDescricaoDTO LicitacaoDescricao { get; set; }
        public DateTime DataInicioCartaConvite { get; set; }
        public DateTime DataFimCartaConvite { get; set; }
        public DateTime DataInicioQuadroComparativo { get; set; }
        public DateTime DataFimQuadroComparativo { get; set; }
        public DateTime DataInicioAssinatura { get; set; }
        public DateTime DataFimAssinatura { get; set; }
        public DateTime DataInicioServico { get; set; }
        public int? PrazoFabricacao { get; set; }
        public int DuracaoCartaConvite { get; set; }
        public int DuracaoQuadroComparativo { get; set; }
        public int DuracaoAssinatura { get; set; }
        public Nullable<DateTime> DataInicioCartaConviteRealizado { get; set; }
        public Nullable<DateTime> DataFimCartaConviteRealizado { get; set; }
        public Nullable<DateTime> DataInicioQuadroComparativoRealizado { get; set; }
        public Nullable<DateTime> DataFimQuadroComparativoRealizado { get; set; }
        public Nullable<DateTime> DataInicioAssinaturaRealizado { get; set; }
        public Nullable<DateTime> DataFimAssinaturaRealizado { get; set; }
        public Nullable<DateTime> DataInicioServicoRealizado { get; set; }

        public List<LicitacaoDTO> ListaLicitacao { get; set; }

        public LicitacaoCronogramaDTO()
        {
            this.ListaLicitacao = new List<LicitacaoDTO>();
        }

    }
}
