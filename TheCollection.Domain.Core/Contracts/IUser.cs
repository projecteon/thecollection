namespace TheCollection.Domain.Core.Contracts {
    using System.Collections.Generic;

    public interface IUser<TRef> where TRef : IRef {
        string Id { get; }

        string Email { get; }

        ICollection<TRef> Contacts { get; }
    }
}
