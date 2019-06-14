namespace ApplicationCore.Interfaces.DataAccessLayer
{
    public interface ISession
    {
        void StartTransaction();
        void CommitTransaction();
        void AbortTransaction();
        bool IsInTransaction();
    }
}