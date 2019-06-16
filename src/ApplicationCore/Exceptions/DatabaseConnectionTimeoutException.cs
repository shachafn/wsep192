using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Exceptions
{
    /// <summary>
    /// Indicates that the connection to the DB has incurred a timeout.
    /// </summary>
    public class DatabaseConnectionTimeoutException : Exception
    {
        //SHOULDNT INHERIT FROM BaseException
        public DatabaseConnectionTimeoutException()
        {
        }

        public DatabaseConnectionTimeoutException(string message) : base(message)
        {
        }

        public DatabaseConnectionTimeoutException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
