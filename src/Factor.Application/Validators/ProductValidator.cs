using Factor.Application.DTOs;
using FluentValidation;

namespace Factor.Application.Validators;

// Application/Validators/ProductValidator.cs
public class ProductDtoValidator : AbstractValidator<ProductDto>
{
    public ProductDtoValidator()
    {
        RuleFor(p => p.ProductCode).NotEmpty().MaximumLength(13);
        RuleFor(p => p.ProductName).NotEmpty().MaximumLength(50);
        RuleFor(p => p.Unit).NotEmpty().MaximumLength(20);
    }
}