
namespace BowlingScoreCalculator.Domain.Exception
{
    public class GameBadRequestException : BadRequestException
    {
        public GameBadRequestException(string message)
            : base(message)
        {
                
        }
    }
}
