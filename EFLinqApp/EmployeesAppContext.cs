using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EFLinqApp
{
    public class EmployeesAppContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Project> Projects { get; set; }

        public EmployeesAppContext()
        {
            //ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // One to Many
            modelBuilder.Entity<Employee>()
                        .HasOne(e => e.Company)
                        .WithMany(c => c.Employees)
                        .OnDelete(DeleteBehavior.SetNull);
            //.HasForeignKey(e => e.CompanyPrimaryKey);

            // One to Many
            modelBuilder.Entity<Company>()
                        .HasMany(c => c.Employees)
                        .WithOne(e => e.Company);


            // One to One
            modelBuilder.Entity<City>()
                        .HasOne(ct => ct.Country)
                        .WithOne(cn => cn.Capital)
                        .HasForeignKey<Country>(cn => cn.CapitalId)
                        .OnDelete(DeleteBehavior.Cascade);

            // Mant to Many
            modelBuilder.Entity<Employee>()
                        .HasMany(e => e.Projects)
                        .WithMany(p => p.Employees)
                        .UsingEntity<EmployeeProject>(
                        l => l.HasOne(e => e.Project)
                              .WithMany(ep => ep.EmployeesProject)
                              .HasForeignKey(e => e.ProjectId),
                        r => r.HasOne(p => p.Employee)
                              .WithMany(ep => ep.EmployeeProjects)
                              .HasForeignKey(p => p.EmployeeId),
                        ep =>
                        {
                            ep.Property(t => t.StartingDate)
                              .HasDefaultValueSql("GETDATE()");
                            ep.HasKey(t => new { t.EmployeeId, t.ProjectId });
                            ep.ToTable("EmployeesProjects");
                        });


            //modelBuilder.Entity<Project>()
            //            .HasMany(p => p.Employees)
            //            .WithMany(e => e.Projects);


            modelBuilder.Entity<Country>()
                        .HasQueryFilter(cn => cn.Id == 1);
            modelBuilder.Entity<Employee>()
                        .HasQueryFilter(e => e.Company!.Country!.Id == 1);
                        
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                          //.UseLazyLoadingProxies()
                          .UseSqlServer("Data Source=3-0;Initial Catalog=ado_db;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }
    }
}
