using FluentValidation;
using ScoreboardLib.Models;

namespace ScoreboardLib.Validation
{
    public class CreateLiveMatchValidator : AbstractValidator<LiveMatch>
    {
        public CreateLiveMatchValidator()
        {
            RuleFor(m => m.HomeTeam).NotNull().NotEmpty();
            RuleFor(m => m.AwayTeam).NotNull().NotEmpty();
            RuleFor(m => m.Score).Must(s => s.Item1 >= 0).Must(s => s.Item2 >= 0);
            RuleFor(m => m.StartedDateTime).NotEmpty();
            RuleFor(m => m).Custom((match, context) =>
            {
                var homeTeam = match.HomeTeam?.Trim().ToLowerInvariant();
                var awayTeam = match.AwayTeam?.Trim().ToLowerInvariant();

                if (homeTeam == awayTeam)
                {
                    context.AddFailure("Teams are equal");
                }
            });
        }
    }
}
