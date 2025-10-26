using FluentValidation;
using TrainMaster.Domain.Entity;
using TrainMaster.Shared.Enums;
using TrainMaster.Shared.Helpers;

namespace TrainMaster.Shared.Validator
{
    public class PessoalProfileRequestValidator : AbstractValidator<PessoalProfileEntity>
    {
        public PessoalProfileRequestValidator()
        {
            RuleFor(p => p.FullName)
                .NotEmpty()
                    .WithMessage(PessoalProfileErrors.PessoalProfile_Error_NameCanNotBeNullOrEmpty.Description())
                .MinimumLength(4)
                    .WithMessage(PessoalProfileErrors.PessoalProfile_Error_NameLenghtLessFour.Description());
        }
    }
}