using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.ObjectModel;
namespace Lind.Euros.Client
{
    public interface IServer : IContext<FormattedMessage>, INotifyPropertyChanged, IDisposable
    {
        Uri Address { get; }
        string Name { get; set; }
        ObservableCollection<IChannel> Channels { get; }
        ObservableCollection<IQuery> Queries { get; }
        User User { get; }
        Task<IChannel> Join(string channel, CancellationToken token = default(CancellationToken));
        IQuery Query(string nick);
        IQuery Query(User user);
        void Ignore(User user);
        void IgnoreHost(string host);
        void Ignore(string nick);
        Task Disconnect(CancellationToken token = default(CancellationToken));
        Task SendCommand(ICommand cmd, CancellationToken token = default(CancellationToken));
        bool IsConnected { get; }
    }
}
