using Microsoft.EntityFrameworkCore;
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

        static public void LinqWhereExample()
        {
            using (EmployeesAppContext context = new())
            {
                ShowEmployeesAll(context);

                Console.WriteLine($"\nExample declarative:");

                int? idRussia = context?.Countries?
                                      .FirstOrDefault(cn => cn.Title.ToLower() == "russia")?
                                      .Id;

                Console.WriteLine($"\nExample declarative:");
                var employeesOpers = (from e in context.Employees
                                                       .Include(e => e.Position)
                                      where e.Company!.Country!.Id == idRussia
                                      select e).ToList();
                foreach (var e in employeesOpers)
                    Console.WriteLine($"{e.Name}\t{e.Company?.Title} ({e.Company?.Country?.Title})");


                Console.WriteLine($"\nExample extends methods:");
                var employeesMethods = context.Employees
                                              .Include(e => e.Position)
                                              .Where(e => e.Company!.Country!.Id == idRussia)
                                              .ToList();
                foreach (var e in employeesMethods)
                    Console.WriteLine($"{e.Name}\t{e.Company?.Title} ({e.Company?.Country?.Title})");


                var employeesMm = context.Employees
                             .Where(e => EF.Functions
                                           .Like(e.Name, "%mm%"));

                foreach (var e in employeesMm)
                    Console.WriteLine($"Name: {e.Name}");
            }

            void ShowEmployeesAll(EmployeesAppContext context)
            {
                var employees = context.Employees
                                       .Include(e => e.Company)
                                            .ThenInclude(c => c.Country)
                                                .ThenInclude(cn => cn.Capital)
                                       .Include(e => e.Position)
                                       .ToList();

                foreach (var e in employees)
                    Console.WriteLine($"{e.Name} ({e.Age}) {e?.Position?.Title}:\t{e?.Company?.Title} ({e?.Company?.Country?.Title} - {e?.Company?.Country?.Capital?.Title})");

            }

            void ShowEmployeesWhereCompanyId(EmployeesAppContext context, int id)
            {
                Console.WriteLine($"\nExample declarative:");
                var employeesOpers = (from e in context.Employees
                                                       .Include(e => e.Position)
                                      where e.Company!.Id == id
                                      select e).ToList();
                foreach (var e in employeesOpers)
                    Console.WriteLine($"{e.Name} {e.Company?.Title}");


                Console.WriteLine($"\nExample extends methods:");
                var employeesMethods = context.Employees
                                              .Include(e => e.Position)
                                              .Where(e => e.Company!.Id == id);
                foreach (var e in employeesMethods)
                    Console.WriteLine($"{e.Name} {e.Company?.Title}");
            }
        }

        static public void FindsExample()
        {
            using (EmployeesAppContext context = new())
            {
                Employee? employeeFiveId = context.Employees
                                                  .Find(15);
                Console.WriteLine($"{employeeFiveId?.Id} {employeeFiveId?.Name}");

                Employee employeeThreeId = context.Employees
                                                  .First(e => e.Id == 3);
                Console.WriteLine($"{employeeThreeId?.Id} {employeeThreeId?.Name}");

                Employee? employeeMore35 = context.Employees
                                                 .FirstOrDefault(e => e.Age >= 25);
                Console.WriteLine($"{employeeMore35?.Id} {employeeMore35?.Name}");

                // Last(), LastOrDefault()

            }
        }

        static public void SelectOrderByExample()
        {
            using (EmployeesAppContext context = new())
            {
                /*
                SELECT e.Name, c.Title AS Company, p.Title AS Position
                    FROM Employees AS e
                        Companies AS c
                        Positions AS p
                        WHERE e.Company.Id = c.ID AND e.PositionId = p.Id
                */
                var employeesOpers = from e in context.Employees
                                     orderby e.Company!.Title, e.Name
                                     select new
                                     {
                                         e.Name,
                                         Company = e.Company!.Title,
                                         Position = e.Position!.Title
                                     };
                foreach (var e in employeesOpers)
                    Console.WriteLine($"{e.Name} {e.Position} {e.Company}");
                Console.WriteLine();

                var employeesMethods = context.Employees
                                              .OrderBy(e => e.Company!.Title)
                                              .ThenBy(e => e.Name)
                                              .Select(e => new EmployeeModel
                                              {
                                                  Name = e.Name,
                                                  Company = e.Company!.Title,
                                                  Position = e.Position!.Title
                                              });
                foreach (var e in employeesMethods)
                    Console.WriteLine($"{e.Name} {e.Position} {e.Company}");
                Console.WriteLine();
            }
        }

        static public void JoinExample()
        {
            using (EmployeesAppContext context = new())
            {
                var employeesOpers = from e in context.Employees
                                     join c in context.Companies
                                        on e.Company equals c
                                     join p in context.Positions
                                        on e.Position equals p
                                     select new
                                     {
                                         e.Name,
                                         Company = c.Title,
                                         Position = p.Title
                                     };
                foreach (var e in employeesOpers)
                    Console.WriteLine($"{e.Name} {e.Position} {e.Company}");
                Console.WriteLine();

                var employeesMethods = context.Employees.Join(
                                                            context.Companies,
                                                            e => e.Company.Id,
                                                            c => c.Id,
                                                            (e, c) => new
                                                            {
                                                                e.Name,
                                                                Company = e.Company!.Title,
                                                                Position = e.Position
                                                            })
                                                         .Join(
                                                            context.Positions,
                                                            ec => ec.Position.Id,
                                                            p => p.Id,
                                                            (ec, p) => new
                                                            {
                                                                ec.Name,
                                                                ec.Company,
                                                                Position = p.Title
                                                            });
                foreach (var e in employeesMethods)
                    Console.WriteLine($"{e.Name} {e.Position} {e.Company}");
                Console.WriteLine();

            }
        }

        static public void GroupByExample()
        {
            using (EmployeesAppContext context = new())
            {
                var employeesCompanyCountOpers = from e in context.Employees
                                                 group e by e.Company.Title into c
                                                 select new
                                                 {
                                                     Company = c.Key,
                                                     Count = c.Count()
                                                 };
                foreach (var cc in employeesCompanyCountOpers)
                    Console.WriteLine($"{cc.Company}\t: {cc.Count}");


                var employeesCompanyCountMethods = context.Employees
                                                          .GroupBy(e => e.Company.Title)
                                                          .Select(c => new
                                                          {
                                                              Company = c.Key,
                                                              Count = c.Count()
                                                          });
                foreach (var cc in employeesCompanyCountMethods)
                    Console.WriteLine($"{cc.Company}\t: {cc.Count}");
            }
        }

        static public void SetOpersExample()
        {
            using (EmployeesAppContext context = new())
            {
                var ids = ShowSets(context);

                // Union
                var union = context.Employees
                                    .Include(e => e.Position)
                                    .Include(e => e.Company)
                                        .ThenInclude(c => c.Country)
                                    .Where(e => e.Company!.Country!.Id == ids.Item1)
                                    .Union(
                                            context.Employees
                                               .Include(e => e.Position)
                                               .Include(e => e.Company)
                                                    .ThenInclude(c => c.Country)
                                               .Where(e => e.Position!.Id == ids.Item2));
                Console.WriteLine("\nUnion:");
                foreach (var u in union)
                    Console.WriteLine(FullInfo(u));


                // Intersect
                var intersect = context.Employees
                                    .Include(e => e.Position)
                                    .Include(e => e.Company)
                                        .ThenInclude(c => c.Country)
                                    .Where(e => e.Company!.Country!.Id == ids.Item1)
                                    .Intersect(
                                            context.Employees
                                               .Include(e => e.Position)
                                               .Include(e => e.Company)
                                                    .ThenInclude(c => c.Country)
                                               .Where(e => e.Position!.Id == ids.Item2));
                Console.WriteLine("\nIntersect:");
                foreach (var i in intersect)
                    Console.WriteLine(FullInfo(i));

                // Except
                var except = context.Employees
                                    .Include(e => e.Position)
                                    .Include(e => e.Company)
                                        .ThenInclude(c => c.Country)
                                    .Where(e => e.Company!.Country!.Id == ids.Item1)
                                    .Except(
                                            context.Employees
                                               .Include(e => e.Position)
                                               .Include(e => e.Company)
                                                    .ThenInclude(c => c.Country)
                                               .Where(e => e.Position!.Id == ids.Item2));
                Console.WriteLine("\nExcept:");
                foreach (var e in except)
                    Console.WriteLine(FullInfo(e));


                except = context.Employees
                                .Include(e => e.Position)
                                .Include(e => e.Company)
                                    .ThenInclude(c => c.Country)
                                .Where(e => e.Position!.Id == ids.Item2)
                                .Except(context.Employees
                                                .Include(e => e.Position)
                                                .Include(e => e.Company)
                                                    .ThenInclude(c => c.Country)
                                                .Where(e => e.Company!.Country!.Id == ids.Item1));
                Console.WriteLine("\nExcept:");
                foreach (var e in except)
                    Console.WriteLine(FullInfo(e));

            }

            string FullInfo(Employee e)
            {
                return $"Name: {e.Name} ({e.Age})\n\t{e?.Company?.Title} ({e?.Company?.Country?.Title})\n\t{e?.Position?.Title}";
            }

            (int, int) ShowSets(EmployeesAppContext context)
            {
                int idRussia = context.Countries
                                      .FirstOrDefault(cn => cn.Title.ToLower() == "russia")!
                                      .Id;

                var employeesRussians = context.Employees
                                               .Include(e => e.Position)
                                               .Include(e => e.Company)
                                                    .ThenInclude(c => c.Country)
                                               .Where(e => e.Company!.Country!.Id == idRussia);
                foreach (var er in employeesRussians)
                    Console.WriteLine(FullInfo(er));
                Console.WriteLine();

                int idDevs = context.Positions
                                       .FirstOrDefault(p => p.Title.ToLower() == "developer")!
                                       .Id;

                var employeesDevs = context.Employees
                                               .Include(e => e.Position)
                                               .Include(e => e.Company)
                                                    .ThenInclude(c => c.Country)
                                               .Where(e => e.Position!.Id == idDevs);
                foreach (var er in employeesDevs)
                    Console.WriteLine(FullInfo(er));
                Console.WriteLine();

                return (idRussia, idDevs);
            }
        }

        static public void AgregateFuncExample()
        {
            using (EmployeesAppContext context = new())
            {
                // Sum, Min, Max, Count, Average - numbers
                // All, Any - bool

                bool resultAll = context.Employees.All(e => e.Age > 25);
                //bool resultAll = context.Employees.All(e => e.Name.EndsWith("y"));
                //bool resultAll = context.Employees.All(e => e.Name.Contains("mm"));
                Console.WriteLine($"All result: {resultAll}");

                bool resultAny = context.Employees.Any(e => e.Age > 25);
                //bool resultAny = context.Employees.Any(e => e.Name.EndsWith("y"));
                //bool resultAny = context.Employees.Any(e => e.Name.Contains("mm"));
                Console.WriteLine($"Any result: {resultAny}");

                int resultCount = context.Employees.Count(e => e.Name.Contains("mm"));
                Console.WriteLine($"Count result: {resultCount}");

                int? resultMin = context.Employees.Min(e => e.Age);
                //int? resultMin = context.Employees
                //                        .Where(e => e.Company!.Country!.Id == 2)
                //                        .Min(e => e.Age);
                Console.WriteLine($"Min result: {resultMin}");

                int? resultMax = context.Employees.Max(e => e.Age);
                //int? resultMax = context.Employees
                //                        .Where(e => e.Company!.Country!.Id == 1)
                //                        .Max(e => e.Age);
                Console.WriteLine($"Max result: {resultMax}");

                double? resultAvg = context.Employees.Average(e => e.Age);
                Console.WriteLine($"Avg result: {resultAvg}");

                int? resultSum = context.Employees.Sum(e => e.Age);
                Console.WriteLine($"Sum result: {resultSum}");
            }
        }

        static public void ChangeTrackingExample()
        {
            using (EmployeesAppContext context = new())
            {
                //context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var employees = context.Employees
                                       //.AsNoTracking()
                                       .Where(e => e.Company!.Id == 1);
                foreach (var e in employees)
                    Console.WriteLine($"{e.Id} {e.Name} {e.Age}");

                var empl = context.Employees.FirstOrDefault(e => e.Id == 3);
                empl!.Age = 20;

                foreach (var e in employees)
                    Console.WriteLine($"{e.Id} {e.Name} {e.Age}");
            }
        }

        static public void QueryFiltersExample()
        {
            using (EmployeesAppContext context = new())
            {
                var employees = context.Employees.IgnoreQueryFilters();
                foreach (var e in employees)
                    Console.WriteLine($"{e.Name}");
                Console.WriteLine();

                var countries = context.Countries.IgnoreQueryFilters();
                foreach (var cn in countries)
                    Console.WriteLine($"{cn.Title}");
            }

        }

    }

    public class EmployeeModel
    {
        public string Name { get; set; }
        public string Company { get; set; }
        public string Position { get; set; }
    }
}
