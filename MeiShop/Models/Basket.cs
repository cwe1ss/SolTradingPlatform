namespace MeiShop.Models
{
    public class Basket
    {
        public string Product { get; set; } = string.Empty;
        public string Vendor { get; set; } = string.Empty;
        public string CustomerCreditCardnumber { get; set; } = string.Empty;
        public double AmountInEuro { get; set; }
    }
}
