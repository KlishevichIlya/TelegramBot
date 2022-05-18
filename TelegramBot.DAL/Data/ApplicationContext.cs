using Microsoft.EntityFrameworkCore;
using TelegramBot.DAL.Entities;

namespace TelegramBot.DAL.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<News> News { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Editor> Editors { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<News>().Property(tn => tn.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<News>().Property(tn => tn.Title).IsRequired();
            modelBuilder.Entity<News>().Property(tn => tn.Href).IsRequired();
            modelBuilder.Entity<News>().Property(tn => tn.Image).IsRequired();
            modelBuilder.Entity<News>().Property(tn => tn.Date).IsRequired();

            modelBuilder.Entity<User>().Property(tn => tn.UserId).IsRequired();
            modelBuilder.Entity<User>().Property(tn => tn.UserName).IsRequired();
            modelBuilder.Entity<User>().Property(tn => tn.DateOfStartSubscription).IsRequired();

            modelBuilder.Entity<Editor>().Property(p => p.Email).IsRequired();
            modelBuilder.Entity<Editor>().Property(p => p.Password).IsRequired();
            modelBuilder.Entity<Editor>().Property(p => p.isAdminRule).IsRequired();
            base.OnModelCreating(modelBuilder);
        }
    }
}
