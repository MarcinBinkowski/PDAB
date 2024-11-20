using System;
using System.Collections.Generic;

namespace PDAB.Models;

public partial class OrderPayment
{
    public int OrderPaymentId { get; set; }

    public int OrderId { get; set; }

    public int PaymentMethodId { get; set; }

    public DateTime PaymentDate { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual PaymentMethod PaymentMethod { get; set; } = null!;
}
