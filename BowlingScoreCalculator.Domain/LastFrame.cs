using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingScoreCalculator.Domain
{
    internal class LastFrame : Frame
    {
        
        private int _thirdThorw;

        public LastFrame(Frame prevFrame) : base(prevFrame)
        {
                
        }
    }
}
