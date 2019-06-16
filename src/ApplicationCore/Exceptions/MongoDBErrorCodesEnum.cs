using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Exceptions
{
    public static class MongoDBErrorCodesEnum
    {
        public static bool IsWriteConflictExcpetion(this MongoCommandException mongoCommandException)
        {
            if (mongoCommandException.Code.Equals(MongoErrorCode.WriteConflict))
            {
                return true;
            }
            return false;
        }

        public enum MongoErrorCode
        {
            WriteConflict = 112
        }
         
    }
}
