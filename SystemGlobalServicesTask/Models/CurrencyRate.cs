namespace SystemGlobalServicesTask.Models
{
    public class CurrencyRate
    {
        public string CharCode { get; set; }
        public decimal Value { get; set; }

        public CurrencyRate(string charCode, decimal value)
        {
            CharCode = charCode;
            Value = value;
        }
    }
}