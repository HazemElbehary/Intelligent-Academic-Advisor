using FCAI.Persistence.Data;

namespace FCAI.APIs.Extension
{
    public static class InitializerExtensions
    {
        public async static Task<WebApplication> InitializeApplicationContext(this WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();
            var services = scope.ServiceProvider;

            var applicationInitializer = services.GetRequiredService<ApplicationInitializer>();
            var Logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                await applicationInitializer.SeedAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Sorry, An Error Occured");
            }

            return app;
        }
    }
}
