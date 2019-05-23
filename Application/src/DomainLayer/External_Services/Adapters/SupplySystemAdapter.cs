
using ApplicationCore.Interfaces.ExternalServices;

namespace DomainLayer.External_Services.Adapters
{
    public class SupplySystemAdapter : ISupplySystem
    {
        public bool IsAvailable()
        {
            return true;
        }
    }
}
