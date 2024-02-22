using ScoreboardLib.Models;
using ScoreboardLib.Validation;

namespace ScoreboardLib.Tests.Validation
{
    [TestClass]
    public class CreateLiveMatchValidatorTests
    {
        private readonly CreateLiveMatchValidator validator = new();

        [TestMethod]
        public void Validate_ShouldPass()
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
            Assert.IsTrue(validationResult.IsValid);
        }

        [TestMethod]
        public void Validate_ShouldFail_WhenEmptyTeam()
        {
            // Arrange
            var match = new LiveMatch
            {
                HomeTeam = "",
                AwayTeam = "Chroatia"
            };

            // Act
            var validationResult = validator.Validate(match);

            // Assert
            Assert.IsNotNull(validationResult);
            Assert.IsFalse(validationResult.IsValid);
        }

        [TestMethod]
        public void Validate_ShouldFail_WhenNullTeam()
        {
            // Arrange
            var match = new LiveMatch
            {
                HomeTeam = "Germany",
                AwayTeam = null
            };

            // Act
            var validationResult = validator.Validate(match);

            // Assert
            Assert.IsNotNull(validationResult);
            Assert.IsFalse(validationResult.IsValid);
        }

        [TestMethod]
        public void Validate_ShouldFail_WhenTeamsEqual()
        {
            // Arrange
            var match = new LiveMatch
            {
                HomeTeam = "Germany   ",
                AwayTeam = "GeRmanY"
            };

            // Act
            var validationResult = validator.Validate(match);

            // Assert
            Assert.IsNotNull(validationResult);
            Assert.IsFalse(validationResult.IsValid);
        }

        [TestMethod]
        public void Validate_ShouldFail_WhenNegativeScore()
        {
            // Arrange
            var match = new LiveMatch
            {
                Id = Guid.NewGuid(),
                HomeTeam = "Germany",
                AwayTeam = "Chroatia",
                Score = (-1, -7),
                StartedDateTime = DateTime.UtcNow
            };

            // Act
            var validationResult = validator.Validate(match);

            // Assert
            Assert.IsNotNull(validationResult);
            Assert.IsFalse(validationResult.IsValid);
        }
    }
}
