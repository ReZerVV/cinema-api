using Cinema.Domain.Common;
using MediatR;

namespace Cinema.Application.Genres.Actions;

internal class GenresActionsHandler :
    IRequestHandler<Queries.GetAll.Request, Queries.GetAll.Response>
{
    private readonly IUnitOfWork _unitOfWork;

    public GenresActionsHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Queries.GetAll.Response> Handle(Queries.GetAll.Request request, CancellationToken cancellationToken)
    {
        return new(_unitOfWork.Genres.GetAll());
    }
}
