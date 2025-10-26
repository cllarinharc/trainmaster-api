using FluentValidation;
using TrainMaster.Domain.Entity;
using TrainMaster.Shared.Enums;
using TrainMaster.Shared.Helpers;

namespace TrainMaster.Shared.Validator
{
    public class AddressRequestValidator : AbstractValidator<AddressEntity>
    {
        public AddressRequestValidator()
        {
            RuleFor(p => p.PostalCode)
                .NotEmpty()
                    .WithMessage(AddressErrors.Address_Error_PostalCodeCanNotBeNullOrEmpty.Description());

            RuleFor(p => p.Street)
                .NotEmpty()
                    .WithMessage(AddressErrors.Address_Error_StreetCanNotBeNullOrEmpty.Description());

            RuleFor(p => p.Neighborhood)
                .NotEmpty()
                    .WithMessage(AddressErrors.Address_Error_NeighborhoodCanNotBeNullOrEmpty.Description());

            RuleFor(p => p.City)
                .NotEmpty()
                    .WithMessage(AddressErrors.Address_Error_CityCanNotBeNullOrEmpty.Description());

            RuleFor(p => p.Uf)
                .NotEmpty()
                    .WithMessage(AddressErrors.Address_Error_UfCanNotBeNullOrEmpty.Description());
        }
    }
}