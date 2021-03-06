﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Domain.Specification.OrdemCompra
{
    public class EntradaMaterialSpecification : BaseSpecification<EntradaMaterial>
    {
        public static Specification<EntradaMaterial> DataMaiorOuIgual(DateTime? data)
        {
            Specification<EntradaMaterial> specification = new TrueSpecification<EntradaMaterial>();

            if (data.HasValue)
            {
                var directSpecification = new DirectSpecification<EntradaMaterial>(l => l.Data >= data);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<EntradaMaterial> DataMenorOuIgual(DateTime? data)
        {
            Specification<EntradaMaterial> specification = new TrueSpecification<EntradaMaterial>();

            if (data.HasValue)
            {
                var directSpecification = new DirectSpecification<EntradaMaterial>(l => l.Data <= data);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<EntradaMaterial> PertenceAoCentroCustoIniciadoPor(string centroCustoId)
        {
            Specification<EntradaMaterial> specification = new TrueSpecification<EntradaMaterial>();

            if (!string.IsNullOrEmpty(centroCustoId))
            {
                var directSpecification = new DirectSpecification<EntradaMaterial>(l => l.CentroCusto.Codigo.StartsWith(centroCustoId));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<EntradaMaterial> MatchingNumeroNotaFiscal(string numeroNotaFiscal)
        {
            Specification<EntradaMaterial> specification = new TrueSpecification<EntradaMaterial>();

            if (!string.IsNullOrEmpty(numeroNotaFiscal))
            {
                var idSpecification = new DirectSpecification<EntradaMaterial>(l => l.NumeroNotaFiscal == numeroNotaFiscal);
                specification &= idSpecification;
            }

            return specification;
        }

        public static Specification<EntradaMaterial> NumeroNotaFiscalTerminaCom(string numeroNotaFiscal)
        {
            Specification<EntradaMaterial> specification = new TrueSpecification<EntradaMaterial>();

            if (!string.IsNullOrEmpty(numeroNotaFiscal))
            {
                var idSpecification = new DirectSpecification<EntradaMaterial>(l => l.NumeroNotaFiscal.EndsWith(numeroNotaFiscal));
                specification &= idSpecification;
            }

            return specification;
        }

        public static Specification<EntradaMaterial> UsuarioPossuiAcessoAoCentroCusto(int? idUsuario, string modulo)
        {
            Specification<EntradaMaterial> specification = new TrueSpecification<EntradaMaterial>();

            if ((idUsuario.HasValue) && (!string.IsNullOrEmpty(modulo)))
            {
                var directSpecification = new DirectSpecification<EntradaMaterial>(l =>
                    l.CentroCusto.ListaUsuarioCentroCusto.Any(c =>
                        c.UsuarioId == idUsuario && c.Modulo.Nome == modulo && c.CentroCusto.Situacao == "A"));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<EntradaMaterial> EhCentroCustoAtivo()
        {
            Specification<EntradaMaterial> specification = new TrueSpecification<EntradaMaterial>();

            var directSpecification = new DirectSpecification<EntradaMaterial>(l => l.CentroCusto.Situacao == "A");
            specification &= directSpecification;

            return specification;
        }


        public static Specification<EntradaMaterial> MatchingFornecedor(int? fornecedorId)
        {
            Specification<EntradaMaterial> specification = new TrueSpecification<EntradaMaterial>();

            if (fornecedorId.HasValue)
            {
                var idSpecification = new DirectSpecification<EntradaMaterial>(l => l.ClienteFornecedorId == fornecedorId);
                specification &= idSpecification;
            }

            return specification;
        }

        public static Specification<EntradaMaterial> MatchingFornecedorNota(int? fornecedorId)
        {
            Specification<EntradaMaterial> specification = new TrueSpecification<EntradaMaterial>();

            if (fornecedorId.HasValue)
            {
                var idSpecification = new DirectSpecification<EntradaMaterial>(l => l.FornecedorNotaId == fornecedorId);
                specification &= idSpecification;
            }

            return specification;
        }

        public static Specification<EntradaMaterial> MatchingAnoEmissaoNota(int ano)
        {
            Specification<EntradaMaterial> specification = new TrueSpecification<EntradaMaterial>();
            var idSpecification = new DirectSpecification<EntradaMaterial>(l => l.DataEmissaoNota.Value.Year == ano);
            specification &= idSpecification;

            return specification;
        }

        public static Specification<EntradaMaterial> EhPendente()
        {
            return new DirectSpecification<EntradaMaterial>(l => l.Situacao == SituacaoEntradaMaterial.Pendente);
        }

        public static Specification<EntradaMaterial> EhCancelada()
        {
            return new DirectSpecification<EntradaMaterial>(l => l.Situacao == SituacaoEntradaMaterial.Cancelada);
        }

        public static Specification<EntradaMaterial> EhFechada()
        {
            return new DirectSpecification<EntradaMaterial>(l => l.Situacao == SituacaoEntradaMaterial.Fechada);
        }
    }
}