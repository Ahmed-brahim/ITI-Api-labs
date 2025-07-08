using lab2._1.Models;
using lab2._1.Repository;

namespace lab2._1.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ITIDbContext _context;

        public IGenericRepository<Student> Students { get; private set; }
        public IGenericRepository<Department> Departments { get; private set; }
        public IGenericRepository<Instructor> Instructors { get; private set; }

        public UnitOfWork(ITIDbContext context)
        {
            _context = context;
            Students = new GenericRepository<Student>(_context);
            Departments = new GenericRepository<Department>(_context);
            Instructors = new GenericRepository<Instructor>(_context);
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
