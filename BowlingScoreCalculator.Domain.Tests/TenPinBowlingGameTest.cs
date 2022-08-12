using BowlingScoreCalculator.Domain.Exception;

namespace BowlingScoreCalculator.Domain.Tests
{
    public class TenPinBowlingGameTest
    {
        [Fact]
        public void TryToDownMoreThanTenPinsInOneThrowShouldThrowException()
        {
            var game = new TenPinBowlingGame();

            var ex = Assert.Throws<FrameException>(() => game.ThrowBall(11));

            Assert.Equal("Frame 1 error. Can not down more then 10 pins in one throw.", ex.Message);
        }

        [Fact]
        public void TryToDownMoreThanTenPinsInOneFrameShouldThrowException()
        {
            var game = new TenPinBowlingGame();

            game.ThrowBall(3);
            var ex = Assert.Throws<FrameException>(() => game.ThrowBall(8));

            Assert.Equal("Frame 1 error. Can not down more than 10 in one frame.", ex.Message);
        }

        [Fact]
        public void TestFirstFrameProgressScore()
        {
            var game = new TenPinBowlingGame();

            game.ThrowBall(3);
            game.ThrowBall(4);

            var scores = game.FrameProgressScores();

            Assert.Equal("7", scores[0]);
            scores.AssertAllAfter(1, "*");
        }
    }
}