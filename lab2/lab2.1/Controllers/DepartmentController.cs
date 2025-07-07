using lab2._1.Dtos;
using lab2._1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab2._1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly ITIDbContext _context;

        public DepartmentController(ITIDbContext context)
        {
            _context = context;
        }

        //GET all departments
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var departments = await _context.Departments
                .Include(d => d.Students)
                .Select(d => new DepartmentDto
                {
                    Name = d.DeptName,
                    StudentsCount = d.Students.Count
                })
                .ToListAsync();

            return Ok(departments);
        }

        // GET by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dept = await _context.Departments
                .Include(d => d.Students)
                .FirstOrDefaultAsync(d => d.DeptId == id);

            if (dept == null)
                return NotFound();

            return Ok(new DepartmentDto
            {
                Name = dept.DeptName,
                StudentsCount = dept.Students.Count
            });
        }

        //POST (Add new department)
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Add([FromBody] InputDepartmentDto input)
        {
            var department = new Department
            {
                DeptName = input.Name,
                DeptDesc = input.Description,
                DeptLocation = input.Location,
                DeptManager = input.ManagerId,
                DeptId = InputDepartmentDto.Id++ 
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            // Return output DTO
            return Ok(new DepartmentDto
            {
                Name = department.DeptName,
                StudentsCount = 0
            });
        }

        //PUT (Update)
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Update(int id, [FromBody] InputDepartmentDto input)
        {
            var dept = await _context.Departments
                .Include(d => d.Students)
                .FirstOrDefaultAsync(d => d.DeptId == id);

            if (dept == null)
                return NotFound();

            dept.DeptName = input.Name;
            dept.DeptDesc = input.Description;
            dept.DeptLocation = input.Location;
            dept.DeptManager = input.ManagerId;

            await _context.SaveChangesAsync();

            return Ok(new DepartmentDto
            {
                Name = dept.DeptName,
                StudentsCount = dept.Students.Count
            });
        }

        //DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept == null)
                return NotFound();

            _context.Departments.Remove(dept);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
