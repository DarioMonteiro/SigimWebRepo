using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Infrastructure.Crosscutting.Validator;

namespace GIR.Sigim.Application.Service
{
    public class BaseAppService : IBaseAppService
    {
        protected IEntityValidator Validator;
        protected List<string> validationErrors;
        protected MessageQueue messageQueue;

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
    }
}