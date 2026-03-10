namespace WebApp.Domain.Base;

public abstract class Entity<T> : IEntity
    where T : struct, IEquatable<T>
{
    private readonly List<IEvent> _domainEvents = [];
    private T _id;
    private int? _requestedHashCode;

    public ICollection<IEvent> DomainEvents => _domainEvents.AsReadOnly();

    public T Id
    {
        get => _id;
        set
        {
            if (value.Equals(default))
            {
                throw new ArgumentException("The ID cannot be the default value.", nameof(Id));
            }

            _id = value;
        }
    }

    public bool IsTransient => Id.Equals(default);

    public static bool operator ==(Entity<T>? left, Entity<T>? right)
    {
        return left?.Equals(right) ?? right is null;
    }

    public static bool operator !=(Entity<T>? left, Entity<T>? right)
    {
        return !(right == left);
    }

    public void AddDomainEvents(IEvent @event)
    {
        _domainEvents.Add(@event);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<T> entity)
        {
            return false;
        }

        if (ReferenceEquals(this, entity))
        {
            return true;
        }

        if (GetType() != entity.GetType())
        {
            return false;
        }

        if (entity.IsTransient || IsTransient)
        {
            return false;
        }

        return entity.Id.Equals(Id);
    }

    public override int GetHashCode()
    {
        if (IsTransient)
        {
            // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
            return base.GetHashCode();
        }

        // ReSharper disable once NonReadonlyMemberInGetHashCode
        _requestedHashCode ??= Id.GetHashCode() ^ 31;

        // ReSharper disable once NonReadonlyMemberInGetHashCode
        return _requestedHashCode.Value;
    }
}
