using System.Diagnostics.CodeAnalysis;
using BowlingScoreCalculator.Domain.Exception;

namespace BowlingScoreCalculator.Domain
{
    internal class Frame
    {
        private const int MaxPossiblePinsDownedInOneFrame = 10;

        private int? _firstThrow;
        private int? _secondThrow;
        private int? _progressScore;
        private readonly Frame? _prevFrame;
        private Frame? _nextFrame;

        public int Position => _prevFrame == null ? 1 : _prevFrame.Position + 1;
        public int? ProgressScore => _progressScore;
        public Frame? PrevFrame => _prevFrame;
        public Frame? NextFrame => _nextFrame;

        
        private Frame()
        {
        }

        protected Frame(Frame prevFrame)
        {
            _prevFrame = prevFrame;
            _prevFrame.SetNextFrame(this);
        }

        public static Frame First()
        {
            return new Frame();
        }

        public static Frame After(Frame prevFrame)
        {
            return new Frame(prevFrame);
        }

        public static Frame Last(Frame prevFrame)
        {
            return new LastFrame(prevFrame);
        }

        public void SetNextFrame(Frame frame)
        {
            _nextFrame = frame;
        }

        public bool TryToGetNextTwo(out int sumNextTwo)
        {
            sumNextTwo = 0;

            if (IsStrike())
            {
                if (IsLastFrame())
                {
                    if (_firstThrow.HasValue && _secondThrow.HasValue)
                    {
                        sumNextTwo = _firstThrow.Value + _secondThrow.Value;
                        return true;
                    }
                }
                else if (TryToGetNextOne(out int nextOne))
                {
                    sumNextTwo = MaxPossiblePinsDownedInOneFrame + nextOne;
                    return true;
                }
            }
            else if (_firstThrow.HasValue && _secondThrow.HasValue)
            {
                sumNextTwo = _firstThrow.Value + _secondThrow.Value;
                return true;
            }

            return false;
        }

        public bool TryToGetNextOne(out int nextOne)
        {
            nextOne = 0;

            if (!_firstThrow.HasValue)
                return false;

            nextOne = _firstThrow.Value;
            return true;
        }
        public void ThrowBall(byte pinsDowned)
        {
            ThrowBallGuard(pinsDowned);
            
            if (!_firstThrow.HasValue)
            {
                _firstThrow = pinsDowned;
            }
            else if (!_secondThrow.HasValue)
            {
                _secondThrow = pinsDowned;
            }

            TryToSetScore();
        }

        public void TryToSetScore()
        {
            if (!IsFirstFrame() && !_prevFrame.ProgressScore.HasValue)
                _prevFrame.TryToSetScore();

            if (IsFirstFrame() || _prevFrame.ProgressScore.HasValue)
                TryToSetLocalScore();
        }

        private void ThrowBallGuard(byte pinsDowned)
        {
            if (pinsDowned > MaxPossiblePinsDownedInOneFrame)
            {
                throw new FrameException(Position, $"Can not down more then {MaxPossiblePinsDownedInOneFrame} pins in one throw.");
            }

            if (_firstThrow.HasValue)
            {
                if (_firstThrow + pinsDowned > MaxPossiblePinsDownedInOneFrame)
                {
                    throw new FrameException(Position, $"Can not down more than {MaxPossiblePinsDownedInOneFrame} in one frame.");
                }
            }

            if (_firstThrow.HasValue && _secondThrow.HasValue)
            {
                throw new FrameException(Position, "Can not throw ball more than two times in one frame.");
            }
        }

        private void TryToSetLocalScore()
        {
            if (_progressScore.HasValue)
                throw new FrameException(Position, "Can not set progress score more than once");

            if (IsStrike())
            {
                if (IsLastFrame())
                {

                }
                else if (_nextFrame.TryToGetNextTwo(out int sumNextTwo))
                {
                    if (IsFirstFrame())
                    {
                        _progressScore = MaxPossiblePinsDownedInOneFrame + sumNextTwo;
                    }
                    else
                    {
                        _progressScore = _prevFrame.ProgressScore + MaxPossiblePinsDownedInOneFrame + sumNextTwo;
                    }
                }
            }
            else if (IsSpare())
            {
                if (IsLastFrame())
                {

                }
                else
                {
                    if (_nextFrame.TryToGetNextOne(out int nextOne))
                    {
                        if (IsFirstFrame())
                        {
                            _progressScore = MaxPossiblePinsDownedInOneFrame + nextOne;
                        }
                        else
                        {
                            _progressScore = _prevFrame.ProgressScore + MaxPossiblePinsDownedInOneFrame + nextOne;
                        }
                    }
                }
            }
            else
            {
                if (IsLastFrame())
                {

                }
                else
                {
                    if (_firstThrow.HasValue && _secondThrow.HasValue)
                    {
                        _progressScore = _firstThrow.Value + _secondThrow.Value;
                    }
                }
            }
        }

        [MemberNotNullWhen(returnValue: false, member: nameof(_nextFrame))]
        private bool IsLastFrame() => _nextFrame == null;

        [MemberNotNullWhen(returnValue: false, member: nameof(_prevFrame))]
        private bool IsFirstFrame() => _prevFrame == null;


        private bool IsSpare() => _firstThrow.HasValue &&
                                 _secondThrow.HasValue &&
                                 _firstThrow + _secondThrow == MaxPossiblePinsDownedInOneFrame;

        private bool IsStrike() => _firstThrow.HasValue &&
                                  _firstThrow.Value == MaxPossiblePinsDownedInOneFrame;

        public override string ToString()
        {
            return $"{base.ToString()} {Position.ToString()}";
        }
    }
}
