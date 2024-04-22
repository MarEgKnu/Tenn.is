namespace Tennis.Services
{
    public class Connection
    {
        protected string connectionString { get; set; }
        protected virtual string MyConnection { set { connectionString = Secret.ConnectionString; } }
    }
}
