namespace School.DAL.Entities;

public record class EntityBase : IEntity
{
    public Guid Id { get; set; }
}