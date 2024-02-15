using AuditorApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using AuditorApp.Models;

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

            }
            dr.Close();
            conn.Close();
            return View(factories);
        }
        public IActionResult Delete (int id)
        {
            SqlConnection conn = Db.Conn();
            SqlCommand cmd = new SqlCommand("DELETE FROM Factories WHERE Id=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
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
        public IActionResult Add(Factory factory)
        {
            SqlConnection conn = Db.Conn();
            SqlCommand cmd = new SqlCommand("INSERT INTO Factories (Name, Adress) OUTPUT inserted.Id VALUES (@name, @adress)", conn);
            cmd.Parameters.AddWithValue("@name", factory.Name);
            cmd.Parameters.AddWithValue("@adress", factory.Adress);
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
;        }
        public IActionResult Update(int id)
        {
            Factory factory = new Factory();
            SqlConnection conn = Db.Conn();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Factories WHERE Id=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();

            factory.Id = (int)dr["Id"];
            factory.Name = (string)dr["Name"];
            factory.Adress = (string)dr["Adress"];
            factory.CreatedDate = (DateTime)dr["CreatedDate"];
            dr.Close();
            conn.Close();
            return View(factory);

        }
        [HttpPost]
        public IActionResult Update(Factory factory) 
        {
            SqlConnection conn = Db.Conn();
            SqlCommand cmd = new SqlCommand("UPDATE Factories SET Name=@name, Adress=@adress WHERE Id=@id", conn);
            cmd.Parameters.AddWithValue("@name", factory.Name);
            cmd.Parameters.AddWithValue("@id", factory.Id);
            cmd.Parameters.AddWithValue("@adress", factory.Adress);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            TempData["success"] = "Güncellendi.";
            return RedirectToAction("Index");
        }
    }
}
