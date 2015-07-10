using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Domain.Entity;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Infrastructure.Crosscutting.Security;
using GIR.Sigim.Infrastructure.Crosscutting.Validator;
using System.Globalization;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Application.DTO;

namespace GIR.Sigim.Application.Service
{
    public class BaseAppService : IBaseAppService
    {
        protected IEntityValidator Validator;
        protected List<string> validationErrors;
        protected MessageQueue messageQueue;

        private CustomPrincipal usuarioLogado;
        public CustomPrincipal UsuarioLogado
        {
            get
            {
                if (usuarioLogado == null)
                    usuarioLogado = AuthenticationServiceFactory.Create().GetUser();

                return usuarioLogado;
            }
        }

        public BaseAppService(MessageQueue messageQueue)
        {
            if (messageQueue == null)
                throw new ArgumentNullException("messageQueue");

            Validator = EntityValidatorFactory.Create();
            validationErrors = new List<string>();
            this.messageQueue = messageQueue;
        }

        public List<string> ValidationErrors
        {
            get { return validationErrors; }
        }

        protected void QueueExeptionMessages(Exception exception)
        {
            var ex = exception;
            do
            {
                messageQueue.Add(ex.Message, TypeMessage.Error);
                ex = ex.InnerException;
            }
            while (ex != null);
        }

        protected string DiretorioImagemRelatorio
        {
            get
            {
                string diretorio = AppDomain.CurrentDomain.BaseDirectory + "//ImagemRelatorio//";
                if (!System.IO.Directory.Exists(diretorio))
                    System.IO.Directory.CreateDirectory(diretorio);

                return diretorio;
            }
        }

        protected string RetiraZerosIniciaisNumeroDocumento(string NumeroDocumento)
        {
            string numeroNotaFiscalSemZerosIniciais = "";
            string pedaco;
            bool achouNumeroDifZero = false;
            for (int x = 0; x <= (NumeroDocumento.Length - 1); x++)
            {
                pedaco = NumeroDocumento.Substring(x, 1);
                if (!achouNumeroDifZero)
                {
                    if (pedaco == "0") continue;
                    achouNumeroDifZero = true;
                }
                numeroNotaFiscalSemZerosIniciais = numeroNotaFiscalSemZerosIniciais + pedaco;
            }

            return numeroNotaFiscalSemZerosIniciais;
        }

        protected int ComparaDatas(string Data1, string Data2)
        {
            int result = 0;
            DateTime data1,data2;

            if (!DateTime.TryParseExact(Data1,"dd/MM/yyyy",CultureInfo.InvariantCulture,DateTimeStyles.None, out data1))
            {
                return 1;
            }

            if (!DateTime.TryParseExact(Data2,"dd/MM/yyyy",CultureInfo.InvariantCulture,DateTimeStyles.None, out data2))
            {
                return -1;
            }

            if (data1 > data2) result = -1;
            else if (data1 < data2) result = 1;

            return result;
        }

        protected bool ValidaData(string Data)
        {
            DateTime dataValida;

            if (!DateTime.TryParse(Data, out dataValida))
            {
                return false;
            }

            return true;
        }

        protected string PrepararIconeRelatorio(CentroCusto centroCusto, IParametros parametros)
        {
            var caminhoImagem = string.Empty;
            var iconeRelatorio = ObterIconeRelatorio(centroCusto) ?? parametros.IconeRelatorio;

            if (iconeRelatorio != null)
            {
                caminhoImagem = DiretorioImagemRelatorio + Guid.NewGuid().ToString() + ".bmp";
                System.Drawing.Image imagem = iconeRelatorio.ToImage();
                imagem.Save(caminhoImagem, System.Drawing.Imaging.ImageFormat.Bmp);
            }

            return caminhoImagem;
        }

        protected string ObterNomeEmpresa(CentroCusto centroCusto, IParametros parametros)
        {
            if (centroCusto != null)
            {
                var centroCustoEmpresa = centroCusto.ListaCentroCustoEmpresa.FirstOrDefault();
                if (centroCustoEmpresa != null)
                    return centroCustoEmpresa.Cliente.Nome;
            }
            return parametros.Cliente != null ? parametros.Cliente.Nome : string.Empty;
        }

        protected void RemoverIconeRelatorio(string caminhoImagem)
        {
            if (System.IO.File.Exists(caminhoImagem))
                System.IO.File.Delete(caminhoImagem);
        }

        private byte[] ObterIconeRelatorio(CentroCusto centroCusto)
        {
            if (centroCusto == null)
                return null;
            else
            {
                var centroCustoEmpresa = centroCusto.ListaCentroCustoEmpresa.FirstOrDefault();
                if ((centroCustoEmpresa != null) && (centroCustoEmpresa.IconeRelatorio != null))
                    return centroCustoEmpresa.IconeRelatorio;
                else
                    return ObterIconeRelatorio(centroCusto.CentroCustoPai);
            }
        }
    }
}