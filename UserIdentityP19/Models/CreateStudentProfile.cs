namespace ExceptionLogger.Models
{
    public class CreateStudentProfile
    {
        public string Name { get; set; } = string.Empty;
        public string Course { get; set; } = string.Empty;
        public string EnrollmentNo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime AdmissionDate { get; set; } = DateTime.Now;
    }
}
