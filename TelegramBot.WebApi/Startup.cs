using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using TelegramBot.BLL;
using TelegramBot.DAL.Data;
using TelegramBot.Service.Handlers;

namespace TelegramBot.Service
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiTest", Version = "v1" });
            });
            services.AddCors();
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            //if(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            //{
            //    services.AddDbContext<ApplicationContext>(options =>
            //        options.UseSqlServer(
            //            Configuration.GetConnectionString("TgBotProd")));
            //}
            //else
            //{
            //    services.AddDbContext<ApplicationContext>(options =>
            //        options.UseSqlServer(
            //            Configuration.GetConnectionString("DefaultConnection")));
            //}
            //services.BuildServiceProvider().GetService<ApplicationContext>().Database.Migrate();
            BLLInjection.Injection(services);
            services.AddScoped<MessageHandler>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            //app.UseSwagger(c =>
            //{
            //    c.PreSerializeFilters.Add((swagger, httpReq) =>
            //    {
            //        swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}/{httpReq.Headers["X-Forwarded-Prefix"]}" } };
            //    });
            //});
            app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "WebApiTest v1"));
           
            //app.UseCors(opt => opt
            //    .WithOrigins(new[] { "http://localhost:3000" })
            //    .AllowAnyHeader()
            //    .AllowAnyMethod()
            //    .AllowCredentials()
            //    );

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}