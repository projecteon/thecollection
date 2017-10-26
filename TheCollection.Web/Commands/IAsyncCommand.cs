using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TheCollection.Web.Commands {

    public interface IAsyncCommand<T> {

        Task<IActionResult> ExecuteAsync(T parameter);
    }

    public interface IAsyncCommand {

        Task<IActionResult> ExecuteAsync();
    }
}
