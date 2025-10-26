using FluentValidation;
using TrainMaster.Domain.Dto;
using TrainMaster.Shared.Enums;
using TrainMaster.Shared.Helpers;

namespace TrainMaster.Shared.Validator
{
    public class CourseRequestValidator : AbstractValidator<CourseDto>
    {
        public CourseRequestValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                    .WithMessage(CourseErrors.Course_Error_NameCanNotBeNullOrEmpty.Description());

            RuleFor(p => p.Description)
                .NotEmpty()
                    .WithMessage(CourseErrors.Course_Error_DescriptionCanNotBeNullOrEmpty.Description());

            RuleFor(p => p.StartDate)
                .NotEmpty()
                    .WithMessage(CourseErrors.Course_Error_StartDateCanNotBeNullOrEmpty.Description());

            RuleFor(p => p.EndDate)
                .NotEmpty()
                    .WithMessage(CourseErrors.Course_Error_EndDateCanNotBeNullOrEmpty.Description());
        }
    }
}