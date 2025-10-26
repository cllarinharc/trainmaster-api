using FluentValidation;
using TrainMaster.Domain.Entity;
using TrainMaster.Shared.Enums;
using TrainMaster.Shared.Helpers;

namespace TrainMaster.Shared.Validator
{
    public class DepartmentRequestValidator : AbstractValidator<DepartmentEntity>
    {
        public DepartmentRequestValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                    .WithMessage(DepartmentErrors.Department_Error_NameCanNotBeNullOrEmpty.Description());

            RuleFor(p => p.Description)
                .NotEmpty()
                    .WithMessage(DepartmentErrors.Department_Error_DescriptionCanNotBeNullOrEmpty.Description());
        }
    }
}