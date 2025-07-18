using FCAI.Application.Abstraction.Exceptions;
using Microsoft.AspNetCore.Mvc;
using FCAI.Persistence;
using FCAI.Application;
using System.Text.Json.Serialization;

namespace FCAI.APIs.Extension
{
    public static class ConfigureAllServices
    {
        public static WebApplicationBuilder AddAllServices(this WebApplicationBuilder builder)
        {
            // Define a named CORS policy:
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowDevAndProd", policy =>
                {
                    policy
                      .WithOrigins(
                         "http://localhost:4200",                    // dev
                         "https://hazemelbehary.github.io"           // prod
                      )
                      .AllowAnyHeader()                              // e.g. Authorization, Content-Type
                      .AllowAnyMethod()                              // GET, POST, etc.
                      .AllowCredentials();                           // allow cookies
                });
            });


            // Catch validation error with models
            builder.Services.AddControllers()
                //.AddNewtonsoftJson(options =>
                //{
                //    options.SerializerSettings.ReferenceLoopHandling
                //    = ReferenceLoopHandling.Ignore;
                //    options.SerializerSettings.ContractResolver
                //        = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
                //    options.SerializerSettings.NullValueHandling
                //        = NullValueHandling.Ignore;
                //})
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = false;
                    options.InvalidModelStateResponseFactory = actionContext =>
                    {
                        var Errors = actionContext.ModelState
                            .Where(P => P.Value!.Errors.Count > 0)
                            .SelectMany(p => p.Value!.Errors.Select(e => e.ErrorMessage))
                            .ToArray();
                        return new BadRequestObjectResult(new ApiExceptionResponse(400, string.Join(";", Errors), string.Empty));
                    };
                });
            
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddAuthAndJWTBearer(builder.Configuration);
            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();


            return builder;
        }
    }
}
