namespace TheCollection.Domain.Core.Contracts {
    using System.Threading.Tasks;

    public interface IAsyncCommandHandler {
        Task<ICommandResult> ExecuteAsync();
    }

    public interface IAsyncCommandHandler<TCommand> where TCommand : ICommand {
        Task<ICommandResult> ExecuteAsync(TCommand command);
    }

    public interface IAsyncCommandHandler<TCommand, TEntity>
            where TCommand : ICommand
             where TEntity : class, IEntity {
        Task<ICommandResult> ExecuteAsync(TCommand command);
    }
}
