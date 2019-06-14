namespace ApplicationCore.Interfaces.DataAccessLayer
{
    public interface IContext
    {
        ISession StartSession();
        ISession GetCurrentSession();
    }
}
