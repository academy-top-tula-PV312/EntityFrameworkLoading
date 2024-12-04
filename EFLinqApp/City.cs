using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFLinqApp
{
    public class City
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;

        public int? CountryId { get; set; }
        public Country? Country { get; set; } 
    }
}
