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
            liveScoreboard = new(liveMatchRepositoryMock.Object);
        }

        [TestMethod]
        public void StartMatch_ShouldCreateAMatchAndInsert()
        {
            // Arrange
            string homeTeam = "Latvia";
            string awayTeam = "Estonia";
            Guid expectedId = Guid.NewGuid();

            liveMatchRepositoryMock.Setup(r => r.Insert(It.IsAny<LiveMatch>()))
                .Returns(expectedId);

            // Act
            Guid result = liveScoreboard.StartMatch(homeTeam, awayTeam);

            // Assert
            Assert.AreEqual(expectedId, result);

            liveMatchRepositoryMock.Verify(r =>
                r.Insert(It.Is<LiveMatch>(m => m.HomeTeam == homeTeam && m.AwayTeam == awayTeam)), Times.Once);
        }
    }
}
