namespace TheCollection.Application.Services.Contracts {
    using System.Threading.Tasks;

    public interface IAsyncCommand {
        Task<IActivityResult> ExecuteAsync();
    }

    public interface IAsyncCommand<T> {
        Task<IActivityResult> ExecuteAsync(T parameter);
    }

    public interface IAsyncCommand<T1, T2> {
        Task<IActivityResult> ExecuteAsync(T1 parameter1, T2 parameter2);
    }
}
