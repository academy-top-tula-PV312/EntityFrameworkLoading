using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFLinqApp
{
    public class Company
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public virtual Country? Country { get; set; }
        public virtual List<Employee> Employees { get; set; } = new();

    }
}
