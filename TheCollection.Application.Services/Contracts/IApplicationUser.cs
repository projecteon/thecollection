namespace TheCollection.Application.Services.Contracts {
    using System.Collections.Generic;
    using TheCollection.Domain.Core.Contracts;

    public interface IApplicationUser : IUser<Domain.RefValue> {
        IEnumerable<IRole> Roles { get; }
        string DocumentType { get; }
    }
}
