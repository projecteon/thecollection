namespace TheCollection.Domain.Contracts {
    using System.Collections.Generic;

    public interface IUser {
        string Id { get; }

        string Email { get; }

        ICollection<RefValue> Contacts { get; }
    }
}
