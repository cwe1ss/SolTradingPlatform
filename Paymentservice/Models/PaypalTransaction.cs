namespace Paymentservice.Models
{
    public class PaypalPaymentTransaction
    {
        public string? PayPalUsername { get; set; }
        public string? PayPalPasswort { get; set; }
        public double? Amount { get; set; }
    }
}
