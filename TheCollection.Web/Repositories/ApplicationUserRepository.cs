namespace TheCollection.Web.Repositories {
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using TheCollection.Domain.Contracts.Repository;
    using TheCollection.Web.Models;

    public class ApplicationUserRepository : IGetRepository<ApplicationUser> {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _context;

        public ApplicationUserRepository(UserManager<ApplicationUser> userManager, IHttpContextAccessor context) {
            _userManager = userManager;
            _context = context;
        }

        public Task<ApplicationUser> GetItemAsync(string id = null) {
            return _userManager.GetUserAsync(_context.HttpContext.User);
        }
    }
}
