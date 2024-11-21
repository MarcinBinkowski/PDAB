using System;
using System.Collections.Generic;

namespace PDAB.Models;

public partial class ProductImage
{
    public int ProductImageId { get; set; }

    public int? ProductId { get; set; }

    public string? ImageUrl { get; set; }

    public virtual Product? Product { get; set; }
}
