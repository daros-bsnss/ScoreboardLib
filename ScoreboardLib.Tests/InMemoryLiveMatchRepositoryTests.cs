using System.Linq;

namespace ScoreboardLib.Tests
{
    [TestClass]
    public class InMemoryLiveMatchRepositoryTests
    {
        private readonly InMemoryLiveMatchRepository repository = new();

        [TestMethod]
        public void GetAll_ShouldReturnEmptyCollection_WhenInit()
        {
            // Arrange
            // Act
            var matches = repository.GetAll();

            // Assert
            Assert.IsNotNull(matches);
            Assert.AreEqual(0, matches.Count());
        }
    }
}
