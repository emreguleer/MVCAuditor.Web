using AuditorApp.Models;
using AuditorApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            conn.Close();

            return View(list);
        }
        public IActionResult Add() 
        {
            List<SelectListItem> factories = new List<SelectListItem>();
            SqlConnection conn = Db.Conn();
            SqlCommand cmd = new SqlCommand("SELECT* FROM Factories WHERE IsDeleted = 0", conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                factories.Add(new SelectListItem
                {
                    Value = dr["Id"].ToString(),
                    Text = dr["Name"].ToString(),
                });
            }
            dr.Close();


            List<SelectListItem> nameAuditors = new List<SelectListItem>();
            List<SelectListItem> lastNameAuditors = new List<SelectListItem>();
            SqlCommand cmdAuditor = new SqlCommand("SELECT * FROM Auditors WHERE IsDeleted = 0", conn);
            SqlDataReader drAuditor = cmdAuditor.ExecuteReader();
            while (drAuditor.Read())
            {
                nameAuditors.Add(new SelectListItem{
                    Value = dr["Id"].ToString(),
                    Text = dr["Name"].ToString(),
                });
            }
            dr.Close();
            while (drAuditor.Read())
            {
                lastNameAuditors.Add(new SelectListItem{
                    Value = dr["Id"].ToString(),
                    Text = dr["LastName"].ToString(),
                });
            }
            dr.Close();


            List<SelectListItem> departmants = new List<SelectListItem>();
            SqlCommand cmdDepartmant = new SqlCommand("SELECT * FROM Departmants WHERE IsDeleted = 0", conn);
            SqlDataReader drDepartmant = cmdDepartmant.ExecuteReader();
            while (drDepartmant.Read())
            {
                departmants.Add(new SelectListItem{ 
                    Value = dr["Id"].ToString(),
                    Text = dr["Name"].ToString(),
                });
            }
            conn.Close();

            ViewData["factories"] = factories;
            ViewData["nameAuditors"] = nameAuditors;
            ViewData["lastNameAuditors"] = lastNameAuditors;
            ViewData["departmants"] = departmants;
            return View();

        }
        [HttpPost]
        public IActionResult Add(Employee employee, List<int> departmants)
        {
            SqlConnection conn = Db.Conn();
            SqlCommand cmd = new SqlCommand("INSERT INTO Employees (Name, LastName, Point, FactoryId, AuditorId) OUTPUT inserted.id VALUES (@name, @lastName, @point, @factoryId, @auditorId)", conn);
            cmd.Parameters.AddWithValue("@name", employee.Name);
            cmd.Parameters.AddWithValue("@lastName", employee.LastName);
            cmd.Parameters.AddWithValue("@point", employee.Point);
            cmd.Parameters.AddWithValue("@factoryId", employee.FactoryId);
            cmd.Parameters.AddWithValue("@auditorId", employee.AuditorId);
            conn.Open();
            int lastId = (int)cmd.ExecuteScalar();
            foreach (int i in departmants)
            {
                cmd = new SqlCommand("INSERT INTO EmployeeDepartmantRel VALUES (@employeeId, @departmantId", conn);
                cmd.Parameters.AddWithValue("@employeeId", lastId);
                cmd.Parameters.AddWithValue("@departmantId", i);
                cmd.ExecuteNonQuery();
            }
            conn.Close();

            return RedirectToAction("Index");
        }
    }
}
