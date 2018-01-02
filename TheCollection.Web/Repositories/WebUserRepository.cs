namespace TheCollection.Web.Repositories {
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using TheCollection.Application.Services.Contracts.Repository;
    using TheCollection.Web.Models;

    public class WebUserRepository : IGetRepository<WebUser> {
        private readonly UserManager<WebUser> _userManager;
        private readonly IHttpContextAccessor _context;

        public WebUserRepository(UserManager<WebUser> userManager, IHttpContextAccessor context) {
            _userManager = userManager;
            _context = context;
        }

        public Task<WebUser> GetItemAsync(string id = null) {
            return _userManager.GetUserAsync(_context.HttpContext.User);
        }
    }
}
