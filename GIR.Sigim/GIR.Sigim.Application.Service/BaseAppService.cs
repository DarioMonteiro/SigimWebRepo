using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Infrastructure.Crosscutting.Security;
using GIR.Sigim.Infrastructure.Crosscutting.Validator;

namespace GIR.Sigim.Application.Service
{
    public class BaseAppService : IBaseAppService
    {
        protected IEntityValidator Validator;
        protected List<string> validationErrors;
        protected MessageQueue messageQueue;

        private IAuthenticationService authenticationService;
        protected IAuthenticationService AuthenticationService
        {
            get
            {
                if (authenticationService == null)
                    authenticationService = AuthenticationServiceFactory.Create();

                return authenticationService;
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

    }
}