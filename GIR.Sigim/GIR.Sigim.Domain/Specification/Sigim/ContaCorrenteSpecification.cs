using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;


namespace GIR.Sigim.Domain.Specification.Sigim
{
    public class ContaCorrenteSpecification : BaseSpecification<ContaCorrente>
    {
        public static Specification<ContaCorrente> PertenceAoBanco(int? bancoId)
        {
            Specification<ContaCorrente> specification = new TrueSpecification<ContaCorrente>();
        
            if (bancoId.HasValue)
                return new DirectSpecification<ContaCorrente>(l => l.BancoId == bancoId);
                
            else
                return new DirectSpecification<ContaCorrente>(l => l.BancoId != 999);           

        }

        public static Specification<ContaCorrente> MatchingBancoId(int? bancoId)
        {
            Specification<ContaCorrente> specification = new TrueSpecification<ContaCorrente>();

            if (bancoId.HasValue)
            {
                var idSpecification = new DirectSpecification<ContaCorrente>(l => l.BancoId == bancoId);
                specification &= idSpecification;
            }

            return specification;

        }

        public static Specification<ContaCorrente> UsuarioPossuiAcessoAoCentroCusto(int? idUsuario, string modulo)
        {
            Specification<ContaCorrente> specification = new TrueSpecification<ContaCorrente>();

            if ((idUsuario.HasValue) && (!string.IsNullOrEmpty(modulo)))
            {
                var directSpecification = new DirectSpecification<ContaCorrente>(l =>
                    l.CentroCusto.ListaUsuarioCentroCusto.Any(c =>
                        c.UsuarioId == idUsuario && c.Modulo.Nome == modulo && c.CentroCusto.Situacao == "A"));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<ContaCorrente> EhAtivo()
        {
            return new DirectSpecification<ContaCorrente>(l => l.Situacao == "A");
        }

    }
}
