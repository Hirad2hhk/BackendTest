using Factor.Application.DTOs;
using FluentValidation;

namespace Factor.Application.Validators;

public class InvoiceItemDtoValidator : AbstractValidator<InvoiceItemDto>
{
    public InvoiceItemDtoValidator()
    {
        RuleFor(i => i.ProductId).GreaterThan(0);
        RuleFor(i => i.Count).GreaterThan(0);
        RuleFor(i => i.UnitPrice).GreaterThan(0);
    }
}