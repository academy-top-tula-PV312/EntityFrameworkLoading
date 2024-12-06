using EFLinqApp;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.WebSockets;


using (EmployeesAppContext context = new())
{
    //context.Employees
    //       .Where(e => e.Company!.Country!.Id == 1)
    //       .ExecuteUpdate(s => s.SetProperty(e => e.Age, e => e.Age - 1));

    //context.Employees.ExecuteDelete();

    string sqlQuery = "SELECT * FROM Employees WHERE PositionId = 1";
    var managers = context.Employees.FromSqlRaw(sqlQuery);

    foreach(var m in managers)
        Console.WriteLine($"{m.Name}");
}
