using EFLinqApp;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.WebSockets;


using (EmployeesAppContext context = new())
{
    //context.Employees
    //       .Where(e => e.Company!.Country!.Id == 1)
    //       .ExecuteUpdate(s => s.SetProperty(e => e.Age, e => e.Age - 1));

    //context.Employees.ExecuteDelete();


    //string sqlQuery = "INSERT INTO Companies (Title, CountryId) VALUES ('Rambler', 1)";
    //context.Database.ExecuteSqlRaw(sqlQuery);

    //int companyId = 1;
    //SqlParameter companyIdParam = new("@companyId", companyId);

    //sqlQuery = $"SELECT * FROM Employees WHERE CompanyId = @companyId";
    //var managers = context.Employees
    //                      .FromSqlRaw(sqlQuery, companyIdParam)
    //                      .OrderBy(x => x.Name);

    //foreach(var m in managers)
    //    Console.WriteLine($"{m.Name}");

    //int age = 30;
    //SqlParameter ageParam = new("@age", age);

    ////var employeesByAge = context.Employees
    ////                            .FromSqlInterpolated($"SELECT * FROM EmployeesByAge({age})");
    //var employeesByAge = context.Employees
    //                            .FromSqlRaw($"SELECT * FROM EmployeesByAge(@age)", ageParam);

    //var employeesByAge = context.EmployeesByAge(age);

    //foreach (var m in employeesByAge)
    //    Console.WriteLine($"{m.Name}");

    SqlParameter companyParam = new SqlParameter("@company", "Yandex");

    var employeesByCompany = context.Employees
                                    .FromSqlRaw("EmployeesByCompany @company", 
                                                companyParam);

    foreach (var m in employeesByCompany)
        Console.WriteLine($"{m.Name}");


}
