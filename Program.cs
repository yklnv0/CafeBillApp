using System;
using System.Collections.Generic;

namespace CafeBillApp
{
    class Program
    {
        static List<MenuItem> bill = new List<MenuItem>();
        static double tipAmount = 0.0;

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
                    case "2":
                        RemoveItem();
                        break;
                    case "3":
                        AddTip();
                        break;
                    case "4":
                        DisplayBill();
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
            Console.WriteLine("+---------------------------+");
            Console.WriteLine("|                           |");
            Console.WriteLine("| ╔═══════════════════════╗ |");
            Console.WriteLine("| ║       Monk's Cafe     ║ |");
            Console.WriteLine("| ╠═══════════════════════╣ |");
            Console.WriteLine("| ║ 1. Add Item           ║ |");
            Console.WriteLine("| ║ 2. Remove Item        ║ |");
            Console.WriteLine("| ║ 3. Add Tip            ║ |");
            Console.WriteLine("| ║ 4. Display Bill       ║ |");
            Console.WriteLine("| ║ 0. Exit               ║ |");
            Console.WriteLine("| ╚═══════════════════════╝ |");
            Console.WriteLine("|                           |");
            Console.WriteLine("+---------------------------+");
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

        public static void RemoveItem()
        {
            if (bill.Count == 0)
            {
                Console.WriteLine("No items to remove.");
                return;
            }

            Console.WriteLine("\nItemNo\tDescription\t\tPrice");
            Console.WriteLine("--------------------------------------");

            for (int i = 0; i < bill.Count; i++)
            {
                Console.WriteLine($"{i + 1}\t{bill[i].Description.PadRight(20)} ${bill[i].Price:F2}");
            }

            Console.Write("Enter the item number to remove or 0 to cancel: ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int index))
            {
                if (index == 0)
                {
                    Console.WriteLine("Operation canceled.");
                    return;
                }

                if (index >= 1 && index <= bill.Count)
                {
                    bill.RemoveAt(index - 1);
                    Console.WriteLine("Remove item was successful.");
                }
                else
                {
                    Console.WriteLine("Invalid item number.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }
        }

        public static void AddTip()
        {
            if (bill.Count == 0)
            {
                Console.WriteLine("No items in the bill to add tip for.");
                return;
            }

            double netTotal = 0;
            foreach (var item in bill)
            {
                netTotal += item.Price;
            }

            Console.WriteLine();
            Console.WriteLine($"Net Total: ${netTotal:F2}");
            Console.WriteLine("1 - Tip Percentage");
            Console.WriteLine("2 - Tip Amount");
            Console.WriteLine("3 - No Tip");
            Console.Write("Enter Tip Method: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.Write("Enter tip percentage: ");
                    if (double.TryParse(Console.ReadLine(), out double percent) && percent >= 0)
                    {
                        tipAmount = Math.Round(netTotal * percent / 100, 2);
                    }
                    else
                    {
                        Console.WriteLine("Invalid percentage.");
                    }
                    break;

                case "2":
                    Console.Write("Enter tip amount: ");
                    if (double.TryParse(Console.ReadLine(), out double fixedTip) && fixedTip >= 0)
                    {
                        tipAmount = fixedTip;
                    }
                    else
                    {
                        Console.WriteLine("Invalid amount.");
                    }
                    break;

                case "3":
                    tipAmount = 0;
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        public static void DisplayBill()
        {
            if (bill.Count == 0)
            {
                Console.WriteLine("There are no items in the bill to display.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("{0,-25}{1,10}", "Description", "Price");
            Console.WriteLine(new string('-', 37));

            double netTotal = 0;
            foreach (var item in bill)
            {
                Console.WriteLine("{0,-25}{1,10}", item.Description, $"${item.Price:F2}");
                netTotal += item.Price;
            }

            Console.WriteLine(new string('-', 37));
            Console.WriteLine(new string('-', 37));

            double gstAmount = Math.Round(netTotal * 0.05, 2);
            double totalAmount = netTotal + tipAmount + gstAmount;

            Console.WriteLine("{0,-25}{1,10}", "Net Total", $"${netTotal:F2}");
            Console.WriteLine("{0,-25}{1,10}", "Tip Amount", $"${tipAmount:F2}");
            Console.WriteLine("{0,-25}{1,10}", "Total GST", $"${gstAmount:F2}");
            Console.WriteLine("{0,-25}{1,10}", "Total Amount", $"${totalAmount:F2}");
        }
    }
}
