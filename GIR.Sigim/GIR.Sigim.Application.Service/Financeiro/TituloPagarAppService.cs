using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public class TituloPagarAppService : BaseAppService, ITituloPagarAppService
    {
        #region Declaração

        private ITituloPagarRepository tituloPagarRepository;

        #endregion

        #region Construtor

        public TituloPagarAppService(ITituloPagarRepository tituloPagarRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.tituloPagarRepository = tituloPagarRepository;
        }

        #endregion

        #region Métodos ITituloPagarAppService

        public bool ExisteNumeroDocumento(Nullable<DateTime> DataEmissao, Nullable<DateTime> DataVencimento, string NumeroDocumento, int? ClienteId)
        {
            bool existe = false;

            if (!string.IsNullOrEmpty(NumeroDocumento) && (ClienteId.HasValue) && (DataEmissao.HasValue))
            {
                List<TituloPagar> listaTituloPagar;
                string numeroNotaFiscal = RetiraZerosIniciaisNumeroDocumento(NumeroDocumento);

                listaTituloPagar =
                    tituloPagarRepository.ListarPeloFiltro(l => l.ClienteId == ClienteId &&
                                                            l.Documento.EndsWith(NumeroDocumento) &&
                                                            l.DataEmissaoDocumento.Year == DataEmissao.Value.Year &&
                                                            ((DataVencimento == null) || ((DataVencimento != null) && (l.DataVencimento == DataVencimento)))).ToList<TituloPagar>();
                if (listaTituloPagar.Count() > 0)
                {
                    string numeroDeZerosIniciais;

                    foreach (var item in listaTituloPagar)
                    {
                        if ((item.Situacao != SituacaoTituloPagar.Cancelado) && (item.TipoTitulo != TipoTitulo.Pai))
                        {
                            var quantidadeDeZerosIniciais = item.Documento.Length - numeroNotaFiscal.Length;
                            numeroDeZerosIniciais = item.Documento.Substring(0, quantidadeDeZerosIniciais);
                            if (string.IsNullOrEmpty(numeroDeZerosIniciais))
                            {
                                numeroDeZerosIniciais = "0";
                            }
                            int resultado;
                            if (int.TryParse(numeroDeZerosIniciais, out resultado))
                            {
                                if (Convert.ToInt32(resultado) == 0)
                                {
                                    existe = true;
                                    break;
                                }
                            }

                        }
                    }
                }
            }

            return existe;
        }

        #endregion
    }
}
