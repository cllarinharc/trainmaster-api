using FluentValidation;
using TrainMaster.Domain.Entity;
using TrainMaster.Shared.Enums;
using TrainMaster.Shared.Helpers;

namespace TrainMaster.Shared.Validator
{
    public class TeamRequestValidator : AbstractValidator<TeamEntity>
    {
        public TeamRequestValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                    .WithMessage(TeamErrors.Team_Error_NameCanNotBeNullOrEmpty.Description());

            RuleFor(p => p.Description)
                .NotEmpty()
                    .WithMessage(TeamErrors.Team_Error_DescriptionCanNotBeNullOrEmpty.Description());
        }
    }
}