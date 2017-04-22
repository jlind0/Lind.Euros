using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.ObjectModel;

namespace Lind.Euros.Client
{
    public interface IRCClient
    {
        ObservableCollection<IServer> Servers { get; }
        Task Connect(Uri server, string name = null, CancellationToken token = default(CancellationToken));
    }
}
