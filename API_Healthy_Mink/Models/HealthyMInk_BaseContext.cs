using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace API_Healthy_Mink.Models
{
    public partial class HealthyMInk_BaseContext : DbContext
    {
        public HealthyMInk_BaseContext()
        {
        }

        public HealthyMInk_BaseContext(DbContextOptions<HealthyMInk_BaseContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Healthy_Mink.db");
        }


        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<Shift> Shifts { get; set; } = null!;
        public static async Task SeedDataAsync(HealthyMInk_BaseContext context)
        {
            if (!context.Roles.Any() && !context.Roles.Any()) 
            {
                DateTime StartTime = Convert.ToDateTime("2024-01-01T09:00:00");
                DateTime EndTime = Convert.ToDateTime("2024-01-01T18:00:00");
                var roles = new List<Role>()
                {
                    new Role { Name = "Менеджер", StartShift = StartTime, EndShift = EndTime },
                    new Role { Name = "Инженер", StartShift = StartTime , EndShift = EndTime },
                    new Role { Name = "Тестировщик свечей", StartShift = StartTime , EndShift = EndTime }
                };
                await context.Roles.AddRangeAsync(roles);
                await context.SaveChangesAsync();
            }
        }
    }
}
