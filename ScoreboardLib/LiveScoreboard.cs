using FluentValidation;
using ScoreboardLib.Exceptions;
using ScoreboardLib.Interfaces;
using ScoreboardLib.Models;
using ScoreboardLib.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

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
            bool areTeamsInLiveMatch = liveMatchRepository.GetAll().Any(m =>
                m.HomeTeam == homeTeam ||
                m.HomeTeam == awayTeam ||
                m.AwayTeam == homeTeam ||
                m.AwayTeam == awayTeam);

            if (areTeamsInLiveMatch)
            {
                throw new LiveMatchAlreadyStartedException();
            }

            var match = new LiveMatch
            {
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                StartedDateTime = DateTime.UtcNow
            };

            createValidator.ValidateAndThrow(match);

            return liveMatchRepository.Insert(match);
        }

        public void UpdateMatchScore(Guid matchId, int homeTeamScore, int awayTeamScore)
        {
            var match = liveMatchRepository.GetAll().FirstOrDefault(m => m.Id == matchId)
                ?? throw new LiveMatchNotStartedException();

            match.Score = (homeTeamScore, awayTeamScore);

            updateValidator.ValidateAndThrow(match);

            liveMatchRepository.Update(match);
        }

        public void FinishMatch(Guid matchId)
        {
            if (!liveMatchRepository.GetAll().Any(m => m.Id == matchId))
            {
                throw new LiveMatchNotStartedException();
            }

            liveMatchRepository.Delete(matchId);
        }

        public IEnumerable<LiveMatch> GetSummary()
        {
            return liveMatchRepository.GetAll()
                .OrderByDescending(m => m.Score.Item1 + m.Score.Item2)
                .ThenByDescending(m => m.StartedDateTime);
        }
    }
}
