using Cinema.Domain.Common;
using Cinema.Domain.Genres.Repositories;
using Cinema.Domain.Movies.Repositories;

namespace Cinema.Infrastructure.Persistence;

internal class UnitOfWork : IUnitOfWork
{
    private readonly CinemaDbContext _db;

    public IGenreRepository Genres { get; private set; }
    public IMediaRepository Medias { get; private set; }
    public IMovieRepository Movies { get; private set; }

    public UnitOfWork(
        IGenreRepository genreRepository,
        IMediaRepository mediaRepository,
        IMovieRepository movieRepository,
        CinemaDbContext cinemaDbContext)
    {
        _db = cinemaDbContext;
        this.Genres = genreRepository;
        this.Medias = mediaRepository;
        this.Movies = movieRepository;
    }

    public void SaveChanges()
    {
        _db.SaveChanges();
    }
}
