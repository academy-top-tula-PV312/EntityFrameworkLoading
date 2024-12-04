using EFLinqApp;
using Microsoft.EntityFrameworkCore;

using(EmployeesAppContext context = new())
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
}

void ShowEmployeesAll(EmployeesAppContext context)
{
    var employees = context.Employees
                           .Include(e => e.Company)
                                .ThenInclude(c => c.Country)
                                    .ThenInclude(cn => cn.Capital)
                           .Include(e => e.Position)
                           .ToList();

    foreach(var e in employees)
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