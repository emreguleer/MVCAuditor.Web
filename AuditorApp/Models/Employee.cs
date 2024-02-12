namespace AuditorApp.Models
{
    public class Employee : BaseModel
    {
        public int Point { get; set; }
        public int FactoryId { get; set; }
        public int AuditorId { get; set; }
        public virtual ICollection<Departmant> Departmants { get; set; } = new List<Departmant>();
    }
}
