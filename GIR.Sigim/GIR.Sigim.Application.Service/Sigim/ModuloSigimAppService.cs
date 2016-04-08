using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Domain.Entity.Sigim;
using Microsoft.VisualBasic;
using GIR.Sigim.Application.DTO.Sigim;


namespace GIR.Sigim.Application.Service.Sigim
{
    public class ModuloSigimAppService : BaseAppService, IModuloSigimAppService
    {

        #region Declaração

        private IUnidadeFederacaoRepository unidadeFederacaoRepository;
        private IFeriadoRepository feriadoRepository;

        #endregion

        #region Construtor

        public ModuloSigimAppService(IUnidadeFederacaoRepository unidadeFederacaoRepository, 
                                     IFeriadoRepository feriadoRepository,
                                     MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.unidadeFederacaoRepository = unidadeFederacaoRepository;
            this.feriadoRepository = feriadoRepository;
        }

        #endregion

        #region IModuloAppService Members

        public DateTime AcertaData(int ano, int mes, int dia)
        {
            int ultimoDia = 0;
            switch (mes)
            {
                case 1 : 
                    ultimoDia = 31;
                    break;
                case 2 : 
                    ultimoDia = 28;
                    break;
                case 3 : 
                    ultimoDia = 31;
                    break;
                case 4 : 
                    ultimoDia = 30;
                    break;
                case 5 : 
                    ultimoDia = 31;
                    break;
                case 6 : 
                    ultimoDia = 30;
                    break;
                case 7 : 
                    ultimoDia = 31;
                    break;
                case 8 : 
                    ultimoDia = 31;
                    break;
                case 9 : 
                    ultimoDia = 30;
                    break;
                case 10 : 
                    ultimoDia = 31;
                    break;
                case 11 : 
                    ultimoDia = 30;
                    break;
                case 12 : 
                    ultimoDia = 31;
                    break;
            }

            if (mes == 2) 
            {
                if ((ano % 4) == 0)  ultimoDia = 29;
            }

            if (dia > ultimoDia){
                dia = ultimoDia;
            }

            DateTime dataRetorno = DateTime.MinValue;

            int anoAux = dataRetorno.Year;
            int mesAux = dataRetorno.Month;
            int diaAux = dataRetorno.Day;

            anoAux = ano - anoAux;
            mesAux = mes - mesAux;
            diaAux = dia - diaAux;

            dataRetorno = dataRetorno.AddYears(anoAux);
            dataRetorno = dataRetorno.AddMonths(mesAux);
            dataRetorno = dataRetorno.AddDays(diaAux);

            return dataRetorno;
        }


        public int ObtemQuantidadeDeMeses(DateTime dataIni, DateTime dataFim)
        {
            return ((dataFim.Month - dataIni.Month) + (12 * (dataFim.Year - dataIni.Year)));
        }

        public int ObtemQuantidadeDeDias(DateTime dataIni, DateTime dataFim)
        {
            return (int)(dataFim.Date - dataIni.Date).TotalDays;
        }

        public DateTime CalculaDataDefasagem(DateTime dataBaseJuros, int defasagemMes, int defasagemDia)
        {
            DateTime dataDefasagem = new DateTime();

            dataDefasagem = dataBaseJuros.Date.AddMonths(defasagemMes).AddDays(defasagemDia);

            return dataDefasagem;
        }

        public decimal CalculaPV(decimal valor, decimal juros, DateTime dataInicial, DateTime dataFinal, int qtdDiasProrrata)
        {
            decimal valorPresente = 0;
            int numeroDeMeses = ObtemQuantidadeDeMeses(dataInicial,dataFinal);
            double valorPotenciacao = (1.0000000000 + (double)juros);
            valorPresente = valor * (1 / Convert.ToDecimal(Math.Pow(valorPotenciacao, numeroDeMeses)));
            double expoente = (qtdDiasProrrata / 30.0000000000);
            decimal valorPresenteProRataDia = valorPresente * (1 / Convert.ToDecimal(Math.Pow(valorPotenciacao , expoente)));

            valorPresente = Math.Round(valorPresenteProRataDia, 5);

            return valorPresente;
        }

        public DateTime RecuperaProximoDiaUtil(DateTime data,string siglaUF)
        {
            bool achouProximoDiaUtil = false;
            DateTime dataUtil = data.AddDays(-1);

            while (!achouProximoDiaUtil)
            {
                dataUtil = dataUtil.AddDays(1);

                UnidadeFederacao unidadeFederacao = 
                    unidadeFederacaoRepository.ListarPeloFiltro(l => l.Sigla == siglaUF,
                                                                l => l.ListaFeriado).FirstOrDefault();
                Feriado feriado = null;

                if (unidadeFederacao != null)
                {
                    feriado = unidadeFederacao.ListaFeriado.Where(l => l.Data.Value.Date == dataUtil.Date).FirstOrDefault();
                }

                if (feriado == null)
                {
                    if ((dataUtil.DayOfWeek != DayOfWeek.Saturday) && (dataUtil.DayOfWeek != DayOfWeek.Sunday))
                    {
                        achouProximoDiaUtil = true;
                    }
                }
            }
            return dataUtil;
        }

        public DateTime RecuperaProximoDiaUtil(DateTime data)
        {
            bool achouProximoDiaUtil = false;
            DateTime dataUtil = data.AddDays(-1);

            while (!achouProximoDiaUtil)
            {
                dataUtil = dataUtil.AddDays(1);

                List<Feriado> listaFeriado =
                    feriadoRepository.ListarPeloFiltro(l => l.Data == dataUtil).ToList<Feriado>();

                if (listaFeriado.Count() == 0)
                {
                    if ((dataUtil.DayOfWeek != DayOfWeek.Saturday) && (dataUtil.DayOfWeek != DayOfWeek.Sunday))
                    {
                        achouProximoDiaUtil = true;
                    }
                }
            }
            return dataUtil;
        }

        public bool OperacaoEmDia(DateTime dataVencimento, DateTime dataDia)
        {         
            if (dataVencimento.Date >= dataDia.Date)
            {
                return true;
            }

            DateTime dataUtil = RecuperaProximoDiaUtil(dataVencimento.Date);
            if (dataUtil.Date >= dataDia.Date)
            {
                return true;
            }

            return false;
        }

        public decimal AplicaPercentual(decimal valor,Nullable<Decimal> percentual)
        {
            decimal valorRetorno = 0;

	        if (percentual.HasValue)
            {
                valorRetorno = Convert.ToDecimal(valor * (percentual / 100));
            }

            return Math.Round(valorRetorno,5);
        }

        public string UnCrypt(string parStrMsg)
        {
            //Programa que descriptografa strings
            string vUnCrypt;
            vUnCrypt = "";
            for (int i = 0; i <= parStrMsg.Length - 1; i++)
            {
                //Feito em VB para ser compativel com o Desktop, 
                //pois utilizando as funcoes do c# , a funcão Asc trazia outro caracter 
                vUnCrypt = vUnCrypt + Strings.Chr((Strings.Asc(Strings.Mid(parStrMsg, (i + 1), 1)) ^ 127) & 127);
                //Feito em VB

                // 1º formar feito em C# 
                //string letra = parStrMsg.Substring(i,1);
                //char tempChar = Convert.ToChar(letra);
                //int ascII = Convert.ToInt32(tempChar);
                ////cambalacho
                //if (ascII == 402)
                //{
                //    ascII = 131;
                //}
                ////acmbalacho
                //int intXOR = ascII ^ 127;
                //char codigoCharLetra = (char)(intXOR & 127);
                //vUnCrypt = vUnCrypt + codigoCharLetra;
                // 1º formar feito em C# 

                // 2º formar feito em C# 
                //vUnCrypt = vUnCrypt + (char)((Convert.ToInt32(Convert.ToChar(parStrMsg.Substring(i, 1))) ^ 127) & 127);
                // 2º formar feito em C# 
            }
            return vUnCrypt;
        }

        public string GetPiece(string parStrText, string parStrDelimiter, long parLngPosicao)
        {
            string[] vetor;
            string sPiece;

            sPiece = "";
            if (Strings.InStr(parStrText, parStrDelimiter) > 0)
            {
                vetor = Strings.Split(parStrText, parStrDelimiter);
                if (parLngPosicao <= vetor.Length)
                {
                    sPiece = vetor[parLngPosicao - 1];
                }
            }
            return sPiece;
        }

        public InformacaoConfiguracaoDTO SetarInformacaoConfiguracao(bool logGirCliente, string hostName)
        {
            InformacaoConfiguracaoDTO informacaoConfiguracao = new InformacaoConfiguracaoDTO();

            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            System.Data.SqlClient.SqlConnectionStringBuilder connectionStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder(connectionString);

            informacaoConfiguracao.HostName = hostName;
            informacaoConfiguracao.LogGirCliente = logGirCliente;
            informacaoConfiguracao.NomeDoBancoDeDados = connectionStringBuilder.InitialCatalog.ToUpper();
            informacaoConfiguracao.Servidor = connectionStringBuilder.DataSource.ToUpper();

            return informacaoConfiguracao;
        }

        public bool ValidaVersaoSigim(string parStrVersao ) 
        {
            int intMajorBD, intMinorBD, intBuildBD, intRevisionBD;
            int intMajorSigim, intMinorSigim, intBuildSigim, intRevisionSigim;
            string strVersaoSigim;

            if (string.IsNullOrEmpty(parStrVersao))
            {
                parStrVersao = "";
            }

            if (parStrVersao == "")
            {
                return false;
            }

            intMajorBD = Convert.ToInt32(GetPiece(parStrVersao, ".", 1));
            intMinorBD = Convert.ToInt32(GetPiece(parStrVersao, ".", 2));
            intBuildBD = Convert.ToInt32(GetPiece(parStrVersao, ".", 3));
            intRevisionBD = Convert.ToInt32(GetPiece(parStrVersao, ".", 4));

            strVersaoSigim = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            intMajorSigim = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major;
            intMinorSigim = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor;
            intBuildSigim = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build;
            intRevisionSigim = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision;


            if ((intMajorSigim != intMajorBD) || (intMinorSigim != intMinorBD) ||
                (intBuildSigim != intBuildBD) || (intRevisionSigim != intRevisionBD))
            {
                return false;
            }

            return  true;
        }

        #endregion


        #region "Métodos privados"


        #endregion

    }
}
