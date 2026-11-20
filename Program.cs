
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
            }
        }
    }
}