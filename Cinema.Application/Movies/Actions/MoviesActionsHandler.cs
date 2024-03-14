using Cinema.Domain.Common;
using Cinema.Domain.Genres.Entities;
using Cinema.Domain.Movies.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Cinema.Application.Movies.Actions;

internal class MoviesActionsHandler :
    IRequestHandler<Commands.Create.Request, Commands.Create.Response>,
    IRequestHandler<Commands.Delete.Request, Commands.Delete.Response>,
    IRequestHandler<Commands.Update.Request, Commands.Update.Response>,
    IRequestHandler<Commands.UpdateAll.Request, Commands.UpdateAll.Response>,
    IRequestHandler<Queries.GetDownloads.Request, Queries.GetDownloads.Response>,
    IRequestHandler<Queries.GetAll.Request, Queries.GetAll.Response>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MoviesActionsHandler(
        IUnitOfWork unitOfWork,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Commands.Create.Response> Handle(Commands.Create.Request request, CancellationToken cancellationToken)
    {
        var genres = request.Genres.Select(g =>
        {
            if (_unitOfWork.Genres.GetByName(g) is not Genre genre)
            {
                genre = Genre.Create(g);
                _unitOfWork.Genres.Create(genre);
            }
            return genre;
        });

        var movie = Movie.Create(
            request.Name, request.EnName, request.Description, request.ShortDescription,
            request.Type, request.Year, request.MovieLength, request.Country, request.Rating, request.Votes,
            request.PosterDownloadUrl, request.BackdropDownloadUrl, request.VideoDownloadUrl, genres);
        _unitOfWork.Movies.Create(movie);
        _unitOfWork.SaveChanges();
        
        return new();
    }

    public async Task<Queries.GetDownloads.Response> Handle(Queries.GetDownloads.Request request, CancellationToken cancellationToken)
    {
        return new(_unitOfWork.Movies.GetDownloads());
    }

    public async Task<Queries.GetAll.Response> Handle(Queries.GetAll.Request request, CancellationToken cancellationToken)
    {
        return new(_unitOfWork.Movies.GetAll(), $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}");
    }

    public async Task<Commands.Delete.Response> Handle(Commands.Delete.Request request, CancellationToken cancellationToken)
    {
        if (_unitOfWork.Movies.GetById(request.Id) is not Movie movie)
            throw new Exception("Movie is not found");
        _unitOfWork.Movies.Delete(movie);
        _unitOfWork.SaveChanges();
        return new();
    }

    public async Task<Commands.Update.Response> Handle(Commands.Update.Request request, CancellationToken cancellationToken)
    {
        if (_unitOfWork.Movies.GetById(request.Id) is not Movie movie)
            throw new Exception("Movie is not found");
        if (request.Name != null)
            movie.Name = request.Name;
        if (request.EnName != null)
            movie.EnName = request.EnName;
        if (request.Description != null)
            movie.Description = request.Description;
        if (request.ShortDescription != null)
            movie.ShortDescription = request.ShortDescription;
        if (request.Type != null)
            movie.Type = request.Type;
        if (request.Year != null)
            movie.Year = request.Year ?? 0;
        if (request.MovieLength != null)
            movie.MovieLength = request.MovieLength ?? 0;
        if (request.Country != null)
            movie.Country = request.Country;
        if (request.Rating != null)
            movie.Rating = request.Rating ?? 0.0f;
        if (request.Votes != null)
            movie.Votes = request.Votes ?? 0;
        if (request.PosterDownloadUrl != null)
        {
            var poster = movie.Medias.First(m => m.Type == Domain.Movies.Enums.MediaType.Poster);
            _unitOfWork.Medias.Delete(poster);
            movie.Medias.Remove(poster);
            movie.Medias.Add(Media.Create(movie, request.PosterDownloadUrl, Domain.Movies.Enums.MediaType.Poster));
        }
        if (request.BackdropDownloadUrl != null)
        {
            var backdrop = movie.Medias.First(m => m.Type == Domain.Movies.Enums.MediaType.Backdrop);
            _unitOfWork.Medias.Delete(backdrop);
            movie.Medias.Remove(backdrop);
            movie.Medias.Add(Media.Create(movie, request.BackdropDownloadUrl, Domain.Movies.Enums.MediaType.Backdrop));
        }
        if (request.VideoDownloadUrl != null)
        {
            var video = movie.Medias.First(m => m.Type == Domain.Movies.Enums.MediaType.Video);
            _unitOfWork.Medias.Delete(video);
            movie.Medias.Remove(video);
            movie.Medias.Add(Media.Create(movie, request.VideoDownloadUrl, Domain.Movies.Enums.MediaType.Video));
        }
        if (request.Genres != null)
        {
            var genres = request.Genres.Select(g =>
            {
                if (_unitOfWork.Genres.GetByName(g) is not Genre genre)
                {
                    genre = Genre.Create(g);
                    _unitOfWork.Genres.Create(genre);
                }
                return genre;
            });
            movie.Genres = genres.ToList();
        }
        _unitOfWork.SaveChanges();
        return new();
    }

    public async Task<Commands.UpdateAll.Response> Handle(Commands.UpdateAll.Request request, CancellationToken cancellationToken)
    {
        if (_unitOfWork.Movies.GetById(request.Id) is not Movie movie)
            throw new Exception("Movie is not found");
        movie.Name = request.Name;
        movie.EnName = request.EnName;
        movie.Description = request.Description;
        movie.ShortDescription = request.ShortDescription;
        movie.Type = request.Type;
        movie.Year = request.Year;
        movie.MovieLength = request.MovieLength;
        movie.Country = request.Country;
        movie.Rating = request.Rating;
        movie.Votes = request.Votes;
        if (movie.Medias.First(m => m.Type == Domain.Movies.Enums.MediaType.Poster).Url != request.PosterDownloadUrl)
        {
            var poster = movie.Medias.First(m => m.Type == Domain.Movies.Enums.MediaType.Poster);
            _unitOfWork.Medias.Delete(poster);
            movie.Medias.Remove(poster);
            movie.Medias.Add(Media.Create(movie, request.PosterDownloadUrl, Domain.Movies.Enums.MediaType.Poster));
        }
        if (movie.Medias.First(m => m.Type == Domain.Movies.Enums.MediaType.Backdrop).Url != request.BackdropDownloadUrl)
        {
            var backdrop = movie.Medias.First(m => m.Type == Domain.Movies.Enums.MediaType.Backdrop);
            _unitOfWork.Medias.Delete(backdrop);
            movie.Medias.Remove(backdrop);
            movie.Medias.Add(Media.Create(movie, request.BackdropDownloadUrl, Domain.Movies.Enums.MediaType.Backdrop));
        }
        if (movie.Medias.First(m => m.Type == Domain.Movies.Enums.MediaType.Video).Url != request.VideoDownloadUrl)
        {
            var video = movie.Medias.First(m => m.Type == Domain.Movies.Enums.MediaType.Video);
            _unitOfWork.Medias.Delete(video);
            movie.Medias.Remove(video);
            movie.Medias.Add(Media.Create(movie, request.VideoDownloadUrl, Domain.Movies.Enums.MediaType.Video));
        }
        var genres = request.Genres.Select(g =>
        {
            if (_unitOfWork.Genres.GetByName(g) is not Genre genre)
            {
                genre = Genre.Create(g);
                _unitOfWork.Genres.Create(genre);
            }
            return genre;
        });
        movie.Genres = genres.ToList();
        _unitOfWork.SaveChanges();
        return new();
    }
}
