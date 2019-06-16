using System;

namespace ApplicationCore.Exceptions
{
    public class ExternalServiceFaultException : BaseException, ICloneableException<Exception>
    {
        public ExternalServiceType Type { get; set; }
        public ExternalServiceFaultException(ExternalServiceType type)
        {
            Type = type;
        }
        public ExternalServiceFaultException(string msg, ExternalServiceType type) :base(msg)
        {
            Type = type;
        }

        /// <summary>
        /// DO NOT USE
        /// </summary>
        public ExternalServiceFaultException(string msg) : base(msg)
        {
        }

        /// <summary>
        /// DO NOT USE
        /// </summary>
        Exception ICloneableException<Exception>.Clone(string msg)
        {
            return new ExternalServiceFaultException(msg);
        }
        public enum ExternalServiceType
        {
            Payment = 0,
            Supply = 1
        }
    }
}
