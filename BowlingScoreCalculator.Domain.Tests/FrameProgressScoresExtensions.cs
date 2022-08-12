using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingScoreCalculator.Domain.Tests
{
    internal static class FrameProgressScoresExtensions
    {
        public static void AssertAllAfter(this List<string> scores, int index, string expectedValue)
        {
            for (int i = index; i < scores.Count; i++)
            {
                Assert.Equal(expectedValue, scores[i]);
            }
        }
    }
}
