using Cinema.Domain.Genres.Repositories;
using Cinema.Domain.Movies.Repositories;

namespace Cinema.Domain.Common;

public interface IUnitOfWork
{
    IGenreRepository Genres { get; }
    IMediaRepository Medias { get; }
    IMovieRepository Movies { get; }
    void SaveChanges();
}