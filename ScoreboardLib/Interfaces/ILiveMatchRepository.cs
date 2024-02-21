using ScoreboardLib.Models;
using System.Collections.Generic;

namespace ScoreboardLib.Interfaces
{
    public interface ILiveMatchRepository
    {
        public IEnumerable<LiveMatch> GetAll();
    }
}
