using Cinema.Application.Dtos;
using Cinema.Application.Utils;
using Cinema.Domain.Common;
using Cinema.Domain.Movies.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Cinema.Application.Actions.Movies.Queries;

internal class MoviesQueriesHandlers :
    IRequestHandler<GetHistories.Request, GetHistories.Response>,
    IRequestHandler<GetAll.Request, GetAll.Response>,
    IRequestHandler<GetById.Request, MovieDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MoviesQueriesHandlers(
        IUnitOfWork unitOfWork,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<GetHistories.Response> Handle(GetHistories.Request request, CancellationToken cancellationToken)
    {
        var movies = _unitOfWork.Movies.GetAll().OrderByDescending(l => l.CreateAt).ToList();
        var pageCount = (int)Math.Ceiling((double)movies.Count / (double)request.Limit);
        movies = movies.Skip(request.Limit * request.Page).Take(request.Limit).ToList();
        return new(
            movies,
            $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}",
            pageCount);
    }



    public async Task<GetAll.Response> Handle(GetAll.Request request, CancellationToken cancellationToken)
    {
        var movies = _unitOfWork.Movies.Search(request.Query, request.Sort, request.Type, request.Genre).ToList();
        var pageCount = (int)Math.Ceiling((double)movies.Count / (double)request.Limit);
        movies = movies.Skip(request.Limit * request.Page).Take(request.Limit).ToList();
        return new(
            movies,
            $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}",
            pageCount);
    }

    public async Task<MovieDto> Handle(GetById.Request request, CancellationToken cancellationToken)
    {
        if (_unitOfWork.Movies.GetById(request.Id) is not Movie movie)
            throw new CinemaError(
                Utils.Errors.CinemaErrorType.VALIDATION_ERROR,
                "Movie with given id is not found.");

        return new(
            movie,
            $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}");
    }
}
