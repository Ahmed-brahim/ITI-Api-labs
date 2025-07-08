using lab2._1.Dtos;
using lab2._1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using global::lab2._1.UnitOfWork;

namespace lab2._1.Controllers
{
    namespace lab2._1.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class DepartmentController : ControllerBase
        {
            private readonly IUnitOfWork _unitOfWork;

            public DepartmentController(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            //GET all departments
            [HttpGet]
            public async Task<IActionResult> GetAll()
            {
                var departments = await _unitOfWork.Departments.GetAllAsync();

                var result = departments.Select(d => new DepartmentDto
                {
                    Name = d.DeptName,
                    StudentsCount = d.Students?.Count ?? 0
                });

                return Ok(result);
            }

            // ✅ GET by ID
            [HttpGet("{id}")]
            public async Task<IActionResult> GetById(int id)
            {
                var dept = await _unitOfWork.Departments
                    .GetByIdWithIncludesAsync(id, d => d.Students); 
                if (dept == null)
                    return NotFound();

                return Ok(new DepartmentDto
                {
                    Name = dept.DeptName,
                    StudentsCount = dept.Students?.Count ?? 0
                });
            }

            //  POST (Add)
            [HttpPost]
            [Consumes("application/json")]
            [Produces("application/json")]
            public async Task<IActionResult> Add([FromBody] InputDepartmentDto input, int id)
            {
                var department = new Department
                {
                    DeptName = input.Name,
                    DeptDesc = input.Description,
                    DeptLocation = input.Location,
                    DeptManager = input.ManagerId,
                    DeptId = id, // Assuming the ID is passed in the URL 
                };

                await _unitOfWork.Departments.AddAsync(department);
                await _unitOfWork.SaveAsync();

                return Ok(new DepartmentDto
                {
                    Name = department.DeptName,
                    StudentsCount = 0
                });
            }

            //  PUT (Update)
            [HttpPut("{id}")]
            [Consumes("application/json")]
            [Produces("application/json")]
            public async Task<IActionResult> Update(int id, [FromBody] InputDepartmentDto input)
            {
                var dept = await _unitOfWork.Departments
                    .GetByIdWithIncludesAsync(id, d => d.Students);

                if (dept == null)
                    return NotFound();

                dept.DeptName = input.Name;
                dept.DeptDesc = input.Description;
                dept.DeptLocation = input.Location;
                dept.DeptManager = input.ManagerId;

                _unitOfWork.Departments.Update(dept);
                await _unitOfWork.SaveAsync();

                return Ok(new DepartmentDto
                {
                    Name = dept.DeptName,
                    StudentsCount = dept.Students?.Count ?? 0
                });
            }

            //DELETE
            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(int id)
            {
                var dept = await _unitOfWork.Departments.GetByIdAsync(id);
                if (dept == null)
                    return NotFound();

                _unitOfWork.Departments.Delete(dept);
                await _unitOfWork.SaveAsync();

                return NoContent();
            }
        }
    }


}
