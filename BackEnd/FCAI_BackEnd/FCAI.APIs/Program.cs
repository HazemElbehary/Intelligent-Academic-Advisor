using FCAI.APIs.Extension;
using FCAI.Application.Abstraction.Exceptions;

namespace FCAI.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services
            // Add services to the container.
            builder.AddAllServices();

            var app = builder.Build();
            #endregion
            
            #region Update DB
            // Update DB automatically
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            try
            {
                await app.MigrateDatabaseAsync();
                logger.LogInformation("Database migration completed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the database.");
                throw new ApiExceptionResponse(400, "DB Error has been occured", string.Empty);
            }

            #endregion

            #region Pipelines
            await app.ConfigAppAsync();

            // check if the connection string is set
            var connectionString = builder.Configuration.GetConnectionString("ApplicationConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ApiExceptionResponse(500, "Database connection string is not configured.");
            }

            app.MapGet("/", () => "API is running...");
            #endregion

            app.Run();
        }
    }
}
