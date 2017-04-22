namespace Lind.Euros.Client
{
    public interface ICommandFactory
    {
        ICommand Create(string raw, string target = null);
    }
}
