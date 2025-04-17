namespace Factor.Application.DTOs;

public class ProductDto
{
    public int ProductId { get; set; }
    public string ProductCode { get; set; }
    public string ProductName { get; set; }
    public string Unit { get; set; }
    public DateTime ChangeDate { get; set; }
}