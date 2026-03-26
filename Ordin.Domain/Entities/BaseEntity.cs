namespace Ordin.Domain.Entities;

public abstract class BaseEntity(Guid id = default)
{
    public Guid Id { get; private set; } = id;
    public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; }
}
