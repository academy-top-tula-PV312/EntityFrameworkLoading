using EFModelsReferncesApp;
using Microsoft.EntityFrameworkCore;


//using(EmployeesAppContext context = new())
//{
//    await context.Database.EnsureDeletedAsync();
//    await context.Database.EnsureCreatedAsync();
//}

//await Examples.ContextInitExample();


using (EmployeesAppContext context = new())
{
    var projects = context.Projects
                          .Include(p => p.Employees)
                                .ThenInclude(e => e.Position);

    foreach(var project in projects)
    {
        Console.WriteLine($"Project: {project.Title}");
        foreach(var employee in project.Employees)
            Console.WriteLine($"\t{employee.Name} {employee?.Position?.Title}");
    }
}





