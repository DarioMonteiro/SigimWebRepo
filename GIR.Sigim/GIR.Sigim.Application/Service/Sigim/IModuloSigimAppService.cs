﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.Service.Sigim
{
    public interface IModuloSigimAppService
    {
        DateTime AcertaData(int ano, int mes, int dia);
        int ObtemQuantidadeDeMeses(DateTime dataIni, DateTime dataFim);
        int ObtemQuantidadeDeDias(DateTime dataIni, DateTime dataFim);
        DateTime CalculaDataDefasagem(DateTime dataBaseJuros, int defasagemMes, int defasagemDia);
        decimal CalculaPV(decimal valor, decimal juros, DateTime dataInicial, DateTime dataFinal, int qtdDiasProrrata);
    }
}