// PERSON 5 - Person5_Payment.cs
// Laman: ProcessPayment
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace RestaurantOrderSystem
{
    partial class Program
    {
        // ============================== PAYMENT ==============================
        // Returns true if finished (paid or canceled), false if the customer
        // should go back to the order menu (cart is kept).
        static bool ProcessPayment(decimal total)
        {
            decimal cash;

            // PRR 1: loop until valid cash that covers the total, or cancel
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Total due: " + FormatMoney(total));
                string input = ReadInput("Enter cash amount in PHP (or C to cancel): ");

                if (input.ToUpper() == "C")
                {
                    ClearCart();
                    Console.WriteLine("Payment canceled. Cart cleared. No record saved.");
                    return true;
                }
                if (!TryParseCash(input, out cash))
                {
                    Console.WriteLine("Invalid amount. Use a non-negative number with at most 2 decimal places.");
                    continue;
                }
                if (cash < total)
                {
                    Console.WriteLine("Not enough cash. Amount still needed: " + FormatMoney(total - cash));
                    Console.WriteLine("Please enter the full amount again or cancel.");
                    continue; // rejected attempts are not accumulated
                }
                break;
            }

            decimal change = cash - total;
            string orderId = GetNextOrderId();

            // SPR 2: if saving fails, do not show a completed sale; allow retry
            while (true)
            {
                if (orderId ! = null)
                {
                    List<string[]> rows = BuildOrderRows(orderId, total, cash, change);
                    if (SaveOrder(rows))
                    {
                        ShowReceipt(rows);
                        ClearCart();
                        return true;
                    }
                }