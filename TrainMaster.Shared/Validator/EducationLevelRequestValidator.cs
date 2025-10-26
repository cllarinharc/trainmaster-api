using FluentValidation;
using TrainMaster.Domain.Entity;
using TrainMaster.Shared.Enums;
using TrainMaster.Shared.Helpers;

namespace TrainMaster.Shared.Validator
{
    public class EducationLevelRequestValidator : AbstractValidator<EducationLevelEntity>
    {
        public EducationLevelRequestValidator()
        {
            RuleFor(p => p.Title)
                .NotEmpty()
                    .WithMessage(EducationLevelErrors.EducationLevel_Error_TitleCanNotBeNullOrEmpty.Description());

            RuleFor(p => p.Institution)
                .NotEmpty()
                    .WithMessage(EducationLevelErrors.EducationLevel_Error_InstitutionCanNotBeNullOrEmpty.Description());

            RuleFor(p => p.StartedAt)
                .NotEmpty()
                    .WithMessage(EducationLevelErrors.EducationLevel_Error_StartedAtCanNotBeNullOrEmpty.Description());

            RuleFor(p => p.EndeedAt)
                .NotEmpty()
                    .WithMessage(EducationLevelErrors.EducationLevel_Error_EndeedAtCanNotBeNullOrEmpty.Description());
        }
    }
}