using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeBillApp
{
    class Program
    {
        static List<MenuItem> bill = new List<MenuItem>();

        static void Main()
        {
            while (true)
            {
                ShowMenu();
                Console.Write("Enter your choice: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AddItem();
                        break;
                    case "0":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("\nMonk's Cafe");
            Console.WriteLine("1. Add Item");
            Console.WriteLine("0. Exit");
        }

        public static void AddItem()
        {
            if (bill.Count >= 5)
            {
                Console.WriteLine("You can't add more than 5 items.");
                return;
            }

            Console.Write("Enter description: ");
            string description = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(description) || description.Length < 3 || description.Length > 20)
            {
                Console.WriteLine("Description must be between 3 and 20 characters.");
                return;
            }

            Console.Write("Enter price: ");
            if (!double.TryParse(Console.ReadLine(), out double price) || price <= 0)
            {
                Console.WriteLine("Price must be a positive number.");
                return;
            }

            bill.Add(new MenuItem(description, price));
            Console.WriteLine("Add item was successful.");
        }
    }
}

