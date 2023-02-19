namespace Stack;

public record Item<T>(T Value)
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime DateTimeCreated { get; } = DateTime.UtcNow;
}