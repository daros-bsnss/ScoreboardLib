using FluentValidation;
using ScoreboardLib.Validation;
using System;

namespace ScoreboardLib.Models
{
    public class UpdateLiveMatchValidator : CreateLiveMatchValidator
    {
        public UpdateLiveMatchValidator() : base()
        {
            RuleFor(m => m.Id).Must(i => i != Guid.Empty);
        }
    }
}
