using ApplicationCore.Interfaces.ExternalServices;

namespace ApplicationCore.Interfaces.DomainLayer
{
    public interface IExternalServicesManager
    {
        IPaymentSystem PaymentSystem { get; }
        ISupplySystem SupplySystem { get; }
    }
}