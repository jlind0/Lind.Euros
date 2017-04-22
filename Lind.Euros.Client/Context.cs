using System;
using System.Threading.Tasks;
using System.Threading;
using System.Reactive;
using System.Reactive.Subjects;
using System.Reactive.Linq;

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
        IObservable<TMessage> OutgoingMessages { get; }
    }
    public interface IIContext<in TMessage> : IContext
        where TMessage : Message
    {
        IObserver<TMessage> IncomingMessages { get; }
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
            : base(server, commandFactory)
        {
            this.IncomingMessageSubject.Subscribe(m => MessageRecieved?.Invoke(TransformMessage(m)), this.TokenSource.Token);
        }
        private CancellationTokenSource TokenSource = new CancellationTokenSource();
        protected Subject<TMessage> IncomingMessageSubject { get; } = new Subject<TMessage>();
        private event Action<TMessage> MessageRecieved;
        public IObservable<TMessage> OutgoingMessages { get { return Observable.FromEvent<TMessage>(x => this.MessageRecieved += x, x => this.MessageRecieved -= x); } }
        public IObserver<TMessage> IncomingMessages { get { return this.IncomingMessageSubject.AsObserver<TMessage>(); } }
        public override Task Close(CancellationToken token = default(CancellationToken))
        {
            this.TokenSource.Cancel();
            return base.Close(token);
        }
        protected virtual TMessage TransformMessage(TMessage incomming)
        {
            return incomming;
        }
    }
}
