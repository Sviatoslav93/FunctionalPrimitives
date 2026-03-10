namespace WebApp.Domain.Base;

public abstract class AuditableEntity<T> : Entity<T> where T : struct, IEquatable<T>
{
    public DateTimeOffset CreatedAt { get; private set; }

    public long CreatedBy { get; private set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public long? UpdatedBy { get; set; }

    public void InitWith(long createdBy, DateTimeOffset createdAt)
    {
        CreatedAt = createdAt;
        CreatedBy = createdBy;
    }
}
