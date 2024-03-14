using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Domain.Common;

public abstract class EntityBase
{
    [Key]
    public string Id { get; private set; }

    [NotMapped]
    public List<IDomainEvent> DomainEvents { get; } = new List<IDomainEvent>();

    protected EntityBase(string id)
    {
        Id = id;
    }

    public void RaiseEvent(IDomainEvent @event)
    {
        DomainEvents.Add(@event);
    }
}
