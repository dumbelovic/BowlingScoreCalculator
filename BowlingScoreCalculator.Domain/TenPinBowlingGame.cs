using BowlingScoreCalculator.Domain.Exception;

namespace BowlingScoreCalculator.Domain
{
    public class TenPinBowlingGame
    {
        private readonly IReadOnlyCollection<Frame> _frames;
        private Frame _currentFrame;

        public bool GameCompeted { get; private set; }

        internal TenPinBowlingGame(IReadOnlyCollection<Frame> frames)
        {
            _frames = frames;
            _currentFrame = _frames.First();
        }

        public void ThrowBall(byte pinsDowned)
        {
            if (GameCompeted)
            {
                throw new GameBadRequestException("Can not throw ball when game is completed.");
            }

            _currentFrame.ThrowBall(pinsDowned);
            _currentFrame.TryToSetScore();

            ContinueGame();
        }

        private void ContinueGame()
        {
            if (!_currentFrame.IsCompleted())
                return;

            if (_currentFrame.IsLastFrame())
            {
                GameCompeted = true;
            }
            else
            {
                _currentFrame = _currentFrame.NextFrame;
            }
        }

        public List<int?> FrameProgressScores()
        {
            return _frames
                .Where(f => f.Position <= _currentFrame.Position)
                .Select(f => f.ProgressScore)
                .ToList();
        }
    }
}
