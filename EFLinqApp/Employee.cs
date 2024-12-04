using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFLinqApp
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? Age { get; set; }


        //public int CompanyPrimaryKey { get; set; } // внешний ключ
        //[ForeignKey("CompanyPrimaryKey")]
        //[Required]
        //public int? CompanyId { get; set; }
        public virtual Company? Company { get; set; } // навигационное свойство
        public virtual Position? Position { get; set; } // навигационное свойство
        public List<Project> Projects { get; set; } = new();

        public List<EmployeeProject> EmployeeProjects { get; set; } = new();
    }
}
