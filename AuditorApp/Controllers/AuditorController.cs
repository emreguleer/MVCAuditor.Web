using AuditorApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace AuditorApp.Controllers
{
    public class AuditorController : Controller
    {
        public IActionResult Index()
        {
            List<Auditor> auditors = new List<Auditor>();
            SqlConnection conn = Db.Conn();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Auditors WHERE IsDelete = 0", conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                auditors.Add(new Auditor
                {
                    Id = (int)dr["Id"],
                    Name = (string)dr["Name"],
                    CreatedDate= (DateTime)dr["CreatedDate"]
                });
            }
            dr.Close();
            conn.Close();
            return View(auditors);
        }
        public IActionResult Delete (int id)
        {
            SqlConnection conn = Db.Conn();
            SqlCommand cmd = new SqlCommand("DELETE FROM Auditors WHERE @id=Id", conn);
            cmd.Parameters.AddWithValue("id", id);
            conn.Open();
            int affectedRows = cmd.ExecuteNonQuery();
            conn.Close();
            if (affectedRows > 0)
            {
                TempData["success"] = "Silme işlemi başarılı !";
            }
            else
            {
                TempData["error"] = "Silme işlemi başarısız.";
            }
            return RedirectToAction("Index");

        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(Auditor auditor)
        {
            SqlConnection conn = Db.Conn();
            SqlCommand cmd = new SqlCommand("INSERT INTO Auditors OUTPUT inserted.Id VALUES @name", conn);
            cmd.Parameters.AddWithValue("@name", auditor.Name);
            conn.Open();
            int id = (int)cmd.ExecuteScalar();
            conn.Close();
            if (id != 0)
            {
                TempData["success"] = "Başarılı !";
            }
            else
            {
                TempData["error"] = "Başarısız !";
            }
            return RedirectToAction("Index");
        }

            
    }
}
