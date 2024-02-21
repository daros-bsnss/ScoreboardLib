using Moq;
using ScoreboardLib.Interfaces;
using ScoreboardLib.Models;

namespace ScoreboardLib.Tests
{
    [TestClass]
    public class LiveScoreboardTests
    {
        private Mock<ILiveMatchRepository> liveMatchRepositoryMock;
        private LiveScoreboard liveScoreboard;

        [TestInitialize]
        public void Initialize()
        {
            liveMatchRepositoryMock = new Mock<ILiveMatchRepository>();
            liveScoreboard = new(liveMatchRepositoryMock);
        }

        [TestMethod]
        public void StartMatch_ShouldCreateAMatchAndInsert()
        {
            // Arrange
            string homeTeam = "Latvia";
            string awayTeam = "Estonia";

            // Act
            liveScoreboard.StartMatch(homeTeam, awayTeam);

            // Assert
            liveMatchRepositoryMock.Verify(r =>
                r.Insert(It.Is<LiveMatch>(m => m.HomeTeam == homeTeam && m.AwayTeam == awayTeam)), Times.Once);
        }
    }
}
