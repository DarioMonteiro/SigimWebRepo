using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.CredCob;

namespace GIR.Sigim.Domain.Specification.CredCob
{
    public class TituloMovimentoSpecification : BaseSpecification<TituloMovimento>
    {

        public static Specification<TituloMovimento> UsuarioPossuiAcessoAoCentroCusto(int? idUsuario, string modulo)
        {
            Specification<TituloMovimento> specification = new TrueSpecification<TituloMovimento>();

            if ((idUsuario.HasValue) && (!string.IsNullOrEmpty(modulo)))
            {
                var directSpecification = new DirectSpecification<TituloMovimento>(l =>
                    l.TituloCredCob.Contrato.Unidade.Bloco.CentroCusto.ListaUsuarioCentroCusto.Any(c =>
                            c.UsuarioId == idUsuario && c.Modulo.Nome == modulo && c.CentroCusto.Situacao == "A"));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<TituloMovimento> EhCentroCustoAtivo()
        {
            Specification<TituloMovimento> specification = new TrueSpecification<TituloMovimento>();

            var directSpecification = new DirectSpecification<TituloMovimento>(l => l.TituloCredCob.Contrato.Unidade.Bloco.CentroCusto.Situacao == "A");
            specification &= directSpecification;

            return specification;
        }

        public static Specification<TituloMovimento> PertenceAoCentroCustoIniciadoPor(string codigoCentroCusto)
        {
            Specification<TituloMovimento> specification = new TrueSpecification<TituloMovimento>();

            if (!string.IsNullOrEmpty(codigoCentroCusto))
            {
                var directSpecification = new DirectSpecification<TituloMovimento>(l => l.TituloCredCob.Contrato.Unidade.Bloco.CentroCusto.Codigo.StartsWith(codigoCentroCusto));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<TituloMovimento> DataPeriodoMaiorOuIgualRelApropriacaoPorClasse(DateTime? data)
        {
            Specification<TituloMovimento> specification = new TrueSpecification<TituloMovimento>();

            if (data.HasValue)
            {
                var directSpecification = new DirectSpecification<TituloMovimento>(l => l.MovimentoFinanceiro.DataConferencia >= data);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<TituloMovimento> DataPeriodoMenorOuIgualRelApropriacaoPorClasse(DateTime? data)
        {
            Specification<TituloMovimento> specification = new TrueSpecification<TituloMovimento>();

            DateTime dataUltimaHora = new DateTime(data.Value.Year, data.Value.Month, data.Value.Day, 23, 59, 59);

            if (data.HasValue)
            {
                var directSpecification = new DirectSpecification<TituloMovimento>(l => l.MovimentoFinanceiro.DataConferencia <= dataUltimaHora);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<TituloMovimento> EhSituacaoIgualConferido()
        {
            Specification<TituloMovimento> specification = new TrueSpecification<TituloMovimento>();

            var directSpecification = new DirectSpecification<TituloMovimento>(l => l.MovimentoFinanceiro.Situacao == "C");
            specification &= directSpecification;

            return specification;
        }

        public static Specification<TituloMovimento> PossuiContaCorrente()
        {
            Specification<TituloMovimento> specification = new TrueSpecification<TituloMovimento>();

            var directSpecification = new DirectSpecification<TituloMovimento>(l => (l.MovimentoFinanceiro.ContaCorrenteId.HasValue && l.MovimentoFinanceiro.ContaCorrenteId > 0));
            specification &= directSpecification;

            return specification;
        }

        public static Specification<TituloMovimento> EhTipoParticipanteTitular()
        {
            Specification<TituloMovimento> specification = new TrueSpecification<TituloMovimento>();

            var directSpecification = new DirectSpecification<TituloMovimento>(l => l.TituloCredCob.Contrato.ListaVendaParticipante.Any(v => v.TipoParticipanteId.Value == 1));
            specification &= directSpecification;

            return specification;
        }

        public static Specification<TituloMovimento> EhValorMovimentoMaiorQueZero()
        {
            Specification<TituloMovimento> specification = new TrueSpecification<TituloMovimento>();

            var directSpecification = new DirectSpecification<TituloMovimento>(l => l.MovimentoFinanceiro.Valor > 0);
            specification &= directSpecification;

            return specification;
        }

        public static Specification<TituloMovimento> SaoClassesExistentes(string[] codigosClasse)
        {
            Specification<TituloMovimento> specification = new TrueSpecification<TituloMovimento>();
            var idSpecification = new DirectSpecification<TituloMovimento>(l => codigosClasse.Any(o => o == l.TituloCredCob.VerbaCobranca.CodigoClasse));
            specification &= idSpecification;

            return specification;
        }

    }
}
