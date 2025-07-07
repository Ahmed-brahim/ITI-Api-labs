using System.ComponentModel.DataAnnotations;

namespace lab2._1.Dtos
{
    public class InputStudentDto
    {
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required.")]
        public string? LastName { get; set; }
        public string? Address { get; set; }
        [Range(6, 40, ErrorMessage = "Age must be between 1 and 120.")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Department ID is required.")]
        public int? DepartmentId { get; set; }
        public int? SupervisorId { get; set; }
    }
}
