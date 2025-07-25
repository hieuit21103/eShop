namespace Payment.API.Services;

public class PaymentService(ILogger<PaymentService> logger, ApplicationDbContext context) : IPaymentService<PaymentService>
{
    private readonly ILogger<PaymentService> _logger = logger;
    private readonly ApplicationDbContext _context = context;

    public async Task<List<PaymentInfo>> GetAllAsync()
    {
        _logger.LogInformation("Fetching all payment infos.");
        return await _context.PaymentInfos.ToListAsync();
    }

    public async Task<PaymentInfo?> GetByOrderIdAsync(Guid orderId)
    {
        _logger.LogInformation("Fetching payment info for OrderId: {OrderId}", orderId);
        var paymentInfo = await _context.PaymentInfos
            .FirstOrDefaultAsync(p => p.OrderId == orderId);

        if (paymentInfo == null)
        {
            _logger.LogWarning("No payment info found for OrderId: {OrderId}", orderId);
        }

        return paymentInfo;
    }


    public async Task<bool> CreateAsync(PaymentInfo paymentInfo)
    {
        _logger.LogInformation("Setting payment info for OrderId: {OrderId}", paymentInfo.OrderId);
        await _context.PaymentInfos.AddAsync(paymentInfo);
        var result = await _context.SaveChangesAsync();
        if (result > 0)
        {
            _logger.LogInformation("Payment info set successfully for OrderId: {OrderId}", paymentInfo.OrderId);
            return true;
        }
        else
        {
            _logger.LogError("Failed to set payment info for OrderId: {OrderId}", paymentInfo.OrderId);
            return false;
        }
    }

    public async Task<bool> UpdateAsync(PaymentInfo paymentInfo)
    {
        _logger.LogInformation("Updating payment info for OrderId: {OrderId}", paymentInfo.OrderId);
        _context.PaymentInfos.Update(paymentInfo);
        var result = await _context.SaveChangesAsync();
        if (result > 0)
        {
            _logger.LogInformation("Payment info updated successfully for OrderId: {OrderId}", paymentInfo.OrderId);
            return true;
        }
        else
        {
            _logger.LogError("Failed to update payment info for OrderId: {OrderId}", paymentInfo.OrderId);
            return false;
        }
    }

    public async Task<bool> DeleteAsync(Guid orderId)
    {
        _logger.LogInformation("Deleting payment info for OrderId: {OrderId}", orderId);
        var paymentInfo = await GetByOrderIdAsync(orderId);
        if (paymentInfo == null)
        {
            _logger.LogWarning("No payment info found to delete for OrderId: {OrderId}", orderId);
            return false;
        }

        _context.PaymentInfos.Remove(paymentInfo);
        var result = await _context.SaveChangesAsync();
        if (result > 0)
        {
            _logger.LogInformation("Payment info deleted successfully for OrderId: {OrderId}", orderId);
            return true;
        }
        else
        {
            _logger.LogError("Failed to delete payment info for OrderId: {OrderId}", orderId);
            return false;
        }
    }
    
}