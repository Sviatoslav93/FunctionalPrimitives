namespace WebApp.Domain.Base;

public interface IEntity
{
    ICollection<IEvent> DomainEvents { get; }

    void AddDomainEvents(IEvent @event);

    void ClearDomainEvents();
}

public interface IEvent
{
}
