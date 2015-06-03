using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GIR.Sigim.Application.Enums
{
    public enum TabelaBasicaFinanceiro : int
    {
        [Description("Assunto do contato")]
        AssuntoContato = 1,

        [Description("Bairro de Interesse")]
        BairroInteresse = 2,

        [Description("Estado civil")]
        EstadoCivil = 3,

        [Description("Fonte de Negócio")]
        FonteNegocio = 4,

        [Description("Grupo")]
        Grupo = 5,

        [Description("Nacionalidade")]
        Nacionalidade = 6,

        [Description("Parentesco")]
        Parentesco = 7,

        [Description("Profissão")]
        Profissao = 8,

        [Description("Ramo de atividade")]
        RamoAtividade = 9,

        [Description("Relacionamento")]
        Relacionamento = 10,

        [Description("Tipologia")]
        Tipologia = 11,

        [Description("Tratamento")]
        Tratamento = 12,

        [Description("Tipo de área")]
        TipoArea = 13,

        [Description("Tipo de característica")]
        TipoCaracteristica = 14,

        [Description("Tipo de especificação")]
        TipoEspecificacao = 15,
       
    }
}
