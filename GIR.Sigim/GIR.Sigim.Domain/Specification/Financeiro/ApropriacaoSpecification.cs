using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;


namespace GIR.Sigim.Domain.Specification.Financeiro
{
    public class ApropriacaoSpecification : BaseSpecification<Apropriacao>
    {

        public static Specification<Apropriacao> UsuarioPossuiAcessoAoCentroCusto(int? idUsuario, string modulo)
        {
            Specification<Apropriacao> specification = new TrueSpecification<Apropriacao>();

            if ((idUsuario.HasValue) && (!string.IsNullOrEmpty(modulo)))
            {
                var directSpecification = new DirectSpecification<Apropriacao>(l =>
                    l.CentroCusto.ListaUsuarioCentroCusto.Any(c =>
                            c.UsuarioId == idUsuario && c.Modulo.Nome == modulo && c.CentroCusto.Situacao == "A"));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Apropriacao> EhCentroCustoAtivo()
        {
            Specification<Apropriacao> specification = new TrueSpecification<Apropriacao>();

            var directSpecification = new DirectSpecification<Apropriacao>(l => l.CentroCusto.Situacao == "A");
            specification &= directSpecification;

            return specification;
        }

        public static Specification<Apropriacao> PertenceAoCentroCustoIniciadoPor(string codigoCentroCusto)
        {
            Specification<Apropriacao> specification = new TrueSpecification<Apropriacao>();

            if (!string.IsNullOrEmpty(codigoCentroCusto))
            {
                var directSpecification = new DirectSpecification<Apropriacao>(l => l.CentroCusto.Codigo.StartsWith(codigoCentroCusto));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Apropriacao> EhSituacaoAPagarProvisionado(bool provisionado)
        {
            if (provisionado)
            {
                return new DirectSpecification<Apropriacao>(l => l.TituloPagar.Situacao == SituacaoTituloPagar.Provisionado);
            }

            return new FalseSpecification<Apropriacao>();

        }

        public static Specification<Apropriacao> EhSituacaoAPagarAguardandoLiberacao(bool aguardandoLiberacao)
        {
            if (aguardandoLiberacao)
            {
                return new DirectSpecification<Apropriacao>(l => l.TituloPagar.Situacao == SituacaoTituloPagar.AguardandoLiberacao);
            }

            return new FalseSpecification<Apropriacao>();
        }

        public static Specification<Apropriacao> EhSituacaoAPagarLiberado(bool liberado)
        {
            if (liberado)
            {
                return new DirectSpecification<Apropriacao>(l => l.TituloPagar.Situacao == SituacaoTituloPagar.Liberado);
            }

            return new FalseSpecification<Apropriacao>();

        }

        public static Specification<Apropriacao> EhSituacaoAPagarCancelado(bool cancelado)
        {
            //Specification<Apropriacao> specification;
            //if (cancelado)
            //{
            //    specification = new TrueSpecification<Apropriacao>();
            //    var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloPagar.Situacao == SituacaoTituloPagar.Cancelado);
            //    specification &= directSpecification;
            //}
            //else
            //{
            //    specification = new FalseSpecification<Apropriacao>();
            //}
            //return specification;

            if (cancelado)
            {
                return new DirectSpecification<Apropriacao>(l => l.TituloPagar.Situacao == SituacaoTituloPagar.Cancelado);
            }

            return new FalseSpecification<Apropriacao>();

        }

        public static Specification<Apropriacao> EhSituacaoAPagarEmitido(bool emitido)
        {
            if (emitido)
            {
                return new DirectSpecification<Apropriacao>(l => l.TituloPagar.Situacao == SituacaoTituloPagar.Emitido);
            }

            return new FalseSpecification<Apropriacao>();
        }

        public static Specification<Apropriacao> EhSituacaoAPagarPago(bool pago)
        {
            if (pago)
            {
                return new DirectSpecification<Apropriacao>(l => l.TituloPagar.Situacao == SituacaoTituloPagar.Pago);
            }

            return new FalseSpecification<Apropriacao>();
        }

        public static Specification<Apropriacao> EhSituacaoAPagarBaixado(bool baixado)
        {
            if (baixado)
            {
                return new DirectSpecification<Apropriacao>(l => l.TituloPagar.Situacao == SituacaoTituloPagar.Baixado);
            }

            return new FalseSpecification<Apropriacao>();
        }

        public static Specification<Apropriacao> EhSituacaoAReceberProvisionado(bool provisionado)
        {
            if (provisionado)
            {
                return new DirectSpecification<Apropriacao>(l => l.TituloReceber.Situacao == SituacaoTituloReceber.Provisionado);
            }

            return new FalseSpecification<Apropriacao>();
        }

        public static Specification<Apropriacao> EhSituacaoAReceberAFaturar(bool aFaturar)
        {
            if (aFaturar)
            {
                return new DirectSpecification<Apropriacao>(l => l.TituloReceber.Situacao == SituacaoTituloReceber.Afaturar);
            }

            return new FalseSpecification<Apropriacao>();
        }

        public static Specification<Apropriacao> EhSituacaoAReceberFaturado(bool faturado)
        {
            if (faturado)
            {
                return new DirectSpecification<Apropriacao>(l => l.TituloReceber.Situacao == SituacaoTituloReceber.Faturado);
            }

            return new FalseSpecification<Apropriacao>();
        }

        public static Specification<Apropriacao> EhSituacaoAReceberPreDatado(bool preDatado)
        {
            if (preDatado)
            {
                return new DirectSpecification<Apropriacao>(l => l.TituloReceber.Situacao == SituacaoTituloReceber.Predatado);
            }

            return new FalseSpecification<Apropriacao>();
        }

        public static Specification<Apropriacao> EhSituacaoAReceberRecebido(bool recebido)
        {
            if (recebido)
            {
                return new DirectSpecification<Apropriacao>(l => l.TituloReceber.Situacao == SituacaoTituloReceber.Recebido);
            }

            return new FalseSpecification<Apropriacao>();
        }

        public static Specification<Apropriacao> EhSituacaoAReceberQuitado(bool quitado)
        {
            if (quitado)
            {
                return new DirectSpecification<Apropriacao>(l => l.TituloReceber.Situacao == SituacaoTituloReceber.Quitado);
            }

            return new FalseSpecification<Apropriacao>();
        }

        public static Specification<Apropriacao> EhSituacaoAReceberCancelado(bool cancelado)
        {
            if (cancelado)
            {
                return new DirectSpecification<Apropriacao>(l => l.TituloReceber.Situacao == SituacaoTituloReceber.Cancelado);
            }

            return new FalseSpecification<Apropriacao>();
        }

        public static Specification<Apropriacao> DataPeriodoTituloPagarMaiorOuIgualPendentesRelApropriacaoPorClasse(string tipoPesquisa, DateTime? data)
        {
            Specification<Apropriacao> specification = new TrueSpecification<Apropriacao>();

            if (tipoPesquisa.ToUpper() == "E")
            {
                if (data.HasValue)
                {
                    var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloPagar.DataEmissaoDocumento >= data);
                    specification &= directSpecification;
                }
            }
            else
            {
                if (data.HasValue)
                {
                    var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloPagar.DataVencimento >= data);
                    specification &= directSpecification;
                }
            }

            return specification;
        }

        public static Specification<Apropriacao> DataPeriodoTituloPagarMaiorOuIgualPagosRelApropriacaoPorClasse(string tipoPesquisa, DateTime? data, bool emitido, bool pago, bool baixado)
        {
            Specification<Apropriacao> specification = new TrueSpecification<Apropriacao>();

            if (tipoPesquisa.ToUpper() == "E")
            {
                if (data.HasValue)
                {
                    var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloPagar.DataEmissaoDocumento >= data);
                    specification &= directSpecification;
                }
            }
            else
            {
                if (tipoPesquisa.ToUpper() == "V")
                {
                    if (data.HasValue)
                    {
                        var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloPagar.DataVencimento >= data);
                        specification &= directSpecification;
                    }
                }
                else
                {
                    if (data.HasValue)
                    {
                        if (emitido)
                        {
                            var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloPagar.DataEmissao >= data);
                            specification &= directSpecification;
                        }
                        if ((pago) && (!emitido))
                        {
                            var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloPagar.DataPagamento >= data);
                            specification &= directSpecification;
                        }
                        if ((baixado) && (!emitido) && (!pago))
                        {
                            var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloPagar.DataBaixa >= data);
                            specification &= directSpecification;
                        }
                    }
                }
            }

            return specification;
        }

        public static Specification<Apropriacao> DataPeriodoPorEmissaoTituloPagarMaiorOuIgualRelAcompanhamentoFinanceiro(DateTime? data)
        {
            Specification<Apropriacao> specification = new TrueSpecification<Apropriacao>();

            var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloPagar.DataEmissao >= data);
            specification &= directSpecification;

            return specification;
        }

        public static Specification<Apropriacao> DataPeriodoPorVencimentoTituloPagarMaiorRelAcompanhamentoFinanceiro(DateTime? data)
        {
            Specification<Apropriacao> specification = new TrueSpecification<Apropriacao>();

            var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloPagar.DataVencimento > data);
            specification &= directSpecification;

            return specification;
        }

        public static Specification<Apropriacao> DataPeriodoPorEmissaoTituloPagarMaiorRelAcompanhamentoFinanceiro(DateTime? data)
        {
            Specification<Apropriacao> specification = new TrueSpecification<Apropriacao>();

            var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloPagar.DataEmissao > data);
            specification &= directSpecification;

            return specification;
        }

        public static Specification<Apropriacao> DataPeriodoTituloPagarMenorOuIgualPendentesRelApropriacaoPorClasse(string tipoPesquisa, DateTime? data)
        {
            Specification<Apropriacao> specification = new TrueSpecification<Apropriacao>();

            DateTime dataUltimaHora = new DateTime(data.Value.Year, data.Value.Month, data.Value.Day, 23, 59, 59);

            if (tipoPesquisa.ToUpper() == "E")
            {
                if (data.HasValue)
                {
                    var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloPagar.DataEmissaoDocumento <= dataUltimaHora);
                    specification &= directSpecification;
                }
            }
            else
            {
                if (data.HasValue)
                {
                    var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloPagar.DataVencimento <= dataUltimaHora);
                    specification &= directSpecification;
                }
            }

            return specification;

        }

        public static Specification<Apropriacao> DataPeriodoTituloPagarMenorOuIgualPagosRelApropriacaoPorClasse(string tipoPesquisa, DateTime? data, bool emitido, bool pago, bool baixado)
        {
            Specification<Apropriacao> specification = new TrueSpecification<Apropriacao>();

            DateTime dataUltimaHora = new DateTime(data.Value.Year, data.Value.Month, data.Value.Day, 23, 59, 59);

            if (tipoPesquisa.ToUpper() == "E")
            {
                if (data.HasValue)
                {
                    var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloPagar.DataEmissaoDocumento <= dataUltimaHora);
                    specification &= directSpecification;
                }
            }
            else
            {
                if (tipoPesquisa.ToUpper() == "V")
                {
                    if (data.HasValue)
                    {
                        var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloPagar.DataVencimento <= dataUltimaHora);
                        specification &= directSpecification;
                    }
                }
                else
                {
                    if (data.HasValue)
                    {
                        if (emitido)
                        {
                            var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloPagar.DataEmissao <= data);
                            specification &= directSpecification;
                        }
                        if ((pago) && (!emitido))
                        {
                            var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloPagar.DataPagamento <= data);
                            specification &= directSpecification;
                        }
                        if ((baixado) && (!emitido) && (!pago))
                        {
                            var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloPagar.DataBaixa <= data);
                            specification &= directSpecification;
                        }
                    }
                }
            }

            return specification;
        }

        public static Specification<Apropriacao> DataPeriodoPorEmissaoTituloPagarMenorOuIgualRelAcompanhamentoFinanceiro(DateTime? data)
        {
            Specification<Apropriacao> specification = new TrueSpecification<Apropriacao>();

            DateTime dataUltimaHora = new DateTime(data.Value.Year, data.Value.Month, data.Value.Day, 23, 59, 59);

            var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloPagar.DataEmissao <= data);
            specification &= directSpecification;

            return specification;
        }

        public static Specification<Apropriacao> DataPeriodoPorVencimentoTituloPagarMenorOuIgualRelAcompanhamentoFinanceiro(DateTime? data)
        {
            Specification<Apropriacao> specification = new TrueSpecification<Apropriacao>();

            DateTime dataUltimaHora = new DateTime(data.Value.Year, data.Value.Month, data.Value.Day, 23, 59, 59);

            var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloPagar.DataVencimento <= data);
            specification &= directSpecification;

            return specification;
        }


        public static Specification<Apropriacao> DataPeriodoTituloReceberMaiorOuIgualPendentesRelApropriacaoPorClasse(string tipoPesquisa, DateTime? data)
        {
            Specification<Apropriacao> specification = new TrueSpecification<Apropriacao>();

            if (tipoPesquisa.ToUpper() == "E")
            {
                if (data.HasValue)
                {
                    var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloReceber.DataEmissaoDocumento >= data);
                    specification &= directSpecification;
                }
            }
            else
            {
                if (data.HasValue)
                {
                    var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloReceber.DataVencimento >= data);
                    specification &= directSpecification;
                }
            }

            return specification;
        }

        public static Specification<Apropriacao> DataPeriodoTituloReceberMenorOuIgualPendentesRelApropriacaoPorClasse(string tipoPesquisa, DateTime? data)
        {
            Specification<Apropriacao> specification = new TrueSpecification<Apropriacao>();

            DateTime dataUltimaHora = new DateTime(data.Value.Year, data.Value.Month, data.Value.Day, 23, 59, 59);

            if (tipoPesquisa.ToUpper() == "E")
            {
                if (data.HasValue)
                {
                    var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloReceber.DataEmissaoDocumento <= dataUltimaHora);
                    specification &= directSpecification;
                }
            }
            else
            {
                if (data.HasValue)
                {
                    var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloReceber.DataVencimento <= dataUltimaHora);
                    specification &= directSpecification;
                }
            }

            return specification;
        }

        public static Specification<Apropriacao> DataPeriodoTituloReceberMaiorOuIgualRecebidosRelApropriacaoPorClasse(string tipoPesquisa, DateTime? data, bool preDatado, bool recebido, bool quitado)
        {
            Specification<Apropriacao> specification = new TrueSpecification<Apropriacao>();

            if (tipoPesquisa.ToUpper() == "E")
            {
                if (data.HasValue)
                {
                    var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloReceber.DataEmissaoDocumento >= data);
                    specification &= directSpecification;
                }
            }
            else
            {
                if (tipoPesquisa.ToUpper() == "V")
                {
                    if (data.HasValue)
                    {
                        var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloReceber.DataVencimento >= data);
                        specification &= directSpecification;
                    }
                }
                else
                {
                    if (data.HasValue)
                    {
                        if ((recebido) || (preDatado))
                        {
                            var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloReceber.DataRecebimento >= data);
                            specification &= directSpecification;
                        }
                        if ((quitado) && (!recebido) && (!preDatado))
                        {
                            var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloReceber.DataBaixa >= data);
                            specification &= directSpecification;
                        }
                    }
                }
            }

            return specification;
        }

        public static Specification<Apropriacao> DataPeriodoTituloReceberMenorOuIgualRecebidosRelApropriacaoPorClasse(string tipoPesquisa, DateTime? data, bool preDatado, bool recebido, bool quitado)
        {
            Specification<Apropriacao> specification = new TrueSpecification<Apropriacao>();

            DateTime dataUltimaHora = new DateTime(data.Value.Year, data.Value.Month, data.Value.Day, 23, 59, 59);

            if (tipoPesquisa.ToUpper() == "E")
            {
                if (data.HasValue)
                {
                    var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloReceber.DataEmissaoDocumento <= dataUltimaHora);
                    specification &= directSpecification;
                }
            }
            else
            {
                if (tipoPesquisa.ToUpper() == "V")
                {
                    if (data.HasValue)
                    {
                        var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloReceber.DataVencimento <= dataUltimaHora);
                        specification &= directSpecification;
                    }
                }
                else
                {
                    if (data.HasValue)
                    {
                        if ((recebido) || (preDatado))
                        {
                            var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloReceber.DataRecebimento <= data);
                            specification &= directSpecification;
                        }
                        if ((quitado) && (!recebido) && (!preDatado))
                        {
                            var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloReceber.DataRecebimento <= data);
                            specification &= directSpecification;
                        }
                    }
                }
            }

            return specification;
        }

        public static Specification<Apropriacao> EhTipoTituloDiferenteDeTituloPai()
        {
            Specification<Apropriacao> specification = new TrueSpecification<Apropriacao>();

            var directSpecification = new DirectSpecification<Apropriacao>(l => l.TituloPagar.TipoTitulo != TipoTitulo.Pai );
            specification &= directSpecification;

            return specification;
        }

        public static Specification<Apropriacao> EhClasseExistente(string codigoClasse)
        {
            Specification<Apropriacao> specification = new TrueSpecification<Apropriacao>();

            if (!string.IsNullOrEmpty(codigoClasse))
            {
                var directSpecification = new DirectSpecification<Apropriacao>(l => l.CodigoClasse == codigoClasse);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Apropriacao> SaoClassesExistentes(string[] codigosClasse)
        {
            Specification<Apropriacao> specification = new TrueSpecification<Apropriacao>();
            var idSpecification = new DirectSpecification<Apropriacao>(l => codigosClasse.Any(o => o == l.CodigoClasse));
            specification &= idSpecification;

            return specification;
        }

        public static Specification<Apropriacao> EhValorDoMovimentoNegativo()
        {
            Specification<Apropriacao> specification = new TrueSpecification<Apropriacao>();

            var directSpecification = new DirectSpecification<Apropriacao>(l => l.Movimento.Valor < 0);
            specification &= directSpecification;

            return specification;
        }

        public static Specification<Apropriacao> EhValorDoMovimentoPositivo()
        {
            Specification<Apropriacao> specification = new TrueSpecification<Apropriacao>();

            var directSpecification = new DirectSpecification<Apropriacao>(l => l.Movimento.Valor > 0);
            specification &= directSpecification;

            return specification;
        }


        public static Specification<Apropriacao> PossuiCaixaNoMovimento()
        {
            Specification<Apropriacao> specification = new TrueSpecification<Apropriacao>();

            var directSpecification = new DirectSpecification<Apropriacao>(l => l.Movimento.CaixaId.HasValue);
            specification &= directSpecification;

            return specification;
        }

        public static Specification<Apropriacao> PossuiContaCorrenteNoMovimento()
        {
            Specification<Apropriacao> specification = new TrueSpecification<Apropriacao>();

            var directSpecification = new DirectSpecification<Apropriacao>(l => l.Movimento.ContaCorrenteId.HasValue);
            specification &= directSpecification;

            return specification;
        }

        public static Specification<Apropriacao> EhSituacaoMovimentoDiferenteDeEstornado()
        {
            Specification<Apropriacao> specification = new TrueSpecification<Apropriacao>();

            var directSpecification = new DirectSpecification<Apropriacao>(l => l.Movimento.Situacao != "E");
            specification &= directSpecification;

            return specification;
        }

        public static Specification<Apropriacao> EhSituacaoMovimentoLancado(bool lancado)
        {
            if (lancado)
            {
                return new DirectSpecification<Apropriacao>(l => l.Movimento.Situacao == "L");
            }

            return new FalseSpecification<Apropriacao>();
        }

        public static Specification<Apropriacao> EhSituacaoMovimentoConferido(bool conferido)
        {
            if (conferido)
            {
                return new DirectSpecification<Apropriacao>(l => l.Movimento.Situacao == "C");
            }

            return new FalseSpecification<Apropriacao>();
        }

        public static Specification<Apropriacao> DataPeriodoMovimentoMaiorOuIgual(DateTime? data)
        {
            Specification<Apropriacao> specification = new TrueSpecification<Apropriacao>();

            if (data.HasValue)
            {
                var directSpecification = new DirectSpecification<Apropriacao>(l => l.Movimento.DataMovimento >= data);
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<Apropriacao> EhTipoMovimentoOperacaoDebito()
        {
            return new DirectSpecification<Apropriacao>(l => l.Movimento.TipoMovimento.Operacao == "D");
        }

        public static Specification<Apropriacao> EhTipoMovimentoCadastradoManualmente()
        {
            return new DirectSpecification<Apropriacao>(l => l.Movimento.TipoMovimento.Automatico == false);
        }



        public static Specification<Apropriacao> DataPeriodoMovimentoMenorOuIgual(DateTime? data)
        {
            Specification<Apropriacao> specification = new TrueSpecification<Apropriacao>();

            DateTime dataUltimaHora = new DateTime(data.Value.Year, data.Value.Month, data.Value.Day, 23, 59, 59);

            if (data.HasValue)
            {
                var directSpecification = new DirectSpecification<Apropriacao>(l => l.Movimento.DataMovimento <= dataUltimaHora);
                specification &= directSpecification;
            }
            return specification;

        }

        public static Specification<Apropriacao> EhMovimentoDiferenteDeCredCob()
        {
            Specification<Apropriacao> specification = new TrueSpecification<Apropriacao>();

            //TipoMovimentoId == 12 refere-se a sistema de crédito cobrança
            var directSpecification = new DirectSpecification<Apropriacao>(l => l.Movimento.TipoMovimentoId != 12);
            specification &= directSpecification;

            return specification;
        }


    }
}
