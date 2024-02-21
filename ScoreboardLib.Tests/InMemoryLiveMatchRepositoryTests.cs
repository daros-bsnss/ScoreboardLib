using ScoreboardLib.Models;

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

        [TestMethod]
        public void Insert_ShouldAddMatch_AndReturnId()
        {
            // Arrange
            var match = new LiveMatch();

            // Act
            Guid id = repository.Insert(match);

            // Assert
            Assert.AreNotEqual(Guid.Empty, id);
        }

        [TestMethod]
        public void Update_ShouldUpdateAndReturnTrue()
        {
            // Arrange
            var match = new LiveMatch();
            Guid id = repository.Insert(match);

            // Act
            bool isUpdated = repository.Update(id, 1, 0);

            // Assert
            Assert.IsTrue(isUpdated);
        }

        [TestMethod]
        public void Update_ShouldReturnFalse_WhenMatchNotFound()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            // Act
            bool isUpdated = repository.Update(id, 1, 0);

            // Assert
            Assert.IsFalse(isUpdated);
        }

        [TestMethod]
        public void Delete_ShouldDeleteAndReturnTrue()
        {
            // Arrange
            var match = new LiveMatch();
            Guid id = repository.Insert(match);

            // Act
            bool isDeleted = repository.Delete(id);

            // Assert
            Assert.IsTrue(isDeleted);
        }

        [TestMethod]
        public void Delete_ShouldReturnFalse_WhenMatchNotFound()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            // Act
            bool isDeleted = repository.Delete(id);

            // Assert
            Assert.IsFalse(isDeleted);
        }
    }
}
