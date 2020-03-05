using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace VoyagerPOSTerminal
{
    public class PointOfSaleTerminal
    {
        readonly IDictionary<string, ItemTotalCalculator> calculators = new Dictionary<string, ItemTotalCalculator>();
        readonly IList<string> items = new List<string>();
        double discount;

        public void SetPricing(string code, decimal price, params VolumePrice[] volumePrices)
        {
            calculators[code] = new ItemTotalCalculator(price, volumePrices);
        }

        public void Scan(params string[] codes)
        {
            foreach (var code in codes)
            {
                if (calculators.ContainsKey(code) == false)
                    throw new InvalidOperationException($"Product '{code}' not found");
            }

            Array.ForEach(codes, items.Add);
        }



        //Calculates total price with and without Bulk Discount
        public Total CalculateTotal()
        {
            var totals = items.Distinct().Select(code =>
            {
                var calculator = calculators[code];
                var volume = items.Count(c => c == code);

                var applyDiscount = calculator
                    .CalculateTotal(volume);
                return ApplyBulkDiscountIfNeeded(applyDiscount, discount);
            });
            return totals.Aggregate(new Total(0, 0), (acc, total) => new Total(acc.NetPrice + total.NetPrice, acc.FinalPrice + total.FinalPrice));
        }

        //applies bulk discount to selected product codes, (A,C) and gives a final price
        static Total ApplyBulkDiscountIfNeeded(Total total, double discount)
        {
            var adjustedFinalPrice = total.FinalPrice != total.NetPrice
                                ? total.FinalPrice
                                : total.NetPrice - total.NetPrice * (decimal)discount;

            return new Total(total.NetPrice, Math.Round(adjustedFinalPrice, 2));
        }

        //sets a total discount
        public void SetTotalDiscount(double discount)
        {
            this.discount = discount;
        }


        // Item Calculator 
        class ItemTotalCalculator
        {
            readonly decimal price;
            readonly VolumePrice[] volumePrices;

            public ItemTotalCalculator(decimal price, params VolumePrice[] volumePrices)
            {
                this.price = price;
                this.volumePrices = volumePrices;
            }

            public Total CalculateTotal(int volume)
            {
                var productPrice = volumePrices
                    .Union(new[] { new VolumePrice(price, 1) })
                    .OrderBy(price => price.PriceForUnit); // sorts prices 

                var sum = new AccumulatorRecord { Total = 0m, Volume = volume };

                var unitPrice = productPrice.Aggregate(
                    sum,
                    (acc, wp) =>
                    {
                        var volumeAmount = acc.Volume / wp.Amount;

                        var total = acc.Total + wp.PricePerUnit * volumeAmount;
                        var newVolume = acc.Volume - wp.Amount * volumeAmount;

                        return new AccumulatorRecord { Total = total, Volume = newVolume };
                    },
                    x => x.Total
                );

                return new Total(volume * price, unitPrice); ;
            }

            class AccumulatorRecord // Kind of a tuple like record type
            {
                public decimal Total;
                public int Volume;
            }
        }

    }
}