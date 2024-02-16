using AuditorApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace AuditorApp.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            List<EmployeeVM> list = new List<EmployeeVM>();
            SqlConnection conn = Db.Conn();
            SqlCommand cmd = new SqlCommand("SELECT e.Id, e.Name,e.LastName, e.Point, f.Name AS FactoryName, a.Name AS AuditorName, a.LastName AS AuditorLastName FROM Employees e JOIN Factories f ON e.FactoryId = f.Id JOIN Auditors a ON e.AuditorId = a.Id", conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new EmployeeVM
                {
                    Id = (int)dr["Id"],
                    Name = (string)dr["Name"],
                    LastName = (string)dr["LastName"],
                    Point = (int)dr["Point"],
                    FactoryName = (string)dr["FactoryName"],
                    AuditorName = (string)dr["FactoryName"],
                    AuditorLastName = (string)dr["AuditorLastName"]
                });
            }
            return View(list);
        }
    }
}
