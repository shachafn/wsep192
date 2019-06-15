namespace DataAccessLayer
{
    public class DatabaseConfiguration
    {
        public readonly string ConnectionString;

        public readonly string DatabaseName;

        public DatabaseConfiguration(string connectionString, string databaseName)
        {
            ConnectionString = connectionString;
            DatabaseName = databaseName;
        }
    }
}
