using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace VoyagerPOSTerminal
{
    public class Tests
    {
        PointOfSaleTerminal pos;

        [SetUp]
        public void SetUp()
        {
            pos = new PointOfSaleTerminal();

            pos.SetPricing("A", 1.25m, new VolumePrice(3m, 3));
            pos.SetPricing("B", 4.25m);
            pos.SetPricing("C", 1.00m, new VolumePrice(5m, 6));
            pos.SetPricing("D", 0.75m);
        }

        // tests the product prices to the product price segment

        [Test]

        [TestCase("", ExpectedResult = 0.00)]
        [TestCase("B", ExpectedResult = 4.25)]
        [TestCase("BB", ExpectedResult = 8.50)]
        [TestCase("ABCDABA", ExpectedResult = 13.25)]
        [TestCase("CCCCCCC", ExpectedResult = 6.00)]
        [TestCase("ABCD", ExpectedResult = 7.25)]
        
        // returns the the price if all is good then the test should pass
        public decimal Scan_product_code_return_price(string codes)
        {
            pos.Scan(codes.Select(c => c.ToString()).ToArray());
            return pos.CalculateTotal().FinalPrice;
        }

        
    }
}

