using Cinema.Application.Services.Abstractions;
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
    private readonly IFileService _fileService;

    public MoviesCommandHandlers(
        IUnitOfWork unitOfWork,
        KinogoApi kinogoApi,
        KinopoiskApi kinopoiskApi,
        IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _kinogoApi = kinogoApi;
        _kinopoiskApi = kinopoiskApi;
        _fileService = fileService;
    }

    public async Task Handle(Create.Request request, CancellationToken cancellationToken)
    {
        var movie = Movie.Create(
            request.Name, request.EnName, request.Description, request.ShortDescription,
            request.Type, request.Year, request.MovieLength, request.Country,
            request.Rating, request.Votes,
            request.Genres.Select(gName =>
            {
                if (_unitOfWork.Genres.GetByName(gName) is not Genre genre)
                    genre = Genre.Create(gName);
                return genre;
            }));

        if (request.PosterUrl != null)
        {
            movie.AddMedia(Media.CreateDownload(request.PosterUrl, Domain.Movies.Enums.MediaType.Poster));
        }
        else if (request.Poster != null)
        {
            var posterFileName = Path.Combine("i", $"{Guid.NewGuid()}{Path.GetExtension(request.Poster.FileName)}");
            _fileService.Load(request.Poster, posterFileName);
            movie.AddMedia(Media.Create(posterFileName, Domain.Movies.Enums.MediaType.Poster));
        }
        else
        {
            throw new CinemaError(
                Utils.Errors.CinemaErrorType.VALIDATION_ERROR,
                "A link or file must be provided for the poster.");
        }

        if (request.BackdropUrl != null)
        {
            movie.AddMedia(Media.CreateDownload(request.BackdropUrl, Domain.Movies.Enums.MediaType.Backdrop));
        }
        else if (request.Backdrop != null)
        {
            var backdropFileName = Path.Combine("i", $"{Guid.NewGuid()}{Path.GetExtension(request.Backdrop.FileName)}");
            _fileService.Load(request.Backdrop, backdropFileName);
            movie.AddMedia(Media.Create(backdropFileName, Domain.Movies.Enums.MediaType.Backdrop));
        }
        else
        {
            throw new CinemaError(
                Utils.Errors.CinemaErrorType.VALIDATION_ERROR,
                "A link or file must be provided for the backdrop.");
        }

        if (request.VideoUrl != null)
        {
            movie.AddMedia(Media.CreateDownload(request.VideoUrl, Domain.Movies.Enums.MediaType.Video));
        }
        else if (request.Video != null)
        {
            var videoFileName = Path.Combine("v", $"{Guid.NewGuid()}{Path.GetExtension(request.Video.FileName)}");
            _fileService.Load(request.Video, videoFileName);
            movie.AddMedia(Media.Create(videoFileName, Domain.Movies.Enums.MediaType.Video));
        }
        else
        {
            throw new CinemaError(
                Utils.Errors.CinemaErrorType.VALIDATION_ERROR,
                "A link or file must be provided for the video.");
        }
        
        _unitOfWork.Movies.Create(movie);
        _unitOfWork.SaveChanges();
    }

    public async Task Handle(Delete.Request request, CancellationToken cancellationToken)
    {
        if (_unitOfWork.Movies.GetById(request.Id) is not Movie movie)
            throw new Exception("Movie is not found");
        foreach (var media in movie.Medias)
            if (_fileService.Remove(media.FileName))
                _unitOfWork.Medias.Delete(media);
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
            _fileService.Remove(poster.FileName);
            _unitOfWork.Medias.Delete(poster);
            movie.Medias.Remove(poster);
            movie.AddMedia(Media.CreateDownload(request.PosterDownloadUrl, Domain.Movies.Enums.MediaType.Poster));
        }
        if (request.BackdropDownloadUrl != null)
        {
            var backdrop = movie.Medias.First(m => m.Type == Domain.Movies.Enums.MediaType.Backdrop);
            _fileService.Remove(backdrop.FileName);
            _unitOfWork.Medias.Delete(backdrop);
            movie.Medias.Remove(backdrop);
            movie.AddMedia(Media.CreateDownload(request.BackdropDownloadUrl, Domain.Movies.Enums.MediaType.Backdrop));
        }
        if (request.VideoDownloadUrl != null)
        {
            var video = movie.Medias.First(m => m.Type == Domain.Movies.Enums.MediaType.Video);
            _fileService.Remove(video.FileName);
            _unitOfWork.Medias.Delete(video);
            movie.Medias.Remove(video);
            movie.AddMedia(Media.CreateDownload(request.VideoDownloadUrl, Domain.Movies.Enums.MediaType.Video));
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
            _fileService.Remove(poster.FileName);
            _unitOfWork.Medias.Delete(poster);
            movie.Medias.Remove(poster);
            movie.AddMedia(Media.CreateDownload(request.PosterDownloadUrl, Domain.Movies.Enums.MediaType.Poster));
        }
        if (movie.Medias.First(m => m.Type == Domain.Movies.Enums.MediaType.Backdrop).Url != request.BackdropDownloadUrl)
        {
            var backdrop = movie.Medias.First(m => m.Type == Domain.Movies.Enums.MediaType.Backdrop);
            _fileService.Remove(backdrop.FileName);
            _unitOfWork.Medias.Delete(backdrop);
            movie.Medias.Remove(backdrop);
            movie.AddMedia(Media.CreateDownload(request.BackdropDownloadUrl, Domain.Movies.Enums.MediaType.Backdrop));
        }
        if (movie.Medias.First(m => m.Type == Domain.Movies.Enums.MediaType.Video).Url != request.VideoDownloadUrl)
        {
            var video = movie.Medias.First(m => m.Type == Domain.Movies.Enums.MediaType.Video);
            _fileService.Remove(video.FileName);
            _unitOfWork.Medias.Delete(video);
            movie.Medias.Remove(video);
            movie.AddMedia(Media.CreateDownload(request.VideoDownloadUrl, Domain.Movies.Enums.MediaType.Video));
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
        if (videoDownloadUrl == null)
            throw new CinemaError(Utils.Errors.CinemaErrorType.VALIDATION_ERROR, "Incorrect link");
        var filmInfo = await _kinopoiskApi.GetFilmById(kinopoiskId);
        var movie = Movie.Create(
            filmInfo.Name, filmInfo.AlternativeName, filmInfo.Description, filmInfo.ShortDescription,
            filmInfo.Type, filmInfo.Year, filmInfo.MovieLength, string.Join(", ", filmInfo.Countries.Select(country => country.Name)),
            filmInfo.Rating.Imdb, (int)filmInfo.Votes.Imdb,
            filmInfo.Genres.Select(g =>
            {
                if (_unitOfWork.Genres.GetByName(g.Name) is not Genre genre)
                    genre = Genre.Create(g.Name);
                return genre;
            }));
        movie.AddMedia(Media.CreateDownload(filmInfo.Poster.Url, Domain.Movies.Enums.MediaType.Poster));
        movie.AddMedia(Media.CreateDownload(filmInfo.Backdrop.Url, Domain.Movies.Enums.MediaType.Backdrop));
        movie.AddMedia(Media.CreateDownload(videoDownloadUrl, Domain.Movies.Enums.MediaType.Video));
        _unitOfWork.Movies.Create(movie);
        _unitOfWork.SaveChanges();
    }
}
