// PERSON 6 - Person6_FileStorage.cs
// Laman: BuildOrderRows, SaveOrder, ReadSavedRecords, GetNextOrderId
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace RestaurantOrderSystem
{
    partial class Program
    {
        // ========================= FILE STORAGE (SPR 1, 2) =========================
        // Format per line: ID|STATUS|ItemName|UnitPrice|Qty|Subtotal|Total|Cash|Change
        static List<string[]> BuildOrderRows(string orderId, decimal total, decimal cash, decimal change)
        {
            List<string[]> rows = new List<string[]>();
            for (int i = 0; i < menuNames.Length; i++)
            {
                if (cartQuantities[i] > 0)
                {
                    rows.Add(new string[]
                    {
                        orderId, "PAID", menuNames[i],
                        menuPrices[i].ToString("F2", Inv),

                        cartQuantities[i].ToString(Inv),
                        CalculateSubtotal(i).ToString("F2", Inv),

                        total.ToString("F2", Inv),
                        cash.ToString("F2", Inv),
                        change.ToString("F2", Inv)
                    });
                }
            }
            return rows;
        }

        static bool SaveOrder(List<string[]> rows)
        {
            try
            {
                Directory.CreateDirectory(OrdersFolder);
                List<string> lines = new List<string>();
                foreach (string[] row in rows)
                {
                    lines.Add(string.Join("|", row));
                }
                File.AppendAllLines(OrdersPath, lines); // closes the file handle automatically
                return true;
            }
            catch (IOException)
            {
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        // Returns saved records, an empty list if the file does not exist yet,
        // or null if the file cannot be read.
        static List<string[]> ReadSavedRecords()
        {
            List<string[]> records = new List<string[]>();

            if (!File.Exists(OrdersPath))
            {
                return records;
            }

            try
            {
                string[] lines = File.ReadAllLines(OrdersPath);
                foreach (string line in lines)
                {
                    if (line.Trim() == "")
                    {
                        continue;
                    }
                    string[] fields = line.Split('|');
                    if (fields.Length == 9 && fields[1] == "PAID")
                    {
                        records.Add(fields);
                    }
                }
                return records;
            }
            catch (IOException)
            {
                return null;
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
        }

        // Next ID is derived from the saved records so it stays unique after restart.
        static string GetNextOrderId()
        {
            List<string[]> records = ReadSavedRecords();
            if (records == null)
            {
                return null;
            }

            int highest = 0;
            foreach (string[] fields in records)
            {
                string id = fields[0];
                if (id.StartsWith("ORD") && int.TryParse(id.Substring(3), NumberStyles.None, Inv, out int number))
                {
                    if (number > highest)
                    {
                        highest = number;
                    }
                }
            }
            return "ORD" + (highest + 1).ToString("D4");
        }
    }
}
