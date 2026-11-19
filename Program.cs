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

