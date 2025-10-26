using FluentValidation;
using TrainMaster.Domain.Entity;
using TrainMaster.Shared.Enums;
using TrainMaster.Shared.Helpers;

namespace TrainMaster.Shared.Validator
{
    public class ProfessionalProfileRequestValidator : AbstractValidator<ProfessionalProfileEntity>
    {
        public ProfessionalProfileRequestValidator()
        {
            RuleFor(p => p.JobTitle)
                .NotEmpty()
                    .WithMessage(ProfessionalProfileErrors.ProfessionalProfile_Error_JobTitleCanNotBeNullOrEmpty.Description());

            RuleFor(p => p.YearsOfExperience)
                .NotEmpty()
                    .WithMessage(ProfessionalProfileErrors.ProfessionalProfile_Error_YearsOfExperienceCanNotBeNullOrEmpty.Description());

            RuleFor(p => p.Skills)
                .NotEmpty()
                    .WithMessage(ProfessionalProfileErrors.ProfessionalProfile_Error_SkillsCanNotBeNullOrEmpty.Description());
        }
    }
}