using System;
using System.Collections.Generic;

namespace WpfApp1.Models;

public partial class DiscountProduct
{
    public int DiscountProductId { get; set; }

    public int DiscountId { get; set; }

    public int ProductId { get; set; }

    public virtual Discount Discount { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
