using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.ObjectModel;

namespace Lind.Euros.Client
{
    public interface IChannel : IContext<ChannelMessage>, INotifyPropertyChanged
    {
        string Topic { get; }
        ObservableCollection<User> Users { get; }
        Task Part(string reason = null, CancellationToken token = default(CancellationToken));
        Task Hop(string reason = null, CancellationToken token = default(CancellationToken));
        string Name { get; }
    }
    public class Channel : Context<ChannelMessage>, IChannel
    {
        public Channel(string name, IServer server, ICommandFactory commandFactory) : base(server, commandFactory)
        {
            this.Name = name;
        }
        public string Name { get; private set; }

        private string topic;
        public string Topic
        {
            get { return topic; }
            set
            {
                if (topic != value)
                {
                    topic = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Topic"));
                }
            }
        }

        public ObservableCollection<User> Users { get; } = new ObservableCollection<User>();

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task Hop(string reason = null, CancellationToken token = default(CancellationToken))
        {
            await this.Part(reason, token);
            await Task.Delay(100);
            await this.SendMessage("/join", token);
        }

        public async Task Part(string reason = null, CancellationToken token = default(CancellationToken))
        {
            await this.SendMessage($"/part {reason}", token);
            OnClosed();
        }
        public async override Task Close(CancellationToken token = default(CancellationToken))
        {
            await Part(token: token);
            await base.Close(token);
        }
        public override Task SendMessage(string message, CancellationToken token = default(CancellationToken))
        {
            var cmd = this.CommandFactory.Create(message, this.Name);
            return this.Server.SendCommand(cmd, token);
        }
    }
}
