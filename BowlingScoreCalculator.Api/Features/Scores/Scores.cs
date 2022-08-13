using FluentValidation;
using MediatR;

namespace BowlingScoreCalculator.Api.Features.Scores
{
    public static class Scores
    {
        public class Request : IRequest<Response>
        {
            public List<byte> PinsDowned { get; set; } = new List<byte>();
        }

        public class Response
        {
            public List<string> FrameProgressScores { get; set; } = new List<string>();

            public bool GameCompleted { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(r => r.PinsDowned).NotEmpty().WithMessage("PinsDowned collection can not be empty.");
            }
        }

        public class ScoresHandler : IRequestHandler<Request, Response>
        {
            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var game = new Domain.TenPinBowlingGame();

                foreach (var pinsDowned in request.PinsDowned)
                {
                    game.ThrowBall(pinsDowned);
                }

                var response = new Response() { FrameProgressScores = game.FrameProgressScores() };
                
                return Task.FromResult(response);
            }
        }
    }
}
