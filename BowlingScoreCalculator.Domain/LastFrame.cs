using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BowlingScoreCalculator.Domain.Exception;

namespace BowlingScoreCalculator.Domain
{
    internal class LastFrame : Frame
    {
        private int? _thirdThrow;

        public LastFrame(Frame prevFrame) 
            : base(prevFrame)
        {}

        internal override bool IsCompleted()
        {
            if (IsStrike() || IsSpare())
            {
                return _secondThrow.HasValue && _thirdThrow.HasValue;
            }

            return _firstThrow.HasValue && _secondThrow.HasValue;
        }

        protected override void ExtendGuard(byte pinsDowned)
        {
            if (!(IsStrike() || IsSpare()))
            {
                if (_firstThrow.HasValue && _secondThrow.HasValue)
                {
                    throw new System.Exception("Can not throw ball more than two times in last frame if there ar no strike or spare.");
                }
            }
        }

        protected override bool SetPinDowned(byte pinsDowned)
        {
            if (base.SetPinDowned(pinsDowned))
                return true;

            _thirdThrow = pinsDowned;
            return true;
        }

        protected override void TryToSetLocalScore()
        {
            if (_progressScore.HasValue)
                throw new FrameException(Position, "Can not set progress score more than once");

            if (!IsCompleted())
                return;

            if (IsStrike() || IsSpare())
            {
                _progressScore = GetPrevProgressScore() + _firstThrow + _secondThrow + _thirdThrow;
            }
            else
            {
                _progressScore = GetPrevProgressScore() + _firstThrow + _secondThrow;
            }
        }

        protected override bool TryToGetNextTwo(out int sumNextTwo)
        {
            sumNextTwo = 0;

            if (_firstThrow.HasValue && _secondThrow.HasValue)
            {
                sumNextTwo = _firstThrow.Value + _secondThrow.Value;
                return true;
            }

            return false;
        }
    }
}
