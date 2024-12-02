using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EFModelsReferncesApp
{
    public class EmployeesAppContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                        .HasOne(e => e.Company)
                        .WithMany(c => c.Employees)
                        .OnDelete(DeleteBehavior.SetNull);
            //.HasForeignKey(e => e.CompanyPrimaryKey);

            modelBuilder.Entity<City>()
                        .HasOne(ct => ct.Country)
                        .WithOne(cn => cn.Capital)
                        .HasForeignKey<Country>(cn => cn.CapitalId)
                        .OnDelete(DeleteBehavior.Cascade);
                        
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                          //.UseLazyLoadingProxies()
                          .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=employees_app_db;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }
    }
}
