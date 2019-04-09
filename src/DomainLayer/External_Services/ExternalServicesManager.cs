using DomainLayer.External_Services.Adapters;
using DomainLayer.External_Services.Interfaces;

namespace DomainLayer.External_Services
{
    public static class ExternalServicesManager
    {
        public static ISupplySystem _supplySystem = new SupplySystemAdapter();
        public static IPaymentSystem _paymentSystem = new PaymentSystemAdapter();
    }
}
