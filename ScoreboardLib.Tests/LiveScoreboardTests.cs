using FluentValidation;
using Moq;
using ScoreboardLib.Exceptions;
using ScoreboardLib.Interfaces;
using ScoreboardLib.Models;
using System.Collections.Generic;

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
        public void StartMatch_ShouldCreateMatchAndInsert()
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
                r.Insert(It.Is<LiveMatch>(m => m.HomeTeam == homeTeam && m.AwayTeam == awayTeam && m.StartedDateTime != default)), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void StartMatch_ShouldThrowException_WhenTeamNamesAreEqual()
        {
            // Arrange
            string homeTeam = "LATVIA ";
            string awayTeam = " latvia";

            // Act
            // Assert
            liveScoreboard.StartMatch(homeTeam, awayTeam);
        }

        [TestMethod]
        [ExpectedException(typeof(LiveMatchAlreadyStartedException))]
        public void StartMatch_ShouldThrowException_WhenMatchWithTheSameTeamStarted()
        {
            // Arrange
            string homeTeam = "Italy";
            string awayTeam = "Finland";

            var storedMatches = new List<LiveMatch> { new() { HomeTeam = awayTeam, AwayTeam = "any" } };

            liveMatchRepositoryMock.Setup(r => r.GetAll())
                .Returns(storedMatches);

            // Act
            // Assert
            Guid result = liveScoreboard.StartMatch(homeTeam, awayTeam);
        }

        [TestMethod]
        public void UpdateMatchScore_ShouldUpdateMatchWithScore()
        {
            // Arrange
            Guid matchId = Guid.NewGuid();
            string homeTeam = "Estonia";
            string awayTeam = "Austria";
            int homeTeamScore = 1;
            int awayTeamScore = 2;

            var match = new LiveMatch
            {
                Id = matchId,
                HomeTeam = homeTeam,
                AwayTeam = awayTeam
            };

            liveMatchRepositoryMock.Setup(r => r.GetAll())
                .Returns(new List<LiveMatch> { match });

            liveMatchRepositoryMock.Setup(r => r.Update(It.IsAny<LiveMatch>()))
                .Returns(true);

            // Act
            liveScoreboard.UpdateMatchScore(matchId, homeTeamScore, awayTeamScore);

            // Assert
            liveMatchRepositoryMock.Verify(r =>
                r.Update(It.Is<LiveMatch>(m =>
                    m.HomeTeam == homeTeam &&
                    m.AwayTeam == awayTeam &&
                    m.Score.Item1 == homeTeamScore &&
                    m.Score.Item2 == awayTeamScore
                )), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void UpdateMatchScore_ShouldThrowException_WhenEmptyId()
        {
            // Arrange
            Guid matchId = Guid.Empty;

            var match = new LiveMatch
            {
                Id = matchId,
                HomeTeam = "Estonia",
                AwayTeam = "Austria"
            };

            liveMatchRepositoryMock.Setup(r => r.GetAll())
                .Returns(new List<LiveMatch> { match });

            // Act
            // Assert
            liveScoreboard.UpdateMatchScore(matchId, 1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void UpdateMatchScore_ShouldThrowException_WhenNegativeScore()
        {
            // Arrange
            Guid matchId = Guid.NewGuid();

            var match = new LiveMatch
            {
                Id = matchId,
                HomeTeam = "Estonia",
                AwayTeam = "Austria"
            };

            liveMatchRepositoryMock.Setup(r => r.GetAll())
                .Returns(new List<LiveMatch> { match });

            // Act
            // Assert
            liveScoreboard.UpdateMatchScore(matchId, -1, -3);
        }

        [TestMethod]
        [ExpectedException(typeof(LiveMatchNotStartedException))]
        public void UpdateMatchScore_ShouldThrowException_WhenMatchNotStarted()
        {
            // Arrange
            liveMatchRepositoryMock.Setup(r => r.GetAll())
                .Returns(Enumerable.Empty<LiveMatch>());

            // Act
            // Assert
            liveScoreboard.UpdateMatchScore(Guid.NewGuid(), 1, 0);
        }

        [TestMethod]
        public void FinishMatch_ShouldRemoveMatchFromLive()
        {
            // Arrange
            Guid matchId = Guid.NewGuid();

            var match = new LiveMatch
            {
                Id = matchId,
                HomeTeam = "Greece",
                AwayTeam = "Ireland"
            };

            liveMatchRepositoryMock.Setup(r => r.GetAll())
                .Returns(new List<LiveMatch> { match });

            liveMatchRepositoryMock.Setup(r => r.Delete(It.IsAny<Guid>()))
                .Returns(true);

            // Act
            liveScoreboard.FinishMatch(matchId);

            // Assert
            liveMatchRepositoryMock.Verify(r =>
                r.Delete(It.Is<Guid>(g => g == matchId)), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(LiveMatchNotStartedException))]
        public void FinishMatch_ShouldThrowException_WhenMatchNotStarted()
        {
            // Arrange
            liveMatchRepositoryMock.Setup(r => r.GetAll())
                .Returns(Enumerable.Empty<LiveMatch>());

            // Act
            // Assert
            liveScoreboard.FinishMatch(Guid.NewGuid());
        }

        [TestMethod]
        public void GetSummary_ShouldReturnEmptyCollection_WhenInit()
        {
            // Arrange
            liveMatchRepositoryMock.Setup(r => r.GetAll())
                .Returns(Enumerable.Empty<LiveMatch>());

            // Act
            var result = liveScoreboard.GetSummary();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetSummary_ShouldReturnLiveMatchesCollection_OrderedByTotalScore()
        {
            // Arrange
            DateTime startedDate = DateTime.UtcNow;
            var matchA = new LiveMatch
            {
                Id = Guid.NewGuid(),
                HomeTeam = "Mexico",
                AwayTeam = "Canada",
                Score = (1, 3),
                StartedDateTime = startedDate
            };
            var matchB = new LiveMatch
            {
                Id = Guid.NewGuid(),
                HomeTeam = "Spain",
                AwayTeam = "Brazil",
                Score = (1, 2),
                StartedDateTime = startedDate
            };
            var matchC = new LiveMatch
            {
                Id = Guid.NewGuid(),
                HomeTeam = "Germany",
                AwayTeam = "Italy",
                Score = (1, 1),
                StartedDateTime = startedDate
            };

            var expectedLiveMatches = new List<LiveMatch> { matchA, matchB, matchC };

            liveMatchRepositoryMock.Setup(r => r.GetAll())
                .Returns(expectedLiveMatches);

            // Act
            var matches = liveScoreboard.GetSummary();

            // Assert
            Assert.IsNotNull(matches);
            Assert.AreEqual(3, matches.Count());

            var matchesArray = matches.ToArray();
            Assert.AreEqual(matchC.Id, matchesArray[0].Id);
            Assert.AreEqual(matchB.Id, matchesArray[1].Id);
            Assert.AreEqual(matchA.Id, matchesArray[2].Id);
        }

        [TestMethod]
        public void GetSummary_ShouldReturnLiveMatchesCollection_OrderedByTotalScore_ThenByDateStarted_WhenEqualScores()
        {
            // Arrange
            var matchA = new LiveMatch
            {
                Id = Guid.NewGuid(),
                HomeTeam = "Mexico",
                AwayTeam = "Canada",
                StartedDateTime = DateTime.UtcNow
            };
            var matchB = new LiveMatch
            {
                Id = Guid.NewGuid(),
                HomeTeam = "Spain",
                AwayTeam = "Brazil",
                StartedDateTime = DateTime.UtcNow.AddMinutes(-10)
            };
            var matchC = new LiveMatch
            {
                Id = Guid.NewGuid(),
                HomeTeam = "Germany",
                AwayTeam = "Italy",
                StartedDateTime = DateTime.UtcNow.AddMinutes(-5)
            };

            var expectedLiveMatches = new List<LiveMatch> { matchA, matchB, matchC };

            liveMatchRepositoryMock.Setup(r => r.GetAll())
                .Returns(expectedLiveMatches);

            // Act
            var matches = liveScoreboard.GetSummary();

            // Assert
            Assert.IsNotNull(matches);
            Assert.AreEqual(3, matches.Count());

            var matchesArray = matches.ToArray();
            Assert.AreEqual(matchA.Id, matchesArray[2].Id);
            Assert.AreEqual(matchB.Id, matchesArray[0].Id);
            Assert.AreEqual(matchC.Id, matchesArray[1].Id);
        }
    }
}
