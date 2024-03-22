using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieCommands = Cinema.Application.Actions.Movies.Commands;
using MovieQueries = Cinema.Application.Actions.Movies.Queries;

namespace Cinema.Api.Controllers.v1;

[ApiController]
[Route("api/v1/movies/")]
public class MoviesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MoviesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [RequestSizeLimit(10737418240)]
    public async Task<IActionResult> Create(
        [FromForm] MovieCommands.Create.Request request)
    {
        await _mediator.Send(request);
        return Ok();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        [FromRoute] string id)
    {
        await _mediator.Send(new MovieCommands.Delete.Request(id));
        return Ok();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(
        [FromRoute] string id,
        [FromBody] MovieCommands.Update.Request request)
    {
        request.Id = id;
        await _mediator.Send(request);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAll(
        [FromRoute] string id,
        [FromBody] MovieCommands.UpdateAll.Request request)
    {
        request.Id = id;
        await _mediator.Send(request);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int limit = 10,
        [FromQuery] int page = 0,
        [FromQuery] string? query = null,
        [FromQuery] string? type = null,
        [FromQuery] string? sort = null,
        [FromQuery] string? genre = null)
    {
        return Ok(await _mediator.Send(new MovieQueries.GetAll.Request(limit, page)
        {
            Query = query,
            Type = type,
            Sort = sort,
            Genre = genre
        }));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        [FromRoute] string id)
    {
        return Ok(await _mediator.Send(new MovieQueries.GetById.Request(id)));
    }

    [HttpPost("loadings")]
    public async Task<IActionResult> CreateLoadings(
        [FromBody] MovieCommands.Loading.Request request)
    {
        await _mediator.Send(request);
        return Ok();
    }

    [HttpGet("histories")]
    public async Task<IActionResult> GetHistories(
        [FromQuery] int limit,
        [FromQuery] int page)
    {
        return Ok(await _mediator.Send(new MovieQueries.GetHistories.Request(limit, page)));
    }
}