using Cinema.Domain.Common;
using Cinema.Domain.Genres.Entities;

namespace Cinema.Domain.Genres.Repositories;

public interface IGenreRepository : IRepository<Genre>
{
    Genre? GetByName(string name);
}
