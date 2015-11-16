using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Specification.Sigim
{
    public class ClienteFornecedorSpecification : BaseSpecification<ClienteFornecedor>
    {
        public static Specification<ClienteFornecedor> NomeContem(string nome)
        {
            Specification<ClienteFornecedor> specification = new TrueSpecification<ClienteFornecedor>();

            if (!string.IsNullOrEmpty(nome))
            {
                var directSpecification = new DirectSpecification<ClienteFornecedor>(l => l.Nome.Contains(nome));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<ClienteFornecedor> RazaoSocialContem(string razaoSocial)
        {
            Specification<ClienteFornecedor> specification = new TrueSpecification<ClienteFornecedor>();

            if (!string.IsNullOrEmpty(razaoSocial))
            {
                var directSpecification = new DirectSpecification<ClienteFornecedor>(l => l.PessoaJuridica.NomeFantasia.Contains(razaoSocial));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<ClienteFornecedor> CnpjContem(string cnpj)
        {
            Specification<ClienteFornecedor> specification = new TrueSpecification<ClienteFornecedor>();

            if (!string.IsNullOrEmpty(cnpj))
            {
                var directSpecification = new DirectSpecification<ClienteFornecedor>(l => l.PessoaJuridica.Cnpj.Contains(cnpj));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<ClienteFornecedor> CpfContem(string cpf)
        {
            Specification<ClienteFornecedor> specification = new TrueSpecification<ClienteFornecedor>();

            if (!string.IsNullOrEmpty(cpf))
            {
                var directSpecification = new DirectSpecification<ClienteFornecedor>(l => l.PessoaFisica.Cpf.Contains(cpf));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<ClienteFornecedor> RgContem(string rg)
        {
            Specification<ClienteFornecedor> specification = new TrueSpecification<ClienteFornecedor>();

            if (!string.IsNullOrEmpty(rg))
            {
                var directSpecification = new DirectSpecification<ClienteFornecedor>(l => l.PessoaFisica.Rg.Contains(rg));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<ClienteFornecedor> EhAtivo()
        {
            return new DirectSpecification<ClienteFornecedor>(l => l.Situacao == "A" || l.Situacao == null);
        }

        public static Specification<ClienteFornecedor> EhClienteContrato()
        {
            return new DirectSpecification<ClienteFornecedor>(l => l.ClienteContrato == "S");
        }

        public static Specification<ClienteFornecedor> EhClienteOrdemCompra()
        {
            return new DirectSpecification<ClienteFornecedor>(l => l.ClienteOrdemCompra == "S");
        }

        public static Specification<ClienteFornecedor> EhClienteAPagar()
        {
            return new DirectSpecification<ClienteFornecedor>(l => l.ClienteAPagar == "S");
        }

        public static Specification<ClienteFornecedor> NomeNoIntervalo(string inicio, string fim)
        {
            Specification<ClienteFornecedor> specification = new TrueSpecification<ClienteFornecedor>();

            if (!string.IsNullOrEmpty(inicio))
            {
                var directSpecification = new DirectSpecification<ClienteFornecedor>(l => l.Nome.CompareTo(inicio) >= 0);
                specification &= directSpecification;
            }

            if (!string.IsNullOrEmpty(fim))
            {
                var directSpecification = new DirectSpecification<ClienteFornecedor>(l => l.Nome.Substring(0, fim.Length).CompareTo(fim) <= 0);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<ClienteFornecedor> RazaoSocialNoIntervalo(string inicio, string fim)
        {
            Specification<ClienteFornecedor> specification = new TrueSpecification<ClienteFornecedor>();

            if (!string.IsNullOrEmpty(inicio))
            {
                var directSpecification = new DirectSpecification<ClienteFornecedor>(l => l.PessoaJuridica.NomeFantasia.CompareTo(inicio) >= 0);
                specification &= directSpecification;
            }

            if (!string.IsNullOrEmpty(fim))
            {
                var directSpecification = new DirectSpecification<ClienteFornecedor>(l => l.PessoaJuridica.NomeFantasia.Substring(0, fim.Length).CompareTo(fim) <= 0);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<ClienteFornecedor> CnpjNoIntervalo(string inicio, string fim)
        {
            Specification<ClienteFornecedor> specification = new TrueSpecification<ClienteFornecedor>();

            if (!string.IsNullOrEmpty(inicio))
            {
                var directSpecification = new DirectSpecification<ClienteFornecedor>(l => l.PessoaJuridica.Cnpj.CompareTo(inicio) >= 0);
                specification &= directSpecification;
            }

            if (!string.IsNullOrEmpty(fim))
            {
                var directSpecification = new DirectSpecification<ClienteFornecedor>(l => l.PessoaJuridica.Cnpj.Substring(0, fim.Length).CompareTo(fim) <= 0);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<ClienteFornecedor> CpfNoIntervalo(string inicio, string fim)
        {
            Specification<ClienteFornecedor> specification = new TrueSpecification<ClienteFornecedor>();

            if (!string.IsNullOrEmpty(inicio))
            {
                var directSpecification = new DirectSpecification<ClienteFornecedor>(l => l.PessoaFisica.Cpf.CompareTo(inicio) >= 0);
                specification &= directSpecification;
            }

            if (!string.IsNullOrEmpty(fim))
            {
                var directSpecification = new DirectSpecification<ClienteFornecedor>(l => l.PessoaFisica.Cpf.Substring(0, fim.Length).CompareTo(fim) <= 0);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<ClienteFornecedor> RgNoIntervalo(string inicio, string fim)
        {
            Specification<ClienteFornecedor> specification = new TrueSpecification<ClienteFornecedor>();

            if (!string.IsNullOrEmpty(inicio))
            {
                var directSpecification = new DirectSpecification<ClienteFornecedor>(l => l.PessoaFisica.Rg.CompareTo(inicio) >= 0);
                specification &= directSpecification;
            }

            if (!string.IsNullOrEmpty(fim))
            {
                var directSpecification = new DirectSpecification<ClienteFornecedor>(l => l.PessoaFisica.Rg.Substring(0, fim.Length).CompareTo(fim) <= 0);
                specification &= directSpecification;
            }

            return specification;
        }

    }
}
