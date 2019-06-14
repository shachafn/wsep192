using ApplicationCore.Interfaces.ExternalServices;

namespace ApplicationCore.Interfaces.DomainLayer
{
    public class DefaultExternalServicesManager : IExternalServicesManager
    {
        public IPaymentSystem PaymentSystem => new DefaultPaymentSystem();

        public ISupplySystem SupplySystem => new DefaultSupplySystem();

        internal class DefaultPaymentSystem : IPaymentSystem
        {
            public bool CancelPayment() => true;

            public bool IsAvailable() => true;

            public int Pay() => 1;
        }
        internal class DefaultSupplySystem : ISupplySystem
        {
            public bool CancelSupply() => true;
            public bool IsAvailable() => true;

            public int Supply() => 1;
        }
    }
}
