using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Telegram.Bot;
using TelegramBot.BLL;
using TelegramBot.BLL.Interfaces;
using TelegramBot.BLL.Services;
using TelegramBot.DAL.Data;
using TelegramBot.DAL.GenericRepository;
using TelegramBot.DAL.NewsRepository;
using TelegramBot.DAL.UnitOfWork;
using TelegramBot.DAL.UserRepository;

namespace TelegramBot.WebApi
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
           
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));
            BLLInjection.Injection(services);
           
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TelegramBot.WebApi", Version = "v1" });
            });
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TelegramBot.WebApi v1"));
            }

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


// services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
//services.AddTransient<IUserRepository, UserRepository>();
//services.AddTransient<INewsRepository, NewsRepository>();
//services.AddTransient<IUnitOfWork, UnitOfWork>();
//  services.AddSingleton<ITelegramBotClient, TelegramBotClient>();
// services.AddTransient<IParser, ParserService>();