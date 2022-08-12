
namespace BowlingScoreCalculator.Domain.Exception
{
    public class FrameException : System.Exception
    {
        public FrameException(int position, string message)
        : base($"Frame {position} error. {message}")
        {
                
        }
    }
}
