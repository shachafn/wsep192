using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Interfaces.ExternalServices;

namespace ApplicationCore.Interfaces.DomainLayer
{
    public class DefaultExternalServicesManager : IExternalServicesManager
    {
        public IPaymentSystem PaymentSystem => new DefaultPaymentSystem();

        public ISupplySystem SupplySystem => new DefaultSupplySystem();

        internal class DefaultPaymentSystem : IPaymentSystem
        {
            public bool IsAvailable() => true;
        }
        internal class DefaultSupplySystem : ISupplySystem
        {
            public bool IsAvailable() => true;
        }
    }
}
