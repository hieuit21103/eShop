namespace PaymentServiceProvider;

public interface IPaymentServiceProvider
{
    string GeneratePaymentUrl(Guid orderId, decimal amount, string returnUrl, string clientIp);
    bool ValidateSignature(Dictionary<string, string> data, string inputHash, string secretKey);
}