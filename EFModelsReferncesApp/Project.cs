using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFModelsReferncesApp
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public List<Employee> Employees { get; set; } = new();
        public List<EmployeeProject> EmployeesProject { get; set; } = new();
    }
}
