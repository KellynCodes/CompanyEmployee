using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Configuration;

namespace Repository.Context
{
    public class CompEmpDbContext : DbContext
    {
        public CompEmpDbContext(DbContextOptions options): base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            //modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }

        public DbSet<Company> Companies { get; set; }
         public DbSet<Employee> Employees { get; set; }
}
}