// PERSON 1 - Person1_MainAndData.cs
// Laman: constants, menu data, cart, Main, ShowMainMenu
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace RestaurantOrderSystem
{
    partial class Program
    {
        const string RestaurantName = "Injels Burger";
        const int MaxQuantity = 99;
        static readonly CultureInfo Inv = CultureInfo.InvariantCulture;
        static readonly string OrdersFolder = Path.Combine(AppContext.BaseDirectory, "records");

        static readonly string OrdersPath = Path.Combine(OrdersFolder, "orders.txt");

        // CUR 2: menu data in arrays with matching indexes
        static readonly string[] menuNames =
        {
            "Regular Burger", "Cheese Burger", "Double Burger",
            "Burger with Egg", "Hotdog Sandwich", "Footlong", "French Fries", "Softdrinks",
            "Buy 1 Take 1 Regular Burger", "Buy 1 Take 1 Cheese Burger", "Buy 1 Take 1 Regular Burger",
            "Chicken Burger", "Bacon Cheeseburger", "Buy 1 Take 1 Footlong"

        };
        static readonly decimal[] menuPrices = {  35.00m, 45.00m, 65.00m, 30.00m, 48.00m, 45.00m,
                            30.00m, 20.00m, 60.00m, 80.00m, 60.00m, 55.00m,75.00m, 90.00m };

        // CUR 3: cart quantities, same index as the menu
        static int[] cartQuantities = new int[menuNames.Length];

        // ============================ MAIN ============================
        static void Main(string[] args)
        {
            bool running = true;

            // CUR 1: main navigation loop
            while (running)
            {
                ShowMainMenu();
                int choice = ReadNumberInRange("Choose (1-3): ", 1, 3);

                switch (choice)
                {
                    case 1:
                        CustomerView();
                        break;
                    case 2:
                        CashierView();
                        break;
                    case 3:
                        running = false;
                        break;
                    default:
                        break; // invalid input already reported
                }
            }

            Console.WriteLine("Thank you! Program ended.");
        }

        static void ShowMainMenu()
        {
            Console.WriteLine("\n");
            Console.WriteLine("\t========================================");
            Console.WriteLine("\t           INJELS BURGER");
            Console.WriteLine("\t         RESTAURANT SYSTEM");
            Console.WriteLine("\t========================================");

            Console.WriteLine("\n\t[ MAIN MENU ]\n");

            Console.WriteLine("\t1. Customer View");
            Console.WriteLine("\t2. Cashier View");
            Console.WriteLine("\t3. Exit");

            Console.WriteLine("\n\t========================================");
        }
    }
}

        }
    }
}