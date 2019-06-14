using ApplicationCore.Interfaces.DataAccessLayer;

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
