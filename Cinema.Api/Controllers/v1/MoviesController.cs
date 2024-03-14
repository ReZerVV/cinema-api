using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieCommands = Cinema.Application.Movies.Actions.Commands;
using MovieQueries = Cinema.Application.Movies.Actions.Queries;

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
    public async Task<IActionResult> Create(
        [FromBody] MovieCommands.Create.Request request)
    {
        try
        {
            return Ok(await _mediator.Send(request));
        }
        catch (Exception e)
        {
            return BadRequest(new { Error = new { Message = e.Message } });
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        [FromRoute] string id)
    {
        try
        {
            return Ok(await _mediator.Send(new MovieCommands.Delete.Request(id)));
        }
        catch (Exception e)
        {
            return BadRequest(new { Error = new { Message = e.Message } });
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(
        [FromRoute] string id,
        [FromBody] MovieCommands.Update.Request request)
    {
        try
        {
            request.Id = id;
            return Ok(await _mediator.Send(request));
        }
        catch (Exception e)
        {
            return BadRequest(new { Error = new { Message = e.Message } });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAll(
        [FromRoute] string id,
        [FromBody] MovieCommands.UpdateAll.Request request)
    {
        try
        {
            request.Id = id;
            return Ok(await _mediator.Send(request));
        }
        catch (Exception e)
        {
            return BadRequest(new { Error = new { Message = e.Message } });
        }
    }

    [HttpGet("downloads")]
    public async Task<IActionResult> GetDownloads()
    {
        try
        {
            return Ok(await _mediator.Send(new MovieQueries.GetDownloads.Request()));
        }
        catch (Exception e)
        {
            return BadRequest(new { Error = new { Message = e.Message } });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            return Ok(await _mediator.Send(new MovieQueries.GetAll.Request()));
        }
        catch (Exception e)
        {
            return BadRequest(new { Error = new { Message = e.Message } });
        }
    }
}