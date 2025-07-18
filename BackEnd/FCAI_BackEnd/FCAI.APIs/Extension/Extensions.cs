using FCAI.APIs.Middlewares;
using FCAI.Domain.Entities;
using FCAI.Persistence.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FCAI.APIs.Extension
{
    public static class Extensions
    {
        public static async Task ConfigAppAsync(this WebApplication app)
        {
            await app.InitializeApplicationContext();
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("AllowDevAndProd");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
        }

        public static async Task MigrateDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Check for pending migrations
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                await dbContext.Database.MigrateAsync();
            }
        }


        // Authentication + JWT Bearer + Configure Identity settings
        public static IServiceCollection AddAuthAndJWTBearer(this IServiceCollection service, IConfiguration configuration)
        {
            service = ConfigureIdentitySettings(service);

            // Configure JWT Bearer authentication
            service.AddAuthentication(Options =>
            {
                Options.DefaultAuthenticateScheme = "Bearer";
                Options.DefaultChallengeScheme = "Bearer";
            })
            .AddJwtBearer((configurationOptions) =>
            {
                configurationOptions.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,

                    ClockSkew = TimeSpan.FromMinutes(0),
                    ValidIssuer = configuration["JwtSettings:Issure"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!))
                };
            });
            return service;
        }

        private static IServiceCollection ConfigureIdentitySettings(IServiceCollection service)
        {
            // Configure Identity settings
            service.AddIdentity<Student, IdentityRole>(IdentityOptions =>
            {
                IdentityOptions.SignIn.RequireConfirmedEmail = false;
                IdentityOptions.SignIn.RequireConfirmedPhoneNumber = false;
                IdentityOptions.SignIn.RequireConfirmedAccount = false;
                IdentityOptions.Lockout.AllowedForNewUsers = true;
                IdentityOptions.Lockout.MaxFailedAccessAttempts = 10;
                IdentityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddSignInManager();
            return service;
        }
    }
}