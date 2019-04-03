namespace ATBridge
{
    public abstract class Driver
    {
        public static IBridge GetBridge()
        {
            ProxyBridge bridge = new ProxyBridge();
            bridge.SetRealBridge(new BridgeImpl());
            return bridge;
        }
    }
}
