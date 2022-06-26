using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Models.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Title)
                .Length(3, 100)
                .WithMessage("Bad title!")
                .NotNull()
                .WithMessage("Gde title?");

            RuleFor(x => x.Price).NotNull();
        }
    }
}
