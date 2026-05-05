using PortfolioWebsite.Objects.InputObjects;
using FluentValidation;

namespace PortfolioWebsite.Validators
{
    public class ExperienceValidator : AbstractValidator<InputExperience>
    {
        public ExperienceValidator()
        {
            RuleFor(e => e.EndDate)
                .Must(LaterThanStartDate).WithMessage("Enddate must be later than startDate");
        }
        private static bool LaterThanStartDate(
            InputExperience experience,
            DateOnly endDate)
            => endDate.CompareTo(experience.StartDate) > 0;

    }
}
