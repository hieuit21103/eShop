namespace Payment.API.Services;

public interface IPaymentService<T> where T : class
{
    Task<List<PaymentInfo>> GetAllAsync();
    Task<PaymentInfo?> GetByOrderIdAsync(Guid orderId);
    Task<bool> CreateAsync(PaymentInfo paymentInfo);
    Task<bool> UpdateAsync(PaymentInfo paymentInfo);
    Task<bool> DeleteAsync(Guid orderId);

}