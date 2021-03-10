using System;

namespace SystemGlobalServicesTask.Models
{
    public class CurrencyRateWithId : CurrencyRate
    {
        public string ID { get; set; }

        public CurrencyRateWithId(string charCode, decimal value, string id) : base(charCode, value)
        {
            ID = id;
        }
    }
}