using ApplicationCore.Interfaces.DomainLayer;
using ApplicationCore.Interfaces.ExternalServices;

namespace DomainLayer.External_Services
{
    public class ExternalServicesManager : IExternalServicesManager
    {
        public ISupplySystem SupplySystem { get; set; }
        public IPaymentSystem PaymentSystem { get; set; }

        public ExternalServicesManager(IPaymentSystem paymentSystem, ISupplySystem supplySystem)
        {
            SupplySystem = supplySystem;
            PaymentSystem = paymentSystem;
        }
    }
}
