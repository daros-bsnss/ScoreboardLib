using ScoreboardLib.Interfaces;
using ScoreboardLib.Models;
using System;
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

        public Guid Insert(LiveMatch match)
        {
            match.Id = Guid.NewGuid();
            matches.Add(match);
            return match.Id;
        }

        public bool Update(LiveMatch match)
        {
            var storedMatch = matches.Find(m => m.Id == match.Id);

            if (storedMatch is null)
            {
                return false;
            }

            storedMatch.Score = match.Score;
            return true;
        }

        public bool Delete(Guid matchId)
        {
            var storedMatch = matches.Find(m => m.Id == matchId);

            if (storedMatch is null)
            {
                return false;
            }

            matches.Remove(storedMatch);
            return true;
        }
    }
}
