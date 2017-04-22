using System;
using System.Threading.Tasks;
using System.Threading;

namespace Lind.Euros.Client
{
    public interface IContext
    {
        IServer Server { get; }
        Task SendMessage(string message, CancellationToken token = default(CancellationToken));
        event EventHandler Closed;
    }
    public interface IOContext<out TMessage> : IContext
        where TMessage : Message
    {
        event Action<TMessage> MessagePosted;
    }
    public interface IIContext<in TMessage> : IContext
        where TMessage : Message
    {
        void PostMessage(TMessage message);
    }
    public interface IContext<TMessage> : IIContext<TMessage>, IOContext<TMessage>
        where TMessage : Message
    { }
    public abstract class Context : IContext
    {
        public Context(IServer server, ICommandFactory commandFactory)
        {
            this.CommandFactory = commandFactory;
            this.Server = server;
        }
        protected ICommandFactory CommandFactory { get; private set; }
        public IServer Server { get; private set; }

        public event EventHandler Closed;
        protected void OnClosed()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
        public virtual Task Close(CancellationToken token = default(CancellationToken))
        {
            OnClosed();
            return Task.FromResult(0);
        }
        public abstract Task SendMessage(string message, CancellationToken token = default(CancellationToken));
    }
    public abstract class Context<TMessage> : Context, IContext<TMessage>
        where TMessage : Message
    {
        public Context(IServer server, ICommandFactory commandFactory)
            : base(server, commandFactory) { }

        public event Action<TMessage> MessagePosted;

        public void PostMessage(TMessage message)
        {
            this.MessagePosted?.Invoke(TransformMessage(message));
        }
        protected virtual TMessage TransformMessage(TMessage message)
        {
            return message;
        }
    }
}
