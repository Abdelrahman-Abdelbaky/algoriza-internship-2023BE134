namespace VezeetaProject.Core.Models
{
    public class Specialization
    {
        public int Id { get; set; } 
        public string SpecializaAr { get; set; }
        public string SpecializaEn { get; set; }
        public List<Doctor> Dotors { get; set; }
    
    }
}
