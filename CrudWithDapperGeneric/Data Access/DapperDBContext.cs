using System.Data;
using System.Data.SqlClient;

namespace CrudWithDapperGeneric.Data_Access
{
    public class DapperDBContext
    {

        private readonly string _connectionString;

        public DapperDBContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);
    }
}
