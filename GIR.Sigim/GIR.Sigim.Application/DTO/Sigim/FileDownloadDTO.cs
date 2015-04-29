using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Sigim
{
    public class FileDownloadDTO
    {
        public Stream Stream { get; private set; }
        public FormatoExportacaoArquivo FormatoExportacaoArquivo { get; private set; }
        public string Nome { get; private set; }

        public string NomeComExtensao
        {
            get { return Nome + Extensao; }
        }

        public string Extensao
        {
            get
            {
                switch (FormatoExportacaoArquivo)
                {
                    case FormatoExportacaoArquivo.RTF:
                        return ".rtf";
                    case FormatoExportacaoArquivo.MSWord:
                        return ".doc";
                    case FormatoExportacaoArquivo.MSExcel:
                        return ".xls";
                    case FormatoExportacaoArquivo.PDF:
                        return ".pdf";
                    case FormatoExportacaoArquivo.MSExcelRecord:
                        return ".xls";
                    case FormatoExportacaoArquivo.CSV:
                        return ".csv";
                    default:
                        return string.Empty;
                }
            }
        }

        public string ContentType
        {
            get
            {
                switch (FormatoExportacaoArquivo)
                {
                    case FormatoExportacaoArquivo.RTF:
                        return "application/rtf";
                    case FormatoExportacaoArquivo.MSWord:
                        return "application/msword";
                    case FormatoExportacaoArquivo.MSExcel:
                        return "application/vnd.ms-excel";
                    case FormatoExportacaoArquivo.PDF:
                        return "application/pdf";
                    case FormatoExportacaoArquivo.MSExcelRecord:
                        return "application/vnd.ms-excel";
                    case FormatoExportacaoArquivo.CSV:
                        return "text/csv";
                    default:
                        return string.Empty;
                }
            }
        }

        public FileDownloadDTO(string nome, Stream stream, FormatoExportacaoArquivo formatoExportacaoArquivo)
        {
            Nome = nome;
            Stream = stream;
            FormatoExportacaoArquivo = formatoExportacaoArquivo;
        }
    }
}