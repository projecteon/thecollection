namespace TheCollection.Domain.Core.Contracts {
    public interface IOwnedEntity: IEntity {
        string OwnerId { get; set; }
    }
}
