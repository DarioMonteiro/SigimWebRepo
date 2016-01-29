using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.CredCob;

namespace GIR.Sigim.Domain.Specification.CredCob
{
    public class TituloCredCobSpecification : BaseSpecification<TituloCredCob>
    {

        public static Specification<TituloCredCob> UsuarioPossuiAcessoAoCentroCusto(int? idUsuario, string modulo)
        {
            Specification<TituloCredCob> specification = new TrueSpecification<TituloCredCob>();

            if ((idUsuario.HasValue) && (!string.IsNullOrEmpty(modulo)))
            {
                var directSpecification = new DirectSpecification<TituloCredCob>(l =>
                    l.Contrato.Unidade.Bloco.CentroCusto.ListaUsuarioCentroCusto.Any(c =>
                            c.UsuarioId == idUsuario && c.Modulo.Nome == modulo && c.CentroCusto.Situacao == "A"));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<TituloCredCob> EhCentroCustoAtivo()
        {
            Specification<TituloCredCob> specification = new TrueSpecification<TituloCredCob>();

            var directSpecification = new DirectSpecification<TituloCredCob>(l => l.Contrato.Unidade.Bloco.CentroCusto.Situacao == "A");
            specification &= directSpecification;

            return specification;
        }

        public static Specification<TituloCredCob> PertenceAoCentroCustoIniciadoPor(string codigoCentroCusto)
        {
            Specification<TituloCredCob> specification = new TrueSpecification<TituloCredCob>();

            if (!string.IsNullOrEmpty(codigoCentroCusto))
            {
                var directSpecification = new DirectSpecification<TituloCredCob>(l => l.Contrato.Unidade.Bloco.CentroCusto.Codigo.StartsWith(codigoCentroCusto));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<TituloCredCob> EhTipoParticipanteTitular()
        {
            Specification<TituloCredCob> specification = new TrueSpecification<TituloCredCob>();

            var directSpecification = new DirectSpecification<TituloCredCob>(l => l.Contrato.VendaParticipanteId.Value == 1);
            specification &= directSpecification;

            return specification;
        }

        public static Specification<TituloCredCob> EhSituacaoDiferenteDeCancelado()
        {
            Specification<TituloCredCob> specification = new TrueSpecification<TituloCredCob>();

            var directSpecification = new DirectSpecification<TituloCredCob>(l => l.Situacao == "C");
            specification &= directSpecification;

            return specification;
        }

        public static Specification<TituloCredCob> DataPeriodoMaiorOuIgualRelApropriacaoPorClasse(DateTime? data)
        {
            Specification<TituloCredCob> specification = new TrueSpecification<TituloCredCob>();

            if (data.HasValue)
            {
                var directSpecification = new DirectSpecification<TituloCredCob>(l => ((l.Situacao == "P" && l.DataVencimento >= data) || 
                                                                                       (l.Situacao == "Q" && l.DataPagamento.Value >= data)));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<TituloCredCob> DataPeriodoMenorOuIgualRelApropriacaoPorClasse(DateTime? data)
        {
            Specification<TituloCredCob> specification = new TrueSpecification<TituloCredCob>();

            DateTime dataUltimaHora = new DateTime(data.Value.Year, data.Value.Month, data.Value.Day, 23, 59, 59);

            if (data.HasValue)
            {
                var directSpecification = new DirectSpecification<TituloCredCob>(l => ((l.Situacao == "P" && l.DataVencimento <= dataUltimaHora) ||
                                                                                       (l.Situacao == "Q" && l.DataPagamento.Value <= dataUltimaHora)));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<TituloCredCob> SaoClassesExistentes(string[] codigosClasse)
        {
            Specification<TituloCredCob> specification = new TrueSpecification<TituloCredCob>();
            var idSpecification = new DirectSpecification<TituloCredCob>(l => codigosClasse.Any(o => o == l.VerbaCobranca.CodigoClasse));
            specification &= idSpecification;

            return specification;
        }


    }
}
