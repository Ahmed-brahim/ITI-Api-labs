using System.ComponentModel.DataAnnotations;

namespace lab1._1.Models
{
    public class Course
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string? Crs_Name { get; set; }
        [MaxLength(150)]
        public string? Crs_Description { get; set; }
        [Range(1, 100, ErrorMessage = "Duration must be between 1 and 100 hours.")]
        public int? Duration { get; set; }
    }
}
