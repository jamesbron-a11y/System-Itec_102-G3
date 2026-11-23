// PERSON 8 - Person8_FindAndSales.cs
// Laman: FindOrder, ShowSales, DeleteSalesHistory
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace RestaurantOrderSystem
{
    partial class Program
    {
        // CAR 1: trim, case-insensitive search
        static void FindOrder()
        {
            string id = ReadInput("Enter Order ID (e.g., ORD0001): ").ToUpper();

            if (id == "")
            {
                Console.WriteLine("Order ID cannot be blank. Please type an ID such as ORD0001.");
                return;
            }

            List<string[]> records = ReadSavedRecords();
            if (records == null)
            {
                Console.WriteLine("ERROR: Could not read the saved orders file.");
                return;
            }

            List<string[]> matches = new List<string[]>();
            foreach (string[] fields in records)
            {
                if (fields[0].ToUpper() == id)
                {
                    matches.Add(fields);
                }
            }

            if (matches.Count == 0)
            {
                Console.WriteLine("Order not found.");
                return;
            }

            ShowReceipt(matches);
        }

        // CAR 2: sales = sum of order totals (counted once per order), read-only
        static void ShowSales()
        {
            List<string[]> records = ReadSavedRecords();
            if (records == null)
            {
                Console.WriteLine("ERROR: Could not read the saved orders file.");
                return;
            }

            HashSet<string> countedIds = new HashSet<string>();
            decimal totalSales = 0m;

            foreach (string[] fields in records)
            {
                if (countedIds.Add(fields[0].ToUpper()))
                {
                    totalSales += ParseSavedMoney(fields[6]);
                }
            }

            Console.WriteLine();
            Console.WriteLine("   SALES REPORT   ");
            Console.WriteLine("Completed orders: " + countedIds.Count);
            Console.WriteLine("Total sales     : " + FormatMoney(totalSales));
            if (countedIds.Count == 0)
            {
                Console.WriteLine("No sales yet");
            }

            // CAR 3: delete ALL saved sales/order history (asks for confirmation first)
            static void DeleteSalesHistory()
            {
                Console.WriteLine();
                Console.WriteLine("--- DELETE SALES HISTORY ---")                 // ERROR 1: missing semicolon

      List<string[]> records = ReadSavedRecords();
                if (records == null)
                {
                    Console.WriteLine("ERROR: Could not read the saved orders file.");
                    return;
                }

                HashSet<string> orderIds = new HashSet<string>();
                foreach (string[] fields in record)                              // ERROR 2: variable name typo (should be records)
                {
                    orderIds.Add(fields[1].ToUpper());                           // ERROR 3: wrong index (should be 0)
                }

                if (orderIds.Length == 0)                                        // ERROR 4: HashSet has Count, not Length
                {
                    Console.WriteLine("No sales history to delete.");
                    return;
                }

            }
    }
}
