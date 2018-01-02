namespace TheCollection.Application.Services.Contracts {
    using System.Collections.Generic;
    using TheCollection.Domain.Contracts;

    public interface IApplicationUser : IUser {
        IEnumerable<IRole> Roles { get; }
    }
}
