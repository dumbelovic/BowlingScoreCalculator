using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BowlingScoreCalculator.Api.Features.Scores
{
    [Produces(MediaTypeNames.Application.Json)]
    [Route("[controller]")]
    [ApiController]
    public class ScoresController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ScoresController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ScoresAsync([FromBody] Scores.Request request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
