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
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _mediator.Send(new MovieQueries.GetAll.Request()));
    }

    [HttpPost("loadings")]
    public async Task<IActionResult> CreateLoadings(
        [FromBody] MovieCommands.Loading.Request request)
    {
        await _mediator.Send(request);
        return Ok();
    }

    [HttpGet("loadings")]
    public async Task<IActionResult> GetLoadings()
    {
        return Ok(await _mediator.Send(new MovieQueries.GetLoadings.Request()));
    }
}