using lab2._1.Models;
using lab2._1.Repository;

namespace lab2._1.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Student> Students { get; }
        IGenericRepository<Department> Departments { get; }
        IGenericRepository<Instructor> Instructors { get; }

        Task<int> SaveAsync();
    }
}
