namespace Cinema.Domain.Common;

public interface IRepository<T>
    where T : EntityBase
{
    T? GetById(string id);
    IEnumerable<T> GetAll();
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
}
