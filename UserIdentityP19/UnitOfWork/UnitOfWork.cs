using ExceptionLogger;
using ExceptionLogger.Repository.AuthRepo;
using ExceptionLogger.Repository.StudentRepo;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ExceptionLogger.Models;

namespace LogException.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IStudentRepository Students { get; }
        public IUserRepository Users { get; }
        public IMapper mapper { get; }

        public UnitOfWork(AppDbContext context,
                          IStudentRepository studentRepository,
                          IUserRepository userRepository,
                          IMapper _mapper,
                          IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            Students = studentRepository;
            Users = userRepository;
            mapper = _mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> CompleteAsync()
        {
            var entries = _context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            var result = await _context.SaveChangesAsync();

            var userId = _httpContextAccessor.HttpContext.User?.Identity?.Name;

            foreach (var entry in entries)
            {
                var auditLog = new AuditLog
                {
                    UserId = userId ?? "System",
                    Action = entry.State.ToString(),
                    Entity = entry.Entity.GetType().Name,
                    EntityId = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey())?.CurrentValue?.ToString(),
                    Timestamp = DateTime.UtcNow,
                    IPAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString()
                };

                await _context.AuditLogs.AddAsync(auditLog);
            }

            await _context.SaveChangesAsync();
            return result;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
