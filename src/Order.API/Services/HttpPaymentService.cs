namespace Order.API.Services;

public class HttpPaymentService(ILogger<HttpPaymentService> logger, HttpClient httpClient, IHttpContextAccessor httpContextAccessor) : IPaymentService
{
    private readonly ILogger<HttpPaymentService> _logger = logger;
    private readonly HttpClient _httpClient = httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<string> CreatePaymentAsync(CustomerOrder customerOrder)
    {
        if (customerOrder == null)
        {
            _logger.LogError("CustomerOrder is null");
            throw new ArgumentNullException(nameof(customerOrder));
        }

        var orderId = customerOrder.Id;
        var userId = customerOrder.UserId;
        var paymentId = customerOrder.PaymentId;
        var amount = customerOrder.TotalPrice;
        var paymentMethod = customerOrder.PaymentMethod;

        _logger.LogInformation("Creating payment for OrderId: {OrderId}, Amount: {Amount}, PaymentMethod: {PaymentMethod}", orderId, amount, paymentMethod);

        var createPaymentRequestDto = new CreatePaymentRequestDto
        {
            OrderId = orderId,
            UserId = userId,
            PaymentId = paymentId,
            Amount = amount,
            PaymentMethod = paymentMethod
        };

        var token = _httpContextAccessor.HttpContext?.Request.Cookies["JWT"];
        if (string.IsNullOrEmpty(token))
        {
            _logger.LogError("JWT token not found in cookies.");
            throw new UnauthorizedAccessException("JWT token missing.");
        }

        var request = new HttpRequestMessage(HttpMethod.Post, "api/payment")
        {
            Content = JsonContent.Create(createPaymentRequestDto)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Payment created successfully for OrderId: {OrderId}", orderId);
        }
        else
        {
            _logger.LogError("Failed to create payment for OrderId: {OrderId}. Status Code: {StatusCode}", orderId, response.StatusCode);
        }

        return await response.Content.ReadAsStringAsync();
    }
}