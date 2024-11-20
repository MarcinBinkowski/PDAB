using System;
using System.Collections.Generic;

namespace PDAB.Models;

public partial class Discount
{
    public int DiscountId { get; set; }

    public string? DiscountName { get; set; }

    public decimal DiscountPercentage { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsActive { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<DiscountProduct> DiscountProducts { get; set; } = new List<DiscountProduct>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
