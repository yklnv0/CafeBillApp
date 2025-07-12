using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CafeBillApp
{
    class Program
    {
        static List<MenuItem> bill = new List<MenuItem>();
        static double tipAmount = 0.0;

        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            ShowMenu();

            while (true)
            {
                Console.Write("\nEnter your choice: ");
                string input = Console.ReadLine()?.Trim();

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
                    case "5":
                        ClearAll();
                        break;
                    case "6":
                        SaveToFile();
                        break;
                    case "7":
                        LoadFromFile();
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
            Console.WriteLine("| ║ 5. Clear All          ║ |");
            Console.WriteLine("| ║ 6. Save to file       ║ |");
            Console.WriteLine("| ║ 7. Load from file     ║ |");
            Console.WriteLine("| ║ 0. Exit               ║ |");
            Console.WriteLine("| ╚═══════════════════════╝ |");
            Console.WriteLine("|                           |");
            Console.WriteLine("+---------------------------+");
        }

        public static void AddItem()
        {
            if (bill.Count >= 5)
            {
                Console.WriteLine("Cannot add more than 5 items.");
                return;
            }

            string description;
            while (true)
            {
                Console.Write("Enter product description (3–20 chars): ");
                description = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(description) && description.Length >= 3 && description.Length <= 20)
                    break;
                Console.WriteLine("Description must be between 3 and 20 characters.");
            }

            double price;
            while (true)
            {
                Console.Write("Enter product price (> 0): ");
                if (double.TryParse(Console.ReadLine(), out price) && price > 0)
                    break;
                Console.WriteLine("Price must be a positive number.");
            }

            bill.Add(new MenuItem(description, price));
            Console.WriteLine("Item successfully added.");
        }

        public static void RemoveItem()
        {
            if (bill.Count == 0)
            {
                Console.WriteLine("No items to remove.");
                return;
            }

            while (true)
            {
                Console.WriteLine("\nItemNo\tDescription\t\tPrice");
                Console.WriteLine("--------------------------------------");

                for (int i = 0; i < bill.Count; i++)
                {
                    Console.WriteLine($"{i + 1}\t{bill[i].Description.PadRight(20)} ${bill[i].Price:F2}");
                }

                Console.Write($"Enter the item number to remove (1–{bill.Count}) or 0 to cancel: ");
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
                        var removed = bill[index - 1];
                        bill.RemoveAt(index - 1);
                        Console.WriteLine($"Item \"{removed.Description}\" removed successfully.");
                        return;
                    }
                }

                Console.WriteLine("Invalid input. Please try again.");
            }
        }

        public static void ClearAll()
        {
            if (bill.Count == 0)
            {
                Console.WriteLine("There are no items to clear.");
                return;
            }

            while (true)
            {
                Console.Write("Are you sure you want to clear all items? (y/n): ");
                string response = Console.ReadLine()?.Trim().ToLower();
                if (response == "y" || response == "yes")
                {
                    bill.Clear();
                    tipAmount = 0;
                    Console.WriteLine("All items have been cleared.");
                    return;
                }
                else if (response == "n" || response == "no")
                {
                    Console.WriteLine("Clear operation cancelled.");
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid response. Please enter 'y' or 'n'.");
                }
            }
        }

        public static void AddTip()
        {
            if (bill.Count == 0)
            {
                Console.WriteLine("No items in the bill to add tip for.");
                return;
            }

            double netTotal = bill.Sum(i => i.Price);

            while (true)
            {
                Console.WriteLine($"\nNet Total: ${netTotal:F2}");
                Console.WriteLine("1 - Enter tip percentage");
                Console.WriteLine("2 - Enter fixed tip amount");
                Console.WriteLine("3 - No tip");
                Console.Write("Choose an option (1–3): ");
                string input = Console.ReadLine()?.Trim();

                if (input == "1")
                {
                    while (true)
                    {
                        Console.Write("Enter tip percentage (e.g., 10 for 10%): ");
                        if (double.TryParse(Console.ReadLine(), out double percent) && percent >= 0)
                        {
                            tipAmount = Math.Round(netTotal * percent / 100, 2);
                            Console.WriteLine($"Tip of {percent}% added: ${tipAmount:F2}");
                            return;
                        }
                        Console.WriteLine("Invalid percentage. Please try again.");
                    }
                }
                else if (input == "2")
                {
                    while (true)
                    {
                        Console.Write("Enter fixed tip amount (e.g., 5.00): ");
                        if (double.TryParse(Console.ReadLine(), out double fixedTip) && fixedTip >= 0)
                        {
                            tipAmount = fixedTip;
                            Console.WriteLine($"Fixed tip added: ${tipAmount:F2}");
                            return;
                        }
                        Console.WriteLine("Invalid amount. Please try again.");
                    }
                }
                else if (input == "3")
                {
                    tipAmount = 0;
                    Console.WriteLine("No tip added.");
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.");
                }
            }
        }


        public static void DisplayBill()
        {
            if (bill.Count == 0)
            {
                Console.WriteLine("There are no items in the bill to display.");
                return;
            }

            Console.WriteLine("\n============= Cafe Bill =============");
            Console.WriteLine("{0,-25}{1,10}", "Description", "Price");
            Console.WriteLine("-------------------------------------");

            double netTotal = 0;

            foreach (var item in bill)
            {
                Console.WriteLine("{0,-25}{1,10}", item.Description, $"${item.Price:F2}");
                netTotal += item.Price;
            }

            Console.WriteLine("-------------------------------------");

            double gstAmount = Math.Round(netTotal * 0.05, 2);
            double totalAmount = netTotal + tipAmount + gstAmount;

            Console.WriteLine("{0,-25}{1,10}", "Net Total", $"${netTotal:F2}");
            Console.WriteLine("{0,-25}{1,10}", "Tip Amount", $"${tipAmount:F2}");
            Console.WriteLine("{0,-25}{1,10}", "GST (5%)", $"${gstAmount:F2}");
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("{0,-25}{1,10}", "Total Amount", $"${totalAmount:F2}");
            Console.WriteLine("=====================================\n");
        }

        public static void SaveToFile()
        {
            if (bill.Count == 0)
            {
                Console.WriteLine("There are no items to save.");
                return;
            }

            string filename;
            while (true)
            {
                Console.Write("Enter the file name (1–10 characters): ");
                filename = Console.ReadLine()?.Trim();

                if (!string.IsNullOrEmpty(filename) && filename.Length <= 10)
                    break;

                Console.WriteLine("Invalid file name.");
            }

            string fullPath = filename + ".csv";

            try
            {
                using (StreamWriter writer = new StreamWriter(fullPath))
                {
                    foreach (var item in bill)
                    {
                        writer.WriteLine($"{item.Description},{item.Price:F2}");
                    }
                }

                Console.WriteLine($"✔ File '{fullPath}' was saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error saving file: {ex.Message}");
            }
        }

        public static void LoadFromFile()
        {
            string filename;
            while (true)
            {
                Console.Write("Enter the file name to load items from (without .csv): ");
                filename = Console.ReadLine()?.Trim();

                if (!string.IsNullOrEmpty(filename) && filename.Length <= 10)
                    break;

                Console.WriteLine("Invalid file name.");
            }

            string fullPath = filename + ".csv";

            if (!File.Exists(fullPath))
            {
                Console.WriteLine("❌ File not found: " + fullPath);
                return;
            }

            try
            {
                string[] lines = File.ReadAllLines(fullPath);
                var loadedItems = new List<MenuItem>();

                foreach (string line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length != 2) continue;

                    string description = parts[0].Trim();
                    string priceText = parts[1].Trim();

                    if (description.Length < 3 || description.Length > 20) continue;
                    if (!double.TryParse(priceText, out double price) || price <= 0) continue;

                    loadedItems.Add(new MenuItem(description, price));
                    if (loadedItems.Count == 5) break;
                }

                if (loadedItems.Count == 0)
                {
                    Console.WriteLine("⚠ No valid items found in the file.");
                    return;
                }

                bill = loadedItems;
                tipAmount = 0;
                Console.WriteLine($"✔ Loaded {loadedItems.Count} items from '{fullPath}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error reading the file: {ex.Message}");
            }
        }
    }
}
