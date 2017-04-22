using System;
using System.Collections.Generic;
using System.Text;

namespace Lind.Euros.Client
{
    public interface IQuery : IContext<UserMessage>
    {
        User OtherUser { get; }
    }
}
