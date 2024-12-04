using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFLinqApp
{
    static public class Examples
    {
        static public async Task ContextInitExample()
        {
            using (EmployeesAppContext context = new())
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();

                City moscow = new() { Title = "Moscow" };
                City wash = new() { Title = "Washington" };
                Country russia = new() { Title = "Russia", Capital = moscow };
                Country usa = new() { Title = "Usa", Capital = wash };

                Company yandex = new() { Title = "Yandex", Country = russia };
                Company mailRu = new() { Title = "Mail Group", Country = russia };
                Company ozon = new() { Title = "Ozon", Country = russia };
                Company google = new() { Title = "Google", Country = usa };
                Company microsoft = new() { Title = "Microsoft", Country = usa };

                Position manager = new() { Title = "Manager" };
                Position developer = new() { Title = "Developer" };
                Position tester = new() { Title = "Tester" };

                List<Employee> employees = new()
                {
                   new() { Name = "Bobby", Age = 29, Company = yandex, Position = manager },
                   new() { Name = "Jimmy", Age = 32, Company = mailRu, Position = manager },
                   new() { Name = "Tommy", Age = 23, Company = yandex, Position = developer },
                   new() { Name = "Sammy", Age = 34, Company = google, Position = developer },
                   new() { Name = "Lenny", Age = 37, Company = microsoft, Position = developer },
                   new() { Name = "Poppy", Age = 21, Company = ozon, Position = developer },
                   new() { Name = "Dotty", Age = 26, Company = microsoft, Position = tester },
                   new() { Name = "Rikky", Age = 33, Company = google, Position = tester },
                   new() { Name = "Denny", Age = 28, Company = ozon, Position = tester },
                };

                Project site = new() { Title = "Create Site for client" };
                site.Employees.Add(employees[0]);
                site.Employees.Add(employees[2]);

                Project mobile = new()
                {
                    Title = "Modify mobile app",
                    Employees = { employees[4], employees[6] }
                };

                Project desktop = new()
                {
                    Title = "Implement desktop editor",
                    Employees = { employees[3], employees[7] }
                };


                context.Cities.AddRange(moscow, wash);
                context.Countries.AddRange(russia, usa);
                context.Positions.AddRange(manager, developer, tester);
                context.Companies.AddRange(yandex, mailRu, ozon, google, microsoft);
                context.Employees.AddRange(employees);
                context.Projects.AddRange(site, mobile, desktop);

                //context.Companies.AddRange(yandex, mailRu);
                //context.Employees.AddRange(bob, jim, tom);

                //context.SaveChanges();


                //var yandexFind = context.Companies.FirstOrDefault(
                //        c => c.Title.ToLower() == "yandex"
                //        );

                //Employee sam = new Employee()
                //{
                //    Name = "Sammy",
                //    Age = 35,
                //    //CompanyId = yandexFind.Id!
                //    Company = yandexFind,
                //};

                //context.Employees.Add(sam);
                //context.SaveChanges();


                //Employee kim = new() { Name = "Kimmy", Age = 37 };
                //Employee len = new() { Name = "Lenny", Age = 22 };

                //Company ozon = new() { Title = "Ozon", Employees = { kim, len } };

                //context.Companies.Add(ozon);
                await context.SaveChangesAsync();
            }
        }

        static public void ContextLoadingExample()
        {
            /*
Eager loading - жадная загрузка
Explicit loading - явная загрузка
Lazy loading - ленивая загрузка
*/


            using (EmployeesAppContext context = new())
            {

                // Eager loading - жадная загрузка
                //var employees = context.Employees
                //                       .Include(e => e.Company)
                //                            .ThenInclude(c => c!.Country)
                //                                .ThenInclude(cn => cn!.Capital)
                //                       .Include(e => e.Position);

                //foreach (var e in employees)
                //    Console.WriteLine($"{e.Name} ({e?.Position?.Title}): {e?.Company?.Title} ({e?.Company?.Country?.Title})");


                // Explicit loading - явная загрузка
                //var company = context.Companies.FirstOrDefault(c => c.Title.ToLower() == "yandex");

                //if(company is not null )
                //    context.Employees.Where(e => e.CompanyId == company.Id).Load();

                ////context.Employees.Load();
                //if(company is not null )
                //    context.Entry(company).Collection(c => c.Employees).Load();

                //Country? country = context.Countries
                //                          .FirstOrDefault(ct => ct.Title.ToLower() == "russia");

                //if(country is not null)
                //    context.Entry(country).Reference(cn => cn.Capital).Load();


                // Lazy loading
                var employees = context.Employees.ToList();
                foreach (var e in employees)
                    Console.WriteLine($"{e.Name} ({e?.Position?.Title}): {e?.Company?.Title} ({e?.Company?.Country?.Title})");

            }
        }
    }
}
