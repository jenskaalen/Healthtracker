using System.Data.SqlClient;

namespace healthtracker.Repository
{
    public class ConnectionFactory : IConnectionFactory
    {
        public SqlConnection GetConnection()
        {
            return new SqlConnection("Server=localhost;Initial catalog=HealthTracker;Trusted_connection=True");
        }
    }
}