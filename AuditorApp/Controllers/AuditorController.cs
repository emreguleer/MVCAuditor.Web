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
            SqlCommand cmd = new SqlCommand("SELECT * FROM Auditors WHERE IsDeleted = 0", conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                auditors.Add(new Auditor
                {
                    Id = (int)dr["Id"],
                    Name = (string)dr["Name"],
                    LastName = (string)dr["LastName"],
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
            SqlCommand cmd = new SqlCommand("INSERT INTO Auditors (Name, LastName) OUTPUT inserted.Id VALUES (@name, @lastName)", conn);
            cmd.Parameters.AddWithValue("@name", auditor.Name);
            cmd.Parameters.AddWithValue("@lastName", auditor.LastName);
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
        public IActionResult Update(int id)
        {
            Auditor auditor = new Auditor();
            SqlConnection conn = Db.Conn();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Auditors WHERE Id=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();

            auditor.Id = (int)dr["Id"];
            auditor.Name = (string)dr["Name"];
            auditor.LastName = (string)dr["LastName"];
            auditor.CreatedDate = (DateTime)dr["CreatedDate"];
            dr.Close();
            conn.Close();
            return View(auditor);

        }
        [HttpPost]
        public IActionResult Update(Auditor auditor)
        {
            SqlConnection conn = Db.Conn();
            SqlCommand cmd = new SqlCommand("UPDATE Auditors SET Name=@name, LastName=@LastName WHERE Id=@id", conn);
            cmd.Parameters.AddWithValue("@name", auditor.Name);
            cmd.Parameters.AddWithValue("@lastName", auditor.LastName);
            cmd.Parameters.AddWithValue("@id", auditor.Id);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            TempData["success"] = "Güncellendi.";
            return RedirectToAction("Index");
        }


    }
}
