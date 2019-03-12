using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace ThePhoneBook.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureMvc();
            services.ConfigureDbContext(Configuration);
            services.ConfigureIdentity(Configuration);
            services.ConfigureCors();
            services.ConfigureOpenApi();
            services.ConfigureApiOptions();
            services.ConfigureVersioning();
            services.ConfigureHealthChecks(Configuration);
            services.ConfigureAuth(Configuration);
            services.ConfigureAutoMapper();
            services.ConfigureTypes();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHealthChecksUI();
            app.UseHealthChecks("/hc", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/ThePhoneBookOpenApiSpecification/swagger.json", "The PhoneBook API");
                    options.RoutePrefix = "";
                    options.DefaultModelExpandDepth(2);
                    options.DefaultModelRendering(ModelRendering.Model);
                    options.DocExpansion(DocExpansion.None);
                });
            }
            else
            {
                app.UseExceptionHandler(appBuilder => appBuilder.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync("An unexpected fault occurred. Please try again later");
                }));
            }

            app.UseCors("Default");

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}