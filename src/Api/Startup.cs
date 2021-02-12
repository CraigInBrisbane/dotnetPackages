using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Linq;
using Application.Interfaces;
using Application.User;
using Application.User.Handlers;
using Infrastructure.Database;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // Get SQL Server sorted
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DatabaseContext>(
                opt => opt.UseSqlServer(connectionString)
             );

            // Inject what we'll be using
            services.AddControllers();
            services.AddSwaggerGen();
            services.AddMediatR(typeof(RegisterUserQuery).Assembly);
            // We're separating mappings into separate entity files. So we will scan all the classes to find mapping profiles, and inject them now. Instead of one at a time.
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies().Where(assembly => assembly.FullName.StartsWith("Application")));

            // Inject our classes
            services.AddScoped<IUserService, UserService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();


            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "API endpoint");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}