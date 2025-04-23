# Practical 20: Global exception handling using middleware, audit logs into database, save logs into files using serilog, implement Unit of Work pattern

This project demonstrates the implementation of authorizing and controlling access and view of functions based on the role of the accessing user.

## Configuration

Update the connection string in `appsettings.json` according to your SQL Server setup:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=Practical19;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

## Applying Migrations

Since migration files are already included in the project, you only need to apply them to create the database structure:

1. Open the Package Manager Console (Tools > NuGet Package Manager > Package Manager Console)
2. Run the following command:
   ```
   Update-Database
   ```
