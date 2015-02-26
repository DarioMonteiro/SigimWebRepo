using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Infrastructure.Crosscutting.Notification
{
    public class MessageQueue
    {
        private List<Message> notificationMessages;

        public MessageQueue()
        {
            notificationMessages = new List<Message>();
        }

        public void Clear()
        {
            notificationMessages.Clear();
        }

        public List<Message> GetAll()
        {
            return notificationMessages;
        }

        public void Add(string text, TypeMessage type)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException("message");

            notificationMessages.Add(new Message(text, type));
        }

        public void AddRange(List<string> messages, TypeMessage type)
        {
            if (messages == null)
                throw new ArgumentNullException("messages");

            foreach (var text in messages.Where(l => !string.IsNullOrEmpty(l)))
                this.Add(text, type);
        }
    }
}