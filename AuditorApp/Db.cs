using System.Data.SqlClient;

namespace AuditorApp
{
    public class Db
    {
        public static SqlConnection Conn()
        {
            return new SqlConnection("Server=DESKTOP-17L4C0E\\SQLEXPRESS;Database=AuditorApp;Integrated Security=True; TrustServerCertificate=Yes");
        }
    }
}
