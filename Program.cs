using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace RestaurantOrderSystem
{
    partial class Program
    {
        // ============================ CUSTOMER VIEW ===========================
        static void CustomerView()
        {
            bool done = false;

            while (!done)
            {
                ShowFoodMenu();
                Console.WriteLine();
                Console.WriteLine("1. Add item");
                Console.WriteLine("2. Review order / Checkout");
                Console.WriteLine("3. Cancel order and return to main menu");
                int choice = ReadNumberInRange("Choose (1-3): ", 1, 3);

                switch (choice)
                {
                    case 1:
                        AddItemToCart();
                        break;
                    case 2:
                        done = ReviewAndConfirm();
                        break;
                    case 3:
                        ClearCart();
                        Console.WriteLine("Order canceled. Cart cleared.");
                        done = true;
                        break;
                    default:
                        break;
                }
            }
        }

        // CUR 3: order entry. Invalid input never changes the cart.
        static void AddItemToCart()
        {
            int itemNumber = ReadNumberInRange("Item number (1-" + menuNames.Length + "): ", 1, menuNames.Length);
            if (itemNumber == -1)
            {
                return;
            }

            int quantity = ReadNumberInRange("Quantity (1-" + MaxQuantity + "): ", 1, MaxQuantity);
            if (quantity == -1)
            {
                return;
            }

            int index = itemNumber - 1;
            if (cartQuantities[index] + quantity > MaxQuantity)
            {
                Console.WriteLine("Rejected: combined quantity for " + menuNames[index]
                    + " cannot exceed " + MaxQuantity + " (you already have " + cartQuantities[index] + ").");
                return;
            }

            cartQuantities[index] += quantity;
            Console.WriteLine("Added " + quantity + " x " + menuNames[index] + ".");
        }

        // CUR 4: returns true when the customer is finished (paid or canceled)
        static bool ReviewAndConfirm()
        {
            ShowCart();

            if (CalculateTotal() == 0m)
            {
                Console.WriteLine("Your order is empty. Please add items before payment.");
                return false;
            }

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("1. Add more items");
                Console.WriteLine("2. Confirm and pay");
                Console.WriteLine("3. Cancel order");
                int choice = ReadNumberInRange("Choose (1-3): ", 1, 3);

                if (choice == 1)
                {
                    return false;
                }
                if (choice == 2)
                {
                    return ProcessPayment(CalculateTotal());
                }
                if (choice == 3)
                {
                    ClearCart();
                    Console.WriteLine("Order canceled. Cart cleared. No record saved.");
                    return true;
                }
                // choice == -1: invalid, ask again
            }
        }

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
                if (orderId != null)
                {
                    List<string[]> rows = BuildOrderRows(orderId, total, cash, change);
                    if (SaveOrder(rows))
                    {
                        ShowReceipt(rows);
                        ClearCart();
                        return true;
                    }
                }

                Console.WriteLine("ERROR: The order could not be saved. Payment was NOT completed.");
                string retry = ReadInput("Retry saving? (Y = retry, any other key = go back): ").ToUpper();
                if (retry != "Y")
                {
                    return false;
                }
                orderId = GetNextOrderId();
            }
        }

        // ========================= INPUT HELPERS =========================
        static string ReadInput(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            return (input ?? "").Trim();
        }

        // Returns the number, or -1 if the input is blank / not a whole number / out of range.
        // NumberStyles.None rejects decimals, signs, spaces and letters. (CUR 5, SPR 3)
        static int ReadNumberInRange(string prompt, int min, int max)
        {
            string input = ReadInput(prompt);
            if (input == "")
            {
                Console.WriteLine("Input cannot be blank.");
                return -1;
            }
            if (!int.TryParse(input, NumberStyles.None, Inv, out int value))
            {
                Console.WriteLine("Invalid input. Enter a whole number only (no letters or decimals).");
                return -1;
            }
            if (value < min || value > max)
            {
                Console.WriteLine("Out of range. Enter a number from " + min + " to " + max + ".");
                return -1;
            }
            return value;
        }

        // PRR 1: non-negative amount, max two decimal places
        static bool TryParseCash(string input, out decimal cash)
        {
            cash = 0m;
            if (input == "")
            {
                return false;
            }
            if (!decimal.TryParse(input, NumberStyles.AllowDecimalPoint, Inv, out cash))
            {
                return false; // text, negative sign, commas, etc.
            }
            int dotIndex = input.IndexOf('.');
            if (dotIndex >= 0 && input.Length - dotIndex - 1 > 2)
            {
                return false; // more than two decimal places
            }
            return true;
        }

        // ============================ CALCULATIONS ============================
        static decimal CalculateSubtotal(int index)
        {
            return menuPrices[index] * cartQuantities[index];
        }

        static decimal CalculateTotal()
        {
            decimal total = 0m;
            for (int i = 0; i < menuNames.Length; i++)
            {
                total += CalculateSubtotal(i);
            }
            return total;
        }

        static string FormatMoney(decimal amount)
        {
            return "PHP " + amount.ToString("N2", Inv);
        }

        static void ClearCart()
        {
            for (int i = 0; i < cartQuantities.Length; i++)
            {
                cartQuantities[i] = 0;
            }
        }
    }
}