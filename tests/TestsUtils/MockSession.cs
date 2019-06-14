using ApplicationCore.Interfaces.DataAccessLayer;
using System;

namespace TestsUtils
{
    public class MockSession : ISession
    {
        private MockContext mockContext;
        private bool _isInTransaction = false;

        public MockSession(MockContext mockContext)
        {
            this.mockContext = mockContext;
        }

        public void StartTransaction()
        {
            if (_isInTransaction)
                throw new InvalidOperationException("Already in transaction.");

            _isInTransaction = true;
        }

        public void CommitTransaction()
        {
            _isInTransaction = false;
        }

        public void AbortTransaction()
        {
            _isInTransaction = false;
        }

        public bool IsInTransaction()
        {
            return _isInTransaction;
        }
    }
}
