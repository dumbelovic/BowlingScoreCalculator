
namespace BowlingScoreCalculator.Domain
{
    public static class TenPinBowlingGameBuilder
    {
        private const int NumberOfFrames = 10;
        public static TenPinBowlingGame Start()
        {
            var frames = InitFrames();
            return new TenPinBowlingGame(frames);
        }

        private static IReadOnlyCollection<Frame> InitFrames()
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
