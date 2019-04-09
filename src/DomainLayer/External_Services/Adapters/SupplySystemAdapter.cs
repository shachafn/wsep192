using DomainLayer.External_Services.Interfaces;

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
