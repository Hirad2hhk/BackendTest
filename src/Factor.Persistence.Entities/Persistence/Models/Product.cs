using System;
using System.Collections.Generic;

namespace Factor.Persistence.Entities.Persistence.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductCode { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public string? Unit { get; set; }

    public DateTime ChangeDate { get; set; }

    public virtual ICollection<FactorDetail> FactorDetails { get; set; } = new List<FactorDetail>();
}
