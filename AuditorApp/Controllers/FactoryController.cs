using AuditorApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace AuditorApp.Controllers
{
    public class FactoryController : Controller
    {
        public IActionResult Index()
        {
            List<Factory> factories = new List<Factory>();
            SqlConnection conn = Db.Conn();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Factories WHERE IsDeleted = 0", conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read()) 
            { 
               factories.Add(new Factory {
                   Id = (int)dr["Id"], 
                   Name = (string)dr["Name"],
                   Adress = (string)dr["Adress"],
                   CreatedDate = (DateTime)dr["CreatedDate"]
               });
                dr.Close();
                conn.Close();
                
            }
            return View(factories);
        }
    }
}
