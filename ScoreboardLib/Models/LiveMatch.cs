using System;
using System.Diagnostics.CodeAnalysis;

namespace ScoreboardLib.Models
{
    [ExcludeFromCodeCoverage]
    public class LiveMatch
    {
        public Guid Id { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public (int, int) Score { get; set; }
        public DateTime StartedDateTime { get; set; }
    }
}
