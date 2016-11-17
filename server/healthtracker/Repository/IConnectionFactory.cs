using System.Data.SqlClient;

namespace healthtracker.Repository
{
    public interface IConnectionFactory
    {
        SqlConnection GetConnection();
    }
}