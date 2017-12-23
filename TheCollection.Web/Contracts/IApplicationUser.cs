namespace TheCollection.Web.Contracts {
    using System.Collections.Generic;
    using AspNetCore.Identity.DocumentDb;
    using TheCollection.Domain.Contracts;

    public interface IApplicationUser: IUser {
        IList<DocumentDbIdentityRole> Roles { get; }
    }
}
