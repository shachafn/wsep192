using ApplicationCore.Interfaces.DomainLayer;
using ApplicationCore.Interfaces.ServiceLayer;
using DomainLayer.Domains;
using DomainLayer.Facade;
using Microsoft.Extensions.Logging.Abstractions;
using ServiceLayer;

namespace TestsUtils
{
    public class MocksCreator
    {
        public static IDomainLayerFacade GetDomainLayerFacade()
        {
            var uoW = new MockUnitOfWork(new MockContext());
            var cartDomain = new ShoppingCartDomain(uoW, new NullLogger<ShoppingCartDomain>());
            var shopDomain = new ShopDomain(cartDomain, uoW, new NullLogger<ShopDomain>());
            return new DomainFacadeTransactionProxy(
                        new DomainLayerFacade(
                            new UserDomain(NullLogger<UserDomain>.Instance, uoW, shopDomain),
                            new DomainLayerFacadeVerifier(NullLogger<DomainLayerFacadeVerifier>.Instance, uoW),
                            NullLogger<DomainLayerFacade>.Instance,
                            new DefaultExternalServicesManager(),
                            uoW
                        ),
                        new NullLogger<DomainFacadeTransactionProxy>(),
                        uoW
                    );
        }

        public static IServiceFacade GetServiceFacade()
        {
            var uoW = new MockUnitOfWork(new MockContext());
            var cartDomain = new ShoppingCartDomain(uoW, new NullLogger<ShoppingCartDomain>());
            var shopDomain = new ShopDomain(cartDomain, uoW, new NullLogger<ShopDomain>());
            return new ServiceFacadeProxy
            (
                new ServiceFacade(
                    new DomainFacadeTransactionProxy(
                        new DomainLayerFacade(
                            new UserDomain(NullLogger<UserDomain>.Instance, uoW, shopDomain),
                            new DomainLayerFacadeVerifier (NullLogger<DomainLayerFacadeVerifier>.Instance, uoW),
                            NullLogger<DomainLayerFacade>.Instance,
                            new DefaultExternalServicesManager(),
                            uoW
                        ),
                        new NullLogger<DomainFacadeTransactionProxy>(),
                        uoW
                    ),
                    NullLogger<ServiceFacade>.Instance
                ),
                new SessionManager(NullLogger<SessionManager>.Instance),
                NullLogger<ServiceFacadeProxy>.Instance
            );
        }
    }
}
