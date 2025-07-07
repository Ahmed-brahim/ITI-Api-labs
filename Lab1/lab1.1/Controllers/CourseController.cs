using lab1._1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lab1._1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CourseController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var courses = _context.Courses.ToList();
            if (!courses.Any()) return NotFound();
            return Ok(courses);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var course = _context.Courses.Find(id);
            if (course == null) return NotFound();
            return Ok(course);
        }

        [HttpGet("byname/{name:alpha}")]
        public IActionResult CouseByName(string name)
        {
            var course = _context.Courses.FirstOrDefault(c => c.Crs_Name == name);
            if (course == null) return NotFound();
            return Ok(course);
        }

        [HttpPost]
        public IActionResult Post(Course course)
        {
            if (course == null || course.Crs_Description.Length > 150 || course.Crs_Name.Length > 50 || course.Duration > 100 || course.Duration < 1)
                return BadRequest();
            _context.Courses.Add(course);
            _context.SaveChanges();
            return StatusCode(201);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Course course)
        {
            if (id != course.Id) return BadRequest();
            var existing = _context.Courses.Find(id);
            if (existing == null) return NotFound();

            existing.Crs_Name = course.Crs_Name;
            existing.Duration = course.Duration;
            existing.Crs_Description = course.Crs_Description;
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCourse(int id)
        {
            var course = _context.Courses.Find(id);
            if (course == null) return NotFound();
            _context.Courses.Remove(course);
            _context.SaveChanges();
            return Ok(_context.Courses.ToList());
        }
    }
}
