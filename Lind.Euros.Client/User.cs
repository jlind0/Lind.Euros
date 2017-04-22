using System.ComponentModel;

namespace Lind.Euros.Client
{
    public class User : INotifyPropertyChanged
    {
        protected IServer Server { get; private set; }
        public User(IServer server, string host, string nick)
        {
            this.Server = server;
            this.Nick = nick;
            this.Host = host;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private string nick;
        public string Nick
        {
            get { return this.nick; }
            set
            {
                if (this.nick != value)
                {
                    this.nick = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Nick"));
                }
            }
        }
        public string Host { get; private set; }
    }
}
