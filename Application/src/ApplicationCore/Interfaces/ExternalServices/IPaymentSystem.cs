namespace ApplicationCore.Interfaces.ExternalServices
{
    public interface IPaymentSystem : IExternalService
    {

        /// <summary>
        /// Charges payment.
        /// </summary>
        /// <returns> Transaction id - an integer in the range [10000, 100000] which indicates 
        /// a transaction number if the transaction succeeds or -1 if the transaction has failed.</returns>
        int Pay();

        /// <summary>
        /// Charges payment.
        /// </summary>
        /// <returns> True if the cancelation succeeded, false otherwise.</returns>
        bool CancelPayment();
    }
}
