using Cinema.Domain.Common;
using Cinema.Domain.Genres.Repositories;
using Cinema.Domain.Movies.Repositories;
using Cinema.Infrastructure.Persistence;
using Cinema.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cinema.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddDbContext<CinemaDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("SqlServer")));
        services.AddDbContext<CinemaDbContext>(opt => opt.UseInMemoryDatabase("Cinema.DB"));
        services
            .AddTransient<IGenreRepository, GenreRepository>()
            .AddTransient<IMediaRepository, MediaRepository>()
            .AddTransient<IMovieRepository, MovieRepository>()
            .AddTransient<IUnitOfWork, UnitOfWork>();
        return services;
    }
}