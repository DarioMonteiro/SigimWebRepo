using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Domain.Specification.Financeiro
{
    public class TituloPagarSpecification : BaseSpecification<TituloPagar>
    {

        public static Specification<TituloPagar> DataPeriodoMaiorOuIgualSituacaoPendentesRelContasPagarTitulos(DateTime? data)
        {
            Specification<TituloPagar> specification = new TrueSpecification<TituloPagar>();

            if (data.HasValue)
            {
                var directSpecification = new DirectSpecification<TituloPagar>(l => l.DataVencimento >= data);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<TituloPagar> DataPeriodoMaiorOuIgualSituacaoPagosRelContasPagarTitulos(SituacaoTituloPagar situacao ,string tipoPesquisa,DateTime? data)
        {
            Specification<TituloPagar> specification = new TrueSpecification<TituloPagar>();

            if (situacao == SituacaoTituloPagar.Emitido)
            {
                if (tipoPesquisa.ToUpper() == "V")
                {
                    if (data.HasValue)
                    {
                        var directSpecification = new DirectSpecification<TituloPagar>(l => l.DataVencimento >= data);
                        specification &= directSpecification;
                    }
                }
                else
                {
                    var directSpecification = new DirectSpecification<TituloPagar>(l => l.DataEmissao >= data);
                    specification &= directSpecification;
                }
            }

            if (situacao == SituacaoTituloPagar.Pago)
            {
                if (tipoPesquisa.ToUpper() == "V")
                {
                    if (data.HasValue)
                    {
                        var directSpecification = new DirectSpecification<TituloPagar>(l => l.DataVencimento >= data);
                        specification &= directSpecification;
                    }
                }
                else
                {
                    var directSpecification = new DirectSpecification<TituloPagar>(l => l.DataPagamento >= data);
                    specification &= directSpecification;
                }
            }

            if (situacao == SituacaoTituloPagar.Baixado)
            {
                if (tipoPesquisa.ToUpper() == "V")
                {
                    if (data.HasValue)
                    {
                        var directSpecification = new DirectSpecification<TituloPagar>(l => l.DataVencimento >= data);
                        specification &= directSpecification;
                    }
                }
                else
                {
                    var directSpecification = new DirectSpecification<TituloPagar>(l => l.DataBaixa >= data);
                    specification &= directSpecification;
                }
            }

            return specification;
        }

        public static Specification<TituloPagar> DataPeriodoMenorOuIgualSituacaoPendentesRelContasPagarTitulos(DateTime? data)
        {
            Specification<TituloPagar> specification = new TrueSpecification<TituloPagar>();

            if (data.HasValue)
            {
                DateTime dataUltimaHora = new DateTime(data.Value.Year, data.Value.Month, data.Value.Day, 23, 59, 59);

                var directSpecification = new DirectSpecification<TituloPagar>(l => l.DataVencimento <= dataUltimaHora);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<TituloPagar> DataPeriodoMenorOuIgualSituacaoPagosRelContasPagarTitulos(SituacaoTituloPagar situacao, string tipoPesquisa, DateTime? data)
        {
            Specification<TituloPagar> specification = new TrueSpecification<TituloPagar>();

            DateTime dataUltimaHora = new DateTime(data.Value.Year, data.Value.Month, data.Value.Day, 23, 59, 59);

            if (situacao == SituacaoTituloPagar.Emitido)
            {
                if (tipoPesquisa.ToUpper() == "V")
                {
                    if (data.HasValue)
                    {
                        var directSpecification = new DirectSpecification<TituloPagar>(l => l.DataVencimento <= dataUltimaHora);
                        specification &= directSpecification;
                    }
                }
                else
                {
                    if (data.HasValue)
                    {
                        var directSpecification = new DirectSpecification<TituloPagar>(l => l.DataEmissao <= dataUltimaHora);
                        specification &= directSpecification;
                    }
                }
            }

            if (situacao == SituacaoTituloPagar.Pago)
            {
                if (tipoPesquisa.ToUpper() == "V")
                {
                    if (data.HasValue)
                    {
                        var directSpecification = new DirectSpecification<TituloPagar>(l => l.DataVencimento <= dataUltimaHora);
                        specification &= directSpecification;
                    }
                }
                else
                {
                    if (data.HasValue)
                    {
                        var directSpecification = new DirectSpecification<TituloPagar>(l => l.DataPagamento <= dataUltimaHora);
                        specification &= directSpecification;
                    }
                }
            }

            if (situacao == SituacaoTituloPagar.Baixado)
            {
                if (tipoPesquisa.ToUpper() == "V")
                {
                    if (data.HasValue)
                    {
                        var directSpecification = new DirectSpecification<TituloPagar>(l => l.DataVencimento <= dataUltimaHora);
                        specification &= directSpecification;
                    }
                }
                else
                {
                    if (data.HasValue)
                    {
                        var directSpecification = new DirectSpecification<TituloPagar>(l => l.DataBaixa <= dataUltimaHora);
                        specification &= directSpecification;
                    }
                }
            }


            return specification;
        }

        public static Specification<TituloPagar> EhSituacaoAPagarProvisionado(bool provisionado)
        {
            if (provisionado)
            {
                return new DirectSpecification<TituloPagar>(l => l.Situacao == SituacaoTituloPagar.Provisionado);
            }

            return new FalseSpecification<TituloPagar>();
        }

        public static Specification<TituloPagar> EhSituacaoAPagarAguardandoLiberacao(bool aguardandoLiberacao)
        {
            if (aguardandoLiberacao)
            {
                return new DirectSpecification<TituloPagar>(l => l.Situacao == SituacaoTituloPagar.AguardandoLiberacao);
            }

            return new FalseSpecification<TituloPagar>();
        }

        public static Specification<TituloPagar> EhSituacaoAPagarLiberado(bool liberado)
        {
            if (liberado)
            {
                return new DirectSpecification<TituloPagar>(l => l.Situacao == SituacaoTituloPagar.Liberado);
            }

            return new FalseSpecification<TituloPagar>();
        }

        public static Specification<TituloPagar> EhSituacaoAPagarCancelado(bool cancelado)
        {
            if (cancelado)
            {
                return new DirectSpecification<TituloPagar>(l => l.Situacao == SituacaoTituloPagar.Cancelado);
            }

            return new FalseSpecification<TituloPagar>();
        }

        public static Specification<TituloPagar> EhSituacaoAPagarEmitido(bool emitido)
        {
            if (emitido)
            {
                return new DirectSpecification<TituloPagar>(l => l.Situacao == SituacaoTituloPagar.Emitido);
            }

            return new FalseSpecification<TituloPagar>();
        }

        public static Specification<TituloPagar> EhSituacaoAPagarPago(bool pago)
        {
            if (pago)
            {
                return new DirectSpecification<TituloPagar>(l => l.Situacao == SituacaoTituloPagar.Pago);
            }

            return new FalseSpecification<TituloPagar>();
        }

        public static Specification<TituloPagar> EhSituacaoAPagarBaixado(bool baixado)
        {
            if (baixado)
            {
                return new DirectSpecification<TituloPagar>(l => l.Situacao == SituacaoTituloPagar.Baixado);
            }

            return new FalseSpecification<TituloPagar>();
        }

        public static Specification<TituloPagar> PertenceAoCentroCustoIniciadoPor(string codigoCentroCusto)
        {
            Specification<TituloPagar> specification = new TrueSpecification<TituloPagar>();

            if (!string.IsNullOrEmpty(codigoCentroCusto))
            {
                var directSpecification = new DirectSpecification<TituloPagar>(l => l.ListaApropriacao.Select(a => a.CentroCusto).Any(a => a.Codigo.StartsWith(codigoCentroCusto)));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<TituloPagar> PertenceAClasseIniciadaPor(string codigoClasse)
        {
            Specification<TituloPagar> specification = new TrueSpecification<TituloPagar>();

            if (!string.IsNullOrEmpty(codigoClasse))
            {
                var directSpecification = new DirectSpecification<TituloPagar>(l => l.ListaApropriacao.Select(a => a.Classe).Any(a => a.Codigo.StartsWith(codigoClasse)));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<TituloPagar> MatchingClienteId(int? clienteId)
        {
            Specification<TituloPagar> specification = new TrueSpecification<TituloPagar>();

            if (clienteId.HasValue)
            {
                var idSpecification = new DirectSpecification<TituloPagar>(l => l.ClienteId == clienteId);
                specification &= idSpecification;
            }

            return specification;
        }

        public static Specification<TituloPagar> IdentificacaoContem(string identificacao)
        {
            Specification<TituloPagar> specification = new TrueSpecification<TituloPagar>();

            if (!string.IsNullOrEmpty(identificacao))
            {
                var directSpecification = new DirectSpecification<TituloPagar>(l => l.Identificacao.Contains(identificacao));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<TituloPagar> DocumentoContem(string documento)
        {
            Specification<TituloPagar> specification = new TrueSpecification<TituloPagar>();

            if (!string.IsNullOrEmpty(documento))
            {
                var directSpecification = new DirectSpecification<TituloPagar>(l => l.Documento.Contains(documento));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<TituloPagar> MatchingTipoCompromissoId(int? tipoCompromissoId)
        {
            Specification<TituloPagar> specification = new TrueSpecification<TituloPagar>();

            if (tipoCompromissoId.HasValue)
            {
                var directSpecification = new DirectSpecification<TituloPagar>(l => l.TipoCompromissoId.Value == tipoCompromissoId);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<TituloPagar> PertenceAhFormaPagamento(Nullable<Int16> formaPagamentoCodigo)
        {
            Specification<TituloPagar> specification = new TrueSpecification<TituloPagar>();

            if (formaPagamentoCodigo.HasValue)
            {
                var directSpecification = new DirectSpecification<TituloPagar>(l => l.FormaPagamento.Value == formaPagamentoCodigo);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<TituloPagar> DocumentoPagamentoContem(string documentoPagamento)
        {
            Specification<TituloPagar> specification = new TrueSpecification<TituloPagar>();

            if (!string.IsNullOrEmpty(documentoPagamento))
            {
                var directSpecification = new DirectSpecification<TituloPagar>(l => l.Movimento.Documento.Contains(documentoPagamento));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<TituloPagar> EhTipoTituloDiferenteDeTituloPai()
        {
            Specification<TituloPagar> specification = new TrueSpecification<TituloPagar>();

            var directSpecification = new DirectSpecification<TituloPagar>(l => l.TipoTitulo != TipoTitulo.Pai);
            specification &= directSpecification;

            return specification;
        }

        public static Specification<TituloPagar> ValorTituloMaiorOuIgualRelContasPagarTitulos(decimal? valor)
        {
            Specification<TituloPagar> specification = new TrueSpecification<TituloPagar>();

            if (valor.HasValue && (valor.Value != 0))
            {
                var directSpecification = new DirectSpecification<TituloPagar>(l => l.ValorTitulo > valor);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<TituloPagar> ValorTituloMenorOuIgualRelContasPagarTitulos(decimal? valor)
        {
            Specification<TituloPagar> specification = new TrueSpecification<TituloPagar>();

            if (valor.HasValue && (valor.Value != 0))
            {
                var directSpecification = new DirectSpecification<TituloPagar>(l => l.ValorTitulo < valor);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<TituloPagar> MatchingMovimentoBancoId(int? bancoId)
        {
            Specification<TituloPagar> specification = new TrueSpecification<TituloPagar>();

            if (bancoId.HasValue)
            {
                var idSpecification = new DirectSpecification<TituloPagar>(l => l.Movimento.ContaCorrente.BancoId == bancoId);
                specification &= idSpecification;
            }

            return specification;
        }

        public static Specification<TituloPagar> MatchingMovimentoContaCorrenteId(int? contaCorrenteId)
        {
            Specification<TituloPagar> specification = new TrueSpecification<TituloPagar>();

            if (contaCorrenteId.HasValue)
            {
                var idSpecification = new DirectSpecification<TituloPagar>(l => l.Movimento.ContaCorrenteId == contaCorrenteId);
                specification &= idSpecification;
            }

            return specification;
        }

        public static Specification<TituloPagar> MatchingMovimentoCaixaId(int? caixaId)
        {
            Specification<TituloPagar> specification = new TrueSpecification<TituloPagar>();

            if (caixaId.HasValue)
            {
                var idSpecification = new DirectSpecification<TituloPagar>(l => l.Movimento.CaixaId == caixaId);
                specification &= idSpecification;
            }

            return specification;
        }

    }
}
