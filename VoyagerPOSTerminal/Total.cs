using System;
using System.Collections.Generic;
using System.Text;

namespace VoyagerPOSTerminal
{
    public class Total
    {
        // gets the final price of the products including the bulk options and prices
        public decimal NetPrice { get; }
        public decimal FinalPrice { get; }

        public Total(decimal netPrice, decimal finalPrice)
        {
            NetPrice = netPrice;
            FinalPrice = finalPrice;
        }
    }
}