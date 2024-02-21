using ScoreboardLib.Interfaces;
using ScoreboardLib.Models;
using System.Collections.Generic;

namespace ScoreboardLib
{
    public class InMemoryLiveMatchRepository : ILiveMatchRepository
    {
        private readonly List<LiveMatch> matches = new();

        public IEnumerable<LiveMatch> GetAll()
        {
            return matches;
        }
    }
}
