using System;
using System.Diagnostics.CodeAnalysis;

namespace ScoreboardLib.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class LiveMatchNotStartedException : Exception
    {
        public LiveMatchNotStartedException() : base("Live match has not started") { }
    }
}
