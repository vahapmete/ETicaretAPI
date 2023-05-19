using ETicaretAPI.Application.ViewModels.Products;
using ETicaretAPI.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Validators.Products
{
    public class CreateProductValidator: AbstractValidator<Product_Create_VM>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage("Required")
                .MaximumLength(150)
                .MinimumLength(3).WithMessage("The name is must be between 3 and 150 characters");

            RuleFor(p => p.Stock)
                .NotEmpty()
                .NotNull()
                .WithMessage("Required")
                .Must(s => s >= 0).WithMessage("This is must not be lower than 0");

            RuleFor(p => p.Price)
               .NotEmpty()
               .NotNull()
               .WithMessage("Required")
               .Must(s => s >= 0).WithMessage("This is must not be lower than 0");
        }
    }
}
 