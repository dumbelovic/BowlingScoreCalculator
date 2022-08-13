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

            Assert.Equal("Frame 1 error. Can not down more than 10 in first two throws.", ex.Message);
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
        public void TwoSuccessiveStrikesProgressScoreCanNotBeEvaluated()
        {
            var game = new TenPinBowlingGame();

            game.ThrowBall(10);
            game.ThrowBall(10);

            var scores = game.FrameProgressScores();

            Assert.Equal("*", scores[0]);
            Assert.Equal("*", scores[1]);
        }

        [Fact]
        public void TwoSuccessiveStrikesProgressScoreOfFirstStrikeShouldBeEvaluatedAfterNextOneThrow()
        {
            var game = new TenPinBowlingGame();

            game.ThrowBall(10);
            game.ThrowBall(10);
            game.ThrowBall(5);

            var scores = game.FrameProgressScores();

            Assert.Equal("25", scores[0]);
            Assert.Equal("*", scores[1]);
            Assert.Equal("*", scores[2]);
        }

        [Fact]
        public void TwoSuccessiveStrikesProgressScoreOfBothStrikesShouldBeEvaluatedAfterNextTwoThrows()
        {
            var game = new TenPinBowlingGame();

            game.ThrowBall(10);
            game.ThrowBall(10);
            game.ThrowBall(5);
            game.ThrowBall(3);

            var scores = game.FrameProgressScores();

            Assert.Equal("25", scores[0]);
            Assert.Equal("43", scores[1]);
            Assert.Equal("51", scores[2]);
        }

        [Fact]
        public void ThreeSuccessiveStrikesProgressScoreOfFirstStrikeShouldBeEvaluated()
        {
            var game = new TenPinBowlingGame();

            game.ThrowBall(10);
            game.ThrowBall(10);
            game.ThrowBall(10);

            var scores = game.FrameProgressScores();

            Assert.Equal("30", scores[0]);
            Assert.Equal("*", scores[1]);
            Assert.Equal("*", scores[2]);
        }

        [Fact]
        public void FourSuccessiveStrikesProgressScoreOfFirstTwoStrikesShouldBeEvaluated()
        {
            var game = new TenPinBowlingGame();

            game.ThrowBall(10);
            game.ThrowBall(10);
            game.ThrowBall(10);
            game.ThrowBall(10);

            var scores = game.FrameProgressScores();

            Assert.Equal("30", scores[0]);
            Assert.Equal("60", scores[1]);
            Assert.Equal("*", scores[2]);
            Assert.Equal("*", scores[3]);
        }

        [Fact]
        public void GameShouldBeCompletedAfterTwentyNormalThrows()
        {
            var game = new TenPinBowlingGame();

            game.ThrowBall(5);
            game.ThrowBall(3);

            game.ThrowBall(5);
            game.ThrowBall(3);

            game.ThrowBall(5);
            game.ThrowBall(3);

            game.ThrowBall(5);
            game.ThrowBall(3);

            game.ThrowBall(5);
            game.ThrowBall(3);

            game.ThrowBall(5);
            game.ThrowBall(3);

            game.ThrowBall(5);
            game.ThrowBall(3);

            game.ThrowBall(5);
            game.ThrowBall(3);

            game.ThrowBall(5);
            game.ThrowBall(3);

            game.ThrowBall(5);
            game.ThrowBall(3);

            var scores = game.FrameProgressScores();

            Assert.Equal("8", scores[0]);
            Assert.Equal("16", scores[1]);
            Assert.Equal("24", scores[2]);
            Assert.Equal("32", scores[3]);
            Assert.Equal("40", scores[4]);
            Assert.Equal("48", scores[5]);
            Assert.Equal("56", scores[6]);
            Assert.Equal("64", scores[7]);
            Assert.Equal("72", scores[8]);
            Assert.Equal("80", scores[9]);

            Assert.True(game.GameCompeted);
        }

        [Fact]
        public void GameShouldBeCompletedAfterTwelveStrikes() // Example 1
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

            Assert.True(game.GameCompeted);
        }

        [Fact]
        public void SixFramesCompletedAllThrowsOne() // Example 2
        {
            var game = new TenPinBowlingGame();

            game.ThrowBall(1);
            game.ThrowBall(1);
            game.ThrowBall(1);
            game.ThrowBall(1);
            game.ThrowBall(1);
            game.ThrowBall(1);
            game.ThrowBall(1);
            game.ThrowBall(1);
            game.ThrowBall(1);
            game.ThrowBall(1);
            game.ThrowBall(1);
            game.ThrowBall(1);

            var scores = game.FrameProgressScores();

            Assert.False(game.GameCompeted);

            Assert.Equal("2", scores[0]);
            Assert.Equal("4", scores[1]);
            Assert.Equal("6", scores[2]);
            Assert.Equal("8", scores[3]);
            Assert.Equal("10", scores[4]);
            Assert.Equal("12", scores[5]);
        }

        [Fact]
        public void SevenFramesCompletedSpareAndStrikesExample() // Example 3
        {
            var game = new TenPinBowlingGame();

            game.ThrowBall(1);
            game.ThrowBall(1);
            game.ThrowBall(1);
            game.ThrowBall(1);
            game.ThrowBall(9);
            game.ThrowBall(1);
            game.ThrowBall(2);
            game.ThrowBall(8);
            game.ThrowBall(9);
            game.ThrowBall(1);
            game.ThrowBall(10);
            game.ThrowBall(10);

            var scores = game.FrameProgressScores();

            Assert.False(game.GameCompeted);

            Assert.Equal("2", scores[0]);
            Assert.Equal("4", scores[1]);
            Assert.Equal("16", scores[2]);
            Assert.Equal("35", scores[3]);
            Assert.Equal("55", scores[4]);
            Assert.Equal("*", scores[5]);
            Assert.Equal("*", scores[6]);
        }

        [Fact]
        public void GutterBallGameWithTenThrowsShouldBeCompleted()
        {
            var game = new TenPinBowlingGame();

            game.ThrowBall(0);
            game.ThrowBall(0);

            game.ThrowBall(0);
            game.ThrowBall(0);

            game.ThrowBall(0);
            game.ThrowBall(0);

            game.ThrowBall(0);
            game.ThrowBall(0);

            game.ThrowBall(0);
            game.ThrowBall(0);

            game.ThrowBall(0);
            game.ThrowBall(0);

            game.ThrowBall(0);
            game.ThrowBall(0);

            game.ThrowBall(0);
            game.ThrowBall(0);

            game.ThrowBall(0);
            game.ThrowBall(0);

            game.ThrowBall(0);
            game.ThrowBall(0);


            var scores = game.FrameProgressScores();

            Assert.True(game.GameCompeted);

            Assert.Equal("0", scores[0]);
            Assert.Equal("0", scores[1]);
            Assert.Equal("0", scores[2]);
            Assert.Equal("0", scores[3]);
            Assert.Equal("0", scores[4]);
            Assert.Equal("0", scores[5]);
            Assert.Equal("0", scores[6]);
            Assert.Equal("0", scores[7]);
            Assert.Equal("0", scores[8]);
            Assert.Equal("0", scores[9]);
        }

    }
}