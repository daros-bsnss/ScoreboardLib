using ScoreboardLib.Models;

namespace ScoreboardLib.Tests.Validation
{
    [TestClass]
    public class UpdateLiveMatchValidatorTests
    {
        private readonly UpdateLiveMatchValidator validator = new();

        [TestMethod]
        public void Validate_ShouldPass()
        {
            // Arrange
            var match = new LiveMatch
            {
                Id = Guid.NewGuid(),
                HomeTeam = "Germany",
                AwayTeam = "Chroatia",
                Score = (1, 0),
                StartedDateTime = DateTime.UtcNow
            };

            // Act
            var validationResult = validator.Validate(match);

            // Assert
            Assert.IsNotNull(validationResult);
            Assert.IsTrue(validationResult.IsValid);
        }

        [TestMethod]
        public void Validate_ShouldFail_WhenEmptyId()
        {
            // Arrange
            var match = new LiveMatch
            {
                HomeTeam = "Germany",
                AwayTeam = "Chroatia"
            };

            // Act
            var validationResult = validator.Validate(match);

            // Assert
            Assert.IsNotNull(validationResult);
            Assert.IsFalse(validationResult.IsValid);
        }
    }
}
