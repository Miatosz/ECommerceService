using ECommerceService.Api.Dto;
using FluentValidation;

namespace ECommerceService.Api.Validators
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(2, 100);

            RuleFor(x => x.Country)
                .MaximumLength(50);
        }
    }
}
