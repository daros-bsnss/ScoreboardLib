using Moq;

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
    }
}
