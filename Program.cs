// PERSON 4 - Person4_CustomerView.cs
// Laman: CustomerView, AddItemToCart, ReviewAndConfirm
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
