using FluentValidation;
using HotelBooking.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.Validations
{
    public class CreateHotelDtoValidator : AbstractValidator<CreateHotelDto>
    {
        public CreateHotelDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Hotel name is required.")
                .MaximumLength(100);

            RuleFor(x =>x.City)
                .NotEmpty().WithMessage("City is required.");

            RuleFor(x => x.Star)
                .InclusiveBetween(1,5).WithMessage("Star must be between 1 and 5.");
        }
    }
}
