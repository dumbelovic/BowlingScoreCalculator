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

        /// <summary>
        /// Calculates game score for 10 pin bowling game
        /// </summary>
        /// <param name="request">Collection of pins downed in one throw</param>
        /// <returns>Progress score for each started frame. Game completed indicator</returns>
        /// /// <remarks>
        /// Perfect game example:
        /// 
        /// Request:
        ///
        ///     POST /Scores
        ///     {
        ///        "pinsDowned": [10,10,10,10,10,10,10,10,10,10,10,10]   
        ///     }
        ///
        /// Response:
        ///
        ///     { 
        ///        “frameProgressScores”: [“30”,”60”,”90”,”120”,”150”,”180”, ”210”, ”240”, ”270”, ”300”], 
        ///        "gameCompleted": true, 
        ///     }
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ScoresAsync([FromBody] Scores.Request request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
