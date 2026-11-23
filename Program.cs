using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace RestaurantOrderSystem;

class Program
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

    static readonly decimal[] menuPrices =
    {
        35.00m, 45.00m, 65.00m, 30.00m, 48.00m, 45.00m,
        30.00m, 20.00m, 60.00m, 80.00m, 60.00m, 55.00m, 75.00m, 90.00m
    };

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

    // ======================== MENUS / DISPLAY ========================
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

    static void ShowFoodMenu()
    {
        Console.WriteLine("\n");
        Console.WriteLine("\t==============================================");
        Console.WriteLine("\t                INJELS BURGER");
        Console.WriteLine("\t                   FOOD MENU");
        Console.WriteLine("\t==============================================");

        Console.WriteLine("\n\t" +
            "NO.".PadRight(8) +
            "FOOD ITEM".PadRight(32) +
            "PRICE");

        Console.WriteLine("\t----------------------------------------------");

        for (int i = 0; i < menuNames.Length; i++)
        {
            Console.WriteLine("\t" +
                (i + 1).ToString().PadRight(8) +
                menuNames[i].PadRight(32) +
                FormatMoney(menuPrices[i]));
        }

        Console.WriteLine("\t==============================================");
    }

    static void ShowCart()
    {
        Console.WriteLine();
        Console.WriteLine("--- ORDER REVIEW ---");

        if (CalculateTotal() == 0m)
        {
            Console.WriteLine("(no items yet)");
            return;
        }

        Console.WriteLine("Item".PadRight(30) + "Unit Price".PadLeft(14) + "Qty".PadLeft(5) + "Subtotal".PadLeft(14));
        for (int i = 0; i < menuNames.Length; i++)
        {
            if (cartQuantities[i] > 0)
            {
                Console.WriteLine(menuNames[i].PadRight(30)
                    + FormatMoney(menuPrices[i]).PadLeft(14)
                    + cartQuantities[i].ToString().PadLeft(5)
                    + FormatMoney(CalculateSubtotal(i)).PadLeft(14));
            }
        }
        Console.WriteLine("ORDER TOTAL: " + FormatMoney(CalculateTotal()));
    }

    // ========================= INPUT HELPERS =========================
    static string ReadInput(string prompt)
    {
        Console.Write(prompt);
        string? input = Console.ReadLine();
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

    // ============================ CUSTOMER VIEW ============================
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
        string? orderId = GetNextOrderId();

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
    static List<string[]>? ReadSavedRecords()
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
    static string? GetNextOrderId()
    {
        List<string[]>? records = ReadSavedRecords();
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
        List<string[]>? records = ReadSavedRecords();

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

                default:
                    break; // invalid input already reported
            }
        }
    }

    // CAR 1: trim, case-insensitive search
    static void FindOrder()
    {
        string id = ReadInput("Enter Order ID (e.g., ORD0001): ").ToUpper();

        if (id == "")
        {
            Console.WriteLine("Order ID cannot be blank. Please type an ID such as ORD0001.");
            return;
        }

        List<string[]>? records = ReadSavedRecords();
        if (records == null)
        {
            Console.WriteLine("ERROR: Could not read the saved orders file.");
            return;
        }

        List<string[]> matches = new List<string[]>();
        foreach (string[] fields in records)
        {
            if (fields[0].ToUpper() == id)
            {
                matches.Add(fields);
            }
        }

        if (matches.Count == 0)
        {
            Console.WriteLine("Order not found.");
            return;
        }

        ShowReceipt(matches);
    }

    // CAR 2: sales = sum of order totals (counted once per order), read-only
    static void ShowSales()
    {
        List<string[]>? records = ReadSavedRecords();
        if (records == null)
        {
            Console.WriteLine("ERROR: Could not read the saved orders file.");
            return;
        }

        HashSet<string> countedIds = new HashSet<string>();
        decimal totalSales = 0m;

        foreach (string[] fields in records)
        {
            if (countedIds.Add(fields[0].ToUpper()))
            {
                totalSales += ParseSavedMoney(fields[6]);
            }
        }

        Console.WriteLine();
        Console.WriteLine("--- SALES REPORT ---");
        Console.WriteLine("Completed orders: " + countedIds.Count);
        Console.WriteLine("Total sales     : " + FormatMoney(totalSales));
        if (countedIds.Count == 0)
        {
            Console.WriteLine("No sales yet");
        }
    }

    // CAR 3: delete ALL saved sales/order history (asks for confirmation first)
    static void DeleteSalesHistory()
    {
        Console.WriteLine();
        Console.WriteLine("--- DELETE SALES HISTORY ---");

        List<string[]>? records = ReadSavedRecords();
        if (records == null)
        {
            Console.WriteLine("ERROR: Could not read the saved orders file.");
            return;
        }

        HashSet<string> orderIds = new HashSet<string>();
        foreach (string[] fields in records)
        {
            orderIds.Add(fields[0].ToUpper());
        }

        if (orderIds.Count == 0)
        {
            Console.WriteLine("No sales history to delete.");
            return;
        }

        Console.WriteLine("WARNING: This will permanently delete " + orderIds.Count + " order(s).");
        Console.WriteLine("Order IDs will start again at ORD0001.");
        string confirm = ReadInput("Type YES to confirm (anything else cancels): ");

        if (confirm.ToUpper() != "YES")
        {
            Console.WriteLine("Deletion canceled. Sales history was kept.");
            return;
        }

        try
        {
            File.Delete(OrdersPath);
            Console.WriteLine("Sales history deleted.");
        }
        catch (IOException)
        {
            Console.WriteLine("ERROR: Could not delete the sales history (file may be open in another program).");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("ERROR: No permission to delete the sales history file.");
        }
    }
}
