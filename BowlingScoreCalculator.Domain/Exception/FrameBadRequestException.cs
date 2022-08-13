
namespace BowlingScoreCalculator.Domain.Exception
{
    public class FrameBadRequestException : BadRequestException
    {
        public FrameBadRequestException(int position, string message)
        : base($"Frame {position} error. {message}")
        {
                
        }
    }
}
