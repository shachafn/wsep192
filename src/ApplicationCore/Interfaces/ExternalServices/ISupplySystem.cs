namespace ApplicationCore.Interfaces.ExternalServices
{
    public interface ISupplySystem : IExternalService
    {
        /// <summary>
        /// Dispatches a delivery to the costumer.
        /// </summary>
        /// <returns>transaction id - an integer in the range [10000, 100000] which
        /// indicates a transaction number if the transaction succeeds or -1 if the transaction has failed. </returns>
        int Supply();

        /// <summary>
        /// Cancels a supply transaction.
        /// </summary>
        /// <returns>True if the cancelation succeeded. False otherwise.</returns>
        bool CancelSupply();
    }
}
