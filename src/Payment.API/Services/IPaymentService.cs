namespace Payment.API.Services;

public interface IPaymentService<T> where T : class
{
    Task<PagedResult<PaymentInfo>> GetAllAsync(int page, int pageSize);
    Task<PaymentInfo?> GetByOrderIdAsync(Guid orderId);
    Task<bool> CreateAsync(PaymentInfo paymentInfo);
    Task<bool> UpdateAsync(PaymentInfo paymentInfo);
    Task<bool> DeleteAsync(Guid orderId);

}