using Microsoft.EntityFrameworkCore;

namespace TelegramBot.Entities
{
    public class ApplicationContext : DbContext
    {
        public DbSet<TelegramNews> TelegramNews { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=ILYA\\MSSQLSERVER01;Database=TelegramBot;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TelegramNews>().Property(tn => tn.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<TelegramNews>().Property(tn => tn.Title).IsRequired();
            modelBuilder.Entity<TelegramNews>().Property(tn => tn.Href).IsRequired();
        }
    }
}
