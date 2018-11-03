namespace TheCollection.Application.Services.Contracts {
    using System.Collections.Generic;

    public interface IActivity {
        string Id { get; }
        string Name { get; }
        IEnumerable<IRole> ValidRoles { get; }
    }
}
