using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Exceptions
{
    /// <summary>
    /// Indicating a general server error, thrown in DomainFacadeTransactionProxy.
    /// Inspect the InnerException for more information.
    /// </summary>
    public class GeneralServerError : BaseException
    {
        public GeneralServerError()
        {
        }

        public GeneralServerError(string msg) : base(msg)
        {
        }

        public GeneralServerError(string msg, Exception ex) : base(msg, ex)
        {
        }
    }
}
