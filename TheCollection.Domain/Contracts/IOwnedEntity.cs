namespace TheCollection.Domain.Contracts {
    public interface IOwnedEntity: IEntity {
        string OwnerId { get; set; }
    }
}
