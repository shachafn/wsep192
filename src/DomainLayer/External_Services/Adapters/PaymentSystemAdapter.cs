using DomainLayer.External_Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.External_Services.Adapters
{
    public class PaymentSystemAdapter : IPaymentSystem
    {
        public bool IsAvailable()
        {
            return true;
        }
    }
}
