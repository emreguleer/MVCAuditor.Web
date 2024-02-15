using AuditorApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace AuditorApp.Controllers
{
    public class DepartmantController : Controller
    {
        public IActionResult Index()
        {
            List<Departmant> departmants = new List<Departmant>();

            SqlConnection conn = Db.Conn();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Departmants WHERE IsDeleted=0", conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                departmants.Add(new Departmant 
                { 
                    Id = (int)dr["Id"], 
                    Name = (string)dr["Name"],
                    CreatedDate = (DateTime)dr["CreatedDate"] 
                });

            }
            conn.Close();

            return View(departmants);
        }
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Departmant departmant)
        {
            SqlConnection con = Db.Conn();
            SqlCommand cmd = new SqlCommand("INSERT INTO Departmants (Name) OUTPUT inserted.Id values(@name)", con);
            cmd.Parameters.AddWithValue("@name", departmant.Name);
            int sonId = 0;
            try
            {
                con.Open();
                sonId = (int)cmd.ExecuteScalar();
                TempData["success"] = "Başarılı";

            }
            catch
            {
                TempData["error"] = "Başarısız";
            }
            finally
            {
                con.Close();

            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            SqlConnection conn = Db.Conn();
            SqlCommand cmd = new SqlCommand("DELETE FROM Departmants WHERE Id=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            int affectedRows = cmd.ExecuteNonQuery();
            conn.Close();

            if (affectedRows > 0)
            {
                TempData["success"] = "Etiket başarıyla silinmiştir !";

            }
            else
            {
                TempData["error"] = "HATA ! Silme işlemi başarısız !";
            }



            return RedirectToAction("Index");
        }
        public IActionResult Update() 
        {
           return View();
        }

        [HttpPost]
        public IActionResult Update(Departmant departmant)
        {
            SqlConnection conn = Db.Conn();
            SqlCommand cmd = new SqlCommand("UPDATE TDepartmants SET Name=@name WHERE Id=@id", conn);
            cmd.Parameters.AddWithValue("@name", departmant.Name);
            cmd.Parameters.AddWithValue("@id", departmant.Id);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            TempData["success"] = "GÜNCELLEME Başarılı !";

            return RedirectToAction("Index");
        }
    }
}
