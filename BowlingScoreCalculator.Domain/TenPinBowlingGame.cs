using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingScoreCalculator.Domain
{
    public class TenPinBowlingGame
    {
        private const int NumberOfFrames = 10;
        private readonly IReadOnlyCollection<Frame> _frames;

        private Frame _currentFrame;
        
        private bool _gameCompeted = false;

        public bool GameCompeted => _gameCompeted;

        public TenPinBowlingGame()
        {
            _frames = InitFrames();
            _currentFrame = _frames.First();
        }

        public void ThrowBall(byte pinsDowned)
        {
            if (_gameCompeted)
            {
                throw new System.Exception("Can not throw ball when game is completed.");
            }

            _currentFrame.ThrowBall(pinsDowned);
            _currentFrame.TryToSetScore();

            if (_currentFrame.IsCompleted())
            {
                if (_currentFrame.IsLastFrame())
                {
                    _gameCompeted = true;
                }
                else
                {
                    _currentFrame = _currentFrame.NextFrame;
                }
            }
        }

        public List<string> FrameProgressScores()
        {
            return _frames
                .Where(f => f.Position <= _currentFrame.Position)
                .Select(f => f.ProgressScore.HasValue ? f.ProgressScore.ToString()! : "*").ToList();
        }

        private IReadOnlyCollection<Frame> InitFrames()
        {
            var frames = new List<Frame>() { Frame.First() };
            
            for (var i = 1; i < NumberOfFrames - 1; i++)
            {
                var prevFrame = frames.Last();
                frames.Add(Frame.After(prevFrame));
            }

            frames.Add(new LastFrame(frames.Last()));

            return frames.AsReadOnly();
        }
    }
}
