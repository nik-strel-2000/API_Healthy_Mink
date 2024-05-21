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
            if (!context.Roles.Any()) 
            {
                DateTime StartTime = Convert.ToDateTime("2024-01-01T09:00:00");
                DateTime EndTime = Convert.ToDateTime("2024-01-01T18:00:00");
                var roles = new List<Role>()
                {
                    new Role { Name = "Менеджер", StartShift = StartTime, EndShift = EndTime },
                    new Role { Name = "Инженер", StartShift = StartTime , EndShift = EndTime },
                    new Role { Name = "Тестировщик свечей", StartShift = StartTime , EndShift = new DateTime(2024, 01, 01 ,21,00,00) }
                };
                await context.Roles.AddRangeAsync(roles);
                await context.SaveChangesAsync();
            }
            if (!context.Employees.Any())
            {
                var employees = new List<Employee>()
                {
                    new Employee { FirstName = "Тамара", LastName = "Иванова", MiddleName = "Ивановна", RoleId = 1 ,NumberRemark =1},
                    new Employee { FirstName = "Иван", LastName = "Иванов", MiddleName = "Иванович", RoleId = 2 },
                    new Employee { FirstName = "Юлий", LastName = "Иванов", MiddleName = "Юрьевич", RoleId = 3 , NumberRemark = 2 }
                };
                await context.Employees.AddRangeAsync(employees);
                await context.SaveChangesAsync();
            }
            if (!context.Shifts.Any())
            {
                DateTime StartTime = new DateTime(2024, 05, 01, 09, 00, 00);
                DateTime EndTime = new DateTime(2024, 05, 01, 18, 00, 00);
                var shifts = new List<Shift>()
                {
                    new Shift { EmployeeId = 1, StartShift = StartTime, EndShift = EndTime, NumberHours = 9 },
                    new Shift { EmployeeId = 1, StartShift = new DateTime(2024, 05, 01, 09, 05, 00) , EndShift = new DateTime(2024, 01, 05, 18, 05, 0), NumberHours = 9 },
                    new Shift { EmployeeId = 1, StartShift = StartTime, EndShift = EndTime, NumberHours = 9 },
                    new Shift { EmployeeId = 2, StartShift = StartTime, EndShift = EndTime, NumberHours = 9 },
                    new Shift { EmployeeId = 2, StartShift = StartTime, EndShift = EndTime, NumberHours = 9 },
                    new Shift { EmployeeId = 2, StartShift = StartTime, EndShift = new DateTime(2024, 05, 01, 18, 01, 0), NumberHours = 9 },
                    new Shift { EmployeeId = 3, StartShift = StartTime, EndShift = new DateTime(2024, 05, 01, 20, 50, 0), NumberHours = 11.833333333333333 },
                    new Shift { EmployeeId = 3, StartShift = StartTime, EndShift = new DateTime(2024, 05, 02, 21, 00, 0), NumberHours = 12 },
                    new Shift { EmployeeId = 3, StartShift = StartTime, EndShift = new DateTime(2024, 05, 03, 20, 50, 0), NumberHours = 11.833333333333333 },
                };
                await context.Shifts.AddRangeAsync(shifts);
                await context.SaveChangesAsync();
            }
        }
    }
}
