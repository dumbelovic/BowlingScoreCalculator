using BowlingScoreCalculator.Domain;
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
            private const byte PinsDownedMaxInputLength = (2 * 9) + 3;
            public Validator()
            {
                RuleFor(r => r.PinsDowned)
                    .NotEmpty()
                    .WithMessage("PinsDowned collection can not be empty.");

                RuleFor(r => r.PinsDowned)
                    .Must(pins => pins.Count <= PinsDownedMaxInputLength)
                    .WithMessage($"PinsDowned collection can not contain more than {PinsDownedMaxInputLength} numbers.");
            } 
        }

        public class ScoresHandler : IRequestHandler<Request, Response>
        {
            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var game = TenPinBowlingGameBuilder.Start(); 

                foreach (var pinsDowned in request.PinsDowned)
                {
                    game.ThrowBall(pinsDowned);
                }

                var response = new Response()
                {
                    FrameProgressScores = game.FrameProgressScores()
                        .Select(f=> f.HasValue ? f.ToString()! : "*")
                        .ToList(),

                    GameCompleted = game.GameCompeted
                };
                
                return Task.FromResult(response);
            }
        }
    }
}
