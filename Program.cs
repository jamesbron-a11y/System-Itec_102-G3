
﻿// PERSON 4 - Person4_CustomerView.cs
// Laman: CustomerView, AddItemToCart, ReviewAndConfirm
using System;
=======
﻿using System;
main
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
                Console.WriteLine("Your order is. Please add items before payment.");
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
=======
        // ========================= INPUT HELPERS =========================
        static string ReadInput(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            return (input ?? &quot; &quot;).Trim();
        }
        // Returns the number, or -1 if the input is blank / not a whole number / out of range.
        // NumberStyles.None rejects decimals, signs, spaces and letters. (CUR 5, SPR 3)
        static int ReadNumberInRange(string prompt, int min, int max)
        {
            string input = ReadInput(prompt);
            if (input == &quot; &quot;)
{
                Console.WriteLine(&quot; Input cannot be blank.&quot;);
                return -1;
            }
            if (!int.TryParse(input, NumberStyles.None, Inv, out int value))
            {
                Console.WriteLine(&quot; Invalid input. Enter a whole number only(no letters or decimals).& quot;);
                return -1;
            }
            if (value & lt; min || value & gt; max)
{
                Console.WriteLine(&quot; Out of range.Enter a number from & quot; +min + &quot; to & quot; +max + &quot;.&quot;);
                return -1;
            }
            return value;
        }
        // PRR 1: nonnegative amount, max two decimal places
        static bool TryParseCash(string input, out decimal cash)
        {
            cash = 0m;
            if (input == &quot; &quot;)
{
                return false;
            }
            if (!decimal.TryParse(input, NumberStyles.AllowDecimalPoint, Inv, out cash))
            {
                return false; // text, negative sign, commas, etc.
            }
            int dotIndex = input.IndexOf(&#39;.&#39;);

if (dotIndex & gt;= 0 & amp; &amp; input.Length - dotIndex - 1 & gt; 2)
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
            for (int i = 0; i & lt; menuNames.Length; i++)
            {
                total += CalculateSubtotal(i);
            }
            return total;
        }
        static string FormatMoney(decimal amount)
        {
            return &quot; PHP & quot; +amount.ToString(&quot; N2 & quot;, Inv);
        }
        static void ClearCart()
        {
            for (int i = 0; i & lt; cartQuantities.Length; i++)
            {
                cartQuantities[i] = 0;
main
            }
        }
    }
}
