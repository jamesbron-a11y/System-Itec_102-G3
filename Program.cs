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

