namespace TheCollection.Presentation.Web.Repositories {
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;
    using TheCollection.Presentation.Web.Models;

    public class WebUserRepository : IGetRepository<IApplicationUser>, ILinqSearchRepository<IApplicationUser> {
        private readonly UserManager<WebUser> _userManager;
        private readonly IHttpContextAccessor _context;

        public WebUserRepository(UserManager<WebUser> userManager, IHttpContextAccessor context) {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IApplicationUser> GetItemAsync(string id = null) {
            return await _userManager.GetUserAsync(_context.HttpContext.User);
        }

        public async Task<IEnumerable<IApplicationUser>> SearchItemsAsync(Expression<Func<IApplicationUser, bool>> predicate = null, int pageSize = 0, int page = 0) {
            return await Task.Run(() => {
                return _userManager.Users;
            });
        }
    }
}
