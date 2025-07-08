using System.ComponentModel.DataAnnotations;

namespace lab2._1.Dtos
{
    public class InputDepartmentDto
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public int? ManagerId { get; set; }
        public static int Id = 100;
    }
}
