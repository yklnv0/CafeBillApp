using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeBillApp
{
    public class MenuItem
    {
        public string Description { get; set; }
        public double Price { get; set; }

        public MenuItem(string description, double price)
        {
            Description = description;
            Price = price;
        }
    }
}
