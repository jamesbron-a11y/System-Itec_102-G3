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
       