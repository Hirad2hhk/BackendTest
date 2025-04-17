using System;
using System.Collections.Generic;

namespace Factor.Persistence.Entities.Persistence.Models;

public partial class FactorDetail
{
    public int FactorDetailId { get; set; }

    public int FactorId { get; set; }

    public int ProductId { get; set; }

    public string? ProductDescription { get; set; }

    public decimal Count { get; set; }

    public int UnitPrice { get; set; }

    public long SumPrice { get; set; }

    public virtual Factor Factor { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
