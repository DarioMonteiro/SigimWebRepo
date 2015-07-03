using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Contrato;

namespace GIR.Sigim.Domain.Specification.Contrato
{
    public class ContratoRetificacaoItemMedicaoSpecification : BaseSpecification<ContratoRetificacaoItemMedicao>
    {

        public static Specification<ContratoRetificacaoItemMedicao> PertenceAoCentroCustoIniciadoPor(string centroCustoId)
        {
            Specification<ContratoRetificacaoItemMedicao> specification = new TrueSpecification<ContratoRetificacaoItemMedicao>();

            if (!string.IsNullOrEmpty(centroCustoId))
            {
                var directSpecification = new DirectSpecification<ContratoRetificacaoItemMedicao>(l => l.Contrato.CentroCusto.Codigo.StartsWith(centroCustoId));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<ContratoRetificacaoItemMedicao> UsuarioPossuiAcessoAoCentroCusto(int? idUsuario, string modulo)
        {
            Specification<ContratoRetificacaoItemMedicao> specification = new TrueSpecification<ContratoRetificacaoItemMedicao>();

            if ((idUsuario.HasValue) && (!string.IsNullOrEmpty(modulo)))
            {
                var directSpecification = new DirectSpecification<ContratoRetificacaoItemMedicao>(l =>
                    l.Contrato.CentroCusto.ListaUsuarioCentroCusto.Any(c =>
                            c.UsuarioId == idUsuario && c.Modulo.Nome == modulo && c.CentroCusto.Situacao == "A"));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<ContratoRetificacaoItemMedicao> PertenceAoContratoComSituacaoLiberado(int? contratoId)
        {
            Specification<ContratoRetificacaoItemMedicao> specification = new TrueSpecification<ContratoRetificacaoItemMedicao>();

            if (contratoId.HasValue)
            {
                var directSpecification = new DirectSpecification<ContratoRetificacaoItemMedicao>(l => l.Contrato.Id == contratoId && l.Situacao == SituacaoMedicao.Liberado);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<ContratoRetificacaoItemMedicao> DocumentoPertenceAhMedicao(string numeroDocumento)
        {
            Specification<ContratoRetificacaoItemMedicao> specification = new TrueSpecification<ContratoRetificacaoItemMedicao>();

            if (!string.IsNullOrEmpty(numeroDocumento))
            {
                var directSpecification = new DirectSpecification<ContratoRetificacaoItemMedicao>(l => l.NumeroDocumento.Contains(numeroDocumento));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<ContratoRetificacaoItemMedicao> DataLiberacaoMaiorOuIgual(DateTime? data)
        {
            Specification<ContratoRetificacaoItemMedicao> specification = new TrueSpecification<ContratoRetificacaoItemMedicao>();

            if (data.HasValue)
            {
                var directSpecification = new DirectSpecification<ContratoRetificacaoItemMedicao>(l => l.DataLiberacao >= data);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<ContratoRetificacaoItemMedicao> DataLiberacaoMenorOuIgual(DateTime? data)
        {
            Specification<ContratoRetificacaoItemMedicao> specification = new TrueSpecification<ContratoRetificacaoItemMedicao>();

            if (data.HasValue)
            {
                var directSpecification = new DirectSpecification<ContratoRetificacaoItemMedicao>(l => l.DataLiberacao <= data);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<ContratoRetificacaoItemMedicao> SituacaoIgualLiberado()
        {
            Specification<ContratoRetificacaoItemMedicao> specification = new TrueSpecification<ContratoRetificacaoItemMedicao>();

            var directSpecification = new DirectSpecification<ContratoRetificacaoItemMedicao>(l => l.Situacao == SituacaoMedicao.Liberado);
            specification &= directSpecification;

            return specification;
        }


        public static Specification<ContratoRetificacaoItemMedicao> FornecedorClientePertenceAhMedicao(int? fornecedorClienteId)
        {
            Specification<ContratoRetificacaoItemMedicao> specification = new TrueSpecification<ContratoRetificacaoItemMedicao>();

            if (fornecedorClienteId.HasValue)
            {
                var directSpecification = new DirectSpecification<ContratoRetificacaoItemMedicao>(l => 
                                                                                                    (
                                                                                                     (l.MultiFornecedorId != null && l.MultiFornecedorId == fornecedorClienteId) ||
                                                                                                     (l.MultiFornecedorId == null &&
                                                                                                      ((l.Contrato.TipoContrato == 0 && l.Contrato.ContratadoId == fornecedorClienteId) ||
                                                                                                       (l.Contrato.TipoContrato != 0 && l.Contrato.ContratanteId == fornecedorClienteId))
                                                                                                     )
                                                                                                    )
                                                                                                 );
                specification &= directSpecification;
            }

            return specification;
        }

    }
}
