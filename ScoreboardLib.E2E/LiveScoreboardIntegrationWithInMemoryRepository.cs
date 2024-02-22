using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ScoreboardLib.E2E
{
    [TestClass]
    public class LiveScoreboardIntegrationWithInMemoryRepository
    {
        private readonly LiveScoreboard liveScoreboard = new(new InMemoryLiveMatchRepository());

        [TestMethod]
        public void ShouldShowCorrectLiveScoreboardSummary_WhenRespectivelyUpdatedScore()
        {
            // Arrange
            (string, string) matchTeamsA = ("Mexico", "Canada");
            (string, string) matchTeamsB = ("Spain", "Brazil");
            (string, string) matchTeamsC = ("Germany", "France");
            (string, string) matchTeamsD = ("Uruguay", "Italy");
            (string, string) matchTeamsE = ("Argentina", "Australia");

            // Act
            Guid matchAId = liveScoreboard.StartMatch(matchTeamsA.Item1, matchTeamsA.Item2);
            Guid matchBId = liveScoreboard.StartMatch(matchTeamsB.Item1, matchTeamsB.Item2);
            Guid matchCId = liveScoreboard.StartMatch(matchTeamsC.Item1, matchTeamsC.Item2);
            Guid matchDId = liveScoreboard.StartMatch(matchTeamsD.Item1, matchTeamsD.Item2);
            Guid matchEId = liveScoreboard.StartMatch(matchTeamsE.Item1, matchTeamsE.Item2);

            liveScoreboard.UpdateMatchScore(matchAId, 0, 5);
            liveScoreboard.UpdateMatchScore(matchBId, 10, 2);
            liveScoreboard.UpdateMatchScore(matchCId, 2, 2);
            liveScoreboard.UpdateMatchScore(matchDId, 6, 6);
            liveScoreboard.UpdateMatchScore(matchEId, 3, 1);

            var liveMatchSummary = liveScoreboard.GetSummary();

            // Assert
            Assert.IsNotNull(liveMatchSummary);
            Assert.AreEqual(0, liveScoreboard.Count());

            var liveMatchSummaryArray = liveMatchSummary.ToArray();
            Assert.AreEqual(matchDId, liveMatchSummaryArray[0].Id);
            Assert.AreEqual(matchBId, liveMatchSummaryArray[1].Id);
            Assert.AreEqual(matchAId, liveMatchSummaryArray[2].Id);
            Assert.AreEqual(matchEId, liveMatchSummaryArray[3].Id);
            Assert.AreEqual(matchCId, liveMatchSummaryArray[4].Id);
        }

        [TestMethod]
        public void ShouldShowCorrectLiveScoreboardSummary_WhenUpdateAndDeleteMatches()
        {
            // Arrange
            (string, string) matchTeamsA_ToFinish = ("Mexico", "Canada");
            (string, string) matchTeamsB_ToFinish = ("Spain", "Brazil");
            (string, string) matchTeamsC = ("Germany", "France");
            (string, string) matchTeamsD_ToFinish = ("Uruguay", "Italy");
            (string, string) matchTeamsE = ("Argentina", "Australia");

            // Act
            Guid matchAId_ToFinish = liveScoreboard.StartMatch(matchTeamsA_ToFinish.Item1, matchTeamsA_ToFinish.Item2);
            Guid matchBId_ToFinish = liveScoreboard.StartMatch(matchTeamsB_ToFinish.Item1, matchTeamsB_ToFinish.Item2);
            Guid matchCId = liveScoreboard.StartMatch(matchTeamsC.Item1, matchTeamsC.Item2);
            Guid matchDId_ToFinish = liveScoreboard.StartMatch(matchTeamsD_ToFinish.Item1, matchTeamsD_ToFinish.Item2);
            Guid matchEId = liveScoreboard.StartMatch(matchTeamsE.Item1, matchTeamsE.Item2);

            liveScoreboard.UpdateMatchScore(matchAId_ToFinish, 1, 1);
            liveScoreboard.UpdateMatchScore(matchBId_ToFinish, 1, 2);
            liveScoreboard.UpdateMatchScore(matchCId, 1, 0);
            liveScoreboard.UpdateMatchScore(matchDId_ToFinish, 1, 4);
            liveScoreboard.UpdateMatchScore(matchEId, 1, 0);

            liveScoreboard.FinishMatch(matchAId_ToFinish);
            liveScoreboard.FinishMatch(matchBId_ToFinish);

            liveScoreboard.UpdateMatchScore(matchEId, 7, 0);
            liveScoreboard.UpdateMatchScore(matchCId, 6, 0);

            liveScoreboard.FinishMatch(matchDId_ToFinish);

            var liveMatchSummary = liveScoreboard.GetSummary();

            // Assert
            Assert.IsNotNull(liveMatchSummary);
            Assert.AreEqual(2, liveScoreboard.Count());
            Assert.IsFalse(liveMatchSummary.Any(m => m.Id == matchAId_ToFinish));
            Assert.IsFalse(liveMatchSummary.Any(m => m.Id == matchBId_ToFinish));
            Assert.IsFalse(liveMatchSummary.Any(m => m.Id == matchDId_ToFinish));

            var liveMatchSummaryArray = liveMatchSummary.ToArray();
            Assert.AreEqual(matchEId, liveMatchSummaryArray[0].Id);
            Assert.AreEqual(matchCId, liveMatchSummaryArray[1].Id);
        }
    }
}