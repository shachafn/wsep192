
using ApplicationCore.Interfaces.ExternalServices;

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
