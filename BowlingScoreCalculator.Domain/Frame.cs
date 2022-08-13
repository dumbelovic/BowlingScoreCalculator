using System.Diagnostics.CodeAnalysis;
using BowlingScoreCalculator.Domain.Exception;

namespace BowlingScoreCalculator.Domain
{
    internal class Frame
    {
        private const int MaxPossiblePinsDowned = 10;

        private readonly Frame? _prevFrame;
        private Frame? _nextFrame;

        protected int? _firstThrow;
        protected int? _secondThrow;
        protected int? _progressScore;
        
        public int Position => _prevFrame == null ? 1 : _prevFrame.Position + 1;
        public int? ProgressScore => _progressScore;
        public Frame? PrevFrame => _prevFrame;
        public Frame? NextFrame => _nextFrame;

        
        private Frame()
        { }

        protected Frame(Frame prevFrame)
        {
            _prevFrame = prevFrame;
            _prevFrame.SetNextFrame(this);
        }

        internal virtual bool IsCompleted() => IsStrike() || _secondThrow.HasValue;

        internal static Frame First() => new();

        internal static Frame After(Frame prevFrame) => new(prevFrame);

        internal static Frame Last(Frame prevFrame) => new LastFrame(prevFrame);

        internal void SetNextFrame(Frame frame) => _nextFrame = frame;

        protected virtual bool TryToGetNextTwo(out int sumNextTwo)
        {
            sumNextTwo = 0;

            if (IsStrike())
            {
                if (_nextFrame!.TryToGetNextOne(out int nextOne))
                {
                    sumNextTwo = MaxPossiblePinsDowned + nextOne;
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

        internal bool TryToGetNextOne(out int nextOne)
        {
            nextOne = 0;

            if (!_firstThrow.HasValue)
                return false;

            nextOne = _firstThrow.Value;
            return true;
        }

        internal void ThrowBall(byte pinsDowned)
        {
            Guard(pinsDowned);
            SetPinDowned(pinsDowned);
        }

        internal void TryToSetScore()
        {
            if (!IsFirstFrame() && !_prevFrame.ProgressScore.HasValue)
                _prevFrame.TryToSetScore();

            if (IsFirstFrame() || _prevFrame.ProgressScore.HasValue)
                TryToSetLocalScore();
        }

        private void Guard(byte pinsDowned)
        {
            GuardCommon(pinsDowned);
            ExtendGuard(pinsDowned);
        }

        protected virtual void ExtendGuard(byte pinsDowned)
        {
            if (_firstThrow.HasValue && _firstThrow + pinsDowned > MaxPossiblePinsDowned)
            {
                throw new FrameBadRequestException(Position, $"Can not down more than {MaxPossiblePinsDowned} pins in first two throws.");
            }

            if (IsStrike())
            {
                throw new InvalidOperationException("Can not throw second ball when first is strike");
            }

            if (_firstThrow.HasValue && _secondThrow.HasValue)
            {
                throw new InvalidOperationException("Can not throw ball more than two times if frame is not last frame.");
            }
        }

        protected virtual bool SetPinDowned(byte pinsDowned)
        {
            if (!_firstThrow.HasValue)
            {
                _firstThrow = pinsDowned;
                return true;
            }
            else if (!_secondThrow.HasValue)
            {
                _secondThrow = pinsDowned;
                return true;
            }

            return false;
        }

        private void GuardCommon(byte pinsDowned)
        {
            if (pinsDowned > MaxPossiblePinsDowned)
            {
                throw new FrameBadRequestException(Position, $"Can not down more then {MaxPossiblePinsDowned} pins in one throw.");
            }
        }

        protected virtual void TryToSetLocalScore()
        {
            if (_progressScore.HasValue)
                throw new InvalidOperationException("Can not set progress score more than once");

            if (!IsCompleted())
                return;

            if (IsStrike())
            {
                if (_nextFrame!.TryToGetNextTwo(out int sumNextTwo))
                {
                    _progressScore = GetPrevProgressScore() + MaxPossiblePinsDowned + sumNextTwo;
                }
            }
            else if (IsSpare())
            {
                if (_nextFrame!.TryToGetNextOne(out int nextOne))
                {
                    _progressScore = GetPrevProgressScore() + MaxPossiblePinsDowned + nextOne;
                }
            }
            else
            {
                if (_firstThrow.HasValue && _secondThrow.HasValue)
                {
                    _progressScore = GetPrevProgressScore() + _firstThrow.Value + _secondThrow.Value;
                }
            }
        }

        [MemberNotNullWhen(returnValue: false, member: nameof(_nextFrame))]
        [MemberNotNullWhen(returnValue: false, member: nameof(NextFrame))]
        internal bool IsLastFrame() => _nextFrame == null;

        [MemberNotNullWhen(returnValue: false, member: nameof(_prevFrame))]
        internal bool IsFirstFrame() => _prevFrame == null;


        protected bool IsSpare() => 
            _firstThrow.HasValue &&
            _secondThrow.HasValue &&
            _firstThrow + _secondThrow == MaxPossiblePinsDowned;

        protected bool IsStrike() => 
            _firstThrow.HasValue &&
            _firstThrow.Value == MaxPossiblePinsDowned;

        protected int GetPrevProgressScore()
        {
            return IsFirstFrame() ? 0 : _prevFrame.ProgressScore!.Value;
        }

        public override string ToString()
        {
            return $"{base.ToString()} {Position.ToString()}";
        }
    }
}
