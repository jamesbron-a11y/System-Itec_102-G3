// PERSON 7 - Person7_ReceiptAndCashierMenu.cs
// Laman: ParseSavedMoney, ShowReceipt, ShowAllOrderIds, CashierView (may Delete sales history option na)
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace RestaurantOrderSystem
{
    partial class Program
    {
        // ============================= RECEIPT =============================
        static decimal ParseSavedMoney(string text)
        {
            decimal.TryParse(text, NumberStyles.AllowDecimalPoint, Inv, out decimal value);
            return value;
        }

        static void ShowReceipt(List<string[]> rows)
        {
            string[] first = rows[0];

            Console.WriteLine();
            Console.WriteLine("========== RECEIPT ==========");
            Console.WriteLine("Restaurant: " + RestaurantName);
            Console.WriteLine("Order ID  : " + first[0]);
            Console.WriteLine("Status    : " + first[1]);
            Console.WriteLine("-----------------------------");
            foreach (string[] row in rows)
            {
                Console.WriteLine(row[2]);
                Console.WriteLine("   " + FormatMoney(ParseSavedMoney(row[3])) + " x " + row[4]
                    + " = " + FormatMoney(ParseSavedMoney(row[5])));
            }
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Total : " + FormatMoney(ParseSavedMoney(first[6])));
            Console.WriteLine("Cash  : " + FormatMoney(ParseSavedMoney(first[7])));
            Console.WriteLine("Change: " + FormatMoney(ParseSavedMoney(first[8])));
            Console.WriteLine("=============================");
        }

        // ============================ CASHIER VIEW ============================
        static void ShowAllOrderIds()
        {
            List<string[]> records = ReadSavedRecords();

            if (records == null)
            {
                Console.WriteLine("ERROR: Could not read saved orders.");
                return;
            }
            if (records.Count == 0)
            {
                Console.WriteLine("No saved orders yet.");
                return;
            }

            HashSet<string> displayedIds = new HashSet<string>();

            Console.WriteLine();
            Console.WriteLine("--- SAVED ORDER IDs ---");
            Console.WriteLine("Order ID".PadRight(15) + "Total");

            foreach (string[] fields in records)
            {
                string id = fields[0].Trim().ToUpperInvariant();

                if (displayedIds.Add(id))
                {
                    Console.WriteLine(id.PadRight(15) +
                        FormatMoney(ParseSavedMoney(fields[6])));
                }
            }

            Console.WriteLine("Total orders: " + displayedIds.Count);
        }

        static void CashierView()
        {
            bool done = false;

            while (!done)
            {
                Console.WriteLine();
                Console.WriteLine("--- CASHIER VIEW ---");
                Console.WriteLine("1. Find order by Order ID");
                Console.WriteLine("2. Display all Order IDs");
                Console.WriteLine("3. View sales report");
                Console.WriteLine("4. Delete sales history");
                Console.WriteLine("5. Return to main menu");

                int choice = ReadNumberInRange("Choose (1-5): ", 1, 5);

                switch (choice)
                {
                    case 1:
                        FindOrder();
                        break;

                    case 2:
                        ShowAllOrderIds();
                        break;

                    case 3:
                        ShowSales();
                        break;

                    case 4:
                        DeleteSalesHistory();
                        break;

                    case 5:
                        done = true;
                        break;
                }
            }
        }
    }
}



