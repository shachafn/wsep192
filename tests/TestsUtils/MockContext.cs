using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.DataAccessLayer;
using System.Collections;
using System.Collections.Generic;

namespace TestsUtils
{
    public class MockContext : IContext
    {
        ISession CurrentSession;
        private bool IsInTransaction { get => (CurrentSession != null) && CurrentSession.IsInTransaction(); }


        public ISession GetCurrentSession()
        {
            return CurrentSession;
        }

        public ISession StartSession()
        {
            CurrentSession = new MockSession(this);
            return CurrentSession;
        }
    }
}
