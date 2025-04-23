using AutoMapper;
using ExceptionLogger.Repository.AuthRepo;
using ExceptionLogger.Repository.StudentRepo;

namespace LogException.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IStudentRepository Students { get; }
        IUserRepository Users { get; }
        IMapper mapper { get; }
        Task CompleteAsync();
    }
}
