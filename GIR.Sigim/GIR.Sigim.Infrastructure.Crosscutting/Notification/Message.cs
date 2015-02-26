using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Infrastructure.Crosscutting.Notification
{
    public class Message
    {
        public string Text { get; private set; }
        public TypeMessage Type { get; private set; }

        public Message(string text, TypeMessage type)
        {
            Text = text;
            Type = type;
        }
    }
}