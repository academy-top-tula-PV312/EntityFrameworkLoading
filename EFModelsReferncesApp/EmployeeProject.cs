using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFModelsReferncesApp
{
    public class EmployeeProject
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public DateTime StartingDate { get; set; }

    }
}
