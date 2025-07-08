using AutoMapper;
using lab2._1.Dtos;
using lab2._1.Models;
using lab2._1.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab2._1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StudentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //GET all students with pagination & search
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10, string? search = "")
        {
            var query = await _unitOfWork.Students
                .GetAllAsync(); 

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.StFname.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                              s.StLname.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            var students =  query
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();
                

            var result = _mapper.Map<List<StudentDto>>(students);
            return Ok(result);
        }

        //  GET by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _unitOfWork.Students
                .GetByIdAsync(id);

            if (student == null)
                return NotFound();

            var dto = _mapper.Map<StudentDto>(student);
            return Ok(dto);
        }

        //  ADD new student
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Add([FromBody] InputStudentDto inputDto, int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var student = _mapper.Map<Student>(inputDto);
            student.DeptId = id; // Assuming the department ID is passed in the URL
            await _unitOfWork.Students.AddAsync(student); 
            await _unitOfWork.SaveAsync();

            // Map back to DTO after insertion
            var studentWithIncludes = await _unitOfWork.Students.GetByIdAsync(student.StId);

            var resultDto = _mapper.Map<StudentDto>(studentWithIncludes);
            return Ok(resultDto);
        }

        //  UPDATE existing student
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Update(int id, [FromBody] InputStudentDto inputDto)
        {
            var existingStudent = await _unitOfWork.Students.GetByIdAsync(id);
            if (existingStudent == null)
                return NotFound();

            _mapper.Map(inputDto, existingStudent);

            _unitOfWork.Students.Update(existingStudent); 
            await _unitOfWork.SaveAsync();

            var studentWithIncludes = await _unitOfWork.Students.GetByIdAsync(id);

            var resultDto = _mapper.Map<StudentDto>(studentWithIncludes);
            return Ok(resultDto);
        }

        // ✅ DELETE student
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(id);
            if (student == null)
                return NotFound();

            _unitOfWork.Students.Delete(student); 
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
