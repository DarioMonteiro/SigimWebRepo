using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;

namespace GIR.Sigim.Application.Service.Sigim
{
    public class ModuloSigimAppService : BaseAppService, IModuloSigimAppService
    {

        #region Declaração

        #endregion

        #region Construtor

        public ModuloSigimAppService(MessageQueue messageQueue)
            : base(messageQueue)
        {
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

        #endregion

    }
}
