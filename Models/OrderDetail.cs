﻿using System;
using System.Collections.Generic;

namespace PDAB.Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public int DiscountId { get; set; }

    public virtual Discount Discount { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
