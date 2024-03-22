using Cinema.Domain.Common;
using Cinema.Domain.Genres.Entities;
using Cinema.Domain.Movies.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Infrastructure.Persistence;

public class CinemaDbContext : DbContext
{
    private readonly IMediator _mediator;

    public DbSet<Genre> Genres { get; set; }
    public DbSet<Media> Medias { get; set; }
    public DbSet<Movie> Movies { get; set; }



    public CinemaDbContext(
        DbContextOptions options,
        IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }

    public override int SaveChanges()
    {
        var result = base.SaveChanges();
        _dispatchDomainEvents().GetAwaiter().GetResult();
        return result;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        await _dispatchDomainEvents();
        return result;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>(e =>
        {
            e.HasMany(e => e.Medias).WithOne(g => g.Movie).HasForeignKey(m => m.MovieId);
            e.HasMany(e => e.Genres).WithMany(g => g.Movies);
        });

        base.OnModelCreating(modelBuilder);
    }

    private async Task _dispatchDomainEvents()
    {
        var domainEventEntities = ChangeTracker.Entries<EntityBase>()
            .Select(po => po.Entity)
            .Where(po => po.DomainEvents.Any())
            .ToArray();

        foreach (var entity in domainEventEntities)
        {
            var events = entity.DomainEvents.ToArray();
            entity.DomainEvents.Clear();
            foreach (var entityDomainEvent in events)
                await _mediator.Publish(entityDomainEvent);
        }
    }
}
