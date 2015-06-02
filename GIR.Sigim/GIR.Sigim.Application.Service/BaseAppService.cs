using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Service.Sigim;
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
    }
}