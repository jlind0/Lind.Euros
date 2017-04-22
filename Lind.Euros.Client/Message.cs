using System;
using System.Collections.Generic;
using System.Text;

namespace Lind.Euros.Client
{
    public class Message
    {
        public DateTime TimeStamp { get; private set; }
        public string Raw { get; private set; }
        public Message(string raw, DateTime? timeStamp = null)
        {
            this.Raw = raw;
            this.TimeStamp = timeStamp ?? DateTime.Now;
        }
    }
    public class FormattedMessage : Message
    {
        public string Formatted { get; private set; }
        public FormattedMessage(string raw, string formatted, DateTime timeStamp)
            : base(raw, timeStamp)
        {
            this.Formatted = formatted;
        }
    }
    public class UserMessage : FormattedMessage
    {
        public User User { get; private set; }
        public UserMessage(string raw, string formatted, User user, DateTime timeStamp)
            : base(raw, formatted, timeStamp)
        {
            this.User = user;
        }
    }
    public class ChannelMessage : UserMessage
    {
        public IChannel Channel { get; private set; }
        public ChannelMessage(string raw, string formatted, User user, IChannel channel, DateTime timeStamp)
            : base(raw, formatted, user, timeStamp)
        {
            this.Channel = channel;
        }
    }
}
