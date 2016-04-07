using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.Service.Sigim
{
    public interface IModuloSigimAppService
    {
        DateTime AcertaData(int ano, int mes, int dia);
        int ObtemQuantidadeDeMeses(DateTime dataIni, DateTime dataFim);
        int ObtemQuantidadeDeDias(DateTime dataIni, DateTime dataFim);
        DateTime CalculaDataDefasagem(DateTime dataBaseJuros, int defasagemMes, int defasagemDia);
        decimal CalculaPV(decimal valor, decimal juros, DateTime dataInicial, DateTime dataFinal, int qtdDiasProrrata);
        DateTime RecuperaProximoDiaUtil(DateTime data, string siglaUF);
        DateTime RecuperaProximoDiaUtil(DateTime data);
        bool OperacaoEmDia(DateTime dataVencimento, DateTime dataDia);
        decimal AplicaPercentual(decimal valor, Nullable<Decimal> percentual);
        string UnCrypt(string parStrMsg);
        string GetPiece(string parStrText, string parStrDelimiter, long parLngPosicao);
        InformacaoConfiguracaoDTO SetarInformacaoConfiguracao(bool logGirCliente, string enderecoIP, string instancia, string stringConexao);
    }
}
