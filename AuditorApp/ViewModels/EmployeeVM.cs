using AuditorApp.Models;

namespace AuditorApp.ViewModels
{
    public class EmployeeVM
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Point { get; set; }
        public string FactoryName { get; set; }
        public string AuditorName { get; set; }
        public string AuditorLastName { get; set; }
        public List<Departmant> Departmants { get; set; } = new List<Departmant>();

    }
}
