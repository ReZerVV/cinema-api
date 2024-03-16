using Cinema.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Cinema.Application.Actions.Movies.Queries;

internal class MoviesQueriesHandlers :
    IRequestHandler<GetLoadings.Request, GetLoadings.Response>,
    IRequestHandler<GetAll.Request, GetAll.Response>
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

    public async Task<GetLoadings.Response> Handle(GetLoadings.Request request, CancellationToken cancellationToken)
    {
        return new(
            _unitOfWork.Movies.GetDownloads(),
            $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}");
    }

    public async Task<GetAll.Response> Handle(GetAll.Request request, CancellationToken cancellationToken)
    {
        return new(
            _unitOfWork.Movies.GetAll(),
            $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}");
    }
}
