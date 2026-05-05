using PortfolioWebsite.Objects.InputObjects;
using FluentValidation;

namespace PortfolioWebsite.Validators
{
    public class EducationValidator : AbstractValidator<InputEducation>
    {
        public EducationValidator()
        {
            RuleFor(e => e.EndDate)
                .Must(LaterThanStartDate).WithMessage("Enddate must be later than startDate");
            //RuleFor(e => e.OnGoing).Must()
        }
        private static bool LaterThanStartDate(InputEducation education, DateOnly endDate)
        {
            if (education.OnGoing)
            {
                return true;
            }

            return endDate.CompareTo(education.StartDate) > 0;
        }
    }
}
