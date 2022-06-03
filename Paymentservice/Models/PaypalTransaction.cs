namespace PaymentService.Models
{
    public class PaypalPaymentTransaction
    {
        public string? PayPalUsername { get; set; }
        public string? PayPalPassword { get; set; }
        public double? Amount { get; set; }
    }
}
