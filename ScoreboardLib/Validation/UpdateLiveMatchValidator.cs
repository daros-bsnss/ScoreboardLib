using FluentValidation;
using ScoreboardLib.Validation;

namespace ScoreboardLib.Models
{
    public class UpdateLiveMatchValidator : CreateLiveMatchValidator
    {
        public UpdateLiveMatchValidator() : base()
        {
            RuleFor(m => m.Id).NotEmpty();
        }
    }
}
