﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Filtros;
using GIR.Sigim.Application.DTO.Comercial;

namespace GIR.Sigim.Application.DTO.Sigim
{
    public class ClienteFornecedorDTO : BaseDTO
    {
        public string Nome { get; set; }
        public string TipoPessoa { get; set; }
        public string Situacao { get; set; }
        public string TipoCliente { get; set; }
        public string ClienteAPagar { get; set; }
        public string ClienteAReceber { get; set; }
        public string ClienteOrdemCompra { get; set; }
        public string ClienteContrato { get; set; }
        public string ClienteAluguel { get; set; }
        public string ClienteEmpreitada { get; set; }
        public PessoaFisicaDTO PessoaFisica { get; set; }
        public PessoaJuridicaDTO PessoaJuridica { get; set; }
        public PaginationParameters PaginationParameters { get; set; }

        public ICollection<VendaParticipanteDTO> ListaVendaParticipante { get; set; }

        public ClienteFornecedorDTO()
        {
            PaginationParameters = new PaginationParameters();
            PaginationParameters.UniqueIdentifier = "_" + Guid.NewGuid().ToString().Replace("-", string.Empty);

            this.ListaVendaParticipante = new HashSet<VendaParticipanteDTO>();
        }
    }
}