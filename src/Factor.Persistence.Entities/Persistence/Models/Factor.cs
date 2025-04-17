using System;
using System.Collections.Generic;

namespace Factor.Persistence.Entities.Persistence.Models;

public partial class Factor
{
    public int FactorId { get; set; }

    public int FactorNo { get; set; }

    public DateOnly FactorDate { get; set; }

    public string? Customer { get; set; }

    public short? DelivaryType { get; set; }

    public long? TotalPrice { get; set; }

    public virtual ICollection<FactorDetail> FactorDetails { get; set; } = new List<FactorDetail>();
}
