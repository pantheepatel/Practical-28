using System.ComponentModel.DataAnnotations;

namespace ExceptionLogger.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string EnrollmentNo { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Course { get; set; }

        [DataType(DataType.Date)]
        public DateTime AdmissionDate { get; set; }

    }
}
