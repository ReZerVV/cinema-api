using Cinema.Application.Utils;
using Cinema.Application.Utils.Api.Kinogo;
using Cinema.Application.Utils.Api.Kinopoisk;
using Cinema.Domain.Common;
using Cinema.Domain.Genres.Entities;
using Cinema.Domain.Movies.Entities;
using MediatR;

namespace Cinema.Application.Actions.Movies.Commands;

internal class MoviesCommandHandlers :
    IRequestHandler<Create.Request>,
    IRequestHandler<Delete.Request>,
    IRequestHandler<Update.Request>,
    IRequestHandler<UpdateAll.Request>,
    IRequestHandler<Loading.Request>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly KinogoApi _kinogoApi;
    private readonly KinopoiskApi _kinopoiskApi;

    public MoviesCommandHandlers(
        IUnitOfWork unitOfWork,
        KinogoApi kinogoApi,
        KinopoiskApi kinopoiskApi)
    {
        _unitOfWork = unitOfWork;
        _kinogoApi = kinogoApi;
        _kinopoiskApi = kinopoiskApi;
    }

    public async Task Handle(Create.Request request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task Handle(Delete.Request request, CancellationToken cancellationToken)
    {
        if (_unitOfWork.Movies.GetById(request.Id) is not Movie movie)
            throw new Exception("Movie is not found");
        _unitOfWork.Movies.Delete(movie);
        _unitOfWork.SaveChanges();
    }

    public async Task Handle(Update.Request request, CancellationToken cancellationToken)
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
    }

    public async Task Handle(UpdateAll.Request request, CancellationToken cancellationToken)
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
    }

    public async Task Handle(Loading.Request request, CancellationToken cancellationToken)
    {
        (string kinopoiskId, string videoDownloadUrl) = _kinogoApi.GetKpIdAndDownloadUrlByUrl(request.Url) ?? (null, null);
        var filmInfo = await _kinopoiskApi.GetFilmById(kinopoiskId);
        var movie = Movie.Create(
            filmInfo.Name, filmInfo.AlternativeName, filmInfo.Description, filmInfo.ShortDescription,
            filmInfo.Type, filmInfo.Year, filmInfo.MovieLength, string.Join(", ", filmInfo.Countries.Select(country => country.Name)),
            filmInfo.Rating.Imdb, (int)filmInfo.Votes.Imdb,
            filmInfo.Poster.Url, filmInfo.Backdrop.Url, videoDownloadUrl,
            filmInfo.Genres.Select(g =>
            {
                if (_unitOfWork.Genres.GetByName(g.Name) is not Genre genre)
                    genre = Genre.Create(g.Name);
                return genre;
            }),
            int.Parse(kinopoiskId));
        _unitOfWork.Movies.Create(movie);
        _unitOfWork.SaveChanges();
    }
}
