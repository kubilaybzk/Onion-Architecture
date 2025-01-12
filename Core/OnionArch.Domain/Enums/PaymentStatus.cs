using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Enums
{
    public enum PaymentStatus
    {
        Created = 0,
        Processing = 1,
        Success = 2,
        Failed = 3,
        Refunded = 4,
        Cancelled = 5
    }
}
