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
                SetConsoleColor(log.Action);
                Console.WriteLine($"[Audit] {log.Entity} - {log.Action} by {log.UserId}");
                Console.ResetColor();
            }
        }
        private static void SetConsoleColor(string action)
        {
            switch (action.ToLower())
            {
                case "added":
                case "create":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "modified":
                case "update":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case "deleted":
                case "delete":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
            }
        }
    }
}
