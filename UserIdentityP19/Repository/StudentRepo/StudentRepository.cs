using Microsoft.EntityFrameworkCore;
using ExceptionLogger.Models;

namespace ExceptionLogger.Repository.StudentRepo
{
    public class StudentRepository(AppDbContext _context) : IStudentRepository
    {
        public async Task AddAsync(Student student)
        {
            _context.Students.Add(student);
        }

        public async Task DeleteAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> GetByIdAsync(int id)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
        }

        public Task UpdateAsync(Student student)
        {
            _context.Students.Update(student);
            return Task.CompletedTask;
        }
    }
}
