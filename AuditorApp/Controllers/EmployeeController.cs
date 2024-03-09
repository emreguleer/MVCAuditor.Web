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
                    AuditorName = (string)dr["AuditorName"],
                    AuditorLastName = (string)dr["AuditorLastName"],
                    Departmants = GetDepartmantsByEmployeeId((int)dr["Id"])


                });
            }
            conn.Close();

            return View(list);
        }
        public IActionResult Add() 
        {
            ViewData["factories"] = GetAllFactories();
            ViewData["nameAuditors"] = GetAllAuditors();
            ViewData["departmants"] = GetAllDepartmants();

            return View();

        }
        [HttpPost]
        public IActionResult Add(Employee employee, List<int> departmants)
        {
            SqlConnection conn = Db.Conn();
            SqlCommand cmd = new SqlCommand("INSERT INTO Employees (Name, LastName, Point, FactoryId, AuditorId) OUTPUT inserted.Id VALUES (@name, @lastName, @point, @factoryId, @auditorId)", conn);
            cmd.Parameters.AddWithValue("@name", employee.Name);
            cmd.Parameters.AddWithValue("@lastName", employee.LastName);
            cmd.Parameters.AddWithValue("@point", employee.Point);
            cmd.Parameters.AddWithValue("@factoryId", employee.FactoryId);
            cmd.Parameters.AddWithValue("@auditorId", employee.AuditorId);
            conn.Open();
            int lastId = (int)cmd.ExecuteScalar();
            foreach (int i in departmants)
            {
                cmd = new SqlCommand("INSERT INTO EmployeeDepartmantRel VALUES (@employeeId,@departmantId)", conn);
                cmd.Parameters.AddWithValue("@employeeId", lastId);
                cmd.Parameters.AddWithValue("@departmantId", i);
                cmd.ExecuteNonQuery();
            }
            conn.Close();

            return RedirectToAction("Index");
        }
        public List<Departmant> GetDepartmantsByEmployeeId(int employeeId)
        {
            List<Departmant> departmants = new List<Departmant>();
            SqlConnection conn = Db.Conn();
            SqlCommand cmd = new SqlCommand("SELECT d.Id, d.Name FROM Departmants d JOIN EmployeeDepartmantRel edr ON d.Id=edr.DepartmantId AND edr.EmployeeId=@employeeId", conn);
            cmd.Parameters.AddWithValue("@employeeId", employeeId);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                departmants.Add(new Departmant
                {
                    Id = (int)dr["Id"],
                    Name = (string)dr["Name"]
                });
            }
            conn.Close();
            return departmants;
        }
        public IActionResult Update(int id)
        {
            SqlConnection conn = Db.Conn();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Employees WHERE Id=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            List<Departmant> departmants = GetDepartmantsByEmployeeId(id);
            Employee employee = new Employee { CreatedDate = (DateTime)dr["CreatedDate"], Name = (string)dr["name"], LastName = (string)dr["LastName"], Point = (int)dr["Point"], Id = id, AuditorId = (int)dr["AuditorId"], FactoryId = (int)dr["FactoryId"], Departmants = departmants };
            conn.Close();

            List<SelectListItem> allDepartmants = GetAllDepartmants();
            List<SelectListItem> factories = GetAllDepartmants();
            List<SelectListItem> auditors = GetAllDepartmants();

            ViewData["factories"] = factories;
            ViewData["nameAuditors"] = auditors;
            ViewData["departmants"] = allDepartmants;
            return View();
        }
        public List<SelectListItem> GetAllDepartmants() 
        {
            SqlConnection conn = Db.Conn();
            List<SelectListItem> departmants = new List<SelectListItem>();
            SqlCommand cmdDepartmant = new SqlCommand("SELECT * FROM Departmants WHERE IsDeleted = 0", conn);
            conn.Open();
            SqlDataReader drDepartmant = cmdDepartmant.ExecuteReader();
            while (drDepartmant.Read())
            {
                departmants.Add(new SelectListItem
                {
                    Value = drDepartmant["Id"].ToString(),
                    Text = drDepartmant["Name"].ToString(),
                });
            }
            conn.Close();

            return departmants;
        }
        public List<SelectListItem> GetAllFactories()
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
            conn.Close();
            dr.Close();
            return factories;
        }
        public List<SelectListItem> GetAllAuditors()
        {
            List<SelectListItem> auditors = new List<SelectListItem>();
            SqlConnection conn = Db.Conn();
            SqlCommand cmdAuditor = new SqlCommand("SELECT * FROM Auditors WHERE IsDeleted = 0", conn);
            conn.Open();
            SqlDataReader drAuditor = cmdAuditor.ExecuteReader();
            while (drAuditor.Read())
            {
                auditors.Add(new SelectListItem
                {
                    Value = drAuditor["Id"].ToString(),
                    Text = drAuditor["Name"].ToString(),
                });
            }
            drAuditor.Close();
            conn.Close();
            return auditors;
        }
    }
}
