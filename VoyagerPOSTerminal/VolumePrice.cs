using System;
using System.Collections.Generic;
using System.Text;

namespace VoyagerPOSTerminal
{
    public class VolumePrice
    {
        public decimal PricePerUnit { get; }
        public int Amount { get; }

        public decimal PriceForUnit => PricePerUnit / Amount;

        public VolumePrice(decimal priceForVolume, int Amount)
        {
            PricePerUnit = priceForVolume;
            this.Amount = Amount;
        }
    }
}