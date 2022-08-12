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
        public void ProgressScoreShouldBeEvaluatedWhenTwoNormalThrows()
        {
            var game = new TenPinBowlingGame();

            game.ThrowBall(3);
            game.ThrowBall(4);

            var scores = game.FrameProgressScores();

            Assert.Equal("7", scores[0]);
        }

        [Fact]
        public void ProgressScoreCanNotBeEvaluatedWhenFirstThrowIsStrike()
        {
            var game = new TenPinBowlingGame();

            game.ThrowBall(10);

            var scores = game.FrameProgressScores();

            Assert.Equal("*", scores[0]);
        }

        [Fact]
        public void ProgressScoreCanNotBeEvaluatedWhenSecondThrowIsSpare()
        {
            var game = new TenPinBowlingGame();

            game.ThrowBall(3);
            game.ThrowBall(7);

            var scores = game.FrameProgressScores();

            Assert.Equal("*", scores[0]);
        }

        [Fact]
        public void ProgressScoreShouldBeEvaluatedWhenSecondThrowIsSpareAfterNextOneThrow()
        {
            var game = new TenPinBowlingGame();

            game.ThrowBall(7);
            game.ThrowBall(3);
            game.ThrowBall(5);

            var scores = game.FrameProgressScores();

            Assert.Equal("15", scores[0]);
            Assert.Equal("*", scores[1]);
        }

        [Fact]
        public void NextFrameProgressScoreCanNotBeEvaluatedWhenPreviousFrameIsStrike()
        {
            var game = new TenPinBowlingGame();

            game.ThrowBall(10);
            game.ThrowBall(3);

            var scores = game.FrameProgressScores();

            Assert.Equal("*", scores[0]);
            Assert.Equal("*", scores[1]);
        }

        [Fact]
        public void NextFrameProgressScoreShouldBeEvaluatedWhenPreviousFrameIsStrikeAfterNextTwoThrows()
        {
            var game = new TenPinBowlingGame();

            game.ThrowBall(10);
            game.ThrowBall(3);
            game.ThrowBall(5);

            var scores = game.FrameProgressScores();

            Assert.Equal("18", scores[0]);
            Assert.Equal("26", scores[1]);
        }

        [Fact]
        public void TwoSuccessiveStrikes()
        {
            var game = new TenPinBowlingGame();

            game.ThrowBall(10);
            game.ThrowBall(10);

            var scores = game.FrameProgressScores();

            Assert.Equal("*", scores[0]);
            Assert.Equal("*", scores[1]);
        }

        [Fact]
        public void PerfectGame()
        {
            var game = new TenPinBowlingGame();

            game.ThrowBall(10);
            game.ThrowBall(10);
            game.ThrowBall(10);
            game.ThrowBall(10);
            game.ThrowBall(10);
            game.ThrowBall(10);
            game.ThrowBall(10);
            game.ThrowBall(10);
            game.ThrowBall(10);
            game.ThrowBall(10);
            game.ThrowBall(10);
            game.ThrowBall(10);

            var scores = game.FrameProgressScores();

            Assert.Equal("30", scores[0]);
            Assert.Equal("60", scores[1]);
            Assert.Equal("90", scores[2]);
            Assert.Equal("120", scores[3]);
            Assert.Equal("150", scores[4]);
            Assert.Equal("180", scores[5]);
            Assert.Equal("210", scores[6]);
            Assert.Equal("240", scores[7]);
            Assert.Equal("270", scores[8]);
            Assert.Equal("300", scores[9]);
        }
    }
}