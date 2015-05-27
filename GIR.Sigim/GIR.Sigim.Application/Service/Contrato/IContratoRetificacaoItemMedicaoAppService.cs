﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Contrato;
using GIR.Sigim.Domain.Entity.Contrato;

namespace GIR.Sigim.Application.Service.Contrato
{
    public interface IContratoRetificacaoItemMedicaoAppService : IBaseAppService
    {
        void ObterQuantidadesEhValoresMedicao(int ContratoId,
                                              int SequenciaItem,
                                              int SequencialCronograma,
                                              ref decimal QuantidadeTotalMedido,
                                              ref decimal ValorTotalMedido,
                                              ref decimal QuantidadeTotalLiberado,
                                              ref decimal ValorTotalLiberado);

        bool ExisteNumeroDocumento(Nullable<DateTime> DataEmissao, string NumeroDocumento, int? ContratadoId);
        bool Salvar(ContratoRetificacaoItemMedicaoDTO dto);
    }
}
