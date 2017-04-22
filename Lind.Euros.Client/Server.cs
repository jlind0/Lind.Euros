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
        void Join(string channel);
        void Ignore(User user);
        void Disconnect();
        Task SendCommand(ICommand cmd, CancellationToken token = default(CancellationToken));
    }
}
