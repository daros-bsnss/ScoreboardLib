using ScoreboardLib.Models;
using System;
using System.Collections.Generic;

namespace ScoreboardLib.Interfaces
{
    public interface ILiveMatchRepository
    {
        public IEnumerable<LiveMatch> GetAll();
        public Guid Insert(LiveMatch match);
        public bool Update(LiveMatch match);
        public bool Delete(Guid matchId);
    }
}
