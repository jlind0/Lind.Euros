using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lind.Euros.Client
{
    public interface IQuery : IContext<UserMessage>
    {
        User OtherUser { get; }
    }
    public class Query : Context<UserMessage>, IQuery
    {
        public Query(IServer server, ICommandFactory commandFactory, User user)
            :base(server, commandFactory)
        {
            this.OtherUser = user;
        }
        public User OtherUser { get; private set; }

        public override Task SendMessage(string message, CancellationToken token = default(CancellationToken))
        {
            return this.Server.SendCommand(this.CommandFactory.Create(message, this.OtherUser.Nick), token);
        }
    }
}
