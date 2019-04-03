using DomainLayer.External_Services.Interfaces;

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
