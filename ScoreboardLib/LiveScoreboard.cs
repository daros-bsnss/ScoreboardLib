using FluentValidation;
using ScoreboardLib.Interfaces;
using ScoreboardLib.Models;
using ScoreboardLib.Validation;
using System;

namespace ScoreboardLib
{
    public class LiveScoreboard
    {
        private readonly CreateLiveMatchValidator createValidator = new();
        private readonly UpdateLiveMatchValidator updateValidator = new();
        private readonly ILiveMatchRepository liveMatchRepository;

        public LiveScoreboard(ILiveMatchRepository liveMatchRepository)
        {
            this.liveMatchRepository = liveMatchRepository;
        }

        public Guid StartMatch(string homeTeam, string awayTeam)
        {
            var match = new LiveMatch
            {
                HomeTeam = homeTeam,
                AwayTeam = awayTeam
            };

            createValidator.ValidateAndThrow(match);

            return liveMatchRepository.Insert(match);
        }
    }
}
