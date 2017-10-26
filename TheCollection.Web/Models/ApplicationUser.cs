namespace TheCollection.Web.Models {

    using System.Collections.Generic;
    using AspNetCore.Identity.DocumentDb;

    public interface IApplicationUser {
        string Id { get; }
        IList<DocumentDbIdentityRole> Roles { get; }
    }

    public class ApplicationUser : DocumentDbIdentityUser<DocumentDbIdentityRole>, IApplicationUser {
    }
}
