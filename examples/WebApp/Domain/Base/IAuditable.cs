namespace WebApp.Domain.Base;

public interface IAuditable
{
    DateTimeOffset CreatedAt { get; }

    long CreatedBy { get; }

    DateTimeOffset UpdatedAt { get; set; }

    long UpdatedBy { get; set; }

    void InitWith(long createdBy, DateTimeOffset createdAt);
}
