using System;
using System.Collections.Generic;

namespace ConsoleApp.Data.EfCore
{
    public partial class Customers2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
        public string Salary { get; set; }
    }
}
