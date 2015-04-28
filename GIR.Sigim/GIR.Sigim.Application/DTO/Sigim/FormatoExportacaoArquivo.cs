using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Sigim
{
    public enum FormatoExportacaoArquivo
    {
        [Description("Rich Text Format (*.rtf)")]
        RTF = 2,

        [Description("Microsoft Word (*.doc)")]
        MSWord = 3,

        [Description("Microsoft Excel (*.xls)")]
        MSExcel = 4,

        [Description("PDF (*.pdf)")]
        PDF = 5,

        [Description("Microsoft Excel - somente dados (*.xls)")]
        MSExcelRecord = 8,

        [Description("CSV (*.csv)")]
        CSV = 10,
    }
}