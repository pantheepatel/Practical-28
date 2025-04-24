using CSharp13Features.Models;
using ExceptionLogger.Models;

namespace CSharp13Features.Utility
{
    public static class AuditHelper
    {
        public static void LogAuditActions(params List<ConsoleLogModel> logs)
        {
            foreach (var log in logs)
            {
                var colorcode = SetConsoleColor(log.Action);
                Console.WriteLine($"{colorcode}{log.Entity} - {log.Action} by {log.UserId}");
                Console.ResetColor();
            }
        }
        private static string SetConsoleColor(string action)
        {
            return action.ToLower() switch
            {
                "added" or "create" => "\e[32m", // Green
                "modified" or "update" => "\e[33m", // Yellow
                "deleted" or "delete" => "\e[31m", // Red
                _ => "\e[37m" // Gray
            };
        }
    }
}
