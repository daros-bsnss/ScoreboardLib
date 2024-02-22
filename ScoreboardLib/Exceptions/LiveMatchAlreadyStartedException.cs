using System;
using System.Diagnostics.CodeAnalysis;

namespace ScoreboardLib.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class LiveMatchAlreadyStartedException : Exception
    {
        public LiveMatchAlreadyStartedException() : base("Live match is already started") { }
    }
}
