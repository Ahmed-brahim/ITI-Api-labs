using AutoMapper;
using lab2._1.Dtos;
using lab2._1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab2._1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ITIDbContext _context;
        private readonly IMapper _mapper;

        public StudentController(ITIDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //GET all with pagination & search
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10, string? search = "")
        {
            var query = _context.Students
                .Include(s => s.Dept)
                .Include(s => s.StSuperNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s =>
                    s.StFname!.Contains(search) ||
                    s.StLname!.Contains(search));
            }

            var students = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = _mapper.Map<List<StudentDto>>(students);
            return Ok(result);
        }

        //GET by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _context.Students
                .Include(s => s.Dept)
                .Include(s => s.StSuperNavigation)
                .FirstOrDefaultAsync(s => s.StId == id);

            if (student == null)
                return NotFound();

            var dto = _mapper.Map<StudentDto>(student);
            return Ok(dto);
        }

        // ADD
        [HttpPost]
        [Consumes("application/json")] //MAKE SURE TO USE THIS FOR JSON INPUT
        [Produces("application/json")] //MAKE SURE TO USE THIS FOR JSON OUTPUT
        public async Task<IActionResult> Add([FromBody] InputStudentDto inputDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var student = _mapper.Map<Student>(inputDto);

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            // Map back to DTO after insertion
            var studentWithIncludes = await _context.Students
                .Include(s => s.Dept)
                .Include(s => s.StSuperNavigation)
                .FirstOrDefaultAsync(s => s.StId == student.StId);

            var resultDto = _mapper.Map<StudentDto>(studentWithIncludes);
            return Ok(resultDto);
        }

        // uPDATE
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Update(int id, [FromBody] InputStudentDto inputDto)
        {
            var existingStudent = await _context.Students.FindAsync(id);
            if (existingStudent == null)
                return NotFound();

            _mapper.Map(inputDto, existingStudent);

            _context.Entry(existingStudent).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // return updated DTO
            var studentWithIncludes = await _context.Students
                .Include(s => s.Dept)
                .Include(s => s.StSuperNavigation)
                .FirstOrDefaultAsync(s => s.StId == id);

            var resultDto = _mapper.Map<StudentDto>(studentWithIncludes);
            return Ok(resultDto);
        }

        //DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return NotFound();

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
