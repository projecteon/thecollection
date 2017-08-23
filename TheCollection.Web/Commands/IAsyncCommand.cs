using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TheCollection.Web.Commands
{
    public interface IAsyncCommand<T>
    {
        Task<IActionResult> ExecuteAsync(T parameter);
    }

    public interface IAsyncCommand
    {
        Task<IActionResult> ExecuteAsync();
    }
}
