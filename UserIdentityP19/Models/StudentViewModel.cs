using System.ComponentModel.DataAnnotations;

namespace ExceptionLogger.Models
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Course { get; set; }
        public string? Email { get; set; }
        public string? EnrollmentNo { get; set; }
        public DateTime AdmissionDate { get; set; }
    }
}
