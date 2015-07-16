using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Domain.Specification.OrdemCompra
{
    public class OrdemCompraItemSpecification : BaseSpecification<OrdemCompraItem>
    {

        public static Specification<OrdemCompraItem> UsuarioPossuiAcessoAoCentroCusto(int? idUsuario, string modulo)
        {
            Specification<OrdemCompraItem> specification = new TrueSpecification<OrdemCompraItem>();

            if ((idUsuario.HasValue) && (!string.IsNullOrEmpty(modulo)))
            {
                var directSpecification = new DirectSpecification<OrdemCompraItem>(l =>
                    l.OrdemCompra.CentroCusto.ListaUsuarioCentroCusto.Any(c =>
                        c.UsuarioId == idUsuario && c.Modulo.Nome == modulo));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<OrdemCompraItem> DataOrdemCompraMaiorOuIgual(DateTime? data)
        {
            Specification<OrdemCompraItem> specification = new TrueSpecification<OrdemCompraItem>();

            if (data.HasValue)
            {
                var directSpecification = new DirectSpecification<OrdemCompraItem>(l => l.OrdemCompra.Data >= data);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<OrdemCompraItem> DataOrdemCompraMenorOuIgual(DateTime? data)
        {
            Specification<OrdemCompraItem> specification = new TrueSpecification<OrdemCompraItem>();

            if (data.HasValue)
            {
                DateTime dataUltimaHora = new DateTime(data.Value.Year, data.Value.Month, data.Value.Day, 23, 59, 59);

                var directSpecification = new DirectSpecification<OrdemCompraItem>(l => l.OrdemCompra.Data <= dataUltimaHora);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<OrdemCompraItem> ClienteFornecedorPertenceAhItemOC(int? clienteFornecedorId)
        {
            Specification<OrdemCompraItem> specification = new TrueSpecification<OrdemCompraItem>();

            if (clienteFornecedorId.HasValue)
            {
                var directSpecification = new DirectSpecification<OrdemCompraItem>(l => l.OrdemCompra.ClienteFornecedorId == clienteFornecedorId);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<OrdemCompraItem> PertenceAoCentroCustoIniciadoPor(string codigoCentroCusto)
        {
            Specification<OrdemCompraItem> specification = new TrueSpecification<OrdemCompraItem>();

            if (!string.IsNullOrEmpty(codigoCentroCusto))
            {
                var directSpecification = new DirectSpecification<OrdemCompraItem>(l => l.OrdemCompra.CentroCusto.Codigo.StartsWith(codigoCentroCusto));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<OrdemCompraItem> PertenceAhClasseIniciadaPor(string codigoClasse)
        {
            Specification<OrdemCompraItem> specification = new TrueSpecification<OrdemCompraItem>();

            if (!string.IsNullOrEmpty(codigoClasse))
            {
                var directSpecification = new DirectSpecification<OrdemCompraItem>(l => l.Classe.Codigo.StartsWith(codigoClasse));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<OrdemCompraItem> PertenceAhClasseInsumoIniciadaPor(string codigoClasseInsumo)
        {
            Specification<OrdemCompraItem> specification = new TrueSpecification<OrdemCompraItem>();

            if (!string.IsNullOrEmpty(codigoClasseInsumo))
            {
                var directSpecification = new DirectSpecification<OrdemCompraItem>(l => l.Material.MaterialClasseInsumo.Codigo.StartsWith(codigoClasseInsumo));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<OrdemCompraItem> PertenceAhOrdemCompraId(int? ordemCompraId)
        {
            Specification<OrdemCompraItem> specification = new TrueSpecification<OrdemCompraItem>();

            if (ordemCompraId.HasValue)
            {
                var directSpecification = new DirectSpecification<OrdemCompraItem>(l => l.OrdemCompraId == ordemCompraId);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<OrdemCompraItem> PertenceMaterialId(int? materialId)
        {
            Specification<OrdemCompraItem> specification = new TrueSpecification<OrdemCompraItem>();

            if (materialId.HasValue)
            {
                var directSpecification = new DirectSpecification<OrdemCompraItem>(l => l.MaterialId == materialId);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<OrdemCompraItem> EhLiberada()
        {
            return new DirectSpecification<OrdemCompraItem>(l => l.OrdemCompra.Situacao == SituacaoOrdemCompra.Liberada);
        }

        public static Specification<OrdemCompraItem> EhPendente()
        {
            return new DirectSpecification<OrdemCompraItem>(l => l.OrdemCompra.Situacao == SituacaoOrdemCompra.Pendente);
        }

        public static Specification<OrdemCompraItem> EhFechada()
        {
            return new DirectSpecification<OrdemCompraItem>(l => l.OrdemCompra.Situacao == SituacaoOrdemCompra.Fechada);
        }

        public static Specification<OrdemCompraItem> EhExibirSomenteComSaldo(bool? exibirSomenteComSaldo)
        {
            Specification<OrdemCompraItem> specification = new TrueSpecification<OrdemCompraItem>();

            if (exibirSomenteComSaldo.HasValue && exibirSomenteComSaldo.Value)
            {
                var directSpecification = new DirectSpecification<OrdemCompraItem>(l => (l.Quantidade - l.QuantidadeEntregue) > 0);
            }

            return specification;
        }


    }
}
