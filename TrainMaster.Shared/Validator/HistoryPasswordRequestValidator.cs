using FluentValidation;
using TrainMaster.Domain.Entity;
using TrainMaster.Shared.Enums;
using TrainMaster.Shared.Helpers;

namespace TrainMaster.Shared.Validator
{
    public class HistoryPasswordRequestValidator : AbstractValidator<HistoryPasswordEntity>
    {
        public HistoryPasswordRequestValidator()
        {
            RuleFor(p => p.OldPassword)
                .NotEmpty()
                    .WithMessage(HistoryPasswordErrors.HistoryPassword_Error_OldPasswordCanNotBeNullOrEmpty.Description());
        }
    }
}