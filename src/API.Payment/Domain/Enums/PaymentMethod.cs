using Domain.Base.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Payment.Domain.Enums
{
    public class PaymentMethod : Enumeration
    {
        public static readonly PaymentMethod Stripe = new PaymentMethod(1, "stripe");

        public PaymentMethod(int id, string name) : base(id, name)
        {

        }
    }
}
