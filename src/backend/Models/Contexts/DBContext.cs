using MySql.Data.MySqlClient;
namespace FRA_Todolist_prj.Models.Contexts
{
    public class DBContext : BaseContext
    {
        public DBContext(string connectionString) : base(connectionString) { }
        public new MySqlConnection GetConnection()
        {
            return new MySqlConnection(_baseConnectionString);
        }
    }
    
    public class BaseContext
    {
        protected readonly string _baseConnectionString;
        protected BaseContext(string connectionString)
        {
            _baseConnectionString = connectionString;
        }
        protected MySqlConnection GetConnection()
        {
            return new MySqlConnection(_baseConnectionString);
        }
    }
}