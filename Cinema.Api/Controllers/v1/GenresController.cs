using MediatR;
using Microsoft.AspNetCore.Mvc;
using GenresQueries = Cinema.Application.Genres.Actions.Queries;

namespace Cinema.Api.Controllers.v1;

[ApiController]
[Route("api/v1/genres")]
public class GenresController : ControllerBase
{
    private readonly IMediator _mediator;

    public GenresController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(_mediator.Send(new GenresQueries.GetAll.Request()));
    }
}
