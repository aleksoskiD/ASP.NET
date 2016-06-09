using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET.Homework1.Models
{
    public class Equipment
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string AssignedTo { get; set; }
    }
}