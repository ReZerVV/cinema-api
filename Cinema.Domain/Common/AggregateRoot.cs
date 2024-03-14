namespace Cinema.Domain.Common;

public abstract class AggregateRoot : EntityBase
{
    protected AggregateRoot(string id) : base(id)
    {

    }
}
