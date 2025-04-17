using Factor.Application.DTOs;
using Factor.Domain;
using FluentValidation;

namespace Factor.Application.Validators;

public class CreateInvoiceDtoValidator : AbstractValidator<CreateInvoiceDto>
{
    public CreateInvoiceDtoValidator()
    {
        RuleFor(i => i.FactorNo).GreaterThan(0);
        RuleFor(i => i.FactorDate).LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));

        RuleFor(i => i.DelivaryType).IsInEnum();
        RuleFor(i => i.Items).NotEmpty();

        RuleForEach(i => i.Items).SetValidator(new InvoiceItemDtoValidator());
    }
}