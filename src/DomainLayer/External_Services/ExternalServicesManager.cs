﻿using ApplicationCore.Interfaces.ExternalServices;
using DomainLayer.External_Services.Adapters;

namespace DomainLayer.External_Services
{
    public class ExternalServicesManager
    {
        public ISupplySystem SupplySystem { get; set; }
        public IPaymentSystem PaymentSystem { get; set; }

        public ExternalServicesManager(IPaymentSystem paymentSystem)
        {
            SupplySystem = new SupplySystemAdapter();
            PaymentSystem = paymentSystem;
        }
    }
}
