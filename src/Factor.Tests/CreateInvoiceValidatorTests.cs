using Factor.Application.DTOs;
using Factor.Application.Interfaces;
using Factor.Application.Validators;
using Factor.Domain;

namespace Factor.Tests;

public class CreateInvoiceValidatorTests
{
    private readonly CreateInvoiceDtoValidator _validator = new();
    private readonly IInvoiceRepository _invoiceRepo;

    public CreateInvoiceValidatorTests()
    {
        // Initialize the repository with appropriate implementation
        _invoiceRepo = new MockInvoiceRepository(); // Replace with actual implementation or mock
    }

    [Fact]
    public void Should_Reject_EmptyInvoice()
    {
        var dto = new CreateInvoiceDto(); // Missing required fields
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "FactorNo");
        Assert.Contains(result.Errors, e => e.PropertyName == "Items");
    }

    [Fact]
    public void Should_Validate_ValidInvoice()
    {
        var dto = new CreateInvoiceDto
        {
            FactorNo = 100,
            FactorDate = DateOnly.FromDateTime(DateTime.Today),
            DelivaryType = Domain.DeliveryType.Courier,
            Items = new List<InvoiceItemDto>
            {
                new() { ProductId = 1, Count = 2, UnitPrice = 5000 }
            }
        };

        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task CreateInvoice_ShouldSetTotalPriceCorrectly()
    {
        var invoiceDto = new CreateInvoiceDto
        {
            FactorNo = 1001,
            FactorDate = DateOnly.FromDateTime(DateTime.Today),
            Customer = "Ali",
            DelivaryType = DeliveryType.Post,
            Items = new()
            {
                new() { ProductId = 1, Count = 2, UnitPrice = 10000 }, // 20,000
                new() { ProductId = 1, Count = 1.5m, UnitPrice = 5000 } // 7,500
            }
        };

        var id = await _invoiceRepo.CreateAsync(invoiceDto);
        var invoice = await _invoiceRepo.GetByIdAsync(id);

        Assert.Equal(27500, invoice?.TotalPrice);
        Assert.Equal(2, invoice?.Items.Count);
    }

    [Fact]
    public async Task UpdateInvoice_ShouldReplaceOldItems()
    {
        var invoiceId = await _invoiceRepo.CreateAsync(new CreateInvoiceDto
        {
            FactorNo = 2002,
            FactorDate = DateOnly.FromDateTime(DateTime.Today),
            Customer = "Test",
            DelivaryType = DeliveryType.Courier,
            Items = new()
            {
                new() { ProductId = 1, Count = 1, UnitPrice = 5000 }
            }
        });

        var result = await _invoiceRepo.UpdateAsync(new UpdateInvoiceDto
        {
            FactorId = invoiceId,
            FactorNo = 2002,
            FactorDate = DateOnly.FromDateTime(DateTime.Today),
            Customer = "Updated",
            DelivaryType = DeliveryType.InPerson,
            Items = new()
            {
                new() { ProductId = 1, Count = 3, UnitPrice = 10000 } // new total: 30,000
            }
        });

        var invoice = await _invoiceRepo.GetByIdAsync(invoiceId);

        Assert.True(result);
        Assert.Single(invoice!.Items);
        Assert.Equal(30000, invoice.TotalPrice);
        Assert.Equal("Updated", invoice.Customer);
    }

    [Fact]
    public async Task UpdateInvoice_ShouldReturnFalse_IfNotFound()
    {
        var dto = new UpdateInvoiceDto
        {
            FactorId = 9999,
            FactorNo = 1,
            FactorDate = DateOnly.FromDateTime(DateTime.Today),
            DelivaryType = DeliveryType.InPerson,
            Items = new() { new() { ProductId = 1, Count = 1, UnitPrice = 1000 } }
        };

        var updated = await _invoiceRepo.UpdateAsync(dto);
        Assert.False(updated);
    }

    [Fact]
    public void Should_Reject_Invoice_With_Invalid_DeliveryType()
    {
        var dto = new CreateInvoiceDto
        {
            FactorNo = 100,
            FactorDate = DateOnly.FromDateTime(DateTime.Today),
            DelivaryType = (DeliveryType)99, // invalid
            Items = new() { new() { ProductId = 1, Count = 1, UnitPrice = 1000 } }
        };

        var validator = new CreateInvoiceDtoValidator();
        var result = validator.Validate(dto);

        Assert.False(result.IsValid);
    }
}

// You'll need to create this mock implementation if it doesn't exist already