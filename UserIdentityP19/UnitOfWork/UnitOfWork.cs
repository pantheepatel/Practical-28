using ExceptionLogger;
using ExceptionLogger.Repository.AuthRepo;
using ExceptionLogger.Repository.StudentRepo;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ExceptionLogger.Models;
using CSharp13Features.Utility;
using CSharp13Features.Models;

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

        public async Task CompleteAsync()
        {
            var entries = _context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            var userId = _httpContextAccessor.HttpContext.User?.Identity?.Name;

            // this will store logs
            var logsList = new List<ConsoleLogModel>();

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
                logsList.Add(new ConsoleLogModel { UserId = auditLog.UserId, Entity = auditLog.Entity, Action = entry.State.ToString() });
                await _context.AuditLogs.AddAsync(auditLog);
            }

            AuditHelper.LogAuditActions(logsList);

            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
