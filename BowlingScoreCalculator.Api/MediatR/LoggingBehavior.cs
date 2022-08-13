using System.Text.Json;
using MediatR;

namespace BowlingScoreCalculator.Api.MediatR
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var requestType = typeof(TRequest).FullName;

            _logger.LogDebug($"Handling {requestType}, request: {JsonSerializer.Serialize(request)} ");
            var response = await next();
            _logger.LogDebug($"Handled {requestType}, response: {JsonSerializer.Serialize(response)}");
            return response;
        }
    }
}
