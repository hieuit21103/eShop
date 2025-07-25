namespace Order.API.Services;

public interface IPaymentService
{
    Task<string> CreatePaymentAsync(CustomerOrder customerOrder);
}