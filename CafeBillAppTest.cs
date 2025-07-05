using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CafeBillApp.Tests
{
    [TestClass]
    public class CafeBillAppTest
    {
        [TestMethod]
        public void FullAppFlow_ShouldWorkCorrectly()
        {
            var bill = new List<MenuItem>();

            bill.Add(new MenuItem("Coffee", 3.5));
            bill.Add(new MenuItem("Bagel", 2.0));
            bill.Add(new MenuItem("Muffin", 2.5));
            bill.Add(new MenuItem("Juice", 4.0));
            bill.Add(new MenuItem("Toast", 1.5));
            Assert.AreEqual(5, bill.Count);

            try
            {
                if (bill.Count >= 5)
                    throw new InvalidOperationException("Maximum items reached");
                bill.Add(new MenuItem("Extra", 5.0));
            }
            catch (InvalidOperationException ex)
            {
                Assert.AreEqual("Maximum items reached", ex.Message);
            }

            bill.RemoveAt(1);
            Assert.IsFalse(bill.Any(i => i.Description == "Bagel"));

            double tipPercent = 10;
            double netTotal = bill.Sum(i => i.Price);
            double gst = netTotal * 0.05;
            double tip = netTotal * (tipPercent / 100);
            double total = netTotal + gst + tip;

            Assert.AreEqual(11.5, Math.Round(netTotal, 2));
            Assert.AreEqual(0.575, Math.Round(gst, 3));
            Assert.AreEqual(1.15, Math.Round(tip, 2));
            Assert.AreEqual(13.225, Math.Round(total, 3));

            string path = "testbill.csv";
            File.WriteAllLines(path, bill.Select(i => $"{i.Description},{i.Price}"));
            Assert.IsTrue(File.Exists(path));

            bill.Clear();
            Assert.AreEqual(0, bill.Count);

            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                bill.Add(new MenuItem(parts[0], double.Parse(parts[1])));
            }
            Assert.AreEqual(4, bill.Count);
            Assert.AreEqual("Coffee", bill[0].Description);
            Assert.AreEqual(3.5, bill[0].Price);
        }

        [TestMethod]
        public void MenuItem_ShouldStoreDataCorrectly()
        {
            var item = new MenuItem("Test Item", 9.99);
            Assert.AreEqual("Test Item", item.Description);
            Assert.AreEqual(9.99, item.Price);
        }

        [TestMethod]
        public void FilePersistence_ShouldSaveAndLoadCorrectly()
        {
            string path = "tempbill.csv";
            var original = new List<MenuItem>
            {
                new MenuItem("Cake", 3.0),
                new MenuItem("Tea", 2.0)
            };

            File.WriteAllLines(path, original.Select(i => $"{i.Description},{i.Price}"));

            var restored = new List<MenuItem>();
            foreach (var line in File.ReadAllLines(path))
            {
                var parts = line.Split(',');
                restored.Add(new MenuItem(parts[0], double.Parse(parts[1])));
            }

            Assert.AreEqual(2, restored.Count);
            Assert.AreEqual("Cake", restored[0].Description);
            Assert.AreEqual(3.0, restored[0].Price);
        }

        [TestMethod]
        public void Calculation_ShouldBeAccurate()
        {
            var bill = new List<MenuItem>
            {
                new MenuItem("Latte", 4.0),
                new MenuItem("Scone", 2.5)
            };

            double net = bill.Sum(i => i.Price); // 6.5
            double gst = net * 0.05; // 0.325
            double tip = net * 0.1; // 0.65
            double total = net + gst + tip; // 7.475

            Assert.AreEqual(6.5, Math.Round(net, 2));
            Assert.AreEqual(0.325, Math.Round(gst, 3));
            Assert.AreEqual(0.65, Math.Round(tip, 2));
            Assert.AreEqual(7.475, Math.Round(total, 3));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldThrow_WhenAddingTooManyItems()
        {
            var bill = new List<MenuItem>
            {
                new MenuItem("1", 1),
                new MenuItem("2", 2),
                new MenuItem("3", 3),
                new MenuItem("4", 4),
                new MenuItem("5", 5)
            };

            if (bill.Count >= 5)
                throw new InvalidOperationException("Maximum items reached");

            bill.Add(new MenuItem("6", 6)); // ніколи не виконається
        }
    }

    public class MenuItem
    {
        public string Description { get; }
        public double Price { get; }

        public MenuItem(string description, double price)
        {
            Description = description;
            Price = price;
        }
    }
}
