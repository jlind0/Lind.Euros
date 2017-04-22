using System.ComponentModel;

namespace Lind.Euros.Client
{
    public class User : INotifyPropertyChanged
    {
        public User(string host, string nick, string name)
        {
            this.Nick = nick;
            this.Host = host;
            this.Name = name;
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
        public string Name { get; private set; }
    }
}
